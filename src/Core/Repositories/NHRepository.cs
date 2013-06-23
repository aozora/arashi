namespace Arashi.Core.Repositories
{
   using System;
   using System.Collections.Generic;

   using global::NHibernate;
   using global::NHibernate.Criterion;
   using global::NHibernate.Impl;

   using uNhAddIns.GenericImpl;
   using uNhAddIns.Pagination;

   /// <summary>
   /// Repository for NHibernate
   /// This class is the same as in Castle.Facilities.NHibernateIntegration
   /// but with more methods like in Rhino.Commons.Repositories.NHRepository
   /// </summary>
   /// <typeparam name="T"></typeparam>
   public class NHRepository<T> : IRepository<T>
   {
      // Fields
      //private readonly ISessionManager sessionManager;
      private readonly Arashi.Core.NHibernate.ISessionFactory sessionFactory;



      #region Constructors

      public NHRepository(Arashi.Core.NHibernate.ISessionFactory sessionFactory /*ISessionManager sessionManager*/)
      {
         //this.sessionManager = sessionManager;
         this.sessionFactory = sessionFactory;
      }

      #endregion

      #region GetSession

      private ISession GetSession()
      {
         //return this.sessionManager.OpenSession();
         return this.sessionFactory.GetSession();
      }

      #endregion

      #region Methods

      public virtual void Flush()
      {
         ISession session = GetSession();
         session.Flush();
      }


      /// <summary>
      /// Register te entity for save in the database when the unit of work
      /// is completed. (INSERT)
      /// </summary>
      /// <param name="entity"></param>
      public virtual void Create(T entity)
      {
         ISession session = GetSession();

         try
         {
            session.Save(entity);
         }
         catch (Exception exception)
         {
            throw new Arashi.Core.Exceptions.ApplicationException("Could not perform Create for " + entity.GetType().Name, exception);
         }

      }



      /// <summary>
      /// Register the entity for deletion when the unit of work
      /// is completed. 
      /// </summary>
      /// <param name="entity"></param>
      public virtual void Delete(T entity)
      {
         ISession session = GetSession();
         try
         {
            session.Delete(entity);
         }
         catch (Exception exception)
         {
            throw new Arashi.Core.Exceptions.ApplicationException("Could not perform Delete for " + entity.GetType().Name, exception);
         }
      }


      #region FindAll overloads

      /// <summary>
      /// Loads all the entities
      /// </summary>
      /// <returns></returns>
      public virtual IList<T> FindAll()
      {
         return FindAll(-2147483648, -2147483648);
      }


      public IList<T> FindAll(Order order, params ICriterion[] criteria)
      {
         ISession session = GetSession();

         ICriteria crit = CreateCriteriaFromArray(session, criteria);
         crit.AddOrder(order);
         return crit.List<T>();
      }



      public IList<T> FindAll(Order[] orders, params ICriterion[] criteria)
      {
         ISession session = GetSession();

         ICriteria crit = CreateCriteriaFromArray(session, criteria);
         foreach (Order order in orders)
         {
            crit.AddOrder(order);
         }
         return crit.List<T>();
      }



      public IList<T> FindAll(params ICriterion[] criteria)
      {
         ISession session = GetSession();
         ICriteria crit = CreateCriteriaFromArray(session, criteria);
         return crit.List<T>();
      }



      public IList<T> FindAll(int firstResult, int numberOfResults, params ICriterion[] criteria)
      {
         ISession session = GetSession();
         ICriteria crit = CreateCriteriaFromArray(session, criteria);
         crit.SetFirstResult(firstResult)
            .SetMaxResults(numberOfResults);
         return crit.List<T>();
      }



      public IList<T> FindAll(int firstResult, int numberOfResults, Order selectionOrder, params ICriterion[] criteria)
      {
         ISession session = GetSession();
         ICriteria crit = CreateCriteriaFromArray(session, criteria);
         crit.SetFirstResult(firstResult)
            .SetMaxResults(numberOfResults);
         crit.AddOrder(selectionOrder);
         return crit.List<T>();
      }





      ///// <summary>
      ///// Loads all the entities that match the criteria by order
      ///// </summary>
      ///// <param name="criterias"></param>
      ///// <param name="sortItems"></param>
      ///// <param name="firstRow"></param>
      ///// <param name="maxRows"></param>
      ///// <returns></returns>
      //public virtual IList<T> FindAll(int firstRow, int maxRows, Order[] sortItems, ICriterion[] criterias)
      //{
      //   using (ISession session = GetSession())
      //   {
      //      try
      //      {
      //         ICriteria criteria = session.CreateCriteria(typeof(T));

      //         if (criterias != null)
      //         {
      //            foreach (ICriterion criterion in criterias)
      //            {
      //               criteria.Add(criterion);
      //            }
      //         }

      //         if (sortItems != null)
      //         {
      //            foreach (Order order in sortItems)
      //            {
      //               criteria.AddOrder(order);
      //            }
      //         }

      //         if (firstRow != -2147483648)
      //         {
      //            criteria.SetFirstResult(firstRow);
      //         }
      //         if (maxRows != -2147483648)
      //         {
      //            criteria.SetMaxResults(maxRows);
      //         }

      //         IList<T> list = criteria.List<T>();

      //         if ((list == null) || (list.Count == 0))
      //         {
      //            return null;
      //         }

      //         return list;
      //      }
      //      catch (Exception exception)
      //      {
      //         throw new DataException("Could not perform FindAll for " + typeof(T).Name, exception);
      //      }
      //   }
      //}



      public IList<T> FindAll(int firstResult,
                              int numberOfResults,
                              Order[] selectionOrder,
                              params ICriterion[] criteria)
      {
         ISession session = GetSession();

         ICriteria crit = CreateCriteriaFromArray(session, criteria);
         crit.SetFirstResult(firstResult)
            .SetMaxResults(numberOfResults);

         foreach (Order order in selectionOrder)
         {
            crit.AddOrder(order);
         }
         return crit.List<T>();
      }


      /// <summary>
      /// Loads all the entities that match the criteria
      /// </summary>
      /// <param name="firstRow"></param>
      /// <param name="maxRows"></param>
      /// <returns></returns>
      public virtual IList<T> FindAll(int firstRow, int maxRows)
      {
         ISession session = GetSession();
         try
         {
            ICriteria criteria = session.CreateCriteria(typeof(T));

            if (firstRow != -2147483648)
            {
               criteria.SetFirstResult(firstRow);
            }
            if (maxRows != -2147483648)
            {
               criteria.SetMaxResults(maxRows);
            }

            IList<T> list = criteria.List<T>();
            return list;
         }
         catch (Exception exception)
         {
            throw new Arashi.Core.Exceptions.ApplicationException("Could not perform FindAll for " + typeof(T).Name, exception);
         }
      }

      #endregion

      #region FindAll with DetachedCriteria

      public IList<T> FindAll(DetachedCriteria criteria, params Order[] orders)
      {
         ISession session = GetSession();
         ICriteria executableCriteria = GetExecutableCriteria(session, criteria, orders);
         return executableCriteria.List<T>();
      }



      public IList<T> FindAll(DetachedCriteria criteria, int firstResult, int maxResults, params Order[] orders)
      {
         ISession session = GetSession();

         ICriteria executableCriteria = GetExecutableCriteria(session, criteria, orders);
         executableCriteria.SetFirstResult(firstResult);
         executableCriteria.SetMaxResults(maxResults);

         return executableCriteria.List<T>();
      }

      #endregion

      #region FindFirst

      public virtual T FindFirst(params ICriterion[] criterias)
      {
         ISession session = GetSession();
         ICriteria crit = CreateCriteriaFromArray(session, criterias);
         return crit.List<T>()[0];
      }



      /// <summary>
      /// Find the entity based on a criteria.
      /// </summary>
      /// <param name="criteria"></param>
      /// <param name="orders"></param>
      /// <returns></returns>
      public virtual T FindFirst(DetachedCriteria criteria, Order[] orders)
      {
         ISession session = GetSession();

         ICriteria executableCriteria = GetExecutableCriteria(session, criteria, orders);

         executableCriteria.SetFirstResult(0);
         executableCriteria.SetMaxResults(1);

         return executableCriteria.UniqueResult<T>();
      }



      /// <summary>
      /// Find the entity based on a criteria.
      /// </summary>
      /// <param name="criteria"></param>
      /// <returns></returns>
      public virtual T FindFirst(DetachedCriteria criteria)
      {
         ISession session = GetSession();
         ICriteria executableCriteria = GetExecutableCriteria(session, criteria, null);

         executableCriteria.SetFirstResult(0);
         executableCriteria.SetMaxResults(1);

         return executableCriteria.UniqueResult<T>();
      }


      /// <summary>
      /// Find the entity based on a criteria.
      /// </summary>
      /// <param name="orders"></param>
      /// <returns></returns>
      public virtual T FindFirst(Order[] orders)
      {
         return FindFirst(null, orders);
      }

      #endregion

      #region FindOne

      /// <summary>
      /// Find a single entity based on a criteria.
      /// Thorws is there is more than one result.
      /// </summary>
      /// <param name="criterias"></param>
      /// <returns></returns>
      public virtual T FindOne(params ICriterion[] criterias)
      {
         ISession session = GetSession();
         ICriteria crit = CreateCriteriaFromArray(session, criterias);
         return crit.UniqueResult<T>();
      }



      /// <summary>
      /// Find a single entity based on a criteria.
      /// Thorws is there is more than one result.
      /// </summary>
      /// <param name="criteria"></param>
      /// <returns></returns>
      public virtual T FindOne(DetachedCriteria criteria)
      {
         ISession session = GetSession();
         ICriteria executableCriteria = GetExecutableCriteria(session, criteria, null);
         return executableCriteria.UniqueResult<T>();
      }



      /// <summary>
      /// Find a single entity based on a criteria.
      /// Thorws is there is more than one result.
      /// </summary>
      /// <param name="namedQuery"></param>
      /// <returns></returns>
      public virtual T FindOne(string namedQuery)
      {
         if (string.IsNullOrEmpty(namedQuery))
         {
            throw new ArgumentNullException("namedQuery");
         }

         ISession session = GetSession();
         IQuery query = session.GetNamedQuery(namedQuery);

         if (query == null)
         {
            throw new ArgumentException("Cannot find named query", "namedQuery");
         }

         return (T)query.UniqueResult();
      }

      #endregion

      #region Exists

      /// <summary>
      /// Check if any instance matches the criteria.
      /// </summary>
      /// <returns><c>true</c> if an instance is found; otherwise <c>false</c>.</returns>
      public virtual bool Exists(DetachedCriteria criteria)
      {
         return 0 != Count(criteria);
      }



      /// <summary>
      /// Check if any instance of the type exists
      /// </summary>
      /// <returns><c>true</c> if an instance is found; otherwise <c>false</c>.</returns>
      public virtual bool Exists()
      {
         return Exists(null);
      }

      #endregion

      #region Count

      /// <summary>
      /// Counts the number of instances matching the criteria.
      /// </summary>
      /// <param name="criteria"></param>
      /// <returns></returns>
      public virtual long Count(DetachedCriteria criteria)
      {
         ISession session = GetSession();
         ICriteria crit = GetExecutableCriteria(session, criteria, null);
         crit.SetProjection(Projections.RowCount());

         object countMayBe_Int32_Or_Int64_DependingOnDatabase = crit.UniqueResult();

         return Convert.ToInt64(countMayBe_Int32_Or_Int64_DependingOnDatabase);
      }



      /// <summary>
      /// Counts the overall number of instances.
      /// </summary>
      /// <returns></returns>
      public virtual long Count()
      {
         return Count(null);
      }

      #endregion

      #region FindAll

      /// <summary>
      /// Loads all the entities that match the given query
      /// </summary>
      /// <param name="queryString"></param>
      /// <returns></returns>
      public virtual IList<T> FindAllWithCustomQuery(string queryString)
      {
         return FindAllWithCustomQuery(queryString, -2147483648, -2147483648);
      }



      /// <summary>
      /// Loads all the entities that match the given query
      /// </summary>
      /// <param name="queryString"></param>
      /// <param name="firstRow"></param>
      /// <param name="maxRows"></param>
      /// <returns></returns>
      public virtual IList<T> FindAllWithCustomQuery(string queryString, int firstRow, int maxRows)
      {
         if (string.IsNullOrEmpty(queryString))
         {
            throw new ArgumentNullException("queryString");
         }

         ISession session = GetSession();

         try
         {
            IQuery query = session.CreateQuery(queryString);
            if (firstRow != -2147483648)
            {
               query.SetFirstResult(firstRow);
            }

            if (maxRows != -2147483648)
            {
               query.SetMaxResults(maxRows);
            }

            IList<T> list = query.List<T>();

            if ((list == null) || (list.Count == 0))
            {
               return null;
            }
            return list;
         }
         catch (Exception exception)
         {
            throw new Arashi.Core.Exceptions.ApplicationException("Could not perform Find for custom query : " + queryString, exception);
         }
      }



      /// <summary>
      /// Loads all the entities that match the named query
      /// </summary>
      /// <param name="namedQuery"></param>
      /// <returns></returns>
      public virtual IList<T> FindAllWithNamedQuery(string namedQuery)
      {
         return FindAllWithNamedQuery(namedQuery, -2147483648, -2147483648);
      }



      /// <summary>
      /// Loads all the entities that match the named query
      /// </summary>
      /// <param name="namedQuery"></param>
      /// <param name="firstRow"></param>
      /// <param name="maxRows"></param>
      /// <returns></returns>
      public virtual IList<T> FindAllWithNamedQuery(string namedQuery, int firstRow, int maxRows)
      {
         if (string.IsNullOrEmpty(namedQuery))
         {
            throw new ArgumentNullException("namedQuery");
         }

         ISession session = GetSession();
         try
         {
            IQuery query = session.GetNamedQuery(namedQuery);

            if (query == null)
            {
               throw new ArgumentException("Cannot find named query", "namedQuery");
            }

            if (firstRow != -2147483648)
            {
               query.SetFirstResult(firstRow);
            }

            if (maxRows != -2147483648)
            {
               query.SetMaxResults(maxRows);
            }

            IList<T> list = query.List<T>();
            if ((list == null) || (list.Count == 0))
            {
               return null;
            }
            return list;
         }
         catch (Exception exception)
         {
            throw new Arashi.Core.Exceptions.ApplicationException("Could not perform Find for named query : " + namedQuery, exception);
         }
      }

      #endregion

      #region FindById

      /// <summary>
      /// Loads one entities that match the given id
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
      public virtual T FindById(object id)
      {
         T obj2;
         ISession session = GetSession();
         try
         {
            obj2 = session.Load<T>(id);
         }
         catch (ObjectNotFoundException)
         {
            throw;
         }
         catch (Exception exception)
         {
            throw new Arashi.Core.Exceptions.ApplicationException("Could not perform FindByPrimaryKey for " + typeof(T).Name, exception);
         }

         return obj2;
      }



      public virtual T FindById(object id, bool allowNull)
      {
         if (allowNull)
         {
            ISession session = GetSession();
            return session.Get<T>(id);
         }
         return FindById(id);
      }

      #endregion

      #region Save & Update

      /// <summary>
      /// Register te entity for insert/save in the database when the unit of work
      /// is completed. (INSERT OR UPDATE)
      /// </summary>
      /// <param name="entity"></param>
      public virtual T Save(T entity)
      {
         ISession session = GetSession();

         try
         {
            session.SaveOrUpdate(entity);
            return entity;
         }
         catch (Exception exception)
         {
            throw new Arashi.Core.Exceptions.ApplicationException("Could not perform Save for " + entity.GetType().Name, exception);
         }

      }



      /// <summary>
      /// Register the entity for update in the database when the unit of work
      /// is completed. (UPDATE)
      /// </summary>
      /// <param name="entity"></param>
      public virtual void Update(T entity)
      {
         ISession session = GetSession();

         try
         {
            session.Update(entity);
         }
         catch (Exception exception)
         {
            throw new Arashi.Core.Exceptions.ApplicationException("Could not perform Update for " + entity.GetType().Name, exception);
         }

      }

      #endregion

      #endregion

      #region Special Methods (Powered by uNHaddins)

      public Paginator<T> GetPaginator(IDetachedQuery detachedQuery, int pageSize)
      {
         IPaginable<T> paginable = GetPaginable(detachedQuery);

         Paginator<T> ptor = new Paginator<T>(
            pageSize,
            paginable,
            GetRowCounter(detachedQuery));

         return ptor;
      }



      public Paginator<T> GetPaginator(DetachedCriteria criteria, int pageSize)
      {
         IPaginable<T> paginable = GetPaginable(criteria);

         Paginator<T> ptor = new Paginator<T>(
            pageSize,
            paginable);

         return ptor;
      }




      public Paginator<T> GetPaginator(IDetachedQuery detachedQuery, IDetachedQuery detachedQueryForRowCounter, int pageSize)
      {
         IPaginable<T> paginable = GetPaginable(detachedQuery);

         Paginator<T> ptor = new Paginator<T>(
            pageSize,
            paginable,
            GetRowCounter(detachedQueryForRowCounter));

         return ptor;
      }



      public IRowsCounter GetRowCounter(IDetachedQuery dq)
      {
         IRowsCounter counter = QueryRowsCounter.Transforming((DetachedQuery)dq);
         return counter;
      }



      private IPaginable<T> GetPaginable(IDetachedQuery query)
      {
         return new PaginableQuery<T>(GetSession(), query);
      }


      private IPaginable<T> GetPaginable(DetachedCriteria criteria)
      {
         return new PaginableCriteria<T>(GetSession(), criteria);
      }



      #endregion

      #region Cache

      public void ClearSessionCache(T entity)
      {
         ISession session = GetSession();
         session.Evict(entity);
      }


      public void ClearSecondLevelCache()
      {
         ISession session = GetSession();
         session.SessionFactory.Evict(typeof(T));
      }

      public void ClearSecondLevelCache(int objectId)
      {
         ISession session = GetSession();
         session.SessionFactory.Evict(typeof(T), objectId);
      }

      #endregion

      #region Helpers

      internal static ICriteria GetExecutableCriteria(ISession session,
                                                      DetachedCriteria criteria,
                                                      Order[] orders)
      {
         ICriteria executableCriteria;
         if (criteria != null)
         {
            executableCriteria = criteria.GetExecutableCriteria(session);
         }
         else
         {
            executableCriteria = session.CreateCriteria(typeof(T));
         }

         //AddCaching(executableCriteria);
         if (orders != null)
         {
            foreach (Order order in orders)
            {
               executableCriteria.AddOrder(order);
            }
         }
         return executableCriteria;
      }



      public static ICriteria CreateCriteriaFromArray(ISession session, ICriterion[] criteria)
      {
         ICriteria crit = session.CreateCriteria(typeof(T));
         foreach (ICriterion criterion in criteria)
         {
            //allow some fancy antics like returning possible return 
            // or null to ignore the criteria
            if (criterion == null)
               continue;
            crit.Add(criterion);
         }
         //AddCaching(crit);
         return crit;
      }


      //public static void AddCaching(IQuery query)
      //{
      //   if (With.Caching.ShouldForceCacheRefresh == false && With.Caching.Enabled)
      //   {
      //      query.SetCacheable(true);
      //      if (With.Caching.CurrentCacheRegion != null)
      //         query.SetCacheRegion(With.Caching.CurrentCacheRegion);
      //   }
      //   else if (With.Caching.ShouldForceCacheRefresh)
      //   {
      //      query.SetForceCacheRefresh(true);
      //   }
      //}



      //public static void AddCaching(ICriteria crit)
      //{
      //   if (With.Caching.ShouldForceCacheRefresh == false &&
      //       With.Caching.Enabled)
      //   {
      //      crit.SetCacheable(true);
      //      if (With.Caching.CurrentCacheRegion != null)
      //         crit.SetCacheRegion(With.Caching.CurrentCacheRegion);
      //   }
      //}


      //internal static IQuery CreateQuery(ISession session, string namedQuery, Parameter[] parameters)
      //{
      //   IQuery query = session.GetNamedQuery(namedQuery);
      //   foreach (Parameter parameter in parameters)
      //   {
      //      if (parameter.Type == null)
      //         query.SetParameter(parameter.Name, parameter.Value);
      //      else
      //         query.SetParameter(parameter.Name, parameter.Value, parameter.Type);
      //   }
      //   AddCaching(query);
      //   return query;
      //}

      #endregion
   }
}