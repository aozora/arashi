//using Dexter.Core.NHibernate.Entities;
//using Dexter.Core.NHibernate.Listener;
using FluentNHibernate.Cfg;

namespace TestBase.Helpers.NHibernate
{
	#region using

	using System;
	using FluentNHibernate.Cfg.Db;
   using Arashi.Core.Domain;

	#endregion

	public partial class NHelper
	{
		private static IPersistenceConfigurer MsSqlConfigureDatabase(string connectionStringConfigKey, bool useCache)
		{
			MsSqlConfiguration cfg = MsSqlConfiguration.MsSql2005
				.ConnectionString(c => c.FromConnectionStringWithKey(connectionStringConfigKey))
				.UseOuterJoin()
				.ProxyFactoryFactory("NHibernate.ByteCode.LinFu.ProxyFactoryFactory, NHibernate.ByteCode.LinFu")
				.DefaultSchema("dbo");

#if DEBUG
			cfg.ShowSql();
#endif

			if (useCache)
			{
				ValidateCacheProvider();

				cfg.Cache(c => c.UseQueryCache().UseMinimalPuts().ProviderClass(NHConfiguration.Instance.CacheProvider));
			}

			return cfg;
		}

		private static IPersistenceConfigurer MySqlConfigureDatabase(string connectionStringConfigKey, bool useCache)
		{
			MySQLConfiguration cfg = MySQLConfiguration.Standard
				.ConnectionString(x => x.FromConnectionStringWithKey(connectionStringConfigKey))
				.UseOuterJoin()
				.DefaultSchema("dbo");

#if DEBUG
			cfg.ShowSql();
#endif

			if (useCache)
			{
				ValidateCacheProvider();

				cfg.Cache(c => c.UseQueryCache().UseMinimalPuts().ProviderClass(NHConfiguration.Instance.CacheProvider));
			}

			return cfg;
		}

		private static IPersistenceConfigurer Oracle9ConfigureDatabase(string connectionStringConfigKey, bool useCache)
		{
			OracleDataClientConfiguration cfg = OracleDataClientConfiguration.Oracle9
				.ConnectionString(x => x.FromConnectionStringWithKey(connectionStringConfigKey))
				.UseOuterJoin()
				.DefaultSchema("dbo");

#if DEBUG
			cfg.ShowSql();
#endif

			if (useCache)
			{
				ValidateCacheProvider();

				cfg.Cache(c => c.UseQueryCache().UseMinimalPuts().ProviderClass(NHConfiguration.Instance.CacheProvider));
			}

			return cfg;
		}

		private static IPersistenceConfigurer Oracle10ConfigureDatabase(string connectionStringConfigKey, bool useCache)
		{
			OracleDataClientConfiguration cfg = OracleDataClientConfiguration.Oracle10
				.ConnectionString(x => x.FromConnectionStringWithKey(connectionStringConfigKey))
				.UseOuterJoin()
				.DefaultSchema("dbo");

#if DEBUG
			cfg.ShowSql();
#endif

			if (useCache)
			{
				ValidateCacheProvider();

				cfg.Cache(c => c.UseQueryCache().UseMinimalPuts().ProviderClass(NHConfiguration.Instance.CacheProvider));
			}

			return cfg;
		}

		private static IPersistenceConfigurer PostgreSqlStandardConfigureDatabase(string connectionStringConfigKey, bool useCache)
		{
			PostgreSQLConfiguration cfg = PostgreSQLConfiguration.Standard
				.ConnectionString(x => x.FromConnectionStringWithKey(connectionStringConfigKey))
				.UseOuterJoin()
				.DefaultSchema("dbo");

#if DEBUG
			cfg.ShowSql();
#endif

			if (useCache)
			{
				ValidateCacheProvider();

				cfg.Cache(c => c.UseQueryCache().UseMinimalPuts().ProviderClass(NHConfiguration.Instance.CacheProvider));
			}

			return cfg;
		}

		private static IPersistenceConfigurer PostgreSql81ConfigureDatabase(string connectionStringConfigKey, bool useCache)
		{
			PostgreSQLConfiguration cfg = PostgreSQLConfiguration.PostgreSQL81
				.ConnectionString(x => x.FromConnectionStringWithKey(connectionStringConfigKey))
				.UseOuterJoin()
				.DefaultSchema("dbo");

#if DEBUG
			cfg.ShowSql();
#endif

			if (useCache)
			{
				ValidateCacheProvider();

				cfg.Cache(c => c.UseQueryCache().UseMinimalPuts().ProviderClass(NHConfiguration.Instance.CacheProvider));
			}

			return cfg;
		}

		private static IPersistenceConfigurer PostgreSql82ConfigureDatabase(string connectionStringConfigKey, bool useCache)
		{
			PostgreSQLConfiguration cfg = PostgreSQLConfiguration.PostgreSQL82
				.ConnectionString(x => x.FromConnectionStringWithKey(connectionStringConfigKey))
				.UseOuterJoin()
				.DefaultSchema("dbo");

#if DEBUG
			cfg.ShowSql();
#endif

			if (useCache)
			{
				ValidateCacheProvider();

				cfg.Cache(c => c.UseQueryCache().UseMinimalPuts().ProviderClass(NHConfiguration.Instance.CacheProvider));
			}

			return cfg;
		}

		private static IPersistenceConfigurer SqlLiteConfigureDatabase(string connectionStringConfigKey, bool useCache)
		{
			SQLiteConfiguration cfg = SQLiteConfiguration.Standard
				//.UsingFile(filePath)
				.ConnectionString(x => x.FromConnectionStringWithKey(connectionStringConfigKey))
				.ProxyFactoryFactory("NHibernate.ByteCode.LinFu.ProxyFactoryFactory, NHibernate.ByteCode.LinFu")
				.UseOuterJoin()
				.DefaultSchema("dbo");

#if DEBUG
			cfg.ShowSql();
#endif

			if (useCache)
			{
				ValidateCacheProvider();
				cfg.Cache(c => c.UseQueryCache().UseMinimalPuts().ProviderClass(NHConfiguration.Instance.CacheProvider));
			}

			return cfg;
		}

		private static IPersistenceConfigurer SqlLiteMemoryConfigureDatabase(bool useCache)
		{
			SQLiteConfiguration cfg = SQLiteConfiguration.Standard
				.InMemory()
				.UseOuterJoin()
				.DefaultSchema("dbo");

			if (useCache)
			{
				ValidateCacheProvider();

				cfg.Cache(c => c.UseQueryCache().UseMinimalPuts().ProviderClass(NHConfiguration.Instance.CacheProvider));
			}

			return cfg;
		}

		private static void ValidateCacheProvider()
		{
			if (string.IsNullOrEmpty(NHConfiguration.Instance.CacheProvider))
				throw new ArgumentException("Invaldia Cache Provider");
		}


		private static FluentConfiguration GenerateFluentConfiguration(IPersistenceConfigurer dbConfiguration)
		{
         //return Fluently.Configure()
         //   .Database(dbConfiguration)
         //   .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Item>());
         return Fluently.Configure()
                  .Database(dbConfiguration)
                  .Mappings(m => m.HbmMappings.AddFromAssemblyOf<ContentItem>());
      }
	}
}