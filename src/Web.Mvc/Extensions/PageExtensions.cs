namespace Arashi.Web.Mvc.Extensions
{
   using System.Linq;
   using System.Web.Mvc;

   using Arashi.Core.Domain;

   using System.Collections.Generic;
   using System.Web.Mvc.Html;

   public static class PageExtensions
   {
      /// <summary>
      /// Render a select with the list of parent Page with a pseudo hyerarchical tree. Also add an empty item at first.
      /// </summary>
      /// <param name="helper"></param>
      /// <param name="name"></param>
      /// <param name="parentPages"></param>
      /// <param name="selectedPage"></param>
      /// <returns></returns>
      public static MvcHtmlString ParentPageDropDownList(this HtmlHelper helper, string name, IList<Page> parentPages, Page selectedPage)
      {
         IEnumerable<SelectListItem> items = from p in parentPages
                                             select new SelectListItem
                                             {
                                                //Text = p.DepthTitle.Replace("-", "&nbsp;&nbsp;&nbsp;"),
                                                Text = p.DepthTitle,
                                                Value = p.Id.ToString(),
                                                Selected = (p.Equals(selectedPage))
                                             };
         return helper.DropDownList(name, items, string.Empty);
      }



      /// <summary>
      /// Create a IEnumerable<SelectListItem> for a list of pages
      /// </summary>
      /// <param name="pages"></param>
      /// <param name="selectedPage"></param>
      /// <returns></returns>
      public static IEnumerable<SelectListItem> AsSelectList(this IList<Page> pages, Page selectedPage)
      {
         return pages.Select(p => new SelectListItem()
         {
            Text = p.Title,
            Value = p.Id.ToString(),
            Selected = (p.Equals(selectedPage))
         });
      }


   }
}