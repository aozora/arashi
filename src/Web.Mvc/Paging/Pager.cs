using System;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace Arashi.Web.Mvc.Paging
{
   using System.Threading;

   using Arashi.Core;
   using Arashi.Services.Localization;

   public class Pager
   {
      private ViewContext viewContext;
      private readonly string searchText;
      private readonly int pageSize;
      private readonly int currentPage;
      private readonly long totalItemCount;
      private readonly RouteValueDictionary linkWithoutPageValuesDictionary;
      private readonly string pageQueryStringName = "page";
      private readonly string searchQueryStringName = "s";
      private readonly ILocalizationService localizationService = IoC.Resolve<ILocalizationService>();


      /// <summary>
      /// Instantiate a new pager
      /// </summary>
      /// <param name="viewContext"></param>
      /// <param name="pageSize"></param>
      /// <param name="currentPage"></param>
      /// <param name="totalItemCount"></param>
      /// <param name="valuesDictionary"></param>
      public Pager(ViewContext viewContext, int pageSize, int currentPage, long totalItemCount, RouteValueDictionary valuesDictionary)
      {
         this.viewContext = viewContext;
         this.pageSize = pageSize;
         this.currentPage = currentPage;
         this.totalItemCount = totalItemCount;
         linkWithoutPageValuesDictionary = valuesDictionary;
      }




      /// <summary>
      /// Instantiate a new pager
      /// </summary>
      /// <param name="viewContext"></param>
      /// <param name="pageSize"></param>
      /// <param name="currentPage"></param>
      /// <param name="totalItemCount"></param>
      /// <param name="valuesDictionary"></param>      
      /// <param name="queryStringName">The name of the parameter in the query string</param>
      public Pager(ViewContext viewContext, int pageSize, int currentPage, long totalItemCount, RouteValueDictionary valuesDictionary, string queryStringName)
      {
         this.viewContext = viewContext;
         this.pageSize = pageSize;
         this.currentPage = currentPage;
         this.totalItemCount = totalItemCount;
         linkWithoutPageValuesDictionary = valuesDictionary;

         if (!string.IsNullOrEmpty(queryStringName))
            this.pageQueryStringName = queryStringName;
      }



      /// <summary>
      /// Instantiate a new pager
      /// </summary>
      /// <param name="viewContext"></param>
      /// <param name="pageSize"></param>
      /// <param name="currentPage"></param>
      /// <param name="totalItemCount"></param>
      /// <param name="valuesDictionary"></param>      
      /// <param name="queryStringName">The name of the parameter in the query string</param>
      /// <param name="searchText">the search pattern</param>
      public Pager(ViewContext viewContext, int pageSize, int currentPage, long totalItemCount, RouteValueDictionary valuesDictionary, string queryStringName, string searchText)
      {
         this.searchText = searchText;
         this.viewContext = viewContext;
         this.pageSize = pageSize;
         this.currentPage = currentPage;
         this.totalItemCount = totalItemCount;
         linkWithoutPageValuesDictionary = valuesDictionary;

         if (!string.IsNullOrEmpty(queryStringName))
            this.pageQueryStringName = queryStringName;
      }




      
      /// <summary>
      /// Render the pager
      /// </summary>
      /// <returns></returns>
      public string RenderHtml()
      {

         int pageCount = (int)Math.Ceiling(this.totalItemCount / (double)this.pageSize);
         int nrOfPagesToDisplay = 10;

         // if there is only 1 page, don't show the pager
         if (pageCount <= 1)
            return string.Empty;

         var sb = new StringBuilder();
         sb.AppendLine("<div class=\"wp-pagenavi\">");

         // TODO: wp-pagenavi: localize!!!
         //$pages_text = str_replace("%CURRENT_PAGE%", number_format_i18n($paged), $pagenavi_options['pages_text']);
         //$pages_text = str_replace("%TOTAL_PAGES%", number_format_i18n($max_page), $pages_text);
         //sb.AppendFormat("<span class=\"pages\">&#8201;Page {0} of {1}&#8201;</span>", currentPage, pageCount);
         sb.AppendFormat("<span class=\"pages\">&#8201;" + this.GlobalResource("Pager_PageOf") + "&#8201;</span>", currentPage, pageCount);


         // Previous
         if (this.currentPage > 1)
         {
            sb.Append(GeneratePageLink("&laquo;", this.currentPage - 1));
         }

         int start = 1;
         int end = pageCount;

         if (pageCount > nrOfPagesToDisplay)
         {
            int middle = (int)Math.Ceiling(nrOfPagesToDisplay / 2d) - 1;
            int below = (this.currentPage - middle);
            int above = (this.currentPage + middle);

            if (below < 4)
            {
               above = nrOfPagesToDisplay;
               below = 1;
            }
            else if (above > (pageCount - 4))
            {
               above = pageCount;
               below = (pageCount - nrOfPagesToDisplay);
            }

            start = below;
            end = above;
         }

         if (start > 2 && nrOfPagesToDisplay < pageCount)
         {
            sb.Append(GeneratePageLink("&laquo; " + this.GlobalResource("Pager_First"), start));
         }

         if (start > 3)
         {
            sb.Append(GeneratePageLink("1", 1));
            sb.Append(GeneratePageLink("2", 2));
            sb.Append("<span class=\"extend\">...</span>");
         }

         for (int i = start; i <= end; i++)
         {
            if (i == this.currentPage)
            {
               sb.AppendFormat("<span class=\"current ui-corner-all\">&#8201;{0}&#8201;</span>", i);
            }
            else
            {
               sb.Append(GeneratePageLink(i.ToString(), i));
            }
         }

         if (end < (pageCount - 3))
         {
            sb.Append("<span class=\"extend\">...</span>");
            sb.Append(GeneratePageLink((pageCount - 1).ToString(), pageCount - 1));
            sb.Append(GeneratePageLink(pageCount.ToString(), pageCount));
         }

         // Next
         if (this.currentPage < pageCount)
         {
            sb.Append(GeneratePageLink("&raquo;", (this.currentPage + 1)));
         }

         if (end < pageCount)
         {
            sb.Append(GeneratePageLink(this.GlobalResource("Pager_Last") + " &raquo;", nrOfPagesToDisplay));
         }

         sb.Append("</div");

         return sb.ToString();
      }



      /// <summary>
      /// Get the html for the anchor
      /// </summary>
      /// <param name="linkText"></param>
      /// <param name="pageNumber"></param>
      /// <returns></returns>
      private string GeneratePageLink(string linkText, int pageNumber)
      {
         var linkValueDictionary = new RouteValueDictionary(this.linkWithoutPageValuesDictionary);
         linkValueDictionary.Add(this.pageQueryStringName, pageNumber);

         if (!string.IsNullOrEmpty(this.searchText))
            linkValueDictionary.Add(searchQueryStringName, this.searchText);

         var virtualPathData = this.viewContext.RouteData.Route.GetVirtualPath(this.viewContext.RequestContext, linkValueDictionary);

         //UriBuilder uriBuilder = new UriBuilder(this.viewContext.RequestContext.HttpContext.Request.Url);
                  

         if (virtualPathData != null)
         {
            string url = string.Concat(this.viewContext.RequestContext.HttpContext.Request.Url.GetLeftPart(UriPartial.Authority),
                                        "/",
                                        virtualPathData.VirtualPath);

            // Fix for url canonicalization: ensure that the path has the ending slash
            if (url.IndexOf('?') > -1 && url.IndexOf("/?") == -1)
               url = url.Insert(url.IndexOf('?'), "/");

            return String.Format("<a href=\"{0}\" class=\"ui-corner-all\">&#8201;{1}&#8201;</a>", url, linkText);
         }
         else
         {
            return null;
         }
      }



      
      /// <summary>
      /// Get a global localized resource
      /// </summary>
      /// <param name="token"></param>
      /// <returns></returns>
      private string GlobalResource(string token)
      {
         return localizationService.GlobalResource(token, Thread.CurrentThread.CurrentUICulture);
      }

   }
}