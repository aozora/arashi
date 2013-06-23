using System.Web.Mvc;
using Arashi.Web.Mvc.Routing.Constraints;

namespace Arashi.Web.Areas.Admin
{
   /// <summary>
   /// ASP.NET MVC 2 Area Registration for Admin Area
   /// </summary>
   public class AdminAreaRegistration : AreaRegistration
   {
      public override string AreaName
      {
         get
         {
            return "Admin";
         }
      }



      public override void RegisterArea(AreaRegistrationContext context)
      {
         //context.MapRoute(
         //    "Admin_default",
         //    "Admin/{controller}/{action}/{id}",
         //    new
         //    {
         //       action = "Index",
         //       id = UrlParameter.Optional
         //    }
         //);


         // Also put the login functionality in the manager area.
         context.MapRoute(null, "Admin/Login/Authenticate", new
         {
            action = "Authenticate",
            controller = "Login"
         });
         context.MapRoute(null, "Admin/Login/{action}", new
         {
            action = "Index",
            controller = "Login"
         });
         context.MapRoute(null, "Admin/Home", new
         {
            action = "Index",
            controller = "Home"
         });

         context.MapRoute("GetThumbnail", "Admin/Images/Thumbnail/{path}/{width}/{height}", new
         {
            action = "Thumbnail",
            controller = "Images",
            path = "",
            width = "",
            height = ""
         });
         context.MapRoute("MediaManager", "Admin/{siteid}/Media/{name}", new
         {
            action = "GetMedia",
            controller = "MediaManager",
            name = ""
         });

         // Default route for ControlPanel
         context.MapRoute(null, "Admin/{siteid}/{Controller}/{action}/{id}", new
         {
            action = "Index",
            controller = "Site",
            id = ""
         });

         //context.MapRoute(null, "Admin/{Controller}/{action}/{id}", new
         //{
         //   action = "Index",
         //   controller = "Site",
         //   id = UrlParameter.Optional
         //});


      }
   }
}
