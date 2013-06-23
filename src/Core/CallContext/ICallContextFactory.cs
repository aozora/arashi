namespace Arashi.Core.CallContext
{
	/// <summary>
	/// This class resolve the context calls implementations
	/// </summary>
	public interface ICallContextFactory
	{

		/// <summary>
		/// Return the WebCallContext or the ThreadCallContext
		/// </summary>
		/// <returns></returns>
		ICallContext RetrieveCallContext ();

		/// <summary>
		/// Determines whether [is web request].
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if [is web request]; otherwise, <c>false</c>.
		/// </returns>
		bool IsWebRequest { get; }
	}
}
