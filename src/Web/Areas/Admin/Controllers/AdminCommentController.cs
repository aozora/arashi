namespace Arashi.Web.Areas.Admin.Controllers
{
   using System;
   using System.Collections.Generic;
   using System.Web.Mvc;

   using Arashi.Core.Domain;
   using Arashi.Core.Extensions;
   using Arashi.Services.Content;
   using Arashi.Services.Membership;
   using Arashi.Web.Mvc.Controllers;
   using Arashi.Web.Mvc.Filters;
   using Arashi.Web.Mvc.Models;
   using Arashi.Web.Mvc.Paging;

   using log4net;

   using uNhAddIns.Pagination;

   /// <summary>
   /// Manage the comment moderation
   /// </summary>
   public class AdminCommentController : SecureControllerBase
   {
      private static readonly ILog log = LogManager.GetLogger(typeof(AdminPostController));
      private const int pageSize = 20;
      private readonly IContentItemService<Post> contentItemService;
      private readonly ICommentService commentService;

      #region Constructor

      public AdminCommentController(IContentItemService<Post> contentItemService,
                                    ICommentService commentService)
      {
         this.contentItemService = contentItemService;
         this.commentService = commentService;
      }

      #endregion

      /// <summary>
      /// Show the list of comments
      /// </summary>
      /// <param name="page"></param>
      /// <param name="status"></param>
      /// <returns></returns>
      [PermissionFilter(RequiredRights = Rights.CommentsView)]
      public ActionResult Index(int? page, string status)
      {
         IList<Comment> comments = null;
         Paginator<Comment> paginator;
         IPagedList<Comment> pagedList = null;

         int currentPageIndex = 1;
         if (page.HasValue)
            currentPageIndex = page.Value;

         if (string.IsNullOrEmpty(status))
            paginator = commentService.FindPagedCommentsBySite(Context.ManagedSite, pageSize);
         else
            paginator = commentService.FindPagedCommentsBySiteAndCommentStatus(Context.ManagedSite, (CommentStatus)Enum.Parse(typeof(CommentStatus), status), pageSize);

         if (paginator.HasPages)
         {
            if (currentPageIndex > paginator.LastPageNumber)
               currentPageIndex = 1;

            comments = paginator.GetPage(currentPageIndex);

            pagedList = new PagedList<Comment>(comments, page.HasValue ? page.Value - 1 : 0, pageSize, paginator.RowsCount.Value);// , totalcount
         }

         ViewData["CommentStatusDictionary"] = base.GetLocalizedEnumList(typeof(CommentStatus));
         ViewData["CommentStatus_Current"] = status;

         return View("Index", pagedList);
      }



      /// <summary>
      /// Change the status of a given comment
      /// </summary>
      /// <param name="id"></param>
      /// <param name="status"></param>
      /// <returns></returns>
      [AcceptVerbs(HttpVerbs.Post)]
      [PermissionFilter(RequiredRights = Rights.CommentsEdit)]
      public ActionResult ChangeStatus(int id, CommentStatus status)
      {
         Comment comment = commentService.GetById(id);

         comment.Status = status;
         comment.UpdatedBy = Context.CurrentUser;
         comment.UpdatedDate = DateTime.Now.ToUniversalTime();

         try
         {
            commentService.SaveComment(comment);

            // send the updated comment view
            return View("~/Areas/Admin/Views/AdminComment/Comment.ascx", comment);
         }
         catch (Exception ex)
         {
            log.Error("AdminCommentController.Update", ex);

            MessageModel message = new MessageModel
            {
               Text = GlobalResource("Message_GenericError"),
               Icon = MessageModel.MessageIcon.Alert,
            };

            return View("MessageUserControl", message);
         }

      }



      /// <summary>
      /// Delete a given comment
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
      [AcceptVerbs(HttpVerbs.Post)]
      [PermissionFilter(RequiredRights = Rights.CommentsDelete)]
      public ActionResult Delete(int id)
      {
         // TODO: implement undo function

         Comment comment = commentService.GetById(id);

         try
         {
            commentService.DeleteComment(comment);

            MessageModel message = new MessageModel
            {
               Text = "The selected comment has been deleted!",
               Icon = MessageModel.MessageIcon.Info,
            };

            return View("MessageUserControl", message);
         }
         catch (Exception ex)
         {
            log.Error("AdminCommentController.Delete", ex);

            MessageModel message = new MessageModel
            {
               Text = GlobalResource("Message_GenericError"),
               Icon = MessageModel.MessageIcon.Alert,
            };

            return View("MessageUserControl", message);
         }
      }



      /// <summary>
      /// Save a reply to a comment
      /// </summary>
      /// <param name="replyToCommentId"></param>
      /// <param name="replyContent"></param>
      /// <returns></returns>
      [AcceptVerbs(HttpVerbs.Post)]
      [PermissionFilter(RequiredRights = Rights.CommentsEdit)]
      public ActionResult SaveReply(int replyToCommentId, string replyContent)
      {
         // get the original comment to reply to
         Comment originalComment = commentService.GetById(replyToCommentId);

         try
         {
            // ensure the content is valid
            if (string.IsNullOrEmpty(replyContent))
            {
               MessageModel noContentMessage = new MessageModel
               {
                  Text = "Please type the content of the reply!",
                  Icon = MessageModel.MessageIcon.Alert
               };

               RegisterMessage(noContentMessage, true);
               return Index(null, string.Empty);
            }

            // get the comment post
            Post post = contentItemService.GetById(originalComment.ContentItem.Id);

            // Check if post comments are already allowed
            if (!post.AllowComments)
            {
               MessageModel commentsCloseMessage = new MessageModel
               {
                  Text = "Sorry, comments are close on this post!",
                  Icon = MessageModel.MessageIcon.Alert
               };

               RegisterMessage(commentsCloseMessage, true);
               return Index(null, string.Empty);
            }

            // Create a new comment
            Comment comment = new Comment
            {
               ContentItem = post,
               Name = Context.CurrentUser.DisplayName,
               Email = Context.CurrentUser.Email,
               Url = Context.CurrentUser.WebSite,
               UserIp = Request.UserHostAddress,
               CommentText = replyContent.StripHtml(),
               CreatedDate = DateTime.UtcNow, 
               CreatedBy = Context.CurrentUser
            };

            // Check if the comment is duplicated (by the same author)
            if (commentService.CheckIfCommentIsDuplicate(comment))
            {
               MessageModel duplicateCommentMessage = new MessageModel
               {
                  Text = "It seems that you have already added this comment",
                  Icon = MessageModel.MessageIcon.Alert
               };

               RegisterMessage(duplicateCommentMessage, true);
               return Index(null, string.Empty);
            }

            // Evaluate comment if valid for auto approval
            // this check if the comments contains spam or must be contained in the moderation queue
            // as set in the Site Settings
            comment.Status = commentService.EvaluateComment(comment);

            // add the new comment to the post
            post.Comments.Add(comment);

            // save the post (and the comment in cascade)
            contentItemService.Save(post);


            MessageModel message = new MessageModel
            {
               Text = "Reply saved!",
               Icon = MessageModel.MessageIcon.Info
            };

            RegisterMessage(message, true);
         }
         catch (Exception ex)
         {
            log.Error("AdminCommentController.SaveReply", ex);

            MessageModel message = new MessageModel
            {
               Text = GlobalResource("Message_GenericError"),
               Icon = MessageModel.MessageIcon.Alert
            };

            RegisterMessage(message, true);
         }

         // Refresh the comments list
         return Index(null, string.Empty);
      }

   }
}
