using System;
using System.Collections.Generic;

namespace Arashi.Core.Domain
{
   /// <summary>
   /// IContentItem, common denominator
   /// for all CMS content
   /// </summary>
   public interface IContentItem
   {
      /// <summary>
      /// System-wide Identifier
      /// </summary>
      int Id { get; set; }

      /// <summary>
      /// Global Identifier
      /// </summary>
      Guid GlobalId { get; set; }

      /// <summary>
      /// Workflow information
      /// </summary>
      WorkflowStatus WorkflowStatus { get; set; }

      /// <summary>
      /// Set the title of the content, and automatically build the name (friendly-url)
      /// </summary>
      string Title { get; set; }

      /// <summary>
      /// Sanitized version of the content title. Usefull to be used in uri.
      /// </summary>
      string FriendlyName { get; set; }

      /// <summary>
      /// Short description
      /// </summary>
      string Summary { get; set; }

      Site Site  { get; set; }

      /// <summary>
      /// Defines the culture info (e.g. en-US)
      /// </summary>
      string Culture { get; set; }

		/// <summary>
		/// Indicates if the content item should be syndicated.
		/// </summary>
		bool Syndicate { get; set; }

      /// <summary>
      /// Version information
      /// </summary>
      int Version { get; set; }

      /// <summary>
      /// Date of creation
      /// </summary>
      DateTime CreatedDate { get; set; }

      /// <summary>
      /// Start of publication
      /// </summary>
      DateTime? PublishedDate { get; set; }

      /// <summary>
      /// End of publication 
      /// </summary>
      DateTime? PublishedUntil { get; set; }

      /// <summary>
      /// Date of last modification
      /// </summary>
      DateTime UpdatedDate { get; set; }

      /// <summary>
      /// Creator
      /// </summary>
      User Author { get; set; }

      /// <summary>
      /// Publisher
      /// </summary>
      User PublishedBy { get; set; }

      /// <summary>
      /// Last Modificator
      /// </summary>
      User UpdatedBy { get; set; }



      /// <summary>
      /// Assigned categories
      /// </summary>
      IList<Category> Categories { get; set; }

      IList<Tag> Tags { get; set; }
      
      IDictionary<String, String> CustomFields { get; set; }

		/// <summary>
		/// Comments
		/// </summary>
		IList<Comment> Comments { get; set; }

      /// <summary>
      /// Defines view and edit roles
      /// </summary>
      IList<ContentItemPermission> ContentItemPermissions { get; set; }

      		/// <summary>
		/// The roles that are allowed to view the content item.
		/// </summary>
		IEnumerable<Role> ViewRoles { get; }

		/// <summary>
		/// Gets the url that corresponds to the content. Inheritors can override this for custom url formatting.
		/// </summary>
		/// <returns></returns>
		string GetContentUrl();

		/// <summary>
		/// Indicates if the content item is new.
		/// </summary>
		bool IsNew { get; }

      /// <summary>
      /// True if the item is logically deleted
      /// </summary>
      bool IsLogicallyDeleted { get; set; }
   }
}