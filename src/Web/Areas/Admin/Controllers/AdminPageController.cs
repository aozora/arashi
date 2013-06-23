using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Web.Mvc;
using Arashi.Core.Domain;
using Arashi.Core.Extensions;
using Arashi.Core.Util;
using Arashi.Services.Content;
using Arashi.Services.Membership;
using Arashi.Web.Areas.Admin.Models;
using Arashi.Web.Mvc.Controllers;
using Arashi.Web.Mvc.Filters;
using Arashi.Web.Mvc.Models;
using Arashi.Web.Mvc.Paging;
using log4net;
using uNhAddIns.Pagination;

namespace Arashi.Web.Areas.Admin.Controllers
{
   using Arashi.Web.Mvc.TemplateEngine;

   public class AdminPageController : SecureControllerBase
   {
      private static readonly ILog log = LogManager.GetLogger(typeof(AdminPageController));
      private const int pageSize = 20;
      private readonly IContentItemService<Page> contentItemService;
      private readonly IPageService pageService;


      #region Constructor

      public AdminPageController(IContentItemService<Page> contentItemService,
                                 ICategoryService categoryService,
                                 ITagService tagService,
                                 IPageService pageService)
      {
         this.contentItemService = contentItemService;
         this.pageService = pageService;
      }

      #endregion

      /// <summary>
      /// Show the page list view
      /// </summary>
      /// <param name="page"></param>
      /// <param name="status"></param>
      /// <returns></returns>
      [PermissionFilter(RequiredRights = Rights.PagesView)]
      public ActionResult Index(int? page, string status)
      {
         Paginator<Page> paginator;
         IPagedList<Page> pagedList = null;

         int currentPageIndex = 1;
         if (page.HasValue)
            currentPageIndex = page.Value;

         if (string.IsNullOrEmpty(status))
            paginator = contentItemService.GetPaginatorBySite(Context.ManagedSite, pageSize, "Position", true);
         else
            paginator = contentItemService.GetPaginatorBySiteAndWorkflowStatus(Context.ManagedSite, (WorkflowStatus)Enum.Parse(typeof(WorkflowStatus), status), pageSize, "Position", true);


         if (paginator.HasPages)
         {
            if (currentPageIndex > paginator.LastPageNumber)
               currentPageIndex = 1;

            IList<Page> pagesList = paginator.GetPage(currentPageIndex);

            pagedList = new PagedList<Page>(pagesList, page.HasValue ? page.Value - 1 : 0, pageSize, paginator.RowsCount.Value);// , totalcount
         }

         ViewData["WorkflowStatusDictionary"] = base.GetLocalizedEnumList(typeof(WorkflowStatus));
         ViewData["WorkflowStatus_Current"] = status;

         return View("Index", pagedList);
      }




      /// <summary>
      /// Save a new sort order for the pages
      /// </summary>
      /// <param name="ordereditems"></param>
      /// <returns></returns>
      [AcceptVerbs(HttpVerbs.Post)]
      [PermissionFilter(RequiredRights = Rights.PagesEdit)]
      public ActionResult Sort(string ordereditems)
      {
         string[] pageIds = ordereditems.Split(',');

         try
         {
            pageService.Sort(pageIds);
         }
         catch (Exception ex)
         {
            log.Error("AdminPageController.Sort", ex);

            MessageModel model = new MessageModel
            {
               Text = GlobalResource("Message_GenericError"),
               Icon = MessageModel.MessageIcon.Alert,
            };
            RegisterMessage(model);
         }

         return Content("OK");
      }


      #region New Page

      /// <summary>
      /// Show the new page view
      /// </summary>
      /// <returns></returns>
      [PermissionFilter(RequiredRights = Rights.PagesEdit)]
      public ActionResult NewPage()
      {
         PageModel model = new PageModel();

         model.Page = new Page();

         // Set defaults based on Site settings
         model.Page.Site = Context.ManagedSite;

         // Get the full WorkflowStatus enum list, and remove the current page status value:
         IDictionary<String, String> statusList = GetLocalizedEnumList(typeof(WorkflowStatus));
         model.WorkflowStatus = new SelectList(statusList, "Key", "Value");
         model.WorkflowStatus = new SelectList(statusList, "Key", "Value", "Published");
         // Trick to force the selecteditem ( see http://blog.benhartonline.com/post/2008/11/24/ASPNET-MVC-SelectList-selectedValue-Gotcha.aspx )
         ViewData["WorkflowStatus"] = "Published";

         ViewData["MonthsList"] = new SelectList(DateUtil.MonthNames(Thread.CurrentThread.CurrentUICulture, true), "Key", "Value", DateTime.Now.Month);

         // Trick to force the selecteditem ( see http://blog.benhartonline.com/post/2008/11/24/ASPNET-MVC-SelectList-selectedValue-Gotcha.aspx )
         ViewData["Month"] = DateTime.Now.Month;

         model.CustomTemplateFiles = GetCustomTemplateFiles(string.Empty);
         model.ParentPages = GetParentPages(true, model.Page);

         return View("NewPage", model);
      }



      /// <summary>
      /// Save a new page
      /// </summary>
      /// <param name="page"></param>
      /// <param name="month"></param>
      /// <param name="day"></param>
      /// <param name="year"></param>
      /// <param name="hour"></param>
      /// <param name="minute"></param>
      /// <returns></returns>
      [AcceptVerbs(HttpVerbs.Post)]
      [ValidateInput(false)]
      [ValidateAntiForgeryToken]
      [PermissionFilter(RequiredRights = Rights.PagesEdit)]
      public ActionResult SaveNew([Bind(Exclude = "Id,ParentPage")] Page page, int month, int day, int year, int hour, int minute, string WorkflowStatus, string ParentPage)
      {
         try
         {
            Page parentPage = null;

            if (!string.IsNullOrEmpty(ParentPage))
               parentPage = contentItemService.GetById(Convert.ToInt32(ParentPage));

            UpdateModel(page, "", new[] { "Title", "Content", "CustomTemplateFile" });

            // set the sanitized friendlytitle
            page.FriendlyName = page.Title.Sanitize();
            page.PublishedDate = new DateTime(year, month, day, hour, minute, 0);
            page.ParentPage = parentPage;

            if (!string.IsNullOrEmpty(WorkflowStatus))
               page.WorkflowStatus = (Arashi.Core.Domain.WorkflowStatus)Enum.Parse(typeof(Arashi.Core.Domain.WorkflowStatus), WorkflowStatus);

            if (page.Content == null)
               page.Content = string.Empty;

            SaveOrUpdate(page);

            return RedirectToAction("Index");
         }
         catch (Exception ex)
         {
            log.Error("AdminPageController.SaveNew", ex);

            if (this.ModelState != null)
            {
               foreach (string key in this.ModelState.Keys)
               {
                  if (this.ModelState[key].Errors.Count > 0)
                  {
                     foreach (ModelError modelError in this.ModelState[key].Errors)
                     {
                        if ((modelError.Exception != null))
                           log.ErrorFormat("Error in Model[\"{0}\"]: {1}", key, modelError.Exception.ToString());
                        else if (!string.IsNullOrEmpty(modelError.ErrorMessage))
                           log.Error(modelError.ErrorMessage);
                     }
                  }
               }
            }

            MessageModel model = new MessageModel
            {
               Text = GlobalResource("Message_GenericError"),
               Icon = MessageModel.MessageIcon.Alert,
            };
            RegisterMessage(model, true);
         }

         return RedirectToAction("NewPage");
      }

      #endregion

      #region Edit Page

      [PermissionFilter(RequiredRights = Rights.PagesView)]
      public ActionResult Edit(int id)
      {
         PageModel model = new PageModel();

         model.Page = contentItemService.GetById(id);

         // Get the full WorkflowStatus enum list, and remove the current page status value:
         IDictionary<String, String> statusList = GetLocalizedEnumList(typeof(WorkflowStatus));
         model.WorkflowStatus = new SelectList(statusList, "Key", "Value");
         statusList.Remove(model.Page.WorkflowStatus.ToString());

         ViewData["MonthsList"] = new SelectList(DateUtil.MonthNames(Thread.CurrentThread.CurrentUICulture, true), "Key", "Value", model.Page.PublishedDate.HasValue ? model.Page.PublishedDate.Value.Month : DateTime.Now.Month);

         // Trick to force the selecteditem ( see http://blog.benhartonline.com/post/2008/11/24/ASPNET-MVC-SelectList-selectedValue-Gotcha.aspx )
         ViewData["Month"] = model.Page.PublishedDate.HasValue ? model.Page.PublishedDate.Value.Month : DateTime.Now.Month;

         model.CustomTemplateFiles = GetCustomTemplateFiles(model.Page.CustomTemplateFile);
         model.ParentPages = GetParentPages(true, model.Page);

         return View("Edit", model);
      }



      /// <summary>
      /// Update the current page
      /// </summary>
      /// <param name="id"></param>
      /// <param name="month"></param>
      /// <param name="day"></param>
      /// <param name="year"></param>
      /// <param name="hour"></param>
      /// <param name="minute"></param>
      /// <param name="WorkflowStatus"></param>
      /// <returns></returns>
      [AcceptVerbs(HttpVerbs.Post)]
      [ValidateInput(false)]
      [ValidateAntiForgeryToken]
      [PermissionFilter(RequiredRights = Rights.PagesEdit)]
      public ActionResult Update(int id, int month, int day, int year, int hour, int minute, string WorkflowStatus, string ParentPage)
      {
         Page page = contentItemService.GetById(id);
         Page parentPage = null;

         if (!string.IsNullOrEmpty(ParentPage))
            parentPage = contentItemService.GetById(Convert.ToInt32(ParentPage));

         try
         {
            UpdateModel(page, new[] { "Title", "Content", "CustomTemplateFile"});

            if (page.Content == null)
               page.Content = string.Empty;

            page.PublishedDate = new DateTime(year, month, day, hour, minute, 0);

            if (!string.IsNullOrEmpty(WorkflowStatus))
               page.WorkflowStatus = (Arashi.Core.Domain.WorkflowStatus)Enum.Parse(typeof(Arashi.Core.Domain.WorkflowStatus), WorkflowStatus);

            page.ParentPage = parentPage;

            SaveOrUpdate(page);

            MessageModel model = new MessageModel
            {
               Text = GlobalResource("Message_PageSaved"),
               Icon = MessageModel.MessageIcon.Alert,
            };
            RegisterMessage(model);
         }
         catch (Exception ex)
         {
            log.Error("AdminPageController.Update", ex);

            if (this.ModelState != null)
            {
               foreach (string key in this.ModelState.Keys)
               {
                  if (this.ModelState[key].Errors.Count > 0)
                  {
                     foreach (ModelError modelError in this.ModelState[key].Errors)
                     {
                        if ((modelError.Exception != null))
                           log.Error(modelError.Exception.ToString());
                        else if (!string.IsNullOrEmpty(modelError.ErrorMessage))
                           log.Error(modelError.ErrorMessage);
                     }
                  }
               }
            }

            MessageModel model = new MessageModel
            {
               Text = GlobalResource("Message_GenericError"),
               Icon = MessageModel.MessageIcon.Alert,
            };
            RegisterMessage(model, true);
         }

         return RedirectToAction("Edit", new { id = id });
      }

      #endregion

      #region Private Helpers

      /// <summary>
      /// Save or update a page
      /// </summary>
      /// <param name="page"></param>
      private void SaveOrUpdate(Page page)
      {
         // TODO: content filtering!!!

         page.Site = Context.ManagedSite;
         page.Culture = Context.ManagedSite.DefaultCulture;
         page.PublishedBy = Context.CurrentUser;
         //page.WorkflowStatus = WorkflowStatus.Published;

         // Store the published dates in UTC
         page.PublishedDate = page.PublishedDate.Value.ToUniversalTime();

         contentItemService.Save(page);

         // Send the pings
         //SendPings(page);

         // Show the confirmation message
         MessageModel model = new MessageModel
         {
            Text = GlobalResource("Message_PageSaved"),
            Icon = MessageModel.MessageIcon.Info,
         };

         RegisterMessage(model, true);
      }



      /// <summary>
      /// Get a SelectList filled with the names of all the available custom templates
      /// </summary>
      /// <param name="customTemplate"></param>
      /// <returns></returns>
      private SelectList GetCustomTemplateFiles(string customTemplate)
      {
         IList<string> customTemplateFiles = new List<string>();
         string systemTemplateFiles = string.Join(",", Enum.GetNames(typeof(ViewHelper.TemplateFile))).Replace("_", string.Empty);
         DirectoryInfo di = new DirectoryInfo(ControllerContext.HttpContext.Server.MapPath(Context.ManagedSite.Template.BasePath));
         FileInfo[] files = di.GetFiles("*.ascx");

         foreach (FileInfo fileInfo in files)
         {
            string name = fileInfo.Name.Substring(0, fileInfo.Name.IndexOf(fileInfo.Extension));
            if (systemTemplateFiles.IndexOf(name) == -1)
               customTemplateFiles.Add(name);
         }

         return new SelectList(customTemplateFiles, customTemplate);
      }



      ///// <summary>
      ///// Get a SelectList with the names (pseudo hyerarchical view) of all pages
      ///// </summary>
      ///// <param name="excludeCurrentPage"></param>
      ///// <param name="currentPage"></param>
      ///// <returns></returns>
      //private SelectList GetParentPages(bool excludeCurrentPage, Page currentPage)
      //{
      //   IList<Page> allPages = contentItemService.FindAllBySite(Context.ManagedSite, "Position", true);
      //   List<Page> tree = new List<Page>();

      //   IEnumerable<Page> topPages = from page in allPages
      //                                where page.ParentPage == null
      //                                select page;

      //   foreach (Page page in topPages)
      //   {
      //      IEnumerable<Page> flat = from p in page.AsDepthFirstEnumerable(x => x.ChildPages)
      //                               select p;
      //      tree.AddRange(flat);
      //   }

      //   SelectList list = new SelectList(tree, "Id", "DepthTitle", currentPage.ParentPage);
      //   return list;
      //}



      private IList<Page> GetParentPages(bool excludeCurrentPage, Page currentPage)
      {
         IList<Page> allPages = contentItemService.FindAllBySite(Context.ManagedSite, "Position", true);
         List<Page> tree = new List<Page>();

         IEnumerable<Page> topPages = from page in allPages
                                      where page.ParentPage == null
                                      select page;

         foreach (Page page in topPages)
         {
            IEnumerable<Page> flat = from p in page.AsDepthFirstEnumerable(x => x.ChildPages)
                                     select p;
            tree.AddRange(flat);
         }

         return tree;
      }

      #endregion
   }
}
