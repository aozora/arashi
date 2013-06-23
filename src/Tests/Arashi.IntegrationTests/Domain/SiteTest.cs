namespace Arashi.IntegrationTests.Domain
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Text;
   using NUnit.Framework;
   using Arashi.Core.Domain;
   using Arashi.IntegrationTests.Infrastructure.DataAccess;
   using SharpTestsEx;


   [TestFixture]
   public class SiteTest : InMemoryTests
   {
      [Test]
      public void CanSaveAndLoadSite()
      {
         Site site = DomainTestHelper.GetTestSite();

         session.Save(site);
         session.Flush();

         session.Evict(site); //remove from session cache

         Site loaded = session.Load<Site>(site.SiteId);
         //Assert.AreEqual(site.Name, loaded.Name);

         loaded.Name.Should().Be.EqualTo(site.Name);
      }

   }
}
