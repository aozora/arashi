using System.Collections.Generic;
using System.Web.Mvc;
using Arashi.Core.Domain;

namespace Arashi.Web.Areas.Admin.Models
{
   using System;
   using System.ComponentModel.DataAnnotations;

   using Arashi.Core.Domain.Validation;

   /// <summary>
   /// UserModel
   /// </summary>
   public class UserModel
   {
      //public User User { get; set; }
      public int UserId { get; set; }
      public String ExternalId { get; set; }
      public String ExternalProviderUri { get; set; }
      public String DisplayName { get; set; }

      //[StringRange(1, 100, ErrorMessage = "The first name length must be between 1 and 100 chars.")]
      public string FirstName { get; set; }

      //[StringRange(1, 100, ErrorMessage = "The last name length must be between 1 and 100 chars.")]
      public string LastName { get; set; }

      public string Description { get; set; }

      [Remote("CheckIfEmailIsAlreadyInUse", "Users", AdditionalFields = "UserId", HttpMethod = "POST", ErrorMessage = "Sorry, this email address is already in use by another user; please choose another one.")]
      [Required(ErrorMessage = "Email address is missing.")]
      [Email(ErrorMessage = "Invalid e-mail address.")]
      public string Email { get; set; }

      public int TimeZone { get; set; }
      public string WebSite { get; set; }
      public bool IsActive { get; set; }
      public DateTime? LastLogin { get; set; }
      public string LastIp { get; set; }
      public string AdminTheme { get; set; }
      public string AdminCulture { get; set; }
      public DateTime CreatedDate { get; set; }
      public DateTime? UpdatedDate { get; set; }


      // Collections
      public SelectList Cultures { get; set; }
      public SelectList AdminThemes { get; set; }
      public SelectList TimeZones { get; set; }
      public ICollection<Role> Roles { get; set; }
      public ICollection<Role> AllRoles { get; set; }


      /// <summary>
      /// Determine if the user is in a give Role.
      /// </summary>
      /// <param name="roleToCheck"></param>
      /// <returns></returns>
      public bool IsInRole(Role roleToCheck)
      {
         foreach (Role role in this.Roles)
         {
            if (role.RoleId == roleToCheck.RoleId && role.Name == roleToCheck.Name)
            {
               return true;
            }
         }
         return false;
      }



      /// <summary>
      /// Determine if the user is in a give Role.
      /// </summary>
      /// <param name="roleName"></param>
      /// <returns></returns>
      public bool IsInRole(string roleName)
      {
         foreach (Role role in this.Roles)
         {
            if (role.Name == roleName)
            {
               return true;
            }
         }
         return false;
      }


   }
}
