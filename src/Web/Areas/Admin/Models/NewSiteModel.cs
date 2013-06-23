using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Arashi.Core.Domain;

namespace Arashi.Web.Areas.Admin.Models
{
   public class NewSiteModel
   {
      [Required(ErrorMessage = "The name of the new site must be specified.")]
      public string Name
      {
         get;
         set;
      }

      public string Description {get; set;}

      [Required(ErrorMessage = "The default host name must be specified.")]
      public string DefaultHostName
      {
         get;
         set;
      }
   }
}
