using System.Collections.Generic;
using System.Web.Mvc;
using Arashi.Core.Domain;
using Arashi.Core.Domain.Extensions;
using Arashi.Services.Membership;
using Arashi.Core.Util;
using Arashi.Web.Areas.Admin.Models;
using Arashi.Web.Mvc.Controllers;
using Arashi.Web.Mvc.Filters;
using log4net;

namespace Arashi.Web.Areas.Admin.Controllers
{
   public class HomeController : SecureControllerBase
   {
      private static readonly ILog log = LogManager.GetLogger(typeof(HomeController));
      

      /// <summary>
      /// Main Action
      /// </summary>
      /// <returns></returns>
      public ActionResult Index()
      {
         // TODO: show only sites for the logged user
         IList<Site> sites = siteService.GetAllSites();
         //IList<SiteModel> models = new List<SiteModel>();

         //foreach (Site site in siteService.GetAllSites())
         //{
         //   if (Context.CurrentUser.HasRight(Rights.AccessAdmin, site))
         //   {
         //      SiteModel model = new SiteModel
         //                           {
         //                              Site = site,
         //                              DefaultHostName = site.DefaultSiteHost == null ? "(not defined)" : site.DefaultSiteHost.HostName,
         //                              //PagesCount = pageService.CountAllPages(site),
         //                              UsersCount = userService.CountAllUsers(site),
         //                              CreatedDate = TimeZoneUtil.AdjustDateToTimeZone(site.CreatedDate, Context.CurrentUser.TimeZone)
         //                           };

         //      models.Add(model);
         //   }
         //}

         return View("Index", sites);
      }



      public ActionResult Credits()
      {
         return View();
      }




      public ActionResult About()
      {
         return View();
      }
   }
}