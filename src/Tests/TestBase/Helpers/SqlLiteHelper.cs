namespace TestBase.Helpers
{
   #region Usings

   using System;

   #endregion

   public class SqlLiteHelper : ITestHelper
   {
      public SqlLiteHelper(string sqlLiteConnectionString, bool recreateDb)
      {
         SqlLiteConnectionString = sqlLiteConnectionString;
         RecreateDb = recreateDb;
      }

      public String SqlLiteConnectionString
      {
         get;
         set;
      }

      public Boolean RecreateDb
      {
         get;
         set;
      }

      private IDisposable RestoreNHibernateSessionManager
      {
         get;
         set;
      }

      #region ITestHelper Members

      public void FixtureSetUp()
      {
         NHibernate.NHConfigurationSection section = new NHibernate.NHConfigurationSection();
         section.ConnectionStringName = SqlLiteConnectionString;
         section.DatabaseType = "SqlLite";
         RestoreNHibernateSessionManager = NHibernate.NHelper.OverrideSessionFactory(section);
         if (RecreateDb)
            NHibernate.NHelper.InitializeSchema();
      }

      public void SetUp(BaseTestFixture fixture)
      {
      }

      public void TearDown(BaseTestFixture fixture)
      {
      }

      public void FixtureTearDown()
      {
         RestoreNHibernateSessionManager.Dispose();
      }

      #endregion
   }



   [AttributeUsage(AttributeTargets.Class)]
   public class UseSqlLiteAttribute : Attribute, ITestHelperAttribute
   {
      public UseSqlLiteAttribute()
         : this("sqlliteconnstring", true)
      {
      }

      public UseSqlLiteAttribute(string sqlLiteConnectionString, bool recreateDb)
      {
         SqlLiteConnectionString = sqlLiteConnectionString;
         RecreateDb = recreateDb;
      }

      public String SqlLiteConnectionString
      {
         get;
         set;
      }

      public Boolean RecreateDb
      {
         get;
         set;
      }

      #region ITestHelperAttribute Members

      public ITestHelper Create()
      {
         return new SqlLiteHelper(SqlLiteConnectionString, RecreateDb);
      }

      #endregion
   }
}