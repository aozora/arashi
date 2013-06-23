namespace Arashi.Web.Components
{
   using System;
   using System.IO;
   using System.Net;
   using System.Text;
   using System.Text.RegularExpressions;
   using System.Web;
   using System.Xml;
   using Arashi.Core;
   using Arashi.Core.Domain;
   using Arashi.Services.Content;
   using Arashi.Services.SiteStructure;
   using Arashi.Web.Helpers;
   using Common.Logging;



   public partial class PingbackHandler : IHttpHandler
   {
      #region Private fields

      private static ILog log = LogManager.GetCurrentClassLogger();
      private const string SUCCESS = "<methodResponse><params><param><value><string>Thanks!</string></value></param></params></methodResponse>";
      private static readonly Regex regex = new Regex(@"(?<=<title.*>)([\s\S]*)(?=</title>)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
      private string html;
      private bool sourceHasLink;
      private string title;

      #endregion

      #region IHttpHandler Members

      public void ProcessRequest(HttpContext context)
      {
         ISiteService siteService = IoC.Resolve<ISiteService>();
         IContentItemService<Post> contentItemService = IoC.Resolve<IContentItemService<Post>>();
         ICommentService commentService = IoC.Resolve<ICommentService>();

         Site currentSite = siteService.GetSiteByHostName(WebHelper.GetHostName());

         // Allow only HTTP POST for pingbacks
         if (context.Request.HttpMethod != "POST")
         {
            log.Error("PingbackHandler Exception: the request MUST be with HTTP POST method!");
            context.Response.StatusCode = 403;
            context.Response.End();
            return;
         }

         if (currentSite == null)
         {
            log.ErrorFormat("PingbackHandler Exception: cannot retrieve current site [Request.Uri: {0}]", context.Request.Url.ToString());
            context.Response.StatusCode = 500;
            context.Response.End();
            return;
         }

         if (!currentSite.AllowPings)
         {
            context.Response.StatusCode = 404;
            context.Response.End();
            return;
         }


         try
         {

            XmlDocument doc = RetrieveXmlDocument(context);
            XmlNodeList list = doc.SelectNodes("methodCall/params/param/value/string") ?? doc.SelectNodes("methodCall/params/param/value");

            if (list != null)
            {
               string sourceUrl = list[0].InnerText.Trim();
               string targetUrl = list[1].InnerText.Trim();
               log.DebugFormat("PingbackHandler: sourceUrl = ", sourceUrl);
               log.DebugFormat("PingbackHandler: targetUrl = ", targetUrl);

               // Examine the source page
               ExamineSourcePage(sourceUrl, targetUrl);

               // Check if there is a ping link
               if (!sourceHasLink)
               {
                  log.Debug("PingbackHandler: sourceHasLink = false. Exiting...");
                  context.Response.StatusCode = 404;
                  context.Response.End();
                  return;
               }

               context.Response.ContentType = "text/xml";

               Uri url = new Uri(targetUrl);

               string postUrl = url.Segments[url.Segments.Length - 1];
               string[] routeSegment = new string[4];

               // get the url witout the querystring
               string path = url.GetLeftPart(UriPartial.Path);

               // remove the authrity and split by slash
               string[] routes = path.Substring(url.GetLeftPart(UriPartial.Authority).Length).Split('/');

               // the post url is in format "/{year}/{month}/{day}/{friendlyname}/"
               if (routes.Length < 4)
               {
                  context.Response.StatusCode = 404;
                  context.Response.End();
                  return;
               }

               int index2 = 0;
               for (int index = 0; index < routes.Length; index++)
               {
                  if (!string.IsNullOrEmpty(routes[index]))
                  {
                     routeSegment[index2] = routes[index];
                     index2++;
                  }
               }

               // last check: year, month and day must be integer
               int dummy;
               for (int index = 0; index < 3; index++)
               {
                  if (!Int32.TryParse(routeSegment[index], out dummy))
                  {
                     log.DebugFormat("PingbackHandler: routeSegment[{0}] = {1} is not int32", index.ToString(), routeSegment[index]);
                     context.Response.StatusCode = 404;
                     context.Response.End();
                     return;
                  }
               }


               //PostDTO post = new PostService().GetByUrl(postUrl);
               Post post = contentItemService.GetPublishedByPublishedDateAndFriendlyName(currentSite, Convert.ToInt32(routeSegment[0]), Convert.ToInt32(routeSegment[1]), Convert.ToInt32(routeSegment[2]), routeSegment[3]);

               if (post != null && !post.AllowPings)
               {
                  log.ErrorFormat("PingbackHandler: post is null or post.AllowPings == false [post is null = {0}, post.AllowPings = {1}]", (post == null).ToString(), post.AllowPings.ToString());
                  context.Response.StatusCode = 403;
                  context.Response.End();
                  return;
               }

               if (post != null)
               {
                  bool isFirstPing = commentService.IsFirstPing(post, CommentType.Pingback, url.ToString());
                  log.ErrorFormat("PingbackHandler: isFirstPing = ", isFirstPing.ToString());

                  if (isFirstPing)
                  {
                     Comment comment = new Comment
                                          {
                                             CommentText = title,
                                             ContentItem = post,
                                             Url = sourceUrl,
                                             UserIp = context.Request.UserHostAddress,
                                             UserAgent = context.Request.UserAgent,
                                             Type = CommentType.Pingback,
                                             Status = CommentStatus.Unapproved,
                                             CreatedDate = DateTime.Now.ToUniversalTime()
                                          };

                     commentService.SaveComment(comment);

                     context.Response.Write(SUCCESS);
                     log.Debug("PingbackHandler: New Pingback created!");
                  }
                  else
                  {
                     SendError(context, 48, "The pingback has already been registered.");
                  }
               }
               else
               {
                  SendError(context, 32, "The specified target URI does not exist.");
               }
            }
         }
         catch (Exception e)
         {
            log.Error("PingbackHandler.ProcessRequest", e);
         }
      }



      public bool IsReusable
      {
         get
         {
            return false;
         }
      }

      #endregion

      /// <summary>
      /// Send a response error in xml format
      /// </summary>
      /// <param name="context"></param>
      /// <param name="code"></param>
      /// <param name="message"></param>
      private static void SendError(HttpContext context, int code, string message)
      {
         StringBuilder sb = new StringBuilder();
         sb.Append("<?xml version=\"1.0\"?>");
         sb.Append("<methodResponse>");
         sb.Append("<fault>");
         sb.Append("<value>");
         sb.Append("<struct>");
         sb.Append("<member>");
         sb.Append("<name>faultCode</name>");
         sb.AppendFormat("<value><int>{0}</int></value>", code);
         sb.Append("</member>");
         sb.Append("<member>");
         sb.Append("<name>faultString</name>");
         sb.AppendFormat("<value><string>{0}</string></value>", message);
         sb.Append("</member>");
         sb.Append("</struct>");
         sb.Append("</value>");
         sb.Append("</fault>");
         sb.Append("</methodResponse>");

         context.Response.Write(sb.ToString());
         log.DebugFormat("PingbackHandler.SendError: code = {0}, message = {1}", code.ToString(), message);
      }



      /// <summary>
      /// Parse the source URL to get the domain.
      /// It is used to fill the Author property of the comment.
      /// </summary>
      private static string GetDomain(string sourceUrl)
      {
         int start = sourceUrl.IndexOf("://") + 3;
         int stop = sourceUrl.IndexOf("/", start);
         return sourceUrl.Substring(start, stop - start).Replace("www.", string.Empty);
      }



      /// <summary>
      /// Get the xml response
      /// </summary>
      /// <param name="context"></param>
      /// <returns></returns>
      private static XmlDocument RetrieveXmlDocument(HttpContext context)
      {
         string xml = ParseRequest(context);
         if (!xml.Contains("<methodName>pingback.ping</methodName>"))
         {
            context.Response.StatusCode = 404;
            context.Response.End();
         }

         XmlDocument doc = new XmlDocument();
         doc.LoadXml(xml);
         return doc;
      }



      /// <summary>
      /// Retrieves the content of the input stream
      /// and return it as plain text.
      /// </summary>
      private static string ParseRequest(HttpContext context)
      {
         byte[] buffer = new byte[context.Request.InputStream.Length];
         context.Request.InputStream.Read(buffer, 0, buffer.Length);

         return Encoding.Default.GetString(buffer);
      }



      /// <summary>
      /// Examine a given page to determine if it contains the source url
      /// </summary>
      /// <param name="sourceUrl"></param>
      /// <param name="targetUrl"></param>
      private void ExamineSourcePage(string sourceUrl, string targetUrl)
      {
         log.Debug("PingbackHandler.ExamineSourcePage: Start");

         log.DebugFormat("PingbackHandler.ExamineSourcePage: sourceUrl = {0}", sourceUrl);
         log.DebugFormat("PingbackHandler.ExamineSourcePage: targetUrl = {0}", targetUrl);

         try
         {
            log.DebugFormat("PingbackHandler.ExamineSourcePage: Creating HttpWebRequest for {0}", sourceUrl);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sourceUrl);

            // Emulate a useragent 'cause some sites block request without useragent
            request.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.0.10) Gecko/2009042316 Firefox/3.0.10 (.NET CLR 3.5.30729)";
            request.Credentials = CredentialCache.DefaultNetworkCredentials;
            request.MaximumAutomaticRedirections = 4;
            request.MaximumResponseHeadersLength = 4;
            
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
               // Get the stream associated with the response.
               using (Stream receiveStream = response.GetResponseStream())
               {
                  // Pipes the stream to a higher level stream reader with the required encoding format. 
                  using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
                  {
                     html = readStream.ReadToEnd();
                     log.DebugFormat("PingbackHandler.ExamineSourcePage: response stream html:\r\n{0}", html);
                     
                     title = regex.Match(html).Value.Trim();
                     log.DebugFormat("PingbackHandler.ExamineSourcePage: response stream html:\r\n{0}", html);

                     sourceHasLink = html.ToUpperInvariant().Contains(targetUrl.ToUpperInvariant());
                     log.DebugFormat("PingbackHandler.ExamineSourcePage: sourceHasLink = {0}", sourceHasLink.ToString());
                  }
               }
            }
         }
         catch (WebException ex)
         {
            log.ErrorFormat("PingbackHandler.ExamineSourcePage: {0}", ex.ToString());
            sourceHasLink = false;
         }
      }
   }
}