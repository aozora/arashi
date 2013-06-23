using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Security.Authentication;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using Arashi.Core.Domain;
using Arashi.Core.Exceptions;
using Arashi.Services.Membership;
using Arashi.Services.Notification;
using Arashi.Web.Areas.Admin.Models;
using Arashi.Web.Helpers;
using log4net;
using xVal.ServerSide;
using ApplicationException = Arashi.Core.Exceptions.ApplicationException;

namespace Arashi.Web.Areas.Admin.Controllers
{
   /// <summary>
   /// Manage the login to the Control Panel
   /// </summary>
   public class LoginController : Arashi.Web.Mvc.Controllers.ControllerBase
   {
      private readonly IAuthenticationService authenticationService;
      private readonly IMessageService messageService;
      private static readonly ILog log = LogManager.GetLogger(typeof(LoginController));


      /// <summary>
      /// Create and initialize an instance of the LoginController class.
      /// </summary>
      /// <param name="authenticationService"></param>
      public LoginController(IAuthenticationService authenticationService, IMessageService messageService)
      {
         this.authenticationService = authenticationService;
         this.messageService = messageService;
      }

      #region Login

      /// <summary>
      /// Show the login view
      /// </summary>
      /// <param name="returnUrl"></param>
      /// <returns></returns>
      public ActionResult Index(string returnUrl)
      {
         ViewData["ReturnUrl"] = returnUrl;

         return View(new LoginModel());
      }



      /// <summary>
      /// Validate the given credentials and if valid perform the logon and then
      /// redirect to the control panel home page.
      /// </summary>
      /// <param name="returnUrl"></param>
      /// <returns></returns>
      [AcceptVerbs(HttpVerbs.Post)]
      [ValidateAntiForgeryToken]
      public ActionResult Authenticate(string returnUrl)
      {
         var loginModel = new LoginModel();

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
         catch (RulesException ex)
         {
            ex.AddModelStateErrors(ModelState, "login");
            //return View();
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
         return RedirectToAction("Index", "Login");
      }



      /// <summary>
      /// Log out the current user
      /// </summary>
      /// <returns></returns>
      public ActionResult Logout()
      {
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
         catch (RulesException ex)
         {
            ex.AddModelStateErrors(ModelState, "login");
            return View("ResetPassword", resetPasswordModel);
         }
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