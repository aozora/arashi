using System;
using Arashi.Core.Domain;
using Castle.MicroKernel;

namespace Arashi.Services
{
   /// <summary>
   /// Provides access to the current context.
   /// </summary>
   public class RequestContextProvider : IRequestContextProvider
   {
      private IKernel kernel;


      /// <summary>
      /// Creates a new instance of the <see cref="RequestContextProvider"></see> class.
      /// </summary>
      /// <param name="kernel"></param>
      public RequestContextProvider(IKernel kernel)
      {
         this.kernel = kernel;
      }



      /// <summary>
      /// Gets the context (for the current request).
      /// </summary>
      /// <returns></returns>
      public IRequestContext GetContext()
      {
         return kernel.Resolve<IRequestContext>();
      }
   }
}