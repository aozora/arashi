namespace Arashi.Core.CallContext
{
	/// <summary>
	/// Th contract for the ICallContext
	/// </summary>
	public interface ICallContext
	{
		/// <summary>
		/// 	Gets the items.
		/// </summary>
		/// <value>The items.</value>
		object this[string key] { get; set; }
	}
}
