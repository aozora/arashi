namespace Arashi.IntegrationTests.Infrastructure.DataAccess
{
   using System.Collections;
   using System.Collections.Generic;
   using System.Data;
   using System.Reflection;
   using Arashi.UnitTests;
   using Arashi.UnitTests.AutoMockingContainer;
   using Castle.MicroKernel.Registration;
   using log4net;
   using NHibernate;
   using NHibernate.Cfg;
   using NHibernate.Tool.hbm2ddl;



   /// <summary>
   /// From: http://ayende.com/Blog/archive/2006/10/14/UnitTestingWithNHibernateActiveRecord.aspx
   /// </summary>
   public class NHibernateInMemoryTestFixtureBase : TestBase
   {
      protected static ISessionFactory sessionFactory;
      protected static Configuration configuration;
      private static readonly ILog log = LogManager.GetLogger(typeof(NHibernateInMemoryTestFixtureBase));



      /// <summary>
      /// Initialize NHibernate and builds a session factory
      /// Note, this is a costly call so it will be executed only one.
      /// </summary>
      public void OneTimeInitalize(params Assembly[] assemblies)
      {
         if (sessionFactory != null)
            return;

         IDictionary<string, string> properties = new Dictionary<string, string>
                                   {
                                      {"connection.driver_class", "NHibernate.Driver.SQLite20Driver"},
                                      {"dialect", "NHibernate.Dialect.SQLiteDialect"},
                                      {"connection.provider", "NHibernate.Connection.DriverConnectionProvider"},
                                      {"connection.connection_string_name","arashi-db"},
                                      {"connection.release_mode","on_close"},
                                      {"proxyfactory.factory_class", "NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle"},
                                      {"show_sql", "true"},
                                      {"query.substitutions", "true 1, false 0, yes 'Y', no 'N'"},
                                      {"hbm2ddl.keywords", "auto-quote"},
                                   };

         configuration = new Configuration();
         configuration.Properties = properties;

         foreach (Assembly assembly in assemblies)
         {
            configuration = configuration.AddAssembly(assembly);
         }

         sessionFactory = configuration.BuildSessionFactory();

         this.AutoMockingContainer().Register(new IRegistration[] { Component.For<NHibernate.Cfg.Configuration>()
                                                 .Instance(configuration)
                                                 .Named("nhibernate.cfg") }
                                         );
         this.AutoMockingContainer().Register(new IRegistration[] { Component.For<ISessionFactory>()
                                                 .Instance(sessionFactory)
                                                 .Named("nhibernate")
                                                 .LifeStyle.Singleton 
                                          });

         //Session session = mocks.StrictMock<NHibernate.ISession>();

         log.Debug("InMemoryTests.OneTimeTestInitialize: NHibernate initialized");

      }



      public ISession CreateSession()
      {
         ISession openSession = sessionFactory.OpenSession();
         IDbConnection connection = openSession.Connection;
         new SchemaExport(configuration).Execute(false, true, false, connection, null);
         return openSession;
      }
   }
}
