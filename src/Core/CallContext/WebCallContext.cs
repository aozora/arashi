namespace Arashi.Core.CallContext
{

   /// <summary>
   /// This class is the implementation of ICallContext
   /// </summary>
   public class WebCallContext : IWebCallContext
   {

      #region ICallContext Members

      /// <summary>
      /// Gets the items.
      /// </summary>
      /// <value>The items.</value>
      public object this[string key]
      {
         get { return System.Runtime.Remoting.Messaging.CallContext.LogicalGetData(key); }
         set { System.Runtime.Remoting.Messaging.CallContext.LogicalSetData(key, value); }
      }

      #endregion
   }
}
