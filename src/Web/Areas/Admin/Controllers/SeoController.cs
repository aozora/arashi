using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Arashi.Core.Domain;
using Arashi.Services.Membership;
using Arashi.Core.Extensions;
using Arashi.Core.Util;
using Arashi.Web.Mvc.Controllers;
using Arashi.Web.Mvc.Filters;
using Arashi.Web.Mvc.Models;
using log4net;

namespace Arashi.Web.Areas.Admin.Controllers
{
   public class SeoController : SecureControllerBase
   {
      private static readonly ILog log = LogManager.GetLogger(typeof(SeoController));


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
