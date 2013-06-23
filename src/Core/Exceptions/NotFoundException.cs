using System;
using System.Runtime.Serialization;

namespace Arashi.Core.Exceptions
{
   /// <summary>
   /// Eccezione generica 
   /// </summary>
   [Serializable]
   public class NotFoundException : Exception
   {
      public NotFoundException()
         : base()
      {
      }

      public NotFoundException(String message)
         : base(message)
      {
      }

      public NotFoundException(String message, Exception innerException)
         : base(message, innerException)
      {
      }

      protected NotFoundException(SerializationInfo info, StreamingContext context)
         : base(info, context)
      {
      }

   }
}