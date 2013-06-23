namespace Arashi.Web.Mvc.Validators
{
   using System.Web.Mvc;

   /// <summary>
   /// All validation data annotation attributes that require more than one
   /// property to validate (or any other additional context from the controller) 
   /// should implement this interface.
   /// </summary>
   public interface ICrossFieldValidationAttribute
   {
      /// <summary>
      /// Determines whether the specified property is valid.
      /// </summary>
      /// <param name="controllerContext">Complete controller context (if required).</param>
      /// <param name="model">The model object to which the property belongs.</param>
      /// <param name="modelMetadata">Model metadata relating to the property holding
      /// the validation data annotation.</param>
      /// <returns>
      /// 	<c>true</c> if the specified property is valid; otherwise, <c>false</c>.
      /// </returns>
      bool IsValid(ControllerContext controllerContext, object model, ModelMetadata modelMetadata);
   }
}
