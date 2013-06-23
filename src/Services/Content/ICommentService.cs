using Arashi.Core.Domain;
using uNhAddIns.Pagination;
using System.Collections.Generic;

namespace Arashi.Services.Content
{
   public interface ICommentService
   {
      /// <summary>
      /// Get a single comment by id.
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
      Comment GetById(int id);

      /// <summary>
      /// Saves a comment.
      /// </summary>
      /// <param name="comment"></param>
      void SaveComment(Comment comment);

      /// <summary>
      /// Delete a comment.
      /// </summary>
      /// <param name="comment"></param>
      void DeleteComment(Comment comment);

      /// <summary>
      /// Get the top 15 recent comments for the given site
      /// </summary>
      /// <param name="site"></param>
      /// <returns></returns>
      IList<Comment> GetRecentComments(Site site);

      Paginator<Comment> FindPagedCommentsBySite(Site site, int pageSize);

      Paginator<Comment> FindPagedCommentsBySiteAndCommentStatus(Site site, CommentStatus status, int pageSize);


      /// <summary>
      /// This method is used to find an exisisting pingback or trackback
      /// </summary>
      /// <param name="item"></param>
      /// <param name="type"></param>
      /// <param name="commentUrl"></param>
      /// <returns></returns>
      bool IsFirstPing(IContentItem item, CommentType type, string commentUrl);


      /// <summary>
      /// Set a CommentStatus for a given comment.
      /// The comment is evaluated to verify if can be auto-approved or classified as spam,
      /// due to differents site configurations.
      /// The code is pased on WordPress comments.php
      /// 
      /// See http://codex.wordpress.org/Comment_Moderation
      /// </summary>
      /// <param name="comment"></param>
      /// <returns></returns>
      CommentStatus EvaluateComment(Comment comment);



      /// <summary>
      /// Verify if a new comment has been already inserted before
      /// </summary>
      /// <param name="commentToCheck"></param>
      /// <returns></returns>
      bool CheckIfCommentIsDuplicate(Comment commentToCheck);
   }
}