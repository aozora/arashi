using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Arashi.Core.Domain;
using Arashi.Services.Membership;
using Arashi.Services.SystemService;
using Arashi.Core.Extensions;
using Arashi.Web.Mvc.Controllers;
using Arashi.Web.Mvc.Filters;
using Arashi.Web.Mvc.Models;
using log4net;

namespace Arashi.Web.Areas.Admin.Controllers
{
   /// <summary>
   /// SystemConfigurationController
   /// </summary>
   [ExceptionFilter(View = "Error")]
   [Authorize]
   [PermissionFilter(RequiredRights = Rights.AdminAccess)]
   public class SystemConfigurationController : SecureControllerBase // SecureControllerBase
   {
      private static readonly ILog log = LogManager.GetLogger(typeof(SystemConfigurationController));
      private ISystemConfigurationService systemConfigurationService;

      #region Constructor

      public SystemConfigurationController(ISystemConfigurationService systemConfigurationService)
      {
         this.systemConfigurationService = systemConfigurationService;
      }

      #endregion



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
            TempData["PersistentMessages"] = new List<MessageModel>(){message};
         }

         return RedirectToAction("Index", "Home");
      }

   }
}