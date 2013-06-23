using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Arashi.Core;
using Arashi.Core.Domain;
using Arashi.Services.Localization;
using Arashi.Web.Mvc.Models;
using Arashi.Web.Mvc.Plugins;
using log4net;

namespace Arashi.Web.Mvc.TemplateEngine
{
   /// <summary>
   /// Base class for all template files.
   /// Must have access to all entities
   /// </summary>
   public partial class TemplateBase<TModel> : ViewUserControl<TModel>
      where TModel : TemplateContentModel
   {
      #region Private Fields

      private static readonly ILog log = LogManager.GetLogger(typeof(TemplateBase<TModel>));
      private Post currentPost;
      private Comment currentComment;
      private PluginHelper pluginHelper;
      private ILocalizationService localizationService;
      #endregion

      #region Protected Properties

      /// <summary>
      /// The plugin helper
      /// </summary>
      protected PluginHelper Plugin {
         get
         {
            if (pluginHelper == null)
               return new PluginHelper(this.ViewContext, this);
            else
               return pluginHelper;
         }
         set
         {
            pluginHelper = value;
         }
      }

      #endregion

      public TemplateBase()
      {
         if (HttpContext.Current.Items["CurrentPost"] != null)
            currentPost = HttpContext.Current.Items["CurrentPost"] as Post;

         localizationService = IoC.Resolve<ILocalizationService>();
      }


      #region Utils

      /// <summary>
      /// Get the virtual path to the specified vied for the template of the current site
      /// </summary>
      /// <param name="templateFile"></param>
      /// <returns></returns>
      public string GetViewVirtualPath(ViewHelper.TemplateFile templateFile)
      {
         if (Model.Site.Template != null)
         {
            return string.Concat(Model.Site.Template.BasePath,
                                 "/",
                                 templateFile.ToString(),
                                 ".ascx");
         }

         return templateFile.ToString();
      }



      /// <summary>
      /// Return the protocol (http:// or https://) along with the domain name and port number (if present)
      /// For example, if the requested page's URL is http://www.yourserver.com:8080/Tutorial01/MyPage.aspx, 
      /// then this method returns the string http://www.yourserver.com:8080
      /// </summary>
      /// <returns></returns>
      protected string GetCurrentSiteUrlRoot()
      {
         //VirtualPathUtility.Combine()
         return Request.Url.GetLeftPart(UriPartial.Authority);
      }



      //protected string GetCurrentUrl()
      //{
      //   string url = string.Concat(ViewContext.RequestContext.HttpContext.Request.Url.GetLeftPart(UriPartial.Authority),
      //                               "/",
      //                               virtualPathData.VirtualPath);

      //   // Fix for url canonicalization: ensure that the path has the ending slash
      //   if (url.IndexOf('?') > -1 && url.IndexOf("/?") == -1)
      //      url = url.Insert(url.IndexOf('?'), "/");
      //}



      /// <summary>
      /// Get a full absolute url for the current request.
      /// </summary>
      /// <param name="partialUrl">
      /// A virtual ("~/") or root-based url ("/")
      /// </param>
      /// <returns></returns>
      protected string GetAbsoluteUrl(string partialUrl)
      {
         return string.Concat(GetCurrentSiteUrlRoot(),
                              "/",
                              partialUrl.StartsWith("~") || partialUrl.StartsWith("/")
                                 ? partialUrl.Substring(1)
                                 : partialUrl);
      }


      /// <summary>
      /// Convert an array of argument in querystring format 
      /// to an equivalent IDictionary
      /// </summary>
      /// <param name="args"></param>
      /// <returns></returns>
      private IDictionary<String, String> GetDictionaryFromQueryStringArray(string args)
      {
         IDictionary<String, String> dic = new Dictionary<string, string>();

         if (string.IsNullOrEmpty(args))
            return dic;

         if (args.Length == 0)
            return dic;

         string[] arguments = args.Split('&');

         foreach (string argument in arguments)
         {
            string[] keyval = argument.Split('=');
            dic.Add(keyval[0], keyval[1]);
         }

         return dic;
      }

      #endregion

      #region Arashi custom WP Extensions

      /// <summary>
      /// Get the url of the current post. THIS IS NOT PRESENT IN WP 2.8
      /// </summary>
      /// <returns></returns>
      protected string post_url()
      {
         return GetAbsoluteUrl(currentPost.GetContentUrl());
      }



      /// <summary>
      /// The function the_post() takes the current item in the collection of posts 
      /// and makes it available for use inside this iteration of The Loop. 
      /// Without the_post(), many of the Template Tags used in your theme would not work
      /// 
      /// Ref: http://codex.wordpress.org/The_Loop_in_Action
      /// </summary>
      /// <returns></returns>
      protected void the_comment(Comment comment)
      {
         currentComment = comment;

         // also set the current comment in the Context, for use by other templates
         HttpContext.Current.Items["CurrentComment"] = currentComment;
      }




      /// <summary>
      /// Return true if the current page display the contact form
      /// </summary>
      /// <returns></returns>
      protected bool is_contact()
      {
         return Model.TemplateFile == ViewHelper.TemplateFile.contact;
      }

      #endregion

      #region Localization Support

      protected string Resource(string token)
      {
         return localizationService.ThemeResource(token, Thread.CurrentThread.CurrentUICulture);
      }


      protected string ResourceFormat(string token, params object[] args)
      {
         return string.Format(Resource(token), args);
      }

      #endregion
   }
}