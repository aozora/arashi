using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Logging;
using NHibernate;
using NHibernate.Criterion;

namespace Arashi.Services.SiteStructure
{
   public class VersionService : ServiceBase, IVersionService
   {
      public VersionService(Arashi.Core.NHibernate.ISessionFactory sessionFactory, ILog log)
         : base(sessionFactory, log)
      {
      }


      public Arashi.Core.Domain.Version GetVersion()
      {
         return Session.CreateCriteria<Arashi.Core.Domain.Version>().UniqueResult<Arashi.Core.Domain.Version>();
      }


      public Arashi.Core.Domain.Version GetVersionForAssembly(string assembly)
      {
         IQuery query = Session.CreateQuery("from Version where Assembly = :assembly")
                           .SetString("assembly", assembly);

         return query.UniqueResult<Arashi.Core.Domain.Version>();
      }

   }
}
