using System.Linq;
using System.Threading;

namespace Arashi.Web.Areas.Admin.Controllers
{
   using System;
   using System.Collections.Generic;
   using System.Web.Mvc;
   using Arashi.Core.Domain;
   using Arashi.Core.Extensions;
   using Arashi.Services.Localization;
   using Arashi.Services.Membership;
   using Arashi.Services.SiteStructure;
   using Arashi.Services.SystemService;
   using Arashi.Web.Mvc.Controllers;
   using Arashi.Web.Mvc.Filters;
   using Arashi.Web.Mvc.Models;
   using Common.Logging;



   /// <summary>
   /// SystemConfigurationController
   /// </summary>
   [ExceptionFilter(View = "Error")]
   [Authorize]
   [PermissionFilter(RequiredRights = Rights.AdminAccess)]
   public class SystemConfigurationController : SecureControllerBase // SecureControllerBase
   {
      private readonly ILog log;
      private readonly ISystemConfigurationService systemConfigurationService;
      private readonly IFeatureService featureService;

      #region Constructor

      public SystemConfigurationController(ILog log, ISystemConfigurationService systemConfigurationService, IFeatureService featureService, ILocalizationService localizationService, IUserService userService, ISiteService siteService)
         : base(log, localizationService, userService, siteService)
      {
         this.log = log;
         this.systemConfigurationService = systemConfigurationService;
         this.featureService = featureService;
      }

      #endregion

      #region System Settings

      /// <summary>
      /// View to display the <see cref="SystemConfiguration"/>
      /// </summary>
      /// <returns></returns>
      [PermissionFilter(RequiredRights = Rights.SystemConfigurationView)]
      public ActionResult Index()
      {
         SystemConfiguration systemConfiguration = systemConfigurationService.Get();

         if (systemConfiguration == null)
            systemConfiguration = new SystemConfiguration();

         return View("Index", systemConfiguration);
      }




      /// <summary>
      /// Save the system configuration
      /// </summary>
      /// <returns></returns>
      [AcceptVerbs(HttpVerbs.Post)]
      [ValidateAntiForgeryToken]
      [PermissionFilter(RequiredRights = Rights.SystemConfigurationEdit)]
      public ActionResult Save()
      {
         SystemConfiguration systemConfiguration = systemConfigurationService.Get();

         try
         {
            UpdateModel(systemConfiguration);
            systemConfigurationService.Save(systemConfiguration);

            MessageModel message = new MessageModel
            {
               Text = "The system configuration has been saved successfully!",
               Icon = MessageModel.MessageIcon.Info,
            };

            TempData["PersistentMessages"] = new List<MessageModel>() { message };
         }
         catch (Exception ex)
         {
            log.Error("SystemConfigurationController.Save", ex);

            foreach (string key in this.ModelState.Keys)
            {
               if (this.ModelState[key].Errors.Count > 0)
                  this.ModelState[key].Errors.Each().Do(error => log.Error(error.Exception.ToString()));
            }

            MessageModel message = new MessageModel
            {
               Text = GlobalResource("Message_GenericError"),
               Icon = MessageModel.MessageIcon.Alert,
            };

            //RegisterMessage(model);
            TempData["PersistentMessages"] = new List<MessageModel>() { message };
         }

         return RedirectToAction("Index", "Home");
      }

      #endregion

      #region Site Features

      [HttpGet]
      [PermissionFilter(RequiredRights = Rights.SystemConfigurationView)]
      public ActionResult SiteFeatures()
      {
         return View("SiteFeatures", siteService.GetAllSites());
      }



      /// <summary>
      /// GetSiteFeatures
      /// </summary>
      /// <param name="sid">Managed Site Id</param>
      /// <param name="sidx"></param>
      /// <param name="sord"></param>
      /// <param name="page"></param>
      /// <param name="rows"></param>
      /// <returns></returns>
      [PermissionFilter(RequiredRights = Rights.SystemConfigurationView)]
      public JsonResult GetSiteFeatures(int sid, string sidx, string sord, int page, int rows)
      {
         Site site = siteService.GetSiteById(sid);

         // if there aren't features for the selected site, addit all disabled
         if (site.Features.Count == 0)
            featureService.SetFeaturesForSite(site);

         return Json(new
         {
            total = site.Features.Count,
            page = page,
            records = site.Features.Count,
            rows = (from sf in site.Features
                    select new
                    {
                       i = sf.SiteFeatureId,
                       cell = new string[] { 
                          sf.SiteFeatureId.ToString(), 
                          sf.Feature.Name, 
                          sf.Enabled.ToString(), 
                          sf.StartDate.AdjustDateToTimeZone(Context.CurrentUser.TimeZone).ToShortDateString(),
                          sf.EndDate.HasValue ? sf.EndDate.Value.AdjustDateToTimeZone(Context.CurrentUser.TimeZone).ToShortDateString() : string.Empty
                       }
                    }
                    ).ToArray()
         },
        JsonRequestBehavior.AllowGet);
      }



      /// <summary>
      /// SaveSiteFeatures
      /// </summary>
      /// <param name="sid">Managed Site id</param>
      /// <param name="id"></param>
      /// <param name="name"></param>
      /// <param name="enabled"></param>
      /// <param name="startdate"></param>
      /// <param name="enddate"></param>
      /// <param name="oper"></param>
      /// <returns></returns>
      [HttpPost]
      public ActionResult SaveSiteFeatures(int sid,string id, string name, string enabled, string startdate, string enddate, string oper)
      {
         log.DebugFormat("id = {0}, name = {1}, enabled = {2}, startdate = {3}, enddate = {4}, oper = {5}, ", id, name, enabled, startdate, enddate, oper);

         Site site = siteService.GetSiteById(sid);

         if (oper == "edit")
         {
            try
            {
            SiteFeature sf = featureService.FindSiteFeatureById(Convert.ToInt32(id));

            sf.Enabled = Convert.ToBoolean(enabled.ToLower());
            sf.StartDate = DateTime.ParseExact(startdate, "d", Thread.CurrentThread.CurrentCulture.DateTimeFormat);

            if (string.IsNullOrEmpty(enddate))
               sf.EndDate = null;
            else
               sf.EndDate = DateTime.ParseExact(enddate, "d", Thread.CurrentThread.CurrentCulture.DateTimeFormat);

            featureService.SaveSiteFeature(sf);

            return Content("true");

            }
            catch (Exception ex)
            {
               log.Error(ex.ToString());
               return Content("false");
            }
         }

         return Content("false");
      }



      [HttpGet]
      public ActionResult EnableAllSiteFeatures(int sid)
      {
         Site site = siteService.GetSiteById(sid);

         try
         {
            foreach (SiteFeature siteFeature in site.Features)
            {
               siteFeature.Enabled = true;
               featureService.SaveSiteFeature(siteFeature);
            }

            siteService.SaveSite(site);

            MessageModel message = new MessageModel
            {
               Text = GlobalResource("AllSiteFeaturesEnabled"),
               Icon = MessageModel.MessageIcon.Info,
            };
            RegisterMessage(message, true);
         }
         catch (Exception ex)
         {
            log.Error(ex.ToString());

            MessageModel message = new MessageModel
            {
               Text = GlobalResource("Message_GenericError"),
               Icon = MessageModel.MessageIcon.Alert,
            };
            RegisterMessage(message, true);
         }

         return View("SiteFeatures", siteService.GetAllSites());

      }

      #endregion

   }
}