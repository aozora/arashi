using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.Mvc;
using Arashi.Core.Domain;
using Arashi.Services.Content;
using Arashi.Services.Membership;
using Arashi.Core.Extensions;
using Arashi.Core.Util;
using Arashi.Web.Areas.Admin.Models;
using Arashi.Web.Components;
using Arashi.Web.Mvc.Controllers;
using Arashi.Web.Mvc.Filters;
using Arashi.Web.Mvc.Models;
using Arashi.Web.Mvc.Paging;
using log4net;
using uNhAddIns.Pagination;

namespace Arashi.Web.Areas.Admin.Controllers
{
   /// <summary>
   /// Manage the creation and edit of posts
   /// </summary>
   public class AdminPostController : SecureControllerBase
   {
      #region Private Fields

      private static readonly ILog log = LogManager.GetLogger(typeof(AdminPostController));
      private const int pageSize = 20;
    	private readonly IContentItemService<Post> contentItemService;
    	private readonly ICategoryService categoryService;
      private readonly ITagService tagService;

      /// <summary>
      /// Regex used to find all hyperlinks
      /// </summary>
      private static readonly Regex urlsRegex = new System.Text.RegularExpressions.Regex(@"<a.*?href=[""'](?<url>.*?)[""'].*?>(?<name>.*?)</a>", RegexOptions.IgnoreCase | RegexOptions.Compiled);
      
      /// <summary>
      /// Regex used to capture all attributes of matched hyperlinks
      /// </summary>
      private static readonly Regex urlsAttributesRegex = new System.Text.RegularExpressions.Regex(@"(?s)(?<=<a[^>]+?)(?<name>\w+)=(?:[""']?(?<value>[^""'>]*)[""']?)(?=.+?>)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

      /// <summary>
      /// Regex used to find the trackback link on a remote web page.
      /// </summary>
      private static readonly Regex trackbackLinkRegex = new Regex("trackback:ping=\"([^\"]+)\"", RegexOptions.IgnoreCase | RegexOptions.Compiled);

      #endregion

      #region Constructor

      public AdminPostController(IContentItemService<Post> contentItemService,
                                 ICategoryService categoryService,
                                 ITagService tagService)
      {
         this.contentItemService = contentItemService;
         this.categoryService = categoryService;
         this.tagService = tagService;
      }

      #endregion

      #region Index
      /// <summary>
      /// Show the post list view
      /// </summary>
      /// <param name="page"></param>
      /// <param name="status"></param>
      /// <returns></returns>
      [PermissionFilter(RequiredRights = Rights.PostsView)]
      public ActionResult Index(int? page, string status)
      {
         IList<Post> posts = null;
         Paginator<Post> paginator;
         IPagedList<Post> pagedList = null;

         int currentPageIndex = 1;
         if (page.HasValue)
            currentPageIndex = page.Value;

         if (string.IsNullOrEmpty(status))
            paginator = contentItemService.GetPaginatorBySite(Context.ManagedSite, pageSize);
         else
            paginator = contentItemService.GetPaginatorBySiteAndWorkflowStatus(Context.ManagedSite, (WorkflowStatus)Enum.Parse(typeof(WorkflowStatus), status), pageSize);


         if (paginator.HasPages)
         {
            if (currentPageIndex > paginator.LastPageNumber)
               currentPageIndex = 1;

            posts = paginator.GetPage(currentPageIndex);

            pagedList = new PagedList<Post>(posts, page.HasValue ? page.Value - 1 : 0, pageSize, paginator.RowsCount.Value);// , totalcount
         }

         ViewData["WorkflowStatusDictionary"] = GetLocalizedEnumList(typeof(WorkflowStatus));
         ViewData["WorkflowStatus_Current"] = status;

         return View("Index", pagedList);
      }

      #endregion 

      #region New Post

      /// <summary>
      /// Show the new post view
      /// </summary>
      /// <returns></returns>
      [PermissionFilter(RequiredRights = Rights.PostsEdit)]
      public ActionResult NewPost()
      {
         PostModel model = new PostModel();
         
         model.Post = new Post();

         // Set defaults based on Site settings
         model.Post.AllowPings = Context.ManagedSite.AllowPings;
         model.Post.AllowComments = Context.ManagedSite.AllowComments;
         model.Post.Site = Context.ManagedSite;

         IEnumerable<Category> list = categoryService.GetAllCategoriesBySite(Context.ManagedSite);
         model.SiteCategories = list;
         model.SiteCategoriesSelectList = new SelectList(list, "Id", "Name");
         model.SiteTags = tagService.GetAllTagsBySite(Context.ManagedSite);
         ViewData["MonthsList"] = new SelectList(DateUtil.MonthNames(Thread.CurrentThread.CurrentUICulture, true), "Key", "Value", DateTime.Now.Month);

         // Get the full WorkflowStatus enum list, and remove the current page status value:
         IDictionary<String, String> statusList = GetLocalizedEnumList(typeof(WorkflowStatus));
         model.WorkflowStatus = new SelectList(statusList, "Key", "Value", "Published");
         // Trick to force the selecteditem ( see http://blog.benhartonline.com/post/2008/11/24/ASPNET-MVC-SelectList-selectedValue-Gotcha.aspx )
         ViewData["WorkflowStatus"] = "Published";


         // Trick to force the selecteditem ( see http://blog.benhartonline.com/post/2008/11/24/ASPNET-MVC-SelectList-selectedValue-Gotcha.aspx )
         ViewData["Month"] = DateTime.Now.Month;

         return View("NewPost", model);
      }



      /// <summary>
      /// Save a new post
      /// </summary>
      /// <param name="post"></param>
      /// <param name="categoryid"></param>
      /// <param name="tagid"></param>
      /// <param name="month"></param>
      /// <param name="day"></param>
      /// <param name="year"></param>
      /// <param name="hour"></param>
      /// <param name="minute"></param>
      /// <returns></returns>
      [PermissionFilter(RequiredRights = Rights.PostsEdit)]
      [AcceptVerbs(HttpVerbs.Post)]
		[ValidateInput(false)]
      [ValidateAntiForgeryToken]
      public ActionResult SaveNew([Bind(Exclude = "Id")] Post post, int[] categoryid, int[] tagid, int month, int day, int year, int hour, int minute)
      {
         try
         {
            UpdateModel(post, new[] { "Title", "Summary", "Content", "WorkflowStatus", "AllowComments", "AllowPings" });

            // set the sanitized friendlytitle
            post.FriendlyName = post.Title.Sanitize();

            post.PublishedDate = new DateTime(year, month, day, hour, minute, 0).ToUniversalTime();

            SaveOrUpdate(post, categoryid, tagid);

            return RedirectToAction("Index");
         }
         catch (Exception ex)
         {
            log.Error("AdminPostController.SaveNew", ex);

            MessageModel model = new MessageModel
            {
               Text = GlobalResource("Message_GenericError"),
               Icon = MessageModel.MessageIcon.Alert,
            };
            RegisterMessage(model);
         }

         return RedirectToAction("NewPost");
      }

      #endregion

      #region Edit Post

      /// <summary>
      /// Show the edit post view
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
      public ActionResult Edit(int id)
      {
         PostModel model = new PostModel();

         model.Post = contentItemService.GetById(id);
         IEnumerable<Category> list = categoryService.GetAllCategoriesBySite(Context.ManagedSite);

         model.SiteCategories = list;
         model.SiteCategoriesSelectList = new SelectList(list, "Id", "Name");
         model.SiteTags = tagService.GetAllTagsBySite(Context.ManagedSite);

         // Get the full WorkflowStatus enum list, and remove the current page status value:
         IDictionary<String, String> statusList = GetLocalizedEnumList(typeof(WorkflowStatus));
         model.WorkflowStatus = new SelectList(statusList, "Key", "Value");
         statusList.Remove(model.Post.WorkflowStatus.ToString());

         ViewData["MonthsList"] = new SelectList(DateUtil.MonthNames(Thread.CurrentThread.CurrentUICulture, true), "Key", "Value", model.Post.PublishedDate.HasValue ? model.Post.PublishedDate.Value.Month : DateTime.Now.Month);

         // Trick to force the selecteditem ( see http://blog.benhartonline.com/post/2008/11/24/ASPNET-MVC-SelectList-selectedValue-Gotcha.aspx )
         ViewData["Month"] = model.Post.PublishedDate.HasValue ? model.Post.PublishedDate.Value.Month : DateTime.Now.Month;

         return View("Edit2", model);
      }



      /// <summary>
      /// Save the updated post
      /// </summary>
      /// <param name="id"></param>
      /// <param name="categoryid"></param>
      /// <param name="tagid"></param>
      /// <param name="month"></param>
      /// <param name="day"></param>
      /// <param name="year"></param>
      /// <param name="hour"></param>
      /// <param name="minute"></param>
      /// <returns></returns>
      [PermissionFilter(RequiredRights = Rights.PostsEdit)]
      [AcceptVerbs(HttpVerbs.Post)]
      [ValidateInput(false)]
      [ValidateAntiForgeryToken]
      public ActionResult Update(int id, int[] categoryid, int[] tagid, int month, int day, int year, int hour, int minute, string WorkflowStatus)
      {
         Post post = contentItemService.GetById(id);

         try
         {
            UpdateModel(post, new[] { "Title", "Summary", "Content", "AllowComments", "AllowPings" });

            post.PublishedDate = new DateTime(year, month, day, hour, minute, 0);

            if (!string.IsNullOrEmpty(WorkflowStatus))
               post.WorkflowStatus = (Arashi.Core.Domain.WorkflowStatus)Enum.Parse(typeof(Arashi.Core.Domain.WorkflowStatus), WorkflowStatus);

            SaveOrUpdate(post, categoryid, tagid);

            MessageModel message = new MessageModel
            {
               Text = GlobalResource("Message_PostSaved"),
               Icon = MessageModel.MessageIcon.Info,
            };
            RegisterMessage(message, true);
         }
         catch (Exception ex)
         {
            log.Error("AdminPostController.Update", ex);

            foreach (string key in this.ModelState.Keys)
            {
               if (this.ModelState[key].Errors.Count > 0)
                  this.ModelState[key].Errors.Each().Do(error => log.Error(error.Exception.ToString()));
            }

            MessageModel message = new MessageModel
            {
               Text = GlobalResource("Message_GenericError"),
               Icon = MessageModel.MessageIcon.Alert,
            };
            RegisterMessage(message, true);
         }

         return RedirectToAction("Edit2", new {id = id} );
      }

      #endregion

      #region Save/Update Helper

      /// <summary>
      /// Common function to save a new post or update an existing one
      /// </summary>
      /// <param name="post"></param>
      /// <param name="categoryid"></param>
      /// <param name="tagid"></param>
      private void SaveOrUpdate(Post post, int[] categoryid, int[] tagid)
      {
         // TODO: content filtering!!!

         // due to the fact that the post title may be empty, 
         // in that case the permalink is equal to the postid
         if (string.IsNullOrEmpty(post.Title))
            post.Title = string.Empty;


         post.Site = Context.ManagedSite;
         post.Culture = Context.ManagedSite.DefaultCulture;
         post.PublishedBy = Context.CurrentUser;
         //post.WorkflowStatus = WorkflowStatus.Published;

         // Store the published dates in UTC
         post.PublishedDate = post.PublishedDate.Value.ToUniversalTime();
         //post.PublishedUntil = post.PublishedUntil.Value.ToUniversalTime();

         if (categoryid != null)
         {
            post.Categories.Clear();
            foreach (int id in categoryid)
            {
               post.Categories.Add(categoryService.GetById(id));
            }
         }

         if (tagid != null)
         {
            post.Tags.Clear();
            foreach (int id in tagid)
            {
               post.Tags.Add(tagService.GetById(id));
            }
         }

         contentItemService.Save(post);

         // Send the pings
         SendPings(post);


         // Show the confirmation message
         MessageModel model = new MessageModel
         {
            Text = GlobalResource("Message_PostSaved"),
            Icon = MessageModel.MessageIcon.Info,
         };

         RegisterMessage(model, true);
      }

      #endregion

      #region Delete Post

      /// <summary>
      /// Logically delete a single post
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
      [PermissionFilter(RequiredRights = Rights.PostsEdit)]
      [AcceptVerbs(HttpVerbs.Post)]
      [ValidateInput(false)]
      public ActionResult Delete(int id)
      {
         Post post = contentItemService.GetById(id);

         try
         {
            post.IsLogicallyDeleted = true;
            contentItemService.Save(post);

            MessageModel message = new MessageModel
            {
               Text = "The selected post has been deleted!",
               Icon = MessageModel.MessageIcon.Info,
            };

            return View("MessageUserControl", message);
         }
         catch (Exception ex)
         {
            log.Error("AdminPostController.Delete", ex);

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

   }
}
