
using Arashi.IntegrationTests.Domain;
using Castle.MicroKernel.Registration;
using NHibernate;

namespace Arashi.IntegrationTests.Services
{
   using System;
   using Arashi.Core.Domain;
   using Arashi.IntegrationTests.Infrastructure.DataAccess;
   using Arashi.Services.File;
   using Arashi.Services.Membership;
   using Arashi.Services.SiteStructure;
   using Arashi.Services.Themes;
   using Arashi.UnitTests.AutoMockingContainer;
   using Common.Logging;
   using NUnit.Framework;
   using Rhino.Mocks;
   using SharpTestsEx;


   [TestFixture]
   public class SiteServiceTest : NHibernateMockedTestFixtureBase //InMemoryTests
   {
      [Test]
      public void CanCreateNewSite()
      {
         Site site = DomainTestHelper.GetTestSite();

         Theme theme = MockRepository.GenerateStub<Theme>();
         theme.BasePath = "~/themes/default";
         theme.Name = "default";
         site.Theme = theme;

         MockRepository mocks = new MockRepository();
         sessionFactory = MockRepository.GenerateMock<Arashi.Core.NHibernate.ISessionFactory>();
         session = MockRepository.GenerateMock<ISession>();
         Site createdSite;

         using (mocks.Record())
         {
            sessionFactory.Expect(x => x.GetSession()).Return(session /*MockRepository.GenerateStub<ISession>()*/);
            session.Expect(x => x.BeginTransaction()).Return(MockRepository.GenerateStub <ITransaction>());
         }
         using (mocks.Playback())
         {
            ISiteService siteService = new SiteService( sessionFactory, // this.AutoMockingContainer().Resolve<Arashi.Core.NHibernate.ISessionFactory>(),
                                                         this.AutoMockingContainer().Resolve<ILog>(),
                                                         this.AutoMockingContainer().Resolve<IUserService>(),
                                                         this.AutoMockingContainer().Resolve<IFileService>());

            createdSite = siteService.CreateNewSite("Test", "test", "test@email.com", "localhost", theme);
         }

         mocks.VerifyAll();

         createdSite.Should().Not.Be.Null();
         createdSite.Name.Should().Be.EqualTo(site.Name);
         createdSite.Hosts.Count.Should().Be.GreaterThanOrEqualTo(1);
         createdSite.Hosts[0].Should().Not.Be.Null();
         createdSite.Theme.Should().Be.EqualTo(theme);

         //NHibernate.ISession session = mocks.StrictMock<NHibernate.ISession>();
         //NHibernate.ITransaction transaction = mocks.Stub<NHibernate.ITransaction>();
         //Student expected = new Student();
         //Student actual;
         //using (mocks.Record())
         //{
         //   Rhino.Mocks.Expect.Call(session.Transaction)
         //       .Return(transaction)
         //       .Repeat.Any();
         //   Rhino.Mocks.Expect.Call(transaction.IsActive)
         //       .Return(true);
         //   Rhino.Mocks.Expect.Call(session.Get<Student>(Guid.Empty))
         //       .Return(expected);
         //}
         //using (mocks.Playback())
         //{
         //   IReadStudent StudentDao = new StudentDAOImpl(session);
         //   actual = StudentDao.GetById(Guid.Empty);
         //}
         //mocks.VerifyAll();
         //Assert.IsNotNull(actual);
         //Assert.AreSame(expected, actual);


      }



   }
}
