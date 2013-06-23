using System;
using Common.Logging;

namespace Arashi.Services.Content
{
   using System.Collections.Generic;
   using System.Text.RegularExpressions;

   using Arashi.Core.Domain;
   using Arashi.Core.NHibernate;
   using Arashi.Core.Repositories;

   using NHibernate;
   using NHibernate.Criterion;

   using uNhAddIns.Pagination;

   /// <summary>
   /// Comment services.
   /// </summary>
   public class CommentService : ServiceBase, ICommentService
   {

      /// <summary>
      /// Creates a new instance of the <see cref="CommentService" /> class.
      /// </summary>
      public CommentService(Arashi.Core.NHibernate.ISessionFactory sessionFactory, ILog log)
         : base(sessionFactory, log)
      {
      }



      /// <summary>
      /// Gets a single comment by id.
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
      public Comment GetById(int id)
      {
         return Repository<Comment>.FindById(id);
      }



      /// <summary>
      /// Get the top 15 recent comments for the given site
      /// </summary>
      /// <param name="site"></param>
      /// <returns></returns>
      public IList<Comment> GetRecentComments(Site site)
      {
         return Session.GetNamedQuery("GetRecentComments")
                                                .SetEntity("site", site)
                                                .SetEnum("status", WorkflowStatus.Published)
                                                .SetDateTime("date", DateTime.Now.ToUniversalTime())
                                                .SetEnum("type", CommentType.Comment)
                                                .SetEnum("commentstatus", CommentStatus.Approved)
                                                .List<Comment>();
      }



      /// <summary>
      /// Saves a comment.
      /// </summary>
      /// <param name="comment"></param>
      public void SaveComment(Comment comment)
      {
         //using (NHTransactionScope tx = new NHTransactionScope())
         //{
         comment.ContentItem.Comments.Add(comment);
         Repository<Comment>.Save(comment);
         //   tx.VoteCommit();
         //}
      }



      /// <summary>
      /// Delete a comment.
      /// </summary>
      /// <param name="comment"></param>
      public void DeleteComment(Comment comment)
      {
         //using (NHTransactionScope tx = new NHTransactionScope())
         //{
         comment.ContentItem.Comments.Remove(comment);
         Repository<Comment>.Delete(comment);
         //   tx.VoteCommit();
         //}
      }



      public Paginator<Comment> FindPagedCommentsBySite(Site site, int pageSize)
      {
         if (site == null)
            throw new ArgumentNullException("site");

         DetachedCriteria criteria = DetachedCriteria.For<Comment>()
                                       .AddOrder(new Order("CreatedDate", false))
                                       .CreateAlias("ContentItem", "ci")
                                       .Add(Restrictions.Eq("ci.IsLogicallyDeleted", false))
                                       .Add(Restrictions.Eq("ci.Site", site));

         return Repository<Comment>.GetPaginator(criteria, pageSize);
      }



      public Paginator<Comment> FindPagedCommentsBySiteAndCommentStatus(Site site, CommentStatus status, int pageSize)
      {
         if (site == null)
            throw new ArgumentNullException("site");

         DetachedCriteria criteria = DetachedCriteria.For<Comment>()
                                       .AddOrder(new Order("CreatedDate", false))
                                       .CreateAlias("ContentItem", "ci")
                                       .Add(Restrictions.Eq("ci.IsLogicallyDeleted", false))
                                       .Add(Restrictions.Eq("ci.Site", site))
                                       .Add(Restrictions.Eq("Status", status));

         return Repository<Comment>.GetPaginator(criteria, pageSize);
      }



      public bool IsFirstPing(IContentItem item, CommentType type, string commentUrl)
      {
         return Session.GetNamedQuery("IsFirstPing")
                  .SetEntity("item", item)
                  .SetEnum("type", type)
                  .SetString("url", commentUrl)
                  .UniqueResult<long>() < 1;
      }



      public CommentStatus EvaluateComment(Comment comment)
      {
         #region Check Max Links in comment text

         string regex = "<[Aa][^>]*[Hh][Rr][Ee][Ff]=['\"]([^\"'>]+)[^>]*>";
         int matchesCount = Regex.Matches(comment.CommentText, regex).Count;

         if (matchesCount > comment.ContentItem.Site.MaxLinksInComments)
            return CommentStatus.Unapproved;

         #endregion

         #region Check Moderation Keys

         string moderationKeys = comment.ContentItem.Site.ModerationKeys;

         if (!string.IsNullOrEmpty(moderationKeys))
         {
            foreach (string key in moderationKeys.Split('\n'))
            {
               if (string.IsNullOrEmpty(key))
                  continue;

               // check for moderation key in the Author Name, Email, Url, Comment text, IP
               if (!string.IsNullOrEmpty(comment.Name))
                  if (comment.Name.IndexOf(key, 0, StringComparison.InvariantCultureIgnoreCase) > -1)
                     return CommentStatus.Unapproved;

               if (comment.Email.IndexOf(key, 0, StringComparison.InvariantCultureIgnoreCase) > -1)
                     return CommentStatus.Unapproved;

               if (comment.Url.IndexOf(key, 0, StringComparison.InvariantCultureIgnoreCase) > -1)
                  return CommentStatus.Unapproved;

               if (comment.CommentText.IndexOf(key, 0, StringComparison.InvariantCultureIgnoreCase) > -1)
                  return CommentStatus.Unapproved;

               if (comment.UserIp.IndexOf(key, 0, StringComparison.InvariantCultureIgnoreCase) > -1)
                  return CommentStatus.Unapproved;
               //if ( preg_match($pattern, $user_agent) ) return false;
            }
         }

         #endregion

         #region Check Blacklist Keys (indetify SPAM)

         string blacklistKeys = comment.ContentItem.Site.BlacklistKeys;

         if (!string.IsNullOrEmpty(blacklistKeys))
         {
            foreach (string blackKey in blacklistKeys.Split('\n'))
            {
               if (string.IsNullOrEmpty(blackKey))
                  continue;

               // check for moderation key in the Author Name, Email, Url, Comment text, IP
               if (!string.IsNullOrEmpty(comment.Name))
                  if (comment.Name.IndexOf(blackKey, 0, StringComparison.InvariantCultureIgnoreCase) > -1)
                     return CommentStatus.Spam;

               if (comment.Email.IndexOf(blackKey, 0, StringComparison.InvariantCultureIgnoreCase) > -1)
                  return CommentStatus.Spam;

               if (comment.Url.IndexOf(blackKey, 0, StringComparison.InvariantCultureIgnoreCase) > -1)
                  return CommentStatus.Spam;

               if (comment.CommentText.IndexOf(blackKey, 0, StringComparison.InvariantCultureIgnoreCase) > -1)
                  return CommentStatus.Spam;

               if (comment.UserIp.IndexOf(blackKey, 0, StringComparison.InvariantCultureIgnoreCase) > -1)
                  return CommentStatus.Spam;

               //if ( preg_match($pattern, $user_agent) ) return false;
            }
         }

         #endregion

         // if all is ok...
         return CommentStatus.Approved;
      }



      
      public bool CheckIfCommentIsDuplicate(Comment commentToCheck)
      {
         DetachedCriteria criteria = DetachedCriteria.For<Comment>()
                                       .Add(Restrictions.Eq("ContentItem", commentToCheck.ContentItem))
                                       .Add(Restrictions.Eq("CommentText", commentToCheck.CommentText))
                                       .SetProjection(Projections.Count("CommentId"));

         if (commentToCheck.CreatedBy != null)
            criteria.Add(Restrictions.Eq("CreatedBy", commentToCheck.CreatedBy));
         else
            criteria.Add(Restrictions.Eq("Email", commentToCheck.Email));
         
         ICriteria c = criteria.GetExecutableCriteria(Session);

         return Convert.ToInt64(c.UniqueResult()) > 0;
      }

   }
}