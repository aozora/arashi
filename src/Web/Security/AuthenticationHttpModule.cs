using System;
using System.Threading;
using System.Web;
using System.Web.Security;
using Arashi.Core;
using Arashi.Services.Membership;
using Arashi.Web.Components;
using Common.Logging;
using Arashi.Core.Domain;

namespace Arashi.Web.Security
{
   /// <summary>
   /// HttpModule to extend Forms Authentication.
   /// </summary>
   public class AuthenticationHttpModule : IHttpModule
   {
      private const int AUTHENTICATION_TIMEOUT = 20;
      private ILog log = LogManager.GetCurrentClassLogger();
      private IUserService userService;



      public AuthenticationHttpModule()
      {
      }


      public void Init(HttpApplication context)
      {
         context.AuthenticateRequest += new EventHandler(Context_AuthenticateRequest);
         userService = IoC.Resolve<IUserService>();
      }



      private void Context_AuthenticateRequest(object sender, EventArgs e)
      {
         HttpApplication app = (HttpApplication)sender;

         if (app.Context.User != null && app.Context.User.Identity.IsAuthenticated)
         {
            // There is a logged-in user with a standard Forms Identity. 
            // Replace it with Arashi principal
            int userId = Int32.Parse(app.Context.User.Identity.Name);
            User user = userService.GetUserById(userId);
            user.IsAuthenticated = true;
            
            RequestContext.Current.SetUser(user);
            
            HttpContext.Current.User = user;
            Thread.CurrentPrincipal = user;
         }

      }


      public void Dispose()
      {
         // Nothing here	
      }



      /// <summary>
      /// Log out the current user.
      /// </summary>
      public void Logout()
      {
         if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
         {
            FormsAuthentication.SignOut();
         }
      }



   }
}