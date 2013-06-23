namespace Arashi.Web.Controllers
{
   using System;
   using System.Collections.Generic;
   using System.Configuration;
   using System.Net;
   using System.Net.Mail;
   using System.Text;
   using System.Web.Mvc;

   using Arashi.Core.Domain;
   using Arashi.Core.Extensions;
   using Arashi.Core.Util;
   using Arashi.Services.Content;
   using Arashi.Services.Localization;
   using Arashi.Services.Membership;
   using Arashi.Services.Notification;
   using Arashi.Services.Search;
   using Arashi.Services.SiteStructure;
   using Arashi.Services.Widget;
   using Arashi.Web.Mvc.Captcha;
   using Arashi.Web.Mvc.Controllers;
   using Arashi.Web.Mvc.Filters;
   using Arashi.Web.Mvc.Models;
   using Arashi.Web.Mvc.Paging;
   using Arashi.Web.Mvc.TemplateEngine;

   using Common.Logging;

   using uNhAddIns.Pagination;

   /// <summary>
   /// Manage all actions for the post/s on the frontend
   /// </summary>
   [SeoUrlCanonicalization]
   public class PostController : ContentControllerBase
   {
      private ILog log;

      #region Constructor

      public PostController(ILog log, ILocalizationService localizationService,
                              ISiteService siteService,
                              IUserService userService,
                              IContentItemService<Post> contentItemService,
                              IContentItemService<Arashi.Core.Domain.Page> contentItemServiceForPage,
                              ICommentService commentService,
                              ICategoryService categoryService,
                              ITagService tagService,
                              ISearchService searchService,
                              IWidgetService widgetService,
                              IMessageService messageService)
         : base(log, localizationService, siteService, userService, contentItemService, contentItemServiceForPage, commentService, categoryService, tagService, searchService, widgetService, messageService)
      {
         this.log = log;
      }

      #endregion

      /// <summary>
      /// Show all the recent post (default home)
      /// </summary>
      /// <param name="page"></param>
      /// <returns></returns>
      public ActionResult Index(int? page)
      {
         IList<Post> posts = null;
         Paginator<Post> paginator;
         IPagedList<Post> pagedList = null;

         int currentPageIndex = 1;
         if (page.HasValue)
            currentPageIndex = page.Value;

         paginator = contentItemService.GetPaginatorForPublishedBySiteAndWorkflowStatus(Context.CurrentSite, WorkflowStatus.Published, Context.CurrentSite.MaxPostsPerPage);

         if (paginator.HasPages)
         {
            if (currentPageIndex > paginator.LastPageNumber)
               currentPageIndex = 1;

            posts = paginator.GetPage(currentPageIndex);

            pagedList = new PagedList<Post>(posts, page.HasValue ? page.Value - 1 : 0, Context.CurrentSite.MaxPostsPerPage, paginator.RowsCount.Value);// , totalcount
         }

         TemplateContentModel model = GetDefaultTemplateContentModel();
         model.Posts = pagedList;
         model.TemplateFile = ViewHelper.TemplateFile.index;
         model.ViewMode = ViewHelper.ViewMode.is_post;

         return ViewContent(model);
      }



      /// <summary>
      /// Show a single page
      /// </summary>
      /// <param name="name"></param>
      /// <returns></returns>
      public ActionResult Page(string name)
      {
         Page page = contentItemServiceForPage.GetPublishedByFriendlyName(Context.CurrentSite, name);

         // if I can't find a post, show a 404 page!!!
         if (page == null)
            return Render404();

         //// if the post allow ping add the X-Pingback header to the response
         // if (page.AllowPings)
         //   this.Response.Headers.Add("X-Pingback", Request.Url.GetLeftPart(UriPartial.Authority) + "/pingback.axd");

         TemplateContentModel model = GetDefaultTemplateContentModel();

         // model.Posts = new PagedList<Page>(new List<Page>() { page }, 0, 1);
         model.CurrentPage = page;
         model.TemplateFile = ViewHelper.TemplateFile.page;
         // model.ViewMode = ViewHelper.ViewMode.is_post;

         return ViewContent(model);
      }



      /// <summary>
      /// Show a single post
      /// </summary>
      /// <param name="year"></param>
      /// <param name="month"></param>
      /// <param name="day"></param>
      /// <param name="name"></param>
      /// <returns></returns>
      public ActionResult Single(string year, string month, string day, string name)
      {
         Post post = contentItemService.GetPublishedByPublishedDateAndFriendlyName(Context.CurrentSite, Convert.ToInt32(year), Convert.ToInt32(month), Convert.ToInt32(day), name);

         // if I can't find a post, show a 404 page!!!
         if (post == null)
            return Render404();

         // if the post allow ping add the X-Pingback header to the response
         if (post.AllowPings)
            this.Response.Headers.Add("X-Pingback", Request.Url.GetLeftPart(UriPartial.Authority) + "/pingback.axd");

         TemplateContentModel model = GetDefaultTemplateContentModel();

         model.Posts = new PagedList<Post>(new List<Post>(){post}, 0, 1);
         model.TemplateFile = ViewHelper.TemplateFile.single;
         model.ViewMode = ViewHelper.ViewMode.is_post;

         return ViewContent(model);
      }



      /// <summary>
      /// Show the archive page for a specific category
      /// </summary>
      /// <param name="name"></param>
      /// <param name="page"></param>
      /// <returns></returns>
      public ActionResult Category(string name, int? page)
      {
         IList<Post> posts = null;
         Paginator<Post> paginator;
         IPagedList<Post> pagedList = null;

         // if there isn't a name, return a 404 error.
         if (string.IsNullOrEmpty(name))
            return Render404();

         int currentPageIndex = 1;
         if (page.HasValue)
            currentPageIndex = page.Value;

         Category currentCategory = categoryService.GetBySiteAndFriendlyName(Context.CurrentSite, name);

         if (currentCategory == null)
            return Render404();

         paginator = contentItemService.GetPaginatorForPublishedBySiteAndCategory(Context.CurrentSite, currentCategory, Context.CurrentSite.MaxPostsPerPage);

         if (paginator.HasPages)
         {
            if (currentPageIndex > paginator.LastPageNumber)
               currentPageIndex = 1;

            posts = paginator.GetPage(currentPageIndex);

            pagedList = new PagedList<Post>(posts, page.HasValue ? page.Value - 1 : 0, Context.CurrentSite.MaxPostsPerPage, paginator.RowsCount.Value);// , totalcount
         }

         TemplateContentModel model = GetDefaultTemplateContentModel();

         model.Posts = pagedList;
         model.TemplateFile = ViewHelper.TemplateFile.archive;
         model.ViewMode = ViewHelper.ViewMode.is_category;
         model.CurrentCategory = currentCategory;

         return ViewContent(model);
      }



      /// <summary>
      /// Show the archive page for a specific tag
      /// </summary>
      /// <param name="name"></param>
      /// <param name="page"></param>
      /// <returns></returns>
      public ActionResult Tag(string name, int? page)
      {
         IList<Post> posts = null;
         Paginator<Post> paginator;
         IPagedList<Post> pagedList = null;

         // if there isn't a name, return a 404 error.
         if (string.IsNullOrEmpty(name))
            return Render404();

         int currentPageIndex = 1;
         if (page.HasValue)
            currentPageIndex = page.Value;

         Tag currentTag = tagService.GetBySiteAndFriendlyName(Context.CurrentSite, name);

         if (currentTag == null)
            return Render404();

         paginator = contentItemService.GetPaginatorForPublishedBySiteAndTag(Context.CurrentSite, currentTag, Context.CurrentSite.MaxPostsPerPage);

         if (paginator.HasPages)
         {
            if (currentPageIndex > paginator.LastPageNumber)
               currentPageIndex = 1;

            posts = paginator.GetPage(currentPageIndex);

            pagedList = new PagedList<Post>(posts, page.HasValue ? page.Value - 1 : 0, Context.CurrentSite.MaxPostsPerPage, paginator.RowsCount.Value);// , totalcount
         }

         TemplateContentModel model = GetDefaultTemplateContentModel();

         model.Posts = pagedList;
         model.TemplateFile = ViewHelper.TemplateFile.archive;
         model.ViewMode = ViewHelper.ViewMode.is_tag;
         model.CurrentTag = currentTag;

         return ViewContent(model);
      }



      public ActionResult Archive(string year, string month, string day, int? page)
      {
         IList<Post> posts = null;
         Paginator<Post> paginator;
         IPagedList<Post> pagedList = null;

         int currentPageIndex = 1;
         if (page.HasValue)
            currentPageIndex = page.Value;

         paginator = contentItemService.GetPaginatorForPublishedBySiteAndPublishedDate(Context.CurrentSite, 
                        Convert.ToInt32(year),
                        string.IsNullOrEmpty(month) ? (int?) null : Convert.ToInt32(month),
                        string.IsNullOrEmpty(day) ? (int?) null : Convert.ToInt32(day),
                        Context.CurrentSite.MaxPostsPerPage);

         if (paginator.HasPages)
         {
            if (currentPageIndex > paginator.LastPageNumber)
               currentPageIndex = 1;

            posts = paginator.GetPage(currentPageIndex);

            pagedList = new PagedList<Post>(posts, page.HasValue ? page.Value - 1 : 0, Context.CurrentSite.MaxPostsPerPage, paginator.RowsCount.Value);// , totalcount
         }

         TemplateContentModel model = GetDefaultTemplateContentModel();

         model.Posts = pagedList;
         model.TemplateFile = ViewHelper.TemplateFile.archive;

         if (!string.IsNullOrEmpty(month) && !string.IsNullOrEmpty(day))
            model.ViewMode = ViewHelper.ViewMode.is_day;
         else if (!string.IsNullOrEmpty(month) && string.IsNullOrEmpty(day))
            model.ViewMode = ViewHelper.ViewMode.is_month;
         else
            model.ViewMode = ViewHelper.ViewMode.is_year;

         return ViewContent(model);
      }



      /// <summary>
      /// Show the author info page
      /// </summary>
      /// <param name="name"></param>
      /// <param name="page"></param>
      /// <returns></returns>
      public ActionResult Author(string name, int? page)
      {
         IList<Post> posts = null;
         Paginator<Post> paginator;
         IPagedList<Post> pagedList = null;

         // if the author cannot be find, return a 404 error.
         if (string.IsNullOrEmpty(name))
            return Render404();

         int currentPageIndex = 1;
         if (page.HasValue)
            currentPageIndex = page.Value;

         User author = userService.FindUserByDisplayName(Context.CurrentSite, name);

         // if the author cannot be find, return a 404 error.
         if (author == null)
            return Render404();

         paginator = contentItemService.GetPaginatorBySiteAndWorkflowStatusAndAuthor(Context.CurrentSite, WorkflowStatus.Published, author, Context.CurrentSite.MaxPostsPerPage);

         if (paginator.HasPages)
         {
            if (currentPageIndex > paginator.LastPageNumber)
               currentPageIndex = 1;

            posts = paginator.GetPage(currentPageIndex);

            pagedList = new PagedList<Post>(posts, page.HasValue ? page.Value - 1 : 0, Context.CurrentSite.MaxPostsPerPage, paginator.RowsCount.Value);// , totalcount
         }

         TemplateContentModel model = GetDefaultTemplateContentModel();

         model.Posts = pagedList;
         model.TemplateFile = ViewHelper.TemplateFile.author;
         model.ViewMode = ViewHelper.ViewMode.is_author;
         model.CurrentAuthor = author;

         return ViewContent(model);
      }

      #region Contact us

      /// <summary>
      /// Show the contact form page
      /// </summary>
      /// <returns></returns>
      public ActionResult Contact()
      {
         TemplateContentModel model = GetDefaultTemplateContentModel();

         model.Posts = null;
         model.TemplateFile = ViewHelper.TemplateFile.contact;

         return ViewContent(model);
      }




      /// <summary>
      /// Save a contact message via ajax
      /// </summary>
      /// <param name="author"></param>
      /// <param name="email"></param>
      /// <param name="subject"></param>
      /// <param name="message"></param>
      /// <param name="recaptcha_challenge_field"></param>
      /// <param name="recaptcha_response_field"></param>
      /// <returns></returns>
      [AcceptVerbs(HttpVerbs.Post)]
      [CaptchaValidator]
      [ValidateInput(false)]
      [ValidateAntiForgeryToken(Salt = "contact")]
      public ActionResult SaveContactAjax(string author, string email, string subject, string message, string recaptcha_challenge_field, string recaptcha_response_field)
      {
         bool isValid = true;
         bool captchaValid = true;
         bool messageSent = false;
         //const string validationErrorCssClass = "wpcf7-validation-errors";
         string validationErrorMessage = ThemeResource("Contact_Message_ValidationErrors");
         string validationMessage = ThemeResource("Contact_Message_Sent");

         // if the captcha is enabled, try to validate it
         if (Context.CurrentSite.EnableCaptchaForComments && !string.IsNullOrEmpty(recaptcha_challenge_field))
         {
            try
            {
               captchaValid = CaptchaValidator.Validate(Context.CurrentSite.CaptchaPrivateKey, recaptcha_challenge_field, recaptcha_response_field);
            }
            catch (WebException ex)
            {
               // timeout for recaptcha
               log.ErrorFormat("PostController.SaveContactAjax: Recaptcha Validator has timed out!\r\n{0}", ex.ToString());
               validationMessage = validationErrorMessage;

               return Json(new
               {
                  message = validationErrorMessage,
                  message_sent = false,
                  isvalid = false
               });
            }
         }

         if (captchaValid)
         {
            // Validate parameters
            if (string.IsNullOrEmpty(author) || 
                string.IsNullOrEmpty(email) || 
                string.IsNullOrEmpty(message) ||
                !Validation.IsEmail(email))
               isValid = false;

            // send message
            if (isValid)
            {
               try
               {
                  Message contactMessage = new Message
                  {
                     Site = Context.CurrentSite,
                     To = this.Context.CurrentSite.Email,
                     From = email,
                     Subject = subject.StripHtml(),
                     Body = message.StripHtml(),
                     Type = MessageType.Contact
                  };

                  messageService.Save(contactMessage);

                  messageSent = true;
               }
               catch (Exception ex)
               {
                  log.Error(ex.ToString());
                  validationMessage = "Sorry, an unexpected error occured...";
               }

            }
            else
            {
               // foreach invalid
               validationMessage = validationErrorMessage;
            }

         }
         else
         {
            // captcha invalid
            validationMessage = validationErrorMessage;
         }


         var jsonData = new
         {
            message = validationMessage,
            message_sent = messageSent,
            isvalid = isValid
            //invalids = (from host in hosts
            //            select new
            //            {
            //               i = host.SiteHostId,
            //               cell = new string[] { host.SiteHostId.ToString(), host.HostName, host.IsDefault.ToString() }
            //            }
            //        ).ToArray()
         };

         return Json(jsonData);
      }



      /// <summary>
      /// Save a contact message
      /// </summary>
      /// <param name="author"></param>
      /// <param name="email"></param>
      /// <param name="subject"></param>
      /// <param name="message"></param>
      /// <param name="captchaValid"></param>
      /// <returns></returns>
      [AcceptVerbs(HttpVerbs.Post)]
      [CaptchaValidator]
      [ValidateInput(false)]
      [ValidateAntiForgeryToken(Salt = "contact")]
      public ActionResult SaveContact(string author, string email, string subject, string message, bool captchaValid)
      {// TODO: use a ContactModel with AllowHtmlAttribute instead of ValidateInput(false)
         bool isValid = captchaValid;
         const string validationErrorCssClass = "wpcf7-validation-errors";
         string validationErrorMessage = ThemeResource("Contact_Message_ValidationErrors");
         string validationMessage = ThemeResource("Contact_Message_Sent");

         // if the captcha is disabled, set the captcha variable to true
         if (!Context.CurrentSite.EnableCaptchaForComments)
         {
            captchaValid = true;
            log.InfoFormat("PostController.SaveContact: WARNING: saving comment with CAPTCHA DISABLED for Site {0}!!", Context.CurrentSite.SiteId.ToString());
         }

         if (captchaValid)
         {
            // Validate parameters
            if (string.IsNullOrEmpty(author) ||
                string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(message) ||
                !Validation.IsEmail(email))
               isValid = false;

            // send message
            if (isValid)
            {
               try
               {
                  //EmailService emailService = new EmailService();
                  //emailService.Send(mail);
                  Message contactMessage = new Message
                  {
                     Site = Context.CurrentSite,
                     To = this.Context.CurrentSite.Email,
                     From = email,
                     Subject = subject.StripHtml(),
                     Body = message.StripHtml(),
                     Type = MessageType.Contact
                  };

                  messageService.Save(contactMessage);

               }
               catch (Exception ex)
               {
                  log.Error(ex.ToString());
                  validationMessage = "Sorry, an unexpected error occured...";
                  ViewData["ContactValidationCssClass"] = validationErrorCssClass;
               }

            }
            else
            {
               validationMessage = validationErrorMessage;
               ViewData["ContactValidationCssClass"] = validationErrorCssClass;
            }

         }

         // put the message in the ViewData
         ViewData["ContactValidationMessage"] = validationMessage;

         TemplateContentModel model = GetDefaultTemplateContentModel();

         model.Posts = null;
         model.TemplateFile = ViewHelper.TemplateFile.contact;

         return RedirectToAction("Contact");
      }



      #endregion

      #region Search

      //[AcceptVerbs(HttpVerbs.Get)]
      //[ValidateAntiForgeryToken(Salt = "searchform")] WORK ONLY FOR HttpVerbs.POST
      public ActionResult Search(string s, int? page)
      {
         int currentPageIndex = 1;
         if (page.HasValue)
            currentPageIndex = page.Value;

         SearchResultCollection results = searchService.FindContent(s, currentPageIndex, Context.CurrentSite.MaxPostsPerPage);

         log.DebugFormat("Search for the query \"{0}\" returned {1} results in {2} seconds", s, results.TotalCount, results.ExecutionTime * 0.0000001F);

         TemplateContentModel model = GetDefaultTemplateContentModel();

         model.SearchResult = results;
         model.TemplateFile = ViewHelper.TemplateFile.search;

         return ViewContent(model);
      }

      #endregion

      #region Save Comments

      /// <summary>
      /// Save a new comment
      /// </summary>
      /// <param name="year"></param>
      /// <param name="month"></param>
      /// <param name="day"></param>
      /// <param name="name"></param>
      /// <param name="author"></param>
      /// <param name="email"></param>
      /// <param name="url"></param>
      /// <param name="comment"></param>
      /// <param name="comment_post_ID"></param>
      /// <param name="captchaValid"></param>
      /// <returns></returns>
      [CaptchaValidator]
      [AcceptVerbs(HttpVerbs.Post)]
      [ValidateInput(false)]
      public ActionResult SaveComment(string year, string month, string day, string name,
                                      string author, string email, string url, string comment, int comment_post_ID,
                                      bool captchaValid)
      {// TODO: use a ContactModel with AllowHtmlAttribute instead of ValidateInput(false)
         // if the captcha is disabled, set the captcha variable to true
         if (!Context.CurrentSite.EnableCaptchaForComments)
         {
            captchaValid = true;
            log.InfoFormat("PostController.SaveComment: WARNING: saving comment with CAPTCHA DISABLED for Site {0}!!", Context.CurrentSite.SiteId.ToString());
         }

         // validation
         if (string.IsNullOrEmpty(author) || string.IsNullOrEmpty(email) || !Validation.IsEmail(email) || !captchaValid)
            return ViewError("", ThemeResource("Comment_Message_ValidationErrors"));


         try
         {
            Post post = contentItemService.GetById(comment_post_ID);

            // Check if post comments are already allowed
            if (!post.AllowComments)
               return ViewError("", ThemeResource("Comment_Message_Closed"));

            // Create a new comment
            Comment c = new Comment
            {
               ContentItem = post,
               Name = author,
               Email = email,
               Url = url,
               UserIp = Request.UserHostAddress,
               CommentText = comment,
               CreatedDate = DateTime.UtcNow
            };

            // Check if the comment is duplicated (by the same author)
            if (commentService.CheckIfCommentIsDuplicate(c))
               return ViewError("", ThemeResource("Comment_Message_DuplicateComment"));

            // Evaluate comment if valid for auto approval
            // this check if the comments contains spam or must be contained in the moderation queue
            // as set in the Site Settings
            c.Status = commentService.EvaluateComment(c);

            // add the new comment to the post
            post.Comments.Add(c);

            // save the post (and the comment in cascade)
            contentItemService.Save(post);

            // strange behavior: RedirectToAction give strange exception (no route found....) bah
            return new RedirectResult(GetAbsoluteUrl(post.GetContentUrl()));
         }
         catch (Exception ex)
         {
            log.Error("PostController.SaveComment", ex);

            return ViewError("", ThemeResource("Comment_Message_GenericError"));
         }

      }

      #endregion

      #region Helpers

      /// <summary>
      /// Return the protocol (http:// or https://) along with the domain name and port number (if present)
      /// For example, if the requested page's URL is http://www.yourserver.com:8080/Tutorial01/MyPage.aspx, 
      /// then this method returns the string http://www.yourserver.com:8080
      /// </summary>
      /// <returns></returns>
      protected string GetCurrentSiteUrlRoot()
      {
         //VirtualPathUtility.Combine()
         return Request.Url.GetLeftPart(UriPartial.Authority);
      }


      
      /// <summary>
      /// Get a full absolute url for the current request.
      /// </summary>
      /// <param name="partialUrl">
      /// A virtual ("~/") or root-based url ("/")
      /// </param>
      /// <returns></returns>
      protected string GetAbsoluteUrl(string partialUrl)
      {
         return string.Concat(GetCurrentSiteUrlRoot(),
                              "/",
                              partialUrl.StartsWith("~") || partialUrl.StartsWith("/")
                                 ? partialUrl.Substring(1)
                                 : partialUrl);
      }

      #endregion
   }
}
