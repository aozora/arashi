using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Arashi.Core.Domain.Validation
{
   public class StringRangeAttribute : ValidationAttribute
   {
      public int MinLength
      {
         get;
         set;
      }


      public int MaxLength
      {
         get;
         set;
      }



      public StringRangeAttribute(int minLength, int maxLength)
      {
         this.MinLength = (minLength < 0) ? 0 : minLength;
         this.MaxLength = (maxLength < 0) ? 0 : maxLength;
      }


#if !SILVERLIGHT
      public override bool IsValid(object value)
      {
         //null or empty is <em>not</em> invalid
         var str = (string)value;
         if (string.IsNullOrEmpty(str))
            return true;

         return (str.Length >= this.MinLength && str.Length <= this.MaxLength);
      }
#else
      protected override ValidationResult IsValid(object value, ValidationContext validationContext)
      {
         //null or empty is <em>not</em> invalid
         var str = (string)value;
         if (string.IsNullOrEmpty(str))
            return ValidationResult.Success;

         if (!(str.Length >= this.MinLength && str.Length <= this.MaxLength))
            return new ValidationResult(null);
         else
            return ValidationResult.Success;
      }
#endif
   }
}