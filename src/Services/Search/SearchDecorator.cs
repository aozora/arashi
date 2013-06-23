using System;
using System.Configuration;
using Arashi.Core.Domain;
using Arashi.Services.Content;
using Arashi.Services.Search;
using Arashi.Core.Domain.Search;


namespace Arashi.Services.Search
{
   public class SearchDecorator<T> : AbstractContentItemServiceDecorator<T> where T : IContentItem
   {
      private readonly ISearchService _searchService;

      public SearchDecorator(IContentItemService<T> contentItemService, ISearchService searchService)
         : base(contentItemService)
      {
         this._searchService = searchService;
      }

      /// <summary>
      /// Todo: move to properties.config
      /// </summary>
      private bool UseInstantIndexing
      {
         get
         {
            return (Boolean.Parse(ConfigurationManager.AppSettings["InstantIndexing"]));
         }
      }

      #region Overrides

      public override T Save(T entity)
      {
         // First, save entity to the database, so it has an identifier. Otherwise, the entity should be indexed with the wrong path.
         entity = base.Save(entity);

         if (UseInstantIndexing && entity is ISearchableContent)
         {
            // index ONLY published entities
            // (see IndexBuilder.BuildDocumentFromContentItem: I get an exception trying to index an entity
            //  that doesn't have the publisheddate)
            if (entity.PublishedDate.HasValue)
            {
               if (entity.IsNew)
               {
                  this._searchService.AddContent(entity);
               }
               else
               {
                  this._searchService.UpdateContent(entity);
               }
            }
         }
         return entity;
      }

      public override void Delete(T entity)
      {
         if (UseInstantIndexing)
         {
            if (entity is ISearchableContent)
            {
               this._searchService.DeleteContent(entity);
            }
         }
         base.Delete(entity);
      }

      #endregion
   }
}