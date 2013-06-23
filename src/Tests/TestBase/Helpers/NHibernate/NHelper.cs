namespace TestBase.Helpers.NHibernate
{
	#region Usings

	using System;
	using System.Diagnostics;

   //using Dexter.Core.NHibernate.Listener;

	using FluentNHibernate.Cfg;
	using FluentNHibernate.Cfg.Db;

	using global::NHibernate;
	using global::NHibernate.Cfg;
	using global::NHibernate.Tool.hbm2ddl;

	using Environment = global::NHibernate.Cfg.Environment;
   using Arashi.Core;

	#endregion

	public partial class NHelper
	{
		private static Configuration configuration;
		private static ISessionFactory sessionFactory;

		/// <summary>
		/// 	Due to static class helper, to make possible for test to change session
		/// 	factory per test, we need a way to override the SessionFactory when needed.
		/// </summary>
		private static ISessionFactory overrideSessionFactory;

		private static Configuration overrideConfiguration;

		static NHelper ()
		{
			IPersistenceConfigurer dbConfiguration = DbConfiguration ( NHConfiguration.Instance );
			CreateSessionFactory ( dbConfiguration );
		}

		public static ISessionFactory SessionFactory
		{
			get { return overrideSessionFactory ?? sessionFactory; }
		}

		public static Configuration Configuration
		{
			get { return overrideConfiguration ?? configuration; }
		}

		public static bool UseCache
		{
			get { return NHConfiguration.Instance.EnableCache; }
		}

		public static ISession GetSession ()
		{
			return SessionFactory.OpenSession ();
		}

		public static bool ShouldUpdateSchema ()
		{
			try
			{
				SchemaValidator validator = new SchemaValidator ( configuration );
				validator.Validate ();

				return false;
			}
			catch ( HibernateException )
			{
				return true;
			}
		}

		public static void UpdateSchema ()
		{
			SchemaUpdate update = new SchemaUpdate ( Configuration );
			update.Execute ( false , true );
		}

		public static void InitializeSchema ()
		{
			SchemaExport export = new SchemaExport ( Configuration );
			export.Execute ( false , true , false );
		}

		// todo: check this method: why create the factory before setting some configuration properties?
		private static void CreateSessionFactory ( IPersistenceConfigurer dbConfiguration )
		{
			FluentConfiguration cfg = GenerateFluentConfiguration ( dbConfiguration );

			configuration = cfg.BuildConfiguration ();
			// sessionFactory = cfg.BuildSessionFactory();

			// causes a bug with nvarchar(max) fields, and truncates the text
			// configuration.Properties[Environment.PrepareSql] = "true";
			configuration.Properties [ Environment.Isolation ] = "ReadCommitted";
			configuration.Properties [ Environment.UseSecondLevelCache ] = "true";
			configuration.Properties [ Environment.UseQueryCache ] = "true";

#if DEBUG
			configuration.Properties [ Environment.GenerateStatistics ] = "true";
#endif

         //configuration.EventListeners.SaveOrUpdateEventListeners = new[]
         //                                                            {
         //                                                               new EntityBaseSaveOrUpdateListener ()
         //                                                            };

			Environment.UseReflectionOptimizer = true;

			try
			{
				sessionFactory = cfg.BuildSessionFactory ();
			}
			catch ( Exception ex )
			{
				Debug.WriteLine ( ex.Message );
				throw;
			}
		}

		#region TestRelatedMethod

		/// <summary>
		/// 	override the session factory until the return object get disposed.
		/// </summary>
		/// <param name = "configurationSection">The new configuration section that will override
		/// 	the standard one configured.</param>
		/// <returns></returns>
		internal static IDisposable OverrideSessionFactory ( NHConfigurationSection configurationSection )
		{
			IPersistenceConfigurer dbConfiguration = DbConfiguration ( configurationSection );
			FluentConfiguration configuration = GenerateFluentConfiguration ( dbConfiguration );
			overrideSessionFactory = configuration.BuildSessionFactory ();
			overrideConfiguration = configuration.BuildConfiguration ();
			return new DisposableAction ( () =>
			                              	{
			                              		overrideSessionFactory = null;
			                              		overrideConfiguration = null;
			                              	} );
		}

		#endregion
	}
}