using System.Web.Routing;

namespace Arashi.Web.Mvc.WebForms
{
   public interface IMvcModule
   {
      /// <summary>
      /// Register additional routes to be added to the Route Table for the module
      /// </summary>
      /// <param name="routes"></param>
      void RegisterRoutes(RouteCollection routes);

      /// <summary>
      /// Register additional ViewLocation path to enable the ViewEngine to find
      /// the module specific views
      /// </summary>
      string[] RegisterViewLocationFormats();
   }
}
