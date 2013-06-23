using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arashi.Web.Mvc.Validators
{
   using System.Web.Mvc;

   public class ModelClientPasswordLengthValidationRule : ModelClientValidationRule
   {
      public ModelClientPasswordLengthValidationRule(string errorMessage, int minCharacters)
      {
         ErrorMessage = errorMessage;

         ValidationType = "passwordLength";

         ValidationParameters.Add("minCharacters", minCharacters);
      }
   }
}
