using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Arashi.Web.Mvc.Paging;
using Arashi.Core.Domain;

namespace Arashi.Web.Areas.Admin.Models
{
   public class ThemesModel
   {
      public IPagedList<Template> Templates {get; set;}

      public Template CurrentTemplate {get; set;}
   }
}