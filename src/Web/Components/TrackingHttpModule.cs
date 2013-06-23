using System;
using System.Configuration;
using System.Web;
using Arashi.Core;
using Arashi.Services.SiteStructure;
using Arashi.Web.Helpers;
using Common.Logging;
using Arashi.Core.Domain;

namespace Arashi.Web.Components
{
   public class TrackingHttpModule : IHttpModule
   {
      private ILog log = LogManager.GetCurrentClassLogger();


      public TrackingHttpModule()
      {
      }

      #region IHttpModule Members

      public void Init(HttpApplication context)
      {
         context.PostAuthenticateRequest += new EventHandler(Context_PostAuthenticateRequest);
      }


      public void Dispose()
      {
         // Nothing here	
      }

      #endregion

      private void Context_PostAuthenticateRequest(object sender, EventArgs e)
      {
         HttpContext context = HttpContext.Current;

         if (context != null)
         {
            HttpRequest request = context.Request;
            string currentUrl = context.Request.Url.GetLeftPart(UriPartial.Path);

            // Esce e non fa niente se il tracking è disabilitato
            if (!Convert.ToBoolean(ConfigurationManager.AppSettings["EnableTracking"]))
               return;

            // Esclude anche tutte le richieste di risorse ASP.NET (*.axd)
            if (currentUrl.EndsWith(".axd"))
               return;

            // Esce se è uno spider
            if (request.Browser.Crawler)
               return;

            // Esce se non è abilitato il tracking per gli indirizzi locali
            if (!Convert.ToBoolean(ConfigurationManager.AppSettings["AllowTrackingForLocalhost"]) &&
                request.IsLocal)
               return;

            string leftPart = context.Request.Url.GetLeftPart(UriPartial.Authority);


            // exit if the request is for the controlpanel
            if (context.Request.Url.ToString().Substring(leftPart.Length).IndexOf(@"/admin/", StringComparison.OrdinalIgnoreCase) > -1)
               return;

            try
            {
               ISiteService siteService = IoC.Resolve<ISiteService>();
               TrackingInfo info = new TrackingInfo();

               // Url & Referrer
               info.HostReferrer = request.UrlReferrer == null ? null : request.UrlReferrer.Host;
               info.UrlReferrer = request.UrlReferrer == null ? null : request.UrlReferrer.ToString();
               info.TrackedUrl = currentUrl.ToString();
               info.HttpMethod = request.HttpMethod;

               // User info
               if (request.IsAuthenticated)
                  info.LoggedUser = context.User.Identity as User;
               else
                  info.AnonymousUserId = request.AnonymousID;

               info.UserIp = request.UserHostAddress;
               info.UserLanguages = request.UserLanguages == null ? "" : string.Join(",", request.UserLanguages);

               // Browser info
               info.BrowserType = request.Browser.Type;
               info.BrowserName = request.Browser.Browser;
               info.BrowserVersion = request.Browser.Version;
               info.BrowserMajor = request.Browser.MajorVersion.ToString();
               info.BrowserMinor = request.Browser.MinorVersionString;
               info.Platform = request.Browser.Platform;
               info.TrackingDate = DateTime.Now.ToUniversalTime();

               // Salva le info di tracking
               siteService.StoreTrackingInfo(info);
            }
            catch (Exception ex)
            {
               log.Error(ex.ToString());
            }
         }
      }



   }
}