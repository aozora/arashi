using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Arashi.Core.Domain;

namespace Arashi.Web.Areas.Admin.Models
{
   public class SiteFeatureModel
   {
      //public virtual Site Site { get; set; }

      public virtual Feature Feature { get; set; }

      public virtual bool Enabled { get; set; }

      public virtual bool StartDate { get; set; }
      public virtual bool EndDate { get; set; }

   }
}