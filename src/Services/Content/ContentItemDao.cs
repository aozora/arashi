using System;
using System.Collections.Generic;
using Arashi.Core.NHibernate;
using Arashi.Core.Repositories;
using Common.Logging;
using NHibernate;
using NHibernate.Criterion;
using Arashi.Core.Domain;
using NHibernate.Impl;
using uNhAddIns.Pagination;
using ISessionFactory = Arashi.Core.NHibernate.ISessionFactory;

namespace Arashi.Services.Content
{
   public class ContentItemDao<T> : ServiceBase, IContentItemDao<T> where T : IContentItem
   {
      protected Type persistentType = typeof(T);

      /// <summary>
      /// Constructor
      /// </summary>
      public ContentItemDao(ISessionFactory sessionFactory, ILog log)
         :base(sessionFactory, log)
      {
      }



      /// <summary>
      /// Loads an instance of type T from the DB based on its ID of type long.
      /// </summary>
      public T GetById(int id)
      {
         return Session.Get<T>(id);
      }

      /// <summary>
      /// Loads an instance of type T from the DB based on its ID of type Guid.
      /// </summary>
      public T GetById(Guid id)
      {
         return Session.Get<T>(id);
      }



      public IList<T> GetByCriteria(DetachedCriteria detachedCriteria)
      {
         ICriteria criteria = detachedCriteria.GetExecutableCriteria(Session);
         return criteria.List<T>();
      }

      public T Save(T entity)
      {
         Session.Save(entity);
         return entity;
      }

      public void Delete(T entity)
      {
         Session.Delete(entity);
      }

   }
}