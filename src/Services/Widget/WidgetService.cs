using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arashi.Core.Repositories;
using Arashi.Core.Domain;
using Common.Logging;

namespace Arashi.Services.Widget
{
   public class WidgetService : ServiceBase, IWidgetService
   {
      public WidgetService(Arashi.Core.NHibernate.ISessionFactory sessionFactory, ILog log)
         : base(sessionFactory, log)
      {
      }


      public IList<Arashi.Core.Domain.Widget> GetWidgetsBySite(Site site)
      {
         return Session.GetNamedQuery("GetWidgetsBySite")
                        .SetEntity("site", site)
                        .SetCacheable(true)
                        .List<Arashi.Core.Domain.Widget>();
      }



      public IList<Arashi.Core.Domain.WidgetType> GetWidgetTypes()
      {
         return Session.GetNamedQuery("GetWidgetTypes")
                        .SetCacheable(true)
                        .List<Arashi.Core.Domain.WidgetType>();
      }
   }
}
