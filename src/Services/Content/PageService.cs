using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arashi.Core.Domain;
using Arashi.Core.NHibernate;
using log4net;

namespace Arashi.Services.Content
{
   public class PageService : IPageService
   {
      private static readonly ILog log = LogManager.GetLogger(typeof(PageService));
      private IContentItemService<Page> contentItemService;


      public PageService(IContentItemService<Page> contentItemService)
      {
         this.contentItemService = contentItemService;
      }




      /// <summary>
      /// Reorder the pages specified in the ids array
      /// </summary>
      /// <param name="ids"></param>
      public void Sort(object[] ids)
      {
         using (NHTransactionScope tx = new NHTransactionScope())
         {
            for (int index = 0; index < ids.Length; index++)
            {
               Page page = contentItemService.GetById(Convert.ToInt32(ids[index]));
               page.Position = index;

               contentItemService.Save(page);
               log.DebugFormat("PageService: Saving page [Id: {0}] with Position {1}", page.Id.ToString(), page.Position);
            }
            
            tx.VoteCommit();
         }
      }

   }
}
