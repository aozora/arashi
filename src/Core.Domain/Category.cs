using System;
using System.Collections.Generic;
using Arashi.Core.Extensions;

namespace Arashi.Core.Domain
{
   /// <summary>
   /// Represents a category to classify content.
   /// </summary>
   public class Category
   {
      private int id;
      private string name;

      #region Properties

      /// <summary>
      /// Persistent Id
      /// </summary>
      public virtual int Id
      {
         get
         {
            return this.id;
         }
         set
         {
            this.id = value;
         }
      }

      /// <summary>
      /// This is a 80 characters long path definition with 16 fragments in following format: .0000
      /// allowing 16 levels of categories with each a maximum of 9999 entries.
      /// </summary>
      /// <remarks>This one is used to simplify hierarchical queries etc (Materialized Path).</remarks>
      public virtual string Path { get; set; }

      /// <summary>
      /// The level (number of parents) of the category
      /// </summary>
      public virtual int Level
      {
         get
         {
            return (this.Path != null ? this.Path.Length / 5 - 1 : 0);
         }
      }

      /// <summary>
      /// The position (in a list of categories) of the category
      /// </summary>
      public virtual int Position { get; set; }


      /// <summary>
      /// The category name
      /// </summary>
      public virtual string Name
      {
         get
         {
            return name;
         }
         set
         {
            name = value;

            // also set the name as a sanitized title
            this.FriendlyName = name.Sanitize();
         }
      }

      /// <summary>
      /// A sanitized version of the name
      /// </summary>
      public virtual string FriendlyName { get; set; }

      public virtual Site Site { get; set; }

      /// <summary>
      /// The parent caetegory.
      /// </summary>
      public virtual Category ParentCategory { get; set; }

      /// <summary>
      /// Child categories.
      /// </summary>
      public virtual IList<Category> ChildCategories { get; set; }

      /// <summary>
      /// Content items associated with this category.
      /// </summary>
      public virtual ICollection<ContentItem> ContentItems { get; set; }

      #endregion

      /// <summary>
      /// Gets the url that corresponds to the content. Inheritors can override this for custom url formatting.
      /// </summary>
      public virtual string GetCategoryUrl()
      {
         if (this.Site == null)
            throw new InvalidOperationException("Unable to get the url for the content because the associated Site is missing.");

         const string defaultUrlFormat = "category/{0}/";

         return String.Format(defaultUrlFormat, this.FriendlyName);
      }


      #region Constructor

      /// <summary>
      /// Creates a new instance of the <see cref="Category"></see> class.
      /// </summary>
      public Category()
      {
         this.id = -1;
         ChildCategories =  new List<Category>();
         ContentItems = new List<ContentItem>();
      }

      #endregion

      #region Equality

      public override bool Equals(object other)
      {
         if (this == other)
            return true;
         Category c = other as Category;
         if (c == null)
            return false;
         if (Id != c.Id)
            return false;
         return true;
      }



      public override int GetHashCode()
      {
         unchecked
         {
            int result = Id.GetHashCode();
            result = 29 * result;
            return result;
         }
      }

      #endregion

   }
}