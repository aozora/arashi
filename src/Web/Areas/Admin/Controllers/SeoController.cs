namespace Arashi.Web.Areas.Admin.Controllers
{
   using System;
   using System.Web.Mvc;
   using Arashi.Core.Domain;
   using Arashi.Core.Extensions;
   using Arashi.Services.Localization;
   using Arashi.Services.Membership;
   using Arashi.Services.SiteStructure;
   using Arashi.Web.Mvc.Controllers;
   using Arashi.Web.Mvc.Filters;
   using Arashi.Web.Mvc.Models;
   using Common.Logging;


   public class SeoController : SecureControllerBase
   {
      private ILog log;

      #region Constructor

      public SeoController(ILog log, ILocalizationService localizationService, IUserService userService, ISiteService siteService)
         : base(log, localizationService, userService, siteService)
      {
         this.log = log;
      }

      #endregion

      [PermissionFilter(RequiredRights = Rights.SiteSettingsView)]
      public ActionResult Index()
      {
         SeoSettings seo = Context.ManagedSite.SeoSettings;

         if (seo == null)
         {
            seo = new SeoSettings
            {
               Site = Context.ManagedSite
            };
         }

         return View("Index", seo);
      }




      [AcceptVerbs(HttpVerbs.Post)]
      [PermissionFilter(RequiredRights = Rights.SiteSettingsEdit)]
      [ValidateAntiForgeryToken]
      public ActionResult Save()
      {
         SeoSettings seo = Context.ManagedSite.SeoSettings ?? new SeoSettings{Site = Context.ManagedSite};

         try
         {
            UpdateModel(seo);

            // TODO: add date
            //seo.UpdatedDate = DateTime.UtcNow;

            Context.ManagedSite.SeoSettings = seo;

            siteService.SaveSite(Context.ManagedSite);
         }
         catch (Exception ex)
         {
            log.Error("SiteController.SaveSettings", ex);

            foreach (string key in this.ModelState.Keys)
            {
               if (this.ModelState[key].Errors.Count > 0)
                  this.ModelState[key].Errors.Each().Do(error => log.Error(error.Exception.ToString()));
            }

            MessageModel model = new MessageModel
            {
               Text = GlobalResource("Message_GenericError"),
               Icon = MessageModel.MessageIcon.Alert,
               CssClass = "margin-topbottom"
            };
            RegisterMessage(model);

            return View("Index", seo);
         }

         return RedirectToAction("Index", "Site", Context.ManagedSite.SiteId);
      }

   }
}
