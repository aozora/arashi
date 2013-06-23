using System;
using System.Collections;
using Arashi.Core.Domain;
using System.Collections.Generic;

namespace Arashi.Services.SiteStructure
{
   /// <summary>
   /// Provides functionality to manage site instances.
   /// </summary>
   public interface ISiteService
   {
      #region Gestione Site

      /// <summary>
      /// Get a single site by site id.
      /// </summary>
      /// <param name="siteId"></param>
      /// <returns></returns>
      Site GetSiteById(int siteId);


      /// <summary>
      /// Get all sites.
      /// </summary>
      /// <returns></returns>
      IList<Site> GetAllSites();

      /// <summary>
      /// Save a site.
      /// </summary>
      /// <param name="site"></param>
      void SaveSite(Site site);

      /// <summary>
      /// Delete a site.
      /// </summary>
      /// <param name="site"></param>
      void DeleteSite(Site site);



      void StoreTrackingInfo(TrackingInfo trackingInfo);


      IList<TrackingInfo> GetTrackingInfoForUsersOnline(int minutesSinceLastInactive);

      #endregion

      #region Templates

      Theme GetThemeById(int templateId);

      /// <summary>
      /// Get all the available templates registered in the system
      /// </summary>
      /// <returns></returns>
      IList<Theme> GetThemes();
      
      #endregion

      #region SiteHosts

      /// <summary>
      /// Get the default sitehost for a given site
      /// </summary>
      /// <param name="site"></param>
      /// <returns></returns>
      SiteHost GetDefaultSiteHost(Site site);


      /// <summary>
      /// Get a site by a host name
      /// </summary>
      /// <param name="host"></param>
      /// <returns></returns>
      Site GetSiteByHostName(string host);


      ///// <summary>
      ///// Get all sitehost of the given site.
      ///// </summary>
      ///// <param name="site"></param>
      ///// <returns></returns>
      IList<SiteHost> GetSiteHostsBySite(Site site);

      /// <summary>
      /// Get a single SiteHost given his host name
      /// </summary>
      /// <param name="host"></param>
      /// <returns></returns>
      SiteHost GetSiteHostByHostName(string host);

      SiteHost GetSiteHostById(int siteHostId);

      ///// <summary>
      ///// Save a site host.
      ///// </summary>
      ///// <param name="siteAlias"></param>
      void SaveSiteHost(SiteHost siteHost);

      void SaveSiteHost(SiteHost siteHost, string newHostName, bool isDefault);


      ///// <summary>
      ///// Delete a site host.
      ///// </summary>
      ///// <param name="siteAlias"></param>
      void DeleteSiteHost(SiteHost siteHost);

      #endregion

      //#region Site Options

      //IList<SiteOption> GetAllSiteOptionsBySite(Site site);

      //SiteOption FindSiteOptionBySiteAndName(Site site, string name);

      //void SaveSiteOption(SiteOption siteOption);

      //void SaveSiteOptions(IList<SiteOption> siteOptions);

      //void DeleteSiteOption(SiteOption siteOption);

      //#endregion

      void SaveSeoSettings(SeoSettings seoSettings);

      Site CreateNewSite(string name, string description, string email, string siteHost, Theme theme);
   }
}
