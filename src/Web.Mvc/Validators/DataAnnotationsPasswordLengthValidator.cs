using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arashi.Web.Mvc.Validators
{
   using System.Web.Mvc;

   public class DataAnnotationsPasswordLengthValidator : DataAnnotationsModelValidator<ValidatePasswordLengthAttribute>
   {
      public DataAnnotationsPasswordLengthValidator(ModelMetadata metadata, ControllerContext context, ValidatePasswordLengthAttribute attribute)
         : base(metadata, context, attribute)
      {
      }

      public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
      {
         yield return new ModelClientPasswordLengthValidationRule(ErrorMessage, Attribute.MinCharacters);
      }

   }
}
