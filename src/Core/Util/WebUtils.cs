using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using log4net;

namespace Arashi.Core.Util
{
   public class WebUtils
   {
      private static readonly ILog log = LogManager.GetLogger(typeof(WebUtils));


      /// <summary>
      /// Downloads a web page from the Internet and returns the HTML as a string. .
      /// </summary>
      /// <param name="url">The URL to download from.</param>
      /// <returns>The HTML or null if the URL isn't valid.</returns>
      public static string DownloadWebPage(Uri url)
      {
         try
         {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers["Accept-Encoding"] = "gzip";
            request.Headers["Accept-Language"] = "en-us";
            request.Credentials = CredentialCache.DefaultNetworkCredentials;
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (WebResponse response = request.GetResponse())
            {
               using (StreamReader reader = new StreamReader(response.GetResponseStream()))
               {
                  return reader.ReadToEnd();
               }
            }
         }
         catch (Exception)
         {
            log.ErrorFormat("Error trying to download the page at url: {0}", url.ToString());
            return null;
         }
      }
   }
}
