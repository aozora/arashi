using System;
using System.Runtime.Serialization;

namespace Arashi.Core.Exceptions
{
   /// <summary>
   /// Eccezione generica 
   /// </summary>
   [Serializable]
   public class AccessForbiddenException : Exception
   {
      public AccessForbiddenException()
         : base()
      {
      }

      public AccessForbiddenException(String message)
         : base(message)
      {
      }

      public AccessForbiddenException(String message, Exception innerException)
         : base(message, innerException)
      {
      }

      protected AccessForbiddenException(SerializationInfo info, StreamingContext context)
         : base(info, context)
      {
      }

   }
}