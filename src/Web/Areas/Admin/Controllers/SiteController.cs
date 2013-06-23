namespace Arashi.Web.Areas.Admin.Controllers
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Web;
   using System.Web.Mvc;

   using Arashi.Core;
   using Arashi.Core.Domain;
   using Arashi.Core.Domain.Dto;
   using Arashi.Core.Extensions;
   using Arashi.Core.Util;
   using Arashi.Services.Content;
   using Arashi.Services.Membership;
   using Arashi.Web.Areas.Admin.Models;
   using Arashi.Web.Mvc.Controllers;
   using Arashi.Web.Mvc.Filters;
   using Arashi.Web.Mvc.Models;

   using log4net;

   using xVal.ServerSide;

   public class SiteController : SecureControllerBase
   {
      private static readonly ILog log = LogManager.GetLogger(typeof(SiteController));
      private IContentItemService<Page> pageService;



      public SiteController(IContentItemService<Page> pageService)
      {
         this.pageService = pageService;
      }



      /// <summary>
      /// View to display the ControlPanel Items for the selected Site
      /// </summary>
      /// <param name="siteid"></param>
      /// <returns></returns>
      [PermissionFilter(RequiredRights = Rights.DashboardAccess)]
      public ActionResult Index(int siteid)
      {
         DashboardModel model = new DashboardModel();
         IList<ControlPanelModel> controlPanelModels = GetSiteControlPanelModel();
         model.ControlPanelModels = controlPanelModels;

         // Quick Stats for Posts
         IContentItemService<Post> contentItemService = IoC.Resolve<IContentItemService<Post>>();
         ContentItemStatsDTO stats = contentItemService.GetStatsBySite(Context.ManagedSite);
         model.ContentItemStats = stats;


         HttpCookie welcomeCookie = this.ControllerContext.HttpContext.Request.Cookies["ShowWelcome"];
         model.ShowWelcome = (welcomeCookie != null);

         // if the cookie exists, set its expiration to now, so it will be canceled
         if (model.ShowWelcome)
         {
            this.ControllerContext.HttpContext.Response.Cookies["ShowWelcome"].Expires = DateTime.Now;
         }

         return View("Index2", model);
      }


      #region Settings

      [PermissionFilter(RequiredRights = Rights.SiteSettingsView)]
      public ActionResult Settings()
      {
         Site site = Context.ManagedSite;

         IList<SiteHost> hosts = siteService.GetSiteHostsBySite(site);
         ViewData["Hosts"] = hosts;

         ViewData["TimeZones"] = new SelectList(TimeZoneUtil.GetTimeZones(), "Key", "Value", Context.ManagedSite.TimeZone);
         ViewData["Cultures"] = new SelectList(Globalization.GetOrderedCultures(), "Name", "DisplayName", site.DefaultCulture);

         // Trick to force the selecteditem ( see http://blog.benhartonline.com/post/2008/11/24/ASPNET-MVC-SelectList-selectedValue-Gotcha.aspx )
         ViewData["SiteStatus"] = new SelectList(GetLocalizedEnumList(typeof(SiteStatus)), "Key", "Value", site.Status.ToString());
         ViewData["Status"] = site.Status.ToString();

         IList<Page> pages = pageService.FindAllBySite(Context.ManagedSite, "Position", true);
         ViewData["Pages"] = new SelectList(pages, "Id", "Title", site.DefaultPage);
         

         return View("Settings", site);
      }



      /// <summary>
      /// Save the site settings
      /// </summary>
      /// <returns></returns>
      [AcceptVerbs(HttpVerbs.Post)]
      [ValidateAntiForgeryToken]
      [PermissionFilter(RequiredRights = Rights.SiteSettingsEdit)]
      public ActionResult Save(string PagesForHomePage)
      {
         Site site = Context.ManagedSite;

         try
         {
            TryUpdateModel(site);

            site.DefaultPage = string.IsNullOrEmpty(PagesForHomePage) ? null : pageService.GetById(Convert.ToInt32(PagesForHomePage));
            site.UpdatedDate = DateTime.UtcNow;

            siteService.SaveSite(site);
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
            };
            RegisterMessage(model);

            ViewData["TimeZones"] = new SelectList(TimeZoneUtil.GetTimeZones(), "Key", "Value", Context.ManagedSite.TimeZone);
            ViewData["Status"] = new SelectList(GetLocalizedEnumList(typeof(SiteStatus)), "Key", "Value", site.Status);
            ViewData["Cultures"] = new SelectList(Globalization.GetOrderedCultures(), "Key", "Value", site.DefaultCulture);
            IList<Page> pages = pageService.FindAllBySite(Context.ManagedSite, "Position", true);
            ViewData["Pages"] = new SelectList(pages, "Id", "Title", site.DefaultPage);

            return View("Settings", site);
         }

         return RedirectToAction("Index", site.SiteId);
      }

      #endregion

      #region New Site

      /// <summary>
      /// Show the view for the creation of a new site
      /// </summary>
      /// <returns></returns>
      [PermissionFilter(RequiredRights = Rights.SiteCreate)]
      public ActionResult NewSite()
      {
         return View("NewSite", new NewSiteModel());
      }



      /// <summary>
      /// Create the new site
      /// </summary>
      /// <returns></returns>
      [AcceptVerbs(HttpVerbs.Post)]
      [ValidateAntiForgeryToken]
      [PermissionFilter(RequiredRights = Rights.SiteCreate)]
      public ActionResult SaveNew()
      {
         NewSiteModel model = new NewSiteModel();

         try
         {
            if (ModelState.IsValid && TryUpdateModel(model))
            {
               Site site = siteService.CreateNewSite(model.Name, model.Description, string.Empty, model.DefaultHostName);


               MessageModel message = new MessageModel
               {
                  Text = "The new site has been created successfully!<br />The Control Panel has already switched to the new site. To manage another one use the site switcher button.",
                  Icon = MessageModel.MessageIcon.Info,
               };
               RegisterMessage(message, true);

               return RedirectToAction("Index", "Site", new {siteid = site.SiteId});
            }
         }
         catch (RulesException ex)
         {
            ex.AddModelStateErrors(ModelState, "newsite");
         }
         catch (Exception ex)
         {
            log.Error("SiteController.SaveNew", ex);

            MessageModel message = new MessageModel
                                    {
                                       Text = GlobalResource("Message_GenericError"),
                                       Icon = MessageModel.MessageIcon.Alert,
                                    };
            RegisterMessage(message, true);
         }

         return View("NewSite", model);
      }

      #endregion

      #region Site Hosts

      /// <summary>
      /// Ajax response for all the Host name for the managed site
      /// </summary>
      /// <param name="sidx"></param>
      /// <param name="sord"></param>
      /// <param name="page"></param>
      /// <param name="rows"></param>
      /// <returns></returns>
      [PermissionFilter(RequiredRights = Rights.SiteSettingsView)]
      public ActionResult GetHosts(string sidx, string sord, int page, int rows)
      {
         IList<SiteHost> hosts = Context.ManagedSite.Hosts;

         var jsonData = new
         {
            total = hosts.Count,
            page = page,
            records = hosts.Count,
            rows = (from host in hosts
                    select new
                    {
                       i = host.SiteHostId,
                       cell = new string[] { host.SiteHostId.ToString(), host.HostName, host.IsDefault.ToString() }
                    }
                    ).ToArray()
         };
         
         return Json(jsonData);
      }



      /// <summary>
      /// Save or Update site host
      /// </summary>
      /// <param name="id">
      /// </param>
      /// <param name="name">
      /// </param>
      /// <param name="oper">
      /// The oper.
      /// </param>
      /// <returns>
      /// </returns>
      [AcceptVerbs(HttpVerbs.Post)]
      [PermissionFilter(RequiredRights = Rights.SiteSettingsEdit)]
      public ActionResult SaveHost(string id, string name, string isdefault, string oper)
      {
         log.DebugFormat("SiteController.SaveHost: id = {0}, name = {1}, isdefault = {2}, oper = {3}", id, name, isdefault, oper);

         SiteHost host = (from h in Context.ManagedSite.Hosts
                          where h.SiteHostId == Convert.ToInt32(id)
                             && h.Site == Context.ManagedSite
                          select h)
                          .SingleOrDefault();

         if (host != null)
         {
            try
            {
               if (oper == "edit")
               {
                  //host.HostName = name;
                  //host.IsDefault = (isdefault.ToLowerInvariant() == "no" ? false : true); // TODO: check in SiteService to reset the IsDefault
                  //siteService.SaveSiteHost(host);
                  siteService.SaveSiteHost(host, name, (isdefault.ToLowerInvariant() == "no" ? false : true));

                  log.Debug("SiteController.SaveHost: save ok!");
               }
               else
               {
                  // delete host

                  // at leat each site MUST have 1 host
                  if (host.Site.Hosts.Count == 1)
                  {
                     log.WarnFormat("SiteController.SaveHost: the managed site has ONLY 1 sitehost, cannot delete it!");
                     Response.StatusCode = 500;
                     return Content("false");
                  }

                  siteService.DeleteSiteHost(host);
                  log.DebugFormat("SiteController.SaveHost: host {0} deleted for SiteId {1}!", host.HostName, host.Site.SiteId.ToString());
               }

               return Content("true");
            }
            catch (Exception ex)
            {
               log.ErrorFormat("SiteController.SaveHost: {0}", ex.ToString());
               Response.StatusCode = 500;
               return Content("false");
            }
         }

         log.ErrorFormat("SiteController.SaveHost: SiteHost not found!");
         Response.StatusCode = 500;
         return Content("false");
      }



      /// <summary>
      /// Add a new host name to the current managed site
      /// </summary>
      /// <param name="newhostname"></param>
      /// <returns></returns>
      [AcceptVerbs(HttpVerbs.Post)]
      [PermissionFilter(RequiredRights = Rights.SiteSettingsEdit)]
      public ActionResult AddHost(string newhostname)
      {
         Site site = Context.ManagedSite;
         
         // Check if the host already exists for the managed site
         SiteHost host = siteService.GetSiteHostByHostName(newhostname);

         if (host != null)
         {
            MessageModel notUniqueHostMessage = new MessageModel()
            {
               Text = string.Format(GlobalResource("Message_SiteHostNotUnique"), newhostname),
               Icon = MessageModel.MessageIcon.Alert
            };
            RegisterMessage(notUniqueHostMessage);

            return View("MessageUserControl", notUniqueHostMessage);
         }

         MessageModel messageModel;

         try
         {
            SiteHost newHost = new SiteHost()
               {
                  HostName = newhostname,
                  IsDefault = false,
                  Site = site,
                  CreatedDate = DateTime.Now.ToUniversalTime()
               };
            siteService.SaveSiteHost(newHost);

            messageModel = new MessageModel
            {
               Text = "The new domain has been saved successfully!",
               Icon = MessageModel.MessageIcon.Info,
            };
            RegisterMessage(messageModel);

         }
         catch (Exception ex)
         {
            log.Error("SiteController.SaveSettings", ex);
            
            messageModel = new MessageModel
            {
               Text = GlobalResource("Message_GenericError"),
               Icon = MessageModel.MessageIcon.Alert,
            };
            RegisterMessage(messageModel);

         }

         return View("MessageUserControl", messageModel);
      }



      /// <summary>
      /// Check if a given host name is already in use
      /// </summary>
      /// <param name="defaultHostName">The host name to check</param>
      /// <returns></returns>
      [AcceptVerbs(HttpVerbs.Post)]
      [PermissionFilter(RequiredRights = Rights.SiteSettingsEdit)]
      public RemoteValidationResult CheckIfSiteHostExists(string defaultHostName)
      {
         SiteHost host = siteService.GetSiteHostByHostName(defaultHostName);

         return host == null ? RemoteValidationResult.Success
                              : RemoteValidationResult.Failure("Sorry, this host name is already in use. Please choose another one.");
      }



      #endregion
   }
}