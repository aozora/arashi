namespace Arashi.Web.Mvc.Filters
{
   using System;
   using System.Security;
   using System.Web.Mvc;
   using Arashi.Core.Domain;
   using Arashi.Core.Domain.Extensions;
   using Common.Logging;


   /// <summary>
   /// This attribute check if a class or method can be executed by the current logged user
   /// </summary>
   [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
   public class PermissionFilterAttribute : FilterAttribute, IAuthorizationFilter
   {
      private readonly ILog log = LogManager.GetCurrentClassLogger();
      private string rights;
      private string[] rightsArray;



      /// <summary>
      /// The required rights as a comma-separated string.
      /// </summary>
      public string RequiredRights
      {
         get
         {
            return rights;
         }
         set
         {
            SetRights(value);
         }
      }



      /// <summary>
      /// Array of required rights.
      /// </summary>
      public string[] RightsArray
      {
         get
         {
            return this.rightsArray;
         }
      }



      private void SetRights(string rightsAsString)
      {
         this.rights = rightsAsString;
         this.rightsArray = rightsAsString.Split(new char[1] { ',' });
      }



      public void OnAuthorization(AuthorizationContext filterContext)
      {
         log.Debug("PermissionFilterAttribute.OnAuthorization - start");

         // Only check authorization when rights are defined and the user is authenticated.);
         if (this.rightsArray.Length > 0 && filterContext.HttpContext.User.Identity.IsAuthenticated)
         {
            // Get the current user from the request. It would be nice if we could inject ICuyahogaContext
            // but that's not possible with the current version.
            User user = filterContext.HttpContext.User as User;

            if (user == null)
               throw new SecurityException("UserNullException");

            foreach (string right in RightsArray)
            {
               if (!user.HasRight(right))
               {
                  log.DebugFormat("PermissionFilterAttribute.OnAuthorization: user.HasRight({0}) == false !!", right);
                  throw new SecurityException("ActionNotAllowedException");
               }
            }
         }
         log.Debug("PermissionFilterAttribute.OnAuthorization - end");
      }

   }
}
