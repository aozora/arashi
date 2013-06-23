using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Arashi.Web.Services
{
   using Arashi.Core;
   using Arashi.Core.Domain;
   using Arashi.Core.NHibernate.Wcf;
   using Arashi.Services.Widget;

   using NHibernate;
   using NHibernate.Linq;

   /// <summary>
   /// WidgetsDomainService
   /// </summary>
   public class WidgetsDomainService : NHibernateDomainService
   {
      //private readonly IWidgetService widgetService;

      /// <summary>
      /// Constructor
      /// </summary>
      /// <param name="sessionFactory"></param>
      public WidgetsDomainService()
         : base(NHibernateFactory.Initialize())
      {
         //widgetService = IoC.Resolve<IWidgetService>();
      }


      // todo: add Site argument
      public IQueryable<Widget> GetAvailableWidgets(int siteId)
      {
         //return RepositoryHelper.GetSession().GetNamedQuery("GetWidgetsBySite")
         //      .SetEntity("site", site)
         //      .SetCacheable(true)
         //      .List<Arashi.Core.Domain.Widget>();

         Site site = Session.Get<Site>(siteId);

         return Session.GetNamedQuery("GetWidgetsBySite")
               .SetEntity("site", site)
               .SetCacheable(true)
               .List<Arashi.Core.Domain.Widget>()
               .AsQueryable();
      }


      //      public IQueryable<SuperEmployee> GetSuperEmployees()
//      {
//         return Session.Linq<SuperEmployee>()
//                 .Where(e => e.Issues > 10);
//      }

//      public SuperEmployee GetSuperEmployee(int employeeID)
//      {
//         return GetSuperEmployees()
//                    .Where(emp => emp.EmployeeID == employeeID)
//                    .FirstOrDefault();
//      }

//      public void InsertSuperEmployee(SuperEmployee superEmployee)
//      {
//         Session.Save(superEmployee);
//      }

//      public void UpdateSuperEmployee(SuperEmployee currentSuperEmployee)
//      {

//         Session.Update(currentSuperEmployee);
//      }


//      public IQueryable<Origin> GetOrigins()
//      {
//         return Session.CreateQuery(
//             @"select emp.Origin as Name, count(emp.Origin) as Count 
//				 from SuperEmployee emp
//				 group by emp.Origin")
//             .SetResultTransformer(new AliasToBeanResultTransformer(typeof(Origin)))
//             .List<Origin>()
//             .AsQueryable();
//      }

   }
}