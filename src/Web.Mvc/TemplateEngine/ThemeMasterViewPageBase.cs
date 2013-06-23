namespace Arashi.Web.Mvc.TemplateEngine
{
   using System.Threading;
   using System.IO;
   using System.Web.Mvc;
   using Arashi.Core;
   using Arashi.Core.Domain;
   using Arashi.Services.Localization;
   using Arashi.Web.Mvc.Models;

   /// <summary>
   /// Base generic ViewPage for Admin pages
   /// </summary>
   /// <typeparam name="TModel"></typeparam>
   public class ThemeMasterViewPageBase<TModel> : ViewPage<TModel>
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
      /// Return the template file for the current view
      /// </summary>
      /// <returns></returns>
      public string GetTemplateFile()
      {
         // if the current templatefile is a page and there is a custom template file specified then search for it
         if (Model.TemplateFile == ViewHelper.TemplateFile.page && !string.IsNullOrEmpty(Model.CurrentPage.CustomTemplateFile))
         {
            if (File.Exists( Path.Combine(this.ViewContext.HttpContext.Server.MapPath(Model.Site.Template.BasePath), Model.CurrentPage.CustomTemplateFile + ".ascx" )))
               return Model.CurrentPage.CustomTemplateFile + ".ascx";
         }

         return Model.TemplateFile.ToString().Replace("_", "") + ".ascx";
      }

   }
}