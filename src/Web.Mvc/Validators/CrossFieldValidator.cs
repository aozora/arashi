namespace Arashi.Web.Mvc.Validators
{
   using System.Collections.Generic;
   using System.ComponentModel.DataAnnotations;
   using System.Web.Mvc;


   /// <summary>
   /// A base class for validators that have attributes that require more
   /// than one property to check for validity.
   /// The standard <see cref="ValidationAttribute.IsValid"/> method
   /// isn't sufficient because this method can't see the rest of the model.
   /// </summary>
   /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
   public abstract class CrossFieldValidator<TAttribute> : DataAnnotationsModelValidator<TAttribute>
       where TAttribute : ValidationAttribute
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="CrossFieldValidator&lt;TAttribute&gt;"/> class.
      /// </summary>
      /// <param name="metadata">Description of the model being validated.</param>
      /// <param name="context">The controller context.</param>
      /// <param name="attribute">The attribute being validated.</param>
      protected CrossFieldValidator(ModelMetadata metadata, ControllerContext context, TAttribute attribute)
         : base(metadata, context, attribute)
      {
      }

      /// <summary>
      /// Returns a list of validation error messages for the model.
      /// </summary>
      /// <param name="container">The container for the model.</param>
      /// <returns>
      /// A list of validation error messages for the model, or an empty list if no errors have occurred.
      /// </returns>
      public override IEnumerable<ModelValidationResult> Validate(object container)
      {
         var attribute = Attribute as ICrossFieldValidationAttribute;

         if (!attribute.IsValid(ControllerContext, container, Metadata))
         {
            yield return new ModelValidationResult
            {
               Message = ErrorMessage
            };
         }
      }
   }
}
