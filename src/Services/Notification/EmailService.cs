using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using Arashi.Core;
using Arashi.Core.Domain;
using Arashi.Services.SystemService;
using log4net;

namespace Arashi.Services.Notification
{
   /// <summary>
   /// This service allow to dispatch email messages
   /// </summary>
   public class EmailService
   {
      #region Private Fields

      private static readonly ILog log = LogManager.GetLogger(typeof(EmailService));
      private SystemConfiguration systemConfiguration;
      private string host;
      private SmtpClient smtpClient;
      private bool configured;
      private NetworkCredential credentials = new NetworkCredential();

      #endregion

      #region Constructor

      public EmailService()
      {
         ISystemConfigurationService service = IoC.Resolve<ISystemConfigurationService>();
         systemConfiguration = service.Get();

         this.host = systemConfiguration.SmtpHost;
         smtpClient = new SmtpClient(host);
      }

      #endregion

      #region ISender Members


      public void Send(Message message)
      {
         // convert the Message entity to e System.Net.Mail.MailMessage
         bool isHtml = message.Body.IndexOf("<body>") > -1;

         MailMessage email = CreateMailMessage(message.From, message.To.Split(';'), null, null, message.Subject, message.Body, Encoding.UTF8, true);

         Send(email);
      }



      /// <summary>
      /// Sends a message
      /// </summary>
      /// <param name="message"><see cref="MailMessage"/> instance</param>
      public void Send(MailMessage message /*, bool sendAsync*/)
      {
         if (message == null)
            throw new ArgumentNullException("message");

         ConfigureSender();

         try
         {
            //// Send the email with the first free thread from thread queue
            //var name = Thread.CurrentThread.ManagedThreadId;
            //ThreadPool.QueueUserWorkItem(ThreadSend, message);

            smtpClient.Send(message);
            log.InfoFormat("EmailService.Send: message queued for sent [message.to = {0}]", message.To.ToString());
         }
         catch (SmtpException ex)
         {
            log.ErrorFormat("Smtp Domain: {0}\r\n\tPort: {1}\r\n\tRequireSSL: {2}\r\n\tUserName: {3}\r\n\tPassword: {4}", 
                              systemConfiguration.SmtpDomain, 
                              systemConfiguration.SmtpHost, 
                              systemConfiguration.SmtpHostPort, 
                              systemConfiguration.SmtpRequireSSL, 
                              systemConfiguration.SmtpUserName, 
                              systemConfiguration.SmtpUserPassword);
            log.Error(ex.ToString());
         }
         catch (Exception ex)
         {
            log.Error(ex.ToString());
            throw;
         }
      }



      /// <summary>
      /// Send the message
      /// </summary>
      /// <param name="message"></param>
      private void ThreadSend(object message)
      {
         MailMessage mailMessage = message as MailMessage;

         if (mailMessage == null)
            throw new ArgumentNullException("message", "EmailService.ThreadSend: message cannot be null!");

         smtpClient.Send(mailMessage);
         log.InfoFormat("EmailService.ThreadSend: message sent to {0}", mailMessage.To.ToString());
      }


      //private void SmtpClient_OnCompleted(object sender, AsyncCompletedEventArgs e)
      //{
      //   //Get the Original MailMessage object
      //   MailMessage mail = (MailMessage)e.UserState;

      //   //write out the subject
      //   string subject = mail.Subject;

      //   if (e.Cancelled)
      //      log.InfoFormat("Send canceled for mail with subject [{0}].", subject);

      //   if (e.Error != null)
      //      log.ErrorFormat("Error {1} occurred when sending mail [{0}] ", subject, e.Error.ToString());
      //   else
      //      log.InfoFormat("Message [{0}] sent.", subject);
      //}


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

         credentials.Domain = systemConfiguration.SmtpDomain;
         credentials.UserName = systemConfiguration.SmtpUserName;
         credentials.Password = systemConfiguration.SmtpUserPassword;

         if (!string.IsNullOrEmpty(credentials.UserName))
            smtpClient.Credentials = credentials;

         //if (!string.IsNullOrEmpty(systemConfiguration.SmtpHostPort))
         smtpClient.Port = systemConfiguration.SmtpHostPort;
         smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
         smtpClient.EnableSsl = systemConfiguration.SmtpRequireSSL;
   
         configured = true;
      }

      #endregion
   }
}