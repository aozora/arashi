using System.Web.Mvc;
using Arashi.Web.Mvc.Partials;

namespace Arashi.Web.Mvc.Partials
{
   public static class PartialRequestsExtensions
   {
      public static void RenderPartialRequest(this HtmlHelper html, string viewDataKey)
      {
         PartialRequest partial = html.ViewContext.ViewData.Eval(viewDataKey) as PartialRequest;
         if (partial != null)
         {
            partial.Invoke(html.ViewContext);
         }
      }
   }
}