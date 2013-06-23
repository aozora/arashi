using System;
using System.Linq;
using System.Collections.Generic;
using Arashi.Core.Domain.Extensions;
using Arashi.Core.Domain.Search;
using Arashi.Core.Extensions;
using Arashi.Core.Util;

namespace Arashi.Core.Domain
{
   public class Page : ContentItem, ISearchableContent //, IVersionableContent
   {
      #region Private Fields

      /// <summary>
      /// Property Content (string)
      /// </summary>
      public virtual string Content
      {
         get;
         set;
      }

      public virtual Page ParentPage { get; set; }
	   public virtual int Position { get; set; }
      public virtual  IList<Page> ChildPages { get; set; }

      /// <summary>
      /// Returns the number of sublevel of the page
      /// </summary>
      public virtual int Depth
      {
         get
         {
            int depth = 0;

            Page p = this;

            while (p.ParentPage != null)
            {
               p = p.ParentPage;
               depth++;
            }

            return depth;
         }
      }

      public virtual string DepthTitle
      {
         get
         {
            return string.Concat(new string('-', Depth), Title);
         }
      }



      /// <summary>
      /// The name of a custom template file for a page (it doesn't include the file extension)
      /// </summary>
      public virtual string CustomTemplateFile { get; set; }

      public override string GetContentUrl()
      {
         // If the contentitem is not published throw an exception!
         if (!PublishedDate.HasValue)
            throw new ApplicationException("Page.GetContentUrl: cannot format url for items without PublishedDate!");

         const string defaultUrlFormat = "page/{0}/";

         if (this.Site == null)
         {
            throw new InvalidOperationException("Unable to get the url for the content because the associated Site is missing.");
         }

         return String.Format(defaultUrlFormat,
                              this.FriendlyName);
      }


	   #endregion

      #region Implementation of ISearchableContent

      /// <summary>
      /// Get the full contents of this ContentItem for indexing
      /// </summary>
      /// <returns></returns>
      public virtual string ToSearchContent(ITextExtractor textExtractor)
      {
         return this.Content;
      }

      /// <summary>
      /// Get a list of <see cref="CustomSearchField"/>s, if any
      /// </summary>
      /// <returns></returns>
      public virtual IList<CustomSearchField> GetCustomSearchFields()
      {
         return new List<CustomSearchField>();
      }

      #endregion

   }
}
