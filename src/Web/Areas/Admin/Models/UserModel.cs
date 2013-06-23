using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Arashi.Core.Domain;

namespace Arashi.Web.Areas.Admin.Models
{
   /// <summary>
   /// UserModel
   /// </summary>
   public class UserModel
   {
      public User User { get; set; }

      public SelectList Cultures { get; set; }
      public SelectList AdminThemes { get; set; }
      public SelectList TimeZones { get; set; }
      public IList<Role> Roles { get; set; }

   }
}
