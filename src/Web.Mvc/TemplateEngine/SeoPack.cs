using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Arashi.Core.Domain;
using Arashi.Core.Extensions;
using Arashi.Web.Mvc.Models;

namespace Arashi.Web.Mvc.TemplateEngine
{
   /// <summary>
   /// SeoPack
   /// This partial class implements the generation of elements 
   /// to provide SEO frienldy meta info.
   /// </summary>
   /// <typeparam name="TModel"></typeparam>
   public partial class TemplateBase<TModel> : ViewUserControl<TModel>
      where TModel : TemplateContentModel
   {
      // max char supported by most Search Engine
      private const int maximumDescriptionLength = 150;
      private const int minimumDescriptionLength = 1;


      /// <summary>
      /// Dynamically generate meta info in the head of the page
      /// </summary>
      /// <returns></returns>
      protected string wp_head()
      {
         // This is "inspired" by 'All-In-One SEO Pack' WP plugin
         string keywords = string.Empty;
         string description = string.Empty;
         StringBuilder meta = new StringBuilder();

         #region Generate meta keywords

         if ((is_home() && !string.IsNullOrEmpty(Model.Site.SeoSettings.HomeKeywords)
              && !is_static_posts_page())
             || is_static_front_page())
         {
            keywords = Model.Site.SeoSettings.HomeKeywords.EmptyIfNull().Trim();
         }
            //else if (is_static_posts_page() /*&& !aioseop_options['aiosp_dynamic_postspage_keywords']*/)
            //{  // and if option = use page set keywords instead of keywords from recent posts
            //   keywords = stripcslashes($this->internationalize(get_post_meta($post->ID, "_aioseop_keywords", true)));
            //}
         else
         {
            keywords = GetAllKeywords();
         }

         #endregion

         #region Generate meta description

         if (is_single() || is_page() || is_static_posts_page())
         {
            if (is_static_front_page())
               description = Model.Site.SeoSettings.HomeDescription.EmptyIfNull().Trim();
            else if (is_page())
               description = GetPostDescription(Model.CurrentPage.Content);
            else
               description = GetPostDescription(Model.Posts[0].Content);
         }
         else if (is_home())
         {
            description = Model.Site.SeoSettings.HomeDescription.EmptyIfNull().Trim();
         }
         else if (is_category())
         {
            description = Html.Encode(Model.CurrentCategory.Name);
         }

         // Set the default description
         if (string.IsNullOrEmpty(description) && !string.IsNullOrEmpty(Model.Site.Description))
            description = Model.Site.Description;

         // don't render the meta description:
         //    - on the home page if it is paginated
         //    only if the description length is > the minimum
         if (!string.IsNullOrEmpty(description) && 
             description.Length > minimumDescriptionLength && 
             !(is_home() && is_paged()) )
         {
            description = description.StripHtml().Trim()
               .Replace("\"", "")
               .Replace("\r\n", " ")   // replace newlines on mac / windows?
               .Replace("\n", " ");    // maybe linux uses this alone

            // description format
            string descriptionFormat = string.IsNullOrEmpty(Model.Site.SeoSettings.DescriptionFormat) ? "%description%" : Model.Site.SeoSettings.DescriptionFormat;

            description = descriptionFormat.Replace("%description%", description)
               .Replace("%blog_title%", get_bloginfo("name"))
               .Replace("%blog_description%", get_bloginfo("description"))
               .Replace("%wp_title%", GetOriginalTitle());

            // add the description
            meta.AppendFormat("<meta name=\"description\" content=\"{0}\" />\r\n", description);
         }

         #endregion

         // add the keywords
         if (!string.IsNullOrEmpty(keywords))
            meta.AppendFormat("<meta name=\"keywords\" content=\"{0}\" />\r\n", keywords);

         // render the noindex/follow
         if ( (is_category() && Model.Site.SeoSettings.UseNoIndexForCategories) ||
              ( !is_category() && is_archive() && !is_tag() && Model.Site.SeoSettings.UseNoIndexForArchives) ||
              ( Model.Site.SeoSettings.UseNoIndexForTags && is_tag()) )
         {
            meta.AppendLine("<meta name=\"robots\" content=\"noindex,follow\" />");
         }


         // render the canonical url
         if (!is_404() && !is_search())
         {
            //? Request.Url.GetLeftPart(UriPartial.Path)
            //"http://localhost:8080/author/marcell_op/"
            //? Request.Url.GetLeftPart(UriPartial.Authority)
            //"http://localhost:8080"
            //? Request.Url.GetLeftPart(UriPartial.Scheme)
            //"http://"
            //? Request.Url.GetLeftPart(UriPartial.Query)
            //"http://localhost:8080/author/marcell_op/?page=2"

            string currentUrl = Request.Url.ToString();

            if (!Request.Url.GetLeftPart(UriPartial.Path).EndsWith("/"))
               currentUrl = Request.Url.GetLeftPart(UriPartial.Path) + "/" + Request.QueryString.ToString();

            meta.AppendFormat("<link rel=\"canonical\" href=\"{0}\" />\r\n", currentUrl);
         }

         return meta.ToString();
      }




      /// <summary>
      /// Return the title of the page [SEO Friendly]  
      /// </summary>
      /// <param name="sep"></param>
      /// <param name="echo"></param>
      /// <param name="seplocation"></param>
      /// <returns></returns>
      protected string wp_title(string sep, bool echo, string seplocation)
      {
         if (is_home() && !is_static_posts_page() )
         {
            string title = Model.Site.SeoSettings.HomeTitle.EmptyIfNull();

            if (string.IsNullOrEmpty(title))
               title = get_option("blogname");

            return Html.Encode(title);
         }
         
         if (is_author())
         {
            string titleFormat = Model.Site.SeoSettings.ArchiveTitleFormat.EmptyIfNull();
            string title = titleFormat.Replace("%blog_title%", get_bloginfo("name"))
               .Replace("%blog_description%", get_bloginfo("description"))
               .Replace("%date%",Html.Encode(Model.CurrentAuthor.DisplayName));

            return title;
         }
         
         if (is_single())
         {
            string category = Model.CurrentCategory != null ? Html.Encode(Model.CurrentCategory.Name) : string.Empty;
            string titleFormat = Model.Site.SeoSettings.PostTitleFormat.EmptyIfNull();
            string title = titleFormat.Replace("%blog_title%", get_bloginfo("name"))
               .Replace("%blog_description%", get_bloginfo("description"))
               .Replace("%post_title%", Model.Posts.Count == 1 ? Html.Encode(Model.Posts[0].Title) : string.Empty)
               .Replace("%category%", category)
               .Replace("%category_title%", category);
            // TODO: add support for author
            //$new_title = str_replace('%post_author_login%', $authordata->user_login, $new_title);
            //$new_title = str_replace('%post_author_nicename%', $authordata->user_nicename, $new_title);
            //$new_title = str_replace('%post_author_firstname%', ucwords($authordata->first_name), $new_title);
            //$new_title = str_replace('%post_author_lastname%', ucwords($authordata->last_name), $new_title);

            return title;
         }
         
         if (is_search() && !string.IsNullOrEmpty(Request.QueryString["s"]))
         {
            string search = Context.Server.UrlDecode(Request.QueryString["s"]).Capitalize();

            string titleFormat = Model.Site.SeoSettings.SearchTitleFormat.EmptyIfNull();

            string title = titleFormat.Replace("%blog_title%", get_bloginfo("name"))
               .Replace("%blog_description%", get_bloginfo("description"))
               .Replace("%search%", search);

            return title;
         }
         
         if (is_category())
         {
            string category_name = single_cat_title("", false);
            string titleFormat = Model.Site.SeoSettings.CategoryTitleFormat.EmptyIfNull();
            string title = titleFormat.Replace("%category_title%", category_name)
               .Replace("%blog_title%", get_bloginfo("name"))
               .Replace("%blog_description%", get_bloginfo("description"));

            return title;
         }

         if (is_page() || is_static_posts_page())
         {
            // we're not in the loop :(
            //$authordata = get_userdata($post->post_author);
            string title;

            if (is_static_front_page())
            {
               if (!string.IsNullOrEmpty(Model.Site.SeoSettings.HomeTitle))
                  title = Model.Site.SeoSettings.HomeTitle;
               else
                  title = get_option("blogname");
            }
            else
            {
               // TODO: manage per-post meta
               //title = $this->internationalize(get_post_meta($post->ID, "_aioseop_title", true));
               //if (!$title) {
               title = get_option("blogname");
               //}

               string titleFormat = Model.Site.SeoSettings.PageTitleFormat.EmptyIfNull();
               string newTitle = titleFormat.Replace("%blog_title%", get_bloginfo("name"))
                                             .Replace("%blog_description%", get_bloginfo("description"))
                                             .Replace("%page_title%", Model.CurrentPage.Title);
               //$new_title = str_replace('%page_author_login%', $authordata->user_login, $new_title);
               //$new_title = str_replace('%page_author_nicename%', $authordata->user_nicename, $new_title);
               //$new_title = str_replace('%page_author_firstname%', ucwords($authordata->first_name), $new_title);
               //$new_title = str_replace('%page_author_lastname%', ucwords($authordata->last_name), $new_title);

               return Html.Encode(newTitle);
            }
         }

         
         if (is_tag())
         {
            string tag = single_tag_title("", true);
            string titleFormat = Model.Site.SeoSettings.TagTitleFormat.EmptyIfNull();
            string title = titleFormat.Replace("%blog_title%", get_bloginfo("name"))
               .Replace("%blog_description%", get_bloginfo("description"))
               .Replace("%tag%", tag);

            return title;
         }
         
         if (is_archive())
         {
            // TODO: fix se ho solo anno, o anche mese e giorno
            // TODO: check: should do the conversion for timezone?
            DateTime publishedDate = Model.Posts.First().PublishedDate.Value;

            string date = publishedDate.ToString("yyyy") + " " + publishedDate.ToString("MMMM").Capitalize();
            string titleFormat = Model.Site.SeoSettings.ArchiveTitleFormat.EmptyIfNull();
            string title = titleFormat.Replace("%blog_title%", get_bloginfo("name"))
               .Replace("%blog_description%", get_bloginfo("description"))
               .Replace("%date%", date);

            return title;
         }
         
         if (is_404())
         {
            string titleFormat = Model.Site.SeoSettings.Page404TitleFormat.EmptyIfNull();
            string title = titleFormat.Replace("%blog_title%", get_bloginfo("name"))
               .Replace("%blog_description%", get_bloginfo("description"))
               .Replace("%request_url%", Request.Url.ToString())
               .Replace("%request_words%", RequestUrlAsWords(Request.Url.ToString()))
               .Replace("%404_title%", Model.Site.Name)
               .Replace("%date%", this.is_302() ? "302" : "404");

            return title;
         }

         return Html.Encode(Model.Site.SeoSettings.HomeTitle);
      }



      /// <summary>
      /// Get a list of word contained in the current request url
      /// </summary>
      /// <param name="request"></param>
      /// <returns></returns>
      private string RequestUrlAsWords(string request)
      {
         request = request.Replace(Request.Url.GetLeftPart(UriPartial.Authority), string.Empty)
            .Replace(".html", " ")
            .Replace(".html", " ")
            .Replace("http:", string.Empty)
            .Replace(".htm", " ")
            .Replace(".", " ")
            .Replace("?", " ")
            .Replace(@"/", " ");

         string[] requestArray = request.Split(' ');

         requestArray.Each().Do(word => word.Capitalize());

         return string.Join(" ", requestArray);
      }



      /// <summary>
      /// Return comma-separated list of unique keywords
      /// </summary>
      /// <returns></returns>
      private string GetAllKeywords()
      {
         if (is_404())
            return null;

         // if we are on synthetic pages
         if (!is_home() && 
             !is_page() && 
             !is_single() && 
             !is_static_front_page() && 
             !is_static_posts_page())
            return null;

         IList<string> keywords = new List<string>();

         if (Model.Posts != null && Model.Posts.Count > 0) 
         {
            foreach (Post post in Model.Posts)
            {
               // custom field keywords
               //string keywords_a = null;
               //string keywords_i = null;
               //string description_a = null;
               //string description_i = null;

               //string id = post.Id.ToString(); // (is_attachment())?($post->post_parent):($post->ID); // if attachment then use parent post id
               // // TODO: manage post meta
               // keywords_i = stripcslashes($this->internationalize(get_post_meta($id, "_aioseop_keywords", true)));
               //$keywords_i = str_replace('"', '', $keywords_i);
               //if (isset($keywords_i) && !empty($keywords_i)) {
               //  $traverse = explode(',', $keywords_i);
               //  foreach ($traverse as $keyword) {
               //     $keywords[] = $keyword;
               //  }
               //}

               // add all tags
               foreach (Tag tag in post.Tags)
               {
                  keywords.Add(Html.Encode(tag.Name));
               }

               if (Model.Site.SeoSettings.UseCategoriesForMeta)
               {
                  foreach (Category category in post.Categories)
                  {
                     keywords.Add(Html.Encode(category.Name));
                  }
               }

            }
         }

         // convert to lower
         keywords.Each().Do(word => word.ToLower());

         // concatenate all with ","
         return string.Join(",", keywords.ToArray());
      }



      /// <summary>
      /// Get the meta description for a given post
      /// </summary>
      /// <param name="post"></param>
      /// <returns></returns>
      private string GetPostDescription(string content)
      {
         //global $aioseop_options;
         // custom desc for aiosep for single post
         //$description = trim(stripcslashes($this->internationalize(get_post_meta($post->ID, "_aioseop_description", true))));
         
         //if (!$description) {
         string description = content.StripHtml(); //$this->trim_excerpt_without_filters_full_length($this->internationalize($post->post_excerpt));

         if (description.Length > maximumDescriptionLength)
            description = description.Substring(0, maximumDescriptionLength); //$this->trim_excerpt_without_filters($this->internationalize($post->post_content));

         // whitespace trim
         Regex rx = new Regex(@"/\s\s+/");
         description = rx.Replace(description, " ");

         return description;
      }



      private string GetOriginalTitle()
      {
         string title = string.Empty;

         if (is_home())
         {
            title = get_option("blogname");
         }
         else if (is_single())
         {
            title = wp_title();
         }
         else if (is_search() && !string.IsNullOrEmpty(Request.Params["s"]))
         {
            title = Request.Params["s"].Capitalize();
         }
         else if (is_category() /*&& !is_feed()*/)
         {
            title = single_cat_title().Capitalize();
         }
         else if (is_page())
         {
            title = wp_title();
         }
         else if (is_tag())
         {
            title = wp_title();
         }
         else if (is_archive())
         {
            title = wp_title();
         }
         else if (is_404())
         {
            string titleFormat = Model.Site.SeoSettings.Page404TitleFormat;

            title = titleFormat.Replace("%blog_title%", get_bloginfo("name"))
               .Replace("%blog_description%", get_bloginfo("description"))
               .Replace("%request_url%", Request.Url.ToString())
               .Replace("%request_words%", RequestUrlAsWords(Request.Url.ToString()));
         }

         return title.Trim();
      }

   }
}