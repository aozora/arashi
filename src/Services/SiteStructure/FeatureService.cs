using System.Collections.Generic;
using Arashi.Core.Domain;
using Arashi.Core.NHibernate;
using Arashi.Core.Repositories;
using Common.Logging;
using System;
using ApplicationException = Arashi.Core.Exceptions.ApplicationException;

namespace Arashi.Services.SiteStructure
{
   public class FeatureService : ServiceBase, IFeatureService
   {
      private readonly ISiteService siteService;

      #region Constructor

      public FeatureService(ISessionFactory sessionFactory, ILog log, ISiteService siteService)
         : base(sessionFactory, log)
      {
         this.siteService = siteService;
      }

      #endregion


      public IList<Feature> FindAll()
      {
         return Repository<Feature>.FindAll();
      }



      public SiteFeature FindSiteFeatureById(int id)
      {
         return Repository<SiteFeature>.FindById(id);
      }


      public void SaveSiteFeature(SiteFeature siteFeature)
      {
         Repository<SiteFeature>.Save(siteFeature);
      }



      public void SetFeaturesForSite(Site site)
      {
         if (site.Features.Count > 0)
         {
            log.WarnFormat("Can't add Features to siteid {0} because it already has {1} features", site.SiteId.ToString(), site.Features.Count.ToString());
            throw new ApplicationException("Site already has features!");
         }

         IList<Feature> features = FindAll();

         foreach (Feature feature in features)
         {
            SiteFeature sf = new SiteFeature()
                                {
                                   Site = site,
                                   Feature = feature,
                                   Enabled = false,
                                   StartDate = DateTime.Now.ToUniversalTime(),
                                   EndDate = null
                                };
            site.Features.Add(sf);
         }

         siteService.SaveSite(site);
         
      }


   }
}
