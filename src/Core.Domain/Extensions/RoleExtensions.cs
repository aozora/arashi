using System;
using Arashi.Core.Domain;
//using Arashi.Services.Membership;

namespace Arashi.Core.Domain.Extensions
{
   public static class RoleExtensions
   {

      /// <summary>
      /// Check if the role has the requested access right.
      /// </summary>
      /// <param name="role"></param>
      /// <param name="rightName"></param>
      /// <returns></returns>
      public static bool HasRight(this Role role, string rightName)
      {
         foreach (Right right in role.Rights)
         {
            if (right.Name.Equals(rightName, StringComparison.InvariantCultureIgnoreCase))
            {
               return true;
            }
         }
         return false;
      }



      //public static bool IsStandardRole(this Role role, StandardRoles standardRoleToCheck)
      //{
      //   string standardRoleName = Enum.GetName(typeof(StandardRoles), standardRoleToCheck).Replace(' ', '_');

      //   return (role.Name == standardRoleName);
      //}


      ///// <summary>
      ///// Indicates if the role is an anonymous role. This means that it only has the 'Anonymous' right and nothing else.
      ///// </summary>
      //public static bool IsAnonymousRole(this Role role)
      //{
      //   return role.Rights.Count == 1 && role.Rights[0].Name.Equals(Rights.Anonymous);
      //}

   }
}