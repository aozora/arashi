namespace Arashi.Web.Areas.Admin.Controllers
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Web.Mvc;
   using Arashi.Core.Domain;
   using Arashi.Services.Localization;
   using Arashi.Services.Membership;
   using Arashi.Services.SiteStructure;
   using Arashi.Web.Areas.Admin.Models;
   using Arashi.Web.Mvc.Controllers;
   using Arashi.Web.Mvc.Filters;
   using Arashi.Web.Mvc.Models;
   using Arashi.Web.Mvc.Paging;
   using Common.Logging;
   using uNhAddIns.Pagination;


   public class RolesController : SecureControllerBase
   {
      private ILog log;
      private const int pageSize = 20;


      #region Constructor

      public RolesController(ILog log, ILocalizationService localizationService, IUserService userService, ISiteService siteService)
         : base(log, localizationService, userService, siteService)
      {
         this.log = log;
      }

      #endregion



      /// <summary>
      /// Users list
      /// </summary>
      /// <returns></returns>
      [PermissionFilter(RequiredRights = Rights.RolesView)]
      public ActionResult Index(int? page)
      {
         IList<Role> pages = null;
         Paginator<Role> paginator;
         IPagedList<Role> pagedList = null;

         int currentPageIndex = 1;
         if (page.HasValue)
            currentPageIndex = page.Value;

         paginator = userService.GetRolePaginatorBySite(Context.ManagedSite, pageSize);

         if (paginator.HasPages)
         {
            if (currentPageIndex > paginator.LastPageNumber)
               currentPageIndex = 1;

            pages = paginator.GetPage(currentPageIndex);

            pagedList = new PagedList<Role>(pages, page.HasValue ? page.Value - 1 : 0, pageSize, paginator.RowsCount.Value);// , totalcount
         }

         return View("Index", pagedList);
      }



      /// <summary>
      /// Show the user edit view
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
      [PermissionFilter(RequiredRights = Rights.RolesView)]
      public ActionResult Edit(int id)
      {
         RoleModel model = new RoleModel()
                              {
                                 Role = userService.GetRoleById(id),
                                 AllRights = userService.GetAllRights()
                                                .OrderBy(r => r.RightGroup)
                                                .ThenBy(r => r.Name)
                                                .ToList()
                              };

         return View("Edit", model);
      }



      [AcceptVerbs(HttpVerbs.Post)]
      [PermissionFilter(RequiredRights = Rights.RolesEdit)]
      [ValidateAntiForgeryToken()]
      public ActionResult Update(int id, string name, int[] rightIds)
      {
         Role role = userService.GetRoleById(id);
			
         // Clear existing roles
         role.Rights.Clear();

         try
         {
            UpdateModel(role, new[] {"Name"});

            if (rightIds != null && rightIds.Length > 0)
            {
               IList<Right> rights = userService.GetRightsByIds(rightIds);
               foreach (Right right in rights)
               {
                  log.DebugFormat("RightId {0}", right.Id);
                  role.Rights.Add(right);
               }
            }


            userService.UpdateRole(role);

            // Show the confirmation message
            MessageModel message = new MessageModel
            {
               Text = "Role saved successfully!",
               Icon = MessageModel.MessageIcon.Info,
               CssClass = "margin-topbottom", 
               IsClosable = true
            };
            RegisterMessage(message, true);

            return RedirectToAction("Index");
         }
         catch (Exception ex)
         {
            log.Error("RolesController.Update", ex);

            MessageModel message = new MessageModel
                                    {
                                       Text = GlobalResource("Message_GenericError"),
                                       Icon = MessageModel.MessageIcon.Alert,
                                       CssClass = "margin-topbottom"
                                    };
            RegisterMessage(message);
         }

         RoleModel model = new RoleModel()
         {
            Role = userService.GetRoleById(id),
            AllRights = userService.GetAllRights()
         };

         return View("Edit", model);
      }



      [PermissionFilter(RequiredRights = Rights.RolesEdit)]
      public ActionResult NewRole()
      {
         RoleModel model = new RoleModel()
         {
            Role = new Role(),
            AllRights = userService.GetAllRights()
         };

         return View("NewRole", model);
      }



      [AcceptVerbs(HttpVerbs.Post)]
      [PermissionFilter(RequiredRights = Rights.RolesEdit)]
      [ValidateAntiForgeryToken()]
      public ActionResult Create(int id, string name, int[] rightIds)
      {
         Role role = new Role();
         role.Site = Context.ManagedSite;

         try
         {
            UpdateModel(role, new[] { "Name" });

            if (rightIds != null && rightIds.Length > 0)
            {
               IList<Right> rights = userService.GetRightsByIds(rightIds);
               foreach (Right right in rights)
               {
                  log.DebugFormat("RightId {0}", right.Id);
                  role.Rights.Add(right);
               }
            }


            userService.UpdateRole(role);

            // Show the confirmation message
            MessageModel message = new MessageModel
            {
               Text = "Role created successfully!",
               Icon = MessageModel.MessageIcon.Info,
               CssClass = "margin-topbottom",
               IsClosable = true
            };
            RegisterMessage(message, true);

            return RedirectToAction("Index");
         }
         catch (Exception ex)
         {
            log.Error("RolesController.Create", ex);

            MessageModel message = new MessageModel
            {
               Text = GlobalResource("Message_GenericError"),
               Icon = MessageModel.MessageIcon.Alert,
               CssClass = "margin-topbottom"
            };
            RegisterMessage(message);
         }

         RoleModel model = new RoleModel()
         {
            Role = userService.GetRoleById(id),
            AllRights = userService.GetAllRights()
         };

         return View("NewRole", model);
      }

   }
}