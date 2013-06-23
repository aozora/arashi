using System;
using System.ComponentModel.DataAnnotations;
using Arashi.Core.Domain.Validation;

namespace Arashi.Web.Areas.Admin.Models
{
   /// <summary>
   /// Custom MVC Model for the user change password
   /// </summary>
   [PropertiesMatch("Password", "PasswordConfirmation", ErrorMessage = "Password doesn't match confirmation, please check it.")]
   public class ChangePasswordModel
   {
      /// <summary>
      /// User password
      /// </summary>
      [Required(ErrorMessage = "You must specify the password.")]
      [StringRange(8, 100, ErrorMessage = "The password length must be between 8 and 100 chars.")]
      public string Password
      {
         get;
         set;
      }



      /// <summary>
      /// User password confirmation
      /// </summary>
      [Required(ErrorMessage = "You must specify the password confirmation.")]
      [StringRange(8, 100, ErrorMessage = "The password confirmation length must be between 8 and 100 chars.")]
      public string PasswordConfirmation
      {
         get;
         set;
      }

   }
}
