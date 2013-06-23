using System;
using Arashi.Core;
using Arashi.Core.Domain;
using Arashi.Services.SiteStructure;
using Arashi.Web.Helpers;
using Castle.Windsor;
using Common.Logging;

namespace Arashi.Web.Install
{
   public class InstallPageBase : System.Web.UI.Page
   {
      private IWindsorContainer container;
      private ILog log = LogManager.GetCurrentClassLogger();


      /// <summary>
      /// A reference to the Windsor container that can be used as a Service Locator for service classes.
      /// </summary>
      protected IWindsorContainer Container
      {
         get
         {
            return container;
         }
      }



      /// <summary>
      /// Constructor.
      /// </summary>
      public InstallPageBase()
      {
         container = IoC.Container;
      }



      protected override void OnInit(EventArgs e)
      {
         // Set the Arashi RequestContext
         IRequestContext requestContext = Container.Resolve<IRequestContext>();

         if (User.Identity.IsAuthenticated && this.User is User)
         {
            requestContext.SetUser((User)this.User);
         }

         //ISiteService siteService = Container.Resolve<ISiteService>();

         //try
         //{
         //   Site currentSite = siteService.GetSiteByHostName(WebHelper.GetHostName());
         //   requestContext.SetCurrentSite(currentSite);
         //   requestContext.CurrentSiteDataFolder = Server.MapPath(currentSite.SiteDataPath);
         //}
         //catch (Exception ex)
         //{
         //   log.Error("An unexpected error occured while setting the current site context.", ex);
         //}
         base.OnInit(e);
      }

   }
}
