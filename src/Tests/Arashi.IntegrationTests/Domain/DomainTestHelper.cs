using System;
using Arashi.Core.Domain;
using Rhino.Mocks;

namespace Arashi.IntegrationTests.Domain
{
   public class DomainTestHelper
   {
      /// <summary>
      /// Get a standard site for test
      /// </summary>
      /// <returns></returns>
      public static Site GetTestSite()
      {
         Site site = new Site(); // MockRepository.GenerateStub<Site>();
         site.Name = "Test";
         site.Description = "test";
         site.DefaultCulture = "en-US";
         site.DefaultPage = null;
         site.DefaultRole = null;
         site.Description = "Test site";
         site.Email = "test@email.com";
         site.Status = SiteStatus.Online;
         site.Theme = null;
         site.TimeZone = 60;
         site.CreatedDate = DateTime.Now;

         SiteHost host = MockRepository.GenerateStub<SiteHost>();
         host.HostName = "localhost";
         host.IsDefault = true;
         host.Site = site;

         site.Hosts.Add(host);


         return site;
      }



      /// <summary>
      /// Get a standard user for a given site
      /// </summary>
      public static User GetTestUser()
      {
         return GetTestUser(GetTestSite());
      }



      /// <summary>
      /// Get a standard user for a given site
      /// </summary>
      /// <param name="site"></param>
      /// <returns></returns>
      public static User GetTestUser(Site site)
      {
         User user = new User()
         {
            Email = "test@arashi.com",
            DisplayName = "test",
            Password = "password",
            CreatedDate = DateTime.Now,
            IsActive = true,
            Site = site
         };

         return user;
      }
   }
}
