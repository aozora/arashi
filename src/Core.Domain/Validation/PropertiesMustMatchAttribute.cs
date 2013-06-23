namespace Arashi.Core.Domain.Validation
{
   #region Usings

   using System;
   using System.ComponentModel;
   using System.ComponentModel.DataAnnotations;
   using System.Globalization;

   #endregion

   /// <summary>
   /// Data annotation attribute for properties that must match
   /// </summary>
   [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
   public sealed class PropertiesMustMatchAttribute : ValidationAttribute
   {
      private const string DEFAULT_ERROR_MESSAGE = "'{0}' and '{1}' do not match.";
      private readonly object typeId = new object();

      public string ConfirmProperty { get; private set; }
      public string OriginalProperty { get; private set; }



      public PropertiesMustMatchAttribute(string originalProperty, string confirmProperty)
         : base(DEFAULT_ERROR_MESSAGE)
      {
         OriginalProperty = originalProperty;
         ConfirmProperty = confirmProperty;
      }

      public override object TypeId
      {
         get { return typeId; }
      }



      public override string FormatErrorMessage(string name)
      {
         return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString, OriginalProperty, ConfirmProperty);
      }



      public override bool IsValid(object value)
      {
         PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(value);
         object originalValue = properties.Find(OriginalProperty, true /* ignoreCase */ ).GetValue(value);
         object confirmValue = properties.Find(ConfirmProperty, true /* ignoreCase */ ).GetValue(value);
         return Equals(originalValue, confirmValue);
      }

   }
}
