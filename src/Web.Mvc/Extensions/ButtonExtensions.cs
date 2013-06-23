namespace Arashi.Web.Mvc.Extensions
{
   using System;
   using System.Text;
   using System.Web.Mvc;
   using System.Web.Routing;
   using System.Web;

   public static class ButtonExtensions
   {
      #region SubmitUI

      public enum UIButtonType
      {
         Submit,
         Button
      }

      /// <summary>
      /// Render a submit button element formatted for jQueryUI CSS Framework
      /// </summary>
      /// <param name="helper"></param>
      /// <param name="text"></param>
      /// <returns></returns>
      public static IHtmlString SubmitUI(this HtmlHelper helper, string text)
      {
         return SubmitUI(helper, UIButtonType.Submit, text);
      }


      /// <summary>
      /// Render a submit button element formatted for jQueryUI CSS Framework
      /// </summary>
      /// <param name="helper"></param>
      /// <param name="text"></param>
      /// <param name="cssClass"></param>
      /// <returns></returns>
      public static IHtmlString SubmitUI(this HtmlHelper helper, string text, string cssClass)
      {
         return SubmitUI(helper, UIButtonType.Submit, text, cssClass);
      }



      /// <summary>
      /// Render a button element formatted for jQueryUI CSS Framework
      /// </summary>
      /// <param name="helper"></param>
      /// <param name="buttonType"></param>
      /// <param name="text"></param>
      /// <returns></returns>
      public static IHtmlString SubmitUI(this HtmlHelper helper, UIButtonType buttonType, string text)
      {
         return helper.Raw(String.Format("<button type=\"{0}\" class=\"button ui-shadow\" >{1}</button>", buttonType.ToString().ToLower(), text));
      }


      public static IHtmlString SubmitUI(this HtmlHelper helper, UIButtonType buttonType, string text, string cssClass)
      {
         return helper.Raw(String.Format("<button type=\"{0}\" class=\"button ui-shadow {2}\" >{1}</button>", buttonType.ToString().ToLower(), text, cssClass));
      }

      #endregion

      #region ActionLink UI

      /// <summary>
      /// Render an ActionLink formatted with jQueryUI CSS style
      /// </summary>
      /// <param name="helper"></param>
      /// <param name="linkText"></param>
      /// <param name="actionName"></param>
      /// <param name="controllerName"></param>
      /// <param name="routeValues"></param>
      /// <param name="linkHtmlAttributes"></param>
      /// <returns></returns>
      public static IHtmlString ActionLinkUI(this HtmlHelper helper, 
                                          string linkText, 
                                          string actionName, 
                                          string controllerName, 
                                          object routeValues, 
                                          object linkHtmlAttributes)
      {
         string anchorCssClass = "button ui-shadow";

         UrlHelper urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
         string url = urlHelper.Action(actionName, controllerName, routeValues);

         StringBuilder html = new StringBuilder();
         html.AppendFormat("<a href=\"{0}\" class=\"{1}\">", url, anchorCssClass);
         
         html.Append(helper.Encode(linkText));
         html.Append("</a>");

         return helper.Raw(html.ToString());
      }

      #endregion

      #region ImageLink

      public static string ImageLink(this HtmlHelper helper, string actionName, string controllerName, string imageUrl, string alternateText)
      {
         return ImageLink(helper, actionName, controllerName, imageUrl, alternateText, null, null, null);
      }



      public static string ImageLink(this HtmlHelper helper, string actionName, string controllerName, string imageUrl, string alternateText, object routeValues)
      {
         return ImageLink(helper, actionName, imageUrl, controllerName, alternateText, routeValues, null, null);
      }



      public static string ImageLink(this HtmlHelper helper, string actionName, string controllerName, string imageUrl, string alternateText, object routeValues, object linkHtmlAttributes, object imageHtmlAttributes)
      {
         var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
         var url = urlHelper.Action(actionName, controllerName,  routeValues);

         // Create link
         var linkTagBuilder = new TagBuilder("a");
         linkTagBuilder.MergeAttribute("href", url);
         linkTagBuilder.MergeAttributes(new RouteValueDictionary(linkHtmlAttributes));

         // Create image
         var imageTagBuilder = new TagBuilder("img");
         imageTagBuilder.MergeAttribute("src", urlHelper.Content(imageUrl));
         imageTagBuilder.MergeAttribute("alt", urlHelper.Encode(alternateText));
         imageTagBuilder.MergeAttributes(new RouteValueDictionary(imageHtmlAttributes));

         // Add image to link
         linkTagBuilder.InnerHtml = imageTagBuilder.ToString(TagRenderMode.SelfClosing);

         return linkTagBuilder.ToString();
      }

      #endregion

   }
}