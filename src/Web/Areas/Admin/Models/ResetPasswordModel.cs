using System;
using System.ComponentModel.DataAnnotations;
using Arashi.Core.Domain.Validation;

namespace Arashi.Web.Areas.Admin.Models
{
   public class ResetPasswordModel
   {
      [Email(ErrorMessage = "Invalid e-mail address.")]
      [Required(ErrorMessage = "Email address is missing.")]
      public string Email
      {
         get;
         set;
      }

   }
}