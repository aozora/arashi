using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arashi.Web.Mvc.TemplateEngine
{
   public class ViewHelper
   {
      public enum TemplateFile
      {
         author,
         contact,
         index,
         single,
         header,
         footer,
         sidebar,
         archive,
         archives,
         comments,
         page,
         search,
         searchform,
         _404,
         _302,
         error
      }


      public enum ViewMode
      {
         is_post,
         is_tag, 
         is_category,
         is_day,
         is_month,
         is_year,
         is_author
      }

   }
}