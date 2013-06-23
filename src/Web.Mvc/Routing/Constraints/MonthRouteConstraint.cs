using System;
using System.Globalization;
using System.Web;
using System.Web.Routing;

namespace Arashi.Web.Mvc.Routing.Constraints
{
   public class MonthRouteConstraint : IRouteConstraint
   {
      #region IRouteConstraint Members

      public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
      {
         if ((routeDirection == RouteDirection.IncomingRequest) &&
             (parameterName.ToLower(CultureInfo.InvariantCulture) == "month"))
         {
            try
            {
               int month = Convert.ToInt32(values["month"]);

               if ((month >= 1) && (month <= 12))
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
