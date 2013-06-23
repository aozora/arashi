using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Arashi.Core.Domain;
using Arashi.Core.Domain.Dto;
using Arashi.Services.Content;
using Arashi.Services.Membership;
using Arashi.Web.Mvc.Controllers;
using Arashi.Web.Mvc.Filters;
using Arashi.Web.Mvc.Models;
using log4net;

namespace Arashi.Web.Areas.Admin.Controllers
{
   // TODO: this should be moved to AdminContentItemController
   public class AdminTagController : SecureControllerBase
   {
      private static readonly ILog log = LogManager.GetLogger(typeof(AdminTagController));
      private readonly ITagService tagService;


      public AdminTagController(ITagService tagService)
      {
         this.tagService = tagService;
      }




      // TODO: check if this can be deleted...
      public ActionResult Index()
      {
         return View();
      }



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
      [PermissionFilter(RequiredRights = Rights.PostsView)]
      public ActionResult GetTagsFormatted()
      {
         IList<TagDTO> dtos = tagService.GetTagCloudBySite(Context.ManagedSite);

         //return View("TagsUserControl", dtos);
         return View("~/Areas/Admin/Views/AdminPost/AdminTagCloud.ascx", dtos);
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



      // TODO: check if this can be deleted...
      public ActionResult Edit(int tagId)
      {
         return View();
      }


   }
}

