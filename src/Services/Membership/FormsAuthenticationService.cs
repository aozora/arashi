using System;
using System.Web;
using System.Web.Security;
using Arashi.Core.Domain;
using Arashi.Core.Domain.Extensions;
using Arashi.Core.Extensions;
using log4net;
using ApplicationException = Arashi.Core.Exceptions.ApplicationException;

namespace Arashi.Services.Membership
{
   public class FormsAuthenticationService : IAuthenticationService
   {
      private IUserService userService;
      private static readonly ILog log = LogManager.GetLogger(typeof(FormsAuthenticationService));

      public FormsAuthenticationService(IUserService userService)
      {
         this.userService = userService;
      }




      /// <summary>
      /// Effettua il login di un dato User
      /// </summary>
      /// <param name="user"></param>
      /// <param name="ipAddress"></param>
      public void LogIn(User user, string ipAddress, bool createPersistentCookie)
      {
         user.IsAuthenticated = true;
         user.LastLogin = DateTime.Now;
         user.LastIp = ipAddress;

         // Save login date and IP
         userService.UpdateUser(user);

         // Create the authentication ticket
         HttpContext.Current.User = new ArashiPrincipal(user);
         FormsAuthentication.SetAuthCookie(user.Name, createPersistentCookie); 
      }



      /// <summary>
      /// Try to authenticate the user.
      /// </summary>
      /// <param name="site"></param>
      /// <param name="username"></param>
      /// <param name="password"></param>
      /// <param name="ipAddress"></param>
      /// <param name="createPersistentCookie"></param>
      /// <returns></returns>
      public User AuthenticateUser(Site site, string username, string password, string ipAddress, bool createPersistentCookie)
      {
         string hashedPassword = password.EncryptToSHA2();

         if (site == null)
         {
            const string msg = "FormsAuthenticationService.AuthenticateUser invoked with Site = NULL";
            log.Error(msg);
            throw new ApplicationException(msg);
         }

         try
         {
            User user = userService.GetUserByEmailAndPassword(site, username, hashedPassword);

            if (user != null)
            {
               LogIn(user, ipAddress, createPersistentCookie);
            }
            else
            {
               log.WarnFormat("Invalid username-password combination: {0}:{1} on SiteId = {2}", username, password, site.SiteId.ToString());
            }
            return user;
         }
         catch (Exception ex)
         {
            log.ErrorFormat("An error occured while logging in user {0} on SiteId = {1}", username, site.SiteId.ToString());
            throw new Exception(String.Format("Unable to log in user '{0}': " + ex.Message, username), ex);
         }
      }




      /// <summary>
      /// Reset a user password creating a new one.
      /// </summary>
      /// <param name="user"></param>
      /// <returns>The new password</returns>
      public string ResetPassword(User user)
      {
         string password = user.GeneratePassword();

         // Save the user
         userService.SaveUser(user);

         return password;
      }

   }
}
