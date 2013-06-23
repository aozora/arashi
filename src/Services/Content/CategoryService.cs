using System;
using System.Collections.Generic;
using Arashi.Core.Domain.Extensions;
using Arashi.Core.NHibernate;
using Arashi.Core.Repositories;
using Arashi.Core.Domain;
using NHibernate;
using NHibernate.Criterion;

namespace Arashi.Services.Content
{
   public class CategoryService : ICategoryService
   {

      public CategoryService()
      {
      }



      /// <summary>
      /// Gets all root categories, ordered by path
      /// </summary>
      /// <returns></returns>
      public IList<Category> GetAllRootCategories(Site site)
      {
         //string hql = "from Arashi.Core.Domain.Category c where c.Site = :site and c.ParentCategory is null order by c.Path asc";
         return RepositoryHelper.GetSession().GetNamedQuery("GetAllRootCategories")
                           .SetEntity("site", site)
                           .List<Category>();
      }



      public Category GetById(int categoryId)
      {
         using (NHTransactionScope tx = new NHTransactionScope())
         {
            Category category = Repository<Category>.FindById(categoryId);
            //NHibernateUtil.Initialize(category.ChildCategories);
            //NHibernateUtil.Initialize(category.ContentItems);
            tx.VoteCommit();
            return category;
         }
      }



      public Category GetBySiteAndFriendlyName(Site site, string friendlyName)
      {
         //string hql = "from Arashi.Core.Domain.Category c where c.Site = :site and c.FriendlyName = :name";
         return RepositoryHelper.GetSession().GetNamedQuery("GetCategoryBySiteAndFriendlyName")
                           .SetEntity("site", site)
                           .SetString("name", friendlyName)
                           .UniqueResult<Category>();
      }



      public void Save(Category category)
      {
         using (NHTransactionScope tx = new NHTransactionScope())
         {
            // Check if the FriendlyName is unique
            long similarCount = GetCountForSimilarFriendlyName(category.Site, category.FriendlyName);

            if (similarCount > 0)
               category.FriendlyName = category.FriendlyName + "-" + (similarCount + 1).ToString();

            Repository<Category>.Save(category);
            tx.VoteCommit();
         }
      }


      
      public void Delete(Category category)
      {
         if (category.ChildCategories.Count > 0)
            throw new ArgumentException("CategoryHasChildCategoriesException");

         if (category.ContentItems.Count > 0)
            throw new ArgumentException("CategoryHasContentItemsException");
         
         using (NHTransactionScope tx = new NHTransactionScope())
         {
            Repository<Category>.Delete(category);
            tx.VoteCommit();
         }

         if (category.ParentCategory != null)
         {
            RemoveCategoryAndReorderSiblings(category, category.ParentCategory.ChildCategories);
         }
         else
         {
            RemoveCategoryAndReorderSiblings(category, category.Site.RootCategories);
         }
      }


      private void RemoveCategoryAndReorderSiblings(Category category, IList<Category> categories)
      {
         categories.Remove(category);
         for (int i = 0; i < categories.Count; i++)
         {
            categories[i].SetPosition(i);
         }
      }



      /// <summary>
      /// Get all categories ordered by hierarchy (via Path).
      /// </summary>
      /// <param name="site"></param>
      /// <returns></returns>
      public IEnumerable<Category> GetAllCategoriesBySite(Site site)
      {
         //string hql = "from Arashi.Core.Domain.Category c where c.Site = :site order by c.Path asc";
         return RepositoryHelper.GetSession().GetNamedQuery("GetAllCategoriesBySite")
                           .SetEntity("site", site)
                           .List<Category>();
      }



      public long GetCountForSimilarFriendlyName(Site site, string friendlyName)
      {
         DetachedCriteria criteria = DetachedCriteria.For<Category>()
                                       .Add(Restrictions.Eq("Site", site))
                                       .Add(Restrictions.Like("FriendlyName", friendlyName, MatchMode.Start))
                                       .SetProjection(Projections.Count("Id"));

         ICriteria c = criteria.GetExecutableCriteria(RepositoryHelper.GetSession());

         return Convert.ToInt64(c.UniqueResult());
      }


   }
}