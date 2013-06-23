using System;
using System.Collections.Generic;
using System.Text;

namespace Arashi.Core.Domain
{
   public class SiteHost
   {
      #region Private Fields

      private int siteHostId;

      #endregion

      #region Public Properties

      public virtual int SiteHostId
      {
         get
         {
            return siteHostId;
         }
         set
         {
            siteHostId = value;
         }
      }

      public virtual Site Site { get; set; }

      public virtual string HostName { get; set; }

      public virtual bool IsDefault { get; set; }

      /// <summary>
      /// E' possibile associare un Theme ad un dato SiteHost; 
      /// in questo caso avrà la precedenza rispetto a quello impostato in Site.Theme
      /// </summary>
      public virtual Theme Theme { get; set; }

      public virtual DateTime CreatedDate { get; set; }

      public virtual DateTime? UpdatedDate { get; set; }

      #endregion

      public SiteHost()
      {
         siteHostId = -1;
      }

      #region Equality

      public override bool Equals(object other)
      {
         if (this == other)
            return true;
         SiteHost sh = other as SiteHost;
         if (sh == null)
            return false;
         if (SiteHostId != sh.SiteHostId)
            return false;
         return true;
      }



      public override int GetHashCode()
      {
         unchecked
         {
            int result;
            result = SiteHostId.GetHashCode();
            result = 29 * result + HostName.GetHashCode();
            return result;
         }
      }

      #endregion
   }
}
