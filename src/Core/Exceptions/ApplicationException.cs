using System;
using System.Runtime.Serialization;

namespace Arashi.Core.Exceptions
{
   /// <summary>
   /// Eccezione generica 
   /// </summary>
   [Serializable]
   public class ApplicationException : Exception
   {
      public ApplicationException() : base()
      {
      }

      public ApplicationException(String message) : base(message)
      {
      }

      public ApplicationException(String message, Exception innerException) : base(message, innerException)
      {
      }

      protected ApplicationException(SerializationInfo info, StreamingContext context) : base(info, context)
      {
      }

   }
}