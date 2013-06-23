namespace Arashi.Web.Mvc.Themes
{
   using System.Web.Mvc;
   using System.Web.Routing;

   public interface IThemeRoutes
   {
      void RegisterRoutes(RouteCollection routes);
   }
}
