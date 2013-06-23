using System.Collections.Generic;
using System.Web.Mvc;
using Arashi.Core.Domain;

namespace Arashi.Web.Areas.Admin.Models
{
   using System;
   using System.ComponentModel.DataAnnotations;

   using Arashi.Core.Domain.Validation;

   /// <summary>
   /// Model used by the NewUser view
   /// </summary>
   public class NewUserModel : UserModel
   {
      /// <summary>
      /// User password
      /// </summary>
      [Required(ErrorMessage = "You must specify the password.")]
      [StringLength(100, ErrorMessage = "The password length must be between 8 and 100 chars.", MinimumLength = 8)]
      [Compare("PasswordConfirmation", ErrorMessage = "The password and confirmation password do not match.")]
      public string Password
      {
         get;
         set;
      }



      /// <summary>
      /// User password confirmation
      /// </summary>
      [Required(ErrorMessage = "You must specify the password confirmation.")]
      [StringLength(100, ErrorMessage = "The password confirmation length must be between 8 and 100 chars.", MinimumLength = 8)]
      public string PasswordConfirmation
      {
         get;
         set;
      }


   }
}
