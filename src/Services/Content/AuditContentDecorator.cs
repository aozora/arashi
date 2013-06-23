using System;
using Arashi.Core.Domain;
using Arashi.Services;

namespace Arashi.Services.Content
{
   /// <summary>
   /// Decorator for IContentItemService that adds auditing (created and modified by etc.) 
   /// to content items.
   /// </summary>
   /// <remarks>
   /// We might have to merge this one with the versioningdecorator.
   /// </remarks>
   /// <typeparam name="T"></typeparam>
   public class AuditContentDecorator<T> : AbstractContentItemServiceDecorator<T> 
      where T : IContentItem
   {
      private readonly IRequestContextProvider requestContextProvider;



      public AuditContentDecorator(IContentItemService<T> contentItemService, IRequestContextProvider requestContextProvider)
         : base(contentItemService)
      {
         this.requestContextProvider = requestContextProvider;
      }



      /// <summary>
      /// Add 'who did what and when' while saving the content item to the database.
      /// </summary>
      /// <param name="entity"></param>
      /// <returns></returns>
      public override T Save(T entity)
      {
         IRequestContext context = this.requestContextProvider.GetContext();
         
         if (entity.IsNew)
         {
            entity.CreatedDate = DateTime.UtcNow;
            entity.Author = context.CurrentUser;
         }

         entity.UpdatedDate = DateTime.UtcNow;
         entity.UpdatedBy = context.CurrentUser;

         return base.Save(entity);
      }

   }
}