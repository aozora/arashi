namespace Arashi.Web.Areas.MobileAdmin
{
   using System.Web.Mvc;



   public class MobileAdminAreaRegistration : AreaRegistration
   {
      public override string AreaName
      {
         get
         {
            return "MobileAdmin";
         }
      }



      public override void RegisterArea(AreaRegistrationContext context)
      {
         context.MapRoute(
             "MobileAdmin_default",
             "admin/mobile/{controller}/{action}/{id}",
             new { action = "Index", id = UrlParameter.Optional }
         );
      }
   }
}
