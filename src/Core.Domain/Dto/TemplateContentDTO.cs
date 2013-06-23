using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arashi.Core.Domain.Dto
{
   public class TemplateContentDTO
   {
      /// <summary>
      /// A list of ALL ***published*** pages
      /// </summary>
      public IList<Page> Pages {get; set;}

      /// <summary>
      /// All the site categories
      /// </summary>
      public IEnumerable<Category> Categories {get; set;}

      /// <summary>
      /// All the site tags
      /// </summary>
      public IList<Tag> Tags {get; set;}


      /// <summary>
      /// All the site tags
      /// </summary>
      public IList<TagDTO> TagCloud {get; set;}


      /// <summary>
      /// Recent comments
      /// </summary>
      public IList<Comment> RecentComments {get; set;}


      /// <summary>
      /// Recent Posts
      /// </summary>
      public IList<Post> RecentPosts {get; set;}

      public IList<ContentItemCalendarDTO> Calendar {get; set;}


   }
}
