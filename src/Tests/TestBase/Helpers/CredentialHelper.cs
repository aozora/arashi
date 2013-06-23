namespace TestBase.Helpers
{
	#region Usings

	using System;
	using System.Security.Principal;
	using System.Threading;

	#endregion

	public class CredentialHelper : ITestHelper
	{
		private IPrincipal originalPrincipal;

		public CredentialHelper( string userName, string[] groups )
		{
			UserName=userName;
			Groups=groups;
		}

		public String UserName { get; set; }

		public String[] Groups { get; set; }

		#region ITestHelper Members

		public void FixtureSetUp()
		{
		}

		public void SetUp( BaseTestFixture fixture )
		{
			IIdentity identity=new GenericIdentity( UserName );
			IPrincipal principal=new GenericPrincipal( identity, Groups );
			originalPrincipal=Thread.CurrentPrincipal;
			Thread.CurrentPrincipal=principal;
		}

		public void TearDown( BaseTestFixture fixture )
		{
			Thread.CurrentPrincipal=originalPrincipal;
		}

		public void FixtureTearDown()
		{
		}

		#endregion
	}

	[AttributeUsage( AttributeTargets.Class | AttributeTargets.Method )]
	public class UseCredentialAttribute : Attribute, ITestHelperAttribute
	{
		public UseCredentialAttribute( string userName, string groups )
		{
			UserName=userName;
			Groups=groups;
		}

		public UseCredentialAttribute( string userName ) : this( userName, "Poster" )
		{
		}

		public String UserName { get; set; }

		public String Groups { get; set; }

		#region ITestHelperAttribute Members

		public ITestHelper Create()
		{
			return new CredentialHelper( UserName, Groups.Split( ',', ';' ) );
		}

		#endregion
	}
}