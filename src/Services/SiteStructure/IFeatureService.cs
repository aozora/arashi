using System.Collections.Generic;
using Arashi.Core.Domain;

namespace Arashi.Services.SiteStructure
{
   public interface IFeatureService
   {
      IList<Feature> FindAll();

      void SaveSiteFeature(SiteFeature siteFeature);

      SiteFeature FindSiteFeatureById(int id);

      void SetFeaturesForSite(Site site);

   }
}