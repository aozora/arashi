namespace Arashi.Web.Controllers
{
   using System;
   using System.Web.Mvc;

   using Arashi.Core;
   using Arashi.Core.Domain;
   using Arashi.Core.Domain.Dto;
   using Arashi.Services.Content;
   using Arashi.Services.Localization;
   using Arashi.Services.Membership;
   using Arashi.Services.Notification;
   using Arashi.Services.Search;
   using Arashi.Services.SiteStructure;
   using Arashi.Services.Widget;
   using Arashi.Web.Mvc.Controllers;
   using Arashi.Web.Mvc.Filters;
   using Arashi.Web.Mvc.Models;
   using Arashi.Web.Mvc.TemplateEngine;

   using Common.Logging;



   /// <summary>
   /// Maintenance Controller
   /// See Arashi.Web.Mvc.Filters.MaintenanceFilter
   /// 
   /// The maintenance mode is activated in the web.config, with a whitelist of IP that can always connect to the app.
   /// </summary>
   [SeoUrlCanonicalization]
   public class MaintenanceController : ContentControllerBase
   {
      private ILog log;

      #region Constructor

      /// <summary>
      /// MaintenanceController
      /// </summary>
      /// <param name="localizationService"></param>
      /// <param name="siteService"></param>
      /// <param name="userService"></param>
      /// <param name="contentItemService"></param>
      /// <param name="contentItemServiceForPage"></param>
      /// <param name="commentService"></param>
      /// <param name="categoryService"></param>
      /// <param name="tagService"></param>
      /// <param name="searchService"></param>
      /// <param name="widgetService"></param>
      /// <param name="messageService"></param>
      public MaintenanceController(ILog log, 
                              ILocalizationService localizationService,
                              ISiteService siteService,
                              IUserService userService,
                              IContentItemService<Post> contentItemService,
                              IContentItemService<Arashi.Core.Domain.Page> contentItemServiceForPage,
                              ICommentService commentService,
                              ICategoryService categoryService,
                              ITagService tagService,
                              ISearchService searchService,
                              IWidgetService widgetService,
                              IMessageService messageService)
         : base(log, localizationService, siteService, userService, contentItemService, contentItemServiceForPage, commentService, categoryService, tagService, searchService, widgetService, messageService)
      {
         this.log = log;
      }

      #endregion

      public ActionResult Index()
      {
         IDtoService dtoService = IoC.Resolve<IDtoService>();
         TemplateContentDTO dto = dtoService.GetTemplateContentDTO(Context.CurrentSite);

         TemplateContentModel model = new TemplateContentModel
                                         {
                                            Site = Context.CurrentSite,
                                            Categories = dto.Categories,
                                            Tags = dto.Tags, 
                                            TemplateFile = ViewHelper.TemplateFile._302
                                         };

         return ViewContent(model);
      }

   }
}
