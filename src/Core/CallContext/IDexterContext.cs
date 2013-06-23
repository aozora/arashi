 #region Disclaimer/Info
 
 /////////////////////////////////////////////////////////////////////////////////////////////////
 //
 //   File:		IDexterContext.cs
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
using System.Web;

namespace Dexter.Core.CallContext {
	public interface IDexterContext {

		/// <summary>
		/// Gets the context.
		/// </summary>
		/// <value>The context.</value>
		HttpContextWrapper Context { get; }
		
		/// <summary>
		/// Gets the tenant key.
		/// </summary>
		/// <value>The tenant key.</value>
		string TenantKey { get; }
		
		/// <summary>
		/// Return the current Principal.
		/// </summary>
		/// <value>The user.</value>
		IPrincipal User { get; }

		/// <summary>
		/// Determines whether [is web request].
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if [is web request]; otherwise, <c>false</c>.
		/// </returns>
		bool IsWebRequest { get; }
	}
}
