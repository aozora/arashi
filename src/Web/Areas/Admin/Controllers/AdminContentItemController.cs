namespace Arashi.Web.Areas.Admin.Controllers
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Text.RegularExpressions;
   using System.Web.Mvc;

   using Arashi.Core.Domain;
   using Arashi.Core.Extensions;
   using Arashi.Core.Util;
   using Arashi.Services.Content;
   using Arashi.Services.Membership;
   using Arashi.Web.Components;
   using Arashi.Web.Mvc.Controllers;
   using Arashi.Web.Mvc.Filters;
   using Arashi.Web.Mvc.Models;

   using log4net;

   /// <summary>
   /// This is a common controller for all actions on a generic <see cref="ContentItem"/>
   /// </summary>
   public class AdminContentItemController : SecureControllerBase
   {
      #region Private Fields

      private static readonly ILog log = LogManager.GetLogger(typeof(AdminContentItemController));
    	private readonly IContentItemService<ContentItem> contentItemService;
    	private readonly ICategoryService categoryService;
      private readonly ITagService tagService;

      /// <summary>
      /// Regex used to find all hyper links
      /// </summary>
      private static readonly Regex urlsRegex = new System.Text.RegularExpressions.Regex(@"<a.*?href=[""'](?<url>.*?)[""'].*?>(?<name>.*?)</a>", RegexOptions.IgnoreCase | RegexOptions.Compiled);
      
      /// <summary>
      /// Regex used to capture all attributes of matched hyper links
      /// </summary>
      private static readonly Regex urlsAttributesRegex = new System.Text.RegularExpressions.Regex(@"(?s)(?<=<a[^>]+?)(?<name>\w+)=(?:[""']?(?<value>[^""'>]*)[""']?)(?=.+?>)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

      /// <summary>
      /// Regex used to find the trackback link on a remote web page.
      /// </summary>
      private static readonly Regex trackbackLinkRegex = new Regex("trackback:ping=\"([^\"]+)\"", RegexOptions.IgnoreCase | RegexOptions.Compiled);

      #endregion

      #region Constructor

      /// <summary>
      /// Constructor
      /// </summary>
      /// <param name="contentItemService"></param>
      /// <param name="categoryService"></param>
      /// <param name="tagService"></param>
      public AdminContentItemController(IContentItemService<ContentItem> contentItemService,
                                 ICategoryService categoryService,
                                 ITagService tagService)
      {
         this.contentItemService = contentItemService;
         this.categoryService = categoryService;
         this.tagService = tagService;
      }

      #endregion

      #region Permalink Edit

      /// <summary>
      /// Save the updated permalink for a given <see cref="ContentItem"/>
      /// </summary>
      /// <param name="permalink"></param>
      /// <param name="id"></param>
      /// <returns></returns>
      [AcceptVerbs(HttpVerbs.Post)]
      [PermissionFilter(RequiredRights = Rights.PostsEdit)]
      public ActionResult UpdatePermalink(string permalink, int id)
      {
         try
         {
            // due to the fact that the post title may be empty, 
            // in that case the permalink is equal to the postid
            if (string.IsNullOrEmpty(permalink))
               permalink = id.ToString();

            ContentItem post = contentItemService.GetById(id);
            post.FriendlyName = permalink.Sanitize();

            contentItemService.Save(post);

            // Show the confirmation message
            MessageModel message = new MessageModel
            {
               Text = "Permalink updated!",
               Icon = MessageModel.MessageIcon.Info,
            };

            return View("MessageUserControl", message);
         }
         catch (Exception ex)
         {
            log.Error("AdminContentItemController.UpdatePermalink", ex);

            MessageModel message = new MessageModel
            {
               Text = GlobalResource("Message_GenericError"),
               Icon = MessageModel.MessageIcon.Alert,
            };

            return View("MessageUserControl", message);
         }
      }

      #endregion

      #region PingBack & TrackBack

      /// <summary>
      /// Try to send trackback and pingback for each link in the content
      /// </summary>
      /// <param name="post"></param>
      private void SendPings(Post post)
      {
         if (post.AllowPings)
         {

            string partialUrl = post.GetContentUrl();
            Uri sourceUrl = new Uri(string.Concat(Request.Url.GetLeftPart(UriPartial.Authority),
                                                  "/",
                                                  partialUrl.StartsWith("~") || partialUrl.StartsWith("/")
                                                     ? partialUrl.Substring(1)
                                                     : partialUrl)
               );

            // cycle for each links in the post
            foreach (Uri url in GetUrlsFromContent(post.Content))
            {
               bool isTrackbackSent = false;

               // Get the remote web page
               string pageContent = WebUtils.DownloadWebPage(url);

               if (string.IsNullOrEmpty(pageContent) || pageContent == "\r\n")
                  continue;

               // try to get the remote trackback url
               Uri trackbackUrl = GetTrackBackUrlFromPage(pageContent);

               log.DebugFormat("AdminPostController.SendPings: TrackBackUrl = {0}", trackbackUrl.ToString());

               // try to send a trackback...
               if (trackbackUrl != null)
               {
                  TrackbackMessage message = new TrackbackMessage(post, trackbackUrl, sourceUrl);
                  isTrackbackSent = Trackback.Send(message);
               }

               // Send a pingback only if the trackback was not sended (or unsuccessfull)
               if (!isTrackbackSent)
                  Pingback.Send(sourceUrl, url);
            }

         }
      }



      /// <summary>
      /// Gets all the URLs from the specified string.
      /// </summary>
      private static IList<Uri> GetUrlsFromContent(string content)
      {
         IList<Uri> urlsList = new List<Uri>();

         foreach (Match myMatch in urlsRegex.Matches(content))
         {
            string url = myMatch.Groups["url"].ToString().Trim();

            // exclude media resources...
            if (url.ToLower().EndsWith(".jpg") ||
               url.ToLower().EndsWith(".jpeg") ||
               url.ToLower().EndsWith(".gif") ||
               url.ToLower().EndsWith(".png") ||
               url.ToLower().EndsWith(".swf") ||
               url.ToLower().EndsWith(".flv"))
               continue;

            Uri uri;
            if (Uri.TryCreate(url, UriKind.Absolute, out uri))
               urlsList.Add(uri);
         }

         return urlsList;
      }



      /// <summary>
      /// Examines the web page source code to retrieve the trackback link from the RDF.
      /// </summary>
      private static Uri GetTrackBackUrlFromPage(string input)
      {
         // try to discover the ping url in the RDF eventually included in the page
         // http://www.sixapart.com/pronet/docs/trackback_spec
         string url = trackbackLinkRegex.Match(input).Groups[1].ToString().Trim();
         Uri uri;

         if (Uri.TryCreate(url, UriKind.Absolute, out uri))
            return uri;

         // Try to find a link sith rel="trackback"
         foreach (Match myMatch in urlsRegex.Matches(input))
         {
            if (myMatch.ToString().IndexOf("rel=\"trackback\"") > -1 ||
                myMatch.ToString().IndexOf("rel='trackback'") > -1)
            {
               url = myMatch.Groups["url"].ToString().Trim();
               if (Uri.TryCreate(url, UriKind.Absolute, out uri))
                  return uri;
            }
         }

         return null;
      }

      #endregion

      #region Edit Custom Fields

      /// <summary>
      /// Add a new custom field
      /// </summary>
      /// <param name="contentitemid"></param>
      /// <param name="key"></param>
      /// <param name="value"></param>
      /// <returns></returns>
      [AcceptVerbs(HttpVerbs.Post)]
      [PermissionFilter(RequiredRights = Rights.PostsEdit)]
      public ActionResult SaveCustomField(string contentitemid, string key, string value)
      {
         try
         {
            ContentItem contentItem = contentItemService.GetById(Convert.ToInt32(contentitemid));
            
            // first check if the same key already exists
            if (contentItem.CustomFields.ContainsKey(key))
            {
               MessageModel duplicateFieldMessage = new MessageModel
               {
                  Text = "There is another custom field with the same key!",
                  Icon = MessageModel.MessageIcon.Alert,
                  CssClass = "margin-topbottom"
               };

               return View("MessageUserControl", duplicateFieldMessage);
            }

            contentItem.CustomFields.Add(key, value);
            contentItemService.Save(contentItem);

            // Show the confirmation message
            MessageModel message = new MessageModel
            {
               Text = GlobalResource("Message_CustomFieldSaved"),
               Icon = MessageModel.MessageIcon.Info,
               CssClass = "margin-topbottom"
            };

            return View("MessageUserControl", message);
         }
         catch (Exception ex)
         {
            log.Error("AdminContentItemController.SaveCustomField", ex);

            MessageModel message = new MessageModel
            {
               Text = GlobalResource("Message_GenericError"),
               Icon = MessageModel.MessageIcon.Alert,
               CssClass = "margin-topbottom"
            };

            return View("MessageUserControl", message);
         }

      }



      /// <summary>
      /// Get the list of custom fields for the given contentitem in JSON format
      /// </summary>
      /// <param name="contentItemId"></param>
      /// <returns></returns>
      [PermissionFilter(RequiredRights = Rights.PostsView)]
      public ActionResult GetCustomFields(int contentItemId)
      {
         IDictionary<string, string> customFields = contentItemService.GetById(contentItemId).CustomFields;

         var jsonData = new
         {
            total = customFields.Count,
            page = 0,
            records = customFields.Count,
            rows = (from customField in customFields
                    select new
                    {
                       i = customField.Key,
                       cell = new string[] { customField.Key, customField.Value }
                    }
                    ).ToArray()
         };

         return Json(jsonData);
      }




      /// <summary>
      /// Update a single custom field (jqGrid)
      /// </summary>
      /// <param name="contentItemId">
      /// </param>
      /// <param name="oper">
      /// jqGrid edit operation: "edit" or "del"
      /// </param>
      /// <param name="id">
      /// The id.
      /// </param>
      /// <param name="value">
      /// </param>
      /// <returns>
      /// </returns>
      [AcceptVerbs(HttpVerbs.Post)]
      [PermissionFilter(RequiredRights = Rights.PostsEdit)]
      public ActionResult UpdateCustomFields(int contentItemId, string oper, string id, string value)
      {

         try
         {
            ContentItem contentItem = contentItemService.GetById(contentItemId);

            if (oper == "del")
            {
               contentItem.CustomFields.Remove(id);
            }
            else
            {
               // edit
               contentItem.CustomFields[id] = value;
            }

            contentItemService.Save(contentItem);
            log.DebugFormat("AdminContentItemController.UpdateCustomFields: Operation \"{0}\" ok!", oper);

            return Content("true");
         }
         catch (Exception ex)
         {
            log.ErrorFormat("AdminContentItemController.UpdateCustomFields: Oper = {0}, ContentItemId = {1}, Key = {2}, Value = {3}", oper, contentItemId.ToString(), id, value);
            log.ErrorFormat("AdminContentItemController.UpdateCustomFields: {0}", ex.ToString());
            Response.StatusCode = 500;
            return Content("false");
         }
      }

      #endregion

   }
}
