namespace TestBase.AutoMockingContainer
{
	#region Usings

	using System;

	#endregion

	public enum MockingStrategyType
	{
		Mock,
		Resolve,
		NoAction
	}

	public class MockingStrategy
	{
		public static readonly MockingStrategy Default=new MockingStrategy {Mock=MockingStrategyType.Mock};
		public static readonly MockingStrategy NoAction=new MockingStrategy {Mock=MockingStrategyType.NoAction};
		public static readonly MockingStrategy Resolve=new MockingStrategy {Mock=MockingStrategyType.Resolve};

		public MockingStrategy( object instance, MockingStrategyType mock )
		{
			Instance=instance;
			Mock=mock;
		}

		public MockingStrategy()
		{
			Mock=MockingStrategyType.Mock;
		}

		public Object Instance { get; set; }
		public MockingStrategyType Mock { get; set; }
	}
}