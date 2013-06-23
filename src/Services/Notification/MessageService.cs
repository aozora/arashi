using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arashi.Services.Notification
{
   using Arashi.Core.Domain;
   using Arashi.Core.NHibernate;
   using Arashi.Core.Repositories;

   using NHibernate.Criterion;

   using uNhAddIns.Pagination;

   public class MessageService : IMessageService
   {
      /// <summary>
      /// Gets a single message by id
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
      public Message GetById(int id)
      {
         return Repository<Message>.FindById(id);
      }



      /// <summary>
      /// Saves a message.
      /// </summary>
      /// <param name="comment"></param>
      public void Save(Message message)
      {
         using (NHTransactionScope tx = new NHTransactionScope())
         {
            Repository<Message>.Save(message);
            tx.VoteCommit();
         }
      }



      /// <summary>
      /// Delete a message
      /// </summary>
      /// <param name="comment"></param>
      public void DeleteComment(Message message)
      {
         using (NHTransactionScope tx = new NHTransactionScope())
         {
            Repository<Message>.Delete(message);
            tx.VoteCommit();
         }
      }



      /// <summary>
      /// Get a list of the queued email messages to send
      /// </summary>
      /// <returns></returns>
      public IList<Message> GetQueuedEmailMessages(int maxMessagesToRetrieve)
      {
         return RepositoryHelper.GetSession().GetNamedQuery("GetQueuedEmailMessages").SetMaxResults(maxMessagesToRetrieve)
                           .List<Message>();

      }




      /// <summary>
      /// Retrieve a list of messages by status and type
      /// </summary>
      /// <param name="status"></param>
      /// <param name="type"></param>
      /// <param name="pageSize"></param>
      /// <returns></returns>
      public Paginator<Message> FindPagedMessagesBySiteAndStatusAndType(Site site, MessageStatus status, MessageType type, string orderBy, bool orderAscending, int pageSize)
      {
         DetachedCriteria criteria = DetachedCriteria.For<Message>()
                                       .AddOrder(new Order(orderBy, orderAscending))
                                       .Add(Restrictions.Eq("Site", site));
                                       
         return Repository<Message>.GetPaginator(criteria, pageSize);
      }




      /// <summary>
      /// Retrieve a list of messages by site
      /// </summary>
      /// <param name="site"></param>
      /// <param name="orderBy"></param>
      /// <param name="orderAscending"></param>
      /// <param name="pageSize"></param>
      /// <returns></returns>
      public Paginator<Message> FindPagedMessagesBySite(Site site, string orderBy, bool orderAscending, int pageSize)
      {
         DetachedCriteria criteria = DetachedCriteria.For<Message>()
                                       .AddOrder(new Order(orderBy, orderAscending))
                                       .Add(Restrictions.Eq("Site", site));

         return Repository<Message>.GetPaginator(criteria, pageSize);
      }





   }
}
