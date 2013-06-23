namespace TestBase.AutoMockingContainer
{
	#region Usings

	using Castle.Core;
	using Castle.MicroKernel;
	using Rhino.Mocks;

	#endregion

	public class AutoMockingDependencyResolver : ISubDependencyResolver
	{
		private readonly IAutoMockingRepository _relatedRepository;

		public AutoMockingDependencyResolver( IAutoMockingRepository relatedRepository )
		{
			_relatedRepository=relatedRepository;
		}

		#region ISubDependencyResolver Members

		public bool CanResolve(
			CreationContext context,
			ISubDependencyResolver parentResolver,
			ComponentModel model,
			DependencyModel dependency )
		{
			return dependency.DependencyType == DependencyType.Service;
		}

		public object Resolve(
			CreationContext context,
			ISubDependencyResolver parentResolver,
			ComponentModel model,
			DependencyModel dependency )
		{
			MockingStrategy strategy=_relatedRepository.GetStrategyFor( dependency );

			if ( strategy.Instance != null )
				return strategy.Instance;
			if ( strategy.Mock == MockingStrategyType.Mock )
				return MockRepository.GenerateStub( dependency.TargetType );
			if ( strategy.Mock == MockingStrategyType.Resolve )
				return _relatedRepository.Resolve( dependency.TargetType );

			return null;
		}

		#endregion
	}
}