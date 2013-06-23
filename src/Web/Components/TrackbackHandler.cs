using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using Arashi.Core;
using Arashi.Core.Domain;
using Arashi.Services.Content;
using Arashi.Services.SiteStructure;
using Arashi.Web.Helpers;
using log4net;

namespace Arashi.Web.Components
{
   public partial class TrackbackHandler : IHttpHandler
   {
      #region Private fields

      private static readonly ILog log = LogManager.GetLogger(typeof(TrackbackHandler));
      private bool sourceHasLink;

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
            log.ErrorFormat("TrackbackHandler Exception: cannot retrieve current site [Request.Uri: {0}]", context.Request.Url.ToString());
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

         string postId = context.Request.Params["id"];
         string title = context.Request.Form["title"];
         string excerpt = context.Request.Form["excerpt"];
         string blogName = context.Request.Form["blog_name"];
         string url = string.Empty;

         log.DebugFormat("TrackbackHandler: postId[{0}] title[{1}] excerpt[{2}] blogName[{3}] url[{4}]", postId, title, excerpt, blogName, url);

         if (!string.IsNullOrEmpty(context.Request.Params["url"]))
            url = context.Request.Params["url"].Split(',')[0];

         Post post = null;

         if (!string.IsNullOrEmpty(title) &&
            !string.IsNullOrEmpty(postId) &&
            !string.IsNullOrEmpty(blogName) &&
            postId.Length > 2)
         {
            //TraceService trackBackService = new TraceService();
            //TrackBackRequestDTO trackBackRequest = null;
            Comment comment = null;
            bool isFirstPing = false;

            try
            {
               //post = new PostService().GetByID(postId.ToInt32());
               post = contentItemService.GetById(Convert.ToInt32(postId));

               string partialUrl = post.GetContentUrl();
               string targetUrl = string.Concat(context.Request.Url.GetLeftPart(UriPartial.Authority),
                                                "/",
                                                partialUrl.StartsWith("~") || partialUrl.StartsWith("/")
                                                   ? partialUrl.Substring(1)
                                                   : partialUrl);
               // Examine the source page
               ExamineSourcePage(url, targetUrl);

               // Create a new Trackback
               comment = new Comment
               {
                  CommentText = excerpt,
                  ContentItem = post,
                  Url = url,
                  UserIp = context.Request.UserHostAddress,
                  UserAgent = context.Request.UserAgent,
                  Type = CommentType.Trackback,
                  Status = CommentStatus.Unapproved,
                  CreatedDate = DateTime.Now.ToUniversalTime()
               };

            }
            catch (Exception ex)
            {
               log.Error("TrackbackHandler ProcessRequest", ex);
            }


            if (post != null && post.AllowPings)
            {
               try
               {
                  isFirstPing = commentService.IsFirstPing(post, CommentType.Trackback, url.ToString());
                  log.DebugFormat("TrackbackHandler: isFirstPing = ", isFirstPing.ToString());
               }
               catch (Exception ex)
               {
                  log.Error("TrackbackHandler ProcessRequest", ex);
               }

               if (isFirstPing && sourceHasLink)
               {
                  commentService.SaveComment(comment);
                  log.Debug("TrackbackHandler: New Trackback created!");
                  context.Response.Write("<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><response><error>0</error></response>");
                  context.Response.End();
               }
               else if (!isFirstPing)
               {
                  context.Response.Write("<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><response><error>Trackback already registered</error></response>");
                  context.Response.End();
               }
               else if (!sourceHasLink)
               {
                  context.Response.Write("<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><response><error>The source page does not link</error></response>");
                  context.Response.End();
               }
            }
            else
            {
               log.ErrorFormat("TrackbackHandler: post is null or post.AllowPings == false [post is null = {0}, post.AllowPings = {1}]", (post == null).ToString(), post.AllowPings.ToString());
               context.Response.Write("<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><response><error>The source page does not link</error></response>");
               context.Response.End();
            }

         }
         else
         {
            log.Debug("TrackbackHandler: some parameters are null, exit.");
            context.Response.Redirect(context.Request.Url.GetLeftPart(UriPartial.Authority));
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
      /// Examine a given page to determine if it contains the source url
      /// </summary>
      /// <param name="sourceUrl"></param>
      /// <param name="targetUrl"></param>
      private void ExamineSourcePage(string sourceUrl, string targetUrl)
      {
         log.Debug("TrackbackHandler.ExamineSourcePage: Start");

         log.DebugFormat("TrackbackHandler.ExamineSourcePage: sourceUrl = {0}", sourceUrl);
         log.DebugFormat("TrackbackHandler.ExamineSourcePage: targetUrl = {0}", targetUrl);

         try
         {
            log.DebugFormat("TrackbackHandler.ExamineSourcePage: Creating HttpWebRequest for {0}", sourceUrl);
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
                     string html = readStream.ReadToEnd();
                     log.DebugFormat("TrackbackHandler.ExamineSourcePage: response stream html:\r\n{0}", html);

                     sourceHasLink = html.ToUpperInvariant().Contains(targetUrl.ToUpperInvariant());
                     log.DebugFormat("TrackbackHandler.ExamineSourcePage: sourceHasLink = {0}", sourceHasLink.ToString());
                  }
               }
            }

            log.Debug("TrackbackHandler.ExamineSourcePage: End");


         }
         catch (WebException)
         {
            sourceHasLink = false;
         }


      }

   }
}