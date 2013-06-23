using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
//using Arashi.Services.Versioning;
using Arashi.Core.Extensions;
using ApplicationException=Arashi.Core.Exceptions.ApplicationException;

namespace Arashi.Core.Domain
{
   /// <summary>
   /// Base Class for CMS Content
   /// </summary>
   public abstract class ContentItem : IContentItem
   {
      #region Protected Fields

      protected int id;
      private string title;

      #endregion

      #region Properties

      /// <summary>
      /// Property Id (long)
      /// </summary>
      public virtual int Id
      {
         get { return id; }
         set { id = value; }
      }

      public virtual Guid GlobalId { get; set; }
      public virtual WorkflowStatus WorkflowStatus { get; set; }

      /// <summary>
      /// Set the title of the content, and automatically build the name (friendly-url)
      /// </summary>
      public virtual string Title
      {
         get
         {
            return title;
         }
         set
         {
            title = value;
         }
      }

      public virtual string FriendlyName { get; set; }
      /// <summary>
      /// The condensed description of the item
      /// </summary>
      public virtual string Summary { get; set; }
      public virtual int Version { get; set; }
      public virtual string Culture { get; set; }
      public virtual bool Syndicate { get; set; }
      public virtual DateTime CreatedDate { get; set; }
      public virtual DateTime? PublishedDate { get; set; }
      public virtual DateTime? PublishedUntil { get; set; }
      public virtual DateTime UpdatedDate { get; set; }
      public virtual User Author { get; set; }
      public virtual User PublishedBy { get; set; }
      public virtual User UpdatedBy { get; set; }
      public virtual Site Site { get; set; }
      public virtual bool AllowComments { get; set; }
      /// <summary>
      /// Allow/Disallow trackbacks & pingbacks
      /// </summary>
      public virtual bool AllowPings { get; set; }
      public virtual ICollection<Category> Categories { get; set; }
      public virtual IList<ContentItemPermission> ContentItemPermissions { get; set; }
      public virtual IList<Comment> Comments { get; set; }
      public virtual ICollection<Tag> Tags { get; set; }
      public virtual IDictionary<String, String> CustomFields { get; set; }

      // Custom SEO properties
      public virtual string MetaDescription { get; set; }
      public virtual string MetaKeywords { get; set; }
      public virtual bool EnableMeta { get; set; }

      /// <summary>
		/// Indicates if the content item is new.s
		/// </summary>
		public virtual bool IsNew
		{
			get { return id == -1; }
		}

      /// <summary>
      /// If true the item is logically deleted
      /// </summary>
      public virtual bool IsLogicallyDeleted { get; set; }


      /// <summary>
      /// Indicates if the content item supports item-level permissions. 
      /// If not, the permissions for the related section
      /// are used.
      /// </summary>
      public virtual bool SupportsItemLevelPermissions
      {
         get
         {
            //return false;
            return true;
         }
      }



      /// <summary>
      /// The roles that are allowed to view the content item.
      /// </summary>
      public virtual IEnumerable<Role> ViewRoles
      {
         get
         {
            //if (SupportsItemLevelPermissions)
            //{
            return ContentItemPermissions.Where(cip => cip.ViewAllowed).Select(cip => cip.Role);
            //}
            //else
            //{
            //   return this._section.SectionPermissions.Where(sp => sp.ViewAllowed).Select(sp => sp.Role);
            //}
         }
      }


      #endregion

      /// <summary>
      /// Constructor
      /// </summary>
      public ContentItem()
      {
         id = -1;
         GlobalId = Guid.NewGuid();
         WorkflowStatus = WorkflowStatus.Draft;
         CreatedDate = DateTime.Now;
         UpdatedDate = DateTime.Now;
         Version = 1;
         Categories = new List<Category>();
         ContentItemPermissions = new List<ContentItemPermission>();
         Comments = new List<Comment>();
         Tags = new List<Tag>();
         CustomFields = new Dictionary<string, string>();
      }


      
		/// <summary>
		/// Gets the url that corresponds to the content. 
		/// Inheritors can override this for custom url formatting.
		/// </summary>
		public virtual string GetContentUrl()
		{
         // If the contentitem is not published throw an exception!
         if (!PublishedDate.HasValue)
            throw new ApplicationException("ContentItem.GetContentUrl: cannot format url for items without PublishedDate!");
         
         const string defaultUrlFormat = "{0}/{1}/{2}/{3}/";

         if (this.Site == null)
         {
            throw new InvalidOperationException("Unable to get the url for the content because the associated Site is missing.");
         }

         return String.Format(defaultUrlFormat, 
                              PublishedDate.Value.Year,
                              PublishedDate.Value.Month.ToString().PadLeft(2, '0'),
                              PublishedDate.Value.Day.ToString().PadLeft(2, '0'),
                              this.FriendlyName);
      }



      //public virtual bool IsViewAllowed(IPrincipal currentPrincipal)
      //{

      //   if (!currentPrincipal.Identity.IsAuthenticated)
      //   {
      //      return this.ViewRoles.Any(vr => vr.HasRight(Rights.Anonymous));
      //   }
      //   if (currentPrincipal is User)
      //   {
      //      return IsViewAllowedForUser((User)currentPrincipal);
      //   }
      //   return false;
      //}



      //public virtual bool IsViewAllowedForUser(User user)
      //{
      //   return this.ViewRoles.Any(user.IsInRole);
      //}


      #region Equality

      public override bool Equals(object other)
      {
         if (this == other)
            return true;
         ContentItem c = other as ContentItem;
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