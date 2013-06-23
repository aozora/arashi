namespace TestBase.Helpers.NHibernate
{
	#region using

	using System;
	using System.Configuration;
	using FluentNHibernate.Cfg.Db;

	#endregion

	public partial class NHelper
	{
		private static IPersistenceConfigurer DbConfiguration(NHConfigurationSection nhConfiguration)
		{
			string dbType = nhConfiguration.DatabaseType;

			if (string.IsNullOrEmpty(dbType))
				throw new ConfigurationErrorsException("You must specify the Database type!");

			//string dbFileName = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "\\App_Data\\", "");

			try
			{
				DbType db = (DbType) Enum.Parse(typeof (DbType), dbType);
				switch (db)
				{
					case DbType.MsSql:
						return MsSqlConfigureDatabase(nhConfiguration.ConnectionStringName, nhConfiguration.EnableCache);
					case DbType.MySql:
						return MySqlConfigureDatabase(nhConfiguration.ConnectionStringName, nhConfiguration.EnableCache);
					case DbType.Oracle9:
						return Oracle9ConfigureDatabase(nhConfiguration.ConnectionStringName, nhConfiguration.EnableCache);
					case DbType.Oracle10:
						return Oracle10ConfigureDatabase(nhConfiguration.ConnectionStringName, nhConfiguration.EnableCache);
					case DbType.PostgreSqlStandard:
						return PostgreSqlStandardConfigureDatabase(nhConfiguration.ConnectionStringName, nhConfiguration.EnableCache);
					case DbType.PostgreSql81:
						return PostgreSql81ConfigureDatabase(nhConfiguration.ConnectionStringName, nhConfiguration.EnableCache);
					case DbType.PostgreSql82:
						return PostgreSql82ConfigureDatabase(nhConfiguration.ConnectionStringName, nhConfiguration.EnableCache);
					case DbType.SqlLite:
						return SqlLiteConfigureDatabase(nhConfiguration.ConnectionStringName, nhConfiguration.EnableCache);
					case DbType.SqlLiteInMemory:
						return SqlLiteMemoryConfigureDatabase(nhConfiguration.EnableCache);
					default:
						throw new ConfigurationErrorsException("The specified database is not supported");
				}
			}
			catch
			{
				throw new ConfigurationErrorsException("The specified database is not supported");
			}
		}
	}
}