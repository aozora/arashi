namespace Arashi.Web.Areas.Admin.Controllers
{
   using System;
   using System.Collections.Generic;
   using System.Collections.Specialized;
   using System.Configuration;
   using System.IO;
   using System.Text;
   using System.Web.Mvc;
   using Arashi.Core.Domain;
   using Arashi.Core.Domain.Extensions;
   using Arashi.Core.Extensions;
   using Arashi.Core.Util;
   using Arashi.Services;
   using Arashi.Services.Membership;
   using Arashi.Web.Areas.Admin.Models;
   using Arashi.Web.Components;
   using Arashi.Web.Mvc.Controllers;
   using Arashi.Web.Mvc.Extensions;
   using Arashi.Web.Mvc.Filters;
   using Arashi.Web.Mvc.Models;
   using Arashi.Web.Mvc.Paging;

   using log4net;

   using uNhAddIns.Pagination;

   using xVal.ServerSide;

   public class UsersController : SecureControllerBase
   {
      private static readonly ILog log = LogManager.GetLogger(typeof(UsersController));
      private const int pageSize = 20;


      public UsersController()
      {
      }



      /// <summary>
      /// Users list
      /// </summary>
      /// <param name="page">
      /// The page id
      /// </param>
      /// <returns>
      /// </returns>
      [PermissionFilter(RequiredRights = Rights.UsersView)]
      public ActionResult Index(int? page)
      {
         IList<User> pages = null;
         Paginator<User> paginator;
         IPagedList<User> pagedList = null;

         int currentPageIndex = 1;
         if (page.HasValue)
            currentPageIndex = page.Value;

         //if (string.IsNullOrEmpty(selectedCulture))
         //paginator = pageService.GetPaginatorBySite(Context.ManagedSite, pageSize);
         paginator = userService.GetUserPaginatorBySite(Context.ManagedSite, pageSize);
         //else
         //   paginator = pageService.GetPaginatorBySiteAndCulture(ManagedSite, selectedCulture, pageSize);

         if (paginator.HasPages)
         {
            if (currentPageIndex > paginator.LastPageNumber)
               currentPageIndex = 1;

            pages = paginator.GetPage(currentPageIndex);

            pagedList = new PagedList<User>(pages, page.HasValue ? page.Value - 1 : 0, pageSize, paginator.RowsCount.Value);// , totalcount
         }

         return View("Index", pagedList);
      }

      #region Edit User

      /// <summary>
      /// Show the user edit view
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
      [PermissionFilter(RequiredRights = Rights.UsersView)]
      public ActionResult Details(int id)
      {
         User user = userService.GetUserById(id);

         UserModel model = new UserModel
                              {
                                 User = userService.GetUserById(id),
                                 Roles = userService.GetRolesBySite(Context.ManagedSite),
                                 TimeZones = new SelectList(TimeZoneUtil.GetTimeZones(), "Key", "Value", user.TimeZone),
                                 AdminThemes = new SelectList(GetAdminThemesList(), user.AdminTheme),
                                 Cultures = new SelectList(Globalization.GetOrderedCultures(), "Name", "DisplayName", user.AdminCulture)
                              };

         return View("Details", model);
      }



      /// <summary>
      /// Save the user details
      /// </summary>
      /// <param name="id">
      /// </param>
      /// <param name="DisplayName">
      /// </param>
      /// <param name="FirstName">
      /// </param>
      /// <param name="LastName">
      /// </param>
      /// <param name="Email">
      /// </param>
      /// <param name="Website">
      /// </param>
      /// <param name="IsActive">
      /// </param>
      /// <param name="TimeZone">
      /// </param>
      /// <param name="Description">
      /// </param>
      /// <param name="AdminTheme">
      /// The Admin Theme.
      /// </param>
      /// <param name="AdminCulture">
      /// The Admin Culture.
      /// </param>
      /// <param name="roleIds">
      /// </param>
      /// <returns>
      /// </returns>
      [ValidateInput(false)]
      [AcceptVerbs(HttpVerbs.Post)]
      [PermissionFilter(RequiredRights = Rights.UsersEdit)]
      [ValidateAntiForgeryToken(Salt = "update")]
      public ActionResult Update(int id, string DisplayName, string FirstName, string LastName, string Email, string Website, bool IsActive, int TimeZone, string Description, string AdminTheme, string AdminCulture, int[] roleIds)
      {
         User user = userService.GetUserById(id);
         UserModel model = new UserModel
         {
            User = userService.GetUserById(id),
            Roles = userService.GetRolesBySite(Context.ManagedSite),
            TimeZones = new SelectList(TimeZoneUtil.GetTimeZones(), "Key", "Value", user.TimeZone),
            AdminThemes = new SelectList(GetAdminThemesList(), user.AdminTheme),
            Cultures = new SelectList(Globalization.GetOrderedCultures(), "Name", "DisplayName", user.AdminCulture)
         };

         try
         {
            string invalidMessage = string.Empty;

            // Unique Email
            if (userService.CountOtherUsersBySiteAndEmail(Context.ManagedSite, Email, model.User) > 0)
            {
               invalidMessage = "Message_UserEmailIsNotUnique";
            }

            // Unique DisplayName Validation
            if (userService.CountOtherUsersBySiteAndDisplayName(Context.ManagedSite, DisplayName, model.User) > 0)
            {
               invalidMessage = "Message_UserDisplayNameIsNotUnique";
            }

            TryUpdateModel<User>(model.User, new[] { "DisplayName", "FirstName", "LastName", "Email", "Website", "IsActive", "TimeZone", "Description", "AdminTheme", "AdminCulture" });

            if (ModelState.IsValid)
            {
               // Clear existing roles
               model.User.Roles.Clear();

               // update the roles
               if (roleIds != null && roleIds.Length > 0)
               {
                  IList<Role> roles = userService.GetRolesByIds(roleIds);
                  foreach (Role role in roles)
                  {
                     model.User.Roles.Add(role);
                  }
               }


               if (invalidMessage.Length == 0)
               {
                  model.User.UpdatedDate = DateTime.UtcNow;
                  ServiceResult result = userService.SaveUser(model.User);

                  // Show the confirmation message
                  MessageModel message = new MessageModel
                  {
                     Text = GlobalResource("Message_UserSaved"),
                     Icon = MessageModel.MessageIcon.Info,
                     CssClass = "margin-topbottom"
                  };
                  RegisterMessage(message, true);

                  return RedirectToAction("Index");
               }
               else
               {
                  MessageModel message = new MessageModel
                  {
                     // get the message by the token!!! uao!
                     Text = GlobalResource(invalidMessage),
                     Icon = MessageModel.MessageIcon.Alert,
                     CssClass = "margin-topbottom"
                  };
                  RegisterMessage(message);
               }


            }
         }
         catch (RulesException ex)
         {
            ex.AddModelStateErrors(ModelState, "user");
         }
         catch (Exception ex)
         {
            log.Error("UsersController.Update", ex);

            MessageModel message = new MessageModel
                                    {
                                       Text = GlobalResource("Message_GenericError"),
                                       Icon = MessageModel.MessageIcon.Alert,
                                       CssClass = "margin-topbottom"
                                    };
            RegisterMessage(message);
         }

         return View("Details", model);
      }



      /// <summary>
      /// Save the new password
      /// </summary>
      /// <param name="id"></param>
      /// <param name="password"></param>
      /// <param name="passwordConfirmation"></param>
      /// <returns></returns>
      [AcceptVerbs(HttpVerbs.Post)]
      [ValidateAntiForgeryToken(Salt = "changepassword")]
      [PermissionFilter(RequiredRights = Rights.UsersEdit)]
      public ActionResult ChangePassword(int id, string password, string passwordConfirmation)
      {
         var changePasswordModel = new ChangePasswordModel()
         {
            Password = password,
            PasswordConfirmation = passwordConfirmation
         };

         User user = userService.GetUserById(id);
         UserModel model = null;

         try
         {
            model = new UserModel
            {
               User = userService.GetUserById(id),
               Roles = userService.GetRolesBySite(Context.ManagedSite),
               TimeZones = new SelectList(TimeZoneUtil.GetTimeZones(), "Key", "Value", user.TimeZone),
               AdminThemes = new SelectList(GetAdminThemesList(), user.AdminTheme),
               Cultures = new SelectList(Globalization.GetOrderedCultures(), "Name", "DisplayName", user.AdminCulture)
            };

           if (ModelState.IsValid && TryUpdateModel(changePasswordModel))
            {
               user.Password = user.HashPassword(changePasswordModel.Password);
               user.PasswordConfirmation = user.HashPassword(changePasswordModel.PasswordConfirmation);
               user.UpdatedDate = DateTime.UtcNow;

               userService.SaveUser(user);

               MessageModel message = new MessageModel
                                       {
                                          Text = GlobalResource("Message_UserPasswordChanged"),
                                          Icon = MessageModel.MessageIcon.Info,
                                       };
               RegisterMessage(message, true);

               return View("Details", model);
            }
            else
            {
               IEnumerable<ErrorInfo> errors = DataAnnotationsValidationRunner.GetErrors(changePasswordModel);
               errors.Each().Do(error => log.DebugFormat("UserController.ChangePassword: Property: {0}, Error: {1}", error.PropertyName, error.ErrorMessage));
               throw new RulesException(errors);
            }

         }
         catch (RulesException ex)
         {
            ex.AddModelStateErrors(ModelState, "changepassword");
            StringBuilder errorMessage = new StringBuilder();
            ex.Errors.Each().Do(error => errorMessage.AppendFormat("{0}<br/>", error.ErrorMessage));

            MessageModel message = new MessageModel
            {
               Text = errorMessage.ToString(),
               Icon = MessageModel.MessageIcon.Alert,
            };
            RegisterMessage(message);
         }
         catch (Exception ex)
         {
            log.Error("UsersController.ChangePassword", ex);

            MessageModel message = new MessageModel
                                    {
                                       Text = GlobalResource("Message_GenericError"),
                                       Icon = MessageModel.MessageIcon.Alert,
                                    };
            RegisterMessage(message, true);
         }

         return View("Details", model);
      }

      #endregion

      #region New User

      /// <summary>
      /// Show the new user view
      /// </summary>
      /// <returns></returns>
      [PermissionFilter(RequiredRights = Rights.UsersEdit)]
      public ActionResult NewUser()
      {
         User user = new User();
         ViewData["Roles"] = userService.GetRolesBySite(Context.ManagedSite);
         ViewData["TimeZones"] = new SelectList(TimeZoneUtil.GetTimeZones(), "Key", "Value", user.TimeZone);
         ViewData["AdminThemes"] = new SelectList(GetAdminThemesList());
         ViewData["Cultures"] = new SelectList(Globalization.GetOrderedCultures(), "Name", "DisplayName", user.AdminCulture);

         return View("NewUser", user);
      }


      /// <summary>
      /// Create a new user
      /// </summary>
      /// <param name="user"></param>
      /// <param name="roleIds">List of selected role id. It is null if no role is selected.</param>
      /// <returns></returns>
      [ValidateInput(false)]
      [AcceptVerbs(HttpVerbs.Post)]
      [PermissionFilter(RequiredRights = Rights.UsersEdit)]
      [ValidateAntiForgeryToken]
      public ActionResult Create(User user, int[] roleIds)
      {
         //User user = new User();
        
         try
         {
            if (ModelState.IsValid)
            {
               UpdateModel(user, new[] { "DisplayName", "FirstName", "LastName", "Email", "Website", "IsActive", "TimeZone", "Description", "AdminTheme", "AdminCulture" });

               // server validation
               string invalidMessage = string.Empty;

               // Unique Email
               if (userService.CountOtherUsersBySiteAndEmail(Context.ManagedSite, user.Email, user) > 0)
               {
                  invalidMessage = "Message_UserEmailIsNotUnique";
               }

               // Unique DisplayName Validation
               if (userService.CountOtherUsersBySiteAndDisplayName(Context.ManagedSite, user.DisplayName, user) > 0)
               {
                  invalidMessage = "Message_UserDisplayNameIsNotUnique";
               }


               if (invalidMessage.Length == 0)
               {
                  user.Password = user.HashPassword(user.Password);
                  user.PasswordConfirmation = user.HashPassword(user.PasswordConfirmation);

                  user.Roles.Clear();

                  if (roleIds != null && roleIds.Length > 0)
                  {
                     IList<Role> roles = userService.GetRolesByIds(roleIds);
                     foreach (Role role in roles)
                     {
                        user.Roles.Add(role);
                     }
                  }

                  user.Site = Context.ManagedSite;
                  user.CreatedDate = DateTime.UtcNow;

                  ServiceResult result = userService.SaveUser(user);

                  if (result.State == ServiceResult.ServiceState.Success)
                  {
                     // Show the confirmation message
                     MessageModel message = new MessageModel
                                               {
                                                  Text =
                                                     string.Format(GlobalResource("Message_UserCreated"),
                                                                   user.DisplayName),
                                                  Icon = MessageModel.MessageIcon.Info,
                                               };
                     RegisterMessage(message, true);

                     return RedirectToAction("Index");
                  }
                  else
                  {
                     MessageModel message = new MessageModel
                                               {
                                                  Text = GlobalResource(result.Message),
                                                  Icon = MessageModel.MessageIcon.Alert,
                                               };
                     RegisterMessage(message);
                  }
               }
               else
               {
                  MessageModel message = new MessageModel
                  {
                     Text = GlobalResource(invalidMessage),
                     Icon = MessageModel.MessageIcon.Alert,
                     CssClass = "margin-topbottom"
                  };
                  RegisterMessage(message);
               }

            }
         }
         catch (RulesException ex)
         {
            ex.AddModelStateErrors(ModelState, "user");
         }
         catch (Exception ex)
         {
            log.Error("UsersController.Update", ex);

            MessageModel message = new MessageModel
                                    {
                                       Text = GlobalResource("Message_GenericError"),
                                       Icon = MessageModel.MessageIcon.Alert,
                                    };
            RegisterMessage(message);
         }

         ViewData["Roles"] = userService.GetRolesBySite(Context.ManagedSite);
         ViewData["TimeZones"] = new SelectList(TimeZoneUtil.GetTimeZones(), "Key", "Value", user.TimeZone);
         ViewData["AdminThemes"] = new SelectList(GetAdminThemesList(), user.AdminTheme);
         ViewData["Cultures"] = new SelectList(Globalization.GetOrderedCultures(), "Name", "DisplayName", user.AdminCulture);

         return View("NewUser", user);
      }

      #endregion

      #region Private Helpers

      /// <summary>
      /// get a list of available admin themes by reading the folders in the ~/resources/css/themes
      /// </summary>
      /// <returns></returns>
      private IList<string> GetAdminThemesList()
      {
         IList<string> themes = new List<string>();
         NameValueCollection section = (NameValueCollection)ConfigurationManager.GetSection("telerik");
         string path = section["cssPath"] + "/themes/";

         DirectoryInfo di = new DirectoryInfo(this.ControllerContext.HttpContext.Server.MapPath(path));

         foreach (DirectoryInfo directoryInfo in di.GetDirectories())
         {
            // exclude source control special folder
            if (directoryInfo.Name.StartsWith(".svn"))
               continue;

            themes.Add(directoryInfo.Name);
         }

         return themes;
      }

      #endregion
   }
}