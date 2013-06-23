using System;
using System.Security;
using System.Security.Principal;

namespace Arashi.Core.Domain
{
   /// <summary>
   /// Summary description for ArashiPrincipal.
   /// </summary>
   public class ArashiPrincipal : IPrincipal
   {
      private User user;


      /// <summary>
      /// Default constructor. 
      /// An instance of an authenticated user is required when creating 
      /// this principal.
      /// </summary>
      /// <param name="user"></param>
      public ArashiPrincipal(User user)
      {
         if (user != null && user.IsAuthenticated)
         {
            this.user = user;
         }
         else
         {
            throw new SecurityException("Cannot create a principal without a valid user");
         }
      }



      /// <summary>
      /// 
      /// </summary>
      public IIdentity Identity
      {
         get { return user; }
      }



      /// <summary>
      /// 
      /// </summary>
      /// <param name="role"></param>
      /// <returns></returns>
      public bool IsInRole(string role)
      {
         foreach (Role roleObject in user.Roles)
         {
            if (roleObject.Name.Equals(role))
               return true;
         }

         return false;
      }



   }
}