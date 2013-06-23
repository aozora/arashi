using System.Threading;
using System.Web;
using Arashi.Core;
using Arashi.Core.Domain;

namespace Arashi.Web.Components
{
   public class RequestContext : IRequestContext
   {
      private Site currentSite;
      private Site managedSite;
      private User currentUser;

      /// <summary>
      /// Gets the current IRequestContext.
      /// <remarks>
      /// This property is just for convenience. 
      /// Only use it from places where the context can't be injected via IoC.
      /// TODO: We need to do something about the IoC dependency here.
      /// </remarks>
      /// </summary>
      public static IRequestContext Current
      {
         get { return IoC.Resolve<IRequestContext>(); }
      }

      /// <summary>
      /// Creates an instance of the RequestContext class.
      /// </summary>
      public RequestContext()
      {
      }

      #region Implementation of IRequestContext

      /// <summary>
      /// The user for the current request.
      /// </summary>
      public User CurrentUser
      {
         get
         {
            return currentUser;
         }
      }



      /// <summary>
      /// The current site.
      /// </summary>
      public Site CurrentSite
      {
         get
         {
            return currentSite;
         }
      }



      /// <summary>
      /// The current managed site.
      /// </summary>
      public Site ManagedSite
      {
         get
         {
            return managedSite;
         }
      }



      /// <summary>
      /// Gets or sets the physical site data directory.
      /// </summary>
      public string CurrentSiteDataFolder {get; set;}



      /// <summary>
      /// Set the user for the current context.
      /// </summary>
      /// <param name="user"></param>
      public void SetUser(User user)
      {
         currentUser = user;
         HttpContext.Current.User = user;
         Thread.CurrentPrincipal = user;
      }



      /// <summary>
      /// Set the site for the current context.
      /// </summary>
      /// <param name="site"></param>
      public void SetCurrentSite(Site site)
      {
         currentSite = site;
      }



      /// <summary>
      /// Set the managed site for the current context.
      /// </summary>
      /// <param name="site"></param>
      public void SetManagedSite(Site site)
      {
         this.managedSite = site;
      }

      #endregion
   }
}