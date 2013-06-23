namespace Arashi.Web.Mvc.Controllers
{
   using System;
   using System.Collections.Generic;
   using System.Globalization;
   using System.Threading;
   using System.Web.Mvc;
   using System.Web.Routing;

   using Arashi.Core.Domain;
   using Arashi.Services.Localization;
   using Arashi.Services.Membership;
   using Arashi.Services.SiteStructure;
   using Arashi.Web.Helpers;
   using Arashi.Web.Mvc.Filters;
   using Arashi.Web.Mvc.Models;
   using Common.Logging;


   /// <summary>
   /// Base Controller for all admin controllers
   /// </summary>
   //[SiteFilter(Order = 1)]
   //[AdminLocalizationFilter(Order = 2)] // MUST be after the SiteFilter
   [ExceptionFilter(View = "Error", Order = 0)]
   public abstract class ControllerBase : Controller
   {
      private readonly ILog log;
      
      protected ISiteService siteService;
      protected IUserService userService;
      protected readonly ILocalizationService localizationService;
      private IRequestContext requestContext;
      private ScriptModel scriptModel;
      private readonly IList<MessageModel> registeredMessages;
      private readonly IList<MessageModel> persistentMessages;
      private CultureInfo currentCulture;

      #region Constructors

      /// <summary>
      /// Constructor
      /// </summary>
      /// <param name="log"></param>
      /// <param name="localizationService"></param>
      /// <param name="userService"></param>
      /// <param name="siteService"></param>
      protected ControllerBase(ILog log, ILocalizationService localizationService, IUserService userService, ISiteService siteService)
      {
         this.log = log;
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
      /// <param name="mvcRequestContext"></param>
      protected override void Initialize(RequestContext mvcRequestContext)
      {
         log.Debug("ControllerBase.Initialize");

         this.SetCurrentSite(mvcRequestContext);
         this.SetManagedSite(mvcRequestContext);

         if (requestContext.CurrentUser != null)
         {
            CultureInfo siteSpecificCulture = CultureInfo.CreateSpecificCulture(requestContext.CurrentUser.AdminCulture);
            Thread.CurrentThread.CurrentCulture = siteSpecificCulture;
            Thread.CurrentThread.CurrentUICulture = siteSpecificCulture;
            currentCulture = siteSpecificCulture;

            log.DebugFormat("ControllerBase.Initialize: Setting Thread.CurrentThread.CurrentCulture = {0}", siteSpecificCulture.TwoLetterISOLanguageName);
         }


         TempDataProvider = new CookieTempDataProvider(mvcRequestContext.HttpContext);
         base.Initialize(mvcRequestContext);
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
            log.Debug("ControllerBase.Context setter");
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

      #region Set CurrentSite & ManagedSite

      private void SetCurrentSite(RequestContext context)
      {
         log.Debug("ControllerBase.SetCurrentSite");

         if (siteService == null)
            throw new InvalidOperationException("Unable to set the current site because the SiteService is unavailable");

         Site currentSite = siteService.GetSiteByHostName(WebHelper.GetHostName());

         if (currentSite == null)
         {
            log.Warn("ControllerBase.SetCurrentSite - currentSite == null");
            return;
         }

         requestContext.SetCurrentSite(currentSite);
         requestContext.CurrentSiteDataFolder = context.HttpContext.Server.MapPath(currentSite.SiteDataPath);
         context.HttpContext.Items["CurrentSite"] = currentSite;
         context.HttpContext.Items["CurrentSiteDataFolder"] = requestContext.CurrentSiteDataFolder;
      }



      private void SetManagedSite(RequestContext context)
      {
         log.Debug("ControllerBase.SetManagedSite");

         if (siteService == null)
            throw new InvalidOperationException("Unable to set the current site because the SiteService is unavailable");

         UrlHelper urlHelper = new UrlHelper(context);

         if (urlHelper.RequestContext.RouteData.Values.ContainsKey("siteid"))
         {
            string siteid = urlHelper.RequestContext.RouteData.Values["siteid"].ToString();
            log.DebugFormat("ControllerBase.SetManagedSite - siteid = {0}", siteid);

            if (!string.IsNullOrEmpty(siteid))
            {
               requestContext.SetManagedSite(siteService.GetSiteById(Convert.ToInt32(siteid)));
               context.HttpContext.Items["ManagedSite"] = requestContext.ManagedSite;
            }
         }
      }


      #endregion

      #region Controller Method Overrides

      protected override void OnActionExecuting(ActionExecutingContext filterContext)
      {
         log.Debug("ControllerBase.OnActionExecuting");

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



      /// <summary>
      /// Returns a IDictionary of the localized values of a given Enum.
      /// key,value == Enum_EnumName, LocalName
      /// </summary>
      /// <param name="e"></param>
      /// <returns></returns>
      protected IDictionary<string, string> GetLocalizedEnumList(Type e)
      {
         IDictionary<string, string> items = new Dictionary<string, string>();
         string[] names = Enum.GetNames(e);

         foreach (int i in Enum.GetValues(e))
         {
            string key = e.Name + "_" + names[i];

            // couple key,value == Enum.EnumName, LocalName
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
