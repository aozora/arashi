using System;
using System.Linq;
using System.Configuration;
using System.Web;
using Arashi.Core;
using Arashi.Core.Cms.Domain;
using Arashi.Core.Cms.Services.SiteStructure;
using Arashi.Core.Extensions;
using Arashi.Web.Helpers;
using log4net;

namespace Arashi.Web.Components
{
   /// <summary>
   /// Maintenance HttpModule
   /// See http://imperugo.tostring.it/blog/post/mettere-un-sito-in-maintenance-mode#feedback
   /// </summary>
   public class MaintenanceHttpModule : IHttpModule
   {
      private static readonly ILog log = LogManager.GetLogger(typeof(MaintenanceHttpModule));

      #region IHttpModule Members

      public void Dispose()
      {
      }


      public void Init(HttpApplication context)
      {
         if (Convert.ToBoolean(ConfigurationManager.AppSettings["MaintenanceMode_Enabled"]))
            context.BeginRequest += ContextBeginRequest;
      }

      #endregion


      private static void ContextBeginRequest(object sender, EventArgs e)
      {
         HttpContext context = HttpContext.Current;

         // exclude the static resources & subfolders
         if (context.Request.Url.ToString().EnsureEndingSlash() != context.Request.Url.GetLeftPart(UriPartial.Authority).EnsureEndingSlash() &&
            context.Request.Url.ToString().Right(5).Contains("."))
            return;

         log.Info("Maintenance Mode ON");
         string[] allowedIP = ConfigurationManager.AppSettings["MaintenanceMode_AllowedIP"].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
         log.Info("Maintenance Mode: Allowed IP = " + ConfigurationManager.AppSettings["MaintenanceMode_AllowedIP"]);

         ISiteService siteService = IoC.Resolve<ISiteService>();
         Site currentSite = siteService.GetSiteByHostName(WebHelper.GetHostName());

         if (currentSite != null)
         {
            string pagePath = string.Concat(currentSite.Template.BasePath.Substring(1), "/302_sitedown.aspx");

            if (context.Request.Url.AbsolutePath == pagePath)
               return;

            if (allowedIP.Contains(context.Request.UserHostAddress))
            {
               log.InfoFormat("Maintenance Mode: Request from {0} is allowed. Process it normally.",
                              context.Request.UserHostAddress);
               return;
            }

            log.Info("Maintenance Mode: redirect to maintenance page!");

            context.Response.StatusCode = 302;
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            context.Response.AddHeader("Location", pagePath);
         }
      }


   }
}
