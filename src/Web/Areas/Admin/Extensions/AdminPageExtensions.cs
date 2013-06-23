using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using Arashi.Core.Domain;
using System.Collections.Generic;
using System.Web.Mvc.Html;

namespace Arashi.Web.Areas.Admin.Extensions
{
   public static class AdminPageExtensions
   {
      public static MvcHtmlString ParentPageDropDownList(this HtmlHelper helper, string name, IList<Page> parentPages, Page selectedPage)
      {
         IEnumerable<SelectListItem> items = from p in parentPages
                                             select new SelectListItem
                                             {
                                                Text = p.DepthTitle.Replace("-", "&nbsp;&nbsp;&nbsp;"),
                                                Value = p.Id.ToString(),
                                                Selected = (p.Equals(selectedPage))
                                             };
         return helper.DropDownList(name, items, string.Empty);
      }


   }
}