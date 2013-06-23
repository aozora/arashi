using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Arashi.Core.Cms.Domain;
using Arashi.Core.NHibernate;
using Arashi.Core.Repositories;
using NHibernate;
using NUnit.Framework;

namespace Arashi.Core.Cms.Test
{
   [TestFixture]
   public class InMemoryTests : NHibernateInMemoryTestFixtureBase
   {
      //private ISession session;


      [TestFixtureSetUp]
      public void OneTimeTestInitialize()
      {
         OneTimeInitalize(typeof(User).Assembly);
      }



      [SetUp]
      public void TestInitialize()
      {
         //session = CreateSessionFactory();
         //CreateSessionFactory();
      }



      [TearDown]
      public void TestCleanup()
      {
         DisposeSession();
      }


      [Test]
      public void CanSaveAndLoadUser()
      {
         ISession session = RepositoryHelper.GetSession();

         Debug.WriteLine("session.IsOpen = " + session.IsOpen.ToString());

         Site site = new Site();
         site.Name = "Test Site";
         site.Email = "test@arashi.com";

         User user = new User();
         user.Email = "test@arashi.com";
         user.Password = "password";
         user.IsActive = true;
         user.Site = site;
         //user.CreatedBy = 
         //user.CreatedDate = 

         using (ITransaction tx = session.BeginTransaction())
         {
            session.SaveOrUpdate(site);
            session.SaveOrUpdate(user);
            tx.Commit();
         }

         session.Evict(user);//remove from session cache

         User loaded = session.Load<User>(user.UserId);

         Assert.That(loaded, Is.EqualTo(user));
      }


      
      [Test]
      public void CanSaveAndLoadUser_Mixed()
      {
         ISession session = RepositoryHelper.GetSession();

         Debug.WriteLine("session.IsOpen = " + session.IsOpen.ToString());

         Site site = new Site();
         site.Name = "Test Site";
         site.Email = "test@arashi.com";

         User user = new User();
         user.Email = "test@arashi.com";
         user.Password = "password";
         user.IsActive = true;
         user.Site = site;
         //user.CreatedBy = 
         //user.CreatedDate = 

         using (ITransaction tx = session.BeginTransaction())
         {
            //session.SaveOrUpdate(site);
            //session.SaveOrUpdate(user);
            Repository<Site>.Save(site);
            Repository<User>.Save(user);
           tx.Commit();
         }

         session.Evict(user);//remove from session cache

         User loaded = session.Load<User>(user.UserId);

         Assert.That(loaded, Is.EqualTo(user));
      }


      
      [Test]
      public void CanSaveAndLoadUser_With_NHTransaction_And_NHRepository()
      {
         Site site = new Site();
         site.Name = "Test Site";
         site.Email = "test@arashi.com";

         User user = new User();
         user.Email = "test@arashi.com";
         user.Password = "password";
         user.IsActive = true;
         user.Site = site;
         //user.CreatedBy = 
         //user.CreatedDate = 

         using (NHTransactionScope tx = new NHTransactionScope())
         {
            Repository<Site>.Save(site);
            Repository<User>.Save(user);
            tx.VoteCommit();
         }

         ISession session = RepositoryHelper.GetSession();
         session.Evict(user);//remove from session cache

         User loaded = session.Load<User>(user.UserId);

         Assert.That(loaded, Is.EqualTo(user));


      }

   }
}