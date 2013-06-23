using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Arashi.Core.Domain;
using Arashi.Core.Domain.Extensions;
using Arashi.Services.Content;
using Arashi.Services.Membership;
using Arashi.Web.Mvc.Controllers;
using Arashi.Web.Mvc.Filters;
using Arashi.Web.Mvc.Models;
using log4net;

namespace Arashi.Web.Areas.Admin.Controllers
{
   public class AdminCategoryController : SecureControllerBase
   {
      private static readonly ILog log = LogManager.GetLogger(typeof(AdminCategoryController));
      private readonly ICategoryService categoryService;


      public AdminCategoryController(ICategoryService categoryService)
      {
         this.categoryService = categoryService;
      }





      public ActionResult Index()
      {
         return View();
      }




      [AcceptVerbs(HttpVerbs.Post)]
      [PermissionFilter(RequiredRights = Rights.PostsEdit)]
      public ActionResult SaveNew(string name, int? parentCategoryId)
      {
         Category category = new Category
         {
            Name = name,
            Site = Context.ManagedSite
         };

         if (parentCategoryId.HasValue)
            category.SetParentCategory(categoryService.GetById(parentCategoryId.Value));
         else
            category.SetParentCategory(null);

         try
         {
           categoryService.Save(category);

           // Show the confirmation message
           MessageModel message = new MessageModel
           {
              Text = GlobalResource("Message_CategorySaved"),
              Icon = MessageModel.MessageIcon.Info,
              CssClass = "margin-topbottom"
           };

           return View("MessageUserControl", message);
         }
         catch (Exception ex)
         {
            log.Error("AdminCategoryController.SaveNew", ex);

            MessageModel message = new MessageModel
            {
               Text = GlobalResource("Message_GenericError"),
               Icon = MessageModel.MessageIcon.Alert,
               CssClass = "margin-topbottom"
            };

            return View("MessageUserControl", message);
         }
      }



      // check if this can be deleted
      public ActionResult Edit(int categoryId)
      {
         return View();
      }



   }
}

