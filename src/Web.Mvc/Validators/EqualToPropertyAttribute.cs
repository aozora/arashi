namespace Arashi.Web.Mvc.Validators
{
   using System;
   using System.ComponentModel.DataAnnotations;
   using System.Web.Mvc;

   /// <summary>
   /// Attribute used to mark the requirement for a property to be  equal
   /// to another property.
   /// </summary>
   [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
   public class EqualToPropertyAttribute : ValidationAttribute, ICrossFieldValidationAttribute
   {
      /// <summary>
      /// Gets or sets the other property we are validating against.
      /// </summary>
      /// <value>The other property.</value>
      public string OtherProperty { get; set; }



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
      public bool IsValid(ControllerContext controllerContext, object model, ModelMetadata modelMetadata)
      {
         if (model == null)
         {
            throw new ArgumentNullException("model");
         }

         // Find the value of the other property.
         var propertyInfo = model.GetType().GetProperty(OtherProperty);
         if (propertyInfo == null)
         {
            throw new InvalidOperationException(string.Format("Couldn't find {0} property on {1}.", OtherProperty, model));
         }

         var otherValue = propertyInfo.GetGetMethod().Invoke(model, null);

         if (modelMetadata.Model == null)
         {
            modelMetadata.Model = string.Empty;
         }

         if (otherValue == null)
         {
            otherValue = string.Empty;
         }

         return modelMetadata.Model.ToString() == otherValue.ToString();
      }



      /// <summary>
      /// Determines whether the specified value of the object is valid.
      /// </summary>
      /// <param name="value">The value of the specified validation object on which the <see cref="T:System.ComponentModel.DataAnnotations.ValidationAttribute"/> is declared.</param>
      /// <returns>
      /// <see langword="true"/> if the specified value is valid; otherwise, <see langword="false"/>.
      /// </returns>
      public override bool IsValid(object value)
      {
         // Work done in other IsValid
         return true;
      }

   }
}
