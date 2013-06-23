using System;
using System.Collections.Generic;
using Arashi.Core.Domain;
using Arashi.Core.Domain.Dto;
using Arashi.Services.Search;
using uNhAddIns.Pagination;

namespace Arashi.Services.Content
{

   public abstract class AbstractContentItemServiceDecorator<T> : IContentItemService<T> where T : IContentItem
   {
      private readonly IContentItemService<T> _contentItemService;

      protected IContentItemService<T> ContentItemService
      {
         get { return this._contentItemService; }
      }


      public AbstractContentItemServiceDecorator(IContentItemService<T> contentItemService)
      {
         _contentItemService = contentItemService;
      }


      public T GetPublishedByFriendlyName(Site site, string friendlyName)
      {
         return _contentItemService.GetPublishedByFriendlyName(site, friendlyName);
      }

      public Paginator<T> GetPaginatorForPublishedBySiteAndWorkflowStatus(Site site, WorkflowStatus status, int pageSize)
      {
         return _contentItemService.GetPaginatorForPublishedBySiteAndWorkflowStatus(site, status, pageSize);
      }


      public virtual T GetById(int id)
      {
         return _contentItemService.GetById(id);
      }

      public virtual T GetById(Guid id)
      {
         return _contentItemService.GetById(id);
      }

      public T GetPublishedByPublishedDateAndFriendlyName(Site site, int year, int month, int day, string friendlyName)
      {
         return _contentItemService.GetPublishedByPublishedDateAndFriendlyName(site, year, month, day, friendlyName);
      }


      public virtual IList<T> FindAllBySite(Site site)
      {
         return _contentItemService.FindAllBySite(site);
      }

      public virtual IList<T> FindAllBySite(Site site, string orderBy, bool orderAscending)
      {
         return _contentItemService.FindAllBySite(site, orderBy, orderAscending);
      }

      public Paginator<T> GetPaginatorBySite(Site site, int pageSize)
      {
         return _contentItemService.GetPaginatorBySite(site, pageSize);
      }

      public Paginator<T> GetPaginatorBySite(Site site, int pageSize, string orderBy, bool orderAscending)
      {
         return _contentItemService.GetPaginatorBySite(site, pageSize, orderBy, orderAscending);
      }

      public Paginator<T> GetPaginatorBySiteAndWorkflowStatus(Site site, WorkflowStatus status, int pageSize)
      {
         return _contentItemService.GetPaginatorBySiteAndWorkflowStatus(site, status, pageSize);
      }

      public Paginator<T> GetPaginatorBySiteAndWorkflowStatus(Site site, WorkflowStatus status, int pageSize, string orderBy, bool orderAscending)
      {
         return _contentItemService.GetPaginatorBySiteAndWorkflowStatus(site, status, pageSize, orderBy, orderAscending);
      }

      public Paginator<T> GetPaginatorBySiteAndWorkflowStatusAndAuthor(Site site, WorkflowStatus status, User author, int pageSize)
      {
         return _contentItemService.GetPaginatorBySiteAndWorkflowStatusAndAuthor(site, status, author, pageSize);
      }

      public Paginator<T> GetPaginatorForPublishedBySiteAndCategory(Site site, Category category, int pageSize)
      {
         return _contentItemService.GetPaginatorForPublishedBySiteAndCategory(site, category, pageSize);
      }

      public Paginator<T> GetPaginatorForPublishedBySiteAndTag(Site site, Tag tag, int pageSize)
      {
         return _contentItemService.GetPaginatorForPublishedBySiteAndTag(site, tag, pageSize);
      }

      public Paginator<T> GetPaginatorForPublishedBySiteAndPublishedDate(Site site, int year, int? month, int? day, int pageSize)
      {
         return _contentItemService.GetPaginatorForPublishedBySiteAndPublishedDate(site, year, month, day, pageSize);
      }



      public IList<ContentItemCalendarDTO> GetPostCalendarForPublishedBySite(Site site)
      {
         return _contentItemService.GetPostCalendarForPublishedBySite(site);
      }

      public ContentItemStatsDTO GetStatsBySite(Site site)
      {
         return _contentItemService.GetStatsBySite(site);
      }

      //public virtual IList<T> FindContentItemsBySection(Section section)
      //{
      //   return _contentItemService.FindContentItemsBySection(section);
      //}


      public IList<T> GetRecentItems(Site site)
      {
         return _contentItemService.GetRecentItems(site);
      }


      public virtual T Save(T entity)
      {
         return _contentItemService.Save(entity);
      }

      public virtual void Delete(T entity)
      {
         _contentItemService.Delete(entity);
      }


      ///// <summary>
      ///// Find ContentItems by the section they're assigned to.
      ///// </summary>
      ///// <param name="section"></param>
      ///// <param name="querySettings"></param>
      ///// <returns></returns>
      //public virtual IList<T> FindContentItemsBySection(Section section, ContentItemQuerySettings querySettings)
      //{
      //   return _contentItemService.FindContentItemsBySection(section, querySettings);
      //}

      ///// <summary>
      ///// Find the currently visible ContentItems by the section they're assigned to.
      ///// </summary>
      ///// <param name="section"></param>
      ///// <param name="querySettings"></param>
      ///// <returns></returns>
      //public virtual IList<T> FindVisibleContentItemsBySection(Section section, ContentItemQuerySettings querySettings)
      //{
      //   return _contentItemService.FindVisibleContentItemsBySection(section, querySettings);
      //}

      /// <summary>
      /// Find currently visible ContentItems for a given category.
      /// </summary>
      /// <param name="category"></param>
      /// <param name="querySettings"></param>
      /// <returns></returns>
      public virtual IList<T> FindVisibleContentItemsByCategory(Category category, ContentItemQuerySettings querySettings)
      {
         return _contentItemService.FindVisibleContentItemsByCategory(category, querySettings);
      }

      ///// <summary>
      ///// Find the archived content items for a given section (published until before today)
      ///// </summary>
      ///// <param name="section"></param>
      ///// <param name="querySettings"></param>
      ///// <returns></returns>
      //public virtual IList<T> FindArchivedContentItemsBySection(Section section, ContentItemQuerySettings querySettings)
      //{
      //   return _contentItemService.FindArchivedContentItemsBySection(section, querySettings);
      //}

      ///// <summary>
      ///// Find the syndicated content items for a given section.
      ///// </summary>
      ///// <param name="section"></param>
      ///// <param name="querySettings"></param>
      ///// <returns></returns>
      //public virtual IList<T> FindSyndicatedContentItemsBySection(Section section, ContentItemQuerySettings querySettings)
      //{
      //   return _contentItemService.FindSyndicatedContentItemsBySection(section, querySettings);
      //}


      public IList<T> FindSyndicatedBySite(Site site, int maxEntries)
      {
         return _contentItemService.FindSyndicatedBySite(site, maxEntries);
      }



      /// <summary>
      /// Find the syndicated content items for a given category.
      /// </summary>
      /// <param name="category"></param>
      /// <param name="querySettings"></param>
      /// <returns></returns>
      public virtual IList<T> FindSyndicatedByCategory(Category category, ContentItemQuerySettings querySettings)
      {
         return _contentItemService.FindSyndicatedByCategory(category, querySettings);
      }



      public long GetCountForSimilarFriendlyNameBySite(Site site, string friendlyName)
      {
         return _contentItemService.GetCountForSimilarFriendlyNameBySite(site, friendlyName);
      }


      public IEnumerable<T> ConvertSearchResultCollectionToContentItemList(SearchResultCollection results)
      {
         return _contentItemService.ConvertSearchResultCollectionToContentItemList(results);
      }

   }
}