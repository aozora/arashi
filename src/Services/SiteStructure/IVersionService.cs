using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arashi.Services.SiteStructure
{
   public interface IVersionService
   {
      Arashi.Core.Domain.Version GetVersion();

      Arashi.Core.Domain.Version GetVersionForAssembly(string assembly);
   }
}
