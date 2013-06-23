using Arashi.Core.Domain;
using Arashi.Services.Content;
using Arashi.Services.Versioning;

namespace Arashi.Services.Versioning
{
   public class VersioningDecorator<T> : AbstractContentItemServiceDecorator<T> where T : IContentItem
   {
      private readonly IVersioningService<T> versioningService;

      public VersioningDecorator(IContentItemService<T> contentItemService, IVersioningService<T> versioningService)
         : base(contentItemService)
      {
         this.versioningService = versioningService;
      }

      #region Overrides

      public override T Save(T entity)
      {
         if (entity is IVersionableContent)
         {
            //call the versioning service to do the work
            this.versioningService.CreateNewVersion(entity);
         }
         return base.Save(entity);
      }

      #endregion
   }
}