using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arashi.Core.Domain;
using Arashi.Services.Localization;
using Arashi.Core.Extensions;
using Arashi.Services.Content;
using Arashi.Core.Domain.Dto;
using Arashi.Core.Util;
using System.Threading;
using Common.Logging;

namespace Arashi.Web.Widgets.Categories
{
   public class CategoriesWidgetComponent : WidgetComponentBase
   {
      #region Private Fields

      private ILog log;
      private readonly ILocalizationService localizationService;
      private readonly ICategoryService categoryService;
      private IEnumerable<Category> categories;

      // Settings
      private string show_count = "true";


      #endregion

      #region Constructor

      public CategoriesWidgetComponent(ILog log, ILocalizationService localizationService, ICategoryService categoryService)
      {
         this.log = log;
         this.localizationService = localizationService;
         this.categoryService = categoryService;
      }

      #endregion

      private string Resource(string token)
      {
         return localizationService.ThemeResource(token, Thread.CurrentThread.CurrentUICulture);
      }



      public override void Init()
      {
         log.Debug("CategoriesWidgetComponent .Init: start");

         // Get the list of categories data
         categories = categoryService.GetAllCategoriesBySite(context.CurrentSite);

         log.Debug("CategoriesWidgetComponent .Init: end");

         base.Init();
      }



      // Pay attention: is slightly different from wp_list_categories !!!!!!!!!!!!!!
      public override string Render()
      {
         if (widget.Settings.Count > 0)
            show_count = widget.Settings["show_count"];

         StringBuilder html = new StringBuilder();
         html.Append("<ul>");

         foreach (Category category in categories)
         {
            if (category.ParentCategory == null)
            {
               html.AppendFormat("<li class=\"cat-item cat-item-{0}\">", category.Id.ToString());

               // Render a category
               html.AppendFormat("<a title=\"{2} {0}\" href=\"{1}\">{0}</a>", 
                                 category.Name, 
                                 GetAbsoluteUrl(category.GetCategoryUrl()),
                                 Resource("Category_LinkTitle"));

               RenderChildCategories(html, category);

               html.Append("</li>");
            }
         }

         html.Append("</ul>");
         return html.ToString();
      }



      /// <summary>
      /// Render the child categories
      /// </summary>
      /// <param name="html"></param>
      /// <param name="category"></param>
      private void RenderChildCategories(StringBuilder html, Category category)
      {
         if (category.ChildCategories.Count > 0)
         {
            html.Append("<ul class=\"children\">");

            foreach (Category childCategory in category.ChildCategories)
            {
               html.AppendFormat("<li class=\"cat-item cat-item-{0}\">", childCategory.Id.ToString());

               // Render a category
               html.AppendFormat("<a title=\"View all posts filed under {0}\" href=\"{1}\">{0}</a>", childCategory.Name, GetAbsoluteUrl(childCategory.GetCategoryUrl()));

               RenderChildCategories(html, childCategory);

               html.Append("</li>");
            }

            html.Append("</ul>");
         }

      }


   }
}
