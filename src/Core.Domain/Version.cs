namespace Arashi.Core.Domain
{
   public class Version
   {
      public virtual int VersionId { get; set; }
      public virtual string Assembly { get; set; }
      public virtual int Major { get; set; }
      public virtual int Minor { get; set; }
      public virtual int Patch { get; set; }

      public Version()
      {
         VersionId = -1;
      }

      #region Equality

      public override bool Equals(object other)
      {
         if (this == other)
            return true;
         Version v = other as Version;
         if (v == null)
            return false;
         if (VersionId != v.VersionId)
            return false;
         return true;
      }



      public override int GetHashCode()
      {
         unchecked
         {
            int result = VersionId.GetHashCode();
            result = 29 * result;
            return result;
         }
      }

      #endregion

   }
}
