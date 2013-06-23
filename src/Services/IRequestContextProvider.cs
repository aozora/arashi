using System;
using Arashi.Core.Domain;

namespace Arashi.Services
{
   /// <summary>
   /// Provides access to the current context.
   /// </summary>
   public interface IRequestContextProvider   //ICuyahogaContextProvider
   {
      /// <summary>
      /// Gets the context (for the current request).
      /// </summary>
      /// <returns></returns>
      IRequestContext GetContext();
   }
}