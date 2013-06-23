using System;
using System.Collections.Generic;
using Arashi.Core.Domain;
using uNhAddIns.Pagination;

namespace Arashi.Services.Content
{
   public interface ICategoryService
   {

		/// <summary>
		/// Get by id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Category GetById(int id);


      /// <summary>
      /// Get a paginated set of tags
      /// </summary>
      /// <param name="site"></param>
      /// <param name="pageSize"></param>
      /// <returns></returns>
      Paginator<Category> GetPaginatorBySite(Site site, int pageSize);


      /// <summary>
      /// Returns a category given its friendly name and site
      /// </summary>
      /// <param name="site"></param>
      /// <param name="friendlyName"></param>
      /// <returns></returns>
      Category GetBySiteAndFriendlyName(Site site, string friendlyName);


		/// <summary>
		/// Gets all categories that have no parent category
		/// </summary>
		/// <returns></returns>
      IList<Category> GetAllRootCategories(Site site);

		/// <summary>
		/// Save a single category.
		/// </summary>
		/// <param name="category"></param>
		void Save(Category category);

		/// <summary>
		/// Delete a single category.
		/// </summary>
		/// <param name="category"></param>
		void Delete(Category category);

      /// <summary>
      /// Get all categories ordered by hierarchy (via Path).
      /// </summary>
      /// <param name="site"></param>
      /// <returns></returns>
      IEnumerable<Category> GetAllCategoriesBySite(Site site);


      /// <summary>
      /// Returns the number of posts with a similar friendlyname.
      /// This is used by the Save method to ensure that each post have a unique friendlyname
      /// </summary>
      /// <param name="site"></param>
      /// <param name="friendlyName"></param>
      /// <returns></returns>
      long GetCountForSimilarFriendlyName(Site site, string friendlyName);

   }
}