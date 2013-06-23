namespace Arashi.Web.Mvc.Filters
{
   using System;
   using System.IO;
   using System.Security;
   using System.Text;
   using System.Threading;
   using System.Web;
   using System.Web.Mvc;
   using Arashi.Core;
   using Arashi.Core.Domain;
   using Arashi.Services.Localization;
   using Arashi.Core.Exceptions;
   using Arashi.Web.Mvc.Controllers;
   using Common.Logging;
   using Elmah;



   public class ContentExceptionFilter : HandleErrorAttribute
   {
      private ILog log = LogManager.GetCurrentClassLogger();

      private ILocalizationService localizationService = IoC.Resolve<ILocalizationService>();

      private string GlobalResource(string token)
      {
         return localizationService.GlobalResource(token, Thread.CurrentThread.CurrentUICulture);
      }


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
            return;

         if (!ExceptionType.IsInstanceOfType(exception))
            return;

         string controllerName = (string)filterContext.RouteData.Values["controller"];
         string actionName = (string)filterContext.RouteData.Values["action"];
         HandleErrorInfo model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);

         string viewPath = this.View;

         if (filterContext.Controller != null)
         {
            ContentControllerBase controllerBase = filterContext.Controller as ContentControllerBase;
            if (controllerBase != null)
            {
               Theme currentTheme;
               IRequestContext context = controllerBase.Context;
               if (context.CurrentSite != null)
               {
                  currentTheme = context.CurrentSite.Theme;

                  if (File.Exists(filterContext.HttpContext.Server.MapPath(currentTheme.BasePath + "/error.aspx")))
                     viewPath = currentTheme.BasePath + "/error.aspx";
               }
            }
         }


         // collect inner exceptions
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
            errorMessage =  "The url you entered is invalid or the site is down for maintenance.";
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

         if (filterContext.RequestContext.RouteData.Values.ContainsKey("siteid"))
            viewData["SiteId"] = filterContext.RequestContext.RouteData.Values["siteid"].ToString();


         // Render error view
         filterContext.Result = new ViewResult
                                   {
                                      ViewName = viewPath,
                                      MasterName = Master,
                                      ViewData = viewData,
                                      TempData = filterContext.Controller.TempData
                                   };

         filterContext.ExceptionHandled = true;
         filterContext.HttpContext.Response.Clear();
         filterContext.HttpContext.Response.StatusCode = statusCode;
         filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
      }



      #region ELMAH support

      private static bool RaiseErrorSignal(Exception e)
      {
         var context = HttpContext.Current;
         if (context == null)
            return false;
         var signal = ErrorSignal.FromContext(context);
         if (signal == null)
            return false;
         signal.Raise(e, context);
         return true;
      }



      private static bool IsFiltered(ExceptionContext context)
      {
         var config = context.HttpContext.GetSection("elmah/errorFilter") as ErrorFilterConfiguration;

         if (config == null)
            return false;

         var testContext = new ErrorFilterModule.AssertionHelperContext(context.Exception, HttpContext.Current);

         return config.Assertion.Test(testContext);
      }



      private static void LogException(Exception e)
      {
         var context = HttpContext.Current;
         ErrorLog.GetDefault(context).Log(new Error(e, context));
      }

      #endregion


   }
}