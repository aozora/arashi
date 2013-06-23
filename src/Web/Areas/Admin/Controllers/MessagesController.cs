namespace Arashi.Web.Areas.Admin.Controllers
{
   using System;
   using System.Collections.Generic;
   using System.Web.Mvc;

   using Arashi.Core.Domain;
   using Arashi.Services.Localization;
   using Arashi.Services.Membership;
   using Arashi.Services.Notification;
   using Arashi.Services.SiteStructure;
   using Arashi.Web.Mvc.Controllers;
   using Arashi.Web.Mvc.Filters;
   using Arashi.Web.Mvc.Models;
   using Arashi.Web.Mvc.Paging;

   using Common.Logging;

   using uNhAddIns.Pagination;



   /// <summary>
   /// Manage the comment moderation
   /// </summary>
   public class MessagesController : SecureControllerBase
   {
      private ILog log;
      private const int pageSize = 20;

      private readonly IMessageService messageService;

      #region Constructor

      public MessagesController(ILog log, IMessageService messageService, ILocalizationService localizationService, IUserService userService, ISiteService siteService)
         : base(log, localizationService, userService, siteService)
      {
         this.log = log;
         this.messageService = messageService;
      }

      #endregion

      /// <summary>
      /// Show the list of comments
      /// </summary>
      /// <param name="page"></param>
      /// <param name="status"></param>
      /// <returns></returns>
      [PermissionFilter(RequiredRights = Rights.MessagesView)]
      public ActionResult Index(int? page, string status, string type)
      {
         IList<Message> messages = null;
         Paginator<Message> paginator;
         IPagedList<Message> pagedList = null;

         int currentPageIndex = 1;
         if (page.HasValue)
            currentPageIndex = page.Value;

         if (string.IsNullOrEmpty(status) && string.IsNullOrEmpty(type))
            paginator = messageService.FindPagedMessagesBySite(Context.ManagedSite, "CreatedDate", false, pageSize);
         else
            paginator = messageService.FindPagedMessagesBySiteAndStatusAndType(Context.ManagedSite, (MessageStatus)Enum.Parse(typeof(MessageStatus), status) , (MessageType)Enum.Parse(typeof(MessageType), type), "CreatedDate", false, pageSize);

         if (paginator.HasPages)
         {
            if (currentPageIndex > paginator.LastPageNumber)
               currentPageIndex = 1;

            messages = paginator.GetPage(currentPageIndex);

            pagedList = new PagedList<Message>(messages, page.HasValue ? page.Value - 1 : 0, pageSize, paginator.RowsCount.Value);// , totalcount
         }

         return View("Index", pagedList);
      }




      /// <summary>
      /// Get the details of a stored message
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
      [PermissionFilter(RequiredRights = Rights.MessagesView)]
      public ActionResult GetMessageBody(int id)
      {
         Message message = messageService.GetById(id);

         return Content(message.Body);
      }




      /// <summary>
      /// Reset the attemps count of a message, so the next time the scheduler will try to send it again
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
      [PermissionFilter(RequiredRights = Rights.MessagesEdit)]
      public ActionResult ResetMessage(int id)
      {
         try
         {
            Message message = messageService.GetById(id);
            message.AttemptsCount = 0;
            message.UpdatedDate = DateTime.UtcNow;
            message.Status = MessageStatus.Queued;

            messageService.Save(message);

            MessageModel messageModel = new MessageModel
            {
               Text = "The selected message has been resetted and queued for send!",
               Icon = MessageModel.MessageIcon.Info,
            };
            RegisterMessage(messageModel, true);
         }
         catch (Exception ex)
         {
            log.Error("MessagesController.Delete", ex);

            MessageModel messageModel = new MessageModel
            {
               Text = GlobalResource("Message_GenericError"),
               Icon = MessageModel.MessageIcon.Alert,
            };

            RegisterMessage(messageModel, true);
         }

         return RedirectToAction("Index");
      }



      /// <summary>
      /// Block a message so it will not be send
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
      [PermissionFilter(RequiredRights = Rights.MessagesEdit)]
      public ActionResult BlockMessage(int id)
      {
         try
         {
            Message message = messageService.GetById(id);
            message.AttemptsCount = 0;
            message.UpdatedDate = DateTime.UtcNow;
            message.Status = MessageStatus.NotSent;
            message.AttemptsCount = 3;

            messageService.Save(message);

            MessageModel messageModel = new MessageModel
            {
               Text = "The selected message has been blocked!",
               Icon = MessageModel.MessageIcon.Info,
            };
            RegisterMessage(messageModel, true);
         }
         catch (Exception ex)
         {
            log.Error("MessagesController.Delete", ex);

            MessageModel messageModel = new MessageModel
            {
               Text = GlobalResource("Message_GenericError"),
               Icon = MessageModel.MessageIcon.Alert,
            };

            RegisterMessage(messageModel, true);
         }

         return RedirectToAction("Index");
      }



      /// <summary>
      /// Delete a given message
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
      [AcceptVerbs(HttpVerbs.Post)]
      [PermissionFilter(RequiredRights = Rights.MessagesDelete)]
      public ActionResult Delete(int id)
      {
         Message message = messageService.GetById(id);

         try
         {
            messageService.DeleteComment(message);

            MessageModel messageModel = new MessageModel
            {
               Text = "The selected message has been deleted!",
               Icon = MessageModel.MessageIcon.Info,
            };

            return View("MessageUserControl", messageModel);
         }
         catch (Exception ex)
         {
            log.Error("MessagesController.Delete", ex);

            MessageModel messageModel = new MessageModel
            {
               Text = GlobalResource("Message_GenericError"),
               Icon = MessageModel.MessageIcon.Alert,
            };

            return View("MessageUserControl", messageModel);
         }
      }

   }
}
