using Arashi.IntegrationTests.Domain;

namespace Arashi.Core.Cms.Test.Services.Membership
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Text;
   using Arashi.Core.Domain;
   using Arashi.IntegrationTests.Infrastructure.DataAccess;
   using Arashi.Services.Membership;
   using NUnit.Framework;
   using Rhino.Mocks;


   // TODO: remove inheritance from InMemoryTests
   [TestFixture]
   public class MembershipTest : InMemoryTests
   {

      [Test]
      public void AuthenticateUserTest()
      {
         string userName = "test@arashi.com";
         string password = "password";
         string ip = "127.0.0.1";

         IAuthenticationService mock = MockRepository.GenerateMock<IAuthenticationService>();
         IUserService stubUserService = MockRepository.GenerateStub<IUserService>();
         //HttpRequestBase requestStub = MockRepository.GenerateStub<HttpRequestBase> ( );

         Site site = DomainTestHelper.GetTestSite();
         User testUser = DomainTestHelper.GetTestUser(site);

         Expect.Call(mock.AuthenticateUser(site, "test@arashi.com", "password", true)).Return(testUser);

         //mock.Expect(m => m.AuthenticateUser(site, "test@arashi.com", "password", true))
         mock.VerifyAllExpectations();

         //User loggedUser = stub.Stub(x => x.AuthenticateUser(site, "test@arashi.com", "password", true));



         //var site = new Mock<Site>();
         //site.SetupGet(s => s.SiteId).Returns(0);
         //site.SetupGet(s => s.Name).Returns("Test Site");

         //var user = new Mock<User>();
         //user.SetupGet(u => u.UserId).Returns(0);
         //user.SetupGet(u => u.Email).Returns(userName);
         //user.SetupGet(u => u.Password).Returns(password);
         //user.SetupGet(u => u.LastIp).Returns(ip);

         //service
         //   .SetupGet(s => s.AuthenticateUser(site.Object, 
         //                                     "test_user", 
         //                                     "test_password", 
         //                                     "127.0.0.1", 
         //                                     false)
         //   )
         //   .Returns(user.Object)
         //   .Verifiable();

         //service.Verify();

      }


   }

}
