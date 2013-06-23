using System;
using System.Collections.Generic;
using System.Linq;
using Arashi.Services.Search;
using Arashi.Core.Extensions;
using Arashi.Web.Mvc.Models;

namespace Arashi.Web.Mvc.TemplateEngine
{
   /// <summary>
   /// This is a special variant of the TemplateBase class in wich some of the
   /// THE_LOOP functions are overriden in order to be used in the search template.
   /// </summary>
   public class SearchTemplateBase<TModel> : TemplateBase<TModel>
      where TModel : TemplateContentModel
   {

      private SearchResult currentPost;


      protected override bool have_posts()
      {
         return Model.SearchResult != null && Model.SearchResult.TotalCount > 0;
      }


      protected void the_post(SearchResult post)
      {
         currentPost = post;

         // also set the current post in the Context, for use by other templates
         //HttpContext.Current.Items["CurrentPost"] = currentPost;
      }



      //protected string post_class()
      //{
      //   StringBuilder sb = new StringBuilder();
      //   sb.Append("class=\"");
      //   sb.AppendFormat("post-{0}", currentPost.ContentItemId.ToString());
      //   sb.AppendFormat(" post hentry", currentPost.ContentItemId.ToString());

      //   IList<Category> categories = GetCategoriesForCurrentSearchResult();
      //   if (categories.Count > 0)
      //   {
      //      foreach (Category category in categories)
      //      {
      //         sb.AppendFormat(" category-{0}", category.FriendlyName);
      //      }
      //   }

      //   IList<Tag> tags = GetTagsForCurrentSearchResult();
      //   if (tags.Count > 0)
      //   {
      //      foreach (Tag tag in tags)
      //      {
      //         sb.AppendFormat(" tag-{0}", tag.FriendlyName);
      //      }
      //   }

      //   sb.Append("\" ");

      //   return sb.ToString();
      //}



      protected override string the_ID()
      {
         return currentPost.ContentItemId.ToString();
      }



      protected override string the_permalink()
      {
         return GetAbsoluteUrl(currentPost.Path);
      }



      protected override string the_title()
      {
         return Html.Encode(currentPost.Title);
      }


      protected override string the_title_attribute()
      {
         return Html.Encode(currentPost.Title.StripHtml());
      }



      protected override string the_time()
      {
         return the_time("");
      }



      protected override string the_time(string format)
      {
         return GetTheTime(currentPost.DatePublished, format);
      }



      //protected string the_category(string separator)
      //{
      //   return RenderCategoriesLinks(GetCategoriesForCurrentSearchResult(), separator);
      //}



      //protected virtual string the_tags(string before, string separator, string after)
      //{
      //   return RenderTagsLinks(GetTagsForCurrentSearchResult(), before, separator, after);
      //}



      protected override string the_excerpt()
      {
         return Html.Encode(currentPost.Summary);
      }




      protected override string next_posts_link(string label)
      {
         if (string.IsNullOrEmpty(label))
            label = Resource("Search_NextPage");

         int pageSize = Model.Site.MaxPostsPerPage;
         int currentPage = Model.SearchResult.PageIndex;
         long totalItemCount = Model.SearchResult.TotalCount;

         int pageCount = (int)Math.Ceiling(totalItemCount / (double)pageSize);

         // if there is only 1 page, don't show the pager
         if (pageCount <= 1)
            return string.Empty;

         // Next
         if (currentPage < pageCount)
            return GenerateNavigationLink(label, currentPage + 1);

         return string.Empty;
      }



      protected override string previous_posts_link(string label)
      {
         if (string.IsNullOrEmpty(label))
            label = Resource("Search_PreviousPage");

         int pageSize = Model.Site.MaxPostsPerPage;
         int currentPage = Model.SearchResult.PageIndex;
         long totalItemCount = Model.SearchResult.TotalCount;

         int pageCount = (int)Math.Ceiling(totalItemCount / (double)pageSize);

         // if there is only 1 page, don't show the pager
         if (pageCount <= 1)
            return string.Empty;

         // Previous
         if (currentPage > 1)
            return GenerateNavigationLink(label, currentPage - 1);

         return string.Empty;
      }



      //#region Utils

      //private IList<Category> GetCategoriesForCurrentSearchResult()
      //{
      //   IList<Category> list = new List<Category>();

      //   string[] ids = currentPost.Category
      //                     .Replace(" ", "")
      //                     .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

      //   foreach (string id in ids)
      //   {
      //      Category category = Model.Categories.Where(c => c.Name == id).SingleOrDefault();
      //      list.Add(category);
      //   }

      //   return list;
      //}



      //private IList<Tag> GetTagsForCurrentSearchResult()
      //{
      //   IList<Tag> list = new List<Tag>();

      //   string[] ids = currentPost.Tag
      //                     .Replace(" ", "")
      //                     .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

      //   foreach (string id in ids)
      //   {
      //      Tag tag = Model.Tags.Where(c => c.Name == id).SingleOrDefault();
      //      list.Add(tag);
      //   }

      //   return list;
      //}

      //#endregion



   }
}
