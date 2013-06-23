using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;
using System.Web;
using System.Web.Mvc;

namespace Arashi.Web.Mvc
{
   ///// <summary>
   ///// CookieTempDataProvider
   ///// </summary>
   [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
   [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
   public class CookieTempDataProvider : ITempDataProvider
   {
      // Fields
      private HttpContextBase _httpContext;
      internal const string TempDataCookieKey = "__ControllerTempData";



      // Methods
      public CookieTempDataProvider(HttpContextBase httpContext)
      {
         if (httpContext == null)
         {
            throw new ArgumentNullException("httpContext");
         }
         this._httpContext = httpContext;
      }



      /// <summary>
      /// Deserialize the tempdata dictionary
      /// </summary>
      /// <remarks>
      /// NOTE: this fix the issue on RedirectToAction, see: http://stackoverflow.com/questions/1326626/asp-net-mvc-cookietempdataprovider-deserializetempdata-returns-null
      /// </remarks>
      /// <param name="base64EncodedSerializedTempData"></param>
      /// <returns></returns>
      public static IDictionary<string, object> DeserializeTempData(string base64EncodedSerializedTempData)
      {
         MemoryStream memStream = new MemoryStream(Convert.FromBase64String(base64EncodedSerializedTempData));
         BinaryFormatter binFormatter = new BinaryFormatter();
         //return (binFormatter.Deserialize(memStream, null) as TempDataDictionary);
         return (binFormatter.Deserialize(memStream, null) as IDictionary<string, object>);
      }



      protected virtual IDictionary<string, object> LoadTempData(ControllerContext controllerContext)
      {
         HttpCookie cookie = this._httpContext.Request.Cookies["__ControllerTempData"];
         if ((cookie == null) || string.IsNullOrEmpty(cookie.Value))
         {
            return new Dictionary<string, object>();
         }
         IDictionary<string, object> deserializedTempData = DeserializeTempData(cookie.Value);
         cookie.Expires = DateTime.MinValue;
         cookie.Value = string.Empty;
         if (((this._httpContext.Response != null) && (this._httpContext.Response.Cookies != null)) && (this._httpContext.Response.Cookies["__ControllerTempData"] != null))
         {
            cookie.Expires = DateTime.MinValue;
            cookie.Value = string.Empty;
         }
         return deserializedTempData;
      }



      protected virtual void SaveTempData(ControllerContext controllerContext, IDictionary<string, object> values)
      {
         string cookieValue = SerializeToBase64EncodedString(values);
         HttpCookie cookie = new HttpCookie("__ControllerTempData")
         {
            HttpOnly = true,
            Value = cookieValue
         };
         this._httpContext.Response.Cookies.Add(cookie);
      }



      public static string SerializeToBase64EncodedString(IDictionary<string, object> values)
      {
         MemoryStream memStream = new MemoryStream();
         memStream.Seek(0L, SeekOrigin.Begin);
         new BinaryFormatter().Serialize(memStream, values);
         memStream.Seek(0L, SeekOrigin.Begin);
         return Convert.ToBase64String(memStream.ToArray());
      }



      IDictionary<string, object> ITempDataProvider.LoadTempData(ControllerContext controllerContext)
      {
         return this.LoadTempData(controllerContext);
      }



      void ITempDataProvider.SaveTempData(ControllerContext controllerContext, IDictionary<string, object> values)
      {
         this.SaveTempData(controllerContext, values);
      }



      // Properties
      public HttpContextBase HttpContext
      {
         get
         {
            return this._httpContext;
         }
      }
   }


   ///// <summary>
   ///// CookieTempDataProvider
   ///// </summary>
   //[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
   //[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
   //public class CookieTempDataProvider : ITempDataProvider
   //{
   //   internal const string TEMP_DATA_COOKIE_KEY = "__ControllerTempData";
   //   private readonly HttpContextBase _httpContext;

   //   public CookieTempDataProvider(HttpContextBase httpContext)
   //   {
   //      if (httpContext == null)
   //      {
   //         throw new ArgumentNullException("httpContext");
   //      }
   //      _httpContext = httpContext;
   //   }

   //   public HttpContextBase HttpContext
   //   {
   //      get
   //      {
   //         return _httpContext;
   //      }
   //   }

   //   #region ITempDataProvider Members

   //   IDictionary<string, object> ITempDataProvider.LoadTempData(ControllerContext controllerContext)
   //   {
   //      return LoadTempData(controllerContext);
   //   }

   //   void ITempDataProvider.SaveTempData(ControllerContext controllerContext, IDictionary<string, object> values)
   //   {
   //      SaveTempData(controllerContext, values);
   //   }

   //   #endregion

   //   protected virtual IDictionary<string, object> LoadTempData(ControllerContext controllerContext)
   //   {
   //      HttpCookie cookie = _httpContext.Request.Cookies[TEMP_DATA_COOKIE_KEY];
   //      if (cookie != null && !string.IsNullOrEmpty(cookie.Value))
   //      {
   //         IDictionary<string, object> deserializedTempData = DeserializeTempData(cookie.Value);

   //         cookie.Expires = DateTime.MinValue;
   //         cookie.Value = string.Empty;

   //         if (_httpContext.Response != null && _httpContext.Response.Cookies != null)
   //         {
   //            HttpCookie responseCookie = _httpContext.Response.Cookies[TEMP_DATA_COOKIE_KEY];
   //            if (responseCookie != null)
   //            {
   //               cookie.Expires = DateTime.MinValue;
   //               cookie.Value = string.Empty;
   //            }
   //         }

   //         return deserializedTempData;
   //      }

   //      return new Dictionary<string, object>();
   //   }

   //   protected virtual void SaveTempData(ControllerContext controllerContext, IDictionary<string, object> values)
   //   {
   //      string cookieValue = SerializeToBase64EncodedString(values);

   //      HttpCookie cookie = new HttpCookie(TEMP_DATA_COOKIE_KEY);
   //      cookie.HttpOnly = true;
   //      cookie.Value = cookieValue;

   //      _httpContext.Response.Cookies.Add(cookie);
   //   }

   //   public static IDictionary<string, object> DeserializeTempData(string base64EncodedSerializedTempData)
   //   {
   //      byte[] bytes = Convert.FromBase64String(base64EncodedSerializedTempData);
   //      MemoryStream memStream = new MemoryStream(bytes);
   //      BinaryFormatter binFormatter = new BinaryFormatter();
   //      return binFormatter.Deserialize(memStream, null) as Dictionary<string, object>;
   //   }

   //   public static string SerializeToBase64EncodedString(IDictionary<string, object> values)
   //   {
   //      MemoryStream memStream = new MemoryStream();
   //      memStream.Seek(0, SeekOrigin.Begin);
   //      BinaryFormatter binFormatter = new BinaryFormatter();
   //      binFormatter.Serialize(memStream, values);
   //      memStream.Seek(0, SeekOrigin.Begin);
   //      byte[] bytes = memStream.ToArray();
   //      return Convert.ToBase64String(bytes);
   //   }
   //}
}