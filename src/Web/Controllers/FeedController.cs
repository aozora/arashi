namespace Arashi.Web.Controllers
{
   using System;
   using System.Collections.Generic;
   using System.ServiceModel.Syndication;
   using System.Web;
   using System.Web.Mvc;
   using Arashi.Core.Domain;
   using Arashi.Services.Content;
   using Arashi.Services.Localization;
   using Arashi.Services.Membership;
   using Arashi.Services.Search;
   using Arashi.Services.SiteStructure;
   using Arashi.Core.Extensions;
   using Arashi.Web.Helpers;
   using Arashi.Web.Mvc;
   using Arashi.Web.Mvc.Controllers;
   using Arashi.Web.Mvc.Filters;
   using Common.Logging;
   using Arashi.Services.Widget;

   using Arashi.Services.Notification;



   /// <summary>
   /// FeedController
   /// </summary>
   [SeoUrlCanonicalization]
   public class FeedController : ContentControllerBase
   {
      private ILog log;



      public FeedController(  ILog log,
                              ILocalizationService localizationService,
                              ISiteService siteService,
                              IUserService userService,
                              IContentItemService<Post> contentItemService,
                              IContentItemService<Arashi.Core.Domain.Page> contentItemServiceForPage,
                              ICommentService commentService,
                              ICategoryService categoryService,
                              ITagService tagService,
                              ISearchService searchService,
                              IWidgetService widgetService,
                              IMessageService messageService)
         : base(log, localizationService, siteService, userService, contentItemService, contentItemServiceForPage, commentService, categoryService, tagService, searchService, widgetService, messageService)
      {
         this.log = log;
      }



      [NotModifiedFilter(Order = 1)]
      public ActionResult RssEntries()
      {
         SyndicationFeed feed = GetSyndicationItems();

         return new SyndicationActionResult()
                   {
                      Feed = feed,
                      Formatter = new Rss20FeedFormatter(feed)
                   };
      }



      [NotModifiedFilter(Order = 1)]
      public ActionResult AtomEntries()
      {
         SyndicationFeed feed = GetSyndicationItems();

         return new SyndicationActionResult()
         {
            Feed = feed,
            Formatter = new Atom10FeedFormatter(feed)
         };
      }



      /// <summary>
      /// Retrieve the list of the most recent post as SyndicationFeed items
      /// </summary>
      /// <returns></returns>
      private SyndicationFeed GetSyndicationItems()
      {
         Site site = Context.CurrentSite;

         SyndicationFeed feed = new SyndicationFeed();
         feed.Title = TextSyndicationContent.CreatePlaintextContent((site.Name));
         feed.Description = TextSyndicationContent.CreatePlaintextContent(HttpUtility.HtmlEncode(site.Description));
         feed.Links.Add(SyndicationLink.CreateAlternateLink(
                          new Uri(//GetFullyQualifiedUrl("~/Default.aspx")
                             Request.Url.GetLeftPart(UriPartial.Authority) + Url.Action("RssEntries", "Feed") // TODO: is correct ???
                          )));
         feed.Links.Add(SyndicationLink.CreateSelfLink(
                          new Uri(//GetFullyQualifiedUrl(Request.RawUrl))
                             WebHelper.GetSiteRoot() // TODO: is correct ???
                          )));
         //feed.Copyright = TextSyndicationContent.CreatePlaintextContent("Copyright xxxxx");
         feed.Language = Context.CurrentSite.DefaultCulture;

         IList<SyndicationItem> items = new List<SyndicationItem>();
         IList<Post> recentEntries = contentItemService.FindSyndicatedBySite(site, site.MaxSyndicationFeeds);
         
         foreach (Post post in recentEntries)
         {
            string content;

            if (Context.CurrentSite.FeedUseSummary)
               content = string.IsNullOrEmpty(post.Summary) ? post.Teaser : post.Summary.StripHtml();
            else
               content = post.Content.StripHtml();


            TextSyndicationContent syndicationContent = new TextSyndicationContent(HttpUtility.HtmlEncode(content), TextSyndicationContentKind.Html);

            SyndicationItem item = new SyndicationItem();
            item.Title = TextSyndicationContent.CreatePlaintextContent(HttpUtility.HtmlEncode(post.Title));
            item.Content = syndicationContent;
            item.Links.Add(SyndicationLink.CreateAlternateLink(new Uri(Request.Url.GetLeftPart(UriPartial.Authority) + "/" + post.GetContentUrl())));
            item.Id = post.Id.ToString();
            item.LastUpdatedTime = post.UpdatedDate;

            foreach (SyndicationPerson person in item.Authors)
            {
               SyndicationPerson authInfo = new SyndicationPerson();
               authInfo.Email = person.Email;
               authInfo.Name = person.Name;

               item.Authors.Add(authInfo);

               // RSS feeds can only have one author, so quit loop after first author has been added
               //if (outputRss)
               break; // exit always....
            }

            items.Add(item);
         }

         feed.Items = items;

         return feed;
      }


   }
}
