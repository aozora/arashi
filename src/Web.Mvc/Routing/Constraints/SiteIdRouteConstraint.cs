using System;
using System.Globalization;
using System.Web;
using System.Web.Routing;

namespace Arashi.Web.Mvc.Routing.Constraints
{
   /// <summary>
   /// Route Constraint for SiteId 
   /// </summary>
   public class SiteIdConstraint : IRouteConstraint
   {
      #region IRouteConstraint Members

      public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
      {
         if ((routeDirection == RouteDirection.IncomingRequest) &&
             (parameterName.ToLower(CultureInfo.InvariantCulture) == "siteid"))
         {
            try
            {
               int year = Convert.ToInt32(values["siteid"]);
               return true;
            }
            catch
            {
               return false;
            }
         }

         return false;
      }

      #endregion

   }
}
