using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arashi.Core.Domain;
using Arashi.Core.Repositories;
using Arashi.Core.NHibernate;
using uNhAddIns.Pagination;
using NHibernate.Criterion;

namespace Arashi.Services.Themes
{
   public class ThemeService : IThemeService
   {

      public IList<Template> FindBySite(Site site)
      {
         if (site == null)
            throw new ArgumentNullException("site");

         return RepositoryHelper.GetSession().GetNamedQuery("ThemeService_FindBySite")
                     .SetEntity("site", site)
                     .List<Template>();
      }



      public Paginator<Template> GetPaginatorForAll(int pageSize)
      {
         DetachedCriteria criteria = DetachedCriteria.For<Template>()
                                       .AddOrder(new Order("Name", true));

         return Repository<Template>.GetPaginator(criteria, pageSize);
      }



      public Paginator<Template> GetPaginatorBySite(Site site, int pageSize)
      {
         if (site == null)
            throw new ArgumentNullException("site");

         DetachedCriteria criteria = DetachedCriteria.For<Template>()
                                       .AddOrder(new Order("Name", true))
                                       .Add(Restrictions.Eq("Site", site));

         return Repository<Template>.GetPaginator(criteria, pageSize);
      }



      public Template GetById(int templateId)
      {
         return RepositoryHelper.GetSession().GetNamedQuery("ThemeService_GetById")
                     .SetInt32("id", templateId)
                     .UniqueResult<Template>();
      }



      public void Save(Template template)
      {
         using (NHTransactionScope tx = new NHTransactionScope())
         {
            Repository<Template>.Save(template);
            tx.VoteCommit();
         }
      }



      public void Delete(Template template)
      {
         using (NHTransactionScope tx = new NHTransactionScope())
         {
            Repository<Template>.Delete(template);
            tx.VoteCommit();
         }
      }

   }
}
