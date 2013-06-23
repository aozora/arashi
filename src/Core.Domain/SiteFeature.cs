using System;

namespace Arashi.Core.Domain
{
   public class SiteFeature
   {

      #region Public Properties

      public virtual int SiteFeatureId { get; set; }

      public virtual Site Site { get; set; }

      public virtual Feature Feature { get; set; }

      public virtual bool Enabled { get; set; }

      public virtual DateTime StartDate { get; set; }
      public virtual DateTime? EndDate { get; set; }


      #endregion


      public SiteFeature()
      {
         SiteFeatureId = -1;
      }


      #region Equality

      public override bool Equals(object other)
      {
         if (this == other)
            return true;
         SiteFeature f = other as SiteFeature;
         if (f == null)
            return false;
         if (SiteFeatureId != f.SiteFeatureId)
            return false;
         return true;
      }



      public override int GetHashCode()
      {
         unchecked
         {
            int result;
            result = SiteFeatureId.GetHashCode();
            result = 29 * result;
            return result;
         }
      }

      #endregion

   }
}
