using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arashi.Core.Cms.Domain;
using Arashi.Core.Cms.Services.Membership;
using NUnit.Framework;
using Moq;

namespace Arashi.Core.Cms.Test.Services.Membership
{
   [TestFixture]
   public class AuthenticationServiceTest
   {

      [Test]
      public void LogInTest()
      {
         string userName = "test@arashi.com";
         string password = "password";
         string ip = "127.0.0.1";


         var service = new Mock<IAuthenticationService>();

         var site = new Mock<Site>();
         site.SetupGet(s => s.SiteId).Returns(0);
         site.SetupGet(s => s.Name).Returns("Test Site");

         var user = new Mock<User>();
         user.SetupGet(u => u.UserId).Returns(0);
         user.SetupGet(u => u.Email).Returns(userName);
         user.SetupGet(u => u.Password).Returns(password);
         user.SetupGet(u => u.LastIp).Returns(ip);

         service
            .SetupGet(s => s.AuthenticateUser(site.Object, 
                                              "test_user", 
                                              "test_password", 
                                              "127.0.0.1", 
                                              false)
            )
            .Returns(user.Object)
            .Verifiable();

         service.Verify();

      }


   }

}
