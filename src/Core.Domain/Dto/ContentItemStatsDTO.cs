using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arashi.Core.Domain.Dto
{
   /// <summary>
   /// DTO for mini statistical info used on the Site home page in the Control Panel
   /// </summary>
   public class ContentItemStatsDTO
   {
      public Site Site {get; set;}
      public IList<ContentItemsCountByWorkflowStatus<Post>> PostCountByWorkflowStatus {get; set;}
      public IList<ContentItemsCountByWorkflowStatus<Page>> PageCountByWorkflowStatus {get; set;}
      public IList<CommentsCountByWorkflowStatus> CommentsCountByStatus {get; set;}
      public long CategoriesTotalCount {get; set;}
      public long TagsTotalCount {get; set;}


      /// <summary>
      /// Constructor
      /// </summary>
      public ContentItemStatsDTO()
      {
         PostCountByWorkflowStatus = new List<ContentItemsCountByWorkflowStatus<Post>>();
         PageCountByWorkflowStatus = new List<ContentItemsCountByWorkflowStatus<Page>>();
         CommentsCountByStatus = new List<CommentsCountByWorkflowStatus>();
         CategoriesTotalCount = 0;
         TagsTotalCount = 0;
      }

   }



   /// <summary>
   /// Statistic DTO for generic <see cref="IContentItem"/>
   /// </summary>
   /// <typeparam name="TContentItem"></typeparam>
   public class ContentItemsCountByWorkflowStatus<TContentItem>
      where TContentItem : IContentItem
   {
      public long Count {get; set;}
      public WorkflowStatus Status {get; set;}
   }



   /// <summary>
   /// Statistic DTO for comments
   /// </summary>
   public class CommentsCountByWorkflowStatus
   {
      public long Count {get; set;}
      public CommentStatus Status {get; set;}
   }


}
