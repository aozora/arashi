using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arashi.Core.Domain;

namespace Arashi.Services.Widget
{
   public interface IWidgetService
   {
      IList<Arashi.Core.Domain.Widget> GetWidgetsBySite(Site site);

      IList<Arashi.Core.Domain.WidgetType> GetWidgetTypes();

   }
}
