using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using xVal.ServerSide;

namespace Arashi.Web.Components
{
   public static class DataAnnotationsValidationRunner
   {
      /// <summary>
      /// Runs each <see cref="ValidationAttribute"/> associated with a property on the supplied instance
      /// and returns an <see cref="ErrorInfo"/> relating to each validation failure. Caution: certain
      /// <see cref="ValidationAttribute"/> types claim to be valid even when they aren't - this runner
      /// would need to detect those special cases if you plan to rely on it. Fortunately, 
      /// other validation runners (e.g., for Castle Validation and NHibernate.Validate) 
      /// report all their errors correctly.
      /// </summary>
      public static IEnumerable<ErrorInfo> GetErrors(object instance)
      {
         var metadataAttrib = instance.GetType().GetCustomAttributes(typeof(MetadataTypeAttribute), true).OfType<MetadataTypeAttribute>().FirstOrDefault();
         var buddyClassOrModelClass = metadataAttrib != null ? metadataAttrib.MetadataClassType : instance.GetType();
         var buddyClassProperties = TypeDescriptor.GetProperties(buddyClassOrModelClass).Cast<PropertyDescriptor>();
         var modelClassProperties = TypeDescriptor.GetProperties(instance.GetType()).Cast<PropertyDescriptor>();

         return from buddyProp in buddyClassProperties
                join modelProp in modelClassProperties on buddyProp.Name equals modelProp.Name
                from attribute in buddyProp.Attributes.OfType<ValidationAttribute>()
                where !attribute.IsValid(modelProp.GetValue(instance))
                select new ErrorInfo(buddyProp.Name, attribute.FormatErrorMessage(string.Empty), instance);
      }
   }
}