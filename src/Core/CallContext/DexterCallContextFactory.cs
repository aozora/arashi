 #region Disclaimer/Info
 
 /////////////////////////////////////////////////////////////////////////////////////////////////
 //
 //   File:		DexterCallContextFactory.cs
 //   Website:		http://dexterblogengine.com/
 //   Authors:		http://dexterblogengine.com/About.ashx
 //   Rev:		1
 //   Created:		19/01/2011
 //   Last edit:		19/01/2011
 //   License:		GNU Library General Public License (LGPL)
 //   File:            DexterCallContextFactory.cs
 //   For updated news and information please visit http://dexterblogengine.com/
 //   Dexter is hosted to Codeplex at http://dexterblogengine.codeplex.com
 //   For any question contact info@dexterblogengine.com
 //
 ///////////////////////////////////////////////////////////////////////////////////////////////////
 
 #endregion
 
using System.Web;

namespace Dexter.Core.CallContext {
	public class DexterCallContextFactory : ICallContextFactory {
		readonly ICallContext contextAsync;
		readonly ICallContext contextWeb;

		/// <summary>
		/// Initializes a new instance of the <see cref="DexterCallContextFactory"/> class.
		/// </summary>
		/// <param name="contextWeb">The context web.</param>
		/// <param name="contextAsync">The context async.</param>
		public DexterCallContextFactory ( IWebCallContext contextWeb , IAsyncCallContext contextAsync ) {
			this.contextWeb = contextWeb;
			this.contextAsync = contextAsync;
		}

		#region ICallContextFactory Members

		/// <summary>
		/// Return the WebCallContext or the ThreadCallContext
		/// </summary>
		/// <returns></returns>
		public ICallContext RetrieveCallContext ( ) {
			return !this.IsWebRequest
			       	? contextAsync
			       	: contextWeb;
		}

		/// <summary>
		/// Determines whether [is web request].
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if [is web request]; otherwise, <c>false</c>.
		/// </returns>
		public bool IsWebRequest {
			get { return HttpContext.Current != null; }
		}

		#endregion
	}
}
