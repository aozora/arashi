using System;
using System.Runtime.Serialization;

namespace Arashi.Core.Exceptions
{
   /// <summary>
   /// Eccezione generica 
   /// </summary>
#if !SILVERLIGHT
   [Serializable]
#endif
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

#if !SILVERLIGHT
      protected ApplicationException(SerializationInfo info, StreamingContext context) : base(info, context)
      {
      }
#endif

   }
}