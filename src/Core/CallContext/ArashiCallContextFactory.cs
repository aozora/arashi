using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Arashi.Core.CallContext
{
   public class ArashiCallContextFactory : ICallContextFactory
   {
      readonly ICallContext contextAsync;
      readonly ICallContext contextWeb;

      /// <summary>
      /// Initializes a new instance of the <see cref="DexterCallContextFactory"/> class.
      /// </summary>
      /// <param name="contextWeb">The context web.</param>
      /// <param name="contextAsync">The context async.</param>
      public ArashiCallContextFactory(IWebCallContext contextWeb, IAsyncCallContext contextAsync)
      {
         this.contextWeb = contextWeb;
         this.contextAsync = contextAsync;
      }

      #region ICallContextFactory Members

      /// <summary>
      /// Return the WebCallContext or the ThreadCallContext
      /// </summary>
      /// <returns></returns>
      public ICallContext RetrieveCallContext()
      {
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
      public bool IsWebRequest
      {
         get { return HttpContext.Current != null; }
      }

      #endregion
   }
}
