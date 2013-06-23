using System;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using Arashi.Core;
using Arashi.Core.Domain;
using Arashi.Services.Localization;

namespace Arashi.Web.Mvc.Views
{
   /// <summary>
   /// Base <see cref="WebViewPage"/> for Admin pages
   /// </summary>
   public abstract class AdminViewPageBase : WebViewPage //ViewPage
   {
      private ILocalizationService localizationService;


      /// <summary>
      /// Constructor
      /// </summary>
      protected AdminViewPageBase()
      {
         localizationService = IoC.Resolve<ILocalizationService>();
      }



      /// <summary>
      /// Get the current RequestContext
      /// </summary>
      public IRequestContext RequestContext
      {
         get
         {
            if (ViewData.ContainsKey("Context") && ViewData["Context"] != null)
               return ViewData["Context"] as IRequestContext;
            else
               return null;
         }
      }



      #region Localization Support

      /// <summary>
      /// Get the name of the current UI culture in format xx-XX
      /// </summary>
      public string CurrentUICultureName
      {
         get
         {
            return Thread.CurrentThread.CurrentUICulture.Name;
         }
      }



      /// <summary>
      /// Get a localized global resource
      /// </summary>
      /// <param name="token"></param>
      /// <returns></returns>
      protected string GlobalResource(string token)
      {
         return localizationService.GlobalResource(token, Thread.CurrentThread.CurrentUICulture);
      }



      /// <summary>
      /// Get a localized global resource filled with format parameters
      /// </summary>
      /// <param name="token"></param>
      /// <param name="args"></param>
      /// <returns></returns>
      protected string GlobalResource(string token, params object[] args)
      {
         return string.Format(GlobalResource(token), args);
      }

      #endregion

   }




   /// <summary>
   /// Base generic <see cref="WebViewPage<TModel>"/> for Admin pages
   /// </summary>
   /// <typeparam name="TModel"></typeparam>
   public abstract class AdminViewPageBase<TModel> : WebViewPage<TModel> //ViewPage<TModel>
      where TModel : class
   {
      private ILocalizationService localizationService;


      /// <summary>
      /// Constructor
      /// </summary>
      protected AdminViewPageBase()
      {
         localizationService = IoC.Resolve<ILocalizationService>();
      }



      /// <summary>
      /// Get the current RequestContext
      /// </summary>
      public IRequestContext RequestContext
      {
         get
         {
            if (ViewData.ContainsKey("Context") && ViewData["Context"] != null)
               return ViewData["Context"] as IRequestContext;
            else
               return null;
         }
      }


      #region Localization Support

      /// <summary>
      /// Get the name of the current UI culture in format xx-XX
      /// </summary>
      public string CurrentUICultureName
      {
         get
         {
            return Thread.CurrentThread.CurrentUICulture.Name;
         }
      }



      /// <summary>
      /// Get a localized global resource
      /// </summary>
      /// <param name="token"></param>
      /// <returns></returns>
      protected string GlobalResource(string token)
      {
         return localizationService.GlobalResource(token, Thread.CurrentThread.CurrentUICulture);
      }



      /// <summary>
      /// Get a localized global resource filled with format parameters
      /// </summary>
      /// <param name="token"></param>
      /// <param name="args"></param>
      /// <returns></returns>
      protected string GlobalResource(string token, params object[] args)
      {
         return string.Format(GlobalResource(token), args);
      }

      #endregion

   }
}