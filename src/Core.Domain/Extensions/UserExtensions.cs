using System;
using System.Collections.Generic;
using Arashi.Core.Extensions;
using Arashi.Core.Domain;

namespace Arashi.Core.Domain.Extensions
{
   public static class UserExtensions
   {
      ///// <summary>
      ///// Check if the user has the requested access rights.
      ///// </summary>
      ///// <param name="user"></param>
      ///// <param name="permission"></param>
      ///// <returns></returns>
      //[Obsolete("Replaced by HasRight().")]
      //public static bool HasPermission(this User user, AccessLevel permission)
      //{
      //   return Array.IndexOf(user.Permissions, permission) > -1;
      //}


      /// <summary>
      /// Check if the user has the requested access right.
      /// </summary>
      /// <param name="user"></param>
      /// <param name="rightName"></param>
      /// <returns></returns>
      public static bool HasRight(this User user, string rightName)
      {
         foreach (Right right in user.Rights)
         {
            if (right.Name.Equals(rightName, StringComparison.InvariantCultureIgnoreCase))
            {
               return true;
            }
         }
         return false;
      }



      /// <summary>
      /// Check if the user has the requested access right for a given site.
      /// </summary>
      /// <param name="user"></param>
      /// <param name="rightName"></param>
      /// <param name="roles"></param>
      /// <returns></returns>
      public static bool HasRight(this User user, string rightName, IList<Role> roles)
      {
         foreach (Role role in roles)
         {
            if (user.IsInRole(role) && role.HasRight(rightName))
            {
               return true;
            }
         }
         return false;
      }



      ///// <summary>
      ///// Indicates if the user has view permissions for a certain Node.
      ///// </summary>
      ///// <param name="page"></param>
      ///// <returns></returns>
      //public static bool CanView(this User user, Page page)
      //{
      //   foreach (PagePermission p in page.PagePermissions)
      //   {
      //      if (p.ViewAllowed && user.IsInRole(p.Role))
      //      {
      //         return true;
      //      }
      //   }
      //   return false;
      //}



      ///// <summary>
      ///// Indicates if the user has view permissions for a certain Section.
      ///// </summary>
      ///// <param name="section"></param>
      ///// <returns></returns>
      //public static bool CanView(this User user, Section section)
      //{
      //   foreach (SectionPermission p in section.SectionPermissions)
      //   {
      //      if (p.ViewAllowed && user.IsInRole(p.Role))
      //      {
      //         return true;
      //      }
      //   }
      //   return false;
      //}



      ///// <summary>
      ///// Indicates if the user has edit permissions for a certain Section.
      ///// </summary>
      ///// <param name="user"></param>
      ///// <param name="section"></param>
      ///// <returns></returns>
      //public static bool CanEdit(this User user, Section section)
      //{
      //   foreach (SectionPermission p in section.SectionPermissions)
      //   {
      //      if (p.EditAllowed && user.IsInRole(p.Role))
      //      {
      //         return true;
      //      }
      //   }
      //   return false;
      //}

      /// <summary>
      /// Create a SHA2 hash of the password.
      /// </summary>
      /// <param name="user"></param>
      /// <param name="plainPassword">The password in clear text</param>
      /// <returns>The SHA2 hash of the password</returns>
      public static string HashPassword(this User user, string plainPassword)
      {
         if (user.ValidatePassword(plainPassword))
         {
            //return Arashi.Core.Util.Encryption.StringToMD5Hash(password);
            return plainPassword.EncryptToSHA2();
         }
         else
         {
            throw new ArgumentOutOfRangeException("Invalid password");
         }
      }



      /// <summary>
      /// Check if the password is valid.
      /// </summary>
      /// <param name="user"></param>
      /// <param name="password"></param>
      /// <returns></returns>
      public static bool ValidatePassword(this User user, string password)
      {
         // Very simple password rule. Extend here when required.
         return (password.Length >= 8);
      }



      /// <summary>
      /// Generates a new password and stores a hashed password in the User instance.
      /// </summary>
      /// <returns>The newly created password.</returns>
      public static string GeneratePassword(this User user)
      {
         int length = 8;
         string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
         string pwd = String.Empty;
         Random rnd = new Random();
         for (int i = 0; i < length; i++)
         {
            pwd += chars[rnd.Next(chars.Length)];
         }
         user.Password = user.HashPassword(pwd);
         return pwd;
      }



      /// <summary>
      /// Determine if the user is in a give Role.
      /// </summary>
      /// <param name="user"></param>
      /// <param name="roleToCheck"></param>
      /// <returns></returns>
      public static bool IsInRole(this User user, Role roleToCheck)
      {
         foreach (Role role in user.Roles)
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
      /// <param name="user"></param>
      /// <param name="roleName"></param>
      /// <returns></returns>
      public static bool IsInRole(this User user, string roleName)
      {
         foreach (Role role in user.Roles)
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