using System;
using System.Collections.Generic;
using Arashi.Core.Domain;
using Arashi.Core.Domain.Dto;
using Arashi.Services.Search;
using uNhAddIns.Pagination;

namespace Arashi.Services.Content
{
	/// <summary>
	/// Service responsible for content item management.
	/// </summary>
	public interface IContentItemService<T> where T : IContentItem
   {
      #region Methods used exclusively by the Content Controllers

      /// <summary>
      /// Get a single content item with the Published status, 
      /// and with the given friendlyname.
      /// This must be used EXCLUSIVELY to retrieve a given page!
      /// </summary>
      /// <param name="site"></param>
      /// <param name="friendlyName"></param>
      /// <returns></returns>
	   T GetPublishedByFriendlyName(Site site, string friendlyName);



      /// <summary>
      /// Get a single content item with the Published status, 
      /// and with the given publisheddate and friendlyname.
      /// This is used for the single template
      /// </summary>
      /// <param name="site"></param>
      /// <param name="year"></param>
      /// <param name="month"></param>
      /// <param name="day"></param>
      /// <param name="friendlyName"></param>
      /// <returns></returns>
      T GetPublishedByPublishedDateAndFriendlyName(Site site, int year, int month, int day, string friendlyName);
      
      /// <summary>
      /// Get a paged list of contentitems published before now
      /// This is used for the index template.
      /// </summary>
      /// <param name="site"></param>
      /// <param name="status"></param>
      /// <param name="pageSize"></param>
      /// <returns></returns>
      Paginator<T> GetPaginatorForPublishedBySiteAndWorkflowStatus(Site site, WorkflowStatus status, int pageSize);


      Paginator<T> GetPaginatorForPublishedBySiteAndCategory(Site site, Category category, int pageSize);

      Paginator<T> GetPaginatorForPublishedBySiteAndTag(Site site, Tag tag, int pageSize);

      /// <summary>
      /// Get a paged list of contentitems published before now
      /// This is used for the archive template.
      /// </summary>
      /// <param name="site"></param>
      /// <param name="year"></param>
      /// <param name="month"></param>
      /// <param name="day"></param>
      /// <param name="pageSize"></param>
      /// <returns></returns>
      Paginator<T> GetPaginatorForPublishedBySiteAndPublishedDate(Site site, int year, int? month, int? day, int pageSize);



      /// <summary>
      /// Return a list of the dates for all published items
      /// </summary>
      /// <param name="site"></param>
      /// <returns></returns>
      IList<ContentItemCalendarDTO> GetPostCalendarForPublishedBySite(Site site);



      /// <summary>
      /// Get a list of the most 15 recent items
      /// </summary>
      /// <param name="site"></param>
      /// <returns></returns>
      IList<T> GetRecentItems(Site site);



      /// <summary>
      /// Get the recent published items ordered from the most recent to the older
      /// </summary>
      /// <param name="site"></param>
      /// <param name="maxEntries"></param>
      /// <returns></returns>
      IList<T> FindSyndicatedBySite(Site site, int maxEntries);

      #endregion


      /// <summary>
		/// Gets a single content item by its primary key.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		T GetById(int id);

		/// <summary>
		/// Gets a single content item by its unique identifier.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		T GetById(Guid id);


		/// <summary>
		/// Find ContentItems by site.
		/// </summary>
		/// <param name="site"></param>
		/// <returns></returns>
		IList<T> FindAllBySite(Site site);

      IList<T> FindAllBySite(Site site, string orderBy, bool orderAscending);


      Paginator<T> GetPaginatorBySite(Site site, int pageSize);

      Paginator<T> GetPaginatorBySite(Site site, int pageSize, string orderBy, bool orderAscending);

      Paginator<T> GetPaginatorBySiteAndWorkflowStatus(Site site, WorkflowStatus status, int pageSize);

      Paginator<T> GetPaginatorBySiteAndWorkflowStatus(Site site, WorkflowStatus status, int pageSize, string orderBy, bool orderAscending);

      Paginator<T> GetPaginatorBySiteAndWorkflowStatusAndAuthor(Site site, WorkflowStatus status, User author, int pageSize);







      /// <summary>
      /// Get a list of quick statistics about the conten items of a given site
      /// </summary>
      /// <param name="site"></param>
      /// <returns></returns>
	   ContentItemStatsDTO GetStatsBySite(Site site);


      /// <summary>
      /// Find currently visible ContentItems for a given category.
      /// </summary>
      /// <param name="category"></param>
      /// <param name="querySettings"></param>
      /// <returns></returns>
      IList<T> FindVisibleContentItemsByCategory(Category category, ContentItemQuerySettings querySettings);



      /// <summary>
      /// Find the syndicated content items for a given category.
      /// </summary>
      /// <param name="category"></param>
      /// <param name="querySettings"></param>
      /// <returns></returns>
      IList<T> FindSyndicatedByCategory(Category category, ContentItemQuerySettings querySettings);

      
		/// <summary>
		/// Save a content item in the database.
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		T Save(T entity);

		/// <summary>
		/// Delete a content item from the database.
		/// </summary>
		/// <param name="entity"></param>
		void Delete(T entity);



      /// <summary>
      /// Returns the number of posts with a similar friendlyname.
      /// This is used by the Save method to ensure that each post have a unique friendlyname
      /// </summary>
      /// <param name="site"></param>
      /// <param name="friendlyName"></param>
      /// <returns></returns>
	   long GetCountForSimilarFriendlyNameBySite(Site site, string friendlyName);


      IEnumerable<T> ConvertSearchResultCollectionToContentItemList(SearchResultCollection results);




   }
}