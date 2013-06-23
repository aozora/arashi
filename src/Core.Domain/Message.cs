using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arashi.Core.Domain
{
   /// <summary>
   /// Status of a message
   /// </summary>
   public enum MessageStatus
   {
      Queued = 0,
      Sending = 1,
      Sent = 2,
      NotSent = 3 // this is set after the 3rd attempt failed
   }



   /// <summary>
   /// Type of message, used to distinguish each other
   /// </summary>
   public enum MessageType
   {
      Email = 0,     // Email messages to send to
      Contact = 1   // External messages received as contact request (typical from a contact form page)
   }


   /// <summary>
   /// A notification message
   /// </summary>
   public class Message
   {
      public virtual int MessageId {get; set;}
      public virtual Site Site {get; set;}
      public virtual string From {get; set;}
      public virtual string To {get; set;}
      public virtual string Cc {get; set;}
      public virtual string Bcc {get; set;}
      public virtual string Subject {get; set;}
      public virtual string Body {get; set;}
      public virtual MessageStatus Status {get; set;}
      public virtual MessageType Type {get; set;}
      public virtual int AttemptsCount {get; set;}
      public virtual DateTime CreatedDate {get; set;}
      public virtual DateTime? UpdatedDate {get; set;}



      public Message()
      {
         MessageId = -1;
         CreatedDate = DateTime.Now.ToUniversalTime();
         Status = MessageStatus.Queued;
         AttemptsCount = 0;
      }



      #region Equality

      public override bool Equals(object other)
      {
         if (this == other)
            return true;
         Message m = other as Message;
         if (m == null)
            return false;
         if (MessageId != m.MessageId)
            return false;
         return true;
      }



      public override int GetHashCode()
      {
         unchecked
         {
            int result = this.MessageId.GetHashCode();
            result = 29 * result;
            return result;
         }
      }

      #endregion

   }
}
