using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Arashi.Core.Domain;
using Arashi.Core.Domain.Dto;
using Arashi.Web.Mvc.Models;

namespace Arashi.Web.Areas.Admin.Models
{
   public class DashboardModel
   {
      public ContentItemStatsDTO ContentItemStats {get; set;}
      
      [Obsolete("This will be replaced by the Features Collection", false)]
      public IList<ControlPanelModel> ControlPanelModels {get; set;}

      public IEnumerable<SiteFeature> SiteFeatures {get; set;}
      public IEnumerable<FeatureCategory> FeatureCategories { get; set; }

      public bool ShowWelcome {get; set;}
   }
}
