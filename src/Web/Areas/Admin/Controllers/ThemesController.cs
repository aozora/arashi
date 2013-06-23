using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Arashi.Core.Domain;
using Arashi.Services.Membership;
using Arashi.Core.Extensions;
using Arashi.Core.Util;
using Arashi.Web.Mvc.Controllers;
using Arashi.Web.Mvc.Filters;
using Arashi.Web.Mvc.Models;
using log4net;
using Arashi.Services.Themes;
using uNhAddIns.Pagination;
using Arashi.Web.Mvc.Paging;
using Arashi.Web.Areas.Admin.Models;

namespace Arashi.Web.Areas.Admin.Controllers
{
   public class ThemesController : SecureControllerBase
   {
      private static readonly ILog log = LogManager.GetLogger(typeof(ThemesController));
      private IThemeService themeService;
      private const int pageSize = 20;

      #region Constructor

      public ThemesController(IThemeService themeService)
      {
         this.themeService = themeService;
      }

      #endregion


      [PermissionFilter(RequiredRights = Rights.TemplatesView)]
      public ActionResult Index(int? page)
      {

         return View("Index", GetIndexData(page));
      }



      /// <summary>
      /// Set the selected template for the managed site
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
      [PermissionFilter(RequiredRights = Rights.TemplatesChange)]
      public ActionResult Change(int id, int? page)
      {
         Template selectedTemplate = themeService.GetById(id);

         Site managedSite = Context.ManagedSite;

         managedSite.Template = selectedTemplate;

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
         Paginator<Template> paginator;
         IPagedList<Template> pagedList = null;

         int currentPageIndex = 1;
         if (page.HasValue)
            currentPageIndex = page.Value;

         paginator = themeService.GetPaginatorForAll(pageSize);

         if (paginator.HasPages)
         {
            if (currentPageIndex > paginator.LastPageNumber)
               currentPageIndex = 1;

            IList<Template> pagesList = paginator.GetPage(currentPageIndex);

            pagedList = new PagedList<Template>(pagesList, page.HasValue ? page.Value - 1 : 0, pageSize, paginator.RowsCount.Value);
         }

         ThemesModel model = new ThemesModel()
         {
            Templates = pagedList,
            CurrentTemplate = Context.ManagedSite.Template
         };

         return model;
      }

   }
}
