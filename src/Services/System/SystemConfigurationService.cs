using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arashi.Core.Domain;
using Arashi.Core.NHibernate;
using Arashi.Core.Repositories;
using Common.Logging;

namespace Arashi.Services.SystemService
{
   using Arashi.Core;

   using NHibernate;

   /// <summary>
   /// SystemConfigurationService
   /// </summary>
   public class SystemConfigurationService : ServiceBase, ISystemConfigurationService
   {
      public SystemConfigurationService(Arashi.Core.NHibernate.ISessionFactory sessionFactory, ILog log)
         : base(sessionFactory, log)
      {
      }

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

            return Session.CreateCriteria<SystemConfiguration>().UniqueResult<SystemConfiguration>();
      }



      /// <summary>
      /// Save a system configuration
      /// </summary>
      /// <param name="systemConfiguration"></param>
      public void Save(SystemConfiguration systemConfiguration)
      {
         Repository<SystemConfiguration>.Save(systemConfiguration);
      }

      #endregion
   }
}
