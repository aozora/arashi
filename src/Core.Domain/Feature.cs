namespace Arashi.Core.Domain
{
   public class Feature
   {

      #region Public Properties

      public virtual int FeatureId { get; set; }

      public virtual int Order { get; set; }

      public virtual FeatureCategory Category {get; set;}

      public virtual string Name { get; set; }

      //public virtual string Description { get; set; }

      public virtual string ImageSrc { get; set; }

      public virtual string LittleImageSrc { get; set; }

      public virtual string ImageAlt { get; set; }

      public virtual string Assembly { get; set; }
      public virtual string Controller { get; set; }
      public virtual string Action { get; set; }
      public virtual string NewAction { get; set; }

      public virtual string Parameters { get; set; }


      #endregion


      public Feature()
      {
         FeatureId = -1;
      }


      #region Equality

      public override bool Equals(object other)
      {
         if (this == other)
            return true;
         Feature f = other as Feature;
         if (f == null)
            return false;
         if (FeatureId != f.FeatureId)
            return false;
         return true;
      }



      public override int GetHashCode()
      {
         unchecked
         {
            int result;
            result = FeatureId.GetHashCode();
            result = 29 * result;
            return result;
         }
      }

      #endregion

   }
}
