using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Arashi.Core;
using System.Configuration;

namespace Arashi.Web.Areas.Admin.Extensions
{
   public static class HtmlEditorExtensions
   {
      public static IHtmlString HtmlEditor(this HtmlHelper htmlHelper, string name, string value)
      {
         return HtmlEditor(htmlHelper, name, value, new RouteValueDictionary());
      }



      public static IHtmlString HtmlEditor(this HtmlHelper htmlHelper, string name, string value, object htmlAttributes)
      {
         return HtmlEditor(htmlHelper, name, value, new RouteValueDictionary(htmlAttributes));
      }



      public static IHtmlString HtmlEditor(this HtmlHelper htmlHelper, string name, string value, IDictionary<string, object> htmlAttributes)
      {
         if (!htmlAttributes.ContainsKey("class"))
            htmlAttributes.Add("class", "htmleditor");

         StringBuilder sb = new StringBuilder();
         
         sb.AppendLine(htmlHelper.TextArea(name, value, htmlAttributes).ToString());
         return htmlHelper.Raw(sb.ToString());
      }




      /// <summary>
      /// Render the support scripts for the Html Editor (CKeditor or TinyMCE)
      /// </summary>
      /// <param name="htmlHelper"></param>
      /// <returns></returns>
      public static IHtmlString HtmlEditorScripts(this HtmlHelper htmlHelper)
      {
         string editorType = ConfigurationManager.AppSettings["ContentEditor"];

         StringBuilder sb = new StringBuilder();

         // Ensure that the support script is registered only once for multiple editors
         if (!htmlHelper.ViewContext.ViewData.ContainsKey("htmleditor"))
         {
            htmlHelper.ViewContext.ViewData["htmleditor"] = true;

            switch (editorType)
            {
               case "codemirror":
                  sb.AppendLine("<script type=\"text/javascript\" src=\"/Resources/codemirror/js/codemirror.js\"></script>");
                  sb.AppendLine("<script type=\"text/javascript\" src=\"/Resources/codemirror/arashi.codemirror.js\"></script>");
                  break;

               case "tinymce34":
               default:
                  sb.AppendLine("<script type=\"text/javascript\" src=\"/Resources/tiny_mce34/jquery.tinymce.js\"></script>");
                  sb.AppendLine("<script type=\"text/javascript\" src=\"/Resources/tiny_mce34/arashi.tinymce.js\"></script>");
                  break;

               //case "tinymce":
               //   sb.AppendLine("<script type=\"text/javascript\" src=\"/Resources/tiny_mce/jquery.tinymce.js\"></script>");
               //   sb.AppendLine("<script type=\"text/javascript\" src=\"/Resources/tiny_mce/arashi.tinymce.js\"></script>");
               //  break;
            }
         }

         return htmlHelper.Raw(sb.ToString());
      }



   }
}
