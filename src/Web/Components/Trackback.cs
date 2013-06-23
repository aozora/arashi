#region Using

using System;
using System.IO;
using System.Net;
using log4net;

#endregion

namespace Arashi.Web.Components
{
   /// <summary>
   /// Sends trackback to website that the blog links to.
   /// </summary>
   /// <remarks>
   /// This class is provided from BlogEngine.NET project
   /// </remarks>
   public static class Trackback
   {
      private static readonly ILog log = LogManager.GetLogger(typeof(Trackback));

      /// <summary>
      /// 
      /// </summary>
      /// <param name="message"></param>
      /// <returns></returns>    
      public static bool Send(TrackbackMessage message)
      {
         if (message == null)
            throw new ArgumentNullException("message");

         OnSending(message.UrlToNotifyTrackback);
         //Warning:next line if for local debugging porpuse please donot remove it until you need to
         //tMessage.PostURL = new Uri("http://www.artinsoft.com/webmaster/trackback.html");
         HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(message.UrlToNotifyTrackback); //HttpHelper.CreateRequest(trackBackItem);
         request.Credentials = CredentialCache.DefaultNetworkCredentials;
         request.Method = "POST";
         request.ContentLength = message.ToString().Length;
         request.ContentType = "application/x-www-form-urlencoded";
         request.KeepAlive = false;
         request.Timeout = 10000;

         using (StreamWriter myWriter = new StreamWriter(request.GetRequestStream()))
         {
            myWriter.Write(message.ToString());
         }

         bool result = false;
         HttpWebResponse response;

         try
         {
            log.DebugFormat("Trackback: trying to get response from trackback url {0}", message.UrlToNotifyTrackback.ToString());

            response = (HttpWebResponse)request.GetResponse();
            OnSent(message.UrlToNotifyTrackback);
            string answer;
            using (System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream()))
            {
               answer = sr.ReadToEnd();
            }

            if (response.StatusCode == HttpStatusCode.OK)
            {
               //todo:This could be a strict XML parsing if necesary/maybe logging activity here too
               if (answer.Contains("<error>0</error>"))
               {
                  log.Debug("Trackback: response OK");
                  result = true;
               }
               else
               {
                  log.Debug("Trackback: response KO!");
                  result = false;
               }
            }
            else
            {
               log.Debug("Trackback: response KO!");
               result = false;
            }
         }
         catch (WebException ex)
         {
            log.Debug(ex.ToString());
            result = false;
         }

         log.Debug("Trackback: end.");
         return result;
      }

      #region

      /// <summary>
      /// Occurs just before a trackback is sent.
      /// </summary>
      public static event EventHandler<EventArgs> Sending;
      private static void OnSending(Uri url)
      {
         if (Sending != null)
         {
            Sending(url, new EventArgs());
         }
      }

      /// <summary>
      /// Occurs when a trackback has been sent
      /// </summary>
      public static event EventHandler<EventArgs> Sent;
      private static void OnSent(Uri url)
      {
         if (Sent != null)
         {
            Sent(url, new EventArgs());
         }
      }

      #endregion

   }

}