using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Arashi.Core.Domain;

namespace Arashi.Web.Areas.Admin.Models
{
   public class PageModel
   {
      public Page Page {get; set;}

      public SelectList WorkflowStatus { get; set; }

      public SelectList CustomTemplateFiles { get; set; }

      //public SelectList ParentPages { get; set; }
      public IList<Page> ParentPages { get; set; }
   }
}
