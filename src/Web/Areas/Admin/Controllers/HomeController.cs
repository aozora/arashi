namespace Arashi.Web.Areas.Admin.Controllers
{
   using Arashi.Services.Localization;
   using Arashi.Services.SiteStructure;
   using System.Collections.Generic;
   using System.Web.Mvc;
   using Arashi.Core.Domain;
   using Arashi.Services.Membership;
   using Arashi.Web.Mvc.Controllers;

   using Common.Logging;


   public class HomeController : SecureControllerBase
   {
      private ILog log;
      
            
      #region Constructor

      public HomeController(ILog log, ILocalizationService localizationService, IUserService userService, ISiteService siteService)
         : base(log, localizationService, userService, siteService)
      {
         this.log = log;
      }

      #endregion



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