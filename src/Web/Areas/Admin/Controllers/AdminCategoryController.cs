namespace Arashi.Web.Areas.Admin.Controllers
{
   using System;
   using System.Collections.Generic;
   using System.Web.Mvc;
   using Arashi.Core.Domain;
   using Arashi.Core.Domain.Extensions;
   using Arashi.Services.Content;
   using Arashi.Services.Membership;
   using Arashi.Web.Mvc.Controllers;
   using Arashi.Web.Mvc.Filters;
   using Arashi.Web.Mvc.Models;
   using Arashi.Web.Mvc.Paging;
   using Common.Logging;
   using uNhAddIns.Pagination;

   using Arashi.Services.Localization;
   using Arashi.Services.SiteStructure;

   public class AdminCategoryController : SecureControllerBase
   {
      private readonly ILog log;
      private readonly ICategoryService categoryService;
      private const int pageSize = 20;


      #region Constructor

      public AdminCategoryController(ILog log, ICategoryService categoryService, ILocalizationService localizationService, IUserService userService, ISiteService siteService)
         : base(log, localizationService, userService, siteService)
      {
         this.log = log;
         this.categoryService = categoryService;
      }

      #endregion




      #region Index

      [PermissionFilter(RequiredRights = Rights.PostsView)]
      public ActionResult Index(int? page)
      {
         IEnumerable<Category> categories = null;
         Paginator<Category> paginator;
         IPagedList<Category> pagedList = null;

         int currentPageIndex = 1;
         if (page.HasValue)
            currentPageIndex = page.Value;

         paginator = categoryService.GetPaginatorBySite(Context.ManagedSite, pageSize);

         if (paginator.HasPages)
         {
            if (currentPageIndex > paginator.LastPageNumber)
               currentPageIndex = 1;

            categories = paginator.GetPage(currentPageIndex);

            pagedList = new PagedList<Category>(categories, page.HasValue ? page.Value - 1 : 0, pageSize, paginator.RowsCount.Value);// , totalcount
         }

         return View("Index", pagedList);
      }

      #endregion




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



      //// check if this can be deleted
      //public ActionResult Edit(int categoryId)
      //{
      //   return View();
      //}



   }
}

