using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Arashi.Core;
using Arashi.Core.NHibernate;
using Arashi.Core.Repositories;
//using Castle.Facilities.NHibernateIntegration;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using log4net;
using log4net.Config;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace Arashi.Core.Cms.Test
{
   /// <summary>
   /// http://ayende.com/Blog/archive/2006/10/14/UnitTestingWithNHibernateActiveRecord.aspx
   /// </summary>
   public class NHibernateInMemoryTestFixtureBase
   {
      private static readonly ILog log = LogManager.GetLogger(typeof(NHibernateInMemoryTestFixtureBase));
      protected static ISessionFactory sessionFactory;
      protected static Configuration configuration;
      protected static ISessionBuilder sessionBuilder;

      //protected static ISessionManager sessionManager;

      //protected string sessionFactoryPath = @"Config\NHibernate.config";



      /// <summary>
      /// Initialize NHibernate and builds a session factory
      /// Note, this is a costly call so it will be executed only one.
      /// </summary>
      public static void OneTimeInitalize(params Assembly[] assemblies)
      {
         //if (sessionFactory != null)
         //   return;

         // Initialize log4net 
         XmlConfigurator.ConfigureAndWatch(new FileInfo(@"D:\dev\Projects\Azora System\trunk\Core.Cms.Test\Config\logging.config"));

         // Init IoC
         IWindsorContainer container = new WindsorContainer(new XmlInterpreter());
         IoC.Initialize(container);

         sessionBuilder = IoC.Resolve<ISessionBuilder>();
         sessionFactory = sessionBuilder.GetSessionFactory();

         // Insert into IoC Container the ISessionFactory instance
         IoC.Container.Kernel.AddComponentInstance("nhibernate.factory", typeof(ISessionFactory), sessionFactory);

         // Use NH SchemaExport to generate Sqlite Database
         ISession session = RepositoryHelper.GetSession();
         IDbConnection connection = session.Connection;
         session.FlushMode = FlushMode.Commit;
         configuration = sessionBuilder.GetConfiguration();

         //new SchemaExport(configuration).Execute(false, true, false, true, connection, null);
         new SchemaExport(configuration).Execute(false, true, false, connection, null);
      }



      protected void FlushSessionAndEvict(object instance)
      {
         // Commits any changes up to this point to the database
         sessionBuilder.GetSession().Flush();

         // Evicts the instance from the current session so that it can be loaded during testing;
         // this gives the test a clean slate, if you will, to work with
         sessionBuilder.GetSession().Evict(instance);
      }



      protected void DisposeSession()
      {
         ISession session = sessionBuilder.GetSession();

         if (session != null)
         {
            session.Dispose();
         }
      }

   }
}