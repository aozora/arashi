using Arashi.UnitTests.AutoMockingContainer;
using Castle.MicroKernel.Registration;
using NUnit.Framework;
using Rhino.Mocks;

namespace Arashi.IntegrationTests.Infrastructure.DataAccess
{
   using Arashi.UnitTests;
   using NHibernate;
   using NHibernate.Cfg;



   public class NHibernateMockedTestFixtureBase : TestBase
   {
      protected static Arashi.Core.NHibernate.ISessionFactory sessionFactory;
      protected static Configuration configuration;
      protected ISession session;
      //protected ITransaction transaction;


      [TestFixtureSetUp]
      public void OneTimeTestInitialize()
      {
         //MockRepository mocks = new MockRepository();
         //session = mocks.StrictMock<ISession>();
         //transaction = mocks.Stub<ITransaction>();

         //sessionFactory = mocks.StrictMock<ISessionFactory>();
         //this.AutoMockingContainer().Register(new IRegistration[] {Component.For<ISessionFactory>().Instance(sessionFactory).LifeStyle.Singleton});

         //this.AutoMockingContainer().Register(new IRegistration[] { Component.For<NHibernate.Cfg.Configuration>()
         //                                        .Instance(configuration)
         //                                        .Named("nhibernate.cfg") }
         //                       );
         //this.AutoMockingContainer().Register(new IRegistration[] { Component.For<ISessionFactory>()
         //                                        .Instance(sessionFactory)
         //                                        .Named("nhibernate")
         //                                        .LifeStyle.Singleton 
         //                                 });


      }


      [SetUp]
      public void TestInitialize()
      {
         //session = this.CreateSession();
      }

      //public ISession CreateSession()
      //{
      //   //ISession openSession = sessionFactory.OpenSession();
      //   //IDbConnection connection = openSession.Connection;
      //   //new SchemaExport(configuration).Execute(false, true, false, connection, null);
      //   //return openSession;
      //}



      [TearDown]
      public void TestCleanup()
      {
         session.Dispose();
      }


   }
}
