using System;

namespace Arashi.Core.Domain
{
   public class SeoSettings
   {
      public virtual int SeoSettingsId {get; set;}
      public virtual Site Site {get; set;}
      public virtual string HomeTitle {get; set;}
      public virtual string HomeDescription {get; set;}
      public virtual string HomeKeywords {get; set;}
      public virtual bool RewriteTitles {get; set;}
      public virtual string PostTitleFormat {get; set;}
      public virtual string PageTitleFormat {get; set;}
      public virtual string CategoryTitleFormat {get; set;}
      public virtual string TagTitleFormat {get; set;}
      public virtual string SearchTitleFormat {get; set;}
      public virtual string ArchiveTitleFormat {get; set;}
      public virtual string Page404TitleFormat {get; set;}
      public virtual string DescriptionFormat {get; set;}
      public virtual bool UseCategoriesForMeta {get; set;}
      public virtual bool GenerateKeywordsForPost {get; set;}
      public virtual bool UseNoIndexForCategories {get; set;}
      public virtual bool UseNoIndexForArchives {get; set;}
      public virtual bool UseNoIndexForTags {get; set;}
      public virtual bool GenerateDescriptions {get; set;}
      public virtual bool CapitalizeCategoryTitles {get; set;}
     

      public SeoSettings()
		{
         this.SeoSettingsId = -1;
         RewriteTitles = true;
         GenerateKeywordsForPost = true;
         UseNoIndexForCategories = true;
         UseNoIndexForArchives = true;
         GenerateDescriptions = true;
         CapitalizeCategoryTitles = true;

         PostTitleFormat = "%post_title% | %blog_title%";            
         PageTitleFormat = "%page_title% | %blog_title%";            
         CategoryTitleFormat = "%category_title% | %blog_title%";    
         TagTitleFormat = "%tag% | %blog_title%";                    
         SearchTitleFormat = "%search% | %blog_title%";              
         ArchiveTitleFormat = "%date% | %blog_title%";
         Page404TitleFormat = "%date% | %blog_title%";    
         DescriptionFormat = "%description%";                        
		}                                                              


      #region Equality

      public override bool Equals(object other)
      {
         if (this == other)
            return true;
         SeoSettings c = other as SeoSettings;
         if (c == null)
            return false;
         if (SeoSettingsId != c.SeoSettingsId)
            return false;
         return true;
      }



      public override int GetHashCode()
      {
         unchecked
         {
            int result = SeoSettingsId.GetHashCode();
            result = 29 * result;
            return result;
         }
      }

      #endregion

   }
}
