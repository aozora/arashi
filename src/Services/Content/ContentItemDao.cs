using System;
using System.Collections.Generic;
using Arashi.Core.NHibernate;
using Arashi.Core.Repositories;
using NHibernate;
using NHibernate.Criterion;
using Arashi.Core.Domain;
using NHibernate.Impl;
using uNhAddIns.Pagination;

namespace Arashi.Services.Content
{
   public class ContentItemDao<T> : IContentItemDao<T> where T : IContentItem
   {
      protected Type persistentType = typeof(T);

      /// <summary>
      /// Constructor
      /// </summary>
      public ContentItemDao()
      {
      }



      /// <summary>
      /// Loads an instance of type T from the DB based on its ID of type long.
      /// </summary>
      public T GetById(int id)
      {
         return (T)RepositoryHelper.GetSession().Get<T>(id);
      }

      /// <summary>
      /// Loads an instance of type T from the DB based on its ID of type Guid.
      /// </summary>
      public T GetById(Guid id)
      {
         return (T)RepositoryHelper.GetSession().Get<T>(id);
      }









      public IList<T> GetByCriteria(DetachedCriteria detachedCriteria)
      {
         ICriteria criteria = detachedCriteria.GetExecutableCriteria(RepositoryHelper.GetSession());
         return criteria.List<T>();
      }

      public T Save(T entity)
      {
         using (NHTransactionScope tx = new NHTransactionScope())
         {
            RepositoryHelper.GetSession().Save(entity);
            tx.VoteCommit();
            return entity;
         }
      }

      public void Delete(T entity)
      {
         using (NHTransactionScope tx = new NHTransactionScope())
         {
            RepositoryHelper.GetSession().Delete(entity);
            tx.VoteCommit();
         }
      }

   }
}