using System;
using System.Web;
using System.Web.Mvc;

namespace Arashi.Web.Mvc.Filters
{
   /// <summary>
   /// This is an action filter to implement the "304 Not Modified filter"
   /// so that feeds that have not changed can return a 304 Not Modified HTTP Response -
   /// saving bandwidth and improving performance of the consumer.
   /// 
   /// See http://www.58bits.com/blog/ASPNET-MVC-304-Not-Modified-Filter-For-Syndication-Content.aspx
   /// </summary>
   public class NotModifiedFilterAttribute : ActionFilterAttribute
   {
      public override void OnResultExecuted(ResultExecutedContext filterContext)
      {
         var response = filterContext.HttpContext.Response;
         var request = filterContext.HttpContext.Request;

         if ((IsSourceModified(request, response) == false))
         {
            response.SuppressContent = true;
            response.StatusCode = 304;
            response.StatusDescription = "Not Modified";
            // Explicitly set the Content-Length header so the client doesn't wait for
            // content but keeps the connection open for other requests
            response.AddHeader("Content-Length", "0");
         }
      }



      private static bool IsSourceModified(HttpRequestBase request, HttpResponseBase response)
      {
         bool dateModified = false;
         bool eTagModified = false;

         string requestETagHeader = request.Headers["If-None-Match"] ?? string.Empty;
         string requestIfModifiedSinceHeader = request.Headers["If-Modified-Since"] ?? string.Empty;
         DateTime requestIfModifiedSince;
         DateTime.TryParse(requestIfModifiedSinceHeader, out requestIfModifiedSince);

         string responseETagHeader = response.Headers["ETag"] ?? string.Empty;
         string responseLastModifiedHeader = response.Headers["Last-Modified"] ?? string.Empty;
         DateTime responseLastModified;
         DateTime.TryParse(responseLastModifiedHeader, out responseLastModified);

         if (requestIfModifiedSince != DateTime.MinValue && responseLastModified != DateTime.MinValue)
         {
            if (responseLastModified > requestIfModifiedSince)
            {
               TimeSpan diff = responseLastModified - requestIfModifiedSince;
               if (diff > TimeSpan.FromSeconds(1))
               {
                  dateModified = true;
               }
            }
         }
         else
         {
            dateModified = true;
         }

         //Leave the default for eTagModified = false so that if we
         //don't get an ETag from the server we will rely on the fileDateModified only           
         if (String.IsNullOrEmpty(responseETagHeader) == false)
         {
            eTagModified = responseETagHeader.Equals(requestETagHeader, StringComparison.Ordinal) == false;
         }

         return (dateModified || eTagModified);
      }

   }
}
