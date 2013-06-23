namespace Arashi.Services.Themes
{
   using System.Collections.Generic;
   using Arashi.Core.Domain;
   using uNhAddIns.Pagination;

   public interface IThemeService
   {

      /// <summary>
      /// Get a paged list of ALL templates
      /// </summary>
      /// <param name="pageSize"></param>
      /// <returns></returns>
      Paginator<Theme> GetPaginatorForAll(int pageSize);

      IList<Theme> GetAll();

      /// <summary>
      /// Get a paged list of templates for a given site
      /// </summary>
      /// <param name="site"></param>
      /// <param name="pageSize"></param>
      /// <returns></returns>
      Paginator<Theme> GetPaginatorBySite(Site site, int pageSize);

      Theme GetById(int themeId);

      void Save(Theme theme);

      void Delete(Theme theme);

   }
}
