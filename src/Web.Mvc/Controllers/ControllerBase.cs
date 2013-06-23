using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Threading;
using System.Web.Mvc;
using System.Web.Routing;
using Arashi.Core;
using Arashi.Services.Localization;
using Arashi.Services.Membership;
using Arashi.Services.SiteStructure;
using Arashi.Core.Domain;
using Arashi.Web.Mvc.Filters;
using Arashi.Web.Mvc.Models;
using Arashi.Web.Mvc.Partials;
using log4net;

namespace Arashi.Web.Mvc.Controllers
{
   /// <summary>
   /// Base Controller for all admin controllers
   /// </summary>
   [SiteFilter(Order = 1)]
   [AdminLocalizationFilter(Order = 2)] // MUST be after the SiteFilter
   [ExceptionFilter(View = "Error")]
   public abstract class ControllerBase : Controller
   {
      private static readonly ILog log = LogManager.GetLogger(typeof(ControllerBase));
      protected ISiteService siteService;
      protected IUserService userService;
      protected readonly ILocalizationService localizationService;
      private IRequestContext requestContext;
      private ScriptModel scriptModel;
      private IList<MessageModel> registeredMessages;
      private IList<MessageModel> persistentMessages;
      private readonly CultureInfo currentCulture;

      #region Constructors

      /// <summary>
      /// Constructor
      /// </summary>
      protected ControllerBase() : this(IoC.Resolve<ILocalizationService>(), IoC.Resolve<IUserService>(), IoC.Resolve<ISiteService>())
      {
      }



      /// <summary>
      /// Constructor
      /// </summary>
      /// <param name="localizationService"></param>
      /// <param name="userService"></param>
      /// <param name="siteService"></param>
      protected ControllerBase(ILocalizationService localizationService, IUserService userService, ISiteService siteService)
      {
         this.userService = userService;
         this.siteService = siteService;
         this.localizationService = localizationService;
         currentCulture = Thread.CurrentThread.CurrentUICulture;
         scriptModel = new ScriptModel();
         registeredMessages = new List<MessageModel>();
         persistentMessages = new List<MessageModel>();
      }

      #endregion

      #region Initialize

      /// <summary>
      /// Override the default Initalize method in order to use the <see cref="CookieTempDataProvider"/>
      /// instead of the default <see cref="SessionStateTempDataProvider"/>.
      /// </summary>
      /// <param name="requestContext"></param>
      protected override void Initialize(RequestContext requestContext)
      {
         TempDataProvider = new CookieTempDataProvider(requestContext.HttpContext);
         base.Initialize(requestContext);
      }

      #endregion
      
      #region Public Properties

      /// <summary>
      /// Get or sets the context.
      /// </summary>
      public IRequestContext Context
      {
         get
         {
            return requestContext;
         }
         set
         {
            requestContext = value;
         }
      }


      public ScriptModel ScriptModel
      {
         get
         {
            return scriptModel;
         }
         set
         {
            scriptModel = value;
         }
      }

      #endregion

      #region Controller Method Overrides

      protected override void OnActionExecuting(ActionExecutingContext filterContext)
      {
         ViewData["ScriptModel"] = ScriptModel;
         ViewData["Context"] = requestContext; // TODO: verificare se serve ancora...

         base.OnActionExecuting(filterContext);
      }

      #endregion

      #region Localization

      /// <summary>
      /// Get a localized global resource given its token
      /// </summary>
      /// <param name="token"></param>
      /// <returns></returns>
      protected string GlobalResource(string token)
      {
         return localizationService.GlobalResource(token, currentCulture);
      }


      // Deprecated
      ///// <summary>
      ///// Get a localized text string for a given key.
      ///// </summary>
      ///// <param name="key"></param>
      ///// <param name="uiCulture"></param>
      ///// <param name="resourceManager"></param>
      ///// <remarks>Spaces are replaced with underscore</remarks>
      ///// <returns></returns>
      //protected string GetText(string key, CultureInfo uiCulture, ResourceManager resourceManager)
      //{
      //   return resourceManager.GetString(key.Replace(' ', '_'), uiCulture);
      //}



      /// <summary>
      /// Returns a IDictionary of the localized values of a given Enum.
      /// key,value == Enum.EnumName, LocalName
      /// </summary>
      /// <param name="e"></param>
      /// <param name="resourceManager"></param>
      /// <returns></returns>
      protected IDictionary<string, string> GetLocalizedEnumList(Type e /*, ResourceManager resourceManager*/)
      {
         //CultureInfo uiCulture = Thread.CurrentThread.CurrentUICulture;

         IDictionary<string, string> items = new Dictionary<string, string>();
         string[] names = Enum.GetNames(e);

         // copia i valori e nomi enumerati in un dictionary
         foreach (int i in Enum.GetValues(e))
         {
            string key = e.Name + "." + names[i];

            // coppia key,value == Enum.EnumName, LocalName
            items.Add(names[i], GlobalResource(key));
         }

         return items;
      }

      #endregion

      //#region FileManager Support

      //protected void RegisterFileManager()
      //{
      //   // Also register partial request for the site chooser component.
      //   ViewData["FileManagerControl"] = new PartialRequest(new
      //   {
      //      area = "ControlPanel",
      //      controller = "FileManager",
      //      action = "Index",
      //      path = "" // questo è il default. TODO: si può configurare qua il path dinamico
      //   });

      //   ScriptModel.AddScript(Models.ScriptModel.ScriptName.Finder);
      //   ScriptModel.AddScript(Models.ScriptModel.ScriptName.ScrollTo);
      //   ScriptModel.AddScriptBlock("finder.init", GetFileResourceContent("~/Resources/js/src/admin.finder-init.js"));
      //}



      //protected string GetFileResourceContent(string virtualFilePath)
      //{
      //   string physicalFilePath = Server.MapPath(virtualFilePath);
      //   FileStream fs;

      //   try
      //   {
      //      fs = new FileStream(physicalFilePath, FileMode.Open, FileAccess.Read);
      //   }
      //   catch (FileNotFoundException ex)
      //   {
      //      log.Error("File not found.", ex);
      //      throw;
      //   }
      //   catch (Exception ex)
      //   {
      //      log.Error("An unexpected error occured while reading a file.", ex);
      //      throw;
      //   }

      //   using (StreamReader sr = new StreamReader(fs))
      //   {
      //      return sr.ReadToEnd();
      //   }
      //}


      //#endregion

      #region Registered Messages

      /// <summary>
      /// Register a message to be rendered
      /// </summary>
      /// <param name="model"></param>
      protected void RegisterMessage(MessageModel model)
      {
         RegisterMessage(model, false);
      }



      /// <summary>
      /// Register a message to be rendered
      /// </summary>
      /// <param name="model"></param>
      /// <param name="isPersistant">Set to true if the message should be persisted between to actions</param>
      protected void RegisterMessage(MessageModel model, bool isPersistant)
      {
         if (isPersistant)
         {
            persistentMessages.Add((model));
            TempData["PersistentMessages"] = persistentMessages;
         }
         else
         {
            registeredMessages.Add(model);
            ViewData["RegisteredMessages"] = registeredMessages;
         }
      }



      #endregion

   }
}
