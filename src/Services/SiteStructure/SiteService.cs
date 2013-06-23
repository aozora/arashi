namespace Arashi.Services.SiteStructure
{
   using System;
   using System.Collections.Generic;
   using System.IO;
   using System.Linq;
   using System.Web;
   using Arashi.Core.Domain;
   using Arashi.Core.Repositories;
   using Arashi.Services.File;
   using Arashi.Services.Membership;
   using Common.Logging;
   using NHibernate;




   /// <summary>
   /// Provides functionality to manage site instances.
   /// </summary>
   public class SiteService : ServiceBase, ISiteService
   {
      private readonly IUserService userService;
      private readonly IFileService fileService;

      #region Constructor

      /// <summary>
      /// Constructor.
      /// </summary>
      /// <param name="log"></param>
      /// <param name="userService"></param>
      /// <param name="fileService"></param>
      /// <param name="sessionFactory"></param>
      public SiteService(Arashi.Core.NHibernate.ISessionFactory sessionFactory, ILog log, IUserService userService, IFileService fileService)
         : base(sessionFactory, log)
      {
         this.userService = userService;
         this.fileService = fileService;
      }

      #endregion


      //   public virtual IRepository<T> Repository<T>
      //{
      //   get
      //   {
      //      return null;
      //   }
      //   set
      //   {

      //   }
      //}


      #region ISiteService Members

      #region New Site

      public Site CreateNewSite(string name, string description, string email, string siteHost, Theme theme)
      {
         if (theme == null)
            throw new NullReferenceException("a default theme is mandatory to create a new site");

         // Site
         Site site = new Site
         {
            Name = name,
            Description = description,
            AllowRegistration = false,
            EnableCaptchaForComments = false,
            AllowPasswordRetrieval = true,
            DefaultCulture = "en-US",
            TimeZone = 0,
            Email = string.IsNullOrEmpty(email) ? "webmaster@localhost.com" : email,
            MaxPostsPerPage = 10,
            MaxCommentsPerPage = 50,
            MaxLinksInComments = 3,
            MaxSyndicationFeeds = 10,
            AllowComments = true,
            AllowCommentsOnlyForRegisteredUsers = false,
            SortCommentsFromOlderToNewest = true,
            FeedUseSummary = true,
            ShowAvatars = true,
            Theme = theme,
            CreatedDate = DateTime.Now.ToUniversalTime()
         };


         // Site Host
         SiteHost host = new SiteHost
         {
            HostName = siteHost,
            IsDefault = true,
            Site = site,
            CreatedDate = DateTime.Now.ToUniversalTime()
         };

         site.Hosts.Add(host);

         //// Site Host for Control Panel
         //SiteHost cpHost = new SiteHost
         //{
         //   HostName = "admin." + siteHost,
         //   IsDefault = false,
         //   Site = site,
         //   CreatedDate = DateTime.Now.ToUniversalTime()
         //};

         //site.Hosts.Add(cpHost);

         SeoSettings seo = new SeoSettings
         {
            Site = site,
            PostTitleFormat = "%post_title% | %blog_title%",
            PageTitleFormat = "%page_title% | %blog_title%",
            CategoryTitleFormat = "%category_title% | %blog_title%",
            TagTitleFormat = "%tag% | %blog_title%",
            SearchTitleFormat = "%search% | %blog_title%",
            ArchiveTitleFormat = "%date% | %blog_title%",
            Page404TitleFormat = "Nothing found for %request_words%",
            DescriptionFormat = "%description%",
            UseCategoriesForMeta = true,
            GenerateKeywordsForPost = true,
            UseNoIndexForArchives = true,
            UseNoIndexForCategories = true,
            UseNoIndexForTags = false,
            GenerateDescriptions = true,
            CapitalizeCategoryTitles = true,
            RewriteTitles = true,
            HomeDescription = "",
            HomeKeywords = "",
            HomeTitle = ""
         };
         //site.SeoSettings = seo;


         //using (INHTransactionScope tx = new NHTransactionScope())
         //{
            log.Debug("SiteService.CreateNewSite: Saving new site...");

            // Save the new site
            SaveSite(site);

            // this is ...odd, but if I comment the following 3 lines I get an error
            // from NH: Could not perform Save for Site ---> NHibernate.PropertyValueException: not-null property references a null or transient valueArashi.Core.Domain.SeoSettings.Site
            site.SeoSettings = seo;
            SaveSeoSettings(seo);
            SaveSite(site);


            log.Debug("SiteService.CreateNewSite: Site saved...");

            log.Debug("SiteService.CreateNewSite: Creating default roles...");

            // create default roles
            userService.CreateDefaultRoles(site);

            //tx.VoteCommit();
            log.Debug("SiteService.CreateNewSite: Transaction commit");
         //}

         log.Debug("SiteService.CreateNewSite: Creating Site folder structure - Start");

         // Create SiteData folder structure
         string siteDataRoot = HttpContext.Current.Server.MapPath(site.SiteDataPath);
         log.Debug("Install.CreateSite: siteDataRoot = " + siteDataRoot);

         DirectoryInfo di = new DirectoryInfo(siteDataRoot);

         // Check if the parent folder is writable
         if (di.Parent != null && !fileService.CheckIfDirectoryIsWritable(di.Parent.FullName))
            throw new IOException(string.Format("Unable to create the site because the directory {0} is not writable.", siteDataRoot));

         string siteDataPhysicalDirectory = siteDataRoot; //Path.Combine(siteDataRoot, site.SiteId.ToString());

         fileService.CreateDirectory(siteDataPhysicalDirectory);
         //CreateDirectory(Path.Combine(siteDataPhysicalDirectory, "UserFiles"));
         fileService.CreateDirectory(Path.Combine(siteDataPhysicalDirectory, "index"));

         log.Debug("SiteService.CreateNewSite: Creating Site folder structure - End");

         return site;
      }


      #endregion

      #region Site

      public Site GetSiteById(int siteId)
      {
         //using (INHTransactionScope tx = new NHTransactionScope())
         //{
            Site site = Session.GetNamedQuery("GetSiteById")
                           .SetInt32("siteid", siteId)
                           .UniqueResult<Site>();
            NHibernateUtil.Initialize(site.Hosts);
            //tx.VoteCommit();

            return site;
         //}

      }



      public Site GetSiteByHostName(string host)
      {
         return Session.GetNamedQuery("GetSiteByHostName")
                        .SetString("host", host)
                        .UniqueResult<Site>();
      }



      public SiteHost GetDefaultSiteHost(Site site)
      {
         return GetSiteHostsBySite(site).SingleOrDefault(h => (h.IsDefault == true));
      }



      public IList<SiteHost> GetSiteHostsBySite(Site site)
      {
         return Session.GetNamedQuery("GetSiteHostsBySite")
                        .SetEntity("site", site)
                        .List<SiteHost>();
      }



      public SiteHost GetSiteHostByHostName(string host)
      {
         return Session.GetNamedQuery("GetSiteHostByHostName")
                        .SetString("host", host)
                        .UniqueResult<SiteHost>();
      }


      public IList<Site> GetAllSites()
      {
         //return Repository<Site>.FindAll();
         return Session.GetNamedQuery("FindAll")
                        .List<Site>();
      }



      public void SaveSite(Site site)
      {
         //using (INHTransactionScope tx = new NHTransactionScope())
         //{
            Repository<Site>.Save(site);
         //   tx.VoteCommit();
         //}
      }



      public void DeleteSite(Site site)
      {
         //if (site.RootNodes.Count > 0)
         //{
         //   throw new Exception("Can't delete a site when there are still related nodes. Please delete all nodes before deleting an entire site.");
         //}
         //else
         //{
         //IList aliases = this.siteStructure.GetSiteAliasesBySite(site);
         //if (aliases.Count > 0)
         //{
         //   throw new Exception("Unable to delete a site when a site has related aliases.");
         //}
         //else
         //{
         try
         {
            // We need to use a specific DAO to also enable clearing the query cache.
            //using (INHTransactionScope tx = new NHTransactionScope())
            //{
               // Clear query cache first
               //session.SessionFactory.EvictQueries("Sites");
               Repository<Site>.ClearSecondLevelCache();

               // Delete site
               Repository<Site>.Delete(site);

            //   tx.VoteCommit();
            //}
         }
         catch (Exception ex)
         {
            log.Error("Error deleting site", ex);
            throw;
         }

         //}				
         //}
      }

      #endregion

      #region SiteHost

      public void SaveSiteHost(SiteHost siteHost)
      {
         //using (INHTransactionScope tx = new NHTransactionScope())
         //{
            // Clear query cache first
            //session.SessionFactory.EvictQueries("Sites");
            Repository<Site>.ClearSecondLevelCache();
            Repository<SiteHost>.Save(siteHost);

         //   tx.VoteCommit();
         //}
      }



      public void SaveSiteHost(SiteHost siteHost, string newHostName, bool isDefault)
      {
         //using (INHTransactionScope tx = new NHTransactionScope())
         //{
            // Clear query cache first
            //session.SessionFactory.EvictQueries("Sites");
            Repository<Site>.ClearSecondLevelCache();

            SiteHost oldDefaultHost = GetDefaultSiteHost(siteHost.Site);

            // if the updated host is set as default, check if it is different fromn the default sitehost,
            // in that case swap the default host
            if (isDefault && oldDefaultHost != null && oldDefaultHost != siteHost)
            {
               oldDefaultHost.IsDefault = false;
               Repository<SiteHost>.Save(oldDefaultHost);
            }

            // Save host
            siteHost.HostName = newHostName;
            siteHost.IsDefault = isDefault;
            Repository<SiteHost>.Save(siteHost);

         //   tx.VoteCommit();
         //}
      }



      public void DeleteSiteHost(SiteHost siteHost)
      {
         // If is the last host don't delete it
         if (siteHost.Site.Hosts.Count == 1)
         {
            log.WarnFormat("SiteService.DeleteSiteHost: Can't delete the host {0} because is the only one for the site", siteHost.HostName);
            return;
         }

         //using (INHTransactionScope tx = new NHTransactionScope())
         //{
            bool wasDefault = siteHost.IsDefault;

            Site site = siteHost.Site;
            site.Hosts.Remove(siteHost);
            Repository<SiteHost>.Delete(siteHost);
            Repository<Site>.Save(site);

            // if the deleted host was the default, then set as default a new one
            if (wasDefault)
            {
               SiteHost newDefaultSiteHost = site.Hosts[0];
               newDefaultSiteHost.IsDefault = true;
               Repository<SiteHost>.Save(newDefaultSiteHost);
            }

         //   tx.VoteCommit();
         //}
      }



      public SiteHost GetSiteHostById(int siteHostId)
      {
         return Repository<SiteHost>.FindById(siteHostId);
      }

      #endregion

      #region Theme

      public Theme GetThemeById(int templateId)
      {
         return Repository<Theme>.FindById(templateId);
      }


      public IList<Theme> GetThemes()
      {
         return Session.GetNamedQuery("GetTemplates")
                        .List<Theme>();
      }

      #endregion

      #region Seo Settings

      public void SaveSeoSettings(SeoSettings seoSettings)
      {
         //using (INHTransactionScope tx = new NHTransactionScope())
         //{
         Repository<SeoSettings>.Save(seoSettings);
         //   tx.VoteCommit();
         //}
      }

      #endregion

      #region Tracking Info

      public void StoreTrackingInfo(TrackingInfo trackingInfo)
      {
         //using (INHTransactionScope tx = new NHTransactionScope())
         //{
         Repository<TrackingInfo>.Save(trackingInfo);
         //   tx.VoteCommit();
         //}
      }



      public IList<TrackingInfo> GetTrackingInfoForUsersOnline(int minutesSinceLastInactive)
      {
         DateTime lastActivityDate = DateTime.Now.AddMinutes(-minutesSinceLastInactive).ToUniversalTime();

         return Session.GetNamedQuery("GetTrackingInfoForUsersOnline")
               .SetDateTime("lastActivityDate", lastActivityDate)
               .List<TrackingInfo>();
      }



      #endregion

      #endregion

   }
}
