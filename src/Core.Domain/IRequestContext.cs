namespace Arashi.Core.Domain
{
   public interface IRequestContext
   {
      /// <summary>
      /// The user for the current request.
      /// </summary>
      User CurrentUser
      {
         get;
      }

      /// <summary>
      /// The current site.
      /// </summary>
      Site CurrentSite
      {
         get;
      }

      /// <summary>
      /// The current managed site.
      /// </summary>
      Site ManagedSite
      {
         get;
      }

      /// <summary>
      /// Gets or sets the physical data folder for the current site.
      /// </summary>
      string CurrentSiteDataFolder
      {
         get;
         set;
      }


      /// <summary>
      /// Set the user for the current context.
      /// </summary>
      /// <param name="user"></param>
      void SetUser(User user);

      /// <summary>
      /// Set the site for the current context.
      /// </summary>
      /// <param name="site"></param>
      void SetCurrentSite(Site site);


      /// <summary>
      /// Set the managed site for the current context.
      /// </summary>
      /// <param name="site"></param>
      void SetManagedSite(Site site);

   }
}