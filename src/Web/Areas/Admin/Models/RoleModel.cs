using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Arashi.Core.Domain;

namespace Arashi.Web.Areas.Admin.Models
{
   public class RoleModel
   {
      public Role Role {get; set;}

      public IList<Right> AllRights {get; set;}
   }
}
