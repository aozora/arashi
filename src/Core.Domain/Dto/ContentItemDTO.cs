using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arashi.Core.Domain
{
   /// <summary>
   /// ContentItem DTO
   /// </summary>
   public class ContentItemDTO
   {
      public virtual int Id {get; set;}
      public virtual WorkflowStatus WorkflowStatus { get; set; }
      public virtual string Title {get; set;}
      public virtual string FriendlyName { get; set; }
      public virtual string Summary { get; set; }
      public virtual int Version { get; set; }
      public virtual string Culture { get; set; }
      public virtual DateTime CreatedDate { get; set; }
      public virtual DateTime? PublishedDate { get; set; }
      public virtual DateTime? PublishedUntil { get; set; }
      public virtual DateTime UpdatedDate { get; set; }
      public virtual User Author { get; set; }
      //public virtual User PublishedBy { get; set; }
      //public virtual User UpdatedBy { get; set; }
      public virtual Site Site { get; set; }

      public virtual IList<Category> Categories { get; set; }
      public virtual IList<Comment> Comments { get; set; }
      public virtual IList<Tag> Tags { get; set; }



      public ContentItemDTO()
      {
         Id = -1;
         Categories = new List<Category>();
         Comments = new List<Comment>();
         Tags = new List<Tag>();
      }


      #region Equality

      public override bool Equals(object other)
      {
         if (this == other)
            return true;
         ContentItemDTO c = other as ContentItemDTO;
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
