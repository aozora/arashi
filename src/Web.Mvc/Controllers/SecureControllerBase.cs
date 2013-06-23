namespace Arashi.Web.Mvc.Controllers
{
   using System.Collections.Generic;
   using System.Linq;
   using System.Web.Mvc;
   using Arashi.Core;
   using Arashi.Core.Domain;
   using Arashi.Services.ControlPanel;
   using Arashi.Services.Localization;
   using Arashi.Services.Membership;
   using Arashi.Services.SiteStructure;
   using Arashi.Web.Mvc.Filters;
   using Arashi.Web.Mvc.Models;
   using Common.Logging;


   /// <summary>
   /// This controller ensure that all action of the derived controllers are accessible
   /// only for authenticated users (AuthorizeAttribute) 
   /// and with the Right AdminAccess (PermissionFilterAttribute)
   /// </summary>
   //[ExceptionFilter()]
   [Authorize] // access only to authenticated and authorized users 
   [PermissionFilter(RequiredRights = Rights.AdminAccess)]
   public class SecureControllerBase : ControllerBase
   {
      #region Private Fields

      private ILog log;

      #endregion

      #region Constructor

      public SecureControllerBase(ILog log, ILocalizationService localizationService, IUserService userService, ISiteService siteService) : base(log, localizationService, userService, siteService)
      {
         this.log = log;
      }

      #endregion

      #region Controller Method Overrides

      /// <summary>
      /// Make available the list of all sites for the current user
      /// </summary>
      /// <param name="filterContext"></param>
      protected override void OnActionExecuting(ActionExecutingContext filterContext)
      {
         log.Debug("SecureControllerBase.OnActionExecuting");

         ViewData["SitesList"] = siteService.GetAllSites();

         if (filterContext.RequestContext.RouteData.Values.ContainsKey("siteid"))
            ViewData["ControlPanelModel"] = GetSiteControlPanelModel();

         base.OnActionExecuting(filterContext);
      }

      #endregion

      /// <summary>
      /// Returns a collection of <see cref="ControlPanelModel"/>
      /// </summary>
      /// <returns></returns>
      public IList<ControlPanelModel> GetSiteControlPanelModel()
      {
         IList<ControlPanelModel> controlPanelModels = new List<ControlPanelModel>();

         // Bind delle icone del Control Panel
         IControlPanelService cpService = IoC.Resolve<IControlPanelService>();
         IList<ControlPanelItem> currentControlPanelItems = cpService.GetControlPanelItems();

         IEnumerable<string> groups = from cpi in currentControlPanelItems
                                      group cpi by cpi.Category into g
                                      select g.Key;

         int counter = 0;

         foreach (string g in groups)
         {
            ControlPanelModel cpm = new ControlPanelModel()
            {
               CategoryId = counter,
               Category = g,
               Items = (from cpi in currentControlPanelItems
                        where cpi.Category == g
                        select cpi).ToList<ControlPanelItem>()
            };
            controlPanelModels.Add(cpm);

            counter++;
         }

         return controlPanelModels;
      }

   }
}
