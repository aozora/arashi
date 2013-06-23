using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Arashi.Core.Domain.Validation
{
   /// <summary>
   /// Compare 2 properties value.
   /// Convert both values to string and compare.
   /// See http://byatool.com/mvc/custom-data-annotations-with-mvc-how-to-check-multiple-properties-at-one-time/
   /// </summary>
   [AttributeUsage(AttributeTargets.Class)]
   public class PropertiesMatchAttribute : ValidationAttribute
   {

      public String FirstPropertyName
      {
         get;
         set;
      }
      public String SecondPropertyName
      {
         get;
         set;
      }



      /// <summary>
      /// Constructor to take in the property names that are supposed to be checked
      /// </summary>
      /// <param name="firstPropertyName"></param>
      /// <param name="secondPropertyName"></param>
      public PropertiesMatchAttribute(String firstPropertyName, String secondPropertyName)
      {
         FirstPropertyName = firstPropertyName;
         SecondPropertyName = secondPropertyName;
      }



      /// <summary>
      /// Check if is valid
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
      public override Boolean IsValid(Object value)
      {
         Type objectType = value.GetType();
         //Get the property info for the object passed in.  This is the class the attribute is
         //  attached to
         //I would suggest caching this part... at least the PropertyInfo[]
         PropertyInfo[] neededProperties = objectType.GetProperties()
            .Where(propertyInfo => propertyInfo.Name == FirstPropertyName || propertyInfo.Name == SecondPropertyName)
            .ToArray();

         if (neededProperties.Count() != 2)
            throw new ApplicationException("PropertiesMatchAttribute error on " + objectType.Name);

         bool isValid = true;

         //Convert both values to string and compare...  Probably could be done better than this
         //  but let's not get bogged down with how dumb I am.  We should be concerned about
         //  dumb you are, jerkface.
         if (!Convert.ToString(neededProperties[0].GetValue(value, null)).Equals(Convert.ToString(neededProperties[1].GetValue(value, null))))
            isValid = false;

         return isValid;
      }

   }
}