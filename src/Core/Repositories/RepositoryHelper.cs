using System;
using NHibernate;
using Arashi.Core.NHibernate;

namespace Arashi.Core.Repositories
{
   public static class RepositoryHelper
   {
      //private static CommonRepository internalCommonRepository
      //{
      //   get
      //   {
      //      return IoC.Resolve<CommonRepository>();
      //   }
      //}


      public static ISession GetSession()
      {
         //return internalCommonRepository.GetSession();
         //ISessionManager sessionManager = IoC.Resolve<ISessionManager>();
         //return sessionManager.OpenSession();
         Arashi.Core.NHibernate.ISessionFactory sessionFactory = IoC.Resolve<Arashi.Core.NHibernate.ISessionFactory>();
         return sessionFactory.GetSession();
      }


      //#region TestRelatedMethod

      ///// <summary>
      ///// 	override the session factory until the return object get disposed.
      ///// </summary>
      ///// <param name = "configurationSection">The new configuration section that will override
      ///// 	the standard one configured.</param>
      ///// <returns></returns>
      //internal static IDisposable OverrideSessionFactory(NHConfigurationSection configurationSection)
      //{
      //   IPersistenceConfigurer dbConfiguration = DbConfiguration(configurationSection);
      //   FluentConfiguration configuration = GenerateFluentConfiguration(dbConfiguration);
      //   overrideSessionFactory = configuration.BuildSessionFactory();
      //   overrideConfiguration = configuration.BuildConfiguration();
      //   return new DisposableAction(() =>
      //   {
      //      overrideSessionFactory = null;
      //      overrideConfiguration = null;
      //   });
      //}

      //#endregion

   }
}