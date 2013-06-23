using System;
using System.Text;
using Arashi.Web.Mvc.Plugins;

namespace Arashi.Web.Plugins.Google
{
   using System.Web;

   public static class Analytics
   {
      /// <summary>
      /// Render the Google Analytics async script
      /// See http://code.google.com/apis/analytics/docs/tracking/asyncTracking.html
      /// </summary>
      /// <param name="pluginHelper"></param>
      /// <returns></returns>
      public static string GoogleAnalytics(this PluginHelper pluginHelper)
      {
         // Exclude localhost 
         bool isLocalhost = pluginHelper.ViewContext.HttpContext.Request.Url.Host.IndexOf("localhost") > -1;

         if (!string.IsNullOrEmpty(pluginHelper.Model.Site.TrackingCode) && !isLocalhost)
         {
            StringBuilder js = new StringBuilder();
            js.Append("<script type=\"text/javascript\">");
            // New async script
            // see http://code.google.com/apis/analytics/docs/tracking/asyncTracking.html
            js.Append("var _gaq = _gaq || [];");
            js.AppendFormat("_gaq.push(['_setAccount', '{0}']);", pluginHelper.Model.Site.TrackingCode);
            js.Append("_gaq.push(['_trackPageview']);");
            js.Append("(function() {");
            js.Append("    var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;");
            js.Append("    ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';");
            js.Append("    var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);");
            js.Append("})();");
            js.Append("</script>");
            
            return js.ToString();
         }
         else
         {
            return string.Empty;
         }
      }

   }
}
