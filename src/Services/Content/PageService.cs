namespace Arashi.Services.Content
{
   using System;
   using Arashi.Core.Domain;
   using Arashi.Core.NHibernate;
   using Common.Logging;


   /// <summary>
   /// Page Service
   /// </summary>
   public class PageService : ServiceBase, IPageService
   {
      private readonly IContentItemService<Page> contentItemService;


      public PageService(Arashi.Core.NHibernate.ISessionFactory sessionFactory, ILog log, IContentItemService<Page> contentItemService)
         : base(sessionFactory, log)
      {
         this.contentItemService = contentItemService;
      }




      /// <summary>
      /// Reorder the pages specified in the ids array
      /// </summary>
      /// <param name="ids"></param>
      public void Sort(object[] ids)
      {
         //using (NHTransactionScope tx = new NHTransactionScope())
         //{
            for (int index = 0; index < ids.Length; index++)
            {
               Page page = contentItemService.GetById(Convert.ToInt32(ids[index]));
               page.Position = index;

               contentItemService.Save(page);
               log.DebugFormat("PageService: Saving page [Id: {0}] with Position {1}", page.Id.ToString(), page.Position);
            }
            
         //   tx.VoteCommit();
         //}
      }

   }
}
