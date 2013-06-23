using System;

namespace Arashi.Core.Domain
{
   /// <summary>
   /// Represents a category to classify content.
   /// </summary>
   public class FeatureCategory
   {

      #region Properties

      /// <summary>
      /// Persistent Id
      /// </summary>
      public virtual int FeatureCategoryId { get; set; }


      /// <summary>
      /// The category name
      /// </summary>
      public virtual string Name { get; set; }


      /// <summary>
      /// Visualization Order 
      /// </summary>
      public virtual int Order { get; set; }


      /// <summary>
      /// Image (64x64)
      /// </summary>
      public virtual string ImageSrc { get; set; }


      #endregion


      #region Constructor

      /// <summary>
      /// Creates a new instance of the <see cref="Category"></see> class.
      /// </summary>
      public FeatureCategory()
      {
         FeatureCategoryId = -1;
      }

      #endregion

      #region Equality

      public override bool Equals(object other)
      {
         if (this == other)
            return true;
         FeatureCategory c = other as FeatureCategory;
         if (c == null)
            return false;
         if (FeatureCategoryId != c.FeatureCategoryId)
            return false;
         return true;
      }



      public override int GetHashCode()
      {
         unchecked
         {
            int result = FeatureCategoryId.GetHashCode();
            result = 29 * result;
            return result;
         }
      }

      #endregion

   }
}