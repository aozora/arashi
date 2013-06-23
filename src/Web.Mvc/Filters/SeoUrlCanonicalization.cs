using System.Web;
using System.Web.Mvc;

namespace Arashi.Web.Mvc.Filters
{
   public class SeoUrlCanonicalization : ActionFilterAttribute
   {
      public override void OnActionExecuting(ActionExecutingContext filterContext)
      {
         // grab the URL:
         HttpContextBase current = filterContext.HttpContext;
         string pathAndQuery = current.Request.Url.PathAndQuery ?? "/";
         string path = current.Request.Url.AbsolutePath ?? "/";

         // check for any upper-case letters:
         if (pathAndQuery != pathAndQuery.ToLower())
         {
            string newLocation = pathAndQuery.ToLower();

            current.Response.StatusCode = 301;
            current.Response.TrySkipIisCustomErrors = true;
            current.Response.Status = "301 Moved Permanently";
            current.Response.AppendHeader("Location", newLocation);
            return;
         }

         // make sure that the path ends in a "/"
         //      (doesn't apply in some cases... but those methods/etc won't
         //          be explicitly decorated with this attribute)
         if (!path.EndsWith("/"))
         {
            //string newLocation = pathAndQuery + "/";
            string newLocation = path + "/" + current.Request.Url.Query; // note: "current.Request.Url.Query" contains also the '?'

            current.Response.StatusCode = 301;
            current.Response.TrySkipIisCustomErrors = true;
            current.Response.Status = "301 Moved Permanently";
            current.Response.AppendHeader("Location", newLocation);
            return;
         }

         base.OnActionExecuting(filterContext);
      }
   }
}
