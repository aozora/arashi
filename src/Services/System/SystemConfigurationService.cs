using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arashi.Core.Domain;
using Arashi.Core.NHibernate;
using Arashi.Core.Repositories;

namespace Arashi.Services.SystemService
{
   using Arashi.Core;

   using NHibernate;

   /// <summary>
   /// SystemConfigurationService
   /// </summary>
   public class SystemConfigurationService : ISystemConfigurationService
   {
      #region Implementation of ISystemConfigurationService

      /// <summary>
      /// Get the instance of current system configuration
      /// </summary>
      /// <returns></returns>
      public SystemConfiguration Get()
      {
         // This method can be called from the EmailSenderJob by the Scheduler that runs in a separate thread.
         // When performing things in a separate thread, you have to obtain a session manually because as if you have noticed, 
         // there is no HttpContext where the session manager retrieves its session from (don't remove isWeb from the config).

         ISessionFactory sessionFactory = IoC.Resolve<ISessionFactory>();
         using (ISession session = sessionFactory.OpenSession())
         {
            return session.CreateCriteria<SystemConfiguration>().UniqueResult<SystemConfiguration>();
         }

         //return Repository<SystemConfiguration>.FindOne();
      }



      /// <summary>
      /// Save a system configuration
      /// </summary>
      /// <param name="systemConfiguration"></param>
      public void Save(SystemConfiguration systemConfiguration)
      {
         using (NHTransactionScope tx = new NHTransactionScope())
         {
            Repository<SystemConfiguration>.Save(systemConfiguration);
            tx.VoteCommit();
         }
      }

      #endregion
   }
}
