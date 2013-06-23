using System.Collections.Generic;
using NHibernate;
using uNhAddIns.Pagination;

namespace Arashi.Core.Repositories
{
   using global::NHibernate.Criterion;

   public interface IRepository<T>
   {
      void Flush();
      void Create(T instance);
      void Delete(T instance);
      T FindById(object id);
      T FindById(object id, bool allowNull);

      /// <summary>
      /// Register te entity for insert/save in the database when the unit of work
      /// is completed. (SAVE OR UPDATE)
      /// </summary>
      /// <param name="instance"></param>
      T Save(T instance);

      /// <summary>
      /// Register the entity for update in the database when the unit of work
      /// is completed. (UPDATE)
      /// </summary>
      /// <param name="instance"></param>
      void Update(T instance);

      IList<T> FindAll();
      IList<T> FindAll(Order order, params ICriterion[] criteria);
      IList<T> FindAll(Order[] orders, params ICriterion[] criteria);
      IList<T> FindAll(params ICriterion[] criteria);
      IList<T> FindAll(int firstResult, int numberOfResults, params ICriterion[] criteria);
      IList<T> FindAll(int firstResult, int numberOfResults, Order selectionOrder, params ICriterion[] criteria);
      IList<T> FindAll(int firstResult, int numberOfResults, Order[] selectionOrder, params ICriterion[] criteria);
      IList<T> FindAll(int firstRow, int maxRows);
      IList<T> FindAll(DetachedCriteria criteria, params Order[] orders);
      IList<T> FindAll(DetachedCriteria criteria, int firstResult, int maxResults, params Order[] orders);

      IList<T> FindAllWithCustomQuery(string queryString);
      IList<T> FindAllWithCustomQuery(string queryString, int firstRow, int maxRows);
      IList<T> FindAllWithNamedQuery(string namedQuery);
      IList<T> FindAllWithNamedQuery(string namedQuery, int firstRow, int maxRows);

      T FindFirst(params ICriterion[] criterias);
      T FindFirst(DetachedCriteria criteria);
      T FindFirst(DetachedCriteria criteria, Order[] orders);
      T FindFirst(Order[] orders);

      T FindOne(params ICriterion[] criteria);
      T FindOne(DetachedCriteria criteria);
      T FindOne(string namedQuery);

      bool Exists(DetachedCriteria criteria);
      bool Exists();

      long Count(DetachedCriteria criteria);
      long Count();

      Paginator<T> GetPaginator(IDetachedQuery detachedQuery, int pageSize);
      Paginator<T> GetPaginator(DetachedCriteria criteria, int pageSize);
      Paginator<T> GetPaginator(IDetachedQuery detachedQuery, IDetachedQuery detachedQueryForRowCounter, int pageSize);
      IRowsCounter GetRowCounter(IDetachedQuery dq);


      /// <summary>
      /// Elimina una data istanza di T dalla SESSION Cache (Evict)
      /// </summary>
      /// <param name="entity"></param>
      void ClearSessionCache(T entity);

      /// <summary>
      /// Elimina tutte le istanze di T dalla cache di SECONDO livello
      /// </summary>
      void ClearSecondLevelCache();

      /// <summary>
      /// Elimina una data istanza di T dalla cache di SECONDO livello
      /// </summary>
      /// <param name="objectId"></param>
      void ClearSecondLevelCache(int objectId);
   }
}