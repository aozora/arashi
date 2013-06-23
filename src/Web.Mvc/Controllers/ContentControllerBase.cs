namespace Arashi.Web.Mvc.Controllers
{
   using System;
   using System.Collections.Generic;
   using System.Globalization;
   using System.Threading;
   using System.Web.Mvc;
   using System.Web.Routing;

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
   using Arashi.Web.Mvc.Filters;
   using Arashi.Web.Mvc.Models;
   using Arashi.Web.Mvc.TemplateEngine;
   using Arashi.Web.Widgets;

   using Castle.MicroKernel;

   using log4net;

   /// <summary>
   /// Base class for frontend cms module controller
   /// </summary>
   [MaintenanceFilter(Order = 0)]
   [SiteFilter(Order = 1)]
   [ContentExceptionFilter(View = "~/Content/Views/Error.aspx", Order = 2)]
   [LocalizationFilter(Order = 3)]
   public abstract class ContentControllerBase : Controller
   {
      #region Private & protected fields

      private static readonly ILog log = LogManager.GetLogger(typeof(ContentControllerBase));
      protected readonly ISiteService siteService;
      protected readonly IUserService userService;
      protected readonly ICategoryService categoryService;
      protected readonly ITagService tagService;
      protected readonly IContentItemService<Post> contentItemService;
      protected readonly IContentItemService<Arashi.Core.Domain.Page> contentItemServiceForPage;
      protected readonly ICommentService commentService;
      protected readonly ISearchService searchService;
      protected readonly IWidgetService widgetService;
      protected readonly ILocalizationService localizationService;
      protected readonly IMessageService messageService;

      private readonly CultureInfo currentCulture;
      private IRequestContext requestContext;
      private readonly IList<IWidgetComponent> registeredWidgetComponents;
      #endregion

      #region Public Properties

      /// <summary>
      /// Get or sets the context.
      /// </summary>
      public IRequestContext Context
      {
         get
         {
            return requestContext;
         }
         set
         {
            requestContext = value;
         }
      }

      #endregion

      #region Constructor

      /// <summary>
      /// Constructor
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
      protected ContentControllerBase( ILocalizationService localizationService,
                                       ISiteService siteService,
                                       IUserService userService,
                                       IContentItemService<Post> contentItemService,
                                       IContentItemService<Page> contentItemServiceForPage,
                                       ICommentService commentService,
                                       ICategoryService categoryService,
                                       ITagService tagService,
                                       ISearchService searchService,
                                       IWidgetService widgetService,
                                       IMessageService messageService)
      {
         this.localizationService = IoC.Resolve<ILocalizationService>();
         this.userService = IoC.Resolve<IUserService>();
         this.siteService = IoC.Resolve<ISiteService>();
         this.categoryService = IoC.Resolve<ICategoryService>();
         this.tagService = IoC.Resolve<ITagService>();
         this.contentItemService = contentItemService;
         this.contentItemServiceForPage = contentItemServiceForPage;
         this.commentService = commentService;
         this.searchService = searchService;
         this.widgetService = widgetService;
         this.messageService = messageService;

         registeredWidgetComponents = new List<IWidgetComponent>();

         currentCulture = Thread.CurrentThread.CurrentUICulture;
      }

      #endregion

      /// <summary>
      /// Override the default Initialize method in order to use the CookieTempDataProvider
      /// instead of the default SessionStateTempDataProvider.
      /// </summary>
      /// <param name="context"></param>
      /// <remarks>
      /// Be careful! Here the ISession isn't available!!!
      /// </remarks>
      protected override void Initialize(RequestContext context)
      {
         this.TempDataProvider = new CookieTempDataProvider(context.HttpContext);

         base.Initialize(context);
      }



      /// <summary>
      /// This must be called by all content actions.
      /// This method ensure that all IWidgetComponent are registered and initialized
      /// </summary>
      /// <param name="model"></param>
      /// <returns></returns>
      protected ViewResult ViewContent(TemplateContentModel model)
      {
         RegisterWidgetComponents();

         model.WidgetComponents = registeredWidgetComponents;

         return View("~/Templates/master.aspx", model);
      }



      /// <summary>
      /// Show the error page; check if the template has an error page, otherwise use the default
      /// </summary>
      /// <returns></returns>
      protected ViewResult ViewError(string errorTitle, string errorMessage)
      {
         string templateErrorPagePath = Context.CurrentSite.Template.BasePath + "/error.aspx";
         const string defaultErrorPagePath = "~/Content/Views/Error.aspx";

         // Set the ViewData
         ViewData["ErrorTitle"] = errorTitle;
         ViewData["ErrorMessage"] = errorMessage;  

         if (System.IO.File.Exists(Server.MapPath(templateErrorPagePath)))
            return View(templateErrorPagePath);
         else
            return View(defaultErrorPagePath);
      }



      /// <summary>
      /// Register and initialize all the WidgetComponent for the current site
      /// </summary>
      protected void RegisterWidgetComponents()
      {
         log.Debug("ContentControllerBase.RegisterWidgetComponents: start");

         IKernel kernel = IoC.Container.Kernel;

         IList<Widget> widgets = widgetService.GetWidgetsBySite(Context.CurrentSite);
         log.DebugFormat("ContentControllerBase.RegisterWidgetComponents: found {0} widgets", widgets.Count.ToString());

         foreach (Widget widget in widgets)
         {
            string key = "widgetcomponent." + widget.Type.ClassName;

            IWidgetComponent component = null;

            if (kernel.HasComponent(key))
            {
               try
               {
                  component = (IWidgetComponent)kernel[key];
               }
               catch (Exception ex)
               {
                  log.ErrorFormat("ContentControllerBase.RegisterWidgetComponents: Exception\n{0}", ex.ToString());
                  throw;
               }
            }
            else
            {
               log.InfoFormat("ContentControllerBase.RegisterWidgetComponents: the component with key {0} is not present in the IoC container!");
            }

            if (component != null)
            {
               component.Context = this.Context;
               component.Widget = widget;
               component.Init();

               registeredWidgetComponents.Add(component);
            }
            else
            {
               log.DebugFormat("component null for key: {0}", key);
            }

         }

         log.Debug("ContentControllerBase.RegisterWidgetComponents: end");
      }



      //#region Registered Messages

      ///// <summary>
      ///// Register a message to be rendered
      ///// </summary>
      ///// <param name="model"></param>
      //protected void RegisterMessage(MessageModel model)
      //{
      //   RegisterMessage(model, false);
      //}



      ///// <summary>
      ///// Register a message to be rendered
      ///// </summary>
      ///// <param name="model"></param>
      ///// <param name="isPersistant">Set to true if the message should be persisted between to actions</param>
      //protected void RegisterMessage(MessageModel model, bool isPersistant)
      //{
      //   if (isPersistant)
      //   {
      //      persistentMessages.Add((model));
      //      TempData["PersistentMessages"] = persistentMessages;
      //   }
      //   else
      //   {
      //      registeredMessages.Add(model);
      //      ViewData["RegisteredMessages"] = registeredMessages;
      //   }
      //}



      //#endregion




      /// <summary>
      /// Get a new instance of TemplateContentModel with the common properties 
      /// already setted (Posts, CurrentCategory, CurrentTag, CurrentAuthor, ViewType & ViewMode are excluded).
      /// </summary>
      /// <returns></returns>
      protected TemplateContentModel GetDefaultTemplateContentModel()
      {
         IDtoService dtoService = IoC.Resolve<IDtoService>();

         TemplateContentDTO dto = dtoService.GetTemplateContentDTO(Context.CurrentSite);


         TemplateContentModel model = new TemplateContentModel
         {
            Site = Context.CurrentSite,
            Categories = dto.Categories,
            Tags = dto.Tags,
            TagCloud = dto.TagCloud,
            Calendar = dto.Calendar,
            Pages = dto.Pages,
            RecentComments = dto.RecentComments,
            RecentPosts = dto.RecentPosts
         };

         return model;
      }



      /// <summary>
      /// Render the template 404 view
      /// </summary>
      /// <returns></returns>
      protected ActionResult Render404()
      {
         TemplateContentModel model = GetDefaultTemplateContentModel();
         model.TemplateFile = ViewHelper.TemplateFile._404;

         this.ControllerContext.HttpContext.Response.StatusCode = 404;

         return ViewContent(model);
      }


      #region Utils

      /// <summary>
      /// Return a virtual path (with ~) for a given view in the current site's template folder
      /// </summary>
      /// <param name="templateFile"></param>
      /// <returns></returns>
      public string GetViewVirtualPath(ViewHelper.TemplateFile templateFile)
      {
         if (Context.CurrentSite.Template != null)
         {
            return string.Concat(Context.CurrentSite.Template.BasePath,
                                                    "/",
                                                    templateFile.ToString().Replace("_", ""),
                                                    ".ascx");
         }

         return templateFile.ToString();
      }

      #endregion

      #region Localization

      /// <summary>
      /// Get a localized resource specific for a theme given its token
      /// </summary>
      /// <param name="token"></param>
      /// <returns></returns>
      protected string ThemeResource(string token)
      {
         return localizationService.ThemeResource(token, currentCulture);
      }

      #endregion


   }
}
