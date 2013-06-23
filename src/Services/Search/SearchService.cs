using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Arashi.Core.Domain;
using Arashi.Services.Content;
using Arashi.Services.Membership;
using Arashi.Core.Domain.Search;
using Arashi.Core.Exceptions;
using Arashi.Services;
using log4net;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Arashi.Core.Domain.Extensions;
using System.Web;

namespace Arashi.Services.Search
{
   public class SearchService : ISearchService
   {
      private static readonly ILog log = LogManager.GetLogger(typeof(SearchService));
      private readonly IUserService _userDao; //userService;
      private readonly IRequestContextProvider _cuyahogaContextProvider; //requestContextProvider;
      private readonly ITextExtractor _textExtractor;
      private readonly IContentItemService<IContentItem> _contentItemService;

      public SearchService(IUserService userDao, IRequestContextProvider cuyahogaContextProvider, ITextExtractor textExtractor, IContentItemService<IContentItem> contentItemService)
      {
         this._userDao = userDao;
         this._cuyahogaContextProvider = cuyahogaContextProvider;
         this._textExtractor = textExtractor;
         this._contentItemService = contentItemService;
      }

      #region ISearchService Members

      public void UpdateContent(SearchContent searchContent)
      {
         using (IndexBuilder indexBuilder = CreateIndexBuilder())
         {
            indexBuilder.UpdateContent(searchContent);
         }
      }

      public void AddContent(SearchContent searchContent)
      {
         using (IndexBuilder indexBuilder = CreateIndexBuilder())
         {
            indexBuilder.AddContent(searchContent);
         }
      }

      public void DeleteContent(SearchContent searchContent)
      {
         using (IndexBuilder indexBuilder = CreateIndexBuilder())
         {
            indexBuilder.DeleteContent(searchContent);
         }
      }

      public void AddContent(IList<SearchContent> searchContents)
      {
         using (IndexBuilder indexBuilder = CreateIndexBuilder())
         {
            foreach (SearchContent searchContent in searchContents)
            {
               indexBuilder.AddContent(searchContent);
            }
         }
      }

      public void AddContent(IContentItem contentItem)
      {
         using (IndexBuilder indexBuilder = CreateIndexBuilder())
         {
            indexBuilder.AddContent(contentItem);
         }
      }

      public void UpdateContent(IContentItem contentItem)
      {
         using (IndexBuilder indexBuilder = CreateIndexBuilder())
         {
            indexBuilder.UpdateContent(contentItem);
         }
      }

      public void DeleteContent(IContentItem contentItem)
      {
         using (IndexBuilder indexBuilder = CreateIndexBuilder())
         {
            indexBuilder.DeleteContent(contentItem);
         }
      }

      public SearchResultCollection FindContent(string queryText, int pageIndex, int pageSize)
      {
         return this.FindContent(queryText, null, pageIndex, pageSize);
      }

      public SearchResultCollection FindContent(string queryText, IList<string> categoryNames, int pageIndex, int pageSize)
      {
         // Check queryText for invalid fields
         if (!string.IsNullOrEmpty(queryText) && queryText.Contains("viewroleid:"))
         {
            throw new SearchException("Don't try to mess with security!");
         }

         IRequestContext requestContext = this._cuyahogaContextProvider.GetContext();
         User currentUser = requestContext.CurrentUser;

         IList<Role> roles;
         if (currentUser != null)
         {
            roles = currentUser.Roles;
         }
         else
         {
            // Assume anonymous access, get all roles that have only anonymous access.
            // TODO: check this!
            //roles = this._userDao.GetRolesBySite(requestContext.CurrentSite).Where(role => role.IsAnonymousRole()).ToList();
            roles = this._userDao.GetRolesBySite(requestContext.CurrentSite);
         }
         IList<int> roleIds = roles.Select(role => role.RoleId).ToList();
         IndexQuery query = new IndexQuery(GetIndexDirectory());

         try
         {
            return query.Find(queryText, categoryNames, pageIndex, pageSize, roleIds);
         }
         catch (ParseException ex)
         {
            log.Error(string.Format("SearchService.FindContent: Invalid query: {0}", queryText), ex);
            throw new SearchException("Invalid search query", ex);
         }
      }

      public SearchIndexProperties GetIndexProperties()
      {
         SearchIndexProperties indexProperties = new SearchIndexProperties();
         indexProperties.IndexDirectory = GetIndexDirectory();

         IndexReader indexReader = IndexReader.Open(indexProperties.IndexDirectory);
         try
         {
            indexProperties.NumberOfDocuments = indexReader.NumDocs();
         }
         finally
         {
            indexReader.Close();
         }
         indexProperties.LastModified = new DateTime(IndexReader.LastModified(indexProperties.IndexDirectory));
         return indexProperties;
      }

      /// <summary>
      /// Rebuild the full-text index.
      /// </summary>
      /// <param name="contentItemsToIndex"></param>
      public void RebuildIndex(Site site, IEnumerable<IContentItem> contentItemsToIndex)
      {
         //Site currentSite = this._cuyahogaContextProvider.GetContext().CurrentSite;
         string indexDirectory = GetIndexDirectoryBySite(site);
         log.DebugFormat("SearchService.RebuildIndex: Site: {0}, indexDirectory = {1}", site.Name, indexDirectory);

         using (IndexBuilder indexBuilder = new IndexBuilder(indexDirectory, true, this._textExtractor))
         {
            // Add all content items
            //var contentItemsToIndex = this._contentItemService.FindAllBySite(currentSite);
            foreach (IContentItem contentItem in contentItemsToIndex)
            {
               // Index ONLY published items
               if (contentItem.PublishedDate.HasValue && contentItem is ISearchableContent)
               {
                  try
                  {
                     indexBuilder.AddContent(contentItem);
                  }
                  catch (Exception ex)
                  {
                     log.Error(string.Format("SearchService.FindContent: Error while indexing ContentItem with id {0}", contentItem.Id), ex);
                     throw;
                  }
               }
            }

            log.Debug("SearchService.FindContent: Optimizing index...");
            indexBuilder.Optimize();
         }
      }

      #endregion

      private IndexBuilder CreateIndexBuilder()
      {
         return new IndexBuilder(GetIndexDirectory(), false, this._textExtractor);
      }

      private string GetIndexDirectory()
      {
         IRequestContext cuyahogaContext = this._cuyahogaContextProvider.GetContext();
         return Path.Combine(cuyahogaContext.CurrentSiteDataFolder, "index");
      }


      private string GetIndexDirectoryBySite(Site site)
      {
         return Path.Combine(HttpContext.Current.Server.MapPath(site.SiteDataPath), "index");
      }



   }
}
