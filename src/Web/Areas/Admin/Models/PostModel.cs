using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Arashi.Core.Domain;

namespace Arashi.Web.Areas.Admin.Models
{
   /// <summary>
   /// PostModel
   /// </summary>
   public class PostModel
   {
      public Post Post {get; set;}

      /// <summary>
      /// List of all the sites categories
      /// </summary>
      public IEnumerable<Category> SiteCategories {get; set;}

      public IList<Tag> SiteTags {get; set;}

      public SelectList SiteCategoriesSelectList {get; set;}

      public SelectList WorkflowStatus { get; set; }

   }
}
