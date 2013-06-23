using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arashi.Services
{
   public class ServiceResult
   {
      /// <summary>
      /// The state of the service action. If the service action has been executed successfully,
      /// return "Success", otherwise if any kind of validation and/or logical error occured,
      /// return "Error".
      /// IMPORTANT NOTE: if any exceptions occurs, they will not be passed useing this object, 
      /// but with the "throw" keyword.
      /// </summary>
      public enum ServiceState
      {
         Success,
         Error
      }


      public ServiceState State {get; set;}

      public string Message {get; set;}

   }
}