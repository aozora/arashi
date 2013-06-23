 #region Disclaimer/Info
 
 /////////////////////////////////////////////////////////////////////////////////////////////////
 //
 //   File:		DexterContext.cs
 //   Website:		http://dexterblogengine.com/
 //   Authors:		http://dexterblogengine.com/About.ashx
 //   Rev:		1
 //   Created:		19/01/2011
 //   Last edit:		19/01/2011
 //   License:		GNU Library General Public License (LGPL)
 // 
 //   For updated news and information please visit http://dexterblogengine.com/
 //   Dexter is hosted to Codeplex at http://dexterblogengine.codeplex.com
 //   For any question contact info@dexterblogengine.com
 //
 ///////////////////////////////////////////////////////////////////////////////////////////////////
 
 #endregion
 
using System.Security.Principal;
using System.Threading;
using System.Web;

namespace Dexter.Core.CallContext {
	public class DexterContext : IDexterContext {
		#region IDexterContext Members

		/// <summary>
		/// 	Gets the context.
		/// </summary>
		/// <value>The context.</value>
		public HttpContextWrapper Context {
			get {
				return IsWebRequest
				       	? new HttpContextWrapper ( HttpContext.Current )
				       	: null;
			}
		}

		/// <summary>
		/// 	Gets the tenant key.
		/// </summary>
		/// <value>The tenant key.</value>
		public string TenantKey {
			get { return null; }
		}

		/// <summary>
		/// 	Return the current Principal.
		/// </summary>
		/// <value>The user.</value>
		public IPrincipal User {
			get { return Thread.CurrentPrincipal; }
		}

		/// <summary>
		/// 	Determines whether [is web request].
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
