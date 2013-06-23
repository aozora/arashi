using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Arashi.Core.Extensions;
using Arashi.Web.Helpers;

namespace Arashi.Web.Mvc.Gravatar
{
   /// <summary>
   /// Extension for the Gravatar images
   /// </summary>
   public static class GravatarExtensions
   {
      /// <summary>
      /// Get a image element with the src attribute for the gravatar service
      /// </summary>
      /// <param name="helper"></param>
      /// <param name="email"></param>
      /// <param name="size"></param>
      /// <returns></returns>
      public static string GravatarImage(this HtmlHelper helper, string email, int size)
      {
         string src = helper.GravatarUrl(email, size);

         return string.Format("<img class=\"avatar\" src=\"{0}\" width=\"{1}\" height=\"{1}\" alt=\"Gravatar\" />", src, size);
      }



      /// <summary>
      /// Get only the url to retrieve the gravatar image
      /// </summary>
      /// <param name="helper"></param>
      /// <param name="email"></param>
      /// <param name="size"></param>
      /// <returns></returns>
      public static string GravatarUrl(this HtmlHelper helper, string email, int size)
      {
         if (email == null)
            email = string.Empty;

         UrlHelper urlHelper = new UrlHelper(helper.ViewContext.RequestContext, helper.RouteCollection);
         string defaultAvatar = helper.ViewContext.HttpContext.Request.Url.GetLeftPart(UriPartial.Authority).Contains("localhost") ? "wavatar" : urlHelper.Encode(WebHelper.GetSiteRoot() + "/Resources/img/gravatar-default.png");

         return string.Concat("http://gravatar.com/avatar/",
                              email.ToLowerInvariant().EncryptToMD5(),
                              ".jpg?s=", size, "&amp;d=", defaultAvatar);
      }


   }
}
