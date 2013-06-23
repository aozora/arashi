using System.Collections.Generic;
using NHibernate;
using uNhAddIns.Pagination;

namespace Arashi.Core.Repositories
{
   using global::NHibernate.Criterion;

   public static class Repository<T>
   {
      private static IRepository<T> internalRepository
      {
         get
         {
            // vedi http://ayende.com/Blog/archive/2006/07/30/7390.aspx
            return IoC.Resolve<IRepository<T>>();
         }
      }


      #region Methods

      public static void Flush()
      {
         internalRepository.Flush();
      }



      /// <summary>
      /// Elimina una data istanza di T dalla SESSION Cache
      /// </summary>
      /// <param name="entity"></param>
      public static void ClearSessionCache(T entity)
      {
         internalRepository.ClearSessionCache(entity);
      }



      /// <summary>
      /// Clear the cache for a given type.
      /// </summary>
      /// <param name="type"></param>
      public static void ClearSecondLevelCache()
      {
         internalRepository.ClearSecondLevelCache();
      }


      /// <summary>
      /// Clear the cache for a given type.
      /// </summary>
      /// <param name="type"></param>
      public static void ClearSecondLevelCache(int objectId)
      {
         internalRepository.ClearSecondLevelCache(objectId);
      }



      /// <summary>
      /// Register the entity for save in the database when the unit of work
      /// is completed. (INSERT)
      /// </summary>
      /// <param name="entity"></param>
      public static void Create(T entity)
      {
         internalRepository.Create(entity);
      }



      /// <summary>
      /// Register the entity for deletion when the unit of work
      /// is completed. 
      /// </summary>
      /// <param name="entity"></param>
      public static void Delete(T entity)
      {
         internalRepository.Delete(entity);
      }

      #region FindAll overloads

      /// <summary>
      /// Loads all the entities
      /// </summary>
      /// <returns></returns>
      public static IList<T> FindAll()
      {
         return internalRepository.FindAll();
      }


      public static IList<T> FindAll(Order order, params ICriterion[] criteria)
      {
         return internalRepository.FindAll(order, criteria);
      }



      public static IList<T> FindAll(Order[] orders, params ICriterion[] criteria)
      {
         return internalRepository.FindAll(orders, criteria);
      }


      public static IList<T> FindAll(params ICriterion[] criteria)
      {
         return internalRepository.FindAll(criteria);
      }



      public static IList<T> FindAll(int firstResult, int numberOfResults, params ICriterion[] criteria)
      {
         return internalRepository.FindAll(firstResult, numberOfResults, criteria);
      }



      public static IList<T> FindAll(int firstResult, int numberOfResults, Order selectionOrder, params ICriterion[] criteria)
      {
         return internalRepository.FindAll(firstResult, numberOfResults, selectionOrder, criteria);
      }



      public static IList<T> FindAll(int firstResult,
                                     int numberOfResults,
                                     Order[] selectionOrder,
                                     params ICriterion[] criteria)
      {
         return internalRepository.FindAll(firstResult, numberOfResults, selectionOrder, criteria);
      }



      /// <summary>
      /// Loads all the entities that match the criteria
      /// </summary>
      /// <param name="firstRow"></param>
      /// <param name="maxRows"></param>
      /// <returns></returns>
      public static IList<T> FindAll(int firstRow, int maxRows)
      {
         return internalRepository.FindAll(firstRow, maxRows);
      }

      #endregion

      #region FindAll with DetachedCriteria

      public static IList<T> FindAll(DetachedCriteria criteria, params Order[] orders)
      {
         return internalRepository.FindAll(criteria, orders);
      }



      public static IList<T> FindAll(DetachedCriteria criteria, int firstResult, int maxResults, params Order[] orders)
      {
         return internalRepository.FindAll(criteria, firstResult, maxResults, orders);
      }

      #endregion

      #region FindFirst

      public static T FindFirst(params ICriterion[] criterias)
      {
         return internalRepository.FindFirst(criterias);
      }

      /// <summary>
      /// Find the entity based on a criteria.
      /// </summary>
      /// <param name="criteria"></param>
      /// <param name="orders"></param>
      /// <returns></returns>
      public static T FindFirst(DetachedCriteria criteria)
      {
         return internalRepository.FindFirst(criteria);
      }


      /// <summary>
      /// Find the entity based on a criteria.
      /// </summary>
      /// <param name="criteria"></param>
      /// <param name="orders"></param>
      /// <returns></returns>
      public static T FindFirst(DetachedCriteria criteria, Order[] orders)
      {
         return internalRepository.FindFirst(criteria, orders);
      }



      /// <summary>
      /// Find the entity based on a criteria.
      /// </summary>
      /// <param name="orders"></param>
      /// <returns></returns>
      public static T FindFirst(Order[] orders)
      {
         return FindFirst(orders);
      }

      #endregion

      #region FindOne

      /// <summary>
      /// Find a single entity based on a criteria.
      /// Thorws is there is more than one result.
      /// </summary>
      /// <param name="criterias"></param>
      /// <returns></returns>
      public static T FindOne(params ICriterion[] criterias)
      {
         return internalRepository.FindOne(criterias);
      }



      /// <summary>
      /// Find a single entity based on a criteria.
      /// Thorws is there is more than one result.
      /// </summary>
      /// <param name="criteria"></param>
      /// <returns></returns>
      public static T FindOne(DetachedCriteria criteria)
      {
         return internalRepository.FindOne(criteria);
      }



      /// <summary>
      /// Find a single entity based on a criteria.
      /// Thorws is there is more than one result.
      /// </summary>
      /// <param name="namedQuery"></param>
      /// <returns></returns>
      public static T FindOne(string namedQuery)
      {
         return internalRepository.FindOne(namedQuery);
      }

      #endregion

      #region Exists

      /// <summary>
      /// Check if any instance matches the criteria.
      /// </summary>
      /// <returns><c>true</c> if an instance is found; otherwise <c>false</c>.</returns>
      public static bool Exists(DetachedCriteria criteria)
      {
         return internalRepository.Exists(criteria);
      }



      /// <summary>
      /// Check if any instance of the type exists
      /// </summary>
      /// <returns><c>true</c> if an instance is found; otherwise <c>false</c>.</returns>
      public static bool Exists()
      {
         return internalRepository.Exists();
      }

      #endregion

      #region Count

      /// <summary>
      /// Counts the number of instances matching the criteria.
      /// </summary>
      /// <param name="criteria"></param>
      /// <returns></returns>
      public static long Count(DetachedCriteria criteria)
      {
         return internalRepository.Count(criteria);
      }



      /// <summary>
      /// Counts the overall number of instances.
      /// </summary>
      /// <returns></returns>
      public static long Count()
      {
         return internalRepository.Count();
      }

      #endregion

      /// <summary>
      /// Loads all the entities that match the given query
      /// </summary>
      /// <param name="queryString"></param>
      /// <returns></returns>
      public static IList<T> FindAllWithCustomQuery(string queryString)
      {
         return internalRepository.FindAllWithCustomQuery(queryString);
      }



      /// <summary>
      /// Loads all the entities that match the given query
      /// </summary>
      /// <param name="queryString"></param>
      /// <param name="firstRow"></param>
      /// <param name="maxRows"></param>
      /// <returns></returns>
      public static IList<T> FindAllWithCustomQuery(string queryString, int firstRow, int maxRows)
      {
         return internalRepository.FindAllWithCustomQuery(queryString, firstRow, maxRows);
      }



      /// <summary>
      /// Loads all the entities that match the named query
      /// </summary>
      /// <param name="namedQuery"></param>
      /// <returns></returns>
      public static IList<T> FindAllWithNamedQuery(string namedQuery)
      {
         return internalRepository.FindAllWithNamedQuery(namedQuery);
      }



      /// <summary>
      /// Loads all the entities that match the named query
      /// </summary>
      /// <param name="namedQuery"></param>
      /// <param name="firstRow"></param>
      /// <param name="maxRows"></param>
      /// <returns></returns>
      public static IList<T> FindAllWithNamedQuery(string namedQuery, int firstRow, int maxRows)
      {
         return internalRepository.FindAllWithNamedQuery(namedQuery, firstRow, maxRows);
      }



      /// <summary>
      /// Loads one entities that match the given id
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
      public static T FindById(object id)
      {
         return internalRepository.FindById(id);
      }



      public static T FindById(object id, bool allowNull)
      {
         return FindById(id, allowNull);
      }


      /// <summary>
      /// Register te entity for insert/save in the database when the unit of work
      /// is completed. (INSERT OR UPDATE)
      /// </summary>
      /// <param name="entity"></param>
      public static void Save(T entity)
      {
         internalRepository.Save(entity);
      }



      /// <summary>
      /// Register the entity for update in the database when the unit of work
      /// is completed. (UPDATE)
      /// </summary>
      /// <param name="entity"></param>
      public static void Update(T entity)
      {
         internalRepository.Update(entity);
      }

      #endregion

      public static Paginator<T> GetPaginator(IDetachedQuery detachedQuery, int pageSize)
      {
         return internalRepository.GetPaginator(detachedQuery, pageSize);
      }


      public static Paginator<T> GetPaginator(DetachedCriteria criteria, int pageSize)
      {
         return internalRepository.GetPaginator(criteria, pageSize);
      }


      public static Paginator<T> GetPaginator(IDetachedQuery detachedQuery, 
                                              IDetachedQuery detachedQueryForRowCounter, 
                                              int pageSize)
      {
         return internalRepository.GetPaginator(detachedQuery, detachedQueryForRowCounter, pageSize);
      }


      public static IRowsCounter GetRowCounter(IDetachedQuery dq)
      {
         return internalRepository.GetRowCounter(dq);
      }

   }
}