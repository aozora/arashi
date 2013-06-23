namespace Arashi.Core
{
   using System;



   /// <summary>
   /// 	Disposabel Action Class
   /// </summary>
   public class DisposableAction : IDisposable
   {
      /// <summary>
      /// 	Initializes a new instance of the <see cref = "DisposableAction" /> class.
      /// </summary>
      /// <param name = "action">The action.</param>
      public DisposableAction(Action action)
      {
         Action = action;
      }



      private Action Action
      {
         get;
         set;
      }

      #region IDisposable Members

      /// <summary>
      /// 	Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
      /// </summary>
      public void Dispose()
      {
         Action();
      }

      #endregion
   }
}
