using Arashi.Core.Extensions;

namespace Arashi.Core.Domain
{
   public class Theme
   {

      #region Public Properties

      public virtual int ThemeId { get; set; }
      public virtual string Name { get; set; }
      public virtual string Description { get; set; }
      //public virtual Site Site { get; set; }
      public virtual string ThumbnailSrc { get; set; }

      /// <summary>
      /// Base path of the Theme in the format ~/themes/{Theme-name}
      /// </summary>
      public virtual string BasePath { get; set; }

      public virtual string CustomOptionsAssembly { get; set; }
      public virtual string CustomOptionsController { get; set; }
      public virtual string CustomOptionsAction { get; set; }

      

      /// <summary>
      /// Return the virtual path encoded in Base64. This is used to pass the path as parameter via querystring/routing
      /// </summary>
      public virtual string EncodedThumbnailVirtualPath
      {
         get
         {
            return string.Concat(BasePath, "/", ThumbnailSrc).EncodeToBase64();
         }
      }

      #endregion

      #region Constructor

      public Theme()
      {
         this.ThemeId = -1;
      }


      #endregion

      #region Equality


      public override bool Equals(object other)
      {
         if (this == other)
            return true;
         Theme theme = other as Theme;
         if (theme == null)
            return false;
         if (this.ThemeId != theme.ThemeId)
            return false;
         return true;
      }
      

      public override int GetHashCode()
      {
         unchecked
         {
            int result;
            result = this.ThemeId.GetHashCode();
            result = 29 * result;
            return result;
         }
      }


      #endregion

   }
}
