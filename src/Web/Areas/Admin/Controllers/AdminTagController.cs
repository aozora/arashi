namespace Arashi.Web.Areas.Admin.Controllers
{
   using System;
   using System.Collections.Generic;
   using System.Web.Mvc;
   using Arashi.Core.Domain;
   using Arashi.Core.Domain.Dto;
   using Arashi.Services.Content;
   using Arashi.Services.Localization;
   using Arashi.Services.Membership;
   using Arashi.Services.SiteStructure;
   using Arashi.Web.Mvc.Controllers;
   using Arashi.Web.Mvc.Filters;
   using Arashi.Web.Mvc.Models;
   using Arashi.Web.Mvc.Paging;
   using Common.Logging;
   using uNhAddIns.Pagination;



   public class AdminTagController : SecureControllerBase
   {
      private readonly ILog log;
      private readonly ITagService tagService;
      private const int pageSize = 20;


      #region Constructor

      public AdminTagController(ILog log, ITagService tagService, ILocalizationService localizationService, IUserService userService, ISiteService siteService)
         : base(log, localizationService, userService, siteService)
      {
         this.log = log;
         this.tagService = tagService;
      }

      #endregion

      #region Index

      [PermissionFilter(RequiredRights = Rights.PostsView)]
      public ActionResult Index(int? page)
      {
         IEnumerable<Tag> tags = null;
         Paginator<Tag> paginator;
         IPagedList<Tag> pagedList = null;

         int currentPageIndex = 1;
         if (page.HasValue)
            currentPageIndex = page.Value;

         paginator = tagService.GetPaginatorBySite(Context.ManagedSite, pageSize);

         if (paginator.HasPages)
         {
            if (currentPageIndex > paginator.LastPageNumber)
               currentPageIndex = 1;

            tags = paginator.GetPage(currentPageIndex);

            pagedList = new PagedList<Tag>(tags, page.HasValue ? page.Value - 1 : 0, pageSize, paginator.RowsCount.Value);// , totalcount
         }

         return View("Index", pagedList);
      }

      #endregion

      /// <summary>
      /// Get the html for a list of li elements for the tag cloud
      /// </summary>
      /// <returns></returns>
      [PermissionFilter(RequiredRights = Rights.PostsView)]
      public ActionResult GetTagId(string name)
      {
         Tag tag = tagService.GetBySiteAndFriendlyName(Context.ManagedSite, name);

         return Content( tag == null ? "" : tag.TagId.ToString() );
      }



      /// <summary>
      /// Get the html for a list of li elements for the tag cloud
      /// </summary>
      /// <returns></returns>
      [AcceptVerbs(HttpVerbs.Post)]
      [PermissionFilter(RequiredRights = Rights.PostsView)]
      public ActionResult GetTagsFormatted()
      {
         IList<TagDTO> dtos = tagService.GetTagCloudBySite(Context.ManagedSite);
         // ~/Areas/Admin/Views/AdminPost/
         return this.PartialView("AdminTagCloud", dtos);
      }



      /// <summary>
      /// Get the html for a list of li elements for all the site tags
      /// </summary>
      /// <returns></returns>
      [PermissionFilter(RequiredRights = Rights.PostsView)]
      public ActionResult GetAllTags()
      {
         IList<Tag> tags = tagService.GetAllTagsBySite(Context.ManagedSite);

         return View("TagsUserControl", tags);
      }



      /// <summary>
      /// Save a new tag for the managed site.
      /// Note that the new tag is not yet associated to the current post
      /// </summary>
      /// <param name="name"></param>
      /// <returns></returns>
      [AcceptVerbs(HttpVerbs.Post)]
      [PermissionFilter(RequiredRights = Rights.PostsEdit)]
      public ActionResult SaveNew(string name)
      {
         Tag tag = new Tag
         {
            Name = name,
            Site = Context.ManagedSite
         };

         try
         {
           tagService.Save(tag);

           // Show the confirmation message
           MessageModel message = new MessageModel
           {
              Text = GlobalResource("Message_TagSaved"),
              Icon = MessageModel.MessageIcon.Info,
              CssClass = "margin-topbottom"
           };

           return View("MessageUserControl", message);
         }
         catch (Exception ex)
         {
            log.Error("UsersController.Update", ex);

            MessageModel message = new MessageModel
            {
               Text = GlobalResource("Message_GenericError"),
               Icon = MessageModel.MessageIcon.Alert,
               CssClass = "margin-topbottom"
            };

            return View("MessageUserControl", message);
         }
      }



      //// TODO: check if this can be deleted...
      //public ActionResult Edit(int tagId)
      //{
      //   return View();
      //}


   }
}

