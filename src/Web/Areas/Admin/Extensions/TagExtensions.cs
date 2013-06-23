namespace Arashi.Web.Areas.Admin.Extensions
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Text;
   using System.Web;
   using System.Web.Mvc;
   using Arashi.Core.Domain.Dto;



   public static class TagExtensions
   {

      public static IHtmlString TagCloud(this HtmlHelper htmlHelper, IList<TagDTO> tagCloud)
      {
         int smallest = 10;
         int largest = 30;
         string unit = "px";
         int number = 15;
         //string format = "flat";
         //string orderby = "name";
         //string order = "ASC";
         //string taxonomy = "post_tag";

         IEnumerable<TagDTO> tagsToRender = tagCloud;

         // eventually restrict the tags to the given number at max
         if (number > 0)
         {
            tagsToRender = (from t in tagCloud
                            where t.Count > 0
                            orderby t.Count descending
                            select t).Take(number);
         }


         IEnumerable<TagDTO> tags = from t in tagsToRender
                                    where t.Count > 0
                                    orderby t.Name ascending
                                    select t;

         // if there are no tags (i.e. fresh setup) then exit
         if (tags == null || tags.Count() == 0)
            return htmlHelper.Raw(string.Empty);

         // determine the font size
         long min_count = tags.Min(t => t.Count);
         long spread = tags.Max(t => t.Count) - min_count;
         int font_spread = largest - smallest;
         if (font_spread < 0)
            font_spread = 1;
         double font_step = (double)font_spread / spread;

         StringBuilder html = new StringBuilder();

         foreach (TagDTO tag in tags)
         {
            //string tag_link = GetAbsoluteUrl(siteTags.Single(t => t.TagId == tag.TagId).GetTagUrl());
            string link = string.Format("<a href=\"#\" rel=\"{0}\" style=\"font-size: {2}{3}\">{1}</a>&nbsp;",
                                        tag.TagId.ToString(),
                                        tag.Name,
                                        (smallest + ((tag.Count - min_count) * font_step)).ToString(),
                                        unit);

            html.Append(link);
         }

         html.Insert(0, "<div id=\"tagcloud\">");
         html.Append("</div>");

         return htmlHelper.Raw(html.ToString());
      }


   }
}
