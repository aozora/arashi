using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Arashi.Core.Domain.Dto;
using Arashi.Web.Mvc.Models;

namespace Arashi.Web.Areas.Admin.Models
{
   public class DashboardModel
   {
      public ContentItemStatsDTO ContentItemStats {get; set;}
      public IList<ControlPanelModel> ControlPanelModels {get; set;}
      public bool ShowWelcome {get; set;}
   }
}
