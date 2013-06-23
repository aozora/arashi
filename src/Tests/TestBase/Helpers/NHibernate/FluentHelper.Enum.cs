namespace TestBase.Helpers.NHibernate
{
	public partial class NHelper
	{
		private enum DbType
		{
			MsSql,
			MySql,
			Oracle9,
			Oracle10,
			PostgreSqlStandard,
			PostgreSql81,
			PostgreSql82,
			SqlLite,
			SqlLiteInMemory
		}
	}
}