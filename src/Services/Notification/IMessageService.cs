namespace Arashi.Services.Notification
{
   using System.Collections.Generic;

   using Arashi.Core.Domain;
   using uNhAddIns.Pagination;


   public interface IMessageService
   {
      /// <summary>
      /// Gets a single message by id
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
      Message GetById(int id);

      /// <summary>
      /// Saves a message.
      /// </summary>
      /// <param name="comment"></param>
      void Save(Message message);

      /// <summary>
      /// Delete a message
      /// </summary>
      /// <param name="comment"></param>
      void DeleteComment(Message message);


      IList<Message> GetQueuedEmailMessages(int maxMessagesToRetrieve);


      /// <summary>
      /// Retrieve a list of messages by status and type
      /// </summary>
      /// <param name="status"></param>
      /// <param name="type"></param>
      /// <param name="pageSize"></param>
      /// <returns></returns>
      Paginator<Message> FindPagedMessagesBySiteAndStatusAndType(Site site, MessageStatus status, MessageType type, string orderBy, bool orderAscending, int pageSize);


      /// <summary>
      /// Retrieve a list of messages by site
      /// </summary>
      /// <param name="site"></param>
      /// <param name="orderBy"></param>
      /// <param name="orderAscending"></param>
      /// <param name="pageSize"></param>
      /// <returns></returns>
      Paginator<Message> FindPagedMessagesBySite(Site site, string orderBy, bool orderAscending, int pageSize);
   }
}
