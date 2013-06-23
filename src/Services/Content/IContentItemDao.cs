using System;
using System.Collections.Generic;
using Arashi.Core.Domain;
using NHibernate.Criterion;
using uNhAddIns.Pagination;

namespace Arashi.Services.Content
{
   public interface IContentItemDao<T> where T : IContentItem
   {
		/// <summary>
		/// Gets a single content item by its prmary key.
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
      /// Gets all content items of T by the given criteria.
      /// </summary>
      /// <param name="detachedCriteria"></param>
      /// <returns></returns>
      IList<T> GetByCriteria(DetachedCriteria detachedCriteria);

		/// <summary>
		/// Save a single content item in the database.
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		T Save(T entity);

		/// <summary>
		/// Delete a single content item from the database.
		/// </summary>
		/// <param name="entity"></param>
		void Delete(T entity);
   }
}