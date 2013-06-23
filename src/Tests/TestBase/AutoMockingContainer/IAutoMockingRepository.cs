namespace TestBase.AutoMockingContainer
{
	#region Usings

	using System;
	using Castle.Core;
	using Castle.Windsor;

	#endregion

	public interface IAutoMockingRepository : IWindsorContainer
	{
		MockingStrategy GetStrategyFor( DependencyModel model );
		void AddStrategy( Type serviceType, MockingStrategy strategy );
	}
}