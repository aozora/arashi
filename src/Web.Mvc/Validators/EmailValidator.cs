namespace Arashi.Web.Mvc.Validators
{
   using System.Collections.Generic;
   using System.Web.Mvc;

   using Arashi.Core.Domain.Validation;


   /// <summary>
   /// Model Validator for EmailAttribute
   /// </summary>
   public class EmailValidator : DataAnnotationsModelValidator<EmailAttribute>
   {
      string pattern;
      string message;

      public EmailValidator(ModelMetadata metadata, ControllerContext context, EmailAttribute attribute)
         : base(metadata, context, attribute)
      {
         pattern = attribute.Pattern;
         message = attribute.ErrorMessage;
      }



      public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
      {
         var rule = new ModelClientValidationRule
         {
            ErrorMessage = message,
            ValidationType = "email"
         };
         rule.ValidationParameters.Add("email", pattern);

         return new[] { rule };
      }

   }
}
