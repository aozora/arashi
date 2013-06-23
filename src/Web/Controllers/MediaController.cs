using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Arashi.Core.Domain;
using Arashi.Core.Util;
using Arashi.Services.Content;
using Arashi.Services.Localization;
using Arashi.Services.Membership;
using Arashi.Services.Search;
using Arashi.Services.SiteStructure;
using Arashi.Services.Widget;
using Arashi.Web.Helpers;
using Arashi.Web.Mvc.Controllers;
using Arashi.Web.Mvc.Filters;
using Common.Logging;

namespace Arashi.Web.Controllers
{
   using Arashi.Services.Notification;

   /// <summary>
   /// Controller for all media files requested by the front end pages
   /// </summary>
   [SeoUrlCanonicalization]
   public class MediaController : ContentControllerBase
   {
      private ILog log;

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
      public MediaController( ILog log, 
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


      /// <summary>
      /// Return a <see cref="FileResult"/> that map to a physical file on the file system
      /// </summary>
      /// <param name="name"></param>
      /// <returns></returns>
      public FileResult GetMedia(string name)
      {
         Site site = Context.CurrentSite;

         //due to the fact that this action can be called from the admin and from the frontend, determine the correct site

         // if the current uri refers to an Admin page....
         if (Request.Url.GetLeftPart(UriPartial.Path).Substring(Request.Url.ToString().IndexOf(WebHelper.GetHostName())).IndexOf("/Admin/") > -1)
            site = Context.ManagedSite;

         string mediaRoot = site.SiteDataPath + "media/";
         string mediaRootFullPath = this.ControllerContext.HttpContext.Server.MapPath(mediaRoot);

         string contentType = MimeTypes.GetMimeTypeName(name.Substring(name.LastIndexOf('.') + 1));

         return base.File(Path.Combine(mediaRootFullPath, name), contentType);
      }

   }
}
