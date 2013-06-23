namespace TestBase.AutoMockingContainer
{
	#region Usings

	using Castle.Core.Configuration;
	using Castle.MicroKernel;

	#endregion

	public class AutoMockingFacility : IFacility
	{
		private readonly IAutoMockingRepository relatedRepository;

		public AutoMockingFacility( IAutoMockingRepository relatedRepository )
		{
			this.relatedRepository=relatedRepository;
		}

		#region IFacility Members

		public void Init( IKernel kernel, IConfiguration facilityConfig )
		{
			kernel.Resolver.AddSubResolver( new AutoMockingDependencyResolver( relatedRepository ) );
		}

		public void Terminate()
		{
		}

		#endregion
	}
}