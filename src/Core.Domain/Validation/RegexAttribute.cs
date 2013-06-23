using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Arashi.Core.Domain.Validation
{
   public class RegexAttribute : ValidationAttribute
   {
      public string Pattern
      {
         get;
         set;
      }

      public RegexOptions Options
      {
         get;
         set;
      }


      public RegexAttribute(string pattern)
         : this(pattern, RegexOptions.None)
      {
      }


      public RegexAttribute(string pattern, RegexOptions options)
      {
         this.Pattern = pattern;
         this.Options = options;
      }



 #if !SILVERLIGHT
     public override bool IsValid(object value)
      {
         //null or empty is <EM>not</EM> invalid
         var str = (string)value;
         if (string.IsNullOrEmpty(str))
            return true;

         bool isMatch = new Regex(this.Pattern, this.Options).IsMatch(str);
         return isMatch;
      }
#else
      protected override ValidationResult IsValid(object value, ValidationContext validationContext)
      {
         //null or empty is <EM>not</EM> invalid
         var str = (string)value;
         if (string.IsNullOrEmpty(str))
            return ValidationResult.Success;

         bool isMatch = new Regex(this.Pattern, this.Options).IsMatch(str);
         
         if (isMatch)
            return ValidationResult.Success;

         return new ValidationResult(null);
      }
#endif

   }
}