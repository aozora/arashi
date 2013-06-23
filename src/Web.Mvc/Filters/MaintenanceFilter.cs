namespace Arashi.Web.Mvc.Filters
{
   using System;
   using System.Configuration;
   using System.Linq;
   using System.Web;
   using System.Web.Mvc;

   using Arashi.Core;
   using Arashi.Core.Domain;
   using Arashi.Core.Extensions;
   using Arashi.Services.SiteStructure;
   using Arashi.Web.Helpers;

   using Common.Logging;



   /// <summary>
   /// This MVC Filter check if the server status in set to "maintenance" status,
   /// The server status is set in the web.config by the "MaintenanceMode_Enabled" key; 
   /// 
   /// Original code here: http://imperugo.tostring.it/blog/post/mettere-un-sito-in-maintenance-mode
   /// </summary>
   public class MaintenanceFilter : ActionFilterAttribute
   {
      private ILog log = LogManager.GetCurrentClassLogger();
      private string view;


      public string View
      {
         get
         {
            if (string.IsNullOrEmpty(view))
            {
               return "/302/";
            }
            return view;
         }
         set
         {
            view = value;
         }
      }



      public override void OnActionExecuting(ActionExecutingContext filterContext)
      {
         // exclude the static resources & subfolders
         if (filterContext.HttpContext.Request.Url.ToString().EnsureEndingSlash() != filterContext.HttpContext.Request.Url.GetLeftPart(UriPartial.Authority).EnsureEndingSlash() &&
            filterContext.HttpContext.Request.Url.ToString().Right(5).Contains("."))
            return;

         // Get server status
         bool serverMaintenanceModeIsActive = Convert.ToBoolean(ConfigurationManager.AppSettings["MaintenanceMode_Enabled"]);

         // If all is online then exit
         if (!serverMaintenanceModeIsActive)
            return;

         // Get site status
         ISiteService siteService = IoC.Resolve<ISiteService>();
         Site currentSite = siteService.GetSiteByHostName(WebHelper.GetHostName());

         log.Info("Maintenance Mode ON");
         string[] allowedIP = ConfigurationManager.AppSettings["MaintenanceMode_AllowedIP"].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
         log.Info("Maintenance Mode: Allowed IP = " + ConfigurationManager.AppSettings["MaintenanceMode_AllowedIP"]);

         if (currentSite != null)
         {
            string pagePath = @"/302/";

            // exclude the maintenance page itself
            if (filterContext.HttpContext.Request.Url.AbsolutePath.EnsureEndingSlash() == pagePath)
               return;
            
            if (allowedIP.Contains(filterContext.HttpContext.Request.UserHostAddress))
            {
               log.InfoFormat("Maintenance Mode: Request from {0} is allowed. Process it normally.",
                              filterContext.HttpContext.Request.UserHostAddress);
               return;
            }

            log.Info("Maintenance Mode: redirect to maintenance page!");


            string viewPath = this.View;
            Theme currentTheme = currentSite.Theme;
            int httpStatusCode = 302;

            //if (currentSite.Status == SiteStatus.Offline)
            //   httpStatusCode = 503;

            //if (File.Exists(filterContext.HttpContext.Server.MapPath(currentTheme.BasePath + "/302_sitedown.aspx")))
            //   viewPath = currentTheme.BasePath + "/302_sitedown.aspx";


            //filterContext.Result = new ViewResult
            //{
            //   ViewName = viewPath,  
            //   TempData = filterContext.Controller.TempData
            //};

            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.StatusCode = httpStatusCode;
            filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            // the following is required by the W3C specifications
            filterContext.HttpContext.Response.AddHeader("Location", pagePath);
         }

      }

   }
}
