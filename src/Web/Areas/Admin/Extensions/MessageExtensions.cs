namespace Arashi.Web.Areas.Admin.Extensions
{
   using System;
   using System.Collections.Generic;
   using System.Web.Mvc;
   using System.Web.Mvc.Html;
   using Arashi.Core.Extensions;
   using Arashi.Web.Mvc.Models;



   public static class MessageExtensions
   {


      /// <summary>
      /// Render a message box
      /// </summary>
      /// <param name="helper"></param>
      /// <param name="text"></param>
      /// <param name="icon"></param>
      public static void RenderMessage(this HtmlHelper helper, string text, string icon)
      {
         helper.RenderMessage(text, icon, null, null);
      }



      /// <summary>
      /// Render a message box
      /// </summary>
      /// <param name="helper"></param>
      /// <param name="text"></param>
      /// <param name="icon"></param>
      /// <param name="isClosable">Set to true if the message box should display a close link to let the user close the box</param>
      public static void RenderMessage(this HtmlHelper helper, string text, string icon, bool isClosable)
      {
         helper.RenderMessage(text, icon, null, null, isClosable);
      }


      /// <summary>
      /// Render a message box
      /// </summary>
      /// <param name="helper"></param>
      /// <param name="text"></param>
      /// <param name="icon"></param>
      /// <param name="cssClass"></param>
      public static void RenderMessage(this HtmlHelper helper, string text, string icon, string cssClass)
      {
         helper.RenderMessage(text, icon, null, null, cssClass, false);
      }



      /// <summary>
      /// Render a message box
      /// </summary>
      /// <param name="helper"></param>
      /// <param name="text"></param>
      /// <param name="icon"></param>
      /// <param name="cssClass"></param>
      /// <param name="isCLosable"></param>
      public static void RenderMessage(this HtmlHelper helper, string text, string icon, string cssClass, bool isCLosable)
      {
         helper.RenderMessage(text, icon, null, null, cssClass, isCLosable);
      }



      /// <summary>
      /// Render a message box
      /// </summary>
      /// <param name="helper"></param>
      /// <param name="text"></param>
      /// <param name="icon"></param>
      /// <param name="actionUri"></param>
      /// <param name="actionText"></param>
      public static void RenderMessage(this HtmlHelper helper,
                                       string text,
                                       string icon,
                                       string actionUri,
                                       string actionText)
      {
         helper.RenderMessage(text, icon, actionUri, actionText, string.Empty, false);
      }



      /// <summary>
      /// Render a message box
      /// </summary>
      /// <param name="helper"></param>
      /// <param name="text"></param>
      /// <param name="icon"></param>
      /// <param name="actionUri"></param>
      /// <param name="actionText"></param>
      /// <param name="isClosable">Set to true if the message box should display a close link to let the user close the box</param>
      public static void RenderMessage(this HtmlHelper helper,
                                       string text,
                                       string icon,
                                       string actionUri,
                                       string actionText,
                                       bool isClosable)
      {
         helper.RenderMessage(text, icon, actionUri, actionText, string.Empty, isClosable);
      }


      /// <summary>
      /// Render a message box
      /// </summary>
      /// <param name="helper"></param>
      /// <param name="text"></param>
      /// <param name="icon"></param>
      /// <param name="actionUri"></param>
      /// <param name="actionText"></param>
      /// <param name="cssClass"></param>
      /// <param name="isClosable">Set to true if the message box should display a close link to let the user close the box</param>
      public static void RenderMessage(this HtmlHelper helper, 
         string text,
         string icon, 
         string actionUri, 
         string actionText,
         string cssClass, 
         bool isClosable)
      {
         MessageModel model = new MessageModel()
         {
            Text = text,
            Icon = ((MessageModel.MessageIcon)Enum.Parse(typeof(MessageModel.MessageIcon), icon.Capitalize())),
            ActionUri = actionUri,
            ActionText = actionText,
            CssClass = cssClass,
            IsClosable = isClosable
         };

         helper.RenderPartial("MessageUserControl", model);
      }



      /// <summary>
      /// Render all the registered messages
      /// </summary>
      /// <param name="helper"></param>
      public static void RenderRegisteredMessages(this HtmlHelper helper)
      {
         if (!helper.ViewContext.ViewData.ContainsKey("RegisteredMessages") &&
             !helper.ViewContext.TempData.ContainsKey("PersistentMessages"))
            return;

         // render normal messages
         IList<MessageModel> models = helper.ViewContext.ViewData["RegisteredMessages"] as IList<MessageModel>;

         if (models != null)
         {
            foreach (MessageModel model in models)
            {
               helper.RenderPartial("MessageUserControl", model);
            }
         }

         // render persistent messages
         IList<MessageModel> persistentModels = helper.ViewContext.TempData["PersistentMessages"] as IList<MessageModel>;

         if (persistentModels != null)
         {
            foreach (MessageModel model in persistentModels)
            {
               helper.RenderPartial("MessageUserControl", model);
            }
         }

      }



   }
}
