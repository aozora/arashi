using System;
using System.Collections.Generic;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.QueryParsers;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Common.Logging;

namespace Arashi.Services.Search
{
   /// <summary>
   /// The IndexQuery class provides functionality to search the Full-Text index.
   /// </summary>
   public class IndexQuery
   {
      private ILog log = LogManager.GetCurrentClassLogger();
      private readonly Directory _indexDirectory;

      /// <summary>
      /// Default constructor.
      /// <param name="physicalIndexDir">The physical directory where the search index resides.</param>
      /// </summary>
      public IndexQuery(string physicalIndexDir)
      {
         this._indexDirectory = FSDirectory.GetDirectory(physicalIndexDir, false);
      }

      /// <summary>
      /// Searches the index.
      /// </summary>
      /// <param name="queryText"></param>
      /// <param name="categoryNames"></param>
      /// <param name="pageIndex"></param>
      /// <param name="pageSize"></param>
      /// <param name="roleIds"></param>
      /// <returns></returns>
      public SearchResultCollection Find(string queryText, IList<string> categoryNames, int pageIndex, int pageSize, IEnumerable<int> roleIds)
      {
         long startTicks = DateTime.Now.Ticks;
         log.Debug("IndexQuery.Find: start");
         log.DebugFormat("IndexQuery.Find: queryText = \"{0}\", pageIndex = {1}, pageSize = {2}", queryText, pageIndex, pageSize);

         // the overall-query
         BooleanQuery query = new BooleanQuery();
         // add our parsed query
         if (!String.IsNullOrEmpty(queryText))
         {
            Query multiQuery = MultiFieldQueryParser.Parse(new[] { queryText, queryText, queryText, queryText, queryText }, new[] { "title", "summary", "contents", "tag", "category" }, new StandardAnalyzer());
            query.Add(multiQuery, BooleanClause.Occur.MUST);
         }

         // TODO: for now this is commented
         // add the security constraint - must be satisfied
         //query.Add(this.BuildSecurityQuery(roleIds), BooleanClause.Occur.MUST);

         // Add the category query (if available)
         if (categoryNames != null)
         {
            query.Add(this.BuildCategoryQuery(categoryNames), BooleanClause.Occur.MUST);
         }

         IndexSearcher searcher = new IndexSearcher(this._indexDirectory);
         Hits hits = searcher.Search(query, new Sort("datepublished", true));

         log.DebugFormat("IndexQuery.Find: hits.Length() = {0}", hits.Length());

         int start = (pageIndex - 1) * pageSize;
         int end = (pageIndex) * pageSize;
         if (hits.Length() <= end)
         {
            end = hits.Length();
         }

         log.DebugFormat("IndexQuery.Find: start = {0}, end = {1}", start, end);

         SearchResultCollection results = new SearchResultCollection(end);
         results.TotalCount = hits.Length();
         results.PageIndex = pageIndex;

         for (int i = start; i < end; i++)
         {
            string[] categories = hits.Doc(i).GetValues("category");
            string[] tags = hits.Doc(i).GetValues("tag");

            SearchResult result = new SearchResult()
            {
               Title = hits.Doc(i).Get("title"),
               Summary = hits.Doc(i).Get("summary"),
               Author = hits.Doc(i).Get("author"),
               Path = hits.Doc(i).Get("path"),
               Category = categories != null ? String.Join(", ", categories) : String.Empty,
               Tag = tags != null ? String.Join(", ", tags) : String.Empty,
               DatePublished = DateTime.Parse((hits.Doc(i).Get("datepublished"))),
               Score = hits.Score(i),
               Boost = hits.Doc(i).GetBoost(),
               ContentItemId = Int32.Parse(hits.Doc(i).Get("contentitemid"))
            };

            results.Add(result);
         }

         searcher.Close();
         results.ExecutionTime = DateTime.Now.Ticks - startTicks;
         log.Debug("IndexQuery.Find: end");

         return results;
      }

      private Query BuildCategoryQuery(IEnumerable<string> categoryNames)
      {
         BooleanQuery categoryQuery = new BooleanQuery();
         foreach (string name in categoryNames)
         {
            categoryQuery.Add(new TermQuery(new Term("category", name)), BooleanClause.Occur.SHOULD);
         }
         return categoryQuery;
      }

      private Query BuildSecurityQuery(IEnumerable<int> roleIds)
      {
         BooleanQuery bQueryContent = new BooleanQuery();
         foreach (int roleId in roleIds)
         {
            bQueryContent.Add(new TermQuery(new Term("viewroleid", roleId.ToString())), BooleanClause.Occur.SHOULD);
         }
         return bQueryContent;
      }
   }
}
