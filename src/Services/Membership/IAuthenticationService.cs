using System;
using Arashi.Core.Domain;

namespace Arashi.Services.Membership
{
	/// <summary>
	/// Provides functionality for authenticating users.
	/// </summary>
	public interface IAuthenticationService
	{
      /// <summary>
      /// Authenticate a user
      /// </summary>
      /// <param name="site"></param>
      /// <param name="userName"></param>
      /// <param name="password"></param>
      /// <param name="ipAddress"></param>
      /// <param name="createPersistentCookie"></param>
      /// <returns></returns>
      User AuthenticateUser(Site site, string userName, string password, string ipAddress, bool createPersistentCookie);

	   string ResetPassword(User user);

      void LogIn(User user, string ipAddress, bool createPersistentCookie);

	}
}
