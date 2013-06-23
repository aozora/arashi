namespace TestBase.AutoMockingContainer
{
	#region Usings

	using System;
	using Castle.Core;

	#endregion

	public class StrategyKey
	{
		public StrategyKey( Type typeKey, string dependencyName )
		{
			TypeKey=typeKey;
			DependencyName=dependencyName;
		}

		public Type TypeKey { get; set; }
		public String DependencyName { get; set; }

		public Boolean IsValidFor( DependencyModel model )
		{
			if ( model.DependencyKey == DependencyName ||
			     model.TargetType == TypeKey )
				return true;

			return false;
		}
	}
}