using System;
using System.Runtime.Serialization;

namespace Arashi.Core.Exceptions
{
   /// <summary>
   /// Eccezione generica 
   /// </summary>
   [Serializable]
   public class PageNullException : Exception
   {
      public PageNullException()
         : base()
      {
      }

      public PageNullException(String message)
         : base(message)
      {
      }

      public PageNullException(String message, Exception innerException)
         : base(message, innerException)
      {
      }

      protected PageNullException(SerializationInfo info, StreamingContext context)
         : base(info, context)
      {
      }

   }
}