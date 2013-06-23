using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arashi.Core.Repositories;
using Arashi.Core.Domain;
namespace Arashi.Services.Widget
{
   public class WidgetService : IWidgetService
   {

      public IList<Arashi.Core.Domain.Widget> GetWidgetsBySite(Site site)
      {
         return RepositoryHelper.GetSession().GetNamedQuery("GetWidgetsBySite")
                        .SetEntity("site", site)
                        .SetCacheable(true)
                        .List<Arashi.Core.Domain.Widget>();
      }



      public IList<Arashi.Core.Domain.WidgetType> GetWidgetTypes()
      {
         return RepositoryHelper.GetSession().GetNamedQuery("GetWidgetTypes")
                        .SetCacheable(true)
                        .List<Arashi.Core.Domain.WidgetType>();
      }
   }
}
