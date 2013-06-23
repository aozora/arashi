using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Arashi.Core.Exceptions
{
   /// <summary>
   /// Custom exception for Search Index
   /// </summary>
   [Serializable]
   public class SearchException : ApplicationException
   {
      public SearchException()
         : base()
      {
      }

      public SearchException(string message)
         : base(message)
      {
      }

      public SearchException(string message, Exception innerException)
         : base(message, innerException)
      {
      }


      protected SearchException(SerializationInfo info, StreamingContext context)
         : base(info, context)
      {
      }

   }
}
