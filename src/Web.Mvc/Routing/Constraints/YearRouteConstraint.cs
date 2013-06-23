using System;
using System.Globalization;
using System.Web;
using System.Web.Routing;

namespace Arashi.Web.Mvc.Routing.Constraints
{
   public class YearRouteConstraint : IRouteConstraint
   {
      #region IRouteConstraint Members

      public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
      {
         if ((routeDirection == RouteDirection.IncomingRequest) &&
             (parameterName.ToLower(CultureInfo.InvariantCulture) == "year"))
         {
            try
            {
               int year = Convert.ToInt32(values["year"]);

               if ((year >= 1900) && (year <= 2100))
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
