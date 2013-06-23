using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Arashi.Core.Domain;
using Arashi.Core.Domain.Dto;
using Arashi.Core.NHibernate;
using Arashi.Services.Search;
using Arashi.Core.Repositories;
using log4net;
using NHibernate;
using NHibernate.Criterion;
using uNhAddIns.Pagination;
using uNhAddIns.Transform;

namespace Arashi.Services.Content
{
   public class ContentItemService<T> : IContentItemService<T> where T : IContentItem
   {
      private static readonly ILog log = LogManager.GetLogger(typeof(ContentItemService<T>));
      protected IContentItemDao<T> contentItemDao;


		public ContentItemService(IContentItemDao<T> contentItemDao)
		{
			this.contentItemDao = contentItemDao;
		}



      #region Methods used exclusively by the Content Controllers


      public T GetPublishedByFriendlyName(Site site, string friendlyName)
      {
         DetachedCriteria criteria = DetachedCriteria.For<T>()
                                       .Add(Restrictions.Eq("FriendlyName", friendlyName))
                                       .Add(Restrictions.Eq("IsLogicallyDeleted", false))
                                       .Add(Restrictions.Eq("Site", site))
                                       .Add(Restrictions.Eq("WorkflowStatus", WorkflowStatus.Published))
                                       .Add(Restrictions.Le("PublishedDate", DateTime.Now.ToUniversalTime()));

         ICriteria c = criteria.GetExecutableCriteria(RepositoryHelper.GetSession());

         return c.UniqueResult<T>();
      }



      public T GetPublishedByPublishedDateAndFriendlyName(Site site, int year, int month, int day, string friendlyName)
      {
         DetachedCriteria criteria = DetachedCriteria.For<T>()
                                       .Add(Restrictions.Eq("FriendlyName", friendlyName))
                                       .Add(Restrictions.Eq("IsLogicallyDeleted", false))
                                       .Add(Restrictions.Eq("Site", site))
                                       .Add(Restrictions.Eq("WorkflowStatus", WorkflowStatus.Published))
                                       .Add(Restrictions.Le("PublishedDate", DateTime.Now.ToUniversalTime()))
                                       .Add(Expression.Sql("YEAR(PublishedDate) = ?", year, NHibernateUtil.Int32))
                                       .Add(Expression.Sql("MONTH(PublishedDate) = ?", month, NHibernateUtil.Int32))
                                       .Add(Expression.Sql("DAY(PublishedDate) = ?", day, NHibernateUtil.Int32));

         ICriteria c = criteria.GetExecutableCriteria(RepositoryHelper.GetSession());

         return c.UniqueResult<T>();
      }



      public Paginator<T> GetPaginatorForPublishedBySiteAndWorkflowStatus(Site site, WorkflowStatus status, int pageSize)
      {
         if (site == null)
            throw new ArgumentNullException("site");

         DetachedCriteria criteria = DetachedCriteria.For<T>()
                                       .AddOrder(new Order("PublishedDate", false))
                                       .Add(Restrictions.Eq("WorkflowStatus", status))
                                       .Add(Restrictions.Le("PublishedDate", DateTime.Now.ToUniversalTime()))
                                       .Add(Restrictions.Eq("IsLogicallyDeleted", false))
                                       .Add(Restrictions.Eq("Site", site));

         return Repository<T>.GetPaginator(criteria, pageSize);
      }



      public Paginator<T> GetPaginatorForPublishedBySiteAndCategory(Site site, Category category, int pageSize)
      {
         if (site == null)
            throw new ArgumentNullException("site");

         DetachedCriteria criteria = DetachedCriteria.For<T>()
                                       .AddOrder(new Order("PublishedDate", false))
                                       .CreateAlias("Categories", "cat")
                                       .Add(Restrictions.Eq("WorkflowStatus", WorkflowStatus.Published))
                                       .Add(Restrictions.Le("PublishedDate", DateTime.Now.ToUniversalTime()))
                                       .Add(Restrictions.Eq("cat.Id", category.Id))
                                       .Add(Restrictions.Eq("IsLogicallyDeleted", false))
                                       .Add(Restrictions.Eq("Site", site));

         return Repository<T>.GetPaginator(criteria, pageSize);
      }



      public Paginator<T> GetPaginatorForPublishedBySiteAndTag(Site site, Tag tag, int pageSize)
      {
         if (site == null)
            throw new ArgumentNullException("site");

         DetachedCriteria criteria = DetachedCriteria.For<T>()
                                       .AddOrder(new Order("PublishedDate", false))
                                       .CreateAlias("Tags", "t")
                                       .Add(Restrictions.Eq("WorkflowStatus", WorkflowStatus.Published))
                                       .Add(Restrictions.Le("PublishedDate", DateTime.Now.ToUniversalTime()))
                                       .Add(Restrictions.Eq("t.TagId", tag.TagId))
                                       .Add(Restrictions.Eq("IsLogicallyDeleted", false))
                                       .Add(Restrictions.Eq("Site", site));

         return Repository<T>.GetPaginator(criteria, pageSize);
      }



      public Paginator<T> GetPaginatorForPublishedBySiteAndPublishedDate(Site site, int year, int? month, int? day, int pageSize)
      {
         if (site == null)
            throw new ArgumentNullException("site");

         DetachedCriteria criteria = DetachedCriteria.For<T>()
                                       .AddOrder(new Order("PublishedDate", false))
                                       .Add(Expression.Sql("YEAR(PublishedDate) = ?", year, NHibernateUtil.Int32))
                                       .Add(Restrictions.Eq("WorkflowStatus", WorkflowStatus.Published))
                                       .Add(Restrictions.Le("PublishedDate", DateTime.Now.ToUniversalTime()))
                                       .Add(Restrictions.Eq("IsLogicallyDeleted", false))
                                       .Add(Restrictions.Eq("Site", site));
         if (month.HasValue)
            criteria.Add(Expression.Sql("MONTH(PublishedDate) = ?", month, NHibernateUtil.Int32));

         if (day.HasValue)
            criteria.Add(Expression.Sql("DAY(PublishedDate) = ?", day, NHibernateUtil.Int32));

         return Repository<T>.GetPaginator(criteria, pageSize);
      }



      public IList<ContentItemCalendarDTO> GetPostCalendarForPublishedBySite(Site site)
      {
         if (site == null)
            throw new ArgumentNullException("site");

         var transformer = new PositionalToBeanResultTransformer(typeof(ContentItemCalendarDTO), new[] { "Year", "Month", "Day", "Count" });

         IList<ContentItemCalendarDTO> list = RepositoryHelper.GetSession().GetNamedQuery("GetPostCalendarForPublishedBySite")
                                                   .SetEntity("site", site)
                                                   .SetDateTime("now", DateTime.Now.ToUniversalTime())
                                                   .SetResultTransformer(transformer)
                                                   .List<ContentItemCalendarDTO>();

         return list;
      }



      public IList<T> FindSyndicatedBySite(Site site, int maxEntries)
      {
         DetachedCriteria criteria = DetachedCriteria.For<T>()
                                       .AddOrder(new Order("PublishedDate", false))
                                       .Add(Restrictions.Eq("WorkflowStatus", WorkflowStatus.Published))
                                       .Add(Restrictions.Le("PublishedDate", DateTime.Now.ToUniversalTime()))
                                       .Add(Restrictions.Eq("IsLogicallyDeleted", false))
                                       .Add(Restrictions.Eq("Site", site))
                                       .SetMaxResults(maxEntries);

         ICriteria c = criteria.GetExecutableCriteria(RepositoryHelper.GetSession());

         return c.List<T>();
      }



      /// <summary>
      /// Get a list of the most 15 recent items, calculated as the items with the higher count of comments
      /// </summary>
      /// <param name="site"></param>
      /// <returns></returns>
      public IList<T> GetRecentItems(Site site)
      {
         //return new List<T>();

         //select top 15 cc.*
         //from cms_ContentItems cc
         //where cc.ContentItemId IN (
         //   select 
         //      --count(CommentId) as c,
         //      comments1_.ContentItemId
               
         //   FROM dbo.cms_Posts this_ 
         //      inner join dbo.cms_ContentItems this_1_ on this_.ContentItemId=this_1_.ContentItemid 
         //      inner join dbo.cms_Comments comments1_ on this_.ContentItemId=comments1_.ContentItemId 

         //   group by comments1_.ContentItemId
         //   --order by count(comments1_.CommentId)
         //)

         DetachedCriteria subquery = DetachedCriteria.For<T>()
                                       .Add(Restrictions.Eq("IsLogicallyDeleted", false))
                                       .Add(Restrictions.Eq("Site", site))
                                       .Add(Restrictions.Eq("WorkflowStatus", WorkflowStatus.Published))
                                       .Add(Restrictions.Le("PublishedDate", DateTime.Now.ToUniversalTime()))
                                       .CreateAlias("Comments", "comments")
                                       .SetProjection(Projections.ProjectionList()
                                          .Add(Projections.GroupProperty("Id"))
                                          //.Add(Projections.Count("comments.CommentId"), "comments-count")
                                       );


         DetachedCriteria criteria = DetachedCriteria.For<T>()
                                       .Add(Subqueries.PropertyIn("Id", subquery))
                                       .SetMaxResults(15);

         ICriteria c = criteria.GetExecutableCriteria(RepositoryHelper.GetSession());
         return c.List<T>();
      }

      #endregion





      public T GetById(int id)
		{
         return (T)RepositoryHelper.GetSession().Get<T>(id);
      }

		public T GetById(Guid id)
		{
         return (T)RepositoryHelper.GetSession().Get<T>(id);
      }




		/// <summary>
		/// Find all content items for the given site.
		/// </summary>
		/// <param name="site"></param>
		/// <returns></returns>
		public IList<T> FindAllBySite(Site site)
		{
         DetachedCriteria criteria = DetachedCriteria.For<T>()
                                       .Add(Restrictions.Eq("IsLogicallyDeleted", false))
                                       .Add(Restrictions.Eq("Site", site));

         return this.contentItemDao.GetByCriteria(criteria);
		}


      public IList<T> FindAllBySite(Site site, string orderBy, bool orderAscending)
		{
         DetachedCriteria criteria = DetachedCriteria.For<T>()
                                      .Add(Restrictions.Eq("IsLogicallyDeleted", false))
                                      .AddOrder(new Order(orderBy, orderAscending))
                                      .Add(Restrictions.Eq("Site", site));

         return this.contentItemDao.GetByCriteria(criteria);
		}



      /// <summary>
      /// Get a paged list of content items of T by the given site
      /// </summary>
      /// <param name="site"></param>
      /// <param name="pageSize"></param>
      /// <returns></returns>
      public Paginator<T> GetPaginatorBySite(Site site, int pageSize)
      {
         if (site == null)
            throw new ArgumentNullException("site");

         DetachedCriteria criteria = DetachedCriteria.For<T>()
                                       .AddOrder(new Order("PublishedDate", false))
                                       .Add(Restrictions.Eq("IsLogicallyDeleted", false))
                                       .Add(Restrictions.Eq("Site", site));

         return Repository<T>.GetPaginator(criteria, pageSize);
      }




      public Paginator<T> GetPaginatorBySite(Site site, int pageSize, string orderBy, bool orderAscending)
      {
         if (site == null)
            throw new ArgumentNullException("site");

         DetachedCriteria criteria = DetachedCriteria.For<T>()
                                       .AddOrder(new Order(orderBy, orderAscending))
                                       .Add(Restrictions.Eq("IsLogicallyDeleted", false))
                                       .Add(Restrictions.Eq("Site", site));

         return Repository<T>.GetPaginator(criteria, pageSize);
      }




      public Paginator<T> GetPaginatorBySiteAndWorkflowStatus(Site site, WorkflowStatus status, int pageSize)
      {
         if (site == null)
            throw new ArgumentNullException("site");

         DetachedCriteria criteria = DetachedCriteria.For<T>()
                                       .AddOrder(new Order("PublishedDate", false))
                                       .Add(Restrictions.Eq("WorkflowStatus", status))
                                       .Add(Restrictions.Eq("IsLogicallyDeleted", false))
                                       .Add(Restrictions.Eq("Site", site));

         return Repository<T>.GetPaginator(criteria, pageSize);
      }



      public Paginator<T> GetPaginatorBySiteAndWorkflowStatus(Site site, WorkflowStatus status, int pageSize, string orderBy, bool orderAscending)
      {
         if (site == null)
            throw new ArgumentNullException("site");

         DetachedCriteria criteria = DetachedCriteria.For<T>()
                                       .AddOrder(new Order(orderBy, orderAscending))
                                       .Add(Restrictions.Eq("WorkflowStatus", status))
                                       .Add(Restrictions.Eq("IsLogicallyDeleted", false))
                                       .Add(Restrictions.Eq("Site", site));

         return Repository<T>.GetPaginator(criteria, pageSize);
      }




      public Paginator<T> GetPaginatorBySiteAndWorkflowStatusAndAuthor(Site site, WorkflowStatus status, User author, int pageSize)
      {
         if (site == null)
            throw new ArgumentNullException("site");
         
         DetachedCriteria criteria = DetachedCriteria.For<T>()
                                       .AddOrder(new Order("UpdatedDate", false))
                                       .Add(Restrictions.Eq("WorkflowStatus", status))
                                       .Add(Restrictions.Eq("Author", author))
                                       .Add(Restrictions.Eq("IsLogicallyDeleted", false))
                                       .Add(Restrictions.Eq("Site", site));

         return Repository<T>.GetPaginator(criteria, pageSize);
      }





      public ContentItemStatsDTO GetStatsBySite(Site site)
      {
         if (site == null)
            throw new ArgumentNullException("site");

         ISession session = RepositoryHelper.GetSession();

         var postTransformer = new PositionalToBeanResultTransformer(typeof(ContentItemsCountByWorkflowStatus<Post>), new[] { "Count", "Status" });
         var pageTransformer = new PositionalToBeanResultTransformer(typeof(ContentItemsCountByWorkflowStatus<Page>), new[] { "Count", "Status" });
         var commentTransformer = new PositionalToBeanResultTransformer(typeof(CommentsCountByWorkflowStatus), new[] { "Count", "Status" });

         IMultiQuery multiQuery = session.CreateMultiQuery()
                                    .Add(session.GetNamedQuery("GetStatsBySite_Posts")
                                       .SetResultTransformer(postTransformer)
                                    )
                                    .Add(session.GetNamedQuery("GetStatsBySite_Pages")
                                       .SetResultTransformer(pageTransformer)
                                    )
                                    .Add(session.GetNamedQuery("GetStatsBySite_Comments")
                                       .SetResultTransformer(commentTransformer)
                                    )
                                    .Add(session.GetNamedQuery("GetStatsBySite_Categories"))
                                    .Add(session.GetNamedQuery("GetStatsBySite_Tags"))
                                    .SetEntity("site", site)
                                    .SetCacheable(true);

         // Manage results
         IList results = multiQuery.List();
         
         // Posts stat
         IList postResults = (IList) results[0];
         IList<ContentItemsCountByWorkflowStatus<Post>> postStats = new List<ContentItemsCountByWorkflowStatus<Post>>();

         for (int index = 0; index < ((IList)results[0]).Count; index++)
         {
            postStats.Add((ContentItemsCountByWorkflowStatus<Post>)((IList)results[0])[index]);
         }


         // Pages Stats
         IList pageResults = (IList)results[1];
         IList<ContentItemsCountByWorkflowStatus<Page>> pageStats = new List<ContentItemsCountByWorkflowStatus<Page>>();

         for (int index = 0; index < ((IList)results[1]).Count; index++)
         {
            pageStats.Add((ContentItemsCountByWorkflowStatus<Page>)((IList)results[1])[index]);
         }


         // comments stat
         IList commentResults = (IList)results[2];
         IList<CommentsCountByWorkflowStatus> commentStats = new List<CommentsCountByWorkflowStatus>();

         for (int index = 0; index < ((IList)results[2]).Count; index++)
         {
            commentStats.Add((CommentsCountByWorkflowStatus)((IList)results[2])[index]);
         }

         long categoryStat = (long)((IList)results[3])[0];
         long tagStat = (long)((IList)results[4])[0];


         // put all in the DTO
         ContentItemStatsDTO stat = new ContentItemStatsDTO
         {
            Site = site,
            PostCountByWorkflowStatus = postStats,
            PageCountByWorkflowStatus = pageStats,
            CommentsCountByStatus = commentStats,
            CategoriesTotalCount = categoryStat,
            TagsTotalCount = tagStat
         };

         return stat;
      }



      /// <summary>
      /// Find currently visible ContentItems for a given category.
      /// </summary>
      /// <param name="category"></param>
      /// <param name="querySettings"></param>
      /// <returns></returns>
      public IList<T> FindVisibleContentItemsByCategory(Category category, ContentItemQuerySettings querySettings)
      {
         DetachedCriteria criteria = GetCriteriaForCategory(category, querySettings)
                                       .Add(Restrictions.Lt("PublishedDate", DateTime.Now))
                                       .Add(Restrictions.Or(Restrictions.IsNull("PublishedUntil"), Restrictions.Gt("PublishedUntil", DateTime.Now)));
         return this.contentItemDao.GetByCriteria(criteria);
      }



      /// <summary>
      /// Find the syndicated content items for a given category.
      /// </summary>
      /// <param name="category"></param>
      /// <param name="querySettings"></param>
      /// <returns></returns>
      public IList<T> FindSyndicatedByCategory(Category category, ContentItemQuerySettings querySettings)
      {
         DetachedCriteria criteria = GetCriteriaForCategory(category, querySettings)
                                       .Add(Restrictions.Eq("Syndicate", true));
         return this.contentItemDao.GetByCriteria(criteria);
      }




      /// <summary>
      /// Save a <see cref="ContentItem"/>. 
      /// Also check if the FriendlyName is unique, otherwise add a counter suffix
      /// </summary>
      /// <param name="entity"></param>
      /// <returns></returns>
		public T Save(T entity)
		{
         log.Debug("ContentItemService.Save: start");

         // Check the culture: if null assign the site default
         if (string.IsNullOrEmpty(entity.Culture))
            entity.Culture = entity.Site.DefaultCulture;

         // Check if the FriendlyName is unique
		   long similarCount = GetCountForSimilarFriendlyNameBySite(entity.Site, entity.FriendlyName);

         // if the entity already exists, than similarCount will be "at leat" 1...
         if ( (entity.IsNew && similarCount > 0) || !entity.IsNew && similarCount > 1 )
            entity.FriendlyName = entity.FriendlyName + "-" + (similarCount + 1).ToString();

         log.Debug("ContentItemService.Save: end");
         return this.contentItemDao.Save(entity);
		}



		public void Delete(T entity)
		{
			this.contentItemDao.Delete(entity);
      }



      public long GetCountForSimilarFriendlyNameBySite(Site site, string friendlyName)
      {
         DetachedCriteria criteria = DetachedCriteria.For<T>()
                                       .Add(Restrictions.Eq("IsLogicallyDeleted", false))
                                       .Add(Restrictions.Eq("Site", site))
                                       .Add(Restrictions.Like("FriendlyName", friendlyName, MatchMode.Start))
                                       .SetProjection(Projections.Count("Id"));
            
         ICriteria c = criteria.GetExecutableCriteria(RepositoryHelper.GetSession());

         return Convert.ToInt64(c.UniqueResult());
      }



      public IList<T> ConvertSearchResultCollectionToContentItemList(SearchResultCollection results)
      {
         IList<T> list = new List<T>();

         foreach (SearchResult result in results)
         {
            // add unique!!!
            SearchResult tmp = result;
            if (list.Count(c => c.Id == tmp.ContentItemId) == 0)
               list.Add(this.GetById(result.ContentItemId));
         }

         return list;
      }





      #region Private Helpers

      //private DetachedCriteria GetCriteriaForSection(Section section, ContentItemQuerySettings querySettings)
      //{
      //   return GetContentItemCriteria()
      //      .Add(Restrictions.Eq("Section", section))
      //      .ApplyOrdering(querySettings.SortBy, querySettings.SortDirection)
      //      .ApplyPaging(querySettings.PageSize, querySettings.PageNumber);
      //}

      private DetachedCriteria GetCriteriaForCategory(Category category, ContentItemQuerySettings querySettings)
      {
         return GetContentItemCriteria()
                  .Add(Restrictions.Eq("IsLogicallyDeleted", false))
                  .CreateAlias("Categories", "cat")
                  .Add(Restrictions.Eq("cat.Id", category.Id))
                  .ApplyOrdering(querySettings.SortBy, querySettings.SortDirection)
                  .ApplyPaging(querySettings.PageSize, querySettings.PageNumber);
      }


      private DetachedCriteria GetCriteriaForSite(Site site, ContentItemQuerySettings querySettings)
      {
         return GetContentItemCriteria()
                  .Add(Restrictions.Eq("IsLogicallyDeleted", false))
                  .CreateAlias("Site", "s")
                  .Add(Restrictions.Eq("s", site))
                  .ApplyOrdering(querySettings.SortBy, querySettings.SortDirection)
                  .ApplyPaging(querySettings.PageSize, querySettings.PageNumber);
      }


      private DetachedCriteria GetContentItemCriteria()
      {
         return DetachedCriteria.For(typeof(T));
      }

      #endregion

   }
}