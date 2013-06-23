using System.Web.Mvc;
using Arashi.Services.Widget;
using Arashi.Web.Mvc.Controllers;
using log4net;

namespace Arashi.Web.Areas.Admin.Controllers
{
   public class WidgetsController : SecureControllerBase
   {
      private static readonly ILog log = LogManager.GetLogger(typeof(WidgetsController));
      private IWidgetService widgetService;
      private const int pageSize = 20;

      #region Constructor

      public WidgetsController(IWidgetService widgetService)
      {
         this.widgetService = widgetService;
      }

      #endregion


      public ActionResult Index(int? page)
      {

         return View("Index"); //, GetIndexData(page));
      }




   }
}
