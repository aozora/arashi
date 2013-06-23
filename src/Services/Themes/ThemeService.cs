using System;
using Common.Logging;

namespace Arashi.Services.Themes
{
   using System.Collections.Generic;
   using Arashi.Core.Domain;
   using Arashi.Core.Repositories;
   using uNhAddIns.Pagination;
   using NHibernate.Criterion;

   public class ThemeService : ServiceBase, IThemeService
   {

      public ThemeService(Arashi.Core.NHibernate.ISessionFactory sessionFactory, ILog log)
         : base(sessionFactory, log)
      {
      }




      public Paginator<Theme> GetPaginatorForAll(int pageSize)
      {
         DetachedCriteria criteria = DetachedCriteria.For<Theme>()
                                       .AddOrder(new Order("Name", true));

         return Repository<Theme>.GetPaginator(criteria, pageSize);
      }



      public IList<Theme> GetAll()
      {
         return Session.GetNamedQuery("ThemeService_GetAll")
                                 .List<Theme>();
      }



      public Paginator<Theme> GetPaginatorBySite(Site site, int pageSize)
      {
         if (site == null)
            throw new ArgumentNullException("site");

         DetachedCriteria criteria = DetachedCriteria.For<Theme>()
                                       .AddOrder(new Order("Name", true))
                                       .Add(Restrictions.Eq("Site", site));

         return Repository<Theme>.GetPaginator(criteria, pageSize);
      }



      public Theme GetById(int themeId)
      {
         return Session.GetNamedQuery("ThemeService_GetById")
                     .SetInt32("id", themeId)
                     .UniqueResult<Theme>();
      }



      public void Save(Theme theme)
      {
         //using (NHTransactionScope tx = new NHTransactionScope())
         //{
         Repository<Theme>.Save(theme);
         //   tx.VoteCommit();
         //}
      }



      public void Delete(Theme theme)
      {
         //using (NHTransactionScope tx = new NHTransactionScope())
         //{
         Repository<Theme>.Delete(theme);
         //   tx.VoteCommit();
         //}
      }

   }
}
