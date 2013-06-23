namespace Arashi.Web.Areas.Admin.Controllers
{
   using System;
   using System.Collections.Generic;
   using System.Web.Mvc;
   using Arashi.Core.Domain;
   using Arashi.Services.Localization;
   using Arashi.Services.Membership;
   using Arashi.Services.SiteStructure;
   using Arashi.Services.Themes;
   using Arashi.Web.Areas.Admin.Models;
   using Arashi.Web.Mvc.Controllers;
   using Arashi.Web.Mvc.Filters;
   using Arashi.Web.Mvc.Models;
   using Arashi.Web.Mvc.Paging;
   using Common.Logging;
   using uNhAddIns.Pagination;


   public class ThemesController : SecureControllerBase
   {
      private ILog log;
      private IThemeService themeService;
      private const int pageSize = 20;

      #region Constructor

      public ThemesController(ILog log, IThemeService themeService, ILocalizationService localizationService, IUserService userService, ISiteService siteService)
         : base(log, localizationService, userService, siteService)
      {
         this.log = log;
         this.themeService = themeService;
      }

      #endregion


      [PermissionFilter(RequiredRights = Rights.TemplatesView)]
      public ActionResult Index(int? page)
      {

         return View("Index", GetIndexData(page));
      }



      /// <summary>
      /// Set the selected Theme for the managed site
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
      [PermissionFilter(RequiredRights = Rights.TemplatesChange)]
      public ActionResult Change(int id, int? page)
      {
         Theme selectedTheme = themeService.GetById(id);

         Site managedSite = Context.ManagedSite;

         managedSite.Theme = selectedTheme;

         try
         {
            siteService.SaveSite(managedSite);

            MessageModel model = new MessageModel
            {
               Text = "The selected theme has been successfully applied to the site!",
               Icon = MessageModel.MessageIcon.Info,
               CssClass = "margin-topbottom"
            };
            RegisterMessage(model);

         }
         catch (Exception ex)
         {
            log.Error(ex.ToString());

            MessageModel model = new MessageModel
            {
               Text = GlobalResource("Message_GenericError"),
               Icon = MessageModel.MessageIcon.Alert,
               CssClass = "margin-topbottom"
            };
            RegisterMessage(model);
         }

         return View("Index", GetIndexData(page));
      }



      private ThemesModel GetIndexData(int? page)
      {
         Paginator<Theme> paginator;
         IPagedList<Theme> pagedList = null;

         int currentPageIndex = 1;
         if (page.HasValue)
            currentPageIndex = page.Value;

         paginator = themeService.GetPaginatorForAll(pageSize);

         if (paginator.HasPages)
         {
            if (currentPageIndex > paginator.LastPageNumber)
               currentPageIndex = 1;

            IList<Theme> pagesList = paginator.GetPage(currentPageIndex);

            pagedList = new PagedList<Theme>(pagesList, page.HasValue ? page.Value - 1 : 0, pageSize, paginator.RowsCount.Value);
         }

         ThemesModel model = new ThemesModel()
         {
            Themes = pagedList,
            CurrentTheme = Context.ManagedSite.Theme
         };

         return model;
      }

   }
}
