namespace Arashi.Web.Mvc.TemplateEngine
{
   using System.IO;
   using System.Web.Mvc;
   using Arashi.Core.Domain;
   using Arashi.Web.Mvc.Models;

   /// <summary>
   /// Base generic WebViewPage for Theme layout views
   /// </summary>
   /// <typeparam name="TModel"></typeparam>
   public abstract class ThemeLayoutWebViewPageBase<TModel> : WebViewPage<TModel>
      where TModel : TemplateContentModel
   {



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



      /// <summary>
      /// Return the Theme file path for the current view
      /// </summary>
      /// <returns></returns>
      public string GetTemplateFile()
      {
         string templateBasePath = Model.Site.Theme.BasePath;

         // if the current templatefile is a page and there is a custom Theme file specified then search for it
         if (Model.TemplateFile == ViewHelper.TemplateFile.page && !string.IsNullOrEmpty(Model.CurrentPage.CustomTemplateFile))
         {
            if (File.Exists(Path.Combine(this.ViewContext.HttpContext.Server.MapPath(Model.Site.Theme.BasePath), Model.CurrentPage.CustomTemplateFile + ".cshtml")))
               return Model.CurrentPage.CustomTemplateFile + ".cshtml";
         }

         string templateFilePath = templateBasePath + "/" + Model.TemplateFile.ToString().Replace("_", "") + ".cshtml";

         return templateFilePath;
      }

   }
}