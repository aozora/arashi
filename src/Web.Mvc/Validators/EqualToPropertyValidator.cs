namespace Arashi.Web.Mvc.Validators
{
   using System.Collections.Generic;
   using System.Web.Mvc;


   public class EqualToPropertyValidator : CrossFieldValidator<EqualToPropertyAttribute>
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="EqualToPropertyValidator"/> class.
      /// </summary>
      /// <param name="metadata">The metadata.</param>
      /// <param name="controllerContext">The controller context.</param>
      /// <param name="attribute">The attribute.</param>
      public EqualToPropertyValidator(ModelMetadata metadata, ControllerContext controllerContext,
                                      EqualToPropertyAttribute attribute) :
         base(metadata, controllerContext, attribute)
      {
      }



      /// <summary>
      /// Gets metadata for client validation.
      /// </summary>
      /// <returns>The metadata for client validation.</returns>
      public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
      {
         var rule = new ModelClientValidationRule
         {
            ValidationType = "equaltoproperty",
            ErrorMessage = Attribute.FormatErrorMessage(Metadata.PropertyName),
         };

         rule.ValidationParameters.Add("otherproperty", Attribute.OtherProperty);

         return new[] { rule };
      }

   }
}
