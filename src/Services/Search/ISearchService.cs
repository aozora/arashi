using System;
using System.Collections.Generic;
using Arashi.Core.Domain;
using Arashi.Core.Domain.Search;


namespace Arashi.Services.Search
{
   public interface ISearchService
   {
      void UpdateContent(SearchContent searchContent);
      void AddContent(SearchContent searchContent);
      void DeleteContent(SearchContent searchContent);
      void AddContent(IList<SearchContent> searchContents);

      void UpdateContent(IContentItem contentItem);
      void AddContent(IContentItem contentItem);
      void DeleteContent(IContentItem contentItem);

      SearchResultCollection FindContent(string queryText, int pageIndex, int pageSize);
      SearchResultCollection FindContent(string queryText, IList<string> categoryNames, int pageIndex, int pageSize);

      /// <summary>
      /// Get information about the full-text index of the current site.
      /// </summary>
      /// <returns></returns>
      SearchIndexProperties GetIndexProperties();

      /// <summary>
      /// Rebuild the full-text index.
      /// </summary>
      /// <param name="searchableModules">A list of (legacy) searchable modules in the installation.</param>
      void RebuildIndex(Site site, IEnumerable<IContentItem> contentItemsToIndex);
   }
}