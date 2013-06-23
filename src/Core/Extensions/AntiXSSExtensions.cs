using System;
using System.Web;
//using Microsoft.Security.Application;

namespace Arashi.Core.Extensions
{
   [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "XSS")]
   public static class AntiXSSExtensions
   {
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings")]
      public static string UrlEncode(this string input)
      {
         //return AntiXss.UrlEncode(input);
         return HttpUtility.UrlEncode(input);
      }

      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings")]
      public static string UrlDecode(this string input)
      {
         return HttpUtility.UrlDecode(input);
      }

      public static string HtmlEncode(this string input)
      {
         //return AntiXss.HtmlEncode(input);
         return HttpUtility.HtmlEncode(input);
      }

      public static string HtmlDecode(this String input)
      {
         if (HttpContext.Current != null)
            return HttpContext.Current.Server.HtmlDecode(input);
         else
            return null;
      }

      public static string HtmlAttributeEncode(this string input)
      {
         //return AntiXss.HtmlAttributeEncode(input);
         return HttpUtility.HtmlAttributeEncode(input);
      }

      //public static string XmlEncode(this string input)
      //{
      //   return AntiXss.XmlEncode(input);
      //}

      //public static string XmlAttributeEncode(this string input)
      //{
      //   return AntiXss.XmlAttributeEncode(input);
      //}

      //public static string JavaScriptEncode(this string input)
      //{
      //   return AntiXss.JavaScriptEncode(input);
      //}

   }
}