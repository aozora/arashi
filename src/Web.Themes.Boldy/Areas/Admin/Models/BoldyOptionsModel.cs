namespace Arashi.Web.Themes.Boldy.Models
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Text;
   using System.Web.Mvc;

   using Arashi.Core.Domain;

   public class BoldyOptionsModel
   {
      public string LogoPath { get; set; }
      public string LogoAlt { get; set; }
      public bool EnableCufonFontReplacement { get; set; }
      public Page HomePageSliderImagesPage { get; set; }
      public Page PortfolioPage { get; set; }

      // HomeBoxXPage is alternative to ReadMoreLink
      public Page HomeBox1Page { get; set; }
      public string HomeBox1ReadMoreLink { get; set; }
      public Page HomeBox2Page { get; set; }
      public string HomeBox2ReadMoreLink { get; set; }
      public Page HomeBox3Page { get; set; }
      public string HomeBox3ReadMoreLink { get; set; }

      // Homepage Blurb (request quote section) 
      public bool DisplayHomepageBlurb { get; set; }
      [AllowHtml]
      public string BlurbText { get; set; }
      public Page RequestQuotePage { get; set; }
      public string RequestQuoteLink { get; set; }

      // utility
      public IList<Page> Pages { get; set; }

   }
}
