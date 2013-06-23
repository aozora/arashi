using System;
using System.Runtime.Serialization;

namespace Arashi.Core.Exceptions
{
   /// <summary>
   /// Eccezione generica 
   /// </summary>
   [Serializable]
   public class SiteNullException : Exception
   {
      public SiteNullException()
         : base()
      {
      }

      public SiteNullException(String message)
         : base(message)
      {
      }

      public SiteNullException(String message, Exception innerException)
         : base(message, innerException)
      {
      }

      protected SiteNullException(SerializationInfo info, StreamingContext context)
         : base(info, context)
      {
      }

   }
}