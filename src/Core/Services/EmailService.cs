using System;
using System.ComponentModel;
using System.Configuration;
using System.Net.Mail;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using log4net;
using Arashi.Core.Extensions;

namespace Arashi.Core.Services
{
   /// <summary>
   /// This service allow to dispatch email messages
   /// </summary>
   public class EmailService
   {
      #region Private Fields

      private static readonly ILog log = LogManager.GetLogger(typeof(EmailService));
      private string host;
      private SmtpClient smtpClient;
      private bool configured;
      private NetworkCredential credentials = new NetworkCredential();

      #endregion

      #region Constructor

      public EmailService()
      {
         this.host = ConfigurationManager.AppSettings["Smtp_Host"];
         smtpClient = new SmtpClient(host);
      }

      #endregion

      #region ISender Members

      /// <summary>
      /// Sends a message
      /// </summary>
      /// <exception cref="ArgumentNullException">If the message is null</exception>
      /// <param name="message">MailMessage instance</param>
      /// <param name="sendAsync"></param>
      public void Send(MailMessage message, bool sendAsync)
      {
         if (message == null)
            throw new ArgumentNullException("message");

         ConfigureSender();

         try
         {
            if (sendAsync)
            {
               smtpClient.SendCompleted += new SendCompletedEventHandler(SmtpClient_OnCompleted);
               smtpClient.SendAsync(message, message);
            }
            else
            {
               smtpClient.Send(message);
            }
            log.InfoFormat("EmailService.Send: message sent to {0}", message.To.ToString());
         }
         catch (Exception ex)
         {
            log.Error(ex.ToString());
            throw;
         }
      }


      private void SmtpClient_OnCompleted(object sender, AsyncCompletedEventArgs e)
      {
         //Get the Original MailMessage object
         MailMessage mail = (MailMessage)e.UserState;

         //write out the subject
         string subject = mail.Subject;

         if (e.Cancelled)
            log.InfoFormat("Send canceled for mail with subject [{0}].", subject);

         if (e.Error != null)
            log.ErrorFormat("Error {1} occurred when sending mail [{0}] ", subject, e.Error.ToString());
         else
            log.InfoFormat("Message [{0}] sent.", subject);
      }


      #endregion

      #region Helpers

      public static MailMessage CreateMailMessage(string from, string[] to, string[] cc, string[] bcc, string subject, string body, Encoding encoding, bool isBodyInHtmlFormat)
      {
         MailMessage mailMessage = new MailMessage();
         mailMessage.From = new MailAddress(from);

         foreach (string recipient in to)
            mailMessage.To.Add(recipient);


         if (cc != null && cc.Length > 0)
            foreach (string recipient in cc)
               mailMessage.CC.Add(recipient);

         if (bcc != null && bcc.Length > 0)
            foreach (string recipient in bcc)
               mailMessage.Bcc.Add(recipient);

         mailMessage.Subject = subject;
         mailMessage.Body = body;
         mailMessage.BodyEncoding = encoding;
         mailMessage.IsBodyHtml = isBodyInHtmlFormat;
         mailMessage.Priority = MailPriority.Normal;

         //foreach (DictionaryEntry entry in message.Headers)
         //{
         //   mailMessage.Headers.Add((string)entry.Key, (string)entry.Value);
         //}

         //foreach (MessageAttachment attachment in message.Attachments)
         //{
         //   Attachment mailAttach;

         //   Stream stream = new MemoryStream(attachment.FileData);
         //   mailAttach = new Attachment(stream, attachment.FileName);

         //   mailMessage.Attachments.Add(mailAttach);
         //}

         return mailMessage;
      }



      /// <summary>
      /// Configures the message or the sender
      /// with port information and eventual credential
      /// informed
      /// </summary>
      private void ConfigureSender()
      {
         if (configured) 
            return;

         credentials.Domain = ConfigurationManager.AppSettings["Smtp_Domain"];
         credentials.UserName = ConfigurationManager.AppSettings["Smtp_UserName"].DecodeFromBase64();
         credentials.Password = ConfigurationManager.AppSettings["Smtp_Password"].DecodeFromBase64();

         if (!string.IsNullOrEmpty(credentials.UserName))
            smtpClient.Credentials = credentials;

         if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["Smtp_Port"]))
            smtpClient.Port = Convert.ToInt32(ConfigurationManager.AppSettings["Smtp_Port"]);

         smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
         smtpClient.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["Smtp_RequireSSL"]);
   
         configured = true;
      }



      ///// <summary>
      ///// Memorizza nello storage (se configurato) l'eccezione avvenuta nell'invio
      ///// </summary>
      ///// <param name="ex"></param>
      //private void ManageSenderError(Exception ex, Message message)
      //{
      //   IMessageStore store = IoC.Resolve<IMessageStore>();

      //   NotificationError error = new NotificationError();
      //   error.ErrorDate = DateTime.Now;
      //   error.ErrorMessage = ex.Message;
      //   error.SenderName = "EmailSender";
         
      //   if (message != null)
      //      error.Message = message;

      //   store.SaveNotificationError(error);

      //   // Se c'è un message allora aggiorna il ShipmentAttempts
      //   if (message != null)
      //   {
      //      if (!message.ShipmentAttemptsCount.HasValue)
      //         message.ShipmentAttemptsCount = 1;
      //      else
      //         message.ShipmentAttemptsCount = message.ShipmentAttemptsCount.Value + 1;

      //      // Aggiorna il messaggio
      //      store.Save(message);
      //   }

      //}

      #endregion
   }
}