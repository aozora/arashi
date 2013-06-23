namespace Arashi.Web.Areas.Admin.Controllers
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Web.Mvc;
   using Arashi.Core;
   using Arashi.Core.Domain;
   using Arashi.Services.Content;
   using Arashi.Services.Localization;
   using Arashi.Services.Membership;
   using Arashi.Services.Search;
   using Arashi.Services.SiteStructure;
   using Arashi.Web.Mvc.Controllers;
   using Arashi.Web.Mvc.Filters;
   using Arashi.Web.Mvc.Models;
   using Common.Logging;


   public class SearchIndexController : SecureControllerBase
   {
      private ILog log;
      private static object lockObject = "";

      #region Constructor

      public SearchIndexController(ILog log, ILocalizationService localizationService, IUserService userService, ISiteService siteService)
         : base(log, localizationService, userService, siteService)
      {
         this.log = log;
      }

      #endregion

      [PermissionFilter(RequiredRights = Rights.SiteSettingsView)]
      public ActionResult Index()
      {
         return View("Index");
      }




      [AcceptVerbs(HttpVerbs.Post)]
      [PermissionFilter(RequiredRights = Rights.SiteSettingsEdit)]
      [ValidateAntiForgeryToken]
      public ActionResult Rebuild()
      {
         try
         {
            //only one thread at a time
            System.Threading.Monitor.Enter(lockObject);

            IContentItemService<IContentItem> contentItemService = IoC.Resolve<IContentItemService<IContentItem>>();
            ISearchService searchService = IoC.Resolve<ISearchService>();
            IEnumerable<IContentItem> contentItemsToIndex = from c in contentItemService.FindAllBySite(Context.ManagedSite)
                                                            where c.PublishedDate.HasValue == true
                                                               && c.PublishedDate.Value <= DateTime.Now
                                                            select c;

            searchService.RebuildIndex(Context.ManagedSite, contentItemsToIndex);

            MessageModel model = new MessageModel
            {
               Text = "Rebuild completed!",
               Icon = MessageModel.MessageIcon.Info,
               CssClass = "margin-topbottom",
               IsClosable = true
            };
            RegisterMessage(model, true);

         }
         catch (Exception ex)
         {
            log.Error(ex.ToString());

            // Show the confirmation message
            MessageModel model = new MessageModel
            {
               Text = ex.Message,
               Icon = MessageModel.MessageIcon.Alert,
               CssClass = "margin-topbottom"
            };
            RegisterMessage(model, true);
         }
         finally
         {
            System.Threading.Monitor.Exit(lockObject);
         }

         return View("Index");
      }

   }
}
