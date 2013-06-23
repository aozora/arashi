namespace Arashi.Web.Areas.Admin.Controllers
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Web.Mvc;

   using Arashi.Core.Domain;
   using Arashi.Core.Extensions;
   using Arashi.Services.Content;
   using Arashi.Services.Localization;
   using Arashi.Services.Membership;
   using Arashi.Services.SiteStructure;
   using Arashi.Services.Themes;
   using Arashi.Web.Mvc.Controllers;
   using Arashi.Web.Mvc.Filters;
   using Arashi.Web.Mvc.Models;
   using Arashi.Web.Themes.Boldy.Models;

   using Common.Logging;

   public class BoldyController : SecureControllerBase
   {
      private ILog log;
      private readonly IContentItemService<Page> contentItemService;

      #region Constructor

      public BoldyController(ILog log,IContentItemService<Page> contentItemService, IThemeService themeService, ILocalizationService localizationService, IUserService userService, ISiteService siteService)
         : base(log, localizationService, userService, siteService)
      {
         this.log = log;
         this.contentItemService = contentItemService;
      }

      #endregion

      /// <summary>
      /// Show the Boldy theme Home Page editor
      /// </summary>
      /// <returns></returns>
      [AcceptVerbs(HttpVerbs.Get)]
      [PermissionFilter(RequiredRights = Rights.SiteSettingsEdit)]
      public ActionResult Index()
      {

         //$options = array ('logo_img', 'logo_alt','contact_email','contact_text','cufon','linkedin_link','twitter_user',
         //                  'latest_tweet','facebook_link','keywords','description','analytics','copyright','home_box1',
         //                  'home_box1_link','home_box2','home_box2_link','home_box3','home_box3_link','blurb_enable','blurb_text',
         //                  'blurb_link','blurb_page', 'footer_actions','actions_hide','portfolio','blog','slider');


         IList<Page> pages = GetParentPages(false); // get the pages in flat tree list

         BoldyOptionsModel model = new BoldyOptionsModel()
            {
               LogoPath = GetOption("boldy_logo_img"),
               LogoAlt = GetOption("boldy_logo_alt"),
               EnableCufonFontReplacement = string.IsNullOrEmpty(GetOption("boldy_cufon")) ? false : Convert.ToBoolean(GetOption("boldy_cufon")),
               HomePageSliderImagesPage = ConvertBoldyOptionToPage(GetOption("boldy_slider"), pages),
               PortfolioPage = ConvertBoldyOptionToPage(GetOption("boldy_portfolio"), pages),
               HomeBox1Page = ConvertBoldyOptionToPage(GetOption("boldy_home_box1"), pages),
               HomeBox1ReadMoreLink = GetOption("boldy_home_box1_link"),
               HomeBox2Page = ConvertBoldyOptionToPage(GetOption("boldy_home_box2"), pages),
               HomeBox2ReadMoreLink = GetOption("boldy_home_box2_link"),
               HomeBox3Page = ConvertBoldyOptionToPage(GetOption("boldy_home_box3"), pages),
               HomeBox3ReadMoreLink = GetOption("boldy_home_box3_link"),
               DisplayHomepageBlurb = string.IsNullOrEmpty(GetOption("boldy_blurb_enable")) ? false : Convert.ToBoolean(GetOption("boldy_blurb_enable")),
               BlurbText = GetOption("boldy_blurb_text"),
               RequestQuotePage = ConvertBoldyOptionToPage(GetOption("boldy_blurb_page"), pages),
               RequestQuoteLink = GetOption("boldy_blurb_link"),
               Pages = pages
            };

         return View("Index", model);
      }


      /// <summary>
      /// Show the Boldy theme Home Page editor
      /// </summary>
      /// <returns></returns>
      [AcceptVerbs(HttpVerbs.Post)]
      [PermissionFilter(RequiredRights = Rights.SiteSettingsEdit)]
      public ActionResult Save(BoldyOptionsModel model)
      {
         try
         {
            if (ModelState.IsValid)
            {
               Site site = Context.ManagedSite;

               site.Options["boldy_logo_img"] = model.LogoPath;
               site.Options["boldy_logo_alt"] = model.LogoAlt;
               site.Options["boldy_cufon"] = model.EnableCufonFontReplacement.ToString().ToLowerInvariant();
               site.Options["boldy_slider"] = model.HomePageSliderImagesPage == null ? string.Empty : model.HomePageSliderImagesPage.Id.ToString();
               site.Options["boldy_portfolio"] = model.PortfolioPage == null ? string.Empty : model.PortfolioPage.Id.ToString();
               site.Options["boldy_home_box1"] = model.HomeBox1Page == null ? string.Empty : model.HomeBox1Page.Id.ToString();
               site.Options["boldy_home_box1_link"] = model.HomeBox1ReadMoreLink;
               site.Options["boldy_home_box2"] = model.HomeBox1Page == null ? string.Empty : model.HomeBox2Page.Id.ToString();
               site.Options["boldy_home_box2_link"] = model.HomeBox2ReadMoreLink;
               site.Options["boldy_home_box3"] = model.HomeBox3Page == null ? string.Empty : model.HomeBox3Page.Id.ToString();
               site.Options["boldy_home_box3_link"] = model.HomeBox3ReadMoreLink;
               site.Options["boldy_blurb_enable"] = model.DisplayHomepageBlurb.ToString().ToLowerInvariant();
               site.Options["boldy_blurb_text"] = model.BlurbText;
               site.Options["boldy_blurb_page"] = model.RequestQuotePage == null ? string.Empty : model.RequestQuotePage.Id.ToString();
               site.Options["boldy_blurb_link"] = model.RequestQuoteLink;

               siteService.SaveSite(site);

               // Show the confirmation message
               MessageModel message = new MessageModel
               {
                  Text = "The options has been saved successfully!",
                  Icon = MessageModel.MessageIcon.Info,
                  CssClass = "margin-topbottom"
               };
               RegisterMessage(message, true);
            }
         }
         catch (Exception ex)
         {
            log.Error("BoldyController.Update", ex);

            MessageModel message = new MessageModel
            {
               Text = GlobalResource("Message_GenericError"),
               Icon = MessageModel.MessageIcon.Alert,
               CssClass = "margin-topbottom"
            };
            RegisterMessage(message);
         }

         model.Pages = GetParentPages(false); // get the pages in flat tree list

         return View("Index", model);
      }



      #region Helpers

      private string GetOption(string option)
      {
         if (Context.ManagedSite.Options.ContainsKey(option))
            return Context.ManagedSite.Options[option];
         else
            return string.Empty;
      }


      private Page ConvertBoldyOptionToPage(string option, IList<Page> pages)
      {
         // convert the option value to the page id. if the conversion fail return null.
         int pageId;
         if (!Int32.TryParse(option, out pageId))
            return null;

         // now search if the page exists...
         Page page = pages.Where(p => p.Id == pageId).SingleOrDefault();
         
         return page;
      }




      private IList<Page> GetParentPages(bool excludeCurrentPage /*, Page currentPage*/)
      {
         IList<Page> allPages = contentItemService.FindAllBySite(Context.ManagedSite, "Position", true);
         List<Page> tree = new List<Page>();

         IEnumerable<Page> topPages = from page in allPages
                                      where page.ParentPage == null
                                      select page;

         foreach (Page page in topPages)
         {
            IEnumerable<Page> flat = from p in page.AsDepthFirstEnumerable(x => x.ChildPages)
                                     select p;
            tree.AddRange(flat);
         }

         return tree;
      }



      //private IDictionary<string, string> SiteOptionsAsDictionary()
      //{
      //   IEnumerable<SiteOption> boldyOptions = Context.ManagedSite.Options.Where(o => o.Name.StartsWith("boldy"));

      //   IDictionary<string, string> dict = new Dictionary<string, string>();

      //   foreach (SiteOption boldyOption in boldyOptions)
      //   {
      //      dict.Add(boldyOption.Name, boldyOption.Value);
      //   }

      //   return dict;
      //}


      #endregion


   }
}
