namespace Arashi.Web.Areas.Admin.Models
{
   using System.ComponentModel.DataAnnotations;
   using System.Web.Mvc;

   using Arashi.Core.Domain;
   using Arashi.Web.Mvc.Validators;

   /// <summary>
   /// Custom MVC Model for the user change password
   /// </summary>
   public class ChangePasswordModel
   {
      public User User {get; set;}



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
