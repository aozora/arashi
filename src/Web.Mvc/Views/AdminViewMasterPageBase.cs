using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Threading;
using Arashi.Core;
using Arashi.Core.Domain;
using Arashi.Core.Domain.Extensions;
using Arashi.Services.Localization;

namespace Arashi.Web.Mvc.Views
{
   using System.Collections.Generic;
   using System.Web.Mvc;
   using System.Web.Mvc.Html;

   using Arashi.Web.Mvc.Models;

   /// <summary>
   /// Base class for the Site.master
   /// </summary>
   public abstract class AdminViewMasterPageBase : WebViewPage // System.Web.Mvc.ViewMasterPage
   {
      private readonly ILocalizationService localizationService;

      #region Constructor

      /// <summary>
      /// Initializes a new instance of the <see cref="AdminViewMasterPageBase"/> class. 
      /// Constructor
      /// </summary>
      protected AdminViewMasterPageBase()
      {
         localizationService = IoC.Resolve<ILocalizationService>();
      }

      #endregion

      #region Public Properties

      /// <summary>
      /// Get the current RequestContext
      /// </summary>
      public IRequestContext RequestContext
      {
         get
         {
            if (ViewData.ContainsKey("Context") && ViewData["Context"] != null)
               return ViewData["Context"] as IRequestContext;
            else
               return null;
         }
      }



      ///// <summary>
      ///// Returns the virtual path for the css resources for the user admin theme
      ///// </summary>
      //public string UserCssThemePath
      //{
      //   get
      //   {
      //      string theme = RequestContext.CurrentUser.AdminTheme.ToLowerInvariant();

      //      // get the configured css path for the telerik extensions
      //      NameValueCollection section = (NameValueCollection)ConfigurationManager.GetSection("telerik");
      //      string path = section["cssPath"] + "/themes/" + theme;

      //      return path;
      //   }
      //}



      /// <summary>
      /// Get the default url for the current managed site, if available
      /// </summary>
      public string ManagedSiteDefaultUrl
      {
         get
         {
            if (RequestContext != null && 
                RequestContext.ManagedSite != null &&
                !string.IsNullOrEmpty(RequestContext.ManagedSite.DefaultUrl()))
               return RequestContext.ManagedSite.DefaultUrl();
            else
               return string.Empty;
         }
      }



      /// <summary>
      /// Returns the managed site id
      /// </summary>
      public string ManagedSiteId
      {
         get
         {
            if (RequestContext != null &&
                RequestContext.ManagedSite != null)
               return RequestContext.ManagedSite.SiteId.ToString();
            else
               return string.Empty;
         }
      }

      #endregion

      #region Localization Support

      /// <summary>
      /// Get a localized global resource
      /// </summary>
      /// <param name="token"></param>
      /// <returns></returns>
      protected string GlobalResource(string token)
      {
         return localizationService.GlobalResource(token, Thread.CurrentThread.CurrentUICulture);
      }



      /// <summary>
      /// Get a localized global resource filled with format parameters
      /// </summary>
      /// <param name="token"></param>
      /// <param name="args"></param>
      /// <returns></returns>
      protected string GlobalResource(string token, params object[] args)
      {
         return string.Format(GlobalResource(token), args);
      }

      #endregion

      public void RenderRegisteredMessages()
      {
         if (!ViewContext.ViewData.ContainsKey("RegisteredMessages") &&
             !ViewContext.TempData.ContainsKey("PersistentMessages"))
            return;

         // render normal messages
         IList<MessageModel> models = ViewContext.ViewData["RegisteredMessages"] as IList<MessageModel>;

         if (models != null)
         {
            foreach (MessageModel model in models)
            {
               Html.RenderPartial("MessageUserControl", model);
            }
         }

         // render persistent messages
         IList<MessageModel> persistentModels = Html.ViewContext.TempData["PersistentMessages"] as IList<MessageModel>;

         if (persistentModels != null)
         {
            foreach (MessageModel model in persistentModels)
            {
               Html.RenderPartial("MessageUserControl", model);
            }
         }

      }

   }
}
