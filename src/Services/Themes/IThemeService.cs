using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arashi.Core.Domain;
using uNhAddIns.Pagination;

namespace Arashi.Services.Themes
{
   public interface IThemeService
   {
      IList<Template> FindBySite(Site site);

      /// <summary>
      /// Get a paged list of ALL templates
      /// </summary>
      /// <param name="pageSize"></param>
      /// <returns></returns>
      Paginator<Template> GetPaginatorForAll(int pageSize);

      /// <summary>
      /// Get a paged list of templates for a given site
      /// </summary>
      /// <param name="site"></param>
      /// <param name="pageSize"></param>
      /// <returns></returns>
      Paginator<Template> GetPaginatorBySite(Site site, int pageSize);

      Template GetById(int templateId);

      void Save(Template template);

      void Delete(Template template);

   }
}
