using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using Castle.Windsor.Installer;
using log4net;
using log4net.Config;
using NHibernate;
using NUnit.Framework;
using Arashi.UnitTests.AutoMockingContainer;

namespace Arashi.IntegrationTests.Infrastructure.DataAccess
{
   [TestFixture]
   public class InMemoryTests : NHibernateInMemoryTestFixtureBase
   {
      protected ISession session;
      private static readonly ILog log = LogManager.GetLogger(typeof(InMemoryTests));

      protected IWindsorContainer Container
      {
         get;
         set;
      }



      [TestFixtureSetUp]
      public void OneTimeTestInitialize()
      {
         //XmlConfigurator.Configure();
         //log.Debug("InMemoryTests.OneTimeTestInitialize: log4net initialized");

         //// Init IoC
         //IWindsorContainer container = new WindsorContainer(new XmlInterpreter());
         //Container = container;
         //log.Debug("InMemoryTests.OneTimeTestInitialize: WindsorContainer initialized");


         Assembly assembly = Assembly.LoadFrom("Arashi.Core.NHibernate.dll");
         OneTimeInitalize(assembly);

      }



      [SetUp]
      public void TestInitialize()
      {
         session = this.CreateSession();
         log.Debug("InMemoryTests.TestInitialize: ISession created");
      }



      [TearDown]
      public void TestCleanup()
      {
         session.Dispose();
         log.Debug("InMemoryTests.TestInitialize: ISession disposed");
      }



      //[Test]
      //public void CanSaveAndLoadSMS()
      //{
      //   SMS sms = new SMS();
      //   sms.Message = "R U There?";
      //   session.Save(sms);
      //   session.Flush();

      //   session.Evict(sms);//remove from session cache

      //   SMS loaded = session.Load<SMS>(sms.Id);
      //   Assert.AreEqual(sms.Message, loaded.Message);
      //}
   }
}
