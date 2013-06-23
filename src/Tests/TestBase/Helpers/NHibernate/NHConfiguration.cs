namespace TestBase.Helpers.NHibernate
{
	#region Usings

	using System.Configuration;

	#endregion

	public class NHConfiguration
	{
		public static readonly string ConnectionString;
		private static readonly NHConfigurationSection section;

		/// <summary>
		/// 	Initializes the <see cref = "NHConfiguration" /> class.
		/// </summary>
		static NHConfiguration ()
		{
			section = ConfigurationManager.GetSection ( "dexter.nhibernate.core.configurationSection" ) as NHConfigurationSection;

			if ( section == null )
				throw new ConfigurationErrorsException ( "Maintenance section not found in the configuration file." );

			ConnectionString = ConfigurationManager.ConnectionStrings [ section.ConnectionStringName ].ConnectionString;
		}

		/// <summary>
		/// 	Gets the instance.
		/// </summary>
		/// <value>The instance.</value>
		public static NHConfigurationSection Instance
		{
			get { return section; }
		}
	}
}