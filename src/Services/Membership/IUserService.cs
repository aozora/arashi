using System;
using Arashi.Core.Domain;
using System.Collections.Generic;
using Arashi.Services;
using uNhAddIns.Pagination;

namespace Arashi.Services.Membership
{
	/// <summary>
	/// Provides functionality for user management.
	/// </summary>
	public interface IUserService
   {
      #region User

      /// <summary>
      /// Get the count of all users for the given site
      /// </summary>
      /// <returns></returns>
      long CountAllUsersForAllSites();


      /// <summary>
      /// Restituisce un oggetto Paginator con tutti gli utenti di un Site
      /// </summary>
      /// <param name="site"></param>
      /// <param name="pageSize"></param>
      /// <returns></returns>
      Paginator<User> GetUserPaginatorBySite(Site site, int pageSize);


      
      /// <summary>
      /// Find all users with the same email........ 
      /// </summary>
      /// <param name="site"></param>
      /// <param name="email"></param>
      /// <returns></returns>
      IList<User> FindUsersByEmail(Site site, string email);


      /// <summary>
      /// Find a single user given its email
      /// Warning, if more users with the same email exists, an exception will be thrown
      /// </summary>
      /// <param name="site"></param>
      /// <param name="email"></param>
      /// <returns></returns>
      User FindUserByEmail(Site site, string email);


      /// <summary>
      /// Find a user given its displayname
      /// </summary>
      /// <param name="site"></param>
      /// <param name="displayName"></param>
      /// <returns></returns>
      User FindUserByDisplayName(Site site, string displayName);


      /// <summary>
      /// Find all users 
      /// </summary>
      /// <param name="site"></param>
      /// <returns></returns>
      IList<User> FindAllUsers(Site site);


      /// <summary>
      /// Get the count of all users for the given site
      /// </summary>
      /// <param name="site"></param>
      /// <returns></returns>
      long CountAllUsers(Site site);


      /// <summary>
      /// Count the site users that have the given email
      /// </summary>
      /// <param name="site"></param>
      /// <param name="email"></param>
      /// <returns></returns>
      long CountOtherUsersBySiteAndEmail(Site site, string email, User user);



      /// <summary>
      /// Return true if the given diplayname is unique, false if is already used by ohter users of the given site
      /// </summary>
      /// <param name="site"></param>
      /// <param name="displayName"></param>
      /// <returns></returns>
      long CountOtherUsersBySiteAndDisplayName(Site site, string displayName, User user);


		/// <summary>
		/// Get a user by ID.
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		User GetUserById(int userId);


      
      /// <summary>
      /// Restituisce l'utente di default per le operazioni di sistema e servizio.
      /// </summary>
      /// <returns></returns>
      User GetSystemUser();



      /// <summary>
      /// Get a user by email and password
      /// </summary>
      /// <param name="site"></param>
      /// <param name="email"></param>
      /// <param name="hashedPassword"></param>
      /// <returns></returns>
      User GetUserByEmailAndPassword(Site site, string email, string hashedPassword);


      /// <summary>
      /// Get a user by Site and OpenID identifier / Facebook user id
      /// </summary>
      /// <param name="site"></param>
      /// <param name="externalId"></param>
      /// <returns></returns>
      User GetUserBySiteAndExternalId(Site site, string externalId);



		/// <summary>
		/// Save/update an existing user. This method will check for email and displayname uniqueness
		/// </summary>
		/// <param name="user"></param>
      ServiceResult SaveUser(User user);



      /// <summary>
      /// Update an existing user.
      /// </summary>
      /// <param name="user"></param>
      void UpdateUser(User user);


		/// <summary>
		/// Logically delete an existing user.
		/// </summary>
		/// <param name="user"></param>
		void DeleteUser(User user);

      /// <summary>
      /// Reset the password of the given user.
      /// </summary>
      /// <param name="user"></param>
      /// <returns></returns>
	   string ResetPassword(User user);

      /// <summary>
      /// Reset the password of the user with the given username and email address.
      /// </summary>
      /// <param name="site"></param>
      /// <param name="username"></param>
      /// <param name="email"></param>
      /// <returns>The new password</returns>
      string ResetPassword(Site site, string email);




      #endregion

      #region Roles

      /// <summary>
      /// Restituisce un oggetto Paginator con tutti i Ruoli di un Site
      /// </summary>
      /// <param name="site"></param>
      /// <param name="pageSize"></param>
      /// <returns></returns>
      Paginator<Role> GetRolePaginatorBySite(Site site, int pageSize);


      /// <summary>
      /// Get all available roles.
      /// </summary>
      /// <param name="site"></param>
      /// <returns></returns>
      IList<Role> GetRolesBySite(Site site);

      //IList<Role> GetRolesByAccessLevel(AccessLevel accessLevel);

	   IList<Role> GetRolesByIds(int[] ids);

      /// <summary>
      /// Get a single role by id.
      /// </summary>
      /// <param name="roleId"></param>
      /// <returns></returns>
      Role GetRoleById(int roleId);

      //Role GetDefaultAdministratorsRole(Site site);


      //Role GetStandardRole(Site site, StandardRoles role);

	   Role GetRoleByName(Site site, string roleName);

      void UpdateRole(Role role);

      void DeleteRole(Role role);

      Paginator<User> GetUsersInRolePaginator(Role role, int pageSize);

      IList<User> GetUsersInRole(Role role);

      #endregion

      #region Rights

	   IList<Right> GetAllRights();

	   IList<Right> GetRightsByIds(int[] rightIds);

      #endregion

      #region Create Default Roles

	   void CreateDefaultRoles(Site site);

      #endregion
   }
}
