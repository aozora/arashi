using Facebook.Web;

namespace Arashi.Web.Areas.Admin.Controllers
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics.CodeAnalysis;
   using System.Security.Authentication;
   using System.Web.Mvc;
   using System.Web.Security;

   using Arashi.Core.Domain;
   using Arashi.Core.Exceptions;
   using Arashi.Core.Extensions;
   using Arashi.Services.Localization;
   using Arashi.Services.Membership;
   using Arashi.Services.Notification;
   using Arashi.Services.SiteStructure;
   using Arashi.Services.SystemService;
   using Arashi.Web.Areas.Admin.Models;
   using Arashi.Web.Helpers;

   using Common.Logging;

   using Facebook;

   using DotNetOpenAuth.Messaging;
   using DotNetOpenAuth.OpenId;
   using DotNetOpenAuth.OpenId.RelyingParty;



   /// <summary>
   /// Manage the login to the Control Panel
   /// </summary>
   public class LoginController : Arashi.Web.Mvc.Controllers.ControllerBase
   {
      private ILog log;
      private readonly IAuthenticationService authenticationService;
      private readonly ISystemConfigurationService systemConfigurationService;
      private readonly IMessageService messageService;
      private static OpenIdRelyingParty openid = new OpenIdRelyingParty();



      /// <summary>
      /// Create and initialize an instance of the LoginController class.
      /// </summary>
      /// <param name="log"></param>
      /// <param name="authenticationService"></param>
      /// <param name="systemConfigurationService"></param>
      /// <param name="messageService"></param>
      /// <param name="localizationService"></param>
      /// <param name="userService"></param>
      /// <param name="siteService"></param>
      public LoginController(ILog log, IAuthenticationService authenticationService, ISystemConfigurationService systemConfigurationService, 
                              IMessageService messageService, ILocalizationService localizationService, IUserService userService, ISiteService siteService)
         : base(log, localizationService, userService, siteService)
      {
         this.log = log;
         this.authenticationService = authenticationService;
         this.messageService = messageService;
         this.systemConfigurationService = systemConfigurationService;
      }

      #region Login

      #region OpenID Login

      /// <summary>
      /// Show the view for the OpenId login
      /// </summary>
      /// <param name="returnUrl"></param>
      /// <returns></returns>
      public ActionResult Index(string returnUrl)
      {
         log.Debug("LoginController.Index");
         SystemConfiguration configuration = systemConfigurationService.Get();

         if (configuration != null)
            ViewData["FacebookAppId"] = configuration.FacebookAppId.ToEmptyStringIfNull();

         ViewData["ReturnUrl"] = returnUrl;
         return View();
      }



      /// <summary>
      /// Submit of the OpenId login
      /// </summary>
      /// <param name="returnUrl"></param>
      /// <returns></returns>
      [AcceptVerbs(HttpVerbs.Post | HttpVerbs.Get)]
      [ValidateInput(false)]
      [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", Justification = "Needs to take same parameter type as Controller.Redirect()")]
      public ActionResult OpenIdAuthenticate(string returnUrl)
      {
         var response = openid.GetResponse();

         if (response == null)
         {
            // Stage 1: user submitting Identifier
            Identifier id;

            if (Identifier.TryParse(Request.Form["openid_identifier"], out id))
            {
               try
               {
                  return openid.CreateRequest(id)
                                 .RedirectingResponse
                                 .AsActionResult();
               }
               catch (ProtocolException ex)
               {
                  log.ErrorFormat("LoginController: error sending OpenID request for id \"{0}\".\r\n{1}", Request.Form["openid_identifier"], ex.ToString());
                  ViewData["AuthenticationFailed"] = "AuthenticationException failed!";
                  return View("Index");
               }
            }
            else
            {
               ViewData["AuthenticationFailed"] = "OpenID identifier invalid!";
               return View("Index");
            }
         }
         else
         {
            // Stage 2: OpenID Provider sending assertion response
            switch (response.Status)
            {
               case AuthenticationStatus.Authenticated:
                  //Session["FriendlyIdentifier"] = response.FriendlyIdentifierForDisplay;

                  try
                  {
                     string hostName = WebHelper.GetHostName();
                     Site currentSite = siteService.GetSiteByHostName(hostName);

                     // the current site may be null if the hostname is not yet assigned!!!
                     // (the hostname don't exists in the SiteHosts table
                     if (currentSite == null)
                     {
                        log.ErrorFormat("The hostname \"{0}\" is not assigned to any site. Please check the configuration!", hostName);
                        throw new SiteNullException(string.Format("The hostname \"{0}\" is not assigned to any site. Please check the configuration!", hostName));
                     }

                     //string userOpenId = response.FriendlyIdentifierForDisplay;
                     string userOpenId = response.ClaimedIdentifier;
                     User user = authenticationService.AuthenticateUser(currentSite, userOpenId, Request.UserHostAddress, true);

                     // if the user is found, then go to the landing page or the requested page (ReturnUrl)
                     if (user != null)
                     {
                        // see http://weblogs.asp.net/jgalloway/archive/2011/01/25/preventing-open-redirection-attacks-in-asp-net-mvc.aspx
                        if (!String.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                           return Redirect(returnUrl);

                        // Check if there are at leat 1 site, in that case redirect to that site home page
                        IList<Site> sites = siteService.GetAllSites();

                        log.DebugFormat("redirect to: {0}", Url.Action("Index", "Site", new { siteid = sites[0].SiteId }));
                        if (sites.Count > 0)
                           return RedirectToAction("Index", "Site", new { siteid = sites[0].SiteId });

                        // otherwise redirect to the "generic" home
                        return RedirectToAction("Index", "Home");
                        //return View("Index");
                     }
                  }
                  catch (Exception ex)
                  {
                     log.Error("Unexpected error while logging in", ex);
                     throw;
                  }
                  break;

               case AuthenticationStatus.Canceled:
                  ViewData["AuthenticationFailed"] = "Canceled at provider";
                  return View("Index");

               case AuthenticationStatus.Failed:
                  ViewData["AuthenticationFailed"] = response.Exception.Message;
                  return View("Index");
            }
         }

         ViewData["AuthenticationFailed"] = "Logon unsucessfull!";
         // return RedirectToAction("Index", "Login");
         return View("Index");
         //return new EmptyResult();
      }

      #endregion

      #region Facebook Connect

      [AcceptVerbs(HttpVerbs.Post)]
      [ValidateInput(false)]
      public ActionResult FacebookConnect(string returnUrl)
      {
         SystemConfiguration configuration = systemConfigurationService.Get();
         ViewData["FacebookAppId"] = configuration.FacebookAppId.ToEmptyStringIfNull();

         var oAuthClient = new FacebookOAuthClient();
         oAuthClient.AppId = configuration.FacebookAppId.ToEmptyStringIfNull();

         string redirectUrl = Url.Action("OAuth", "Login");

         oAuthClient.RedirectUri = new Uri(Request.Url.GetLeftPart(UriPartial.Authority) + redirectUrl);
         var loginUri = oAuthClient.GetLoginUrl(new Dictionary<string, object> { { "state", returnUrl } });
         return Redirect(loginUri.AbsoluteUri);
      }


      [HttpGet]
      public ActionResult OAuth(string code, string state, string returnUrl)
      {
         FacebookOAuthResult oauthResult;
         SystemConfiguration configuration = systemConfigurationService.Get();
         string redirectUrl = Url.Action("oauth", "Login");

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
                  string hostName = WebHelper.GetHostName();
                  Site currentSite = siteService.GetSiteByHostName(hostName);

                  // the current site may be null if the hostname is not yet assigned!!!
                  // (the hostname don't exists in the SiteHosts table
                  if (currentSite == null)
                  {
                     log.ErrorFormat("The hostname \"{0}\" is not assigned to any site. Please check the configuration!", hostName);
                     throw new SiteNullException(string.Format("The hostname \"{0}\" is not assigned to any site. Please check the configuration!", hostName));
                  }

                  User user = authenticationService.AuthenticateUser(currentSite, facebookId.ToString(), Request.UserHostAddress, true);

                  // if the user is found, then go to the landing page or the requested page (ReturnUrl)
                  if (user != null)
                  {
                     // see http://weblogs.asp.net/jgalloway/archive/2011/01/25/preventing-open-redirection-attacks-in-asp-net-mvc.aspx
                     if (!String.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);

                     // Check if there are at leat 1 site, in that case redirect to that site home page
                     IList<Site> sites = siteService.GetAllSites();

                     log.DebugFormat("redirect to: {0}", Url.Action("Index", "Site", new
                     {
                        siteid = sites[0].SiteId
                     }));
                     if (sites.Count > 0)
                        return RedirectToAction("Index", "Site", new
                        {
                           siteid = sites[0].SiteId
                        });

                     // otherwise redirect to the "generic" home
                     return RedirectToAction("Index", "Home");
                  }
               }
               catch (Exception ex)
               {
                  log.Error("Unexpected error while logging in", ex);
                  throw;
               }

               //InMemoryUserStore.Add(new FacebookUser
               //{
               //   AccessToken = accessToken,
               //   Expires = expiresOn,
               //   FacebookId = facebookId,
               //   Name = (string)me.name,
               //});

               //FormsAuthentication.SetAuthCookie(facebookId.ToString(), false);

               //// prevent open redirection attack by checking if the url is local.
               //if (Url.IsLocalUrl(state))
               //{
               //   return Redirect(state);
               //}
               //else
               //{
               //   return RedirectToAction("Index", "Home");
               //}

            }
            else
            {
               log.WarnFormat("LoginController.FacebookConnect - Facebook rejected authentication!");
               ViewData["AuthenticationFailed"] = "Logon unsucessfull!";
               return View("Index");
            }
         }

         ViewData["AuthenticationFailed"] = "Logon unsucessfull!";
         return View("Index");
      }


      ///// <summary>
      ///// Login with Facebook Connect
      ///// </summary>
      ///// <param name="returnUrl"></param>
      ///// <returns></returns>
      //[AcceptVerbs(HttpVerbs.Post)]
      //[ValidateInput(false)]
      //public ActionResult FacebookConnect(string returnUrl)
      //{
      //   SystemConfiguration configuration = systemConfigurationService.Get();
      //   ViewData["FacebookAppId"] = configuration.FacebookAppId.ToEmptyStringIfNull();

      //   //FacebookApp app = new FacebookApp(new FacebookSettings()
      //   //    {
      //   //       AppId = configuration.FacebookAppId.ToEmptyStringIfNull(),
      //   //       ApiKey = configuration.FacebookApiKey,
      //   //       ApiSecret = configuration.FacebookApiSecret,
      //   //       CookieSupport = configuration.FacebookCookieSupport
      //    //});

      //   //FacebookWebContext fbWebContext = FacebookWebContext.Current;
      //   ////var auth = new Facebook.Web.FacebookWebAuthorizer(Settings.FBAppId, Settings.FBConnectAPISecret, (System.Web.HttpContextBase)httpContext);
      //   //FacebookWebClient app = new FacebookWebClient(configuration.FacebookAppId.ToEmptyStringIfNull(),
      //   //                                              configuration.FacebookApiSecret);

      //   // if the user is already logged
      //   if (app.Session != null)
      //   {
      //      long facebookUserId = app.Session.UserId;

      //      try
      //      {
      //         string hostName = WebHelper.GetHostName();
      //         Site currentSite = siteService.GetSiteByHostName(hostName);

      //         // the current site may be null if the hostname is not yet assigned!!!
      //         // (the hostname don't exists in the SiteHosts table
      //         if (currentSite == null)
      //         {
      //            log.ErrorFormat("The hostname \"{0}\" is not assigned to any site. Please check the configuration!", hostName);
      //            throw new SiteNullException(string.Format("The hostname \"{0}\" is not assigned to any site. Please check the configuration!", hostName));
      //         }

      //         User user = authenticationService.AuthenticateUser(currentSite, facebookUserId.ToString(), Request.UserHostAddress, true);

      //         // if the user is found, then go to the landing page or the requested page (ReturnUrl)
      //         if (user != null)
      //         {
      //            // see http://weblogs.asp.net/jgalloway/archive/2011/01/25/preventing-open-redirection-attacks-in-asp-net-mvc.aspx
      //            if (!String.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
      //               return Redirect(returnUrl);

      //            // Check if there are at leat 1 site, in that case redirect to that site home page
      //            IList<Site> sites = siteService.GetAllSites();

      //            log.DebugFormat("redirect to: {0}", Url.Action("Index", "Site", new { siteid = sites[0].SiteId }));
      //            if (sites.Count > 0)
      //               return RedirectToAction("Index", "Site", new { siteid = sites[0].SiteId });

      //            // otherwise redirect to the "generic" home
      //            return RedirectToAction("Index", "Home");
      //         }
      //      }
      //      catch (Exception ex)
      //      {
      //         log.Error("Unexpected error while logging in", ex);
      //         throw;
      //      }
      //   }
      //   else // not logged in
      //   {
      //      log.WarnFormat("LoginController.FacebookConnect - Facebook rejected authentication!");
      //      ViewData["AuthenticationFailed"] = "Logon unsucessfull!";
      //      return View("Index");
      //   }


      //   ViewData["AuthenticationFailed"] = "Logon unsucessfull!";
      //   return View("Index");
      //}

      #endregion

      #region Classic Login with username/password

      /// <summary>
      /// Show the classic login form with username & password
      /// </summary>
      /// <param name="returnUrl"></param>
      /// <returns></returns>
      public ActionResult IndexClassic(string returnUrl)
      {
         ViewData["ReturnUrl"] = returnUrl;

         return View("IndexClassic", new ClassicLoginModel());
      }



      /// <summary>
      /// Validate the given credentials and if valid perform the logon and then
      /// redirect to the control panel home page.
      /// </summary>
      /// <param name="returnUrl"></param>
      /// <returns></returns>
      [AcceptVerbs(HttpVerbs.Post)]
      [ValidateAntiForgeryToken(Salt = "loginclassic")]
      public ActionResult Authenticate(string returnUrl)
      {
         var loginModel = new ClassicLoginModel();

         try
         {
            string hostName = WebHelper.GetHostName();
            Site currentSite = siteService.GetSiteByHostName(hostName);

            // the current site may be null if the hostname is not yet assigned!!!
            // (the hostname don't exists in the SiteHosts table
            if (currentSite == null)
            {
               log.ErrorFormat("The hostname \"{0}\" is not assigned to any site. Please check the configuration!", hostName);
               throw new SiteNullException(string.Format("The hostname \"{0}\" is not assigned to any site. Please check the configuration!", hostName));
            }

            if (ModelState.IsValid && TryUpdateModel(loginModel))
            {
               User user = authenticationService.AuthenticateUser(currentSite, loginModel.Email, loginModel.Password, Request.UserHostAddress, loginModel.RememberMe);

               // if the user is found, then go to the landing page or the requested page (ReturnUrl)
               if (user != null)
               {
                  if (!String.IsNullOrEmpty(returnUrl))
                     return Redirect(returnUrl);

                  // Check if there are at leat 1 site, in that case redirect to that site home page
                  IList<Site> sites = siteService.GetAllSites();

                  log.DebugFormat("redirect to: {0}", Url.Action("Index", "Site", new {siteid = sites[0].SiteId}));
                  if (sites.Count > 0)
                     return RedirectToAction("Index", "Site", new {siteid = sites[0].SiteId});

                  // otherwise redirect to the "generic" home
                  return RedirectToAction("Index", "Home");
               }

            }
         }
         catch (AuthenticationException)
         {
            log.WarnFormat("User {0} unsuccesfully logged in with password {1}.", loginModel.Email, loginModel.Password);
            throw;
         }
         catch (Exception ex)
         {
            log.Error("Unexpected error while logging in", ex);
            throw;
         }

         ViewData["AuthenticationFailed"] = "Logon unsucessfull!";
         return RedirectToAction("IndexClassic", "Login");
      }

      #endregion


      /// <summary>
      /// Log out the current user
      /// </summary>
      /// <returns></returns>
      public ActionResult Logout()
      {
         //FormsAuthentication.SignOut();
         //var oAuthClient = new FacebookOAuthClient();
         //oAuthClient.RedirectUri = new Uri(logoffUrl);
         //var logoutUrl = oAuthClient.GetLogoutUrl();
         //return Redirect(logoutUrl.AbsoluteUri);
         FormsAuthentication.SignOut();
         return RedirectToAction("Index", "Login", new {});
      }

      #endregion

      #region Reset Password

      /// <summary>
      /// Show the view for the password reset
      /// </summary>
      /// <param name="returnUrl"></param>
      /// <returns></returns>
      public ActionResult ResetPassword(string returnUrl)
      {
         ViewData["ReturnUrl"] = returnUrl;

         return View(new ResetPasswordModel());
      }


      
      /// <summary>
      /// Generate a new password and email it
      /// </summary>
      /// <param name="returnUrl"></param>
      /// <returns></returns>
      [AcceptVerbs("POST")]
      [ValidateAntiForgeryToken]
      public ActionResult GenerateAndSendNewPassword(string returnUrl)
      {
         var resetPasswordModel = new ResetPasswordModel();

         try
         {
            string hostName = WebHelper.GetHostName();
            Site currentSite = siteService.GetSiteByHostName(hostName);

            if (ModelState.IsValid && TryUpdateModel(resetPasswordModel))
            {
               User user = userService.FindUserByEmail(currentSite, resetPasswordModel.Email);

               // if the user is found, then go to the landing page or the requested page (ReturnUrl)
               if (user != null)
               {
                  string newPassword = authenticationService.ResetPassword(user);

                  string message = string.Format("Hi, your new password is:\r\n{0}\r\n\r\n- Arashi Team", newPassword);
                  //MailMessage mail = EmailService.CreateMailMessage(Context.CurrentSite.Email, new string[] { Context.CurrentSite.Email }, null, null, "Arashi - Password reset", message, Encoding.UTF8, false);

                  try
                  {
                     //EmailService emailService = new EmailService();
                     //emailService.Send(mail);
                     Message contactMessage = new Message
                     {
                        Site = Context.CurrentSite,
                        To = Context.CurrentSite.Email,
                        From = Context.CurrentSite.Email,
                        Subject = "Arashi - Password reset",
                        Body = message,
                        Type = MessageType.Email
                     };

                     messageService.Save(contactMessage);

                     ViewData["ResetPasswordSuccessfull"] = "The new password will arrive to your mailbox in a few minutes!";
                  }
                  catch (Exception ex)
                  {
                     log.Error(ex.ToString());
                     ViewData["ResetPasswordFailed"] = "Sorry, an error occurred sending the email...";
                  }

                  return View("ResetPassword", resetPasswordModel);
               }

            }
         }
         //catch (RulesException ex)
         //{
         //   ex.AddModelStateErrors(ModelState, "login");
         //   return View("ResetPassword", resetPasswordModel);
         //}
         catch (Exception ex)
         {
            log.Error(ex.ToString());
            throw;
         }

         ViewData["ResetPasswordFailed"] = "The email address is incorrect or don't belong to a user of the system.";
         return View("ResetPassword", resetPasswordModel);
      }


      #endregion

   }
}