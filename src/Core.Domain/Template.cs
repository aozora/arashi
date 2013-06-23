using Arashi.Core.Extensions;

namespace Arashi.Core.Domain
{
   public class Template
   {

      #region Public Properties

      public virtual int TemplateId { get; set; }
      public virtual string Name { get; set; }
      public virtual Site Site { get; set; }
      public virtual string ThumbnailSrc { get; set; }

      /// <summary>
      /// Base path of the template in the format ~/Templates/{template-name}
      /// </summary>
      public virtual string BasePath
      {
         get;
         set;
      }


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


      public Template()
      {
         TemplateId = -1;
      }


      #endregion

      #region Equality


      public override bool Equals(object other)
      {
         if (this == other)
            return true;
         Template template = other as Template;
         if (template == null)
            return false;
         if (TemplateId != template.TemplateId)
            return false;
         return true;
      }
      

      public override int GetHashCode()
      {
         unchecked
         {
            int result;
            result = TemplateId.GetHashCode();
            result = 29 * result;
            return result;
         }
      }


      #endregion

   }
}
