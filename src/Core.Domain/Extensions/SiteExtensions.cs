using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arashi.Core.Domain;

namespace Arashi.Core.Domain.Extensions
{
   public static class SiteExtensions
   {
      public static string DefaultUrl(this Site site)
      {
         SiteHost host = site.Hosts.SingleOrDefault(h => (h.IsDefault == true));

         string defaultSiteHost = host == null ? "(Not defined)" : host.HostName;

         return string.Format("http://{0}/", defaultSiteHost);
      }

   }
}
