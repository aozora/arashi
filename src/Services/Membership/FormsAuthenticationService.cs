using System;
using System.Web;
using System.Web.Security;
using Arashi.Core.Domain;
using Arashi.Core.Domain.Extensions;
using Arashi.Core.Extensions;
using Common.Logging;
using ApplicationException = Arashi.Core.Exceptions.ApplicationException;

namespace Arashi.Services.Membership
{
   public class FormsAuthenticationService : ServiceBase, IAuthenticationService
   {
      private IUserService userService;

      public FormsAuthenticationService(Arashi.Core.NHibernate.ISessionFactory sessionFactory, ILog log, IUserService userService)
         : base(sessionFactory, log)
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
      /// Try to authenticate the user with username & password
      /// </summary>
      /// <param name="site"></param>
      /// <param name="email"></param>
      /// <param name="password"></param>
      /// <param name="ipAddress"></param>
      /// <param name="createPersistentCookie"></param>
      /// <returns></returns>
      public User AuthenticateUser(Site site, string email, string password, string ipAddress, bool createPersistentCookie)
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
            User user = userService.GetUserByEmailAndPassword(site, email, hashedPassword);

            if (user != null)
            {
               LogIn(user, ipAddress, createPersistentCookie);
            }
            else
            {
               log.WarnFormat("Invalid username-password combination: {0}:{1} on SiteId = {2}", email, password, site.SiteId.ToString());
            }
            return user;
         }
         catch (Exception ex)
         {
            log.ErrorFormat("An error occured while logging in user {0} on SiteId = {1}", email, site.SiteId.ToString());
            throw new Exception(String.Format("Unable to log in user '{0}': " + ex.Message, email), ex);
         }
      }



      /// <summary>
      /// Try to authenticate the user with OpenID
      /// </summary>
      /// <param name="site"></param>
      /// <param name="externalId">OpenID identifier or Facebook User Id</param>
      /// <param name="ipAddress"></param>
      /// <param name="createPersistentCookie"></param>
      /// <returns></returns>
      public User AuthenticateUser(Site site, string externalId, string ipAddress, bool createPersistentCookie)
      {
         if (site == null)
         {
            const string msg = "FormsAuthenticationService.AuthenticateUser invoked with Site = NULL";
            log.Error(msg);
            throw new ApplicationException(msg);
         }

         try
         {
            User user = userService.GetUserBySiteAndExternalId(site, externalId);

            if (user != null)
            {
               LogIn(user, ipAddress, createPersistentCookie);
            }
            else
            {
               log.WarnFormat("Invalid External Identifier: {0} on SiteId = {1}", externalId, site.SiteId.ToString());
            }
            return user;
         }
         catch (Exception ex)
         {
            log.ErrorFormat("An error occured while logging in External Identifier {0} on SiteId = {1}", externalId, site.SiteId.ToString());
            throw new Exception(String.Format("Unable to log in External Identifier '{0}': " + ex.Message, externalId), ex);
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
