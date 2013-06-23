using System.Web;

namespace Arashi.Web.Components
{
   public class Log4NetContextHelper
   {
      public override string ToString()
      {
         HttpContext context = HttpContext.Current;
         if (context != null)
         {
            try
            {
               if (context.Request != null)
               {
                  //UrlBuilder url = new UrlBuilder(context.Request.Url);
                  //return url.ToString("p"); // in chiaro!
                  return context.Request.Url.ToString();
               }
            }
            catch
            {
            }
         }
         return string.Empty;
      }
   }
}