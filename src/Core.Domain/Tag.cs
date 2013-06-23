using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arashi.Core.Extensions;

namespace Arashi.Core.Domain
{
   public class Tag
   {
      private string name;

      #region Public Properties

      public virtual int TagId
      {
         get;
         set;
      }

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
      /// Friendly name used in the url. Is forced to be lower-case
      /// </summary>
      public virtual string FriendlyName
      {
         get;
         set;
      }


      public virtual Site Site {get; set;}


      /// <summary>
      /// Content items associated with this category.
      /// </summary>
      public virtual IList<ContentItem> ContentItems
      {
         get;
         set;
      }

      #endregion

      /// <summary>
      /// Gets the url that corresponds to the content. Inheritors can override this for custom url formatting.
      /// </summary>
      public virtual string GetTagUrl()
      {
         if (this.Site == null)
            throw new InvalidOperationException("Unable to get the url for the content because the associated Site is missing.");

         const string defaultUrlFormat = "tag/{0}/";

         return String.Format(defaultUrlFormat, this.FriendlyName);
      }

      #region Constructor

      public Tag()
      {
         TagId = -1;
         ContentItems = new List<ContentItem>();
      }

      #endregion

      #region Equality

      public override bool Equals(object other)
      {
         if (this == other)
            return true;
         Tag t = other as Tag;
         if (t == null)
            return false;
         if (TagId != t.TagId)
            return false;
         return true;
      }



      public override int GetHashCode()
      {
         unchecked
         {
            int result;
            result = TagId.GetHashCode();
            result = 29 * result;
            return result;
         }
      }

      #endregion


   }
}
