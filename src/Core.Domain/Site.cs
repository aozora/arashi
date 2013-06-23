using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Collections.Generic;
using Arashi.Core.Domain.Validation;

namespace Arashi.Core.Domain
{

   public class Site
   {
      #region Public Properties

      public virtual int SiteId { get; set; }

      /// <summary>
      /// The name/title of the site.
      /// For SEO purpose must be less or equal 70 chars.
      /// </summary>
      [Required(ErrorMessage = "A site must have a name.")]
      public virtual string Name
      {
         get;
         set;
      }

      /// <summary>
      /// This is the tagline. Is also used to generate the meta description (SEO).
      /// For SEO purpose must be less or equal 160 chars.
      /// </summary>
      public virtual string Description { get; set; }

      public virtual bool AllowRegistration { get; set; }
      public virtual bool AllowPasswordRetrieval { get; set; }
      public virtual int TimeZone { get; set; }

      [Required(ErrorMessage = "Email address is missing.")]
      [Email(ErrorMessage = "Invalid e-mail address.")]
      public virtual string Email
      {
         get;
         set;
      }

      /// <summary>
      /// The default role for registered users.
      /// </summary>
      public virtual Role DefaultRole { get; set; }
      public virtual Page DefaultPage { get; set; }
      public virtual SiteHost DefaultSiteHost
      {
         get
         {
            return Hosts.SingleOrDefault(h => (h.IsDefault == true));
         }
      }
      public virtual string DateFormat { get; set; }
      public virtual string TimeFormat { get; set; }
      public virtual string DefaultCulture { get; set; }
      public virtual SiteStatus Status { get; set; }

      // Post Settings
      public virtual int MaxPostsPerPage { get; set; }
      public virtual int MaxSyndicationFeeds { get; set; }
      public virtual bool FeedUseSummary { get; set; }
      
      // Comment Settings
      public virtual bool AllowPings { get; set; }
      public virtual bool AllowComments { get; set; }
      public virtual bool AllowCommentsOnlyForRegisteredUsers { get; set; }
      public virtual int MaxCommentsPerPage { get; set; }
      public virtual bool SortCommentsFromOlderToNewest { get; set; }
      public virtual bool SendEmailForNewComment { get; set; }
      public virtual bool SendEmailForNewModeration { get; set; }
      public virtual bool ShowAvatars { get; set; }
      public virtual int MaxLinksInComments { get; set; }
      public virtual string ModerationKeys { get; set; } 
      public virtual string BlacklistKeys { get; set; } 
      public virtual bool EnableCaptchaForComments { get; set; }
      public virtual string CaptchaPrivateKey { get; set; } 
      public virtual string CaptchaPublicKey { get; set; } 

      public virtual Template Template { get; set; }
      public virtual SeoSettings SeoSettings { get; set; }
      public virtual string TrackingCode { get; set; }

      public virtual DateTime CreatedDate { get; set; }
      public virtual DateTime? UpdatedDate { get; set; }

      protected int Version { get; set; }

      /// <summary>
      /// The virtual path of the site data directory (starting with ~/ and ending with /), i.e. "~/Sites/3/"
      /// If the site is new and not already saved, return an empty string.
      /// </summary>
      public virtual string SiteDataPath
      {
         get
         {
            //if (SiteId <= 0)
            //{
            //   throw new InvalidOperationException("Unable to get the site data folder path when the site isn't saved yet.");
            //}
            if (SiteId > -1)
               return string.Format("~/Sites/{0}/", SiteId);
            else
               return string.Empty;
         }
      }



      // Collections
      public virtual IList<SiteHost> Hosts { get; set; }
      //public virtual IList<Role> Roles { get; set; }

      /// <summary>
      /// The root categories that are related to the site.
      /// </summary>
      public virtual IList<Category> RootCategories { get; set; }



      #endregion

      #region ctor

      /// <summary>
      /// Constructor. Inizializza i default
      /// </summary>
      public Site()
      {
         SiteId = -1;
         Status = 0; // SiteStatus.Active

         Hosts = new List<SiteHost>();
         RootCategories = new List<Category>();
      }

      #endregion

      #region Equality

      public override bool Equals(object other)
      {
         if (this == other)
            return true;
         Site site = other as Site;
         if (site == null)
            return false;
         if (SiteId != site.SiteId)
            return false;
         return true;
      }



      public override int GetHashCode()
      {
         unchecked
         {
            int result;
            result = SiteId.GetHashCode();
            result = 29 * result;
            return result;
         }
      }

      #endregion
   }

}
