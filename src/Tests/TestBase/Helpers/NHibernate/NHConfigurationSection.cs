namespace TestBase.Helpers.NHibernate
{
	#region Usings

	using System.Configuration;

	#endregion

	public class NHConfigurationSection : ConfigurationSection
	{
		[ConfigurationProperty ( "connectionStringName" , IsRequired = true )]
		public string ConnectionStringName
		{
			get { return ( string ) this [ "connectionStringName" ]; }
			set { this [ "connectionStringName" ] = value; }
		}

		[ConfigurationProperty ( "enableCache" , DefaultValue = true , IsRequired = true )]
		public bool EnableCache
		{
			get { return ( bool ) this [ "enableCache" ]; }
			set { this [ "enableCache" ] = value; }
		}

		[ConfigurationProperty ( "cacheProvider" , IsRequired = true )]
		public string CacheProvider
		{
			get { return ( string ) this [ "cacheProvider" ]; }
			set { this [ "cacheProvider" ] = value; }
		}

		[ConfigurationProperty ( "databaseType" , IsRequired = true )]
		public string DatabaseType
		{
			get { return ( string ) this [ "databaseType" ]; }
			set { this [ "databaseType" ] = value; }
		}
	}
}