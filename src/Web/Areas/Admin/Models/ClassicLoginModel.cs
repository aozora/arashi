namespace Arashi.Web.Areas.Admin.Models
{
   using System.ComponentModel.DataAnnotations;
   using Arashi.Core.Domain.Validation;

   /// <summary>
   /// Classic Login Model for authentication with username & password
   /// </summary>
   public class ClassicLoginModel
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
      [StringLength(100, ErrorMessage = "The password length must be between 8 and 100 chars.", MinimumLength = 8)]
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
      public ClassicLoginModel()
      {
         RememberMe = false;
      }
   }
}