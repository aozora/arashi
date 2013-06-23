using System;
using NHibernate.Cfg;

namespace Arashi.Core.NHibernate
{
   /// <summary>
   /// The implementation of <see cref="INHibernateHelper"/>.
   /// </summary>
   public class NHibernateHelper : INHibernateHelper
   {
      global::NHibernate.Cfg.Configuration configuration;

      /// <summary>
      /// Gets or sets the session factory.
      /// </summary>
      /// <value>The session factory.</value>
      public global::NHibernate.ISessionFactory SessionFactory
      {
         get;
         set;
      }


      /// <summary>
      /// Return the NHibernate configuration.
      /// </summary>
      /// <value>The configuration.</value>
      public global::NHibernate.Cfg.Configuration Configuration
      {
         get
         {
            return configuration;
         }
      }



      /// <summary>
      /// Configures NHibernate
      /// </summary>
      public void Configure()
      {
         // Arashi.Core.NHibernate

         configuration = new Configuration();
         configuration.Configure();
         configuration = configuration.AddAssembly("Arashi.Core.NHibernate");

         SessionFactory = configuration.BuildSessionFactory();
      }



      /// <summary>
      /// Gets the session.
      /// </summary>
      /// <returns></returns>
      public global::NHibernate.ISession GetSession()
      {
         return SessionFactory.OpenSession();
      }

      ///// <summary>
      ///// Gets a value indicating whether [use cache].
      ///// </summary>
      ///// <value>
      ///// 	<c>true</c> if NHibernate should cache the queries; otherwise, <c>false</c>.
      ///// </value>
      //public bool UseCache
      //{
      //   get
      //   {
      //      return hostConfiguration.NHibernate.EnableCache;
      //   }
      //}

      ///// <summary>
      ///// Checks if the database schema should be updated.
      ///// </summary>
      ///// <returns>
      ///// 	<c>True</c> if the schema requires the update;  otherwise, <c>false</c>.
      ///// </returns>
      //public bool ShouldUpdateSchema()
      //{
      //   var validator = new SchemaValidator(configuration);
      //   validator.Validate();

      //   return false;
      //}

      ///// <summary>
      ///// Updates the database schema.
      ///// </summary>
      //public void UpdateSchema()
      //{
      //   var update = new SchemaUpdate(configuration);
      //   update.Execute(false, true);
      //}

      ///// <summary>
      ///// Initializes the schema.
      ///// </summary>
      //public void InitializeSchema()
      //{
      //   var export = new SchemaExport(configuration);
      //   export.Execute(false, true, false);
      //}

      ///// <summary>
      ///// Drops the schema.
      ///// </summary>
      //public void DropSchema()
      //{
      //   var export = new SchemaExport(configuration);
      //   export.Execute(false, true, true);
      //}

      ///// <summary>
      ///// Gets the get connection string.
      ///// </summary>
      ///// <value>The get connection string.</value>
      //public string GetConnectionString
      //{
      //   get
      //   {
      //      //return configuration.Properties["connection.connection_string"];
      //   }
      //}

      //static void GetDataBaseDialect(IEnvironmentConfiguration environmentConfiguration, IDbIntegrationConfigurationProperties db, string dbSchema, out string databaseObjects)
      //{
      //   var databaseType = (DbType)Enum.Parse(typeof(DbType), environmentConfiguration.NHibernate.DatabaseType);
      //   switch (databaseType)
      //   {
      //      case DbType.MsSql2000:
      //         db.Dialect<MsSqlServer2000Dialect>();
      //         db.Driver<SqlClientDriver>();
      //         databaseObjects = Resources.DatabaseObjects.MsSQL2000;

      //         if (databaseObjects != null)
      //            databaseObjects = Regex.Replace(databaseObjects, @"(\[.*\])", string.Format("[{0}].$1", dbSchema));

      //         break;
      //      case DbType.MsSql2005:
      //         db.Dialect<MsSqlServer2005Dialect>();
      //         db.Driver<SqlClientDriver>();
      //         databaseObjects = Resources.DatabaseObjects.MsSQL2005;

      //         if (databaseObjects != null)
      //            databaseObjects = Regex.Replace(databaseObjects, @"(\[.*\])", string.Format("[{0}].$1", dbSchema));

      //         break;
      //      case DbType.MsSql2008:
      //         db.Dialect<MsSqlServer2008Dialect>();
      //         db.Driver<SqlClientDriver>();
      //         databaseObjects = Resources.DatabaseObjects.MsSQL2008;

      //         if (databaseObjects != null)
      //            databaseObjects = Regex.Replace(databaseObjects, @"(\[.*\])", string.Format("[{0}].$1", dbSchema));

      //         break;
      //      case DbType.Azure:
      //         db.Dialect<MsSqlAzureDialect>();
      //         db.Driver<SqlClientDriver>();
      //         databaseObjects = Resources.DatabaseObjects.MsSQL2008;

      //         if (databaseObjects != null)
      //            databaseObjects = Regex.Replace(databaseObjects, @"(\[.*\])", string.Format("[{0}].$1", dbSchema));

      //         break;
      //      case DbType.MySql:
      //         db.Dialect<MySqlDialect>();
      //         db.Driver<MySqlDataDriver>();
      //         databaseObjects = null;
      //         break;
      //      case DbType.MySql5:
      //         db.Dialect<MySql5Dialect>();
      //         db.Driver<MySqlDataDriver>();
      //         databaseObjects = null;
      //         break;
      //      case DbType.Oracle9:
      //         db.Dialect<Oracle9iDialect>();
      //         db.Driver<OracleClientDriver>();
      //         databaseObjects = null;
      //         break;
      //      case DbType.Oracle10:
      //         db.Dialect<Oracle10gDialect>();
      //         db.Driver<OracleClientDriver>();
      //         databaseObjects = null;
      //         break;
      //      case DbType.SqlLite:
      //         db.Dialect<SQLiteDialect>();
      //         db.Driver<SQLite20Driver>();
      //         databaseObjects = null;
      //         break;
      //      case DbType.SqlLiteInMemory:
      //         db.Dialect<SQLiteDialect>();
      //         db.Driver<SQLite20Driver>();
      //         databaseObjects = null;
      //         break;
      //      default:
      //         throw new ConfigurationErrorsException("The specified database is not supported");
      //   }
      //}

      //HbmMapping GetMapping()
      //{
      //   var mapper = new DomainMapper(configuration);
      //   return mapper.Mapper.CompileMappingFor(mapper.DomainEntities);
      //}

      //void SetEntityCache()
      //{
      //   configuration.Properties[global::NHibernate.Cfg.Environment.UseSecondLevelCache] = "true";
      //   configuration.Properties[global::NHibernate.Cfg.Environment.UseQueryCache] = "true";
      //   configuration.Properties[global::NHibernate.Cfg.Environment.CacheProvider] = hostConfiguration.NHibernate.CacheProvider;
      //   configuration.Properties[global::NHibernate.Cfg.Environment.CacheDefaultExpiration] = CacheRules.DefaultTimeOut.ToString();

      //   configuration.QueryCache()
      //      .ResolveRegion(CacheRules.Regions.Tolerant)
      //      .Using<TolerantQueryCache>()
      //      .AlwaysTolerant();

      //   configuration.EntityCache<Item>(ce =>
      //   {
      //      ce.Strategy = EntityCacheUsage.ReadWrite;
      //      ce.Collection(e => e.Tags, cc =>
      //      {
      //         cc.Strategy = EntityCacheUsage.ReadWrite;
      //         ce.RegionName = CacheRules.Regions.Tag;
      //      });
      //      ce.Collection(e => e.Comments, cc =>
      //      {
      //         cc.Strategy = EntityCacheUsage.ReadWrite;
      //         ce.RegionName = CacheRules.Regions.Comment;
      //      });
      //      ce.RegionName = CacheRules.Regions.Post;
      //   });

      //   configuration.EntityCache<Category>(ce =>
      //   {
      //      ce.Strategy = EntityCacheUsage.ReadWrite;
      //      ce.Collection(e => e.Categories, cc =>
      //      {
      //         cc.Strategy = EntityCacheUsage.ReadWrite;
      //         ce.RegionName = CacheRules.Regions.Category;
      //      });
      //      ce.RegionName = CacheRules.Regions.Category;
      //   });

      //   configuration.EntityCache<BlogRoll>(ce =>
      //   {
      //      ce.Strategy = EntityCacheUsage.NonStrictReadWrite;
      //      ce.RegionName = CacheRules.Regions.Tolerant;
      //   });

      //   configuration.EntityCache<Role>(ce =>
      //   {
      //      ce.Strategy = EntityCacheUsage.ReadWrite;
      //      ce.RegionName = CacheRules.Regions.User;
      //   });

      //   configuration.EntityCache<User>(ce =>
      //   {
      //      ce.Strategy = EntityCacheUsage.ReadWrite;
      //      ce.Collection(e => e.Roles, cc =>
      //      {
      //         cc.Strategy = EntityCacheUsage.ReadWrite;
      //         ce.RegionName = CacheRules.Regions.User;
      //      });
      //      ce.RegionName = CacheRules.Regions.User;
      //   });

      //   configuration.EntityCache<TagItem>(ce =>
      //   {
      //      ce.Strategy = EntityCacheUsage.NonStrictReadWrite;
      //      ce.RegionName = CacheRules.Regions.Tag;
      //   });

      //   configuration.EntityCache<EmailMessage>(ce => ce.Strategy = EntityCacheUsage.ReadWrite);
      //}
   }
}
