using System;

namespace Arashi.Web.Mvc.Models
{
   [Serializable]
   public class MessageModel
   {
      private string uiState;
      private MessageIcon icon;

      public enum MessageIcon
      {
         Alert,
         Info
      }

      /// <summary>
      /// The icon type to display
      /// </summary>
      public MessageIcon Icon {
         get
         {
            return icon;
         }
         set
         {
            icon = value;

            if (icon == MessageIcon.Alert)
               uiState = "error";
            else
               uiState = "highlight";
         }
      }

      public string UIState
      {
         get
         {
            return uiState;
         }
      }

      /// <summary>
      /// The text of the message
      /// </summary>
      public string Text { get; set; }

      /// <summary>
      /// The uri of an optional link to display into the message in a {0} placeholder.
      /// </summary>
      public string ActionUri { get; set; }

      /// <summary>
      /// The text of the optional link. If missing, will use the default "link" text.
      /// </summary>
      public string ActionText { get; set; }

      public string CssClass { get; set; }

      public bool IsClosable { get; set; }


      /// <summary>
      /// This property is used on the View to get the full text to be rendered inside the message html
      /// </summary>
      public string FullText
      {
         get
         {
            // if an actionuri is specified...
            if (!string.IsNullOrEmpty(ActionUri))
            {
               if (string.IsNullOrEmpty(ActionText))
                  ActionText = "link";

               // create the link
               string anchor = string.Format("<a href=\"{0}\">{1}</a>", ActionUri, ActionText);

               if (Text.IndexOf(@"{0}") > -1)
               {
                  return string.Format(Text, anchor);
               }
               else
               {
                  return string.Concat(Text, "&nbsp;", anchor);
               }
            }
            else
            {
               return Text;
            }
         }
      }

   }
}
