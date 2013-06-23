namespace Arashi.Web.Areas.Admin.Controllers
{
   using System;
   using System.Collections.Generic;
   using System.Collections.Specialized;
   using System.Configuration;
   using System.Diagnostics.CodeAnalysis;
   using System.IO;
   using System.Linq;
   using System.Text;
   using System.Web.Mvc;
   using Arashi.Core.Domain;
   using Arashi.Core.Domain.Extensions;
   using Arashi.Core.Exceptions;
   using Arashi.Core.Extensions;
   using Arashi.Core.Util;
   using Arashi.Services;
   using Arashi.Services.Localization;
   using Arashi.Services.Membership;
   using Arashi.Services.SiteStructure;
   using Arashi.Services.SystemService;
   using Arashi.Web.Areas.Admin.Models;
   using Arashi.Web.Helpers;
   using Arashi.Web.Mvc.Controllers;
   using Arashi.Web.Mvc.Filters;
   using Arashi.Web.Mvc.Models;
   using Arashi.Web.Mvc.Paging;

   using DotNetOpenAuth.Messaging;
   using DotNetOpenAuth.OpenId;
   using DotNetOpenAuth.OpenId.RelyingParty;

   using Facebook;

   using Common.Logging;

   using uNhAddIns.Pagination;


   public class UsersController : SecureControllerBase
   {
      private ILog log;
      private const int pageSize = 20;
      private readonly ISystemConfigurationService systemConfigurationService;
      private static OpenIdRelyingParty openid = new OpenIdRelyingParty();


      #region Constructor

      public UsersController(ILog log, ISystemConfigurationService systemConfigurationService, ILocalizationService localizationService, IUserService userService, ISiteService siteService)
         : base(log, localizationService, userService, siteService)
      {
         this.log = log;
         this.systemConfigurationService = systemConfigurationService;
      }

      #endregion

      #region Users List

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

      #endregion

      #region Edit User

      /// <summary>
      /// Show the user edit view
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
      [PermissionFilter(RequiredRights = Rights.UsersView)]
      public ActionResult Details(int id)
      {
         SystemConfiguration configuration = systemConfigurationService.Get();

         if (configuration != null)
            ViewData["FacebookAppId"] = configuration.FacebookAppId.ToEmptyStringIfNull();
         
         User user = userService.GetUserById(id);

         UserModel model = new UserModel
                              {
                                 UserId = user.UserId,
                                 ExternalId = user.ExternalId,
                                 ExternalProviderUri = user.ExternalProviderUri,
                                 DisplayName = user.DisplayName,
                                 FirstName = user.FirstName,
                                 LastName = user.LastName,
                                 Description = user.Description,
                                 Email = user.Email,
                                 TimeZone = user.TimeZone,
                                 WebSite = user.WebSite,
                                 IsActive = user.IsActive,
                                 LastLogin = user.LastLogin,
                                 LastIp = user.LastIp,
                                 AdminTheme = user.AdminTheme,
                                 AdminCulture = user.AdminCulture,
                                 CreatedDate = user.CreatedDate,
                                 UpdatedDate = user.UpdatedDate,
                                 Roles = user.Roles,
                                 AllRoles = userService.GetRolesBySite(Context.ManagedSite),
                                 TimeZones = new SelectList(TimeZoneUtil.GetTimeZones(), "Key", "Value", user.TimeZone),
                                 AdminThemes = new SelectList(GetAdminThemesList(), user.AdminTheme),
                                 Cultures = new SelectList(Globalization.GetOrderedCultures(), "Name", "DisplayName", user.AdminCulture)
                              };

         return View("Details", model);
      }



      /// <summary>
      /// Save the user details
      /// </summary>
      /// <param name="userModel"></param>
      /// <param name="id"></param>
      /// <param name="roleIds"></param>
      /// <returns></returns>
      //[ValidateInput(false)]
      [AcceptVerbs(HttpVerbs.Post)]
      [PermissionFilter(RequiredRights = Rights.UsersEdit)]
      [ValidateAntiForgeryToken(Salt = "update")]
      public ActionResult Update(UserModel userModel, int id, int[] roleIds)
      {
         User user = userService.GetUserById(id);

         try
         {
            string invalidMessage = string.Empty;

            // Unique Email
            if (userService.CountOtherUsersBySiteAndEmail(Context.ManagedSite, userModel.Email, user) > 0)
            {
               invalidMessage = "Message_UserEmailIsNotUnique";
            }

            // Unique DisplayName Validation
            if (userService.CountOtherUsersBySiteAndDisplayName(Context.ManagedSite, user.DisplayName, user) > 0)
            {
               invalidMessage = "Message_UserDisplayNameIsNotUnique";
            }

            // try to update the User entity with the info from the model
            TryUpdateModel<User>(user, new[] { "DisplayName", "ExternalId", "FirstName", "LastName", "Email", "Website", "IsActive", "TimeZone", "Description", "AdminTheme", "AdminCulture" });

            if (ModelState.IsValid)
            {
               // Clear existing roles
               user.Roles.Clear();

               // update the roles
               if (roleIds != null && roleIds.Length > 0)
               {
                  IList<Role> roles = userService.GetRolesByIds(roleIds);
                  foreach (Role role in roles)
                  {
                     user.Roles.Add(role);
                  }
               }


               if (invalidMessage.Length == 0)
               {
                  user.UpdatedDate = DateTime.UtcNow;
                  ServiceResult result = userService.SaveUser(user);

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
            else
            {
               StringBuilder errorMessage = new StringBuilder();

               if (!ModelState.IsValid)
               {
                  var errorList = (from item in ModelState
                                   where item.Value.Errors.Any()
                                   select item.Value.Errors[0].ErrorMessage).ToList();
                  errorList.Each().Do(error => errorMessage.AppendFormat("{0}<br/>", error));
               }

               log.Debug("ChangePassword[POST] - Error validating the model");
               //log.Debug(errorMessage.ToString());

               MessageModel message = new MessageModel
               {
                  Text = GlobalResource("Message_GenericError") + "</br>" + errorMessage.ToString(),
                  Icon = MessageModel.MessageIcon.Alert,
                  CssClass = "margin-topbottom"
               };
               RegisterMessage(message);
            }
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

         userModel.Roles = user.Roles;
         userModel.AllRoles = userService.GetRolesBySite(Context.ManagedSite);
         userModel.TimeZones = new SelectList(TimeZoneUtil.GetTimeZones(), "Key", "Value", user.TimeZone);
         userModel.AdminThemes = new SelectList(GetAdminThemesList(), user.AdminTheme);
         userModel.Cultures = new SelectList(Globalization.GetOrderedCultures(), "Name", "DisplayName", user.AdminCulture);

         return View("Details", userModel);
      }



      /// <summary>
      /// Check if the given email address is already in user by other users
      /// </summary>
      /// <param name="email"></param>
      /// <returns></returns>
      [AcceptVerbs(HttpVerbs.Post)]
      [PermissionFilter(RequiredRights = Rights.UsersEdit)]
      public ActionResult CheckIfEmailIsAlreadyInUse(int UserId, string Email)
      {
         bool isValid = false;
         User user = userService.GetUserById(UserId);

         isValid = !(userService.CountOtherUsersBySiteAndEmail(Context.ManagedSite, Email, user) > 0);

         return Json(isValid, JsonRequestBehavior.AllowGet);
      }



      /// <summary>
      /// Remove the externalid
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
      [AcceptVerbs(HttpVerbs.Get)]
      [PermissionFilter(RequiredRights = Rights.UsersEdit)]
      public ActionResult RemoveExternalId(int id)
      {
         User user = userService.GetUserById(id);

         try
         {
            user.ExternalId = null;
            user.ExternalProviderUri = null;

            userService.SaveUser(user);

            //MessageModel message = new MessageModel
            //{
            //   Text = GlobalResource("Message_ExternalIdRemoved"),
            //   Icon = MessageModel.MessageIcon.Info,
            //};
            //RegisterMessage(message, true);

            return RedirectToAction("Details", new { id = user.UserId });
         }
         catch (Exception ex)
         {
            log.Error("UsersController.RemoveExternalId", ex);

            MessageModel message = new MessageModel
            {
               Text = GlobalResource("Message_GenericError"),
               Icon = MessageModel.MessageIcon.Alert,
            };
            RegisterMessage(message, true);
         }

         return RedirectToAction("Details", new { id = user.UserId });
      }


      #region External Provider registration

      /// <summary>
      /// Authenticate with an OpenID provider and associate the current user with it
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
      [AcceptVerbs(HttpVerbs.Post | HttpVerbs.Get)]
      //[ValidateInput(false)]
      [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", Justification = "Needs to take same parameter type as Controller.Redirect()")]
      public ActionResult OpenIdConnect(int id)
      {
         User user = userService.GetUserById(id);
         var response = openid.GetResponse();

         if (response == null)
         {
            // Stage 1: user submitting Identifier
            Identifier openIdIdentifier;

            if (Identifier.TryParse(Request.Form["openid_identifier"], out openIdIdentifier))
            {
               try
               {
                  return openid.CreateRequest(openIdIdentifier)
                                 .RedirectingResponse
                                 .AsActionResult();
               }
               catch (ProtocolException ex)
               {
                  log.ErrorFormat("UsersController: error sending OpenID request for id \"{0}\".\r\n{1}", Request.Form["openid_identifier"], ex.ToString());
                  //ViewData["AuthenticationFailed"] = "AuthenticationException failed!";
                  MessageModel message = new MessageModel
                  {
                     Text = "AuthenticationException failed!",
                     Icon = MessageModel.MessageIcon.Alert,
                  };
                  RegisterMessage(message, true);                  
               }
            }
            else
            {
               ViewData["AuthenticationFailed"] = "OpenID identifier invalid!";
               MessageModel message = new MessageModel
               {
                  Text = "OpenID identifier invalid!",
                  Icon = MessageModel.MessageIcon.Alert,
               };
               RegisterMessage(message, true);
            }
         }
         else
         {
            // Stage 2: OpenID Provider sending assertion response
            switch (response.Status)
            {
               case AuthenticationStatus.Authenticated:

                  try
                  {
                     //string userOpenId = response.FriendlyIdentifierForDisplay;
                     string userOpenId = response.ClaimedIdentifier;

                     user.ExternalId = userOpenId;
                     user.ExternalProviderUri = response.Provider.Uri.Authority;
                     user.UpdatedDate = DateTime.UtcNow;

                     userService.SaveUser(user);

                     MessageModel successMessage = new MessageModel
                     {
                        Text = string.Format("The user {0} is now associated with the selected provider!", user.DisplayName),
                        Icon = MessageModel.MessageIcon.Info,
                     };
                     RegisterMessage(successMessage, true);
                  }
                  catch (Exception ex)
                  {
                     log.Error("Unexpected error while logging in", ex);
                     MessageModel errorMessage = new MessageModel
                     {
                        Text = GlobalResource("Message_GenericError"),
                        Icon = MessageModel.MessageIcon.Alert,
                     };
                     RegisterMessage(errorMessage, true);
                  }
                  break;

               case AuthenticationStatus.Canceled:
                  MessageModel authCanceledMessage = new MessageModel
                  {
                     Text = "Canceled at provider",
                     Icon = MessageModel.MessageIcon.Alert,
                  };
                  RegisterMessage(authCanceledMessage, true);
                  break;

               case AuthenticationStatus.Failed:
                  MessageModel authFailedMessage = new MessageModel
                  {
                     Text = response.Exception.Message,
                     Icon = MessageModel.MessageIcon.Alert,
                  };
                  RegisterMessage(authFailedMessage, true);
                  break;
            }
         }

         return RedirectToAction("Details", new { id = user.UserId });
      }


      [AcceptVerbs(HttpVerbs.Post)]
      [ValidateInput(false)]
      public ActionResult FacebookConnect(int id)
      {
         User user = userService.GetUserById(id);
         SystemConfiguration configuration = systemConfigurationService.Get();
         ViewData["FacebookAppId"] = configuration.FacebookAppId.ToEmptyStringIfNull();

         var oAuthClient = new FacebookOAuthClient();
         oAuthClient.AppId = configuration.FacebookAppId.ToEmptyStringIfNull();

         string redirectUrl = Url.Action("OAuth", "Users", new { id = user.UserId });

         oAuthClient.RedirectUri = new Uri(Request.Url.GetLeftPart(UriPartial.Authority) + redirectUrl);
         //var loginUri = oAuthClient.GetLoginUrl(new Dictionary<string, object> { { "state", returnUrl } });
         var loginUri = oAuthClient.GetLoginUrl();
         return Redirect(loginUri.AbsoluteUri);
      }


      //[HttpPost]
      public ActionResult OAuth(int id, string code, string state, string returnUrl)
      {
         FacebookOAuthResult oauthResult;
         SystemConfiguration configuration = systemConfigurationService.Get();
         string redirectUrl = Url.Action("OAuth", "Users");
         User user = userService.GetUserById(id);

         if (FacebookOAuthResult.TryParse(Request.Url, out oauthResult))
         {
            if (oauthResult.IsSuccess)
            {
               var oAuthClient = new FacebookOAuthClient();
               oAuthClient.AppId = configuration.FacebookAppId.ToEmptyStringIfNull();
               oAuthClient.AppSecret = configuration.FacebookApiSecret;
               oAuthClient.RedirectUri = new Uri(Request.Url.GetLeftPart(UriPartial.Authority) + redirectUrl);
               dynamic tokenResult = oAuthClient.ExchangeCodeForAccessToken(code);
               string accessToken = tokenResult.access_token;

               DateTime expiresOn = DateTime.MaxValue;

               if (tokenResult.ContainsKey("expires"))
                  DateTimeConvertor.FromUnixTime(tokenResult.expires);

               FacebookClient fbClient = new FacebookClient(accessToken);
               dynamic me = fbClient.Get("me?fields=id,name");
               long facebookId = Convert.ToInt64(me.id);

               try
               {
                  user.ExternalId = facebookId.ToString();
                  user.ExternalProviderUri = "www.facebook.com"; //app.GetLoginUrl().Authority;
                  user.UpdatedDate = DateTime.UtcNow;

                  userService.SaveUser(user);

                  //MessageModel successMessage = new MessageModel
                  //{
                  //   Text = string.Format("The user {0} is now associated with the selected Facebook account!", user.DisplayName),
                  //   Icon = MessageModel.MessageIcon.Info,
                  //};
                  //RegisterMessage(successMessage, true);
               }
               catch (Exception ex)
               {
                  log.Error("Unexpected error while logging in", ex);
                  MessageModel authFailedMessage = new MessageModel
                  {
                     Text = GlobalResource("Message_GenericError"),
                     Icon = MessageModel.MessageIcon.Alert,
                  };
                  RegisterMessage(authFailedMessage, true);
               }
            }
            else
            {
               log.WarnFormat("LoginController.FacebookConnect - Facebook rejected authentication!");
               MessageModel authFailedMessage = new MessageModel
               {
                  Text = "Sorry, Facebook rejected the authentication!",
                  Icon = MessageModel.MessageIcon.Alert,
               };
               RegisterMessage(authFailedMessage, true);
            }
         }

         //ViewData["AuthenticationFailed"] = "Logon unsucessfull!";
         return RedirectToAction("Details", new {id = user.UserId});
      }


      ///// <summary>
      ///// Authenticate with facebook Connect and associate the current user with it
      ///// </summary>
      ///// <param name="id"></param>
      ///// <returns></returns>
      //[AcceptVerbs(HttpVerbs.Post)]
      ////[ValidateInput(false)]
      //public ActionResult FacebookConnect(int id)
      //{
      //   User user = userService.GetUserById(id);

      //   SystemConfiguration configuration = systemConfigurationService.Get();
      //   ViewData["FacebookAppId"] = configuration.FacebookAppId.ToEmptyStringIfNull();

      //   FacebookApp app = new FacebookApp(new FacebookSettings()
      //   {
      //      AppId = configuration.FacebookAppId,
      //      ApiKey = configuration.FacebookApiKey,
      //      ApiSecret = configuration.FacebookApiSecret,
      //      CookieSupport = configuration.FacebookCookieSupport
      //   });

      //   // if the user is already logged
      //   if (app.Session != null)
      //   {
      //      long facebookUserId = app.Session.UserId;

      //      try
      //      {
      //         user.ExternalId = facebookUserId.ToString();
      //         user.ExternalProviderUri = app.GetLoginUrl().Authority;
      //         user.UpdatedDate = DateTime.UtcNow;

      //         userService.SaveUser(user);

      //         MessageModel successMessage = new MessageModel
      //         {
      //            Text = string.Format("The user {0} is now associated with the selected Facebook account!", user.DisplayName),
      //            Icon = MessageModel.MessageIcon.Info,
      //         };
      //         RegisterMessage(successMessage, true);
      //      }
      //      catch (Exception ex)
      //      {
      //         log.Error("Unexpected error while logging in", ex);
      //         MessageModel authFailedMessage = new MessageModel
      //         {
      //            Text = GlobalResource("Message_GenericError"),
      //            Icon = MessageModel.MessageIcon.Alert,
      //         };
      //         RegisterMessage(authFailedMessage, true);
      //      }
      //   }
      //   else // not logged in
      //   {
      //      log.WarnFormat("LoginController.FacebookConnect - Facebook rejected authentication!");
      //      MessageModel authFailedMessage = new MessageModel
      //      {
      //         Text = "Sorry, Facebook rejected the authentication!",
      //         Icon = MessageModel.MessageIcon.Alert,
      //      };
      //      RegisterMessage(authFailedMessage, true);
      //   }

      //   return RedirectToAction("Details", new
      //   {
      //      id = user.UserId
      //   });
      //}

      #endregion

      #endregion

      #region Change Password

      /// <summary>
      /// Show the Change Password view
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
      [AcceptVerbs(HttpVerbs.Get)]
      [PermissionFilter(RequiredRights = Rights.UsersEdit)]
      public ActionResult ChangePassword(int id)
      {
         User user = userService.GetUserById(id);

         ChangePasswordModel model = new ChangePasswordModel()
         {
            User = user
         };

         return this.View("ChangePassword", model);
      }



      /// <summary>
      /// Save the new password
      /// </summary>
      /// <param name="id"></param>
      /// <param name="changePasswordModel"></param>
      /// <returns></returns>
      [AcceptVerbs(HttpVerbs.Post)]
      [ValidateAntiForgeryToken(Salt = "changepassword")]
      [PermissionFilter(RequiredRights = Rights.UsersEdit)]
      public ActionResult ChangePassword(int id, ChangePasswordModel changePasswordModel)
      {
         User user = userService.GetUserById(id);

         changePasswordModel.User = user;

         try
         {
            if (ModelState.IsValid && TryUpdateModel(changePasswordModel, new[] { "Password", "PasswordConfirmation" }))
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

               return RedirectToAction("Details", new {id = user.UserId});
            }
            else
            {
               StringBuilder errorMessage = new StringBuilder();

               if (!ModelState.IsValid)
               {
                  var errorList = (from item in ModelState
                                   where item.Value.Errors.Any()
                                   select item.Value.Errors[0].ErrorMessage).ToList();
                  errorList.Each().Do(error => errorMessage.AppendFormat("{0}<br/>", error));
               }

               log.Debug("ChangePassword[POST] - Error validating the model");
               log.Debug(errorMessage.ToString());
               MessageModel message = new MessageModel
               {
                  Text = GlobalResource("Message_GenericError") + "</br>" + errorMessage.ToString(),
                  Icon = MessageModel.MessageIcon.Alert,
                  CssClass = "margin-topbottom"
               };
               RegisterMessage(message);
            }
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

         return View("ChangePassword", changePasswordModel);
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

         NewUserModel model = new NewUserModel
         {
            UserId = user.UserId,
            ExternalId = user.ExternalId,
            ExternalProviderUri = user.ExternalProviderUri,
            DisplayName = user.DisplayName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Description = user.Description,
            Email = user.Email,
            TimeZone = user.TimeZone,
            WebSite = user.WebSite,
            IsActive = user.IsActive,
            LastLogin = user.LastLogin,
            LastIp = user.LastIp,
            AdminTheme = user.AdminTheme,
            AdminCulture = user.AdminCulture,
            CreatedDate = user.CreatedDate,
            UpdatedDate = user.UpdatedDate,
            Roles = user.Roles,
            AllRoles = userService.GetRolesBySite(Context.ManagedSite),
            TimeZones = new SelectList(TimeZoneUtil.GetTimeZones(), "Key", "Value", user.TimeZone),
            AdminThemes = new SelectList(GetAdminThemesList(), user.AdminTheme),
            Cultures = new SelectList(Globalization.GetOrderedCultures(), "Name", "DisplayName", user.AdminCulture)
         };

         return View("NewUser", model);
      }



      /// <summary>
      /// Create a new user
      /// </summary>
      /// <param name="userModel"></param>
      /// <param name="roleIds">List of selected role id. It is null if no role is selected.</param>
      /// <returns></returns>
      //[ValidateInput(false)]
      [AcceptVerbs(HttpVerbs.Post)]
      [PermissionFilter(RequiredRights = Rights.UsersEdit)]
      [ValidateAntiForgeryToken]
      public ActionResult Create(NewUserModel userModel, int[] roleIds)
      {
         User user = new User();

         try
         {
            if (ModelState.IsValid)
            {
               TryUpdateModel(userModel, new[] { "DisplayName", "Email", "Password", "PasswordConfirmation", "IsActive", "TimeZone", "AdminCulture" });

               // server validation
               string invalidMessage = string.Empty;

               // Unique Email
               if (userService.CountOtherUsersBySiteAndEmail(Context.ManagedSite, userModel.Email, user) > 0)
               {
                  invalidMessage = "Message_UserEmailIsNotUnique";
               }

               // Unique DisplayName Validation
               if (userService.CountOtherUsersBySiteAndDisplayName(Context.ManagedSite, userModel.DisplayName, user) > 0)
               {
                  invalidMessage = "Message_UserDisplayNameIsNotUnique";
               }


               if (invalidMessage.Length == 0)
               {
                  user.Password = user.HashPassword(userModel.Password);
                  user.PasswordConfirmation = user.HashPassword(userModel.PasswordConfirmation);

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
                                                                   userModel.DisplayName),
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

         userModel.Roles = user.Roles;
         userModel.AllRoles = userService.GetRolesBySite(Context.ManagedSite);
         userModel.TimeZones = new SelectList(TimeZoneUtil.GetTimeZones(), "Key", "Value", user.TimeZone);
         userModel.AdminThemes = new SelectList(GetAdminThemesList(), user.AdminTheme);
         userModel.Cultures = new SelectList(Globalization.GetOrderedCultures(), "Name", "DisplayName", user.AdminCulture);

         return View("NewUser", userModel);
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
         //NameValueCollection section = (NameValueCollection)ConfigurationManager.GetSection("telerik");
         //string path = section["cssPath"] + "/themes/";

         //DirectoryInfo di = new DirectoryInfo(this.ControllerContext.HttpContext.Server.MapPath(path));

         //foreach (DirectoryInfo directoryInfo in di.GetDirectories())
         //{
         //   // exclude source control special folder
         //   if (directoryInfo.Name.StartsWith(".svn"))
         //      continue;

         //   themes.Add(directoryInfo.Name);
         //}
         themes.Add("Arashi");

         return themes;
      }

      #endregion
   }
}