using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arashi.Core.Domain;
using Arashi.Core.Domain.Dto;
using Arashi.Services.Search;
using Arashi.Web.Mvc.Paging;
using Arashi.Web.Mvc.TemplateEngine;
using Arashi.Web.Widgets;

namespace Arashi.Web.Mvc.Models
{
   public class TemplateContentModel
   {
      public Site Site {get; set;}

      public IList<IWidgetComponent> WidgetComponents {get; set;}


      /// <summary>
      /// A paged list of ***published*** posts
      /// </summary>
      public IPagedList<Post> Posts {get; set;}

      /// <summary>
      /// A list of ALL ***published*** pages
      /// </summary>
      public IList<Page> Pages {get; set;}


      public SearchResultCollection SearchResult {get; set;}

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


      /// <summary>
      /// If not null, returns the current page.
      /// This is used only in the page template.
      /// </summary>
      public Page CurrentPage {get; set;}


      /// <summary>
      /// If not null, returns the current category.
      /// This is used only to display categories archive.
      /// </summary>
      public Category CurrentCategory {get; set;}


      /// <summary>
      /// If not null, returns the current tag.
      /// This is used only to display tags archive.
      /// </summary>
      public Tag CurrentTag {get; set;}


      /// <summary>
      /// The user that created posts. This is used ONLY for the author page
      /// </summary>
      public User CurrentAuthor {get; set;}


      public ViewHelper.TemplateFile TemplateFile {get; set;}
 
      public ViewHelper.ViewMode ViewMode {get; set;}


      public IList<ContentItemCalendarDTO> Calendar {get; set;}
   }
}
