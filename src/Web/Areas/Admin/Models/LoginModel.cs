using System;
using System.ComponentModel.DataAnnotations;
using Arashi.Core.Domain.Validation;

namespace Arashi.Web.Areas.Admin.Models
{
   /// <summary>
   /// LoginModel
   /// </summary>
   public class LoginModel
   {
      /// <summary>
      /// User email
      /// </summary>
      [Email(ErrorMessage = "Invalid e-mail address.")]
      [Required(ErrorMessage = "Email address is missing.")]
      public string Email
      {
         get;
         set;
      }


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
      /// Remember me flag
      /// </summary>
      public bool RememberMe { get; set; }


      /// <summary>
      /// Constructor
      /// </summary>
      public LoginModel()
      {
         RememberMe = false;
      }
   }
}