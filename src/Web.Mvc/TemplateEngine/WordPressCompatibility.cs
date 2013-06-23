namespace Arashi.Web.Mvc.TemplateEngine
{
   using System;
   using System.Collections;
   using System.Collections.Generic;
   using System.Diagnostics.CodeAnalysis;
   using System.Globalization;
   using System.IO;
   using System.Linq;
   using System.Text;
   using System.Text.RegularExpressions;
   using System.Threading;
   using System.Web;
   using System.Web.Mvc;
   using System.Web.Mvc.Html;
   using System.Web.Routing;

   using Arashi.Core.Domain;
   using Arashi.Core.Domain.Dto;
   using Arashi.Core.Extensions;
   using Arashi.Core.Util;
   using Arashi.Web.Mvc.Gravatar;
   using Arashi.Web.Mvc.Models;
   using Arashi.Web.Mvc.Paging;
   using Arashi.Web.Widgets;

   using HtmlAgilityPack;

   using Telerik.Web.Mvc.UI;


   /// <summary>
   /// WordPress compatibility functions
   /// </summary>
   /// <typeparam name="TModel"></typeparam>
   [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Reviewed. Suppression is OK here.")]
   public partial class TemplateBase<TModel> : ViewUserControl<TModel>
      where TModel : TemplateContentModel
   {
      #region Enumerations

      private enum ArchiveList
      {
         yearly,
         monthly,
         daily,
         weekly,
         postbypost
      }

      #endregion

      #region WordPress compatibility methods

      #region General "wp" functions

      /// <summary>
      /// Displays or returns the title of the page
      /// The title text depends on the query:
      ///   
      ///   Single post or a Page
      ///       the title of the post (or Page) 
      ///   Date-based archive 
      ///       the date (e.g., "2006", "2006 - January") 
      ///   Category 
      ///       the name of the category 
      ///   Author page 
      ///       the public name of the user 
      /// 
      /// </summary>
      /// <returns></returns>
      protected string wp_title()
      {
         return wp_title(null, true, "left");
      }

      // Overwritten by SeoPack
      //protected string wp_title(string sep, bool echo, string seplocation)
      //{
      //   if (sep == null)
      //      sep = "&raquo;";

      //   if (!seplocation.Equals("left", StringComparison.InvariantCultureIgnoreCase) &&
      //      !seplocation.Equals("right", StringComparison.InvariantCultureIgnoreCase))
      //      throw new ApplicationException("wp_title: argument seplocation can only be 'right' or 'left'");

      //   // TODO: implement routing recognition...
      //   return string.Concat(seplocation == "left" ? sep : string.Empty,
      //                        Html.Encode(Model.Site.Name),
      //                        seplocation == "right" ? sep : string.Empty
      //                        );
      //}

      /// <summary>
      /// The get_bloginfo() function returns information about your blog which can then be used 
      /// elsewhere in your code. This function, as well as bloginfo(), can also be used 
      /// to display your blog information
      /// </summary>
      /// <param name="parameter"></param>
      /// <returns></returns>
      /// <remarks>
      /// The string returned is encoded with Html.Encode()
      /// </remarks>
      protected string get_bloginfo(string parameter)
      {
         switch (parameter)
         {
            case "home":
               return GetCurrentSiteUrlRoot();
            case "url":
               return Request.Url.ToString();
            case "html_type":
               return "text/html";
            case "charset":
               return "iso-8859-1";
            case "template_url":
            case "template_directory":
               return GetAbsoluteUrl(VirtualPathUtility.ToAbsolute(Model.Site.Template.BasePath));
            case "stylesheet_directory":
               return GetAbsoluteUrl(VirtualPathUtility.ToAbsolute(Model.Site.Template.BasePath));
            case "stylesheet_url":
               return GetAbsoluteUrl(VirtualPathUtility.ToAbsolute(string.Concat(Model.Site.Template.BasePath, "/style.css")));
            case "name":
               return Html.Encode(Model.Site.Name);
            case "description":
               return Html.Encode(Model.Site.Description);
            case "rss2_url":
               return GetCurrentSiteUrlRoot() + "/feed/";
            case "atom_url":
               return GetCurrentSiteUrlRoot() + "/feed/atom/";
            case "pingback_url":
               return GetCurrentSiteUrlRoot() + "/pingback.axd";
            case "text_direction":
               return "ltr"; //TODO: Implement text_direction!!!
            default:
               return string.Empty;
         }
      }

      // TODO: support all WP params
      // TODO: all this must be dynamical
      /// <summary>
      /// Displays information about your blog, mostly gathered from the information you supply 
      /// in your User Profile and General Options from the Administration panels 
      /// </summary>
      /// <param name="parameter"></param>
      /// <returns></returns>
      protected string bloginfo(string parameter)
      {
         return get_bloginfo(parameter);
      }

      // This is in SeoPack.cs
      //protected string wp_head()
      //{
      //}


      /// <summary>
      /// Render all the scripts registered with wp_enqueue_script
      /// </summary>
      /// <returns></returns>
      protected string wp_footer()
      {
         // Call the rendering of registered scripts
         Html.Telerik().ScriptRegistrar().DefaultGroup(group => group.Combined(!Context.IsDebuggingEnabled));
         Html.Telerik().ScriptRegistrar().Render();

         return "";
      }




      [Obsolete("use wp_list_categories", false)]
      protected string wp_list_cats(string args)
      {
         return wp_list_categories(args);
      }

      /// <summary>
      /// Displays a list of Categories as links. When a Category link is clicked, 
      /// all the posts in that Category will display on a Category Page using the 
      /// appropriate Category Template dictated by the Template Hierarchy rules
      /// </summary>
      /// <param name="args"></param>
      /// <returns></returns>
      protected string wp_list_categories(string args)
      {
         string title_li = "Categories";
         string show_count = "0";

         IDictionary<String, String> arguments = GetDictionaryFromQueryStringArray(args);

         // TODO: SUPPORT ALL OPTION!!!!!!!!!!!!

         // Set defaults if argument missings
         if (arguments.ContainsKey("title_li"))
         {
            title_li = arguments["title_li"];
         }

         if (arguments.ContainsKey("show_count"))
         {
            show_count = arguments["show_count"];
         }

         StringBuilder html = new StringBuilder();
         html.Append("<li>");
         html.Append(title_li);
         html.Append("<ul>");

         foreach (Category category in Model.Categories)
         {
            if (category.ParentCategory == null)
            {
               html.AppendFormat("<li class=\"cat-item cat-item-{0}\">", category.Id.ToString());

               // Render a category
               html.AppendFormat("<a title=\"View all posts filed under {0}\" href=\"{1}\">{0}</a>", category.Name, GetAbsoluteUrl(category.GetCategoryUrl()));

               RenderChildCategories(html, category);

               html.Append("</li>");
            }
         }

         html.Append("</ul>");
         html.Append("</li>");
         return html.ToString();
      }

      private void RenderChildCategories(StringBuilder html, Category category)
      {
         if (category.ChildCategories.Count > 0)
         {
            html.Append("<ul class=\"children\">");

            foreach (Category childCategory in category.ChildCategories)
            {
               html.AppendFormat("<li class=\"cat-item cat-item-{0}\">", childCategory.Id.ToString());

               // Render a category
               html.AppendFormat("<a title=\"{2} {0}\" href=\"{1}\">{0}</a>", childCategory.Name, GetAbsoluteUrl(childCategory.GetCategoryUrl()), Resource("Category_LinkTitle"));

               RenderChildCategories(html, childCategory);

               html.Append("</li>");
            }

            html.Append("</ul>");
         }
      }

      /// <summary>
      /// Displays bookmarks found in the Administration > Links panel. 
      /// This Template Tag allows the user to control how the bookmarks are sorted and displayed
      /// NOTE: wp_list_bookmarks() intended to replace the deprecated Template tags 
      ///   get_links_list() and get_links()
      /// </summary>
      /// <param name="args"></param>
      /// <returns></returns>
      protected string wp_list_bookmarks(string args)
      {
         // TODO: to implement
         return "";
      }

      protected string wp_list_bookmarks()
      {
         return wp_list_bookmarks(null);
      }

      /// <summary>
      /// This function displays a date-based archives list in the same way as get_archives(). 
      /// The only difference is that parameter arguments are given to the function in query string format
      /// </summary>
      /// <param name="args"></param>
      /// <returns></returns>
      protected string wp_get_archives(string args)
      {
         IDictionary<String, String> arguments = GetDictionaryFromQueryStringArray(args);

         StringBuilder html = new StringBuilder();
         ArchiveList type = ArchiveList.monthly; // default
         string format = "html";
         bool showPostCount = false;
         int limit = 15;
         string before = string.Empty;
         string after = string.Empty;

         if (arguments.ContainsKey("type"))
         {
            type = (ArchiveList)Enum.Parse(typeof(ArchiveList), arguments["type"]);
         }

         if (arguments.ContainsKey("format"))
         {
            format = arguments["format"];
         }

         if (arguments.ContainsKey("limit"))
         {
            limit = Convert.ToInt32(arguments["limit"]);
         }

         if (arguments.ContainsKey("show_post_count"))
         {
            showPostCount = Convert.ToBoolean(arguments["show_post_count"]);
         }

         // TODO: Support other enums of ArchiveList
         if (type == ArchiveList.monthly)
         {
            var calendar = (from dto in Model.Calendar
                            group dto by new { dto.Year, dto.Month }
                            into g select new { Year = g.Key.Year, Month = g.Key.Month, Count = g.Sum(dto => dto.Count) });

            foreach (var item in calendar)
            {
               string text = DateUtil.MonthNames(Thread.CurrentThread.CurrentUICulture, false)[item.Month].Capitalize() + "&nbsp;" + item.Year.ToString();
               string href = string.Concat(GetCurrentSiteUrlRoot(), "/", item.Year.ToString(), "/", item.Month.ToString().PadLeft(2, '0'), "/");
               string postCount = showPostCount ? string.Format("({0})", item.Count.ToString()) : string.Empty;
               string template;

               // {0} text
               // {1} href
               // {2} before
               // {3} after
               // {4} count
               switch (format)
               {
                  case "option":
                     template = "<option value=\"{1}\"><a title=\"{0}\" href=\"{1}\">{2}{0}{3}{4}</a><option>";
                     break;
                  case "link":
                     template = "<a title=\"{0}\" href=\"{1}\">{2}{0}{3}{4}</a><br />";
                     break;
                  case "custom":
                     template = "{1}{0}{3}{4}";
                     break;
                  case "html":
                  default:
                     template = "<li><a title=\"{0}\" href=\"{1}\">{2}{0}{3}{4}</a></li>";
                     break;
               }

               html.AppendFormat(template, text, href, before, after, postCount);
            }
         }
         else if (type == ArchiveList.postbypost)
         {
            IEnumerable<Post> recentPosts = Model.RecentPosts.Take(limit);

            foreach (Post post in recentPosts)
            {
               string text = post.Title.StripHtml();
               string href = string.Concat(GetCurrentSiteUrlRoot(), "/", post.GetContentUrl());

               string template;

               // {0} text
               // {1} href
               // {2} before
               // {3} after
               switch (format)
               {
                  case "option":
                     template = "<option value=\"{1}\"><a title=\"{0}\" href=\"{1}\">{2}{0}{3}</a><option>";
                     break;
                  case "link":
                     template = "<a title=\"{0}\" href=\"{1}\">{2}{0}{3}</a><br />";
                     break;
                  case "custom":
                     template = "{1}{0}{3}";
                     break;
                  case "html":
                  default:
                     template = "<li><a title=\"{0}\" href=\"{1}\">{2}{0}{3}</a></li>";
                     break;
               }

               html.AppendFormat(template, text, href, before, after);
            }
         }

         return html.ToString();
      }

      /// <summary>
      /// Displays a list of WordPress Pages as links.
      /// See http://codex.wordpress.org/Template_Tags/wp_list_pages
      /// </summary>
      /// <returns></returns>
      protected string wp_list_pages(string args)
      {
         if (Model.Pages == null)
         {
            return string.Empty;
         }

         IDictionary<String, String> arguments = GetDictionaryFromQueryStringArray(args);

         #region Parameters Defaults & Validation

         // Defaults

         // depth 
         //    0 (default) Displays pages at any depth and arranges them hierarchically in nested lists
         //    1 Displays top-level Pages only
         //    2, 3 … Displays Pages to the given depth
         int depth = 0;
         string show_date = "";
         string date_format = get_option("date_format");
         int child_of = 0;
         string exclude = ""; // TODO da gestire
         string title_li = "Pages";
         //'echo' => 1,
         string authors = "";
         string sort_column = "menu_order, post_title";
         string link_before = "";
         string link_after = "";

         if (arguments.ContainsKey("depth"))
         {
            depth = Convert.ToInt32(arguments["depth"]);
         }

         if (arguments.ContainsKey("show_date"))
         {
            show_date = arguments["show_date"];
         }

         if (arguments.ContainsKey("date_format"))
         {
            date_format = arguments["date_format"];
         }

         if (arguments.ContainsKey("child_of"))
         {
            child_of = Convert.ToInt32(arguments["child_of"]);
         }

         if (arguments.ContainsKey("exclude"))
         {
            exclude = arguments["exclude"];
         }

         if (arguments.ContainsKey("title_li"))
         {
            title_li = arguments["title_li"];
         }

         if (arguments.ContainsKey("authors"))
         {
            authors = arguments["authors"];
         }

         if (arguments.ContainsKey("sort_column"))
         {
            sort_column = arguments["sort_column"];
         }

         if (arguments.ContainsKey("link_before"))
         {
            link_before = arguments["link_before"];
         }

         if (arguments.ContainsKey("link_after"))
         {
            link_after = arguments["link_after"];
         }

         #endregion

         StringBuilder html = new StringBuilder();
         bool isSelected = false;

         if (Model.Pages.Count > 0)
         {
            if (!string.IsNullOrEmpty(title_li))
            {
               html.AppendLine("<li class=\"pagenav\">" + title_li + "<ul>");
            }

            IEnumerable<Page> sortedPages;
            IEnumerable<Page> topPages = Model.Pages;

            // check if a "top" page is specified
            if (child_of != 0)
            {
               Page child = (from p in Model.Pages
                             where p.Id == child_of
                             select p).Single();

               topPages = child.AsDepthFirstEnumerable(x => x.ChildPages);
            }

            int realDepth = depth - 1;
            bool showChilds = realDepth > 0;
            if (showChilds)
            {
               sortedPages = from p in topPages
                             where p.Depth <= realDepth && p.Depth > 0
                              && p.WorkflowStatus == WorkflowStatus.Published
                             orderby p.Position ascending
                             select p;
            }
            else
            {
               // show only page with depth == 0
               sortedPages = from p in topPages
                             where p.WorkflowStatus == WorkflowStatus.Published
                                && p.Depth == 0
                             orderby p.Position ascending
                             select p;
            }


            foreach (Page page in sortedPages)
            {
               bool isParent = false;

               // note: Model.CurrentPage is null for the home page (index.ascx template)
               if (Model.CurrentPage != null && Model.CurrentPage.ParentPage != null && Model.CurrentPage.ParentPage.Equals(page))
                  isParent = true;

               isSelected = (this.is_page() && Model.CurrentPage == page) || (this.is_page() && isParent);

               html.AppendFormat("<li class=\"page_item page_item-{0}\"><a href=\"{1}\" class=\"{3}\">{2}</a></li>", page.Id.ToString(), GetAbsoluteUrl(page.GetContentUrl()), Html.Encode(page.Title), (isSelected ? "selected" : string.Empty));
            }

            if (!string.IsNullOrEmpty(title_li))
            {
               html.AppendLine("</ul></li>");
            }
         }

         return html.ToString();
      }



      /// <summary>
      /// This Template Tag returns the URL that allows the user to log in to the site.
      /// </summary>
      protected string wp_login_url()
      {
         return get_option("site_url") + "/login/";
      }

      /// <summary>
      /// Displays a list of tags in what is called a 'tag cloud', where the size of each tag 
      /// is determined by how many times that particular tag has been assigned to posts.
      /// See http://codex.wordpress.org/Template_Tags/wp_tag_cloud
      /// </summary>
      /// <param name="args"></param>
      /// <returns></returns>
      protected string wp_tag_cloud(string args)
      {
         IDictionary<String, String> arguments = GetDictionaryFromQueryStringArray(args);

         #region Parameters Defaults & Validation

         int smallest = 8;
         int largest = 22;
         string unit = "pt";
         int number = 45;
         string format = "flat";
         string orderby = "name";
         string order = "ASC";
         //'exclude'  => , 
         //'include'  => , 
         //string link = "view";
         string taxonomy = "post_tag";
         //'echo'     => true 

         if (arguments.ContainsKey("smallest"))
         {
            smallest = Convert.ToInt32(arguments["smallest"]);
         }

         if (arguments.ContainsKey("largest"))
         {
            largest = Convert.ToInt32(arguments["largest"]);
         }

         if (arguments.ContainsKey("unit"))
         {
            const string allowedUnit = "pt,px,em,%";
            if (allowedUnit.IndexOf(arguments["unit"]) >
                -1)
            {
               unit = arguments["unit"];
            }
         }

         if (arguments.ContainsKey("format"))
         {
            const string allowedFormat = "flat,list,array";
            if (allowedFormat.IndexOf(arguments["format"]) >
                -1)
            {
               format = arguments["format"];
            }
         }

         if (arguments.ContainsKey("orderby"))
         {
            const string allowedFormat = "name,count";
            if (allowedFormat.IndexOf(arguments["orderby"]) >
                -1)
            {
               orderby = arguments["orderby"];
            }
         }

         if (arguments.ContainsKey("order"))
         {
            const string allowedFormat = "ASC,DESC,RAND";
            if (allowedFormat.IndexOf(arguments["order"]) >
                -1)
            {
               order = arguments["order"];
            }
         }

         if (arguments.ContainsKey("taxonomy"))
         {
            const string allowedFormat = "post_tag,category,link_category";
            if (allowedFormat.IndexOf(arguments["taxonomy"]) >
                -1)
            {
               taxonomy = arguments["taxonomy"];
            }
         }

         #endregion

         IEnumerable<TagDTO> tagsToRender = Model.TagCloud;
         IEnumerable<TagDTO> tags;

         // eventually restrict the tags to the given number at max
         if (number > 0)
         {
            tagsToRender = (from t in Model.TagCloud
                            where t.Count > 0
                            orderby t.Count descending
                            select t).Take(number);
         }

         // sorry, but the DynamicLinq library doesn't work...
         if (orderby == "count" &&
             order == "ASC")
         {
            tags = from t in tagsToRender
                   where t.Count > 0
                   orderby t.Count ascending
                   select t;
         }
         else if (orderby == "count" &&
                  order == "DESC")
         {
            tags = from t in tagsToRender
                   where t.Count > 0
                   orderby t.Count descending
                   select t;
         }
         else if (orderby == "name" &&
                  order == "DESC")
         {
            tags = from t in tagsToRender
                   where t.Count > 0
                   orderby t.Name descending
                   select t;
         }
         else // (orderby == "name" && order == "ASC")
         {
            tags = from t in tagsToRender
                   where t.Count > 0
                   orderby t.Name ascending
                   select t;
         }

         // if there are no tags (i.e. fresh setup) then exit
         if (tags == null ||
             tags.Count() == 0)
         {
            return string.Empty;
         }

         // determine the font size
         long min_count = tags.Min(t => t.Count);
         long spread = tags.Max(t => t.Count) - min_count;
         int font_spread = largest - smallest;
         if (font_spread < 0)
         {
            font_spread = 1;
         }
         double font_step = (double)font_spread / spread;

         StringBuilder html = new StringBuilder();

         foreach (TagDTO tag in tags)
         {
            string tag_link = GetAbsoluteUrl(Model.Tags.Single(t => t.TagId == tag.TagId).GetTagUrl());
            string link = string.Format("<a href='{0}' class='tag-link-{1}' title='{2} {6}' rel='tag' style='font-size: {3}{4}'>{5}</a>", tag_link, tag.TagId.ToString(), tag.Count.ToString(), (smallest + ((tag.Count - min_count) * font_step)).ToString(), unit, tag.Name, Resource("TagCloud_TitleTopics"));

            if (format == "list")
            {
               html.AppendFormat("<li>{0}</li>", link);
            }
            else
            {
               html.Append(link);
            }
         }

         if (format == "list")
         {
            html.Insert(0, "<ul class='wp-tag-cloud'>");
            html.Append("</ul>");
         }

         return html.ToString();
      }

      protected string wp_generate_tag_cloud(IList<Tag> tags, IDictionary<String, String> args)
      {
         throw new NotImplementedException();
      }

      /// <summary>
      /// A safe way of getting values for a named option from the options database table.
      /// </summary>
      /// <param name="option"></param>
      /// <returns></returns>
      protected string get_option(string option)
      {
         switch (option)
         {
            case "home":
               return GetCurrentSiteUrlRoot();
               break;

            case "siteurl":
               return GetCurrentSiteUrlRoot();
               break;

            case "blogname":
               return Model.Site.Name;
               break;

            case "comment_registration":
               return Convert.ToInt32(Model.Site.AllowCommentsOnlyForRegisteredUsers).ToString();
               break;

               // TODO: date_format
               //case "date_format":
               //   return Model.Site.Name;
               //   break;

            default:
               return string.Empty;
         }
      }

      protected void get_header()
      {
         Html.RenderPartial(GetViewVirtualPath(ViewHelper.TemplateFile.header));
      }

      protected void get_sidebar()
      {
         Html.RenderPartial(GetViewVirtualPath(ViewHelper.TemplateFile.sidebar));
      }

      protected void get_footer()
      {
         Html.RenderPartial(GetViewVirtualPath(ViewHelper.TemplateFile.footer));
      }

      protected void get_search_form()
      {
         Html.RenderPartial(GetViewVirtualPath(ViewHelper.TemplateFile.searchform));
      }

      protected string get_avatar(string email, int size)
      {
         return Html.GravatarImage(email, size);
      }

      /// <summary>
      /// Return true if the current user is logged and authenticated
      /// </summary>
      /// <returns></returns>
      protected bool is_user_logged_in()
      {
         return Request.IsAuthenticated;
      }

      protected bool is_home()
      {
         return Model.TemplateFile == ViewHelper.TemplateFile.index;
      }

      protected bool is_single()
      {
         return Model.TemplateFile == ViewHelper.TemplateFile.single;
      }

      /// <summary>
      /// Return true if the current TemplateFile is 404 or 302
      /// </summary>
      /// <returns></returns>
      protected bool is_404()
      {
         return Model.TemplateFile == ViewHelper.TemplateFile._404 || Model.TemplateFile == ViewHelper.TemplateFile._302;
      }

      /// <summary>
      /// Return true if the current TemplateFile is 404 or 302
      /// </summary>
      /// <returns></returns>
      protected bool is_302()
      {
         return Model.TemplateFile == ViewHelper.TemplateFile._302;
      }

      protected bool is_archive()
      {
         return Model.TemplateFile == ViewHelper.TemplateFile.archive;
      }

      protected bool is_search()
      {
         return Model.TemplateFile == ViewHelper.TemplateFile.search;
      }

      protected bool is_page()
      {
         return Model.TemplateFile == ViewHelper.TemplateFile.page;
      }

      protected bool is_page(string friendlyname)
      {
         return Model.TemplateFile == ViewHelper.TemplateFile.page && Model.CurrentPage != null && Model.CurrentPage.FriendlyName == friendlyname;
      }

      protected bool is_page(int pageId)
      {
         return Model.TemplateFile == ViewHelper.TemplateFile.page && Model.CurrentPage != null && Model.CurrentPage.Id == pageId;
      }


     protected bool is_paged()
      {
         return !string.IsNullOrEmpty(Context.Request.QueryString["page"]);
      }

      // TODO: edit for support page
      protected bool is_static_front_page()
      {
         return (Model.Site.DefaultPage != null && is_home());
         //return get_option('show_on_front') == 'page' && is_page() && $post->ID == get_option('page_on_front');
         return false;
      }

      /// <summary>
      /// Return true if the current page is home and is a "recent post" page (not a static Domain.Page)
      /// </summary>
      /// <returns></returns>
      protected bool is_static_posts_page()
      {
         return (Model.Site.DefaultPage != null && is_home());
         //return get_option('show_on_front') == 'page' && is_home() && $post->ID == get_option('page_for_posts');
         //return false; // inverse of is_static_front_page (see options show home page)
      }

      protected bool is_post()
      {
         return Model.ViewMode == ViewHelper.ViewMode.is_post;
      }

      protected bool is_tag()
      {
         return Model.ViewMode == ViewHelper.ViewMode.is_tag;
      }

      protected bool is_category()
      {
         return Model.ViewMode == ViewHelper.ViewMode.is_category;
      }

      protected bool is_day()
      {
         return Model.ViewMode == ViewHelper.ViewMode.is_day;
      }

      protected bool is_month()
      {
         return Model.ViewMode == ViewHelper.ViewMode.is_month;
      }

      protected bool is_year()
      {
         return Model.ViewMode == ViewHelper.ViewMode.is_year;
      }

      protected bool is_date()
      {
         return is_year() || is_month() || is_day();
      }

      protected bool is_author()
      {
         return Model.ViewMode == ViewHelper.ViewMode.is_author;
      }

      /// <summary>
      /// Displays or returns the category title for the current page. 
      /// For pages displaying WordPress tags rather than categories (e.g. "/tag/geek") 
      /// the name of the tag is displayed instead of the category. 
      /// Can be used only outside The Loop.
      /// </summary>
      /// <returns></returns>
      protected string single_cat_title(string prefix, bool display)
      {
         if (Model.CurrentCategory != null)
         {
            return display ? Html.Encode(prefix + Model.CurrentCategory.Name) : Html.Encode(Model.CurrentCategory.Name);
         }
         else
         {
            return string.Empty;
         }
      }

      protected string single_cat_title()
      {
         return single_cat_title(null, false);
      }

      /// <summary>
      /// Displays or returns the tag title for the current page
      /// </summary>
      /// <param name="prefix"></param>
      /// <param name="display"></param>
      /// <returns></returns>
      protected string single_tag_title(string prefix, bool display)
      {
         if (Model.CurrentTag != null)
         {
            return display ? Html.Encode(prefix + Model.CurrentTag.Name) : Html.Encode(Model.CurrentTag.Name);
         }
         else
         {
            return string.Empty;
         }
      }

      protected string single_tag_title()
      {
         return single_tag_title(null, false);
      }

      protected string _e()
      {
         throw new NotImplementedException();
      }

      // see http://www.nathanrice.net/blog/wordpress-2-8-and-the-body_class-function/
      protected string body_class()
      {
         // TODO: generate full support for body_class !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

         string controller = ViewContext.RouteData.Values["controller"].ToString().ToLower();
         string action = ViewContext.RouteData.Values["action"].ToString().ToLower();
         string bodyClass = string.Empty;

         switch (controller)
         {
            case "post":
               if (action == "index")
               {
                  bodyClass = "home blog";
               }
               else if (action == "single")
               {
                  bodyClass = "single";
               }
               break;
            case "category":
               bodyClass = "category";
               break;
            case "tag":
               bodyClass = "tag";
               break;
            case "archives":
               bodyClass = "archives";
               break;
         }

         return string.Format("class=\"{0}\"", bodyClass);
      }

      #endregion

      #region THE LOOP functions

      protected virtual bool have_posts()
      {
         return Model.Posts != null && Model.Posts.Count != 0;
      }

      protected virtual string the_ID()
      {
         return currentPost.Id.ToString();
      }

      /// <summary>
      /// The function the_post() takes the current item in the collection of posts 
      /// and makes it available for use inside this iteration of The Loop. 
      /// Without the_post(), many of the Template Tags used in your theme would not work
      /// 
      /// Ref: http://codex.wordpress.org/The_Loop_in_Action
      /// </summary>
      /// <returns></returns>
      protected virtual void the_post(Post post)
      {
         currentPost = post;

         // also set the current post in the Context, for use by other templates
         HttpContext.Current.Items["CurrentPost"] = currentPost;
      }

      /// <summary>
      /// If the current post has an explicit summary, returns that one.
      /// Otherwise, returns the first 55 words of the post content (stripped of html tags).
      /// </summary>
      /// <returns></returns>
      protected virtual string the_excerpt()
      {
         if (string.IsNullOrEmpty(currentPost.Summary))
         {
            return currentPost.Teaser;
         }
         else
         {
            return currentPost.Summary;
         }
      }

      /// <summary>
      /// The post permalink
      /// </summary>
      /// <returns></returns>
      protected virtual string the_permalink()
      {
         return get_permalink();
      }

      protected string get_permalink()
      {
         return GetAbsoluteUrl(currentPost.GetContentUrl());
      }

      protected string get_permalink(ContentItem item)
      {
         return GetAbsoluteUrl(item.GetContentUrl());
      }

      protected virtual string the_title()
      {
         return Html.Encode(currentPost.Title);
      }

      /// <summary>
      /// Displays or returns the title of the current post. 
      /// It somewhat duplicates the functionality of the_title(), but provides a 'clean' version 
      /// of the title by stripping HTML tags and converting certain characters (including quotes) 
      /// to their character entity equivalent
      /// </summary>
      /// <returns></returns>
      protected virtual string the_title_attribute()
      {
         return Html.Encode(currentPost.Title.StripHtml());
      }

      protected string the_content()
      {
         return the_content(null, false, null);
      }

      /// <summary>
      /// Displays the contents of the current post. This tag must be within The_Loop
      /// See http://codex.wordpress.org/The_Loop_in_Action
      /// </summary>
      /// <param name="more_link_text">The link text to display for the "more" link</param>
      /// <returns></returns>
      protected string the_content(string more_link_text)
      {
         return the_content(more_link_text, false, null);
      }

      /// <summary>
      /// Displays the contents of the current post. This tag must be within The_Loop
      /// See http://codex.wordpress.org/The_Loop_in_Action
      /// </summary>
      /// <param name="more_link_text">The link text to display for the "more" link</param>
      /// <param name="strip_teaser">Should the text before the "more" link be hidden (TRUE) or displayed (FALSE, default)</param>
      /// <param name="more_file"></param>
      /// <returns></returns>
      protected string the_content(string more_link_text, bool strip_teaser, string more_file)
      {
         bool hasTeaser = false;
         string teaser;
         string content = currentPost.Content;

         if (string.IsNullOrEmpty(more_link_text))
         {
            more_link_text = Resource("Post_MoreLinkText");
         }

         // Search for the "more" quicktag
         // TODO: test if indexof is faster
         Regex rx = new Regex("<!--more(.*?)?-->", RegexOptions.Compiled);
         MatchCollection matches = rx.Matches(currentPost.Content);

         // if the the "more" quicktag is present in the content...
         if (matches.Count > 0 &&
             !string.IsNullOrEmpty(more_link_text))
         {
            hasTeaser = true;
            string moreLink = matches[0].ToString(); // this extract the <!--more-->

            if (strip_teaser)
            {
               content = content.Substring(content.IndexOf(moreLink) + moreLink.Length - 1);
            }
            else
            {
               // if I'm showing the single post, show the full content with the quicktag
               // replaced by the anchor destination
               if (is_single())
               {
                  content = currentPost.Content.Replace(moreLink, string.Format("<span id=\"more-{0}\"></span>", currentPost.Id.ToString()));
               }
               else
               {
                  teaser = content.Substring(0, content.IndexOf(moreLink));

                  // force_balance_tags to ensure no unclosed tags
                  HtmlAgilityPack.HtmlDocument doc = new HtmlDocument();
                  doc.OptionFixNestedTags = true;
                  doc.LoadHtml(teaser);
                  //IEnumerable<HtmlParseError> errors = doc.ParseErrors;
                  StringWriter output = new StringWriter();

                  doc.Save(output);
                  content = output.ToString();
               }
            }
         }

         if (is_single())
         {
            return content;
         }
         else
         {
            string link = string.Format("<a href=\"{0}#more-{1}\" class=\"more-link\">{2}</a>", get_permalink(), the_ID(), more_link_text);
            return content + link;
         }
      }



      ///// <summary>
      ///// Display Post Thumbnail as set in post's edit screen. Thumbnail images are given class "attachment-thumbnail"
      ///// </summary>
      ///// <returns></returns>
      //protected virtual string the_post_thumbnail()
      //{
      //   return the_post_thumbnail("post-thumbnail", null);
      //}



      ///// <summary>
      ///// Display Post Thumbnail as set in post's edit screen. Thumbnail images are given class "attachment-thumbnail"
      ///// </summary>
      ///// <param name="size">
      ///// Image size.
      ///// Default: 'post-thumbnail', which theme sets using set_post_thumbnail_size( $width, $height, $crop_flag );
      ///// Either a string keyword(thumbnail, medium, large or full) or a 2-item array representing width and height in pixels, e.g. array(32,32)
      ///// </param>
      ///// <param name="attr">
      ///// </param>
      ///// <returns></returns>
      //protected virtual string the_post_thumbnail(string size, string attr)
      //{
      //   string img = "<img width=\"134\" height=\"169\" title=\"icon_product\" alt=\"\" class=\"attachment-post-thumbnail wp-post-image\" src=\"{0}\">";
      //   return "";
      //}



//      protected string wp_get_attachment_image(int attachment_id, string size, bool icon, string attr ) 
//      {
//         if (string.IsNullOrEmpty(size))
//            size = "thumbnail";

//         if (string.IsNullOrEmpty(attr))
//            size = string.empty;

//         string html;

//         return html;
//   //$html = '';
//   //$image = wp_get_attachment_image_src($attachment_id, $size, $icon);
//   //if ( $image ) {
//   //   list($src, $width, $height) = $image;
//   //   $hwstring = image_hwstring($width, $height);
//   //   if ( is_array($size) )
//   //      $size = join('x', $size);
//   //   $attachment =& get_post($attachment_id);
//   //   $default_attr = array(
//   //      'src'	=> $src,
//   //      'class'	=> "attachment-$size",
//   //      'alt'	=> trim(strip_tags( $attachment->post_excerpt )),
//   //      'title'	=> trim(strip_tags( $attachment->post_title )),
//   //   );
//   //   $attr = wp_parse_args($attr, $default_attr);
//   //   $attr = apply_filters( 'wp_get_attachment_image_attributes', $attr, $attachment );
//   //   $attr = array_map( 'esc_attr', $attr );
//   //   $html = rtrim("<img $hwstring");
//   //   foreach ( $attr as $name => $value ) {
//   //      $html .= " $name=" . '"' . $value . '"';
//   //   }
//   //   $html .= ' />';
//   }

//   return $html;
//}




      /// <summary>
      /// Outputs the class="whatever" piece for that div. 
      /// This includes several different classes of value: 
      ///   post, 
      ///   hentry (for hAtom microformat pages), 
      ///   category-X (where X is the slug of every category the post is in), 
      ///   and tag-X (similar, but with tags). 
      /// It also adds "sticky" for posts marked as sticky posts. 
      /// These make it easy to style different parts of the theme in different ways. 
      /// 
      /// Ref: http://codex.wordpress.org/Migrating_Plugins_and_Themes_to_2.7
      /// 
      /// I.e: "post-3 post hentry category-uncategorized tag-nausicaa tag-totoro"
      /// 
      /// </summary>
      /// <returns></returns>
      protected string post_class()
      {
         StringBuilder sb = new StringBuilder();
         sb.Append("class=\"");
         sb.AppendFormat("post-{0}", currentPost.Id.ToString());
         sb.AppendFormat(" post hentry", currentPost.Id.ToString());

         if (currentPost.Categories.Count > 0)
         {
            foreach (Category category in currentPost.Categories)
            {
               sb.AppendFormat(" category-{0}", category.FriendlyName);
            }
         }

         if (currentPost.Tags.Count > 0)
         {
            foreach (Tag tag in currentPost.Tags)
            {
               sb.AppendFormat(" tag-{0}", tag.FriendlyName);
            }
         }

         sb.Append("\" ");

         return sb.ToString();
      }

      protected string the_author()
      {
         return currentPost.Author.DisplayName;
      }

      protected string the_author_posts_link(User author)
      {
         return string.Format("<a href=\"{0}\" title=\"Posts by {1}\">{1}</a>", get_author_posts_url(author), currentPost.Author.DisplayName);
      }

      /// <summary>
      /// This MUST be used inside THE LOOP
      /// </summary>
      /// <param name="author"></param>
      /// <returns></returns>
      protected string get_author_posts_url(User author)
      {
         return GetAbsoluteUrl(author.GetAuthorUrl());
      }

      /// <summary>
      /// Return the Date/Time of a post
      /// 
      /// For formatting: http://codex.wordpress.org/Formatting_Date_and_Time
      /// </summary>
      /// <returns></returns>
      protected virtual string the_time()
      {
         return the_time("");
      }

      protected virtual string the_time(string format)
      {
         Post post = null;

         if (currentPost == null &&
             Model.Posts.Count > 0)
         {
            post = Model.Posts.First();
         }
         else
         {
            post = currentPost;
         }

         return GetTheTime(post.PublishedDate.Value, format);
      }

      protected string date(string format)
      {
         return GetTheTime(DateTime.Now, format);
      }

      protected string GetTheTime(DateTime dateTime, string format)
      {
         // Convert from php format
         string dotnetFormat = format.Replace("F", "MMMM").Replace("Y", "yyyy").Replace("j", "dd").Replace("M", "MMM").Replace("S", "") // php: st, nd, rd or th
            .Replace("l", "dddd");

         // TODO: urgent: set site parameter for Post DateTime format!!!!!!!!!!!!!!!!
         CultureInfo currentCulture = Thread.CurrentThread.CurrentUICulture;

         if (is_user_logged_in())
         {
            return dateTime.AdjustDateToTimeZone((Context.User as User).TimeZone).ToString(dotnetFormat, currentCulture).Capitalize();
         }
         else
         {
            return dateTime.AdjustDateToTimeZone(Model.Site.TimeZone).ToString(dotnetFormat, currentCulture).Capitalize();
         }
      }

      /// <summary>
      /// Displays a link to the tag or tags a post belongs to
      /// </summary>
      /// <param name="before">Text to display before the actual tags are displayed. Defaults to Tags:</param>
      /// <param name="separator">Text or character to display between each tag link. The default is a comma (,) between each tag</param>
      /// <param name="after">Text to display after the last tag. The default is to display nothing</param>
      /// <returns></returns>
      protected virtual string the_tags(string before, string separator, string after)
      {
         return RenderTagsLinks(currentPost.Tags, before, separator, after);
      }

      protected string RenderTagsLinks(IList<Tag> list, string before, string separator, string after)
      {
         if (list.Count == 0)
         {
            return string.Empty;
         }

         if (string.IsNullOrEmpty(before))
         {
            before = "Tags:";
         }

         if (string.IsNullOrEmpty(separator))
         {
            separator = ",";
         }

         StringBuilder tags = new StringBuilder();

         int index = 0;
         tags.Append(before);
         foreach (Tag tag in list)
         {
            tags.AppendFormat("<a rel=\"tag\" href=\"{0}\">{1}</a>", GetAbsoluteUrl(tag.GetTagUrl()), Html.Encode(tag.Name));

            if (index < (currentPost.Tags.Count - 1))
            {
               tags.Append(separator);
            }

            index++;
         }
         tags.Append(after);

         return tags.ToString();
      }

      /// <summary>
      /// Displays a link to the category or categories a post belongs to
      /// </summary>
      /// <param name="separator"></param>
      /// <returns></returns>
      protected virtual string the_category(string separator)
      {
         return RenderCategoriesLinks(currentPost.Categories, separator);
      }

      // TODO: manage the parents!!
      protected virtual string the_category(string separator, string parents)
      {
         return the_category(separator);
      }

      protected string RenderCategoriesLinks(IList<Category> list, string separator)
      {
         StringBuilder categories = new StringBuilder();

         // TODO: manage the case when there are no categories (manage uncategorized)

         int index = 0;
         foreach (Category category in list)
         {
            categories.AppendFormat("<a rel=\"category\" href=\"{0}\">{1}</a>", GetAbsoluteUrl(category.GetCategoryUrl()), Html.Encode(category.Name));

            if (index < (currentPost.Categories.Count - 1))
            {
               categories.Append(separator);
            }

            index++;
         }

         return categories.ToString();
      }

      #region Comments & Pings

      protected bool have_comments()
      {
         return currentPost.Comments.Count > 0;
      }

      /// <summary>
      /// Loads the comment template. For use in single post and page displays
      /// </summary>
      protected void comments_template()
      {
         Html.RenderPartial(GetViewVirtualPath(ViewHelper.TemplateFile.comments), Model);
      }

      /// <summary>
      /// Displays the type of comment (regular comment, Trackback or Pingback) a comment entry is. 
      /// This tag must be within The Loop, or a comment loop
      /// </summary>
      /// <returns></returns>
      protected string comment_type(string commentText, string trackbackText, string pingbackText)
      {
         if (string.IsNullOrEmpty(commentText))
         {
            commentText = Resource("Comment_Type_Comment");
         }

         if (string.IsNullOrEmpty(trackbackText))
         {
            trackbackText = Resource("Comment_Type_Trackback");
         }

         if (string.IsNullOrEmpty(pingbackText))
         {
            pingbackText = Resource("Comment_Type_Pingback");
         }

         switch (currentComment.Type)
         {
            case CommentType.Pingback:
               return pingbackText;
               break;
            case CommentType.Trackback:
               return trackbackText;
               break;
            case CommentType.Comment:
            default:
               return commentText;
               break;
         }
      }

      /// <summary>
      /// Generates two hidden inputs for the comment form to identify 
      /// the comment_post_ID (the Post ID) and comment_parent for threaded comments
      /// (the last is not yet supported now)
      /// </summary>
      /// <returns></returns>
      protected string comment_id_fields()
      {
         return "<input type=\"hidden\" id=\"comment_post_ID\" value=\"" + currentPost.Id.ToString() + "\" name=\"comment_post_ID\"/>";
         // <input type="hidden" value="0" id="comment_parent" name="comment_parent"/>
      }

      protected string get_comment_author_url()
      {
         return currentComment.Url;
      }

      protected string comment_author_url()
      {
         return get_comment_author_url();
      }

      protected string comment_author_link()
      {
         return get_comment_author_link();
      }

      protected string get_comment_author_link()
      {
         string url = get_comment_author_url();
         string author = get_comment_author();

         if (string.IsNullOrEmpty(url) ||
             url == "http://")
         {
            return author;
         }
         else
         {
            return string.Format("<a href='{0}' rel='external nofollow' class='url'>{1}</a>", url, author);
         }
      }

      protected string get_comment_author()
      {
         if (string.IsNullOrEmpty(currentComment.AuthorName))
         {
            return Resource("Comment_Author_Anonymous");
         }
         else
         {
            return currentComment.AuthorName;
         }
      }

      protected string comment_date()
      {
         return get_comment_date();
      }

      protected string get_comment_date()
      {
         if (is_user_logged_in())
         {
            return currentComment.CreatedDate.AdjustDateToTimeZone((Context.User as User).TimeZone).ToLongDateString();
         }
         else
         {
            return currentComment.CreatedDate.AdjustDateToServerTimeZone(Model.Site.TimeZone).ToLongDateString();
         }
      }

      protected string comment_time()
      {
         return get_comment_time();
      }

      protected string get_comment_time()
      {
         if (is_user_logged_in())
         {
            return currentComment.CreatedDate.AdjustDateToTimeZone((Context.User as User).TimeZone).ToShortTimeString();
         }
         else
         {
            return currentComment.CreatedDate.AdjustDateToServerTimeZone(Model.Site.TimeZone).ToShortTimeString();
         }
      }

      protected string post_comments_feed_link(string link_text)
      {
         return post_comments_feed_link(link_text, null, null);
      }

      protected string post_comments_feed_link(string link_text, int? post_id, string feed)
      {
         return string.Format("<a href='{0}'>{1}</a>", GetAbsoluteUrl(currentPost.GetContentUrl()) + "feed/", link_text);
      }

      protected string comment_class(int index)
      {
         string odd = string.Empty;
         string thread_alt = string.Empty;

         if (index % 2 != 0)
         {
            odd = "odd";
            thread_alt = "alt thread-alt";
         }

         return string.Format(" class=\"comment {1} byuser comment-author-admin bypostauthor thread-{1} {2} depth-1\" ", currentComment.CommentId.ToString(), odd, thread_alt);
      }

      /// <summary>
      /// Displays a link to the comments popup window if comments_popup_script() is used, 
      /// otherwise it displays a normal link to comments
      /// </summary>
      /// <param name="zero">Text to display when there are no comments. Defaults to 'No Comments'</param>
      /// <param name="one">Text to display when there is one comment. Defaults to '1 Comment'</param>
      /// <param name="more">Text to display when there are more than one comments. '%' is replaced by the number of comments, so '% so far' is displayed as "5 so far" when there are five comments. Defaults to '% Comments'</param>
      /// <param name="cssClass">CSS (stylesheet) class for the link. This has no default value</param>
      /// <param name="none">Text to display when comments are disabled. Defaults to 'Comments Off'</param>
      /// <returns></returns>
      protected string comments_popup_link(string zero, string one, string more, string cssClass, string none)
      {
         int count = currentPost.Comments.Count;

         if (string.IsNullOrEmpty(zero))
         {
            zero = Resource("Comment_CommentsNumber_Zero");
         }

         if (string.IsNullOrEmpty(one))
         {
            one = Resource("Comment_CommentsNumber_One");
         }

         if (string.IsNullOrEmpty(more))
         {
            more = Resource("Comment_CommentsNumber_More");
         }

         if (string.IsNullOrEmpty(none))
         {
            none = Resource("Comment_CommentsNumber_Off");
         }

         string text;
         switch (count)
         {
            case 0:
               text = zero;
               break;
            case 1:
               text = one;
               break;
            default:
               text = more.Replace("%", count.ToString());
               // manage comments off!!!!
               break;
         }

         string nameLink = count == 0 ? "#respond" : "#comments";
         string css = string.IsNullOrEmpty(cssClass) ? string.Empty : " class=\"" + cssClass + "\" ";
         string link = string.Format("<a title=\"{0}\" href=\"{1}{3}\"{4}>{2}</a>", Resource("Comment_Link_CommentOn") + Html.Encode(currentPost.Title), GetAbsoluteUrl(currentPost.GetContentUrl()), // TODO: manage comments!!!
            text, nameLink, css);

         // TODO: manage comments disabled!!!
         return link;
      }

      protected string comments_popup_link(string zero, string one, string more)
      {
         return comments_popup_link(zero, one, more, null, null);
      }

      /// <summary>
      /// Displays the total number of comments, Trackbacks, and Pingbacks for a pos
      /// </summary>
      /// <returns></returns>
      protected string comments_number(string zero, string one, string more)
      {
         switch (currentPost.Comments.Where(c => c.Status == CommentStatus.Approved).Count())
         {
            case 0:
               return zero;
            case 1:
               return one;
            default:
               return more.Replace("%", currentPost.Comments.Count.ToString());
         }
      }

      /// <summary>
      /// Displays all comments for a post or Page
      /// </summary>
      /// <returns></returns>
      protected string wp_list_comments(string args)
      {
         string walker;
         int max_depth;
         string style = "ul"; // 'div', 'ol', or 'ul'
         string callback; // see http://codex.wordpress.org/Template_Tags/wp_list_comments
         string end_callback;

         // TODO: manage arg "type"
         string type = "all"; // The type of comment(s) to display. Can be 'all', 'comment', 'trackback', 'pingback', or 'pings'. 'pings' is 'trackback' and 'pingback' together.
         string page;
         string per_page;
         int avatar_size = 32; // Size that the avatar should be shown as, in pixels. Default: 32. http://gravatar.com/ supports sizes between 1 and 512
         bool reverse_top_level;
         bool reverse_children;

         // Parse the arguments
         IDictionary<String, String> arguments = GetDictionaryFromQueryStringArray(args);

         // Set defaults if argument missings
         if (arguments.ContainsKey("walker"))
         {
            walker = arguments["walker"];
         }

         if (arguments.ContainsKey("max_depth"))
         {
            max_depth = Convert.ToInt32(arguments["max_depth"]);
         }

         if (arguments.ContainsKey("style"))
         {
            style = arguments["style"];

            if (!style.Equals("div", StringComparison.InvariantCultureIgnoreCase) && !style.Equals("ol", StringComparison.InvariantCultureIgnoreCase) &&
                !style.Equals("ul", StringComparison.InvariantCultureIgnoreCase))
            {
               throw new Core.Exceptions.ApplicationException("wp_list_comments: invalid value (" + style + ") for 'style' argument");
            }
         }

         if (arguments.ContainsKey("type"))
         {
            type = arguments["type"];

            if (!style.Equals("all", StringComparison.InvariantCultureIgnoreCase) && !style.Equals("comment", StringComparison.InvariantCultureIgnoreCase) && !style.Equals("pingback", StringComparison.InvariantCultureIgnoreCase) && !style.Equals("pings", StringComparison.InvariantCultureIgnoreCase) &&
                !style.Equals("trackback", StringComparison.InvariantCultureIgnoreCase))
            {
               throw new Core.Exceptions.ApplicationException("wp_list_comments: invalid value (" + type + ") for 'type' argument");
            }
         }

         if (arguments.ContainsKey("avatar_size"))
         {
            avatar_size = Convert.ToInt32(arguments["avatar_size"]);
         }

         StringBuilder html = new StringBuilder();

         int index = 0;

         IEnumerable<Comment> commentsToDisplay = from c in currentPost.Comments
                                                  where c.Status == CommentStatus.Approved
                                                  select c;

         foreach (Comment comment in commentsToDisplay)
         {
            string odd = "even";
            string thread_alt = "";

            if (index % 2 != 0)
            {
               odd = "odd";
               thread_alt = "alt thread-alt";
            }

            // TODO: use comment_class
            html.AppendFormat("<li id=\"comment-{0}\" class=\"comment {1} byuser comment-author-admin bypostauthor thread-{1} {2} depth-1\">", comment.CommentId.ToString(), odd, thread_alt);
            html.AppendFormat("<div class=\"comment-body\" id=\"div-comment-{0}\">", comment.CommentId.ToString());
            html.Append("<div class=\"comment-author vcard\">");

            html.Append(Html.GravatarImage(comment.Email, avatar_size));
            html.Append("<cite class=\"fn\">" + Html.Encode(comment.AuthorName) + "</cite>&nbsp;<span class=\"says\">says:</span>");
            html.Append("</div>");

            html.Append("<div class=\"comment-meta commentmetadata\">");

            html.AppendFormat("<a href=\"{0}#comment-{1}\">", GetAbsoluteUrl(currentPost.GetContentUrl()), comment.CommentId.ToString());
            // <a title=\"Edit comment\" href=\"http://localhost/wordpress/wp-admin/comment.php?action=editcomment&amp;c=2\" class=\"comment-edit-link\">(Edit)</a>");
            html.AppendFormat("{0} at {1}", comment.CreatedDate.ToLongDateString(), comment.CreatedDate.ToShortTimeString());
            html.Append("</a></div>");
            html.AppendFormat("<p>{0}</p>", Html.Encode(comment.CommentText));
            //      <div class="reply">
            //            </div>
            //            </div>
            html.Append("</li>");

            index++;
         }

         return html.ToString();
      }

      protected string wp_list_comments()
      {
         return wp_list_comments(string.Empty);
      }

      /// <summary>
      /// Checks if comments are allowed for the current Post being processed
      /// (check Post.AllowComments property)
      /// </summary>
      /// <returns></returns>
      protected bool comments_open()
      {
         return currentPost.AllowComments;
      }

      /// <summary>
      /// checks if trackbacks & pingbacks are allowed for the current Post being processed 
      /// </summary>
      /// <returns></returns>
      protected bool pings_open()
      {
         return currentPost.AllowPings;
      }

      protected string trackback_url()
      {
         return GetCurrentSiteUrlRoot() + "/trackback.axd?id=" + the_ID() + "&url=" + the_permalink();
      }

      protected string trackback_rdf()
      {
         if (!string.IsNullOrEmpty(Request.UserAgent) &&
             Request.UserAgent.IndexOf("W3C_Validator") == -1)
         {
            const string rdf = @"
<rdf:RDF xmlns:rdf=""http://www.w3.org/1999/02/22-rdf-syntax-ns#""
   xmlns:dc=""http://purl.org/dc/elements/1.1/""
   xmlns:trackback=""http://madskills.com/public/xml/rss/module/trackback/"">
	<rdf:Description rdf:about=""{0}""
		  dc:identifier=""{0}""
		  dc:title=""{1}"" 
		  trackback:ping=""{2}"" />
</rdf:RDF>";

            return string.Format(rdf, the_permalink(), the_title().Replace("--", "&#x2d;&#x2d;").StripHtml(), trackback_url());
         }
         return string.Empty;
      }

      /// <summary>
      /// Displays text based on comment reply status. This only affects users with Javascript disabled
      /// </summary>
      /// <param name="noreplytext">Text to display when not replying to a comment. Default is 'Leave a Reply' </param>
      /// <param name="replytext">Text to display when replying to a comment. Accepts "%s" for the author of the comment being replied to. Default is 'Leave a Reply to %s' </param>
      /// <param name="linktoparent">Boolean to control making the author's name a link to their comment. Default is TRUE.</param>
      /// <returns></returns>
      protected string comment_form_title(string noreplytext, string replytext, bool linktoparent)
      {
         if (string.IsNullOrEmpty(noreplytext))
         {
            noreplytext = "Leave a Reply";
         }

         if (string.IsNullOrEmpty(replytext))
         {
            replytext = "Leave a Reply to %s";
         }

         // TODO: gestire qua linktoparent e replytext

         //replytext = replytext.Replace("%s", )

         return noreplytext;
      }

      protected string comment_form_title(string noreplytext, string replytext)
      {
         return comment_form_title(noreplytext, replytext, true);
      }

      protected string comment_form_title()
      {
         return comment_form_title("", "", true);
      }

      #endregion

      #endregion

      #region WP Default Navigation

      /// <summary>
      /// This creates a link to the previous posts. 
      /// Yes, it says "next posts," but it's named that just to confuse you. 
      /// It assumes that your posts are displaying in reverse chronological order 
      /// (most recent posts first) causing the next page to show posts from earlier in the timeline.
      /// </summary>
      /// <param name="label"></param>
      /// <param name="max_pages"></param>
      /// <returns></returns>
      protected string next_posts_link(string label, int max_pages)
      {
         if (string.IsNullOrEmpty(label))
         {
            label = Resource("Pager_Simple_NewerEntries");
         }

         int pageSize = Model.Posts.PageSize;
         int currentPage = Model.Posts.PageNumber;
         long totalItemCount = Model.Posts.TotalItemCount;

         int pageCount = (int)Math.Ceiling(totalItemCount / (double)pageSize);

         // if there is only 1 page, don't show the pager
         if (pageCount <= 1)
         {
            return string.Empty;
         }

         // Next
         if (currentPage < pageCount)
         {
            return GenerateNavigationLink(label, currentPage + 1);
         }

         return string.Empty;
      }

      protected virtual string next_posts_link(string label)
      {
         return next_posts_link(label, 0);
      }

      protected string next_posts_link()
      {
         return next_posts_link(null, 0);
      }

      /// <summary>
      /// This creates a link to the next posts. 
      /// Yes, it says "previous posts," but it's named that just to confuse you. 
      /// It assumes that your posts are displaying in reverse chronological order 
      /// (most recent posts first) causing the previous page to show posts from later in the timeline.
      /// </summary>
      /// <returns></returns>
      protected string previous_posts_link(string label, int max_pages)
      {
         if (string.IsNullOrEmpty(label))
         {
            label = Resource("Pager_Simple_OlderEntries");
         }

         int pageSize = Model.Posts.PageSize;
         int currentPage = Model.Posts.PageNumber;
         long totalItemCount = Model.Posts.TotalItemCount;

         int pageCount = (int)Math.Ceiling(totalItemCount / (double)pageSize);

         // if there is only 1 page, don't show the pager
         if (pageCount <= 1)
         {
            return string.Empty;
         }

         // Previous
         if (currentPage > 1)
         {
            return GenerateNavigationLink(label, currentPage - 1);
         }

         return string.Empty;
      }

      protected virtual string previous_posts_link(string label)
      {
         return previous_posts_link(label, 0);
      }

      protected string previous_posts_link()
      {
         return previous_posts_link(null, 0);
      }

      protected string GenerateNavigationLink(string linkText, int pageNumber)
      {
         var pageLinkValueDictionary = new RouteValueDictionary();
         pageLinkValueDictionary.Add("page", pageNumber);
         var virtualPathData = this.ViewContext.RouteData.Route.GetVirtualPath(this.ViewContext.RequestContext, pageLinkValueDictionary);

         if (virtualPathData != null)
         {
            string url = string.Concat(this.ViewContext.RequestContext.HttpContext.Request.Url.GetLeftPart(UriPartial.Authority), "/", virtualPathData.VirtualPath);

            // Fix url canonicalization: ensure that the path has the ending slash
            if (url.IndexOf('?') > -1 &&
                url.IndexOf("/?") == -1)
            {
               url = url.Insert(url.IndexOf('?'), "/");
            }

            // if we are rendering the search result, there is the "s" params in the request
            if (!string.IsNullOrEmpty(this.ViewContext.HttpContext.Request.Params["s"]))
            {
               url += "&s=" + this.ViewContext.HttpContext.Request.Params["s"];
            }

            return String.Format("<a href=\"{0}\">{1}</a>", url, linkText);
         }
         else
         {
            return null;
         }
      }

      #endregion

      #region wp_page_navi

      protected string wp_pagenavi(string before, string after)
      {
         return Html.Pager(Model.Posts.PageSize, Model.Posts.PageNumber, Model.Posts.TotalItemCount);
      }

      protected string wp_pagenavi()
      {
         return wp_pagenavi(string.Empty, string.Empty);
      }

      #endregion

      #region Dynamic Sidebar

      protected bool dynamic_sidebar()
      {
         return dynamic_sidebar(0);
      }

      protected bool dynamic_sidebar(int index)
      {
         return dynamic_sidebar(index, null, null, null, null);
      }

      protected bool dynamic_sidebar(string before_widget, string after_widget, string before_title, string after_title)
      {
         return dynamic_sidebar(0, before_widget, after_widget, before_title, after_title);
      }

      /// <summary>
      /// Render all the registered widgets in the sidebar placeholder.
      /// Each widgetcomponent must render each items in li tags
      /// </summary>
      /// <param name="index"></param>
      /// <param name="before_widget"></param>
      /// <param name="after_widget"></param>
      /// <param name="before_title"></param>
      /// <param name="after_title"></param>
      /// <returns></returns>
      protected bool dynamic_sidebar(int index, string before_widget, string after_widget, string before_title, string after_title)
      {
         // TODO: support multiple sidebar

         if (string.IsNullOrEmpty(before_widget))
         {
            before_widget = "<li id=\"%1$s\" class=\"widget %2$s\">";
         }
         if (string.IsNullOrEmpty(after_widget))
         {
            after_widget = "</li>";
         }
         if (string.IsNullOrEmpty(before_title))
         {
            before_title = "<h2 class=\"widgettitle\">";
         }
         if (string.IsNullOrEmpty(after_title))
         {
            after_title = "</h2>";
         }

         before_widget = before_widget.Replace("%1$s", "{0}").Replace("%2$s", "{1}");

         // Filter only for sidebar
         IEnumerable<IWidgetComponent> registeredWidgetComponents = from w in Model.WidgetComponents
                                                                    where w.Widget.PlaceHolder == WidgetPlaceHolder.sidebar
                                                                    select w;

         if (registeredWidgetComponents.Count() == 0)
         {
            return false;
         }

         StringBuilder html = new StringBuilder();

         foreach (IWidgetComponent wc in registeredWidgetComponents)
         {
            html.AppendFormat(before_widget, wc.Widget.Type.Name.ToLower() + "-" + wc.Widget.WidgetId.ToString(), "widget_" + wc.Widget.Type.Name.ToLower());
            html.Append(before_title);
            html.Append(Html.Encode(wc.Widget.Title));
            html.AppendLine(after_title);

            string widgetHtml = wc.Render();
            if (!string.IsNullOrEmpty(widgetHtml))
            {
               //html.AppendLine("<ul>");
               html.AppendLine(widgetHtml);
               //html.AppendLine("</ul>");
            }

            html.AppendLine(after_widget);
         }

         Context.Response.Write(html.ToString());

         return true;
      }

      #endregion

      /// <summary>
      /// This function returns the values of the custom fields with the specified key from the specified pos
      /// </summary>
      /// <param name="post_id">
      /// The post_id.
      /// </param>
      /// <param name="key">
      /// The key.
      /// </param>
      /// <param name="single">
      /// [ignored]
      /// If $single is set to false, or left blank, the function returns an array containing all values of the specified key.
      /// If $single is set to true, the function returns the first value of the specified key (not in an array)
      /// </param>
      /// <returns>
      /// </returns>
      protected string get_post_meta(int post_id, string key, bool single)
      {
         if (this.is_page())
         {
            Page page = (from p in Model.Pages
                         where p.Id == post_id
                         select p).SingleOrDefault();

            if (page != null && page.CustomFields.Count > 0 && page.CustomFields.ContainsKey(key))
               return page.CustomFields[key];
         }
         else if (this.is_post())
         {
            Post post = (from p in Model.Posts
                         where p.Id == post_id
                         select p).SingleOrDefault();

            if (post != null && post.CustomFields.Count > 0 && post.CustomFields.ContainsKey(key))
               return post.CustomFields[key];
         }

         return string.Empty;
      }

      ///// <summary>
      ///// This function returns the values of the custom fields with the specified key from the specified pos
      ///// </summary>
      ///// <param name="post_id"></param>
      ///// <param name="key"></param>
      ///// <param name="single">
      ///// If $single is set to false, or left blank, the function returns an array containing all values of the specified key.
      ///// If $single is set to true, the function returns the first value of the specified key (not in an array)
      ///// </param>
      ///// <returns></returns>
      //protected string[] get_post_meta(int post_id, string key, bool single)
      //{
      //   if (this.have_posts())
      //   {
      //      Post post = (from p in Model.Posts
      //                   where p.Id == post_id
      //                   select p).SingleOrDefault();

      //      if (post != null && post.CustomFields.Count > 0 && post.CustomFields.ContainsKey(key))
      //         return new string[1]{ post.CustomFields[key] };
      //   }

      //   return new string[];
      //}


      #region "wp_enqueue_style" & "" & custom function "wp_enqueue_style_render"

      /// <summary>
      /// Enqueue a CSS style file
      /// </summary>
      /// <param name="handle">Name of the stylesheet</param>
      /// <param name="src">Path to the stylesheet from the root directory </param>
      protected void wp_enqueue_style(string handle, string src)
      {
         wp_enqueue_style(handle, src, string.Empty, "v1", "screen");
      }



      /// <summary>
      /// Enqueue a CSS style file
      /// </summary>
      /// <param name="handle">Name of the stylesheet</param>
      /// <param name="src">Path to the stylesheet from the root directory </param>
      /// <param name="deps">rray of handles of any stylesheet that this stylesheet depends on; stylesheets that must be loaded before this stylesheet. false if there are no dependencies</param>
      /// <param name="ver">
      /// String specifying the stylesheet version number, if it has one. This parameter is used to ensure that the correct version is sent to the 
      /// client regardless of caching, and so should be included if a version number is available and makes sense for the stylesheet.
      /// </param>
      /// <param name="media">
      /// String specifying the media for which this stylesheet has been defined. Examples: 'all', 'screen', 'handheld', 'print'
      /// </param>
      protected void wp_enqueue_style(string handle, string src, string deps, string ver, string media)
      {
         log.DebugFormat("WordPressCompatibility.wp_enqueue_style: src = {0}", src);

         // get a FileInfo from the full path
         string virtualSrc = string.Concat(VirtualPathUtility.ToAppRelative(Model.Site.Template.BasePath), src);
         string defaultPath = virtualSrc.Substring(0, virtualSrc.LastIndexOf(Path.AltDirectorySeparatorChar));

         FileInfo stylesheetFileInfo = new FileInfo(Context.Server.MapPath(virtualSrc));

         Html.Telerik().StyleSheetRegistrar()
                           .DefaultGroup(dg => dg.DefaultPath(defaultPath)
                              .Add(stylesheetFileInfo.Name)
                           );
      }



      /// <summary>
      /// This render all the styles registered with wp_enqueue_style.
      /// It must be called after *all* the wp_enqueue_style calls.
      /// </summary>
      protected void wp_enqueue_style_render()
      {
         // render the registered scripts
         Html.Telerik().StyleSheetRegistrar().Render();
      }



      /// <summary>
      /// A safe way of adding javascripts to a WordPress generated page
      /// </summary>
      /// <remarks>
      /// See:
      ///   http://codex.wordpress.org/Function_Reference/wp_enqueue_script
      ///   http://weblogtoolscollection.com/archives/2010/05/06/adding-scripts-properly-to-wordpress-part-1-wp_enqueue_script/
      /// 
      /// Note: this overload load scripts from the ~/Content/ folder
      /// </remarks>
      /// <param name="handle">Name of the script. Lowercase string</param>
      protected void wp_enqueue_script(string handle)
      {
         //wp_enqueue_script(handle, src, string.Empty, "v1", "true");
         log.DebugFormat("WordPressCompatibility.wp_enqueue_script: handle = {0}", handle);

         // get a FileInfo from the full path
         //string virtualSrc = Url.Content("~/Content/js/" + handle + ".js");  //string.Concat(VirtualPathUtility.ToAppRelative(Model.Site.Template.BasePath), src);
         //string defaultPath = virtualSrc.Substring(0, virtualSrc.LastIndexOf(Path.AltDirectorySeparatorChar));

         //// get a FileInfo from the full path
         //FileInfo scriptFileInfo = new FileInfo(Context.Server.MapPath(virtualSrc));

         Html.Telerik().ScriptRegistrar()
                           .DefaultGroup(dg => dg.DefaultPath("~/Content/js/")
                                                 .Add(handle + ".js")
                           );
      }



      /// <summary>
      /// A safe way of adding javascripts to a WordPress generated page
      /// </summary>
      /// <remarks>
      /// See:
      ///   http://codex.wordpress.org/Function_Reference/wp_enqueue_script
      ///   http://weblogtoolscollection.com/archives/2010/05/06/adding-scripts-properly-to-wordpress-part-1-wp_enqueue_script/
      /// </remarks>
      /// <param name="handle">Name of the script. Lowercase string</param>
      /// <param name="src">URL to the script</param>
      protected void wp_enqueue_script(string handle, string src)
      {
         wp_enqueue_script(handle, src, string.Empty, "v1", "true");
      }



      /// <summary>
      /// A safe way of adding javascripts to a WordPress generated page
      /// </summary>
      /// <remarks>
      /// See:
      ///   http://codex.wordpress.org/Function_Reference/wp_enqueue_script
      ///   http://weblogtoolscollection.com/archives/2010/05/06/adding-scripts-properly-to-wordpress-part-1-wp_enqueue_script/
      /// </remarks>
      /// <param name="handle">Name of the script. Lowercase string</param>
      /// <param name="src">URL to the script</param>
      /// <param name="deps">Array of handles of any script that this script depends on</param>
      /// <param name="ver">
      /// String specifying the script version number, if it has one. Defaults to false. 
      /// This parameter is used to ensure that the correct version is sent to the client regardless of caching, 
      /// and so should be included if a version number is available and makes sense for the script
      /// </param>
      /// <param name="in_footer">
      /// Normally scripts are placed in the <head> section. If this parameter is true the script is placed at the bottom of the <body>. 
      /// This requires the theme to have the wp_footer() hook in the appropriate place. Note that you have to enqueue your script before wp_head is run, 
      /// even if it will be placed in the footer. 
      /// </param>
      /// <returns></returns>
      protected void wp_enqueue_script(string handle, string src, string deps, string ver, string in_footer)
      {
         log.DebugFormat("WordPressCompatibility.wp_enqueue_script: src = {0}", src);

         // get a FileInfo from the full path
         string virtualSrc = string.Concat(VirtualPathUtility.ToAppRelative(Model.Site.Template.BasePath), src);
         string defaultPath = virtualSrc.Substring(0, virtualSrc.LastIndexOf(Path.AltDirectorySeparatorChar));

         // get a FileInfo from the full path
         FileInfo scriptFileInfo = new FileInfo(Context.Server.MapPath(virtualSrc));

         Html.Telerik().ScriptRegistrar()
                           .DefaultGroup(dg => dg.DefaultPath(defaultPath)
                                                 //.Combined(!Context.IsDebuggingEnabled)
                                                 .Add(scriptFileInfo.Name)
                           );
      }

      #endregion

      #endregion
   }
}