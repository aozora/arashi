namespace Arashi.Services.Membership
{
   using Arashi.Core.Domain;

   /// <summary>
   /// Provides functionality for authenticating users.
   /// </summary>
   public interface IAuthenticationService
   {
      /// <summary>
      /// Authenticate a user with username & password
      /// </summary>
      /// <param name="site"></param>
      /// <param name="email"></param>
      /// <param name="password"></param>
      /// <param name="ipAddress"></param>
      /// <param name="createPersistentCookie"></param>
      /// <returns></returns>
      User AuthenticateUser(Site site, string email, string password, string ipAddress, bool createPersistentCookie);

      /// <summary>
      /// Authenticate a user with OpenID
      /// </summary>
      /// <param name="site"></param>
      /// <param name="externalId">OpenID identifier or Facebook User Id</param>
      /// <param name="ipAddress"></param>
      /// <param name="createPersistentCookie"></param>
      /// <returns></returns>
      User AuthenticateUser(Site site, string externalId, string ipAddress, bool createPersistentCookie);

      string ResetPassword(User user);

      void LogIn(User user, string ipAddress, bool createPersistentCookie);

   }
}
