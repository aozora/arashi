namespace Arashi.Web.Areas.Admin.Controllers
{
   using System.Web.Mvc;
   using Arashi.Services.Localization;
   using Arashi.Services.Membership;
   using Arashi.Services.SiteStructure;
   using Arashi.Services.Widget;
   using Arashi.Web.Mvc.Controllers;
   using Common.Logging;



   public class WidgetsController : SecureControllerBase
   {
      private ILog log;
      private IWidgetService widgetService;
      private const int pageSize = 20;

            
      #region Constructor

      public WidgetsController(ILog log, IWidgetService widgetService, ILocalizationService localizationService, IUserService userService, ISiteService siteService)
         : base(log, localizationService, userService, siteService)
      {
         this.log = log;
         this.widgetService = widgetService;
      }

      #endregion


      public ActionResult Index(int? page)
      {

         return View("Index"); //, GetIndexData(page));
      }




   }
}
