namespace Arashi.Services.Membership
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using Arashi.Core.Domain;
   using Arashi.Core.Domain.Extensions;
   using Arashi.Core.Extensions;
   using Arashi.Core.NHibernate;
   using Arashi.Core.Repositories;
   using Arashi.Services;
   using Common.Logging;
   using NHibernate;
   using NHibernate.Criterion;
   using NHibernate.Impl;
   using uNhAddIns.Pagination;



	/// <summary>
	/// Service for user & roles management
	/// </summary>
	public class DefaultUserService : ServiceBase, IUserService
	{

	   /// <summary>
		/// Constructor.
		/// </summary>
      public DefaultUserService(Arashi.Core.NHibernate.ISessionFactory sessionFactory, ILog log)
         : base(sessionFactory, log)
      {
      }


      public long CountAllUsersForAllSites()
      {
         return Session.GetNamedQuery("CountAllUsersForAllSites")
                    .UniqueResult<long>();
      }
      
		#region IUserService Members

      #region User

      //public IList<Domain.User> GetAllSiteUsers(Site site)
      //{
      //   using (ISession session = RepositoryHelper.GetSession())
      //   {
      //      string hql = "from User u where u.Site = :site and u.IsLogicallyDeleted = 0 and u.IsActive = 1";
      //      IQuery query = session.CreateQuery(hql);
      //      query.SetEntity("site", site);

      //      return query.List<Domain.User>();
      //   }
      //}


	   public Paginator<User> GetUserPaginatorBySite(Site site, int pageSize)
      {
         if (site == null)
            throw new ArgumentNullException("site");

	      IDetachedQuery query = new DetachedQuery("from User u where u.Site = :site and u.IsLogicallyDeleted = 0");
         query.SetEntity("site", site);

         return Repository<User>.GetPaginator(query, pageSize);
      }


      public IList<User> FindUsersByEmail(Site site, string email)
      {
         if (site == null) 
            throw new ArgumentNullException("site");

         DetachedCriteria crit = DetachedCriteria.For<User>();

         crit.Add(Restrictions.Eq("Email", email));
         crit.Add(Restrictions.Eq("Site", site));
         crit.Add(Restrictions.Eq("IsLogicallyDeleted", false));
         Order[] o = new Order[] { new Order("Email", true) };
         return Repository<User>.FindAll(crit, o);
      }



      /// <summary>
      /// L'utente data una email per un dato site.
      /// Attenzione, può generare exception se ci sono doppioni
      /// </summary>
      /// <param name="site"></param>
      /// <param name="email"></param>
      /// <returns></returns>
      public User FindUserByEmail(Site site, string email)
      {
         if (site == null)
            throw new ArgumentNullException("site");

         DetachedCriteria crit = DetachedCriteria.For<User>();

         crit.Add(Restrictions.Eq("Email", email));
         crit.Add(Restrictions.Eq("Site", site));
         crit.Add(Restrictions.Eq("IsLogicallyDeleted", false));
         
         return Repository<User>.FindOne(crit);
      }



      public User FindUserByDisplayName(Site site, string displayName)
      {
         if (site == null)
            throw new ArgumentNullException("site");

         DetachedCriteria crit = DetachedCriteria.For<User>();

         crit.Add(Restrictions.Eq("DisplayName", displayName));
         crit.Add(Restrictions.Eq("Site", site));
         crit.Add(Restrictions.Eq("IsLogicallyDeleted", false));

         return Repository<User>.FindOne(crit);
      }




	   public IList<User> FindAllUsers(Site site)
      {
         if (site == null)
            throw new ArgumentNullException("site");

         DetachedCriteria criteria = DetachedCriteria.For<User>();
         criteria.Add(Restrictions.Eq("IsLogicallyDeleted", false));
         criteria.Add(Restrictions.Eq("Site", site));

         return Repository<User>.FindAll(criteria);
      }



      public long CountAllUsers(Site site)
      {
         if (site == null)
            throw new ArgumentNullException("site");

         return Session.GetNamedQuery("CountAllUsers")
                    .SetEntity("site", site)
                    .UniqueResult<long>();
      }




      public long CountOtherUsersBySiteAndEmail(Site site, string email, User user)
      {
         if (site == null)
            throw new ArgumentNullException("site");

         long ret = Session.GetNamedQuery("CountOtherUsersBySiteAndEmail")
                    .SetEntity("site", site)
                    .SetString("email", email.Trim())
                    .SetInt32("id", user.UserId)
                   .UniqueResult<long>();
         return ret;
      }



      public long CountOtherUsersBySiteAndDisplayName(Site site, string displayName, User user)
      {
         if (site == null)
            throw new ArgumentNullException("site");

         return Session.GetNamedQuery("CountOtherUsersBySiteAndDisplayName")
                  .SetEntity("site", site)
                  .SetString("name", displayName.Trim())
                  .SetInt32("id", user.UserId)
                  .UniqueResult<long>();
      }



		public User GetUserById(int userId)
		{
         IQuery query = Session.GetNamedQuery("GetUserById")
                     .SetInt32("userid", userId);
         return query.UniqueResult<User>();
		}



      public User GetSystemUser()
      {
         return Repository<User>.FindById(0);
      }




      public User GetUserByEmailAndPassword(Site site, string email, string hashedPassword)
      {
         if (site == null)
            throw new ArgumentNullException("site");

         return Session.GetNamedQuery("GetUserByEmailAndPassword")
                  .SetEntity("site", site)
                  .SetString("password", hashedPassword)
                  .SetString("email", email)
                  .UniqueResult<User>();
      }


      /// <summary>
      /// Get a user by Site and OpenID identifier / Facebook user id
      /// </summary>
      /// <param name="site"></param>
      /// <param name="externalId"></param>
      /// <returns></returns>
      public User GetUserBySiteAndExternalId(Site site, string externalId)
      {
         if (site == null)
            throw new ArgumentNullException("site");

         DetachedCriteria criteria = DetachedCriteria.For<User>();
         criteria.Add(Restrictions.Eq("IsLogicallyDeleted", false));
         criteria.Add(Restrictions.Eq("Site", site));
         criteria.Add(Restrictions.Eq("ExternalId", externalId));
         
         return Repository<User>.FindOne(criteria);

         //return RepositoryHelper.GetSession().GetNamedQuery("GetUserBySiteAndOpenID")
         //         .SetEntity("site", site)
         //         .SetString("openid", openIdIdentifier)
         //         .UniqueResult<User>();
      }



	   public ServiceResult SaveUser(User user)
      {
         //using (NHTransactionScope tx = new NHTransactionScope())
         //{
            ServiceResult result = new ServiceResult()
            {
               State = ServiceResult.ServiceState.Success
            };

            user.Email = user.Email.Trim();
            user.DisplayName = user.DisplayName.Trim();
            Repository<User>.Save(user);
            //tx.VoteCommit();

            return result;
         //}
      }



      public void UpdateUser(User user)
      {
         //using (NHTransactionScope tx = new NHTransactionScope())
         //{
         Repository<User>.Save(user);
         //   tx.VoteCommit();
         //}
      }



      public void DeleteUser(User user)
      {
         //using (NHTransactionScope tx = new NHTransactionScope())
         //{
         user.IsLogicallyDeleted = true;
         Repository<User>.Save(user);
         //   tx.VoteCommit();
         //}
      }



	   public string ResetPassword(Site site, string email)
		{
			User user = FindUserByEmail(site, email);

         if (user == null)
            throw new NullReferenceException("No user found with the given username and email");

	      return ResetPassword(user);
		}



		public string ResetPassword(User user)
		{
         string newPassword = user.GeneratePassword();

         SaveUser(user);
         return newPassword;
      }


      #endregion

      #region Role

      //public Role GetStandardRole(Site site, StandardRoles role)
      //{
      //   DetachedCriteria criteria = DetachedCriteria.For<Role>();
      //   criteria.Add(Restrictions.Eq("Site", site));
         
      //   string roleName = Enum.GetName(typeof(StandardRoles), role).Replace('_', ' ');
      //   criteria.Add(Restrictions.Eq("Name", roleName));

      //   return Repository<Role>.FindOne(criteria);
      //}



      public Role GetRoleByName(Site site, string roleName)
      {
         DetachedCriteria criteria = DetachedCriteria.For<Role>();
         criteria.Add(Restrictions.Eq("Site", site));
         criteria.Add(Restrictions.Eq("Name", roleName.Replace('_', ' ')));

         return Repository<Role>.FindOne(criteria);
      }



      public Paginator<Role> GetRolePaginatorBySite(Site site, int pageSize)
      {
         IDetachedQuery query = new DetachedQuery("from Role r where r.Site = :site");
         query.SetEntity("site", site);

         return Repository<Role>.GetPaginator(query, pageSize);
      }


      
      public IList<Role> GetRolesBySite(Site site)
      {
         IQuery query = Session.CreateQuery("from Role r where r.Site = :site order by r.Name")
            .SetEntity("site", site);
         return query.List<Role>();
      }



      public IList<Role> GetRolesByIds(int[] ids)
      {
         return Session.CreateQuery("from Role where RoleId in (:ids)")
            .SetParameterList("ids", ids)
            .List<Role>();
      }



	   public Role GetRoleById(int roleId)
      {
         return Repository<Role>.FindById(roleId);
      }


      // TODO: rename to Save
      public void UpdateRole(Role role)
      {
         //using (NHTransactionScope tx = new NHTransactionScope())
         //{
         Repository<Role>.Save(role);
         //   tx.VoteCommit();
         //}
      }


      public void DeleteRole(Role role)
      {
         //using (NHTransactionScope tx = new NHTransactionScope())
         //{
         Repository<Role>.Delete(role);
         //   tx.VoteCommit();
         //}
      }

      #endregion

      #region Users In Role

      public Paginator<User> GetUsersInRolePaginator(Role role, int pageSize)
      {
         IDetachedQuery query = new DetachedQuery("from User u where :role in elements(u.Roles) and u.Site = :site and u.IsLogicallyDeleted = 0");
         query.SetEntity("role", role);
         query.SetEntity("site", role.Site);

         return Repository<User>.GetPaginator(query, pageSize);
      }



      public IList<User> GetUsersInRole(Role role)
      {
         IDetachedQuery dq = new DetachedQuery("from User u where :role in elements(u.Roles) and u.Site = :site and u.IsLogicallyDeleted = 0");
         dq.SetEntity("role", role);
         dq.SetEntity("site", role.Site);

         using (ISession session = Session){
            IQuery query = dq.GetExecutableQuery(session);

            return query.List<User>();
         }
      }

      #endregion

      #region Rights

      public IList<Right> GetAllRights()
      {
         //return RepositoryHelper.GetSession().GetNamedQuery("GetAllRights")
         //         .List<Right>();
         var query = Session.QueryOver<Right>()
                        .OrderBy(r => r.Name).Asc
                        .List();
         return query;
      }



      public IList<Right> GetRightsByIds(int[] rightIds)
      {
         //return RepositoryHelper.GetSession().GetNamedQuery("GetRightsByIds")
         //         .SetParameterList("ids", rightIds)
         //         .List<Right>();
         var query = Session.QueryOver<Right>()
                        .Where(r => r.Id.IsIn(rightIds))
                        .List();
         return query;
      }

	   #endregion

      #region Create Default Roles

      /// <summary>
      /// Create the default roles 
      /// </summary>
      /// <param name="site"></param>
      public void CreateDefaultRoles(Site site)
      {
         log.Debug("DefaultUserService.CreateDefaultRoles: Start");

         string[] administratorRoleRights = new string[]{Rights.AdminAccess,
                                                         Rights.SiteCreate,
                                                         Rights.SiteDelete,
                                                         Rights.DashboardAccess ,
                                                         Rights.PostsView,
                                                         Rights.PostsEdit,
                                                         Rights.PostsDelete,
                                                         Rights.CommentsView,
                                                         Rights.CommentsEdit,
                                                         Rights.CommentsDelete,
                                                         Rights.PagesView,
                                                         Rights.PagesEdit,
                                                         Rights.PagesDelete,
                                                         Rights.SiteSettingsView,
                                                         Rights.SiteSettingsEdit,
                                                         Rights.TemplatesView,
                                                         Rights.TemplatesChange ,
                                                         Rights.UsersView,
                                                         Rights.UsersEdit,
                                                         Rights.UsersDelete,
                                                         Rights.RolesView,
                                                         Rights.RolesEdit,
                                                         Rights.RolesDelete,
                                                         Rights.FilesView,
                                                         Rights.FilesEdit,
                                                         Rights.FilesUpload,
                                                         Rights.FilesDelete,
                                                         Rights.SystemConfigurationView,
                                                         Rights.SystemConfigurationEdit
                                                      };

         string[] editorsRoleRights = new string[]{   Rights.AdminAccess,
                                                      Rights.DashboardAccess ,
                                                      Rights.PostsView,
                                                      Rights.PostsEdit,
                                                      Rights.CommentsView,
                                                      Rights.CommentsEdit,
                                                      Rights.CommentsDelete,
                                                      Rights.PagesView,
                                                      Rights.SiteSettingsView,
                                                      Rights.TemplatesView,
                                                      Rights.UsersView,
                                                      Rights.RolesView,
                                                      Rights.FilesView,
                                                      Rights.FilesEdit,
                                                      Rights.FilesUpload,
                                                      Rights.FilesDelete
                                                   };

         string[] authenticatedUsersRoleRights = new string[]{ Rights.AdminAccess,
                                                               Rights.DashboardAccess 
                                                            };

         string[] demoUsersRoleRights = new string[]{ Rights.AdminAccess,
                                                      Rights.DashboardAccess ,
                                                      Rights.PostsView,
                                                      Rights.CommentsView,
                                                      Rights.PagesView,
                                                      Rights.SiteSettingsView,
                                                      Rights.TemplatesView,
                                                      Rights.UsersView,
                                                      Rights.RolesView,
                                                      Rights.FilesView,
                                                   };

         const string administratorsRoleLabel = "Administrators";
         const string editorsRoleLabel = "Editors";
         const string authenticatedUsersLabel = "Authenticated Users";
         const string demoUsersRoleLabel = "Demo Users";

         IList<Right> rights = GetAllRights();

         // Create Admnistrators Role
         Role administratorsRole = new Role
         {
            Name = administratorsRoleLabel,
            Site = site
         };

         // Add the configured rights
         administratorRoleRights.Each().Do((rightName) =>
         {
            Right right = (from r in rights
                           where r.Name == rightName
                           select r).Single();

            administratorsRole.Rights.Add(right);
         }
                                       );

         // Create Editors Role
         Role editorsRole = new Role
         {
            Name = editorsRoleLabel,
            Site = site
         };

         // Add the configured rights
         editorsRoleRights.Each().Do((rightName) =>
         {
            Right right = (from r in rights
                           where r.Name == rightName
                           select r).Single();

            editorsRole.Rights.Add(right);
         }
                                       );

         // Create Authenticated Users Role
         Role authenticatedUsersRole = new Role
         {
            Name = authenticatedUsersLabel,
            Site = site
         };

         // Add the configured rights
         authenticatedUsersRoleRights.Each().Do((rightName) => {
                                                                  Right right = (from r in rights
                                                                                 where r.Name == rightName
                                                                                 select r).Single();

                                                                  authenticatedUsersRole.Rights.Add(right);
                                                               }
                                                            );

         // Create Authenticated Users Role
         Role demoRole = new Role
         {
            Name = demoUsersRoleLabel,
            Site = site
         };

         // Add the configured rights
         demoUsersRoleRights.Each().Do((rightName) =>
                                          {
                                             Right right = (from r in rights
                                                            where r.Name == rightName
                                                            select r).Single();

                                             demoRole.Rights.Add(right);
                                          }
                                       );

         //using (NHTransactionScope tx = new NHTransactionScope())
         //{
            UpdateRole(administratorsRole);
            UpdateRole(editorsRole);
            UpdateRole(authenticatedUsersRole);
            UpdateRole(demoRole);

            //tx.VoteCommit();

            log.Debug("DefaultUserService.CreateDefaultRoles: Default Roles created");
         //}

         log.Debug("DefaultUserService.CreateDefaultRoles: End");
      }

      #endregion

      #endregion
   }
}
