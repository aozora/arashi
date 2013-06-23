using System;
using System.Security;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Arashi.Core.Exceptions;
using Elmah;
using log4net;

namespace Arashi.Web.Mvc.Filters
{
   /// <summary>
   /// This filter is used only for the admin controllers
   /// </summary>
   public class ExceptionFilter : HandleErrorAttribute
   {
      private static readonly ILog log = LogManager.GetLogger(typeof(ExceptionFilter));

      // TODO: usare locale resources

      /// <summary>
      /// Filter attribute that allows for more graceful error handling, 
      /// including localized exceptions.
      /// </summary>
      /// <param name="filterContext"></param>
      public override void OnException(ExceptionContext filterContext)
      {
         if (filterContext == null)
            throw new ArgumentNullException("filterContext");

         // Log with ELMAH
         LogException(filterContext.Exception);


         // If custom errors are disabled, we need to let the normal ASP.NET exception handler
         // execute so that the user can see useful debugging information.
         if (filterContext.ExceptionHandled || !filterContext.HttpContext.IsCustomErrorEnabled)
            return;

         string errorTitle;
         string errorMessage;
         StringBuilder debugInfo = new StringBuilder();
         int statusCode;

         Exception exception = filterContext.Exception;

         // If this is not an HTTP 500 (for example, if somebody throws an HTTP 404 from an action method),
         // ignore it.
         if (new HttpException(null, exception).GetHttpCode() != 500)
         {
            return;
         }

         if (!ExceptionType.IsInstanceOfType(exception))
         {
            return;
         }

         string controllerName = (string)filterContext.RouteData.Values["controller"];
         string actionName = (string)filterContext.RouteData.Values["action"];
         HandleErrorInfo model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);

         while (exception.InnerException != null)
         {
            debugInfo.AppendLine("[INNER EXCEPTION]<br />");
            debugInfo.AppendLine(exception.ToString());
            exception = exception.InnerException;
         }



         // Specific http error code
         if (exception is SiteNullException)
         {
            statusCode = 503;
            errorTitle = exception.Message;
            errorMessage = "The url you entered is invalid, the hostname is not registered or the site is down for maintenance.";
         }
         else if (exception is PageNullException)
         {
            statusCode = 404;
            errorTitle = "404 Page not found";
            errorMessage = @"Sorry, but the page you were trying to get to does not exist.";

            Uri currentUrl = filterContext.HttpContext.Request.Url;
            Uri referrerUrl = filterContext.RequestContext.HttpContext.Request.UrlReferrer;

            // Se l'host dell'url corrente è lo stesso del referrer,
            // vuol dire che l'errore 404 è generato da un link errato presente
            // sull'host corrente
            if (currentUrl.Host == referrerUrl.Host)
            {
               log.Error("BROKEN LINK DETECTED: " + currentUrl.ToString());
               errorMessage = @"Sorry, but the page you were trying to get to does not exist.
<br />
Apparently, we have a broken link on our page. An e-mail has just been sent to the person who can fix this and it should be corrected shortly. No further action is required on your part.";
            }
            else
            {
               errorMessage = @"Sorry, but the page you were trying to get to does not exist.
   <br />
   It looks like this was the result of either
   <br />
      * a mistyped address
      * or an out-of-date bookmark in your web browser.
   <br />
   You may want to try searching this site or using our site map to find what you were looking for.";
            }
         }
         else if (exception is SecurityException)
         {
            statusCode = 403;
            errorTitle = "403 Access forbidden";
            errorMessage = "You are not authorized to access the requested resource.";
         }
         else
         {
            statusCode = 500;
            errorTitle = "Error!";  // "500 An error occured:";
            errorMessage = "We are sorry for the inconvenience, an unexpected error occured.<br />";
         }


         // log the error
         log.ErrorFormat("Exception in Controller {0}, Action {1}", controllerName, actionName);
         log.Error(errorMessage);
         log.Error(exception.ToString());

         // prepare data for rendering
         var viewData = new ViewDataDictionary<HandleErrorInfo>(model);
         viewData["ErrorTitle"] = errorTitle;
         viewData["ErrorMessage"] = errorMessage;
         viewData["DebugInfo"] = debugInfo;
         viewData["ErrorCode"] = statusCode;
         viewData["SiteId"] = filterContext.RequestContext.RouteData.Values["siteid"] == null ? null : filterContext.RequestContext.RouteData.Values["siteid"].ToString();
         
         // Render error view
         filterContext.Result = new ViewResult
                                   {
                                      ViewName = View,
                                      MasterName = Master,
                                      ViewData = viewData,
                                      TempData = filterContext.Controller.TempData
                                   };

         filterContext.ExceptionHandled = true;
         filterContext.HttpContext.Response.Clear();
         filterContext.HttpContext.Response.StatusCode = statusCode;
         filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
      }



      private static void LogException(Exception e)
      {
         var context = HttpContext.Current;
         ErrorLog.GetDefault(context).Log(new Error(e, context));
      }


   }
}