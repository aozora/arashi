using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arashi.Core.Domain;
using System.Web;

namespace Arashi.Web.Widgets
{
   public class WidgetComponentBase :IWidgetComponent
   {
      protected Widget widget;
      protected IRequestContext context;

      #region IWidgetBase Implementations

      public Widget Widget
      {
         get
         {
            return widget;
         }
         set
         {
            widget = value;
         }
      }



      public IRequestContext Context
      {
         set
         {
            context = value;
         }
      }




      public virtual void Init()
      {
      }



      public virtual string Render()
      {
         throw new NotImplementedException();
      }

      #endregion

      #region Protected Helpers

      /// <summary>
      /// Return the protocol (http:// or https://) along with the domain name and port number (if present)
      /// For example, if the requested page's URL is http://www.yourserver.com:8080/Tutorial01/MyPage.aspx, 
      /// then this method returns the string http://www.yourserver.com:8080
      /// </summary>
      /// <returns></returns>
      protected string GetCurrentSiteUrlRoot()
      {
         return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
      }



      /// <summary>
      /// Get a full absolute url for the current request.
      /// </summary>
      /// <param name="partialUrl">
      /// A virtual ("~/") or root-based url ("/")
      /// </param>
      /// <returns></returns>
      protected string GetAbsoluteUrl(string partialUrl)
      {
         return string.Concat(GetCurrentSiteUrlRoot(),
                              "/",
                              partialUrl.StartsWith("~") || partialUrl.StartsWith("/")
                                 ? partialUrl.Substring(1)
                                 : partialUrl);
      }


      #endregion

   }
}
