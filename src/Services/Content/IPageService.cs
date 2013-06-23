using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arashi.Services.Content
{
   public interface IPageService
   {
      /// <summary>
      /// Reorder the pages specified in the ids array
      /// </summary>
      /// <param name="ids"></param>
      void Sort(object[] ids);

   }
}
