using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Arashi.Services.Versioning;
using Arashi.Core.Domain.Search;
using Arashi.Core.Extensions;

namespace Arashi.Core.Domain
{
   public class Post : ContentItem, ISearchableContent //, IVersionableContent
   {

      /// <summary>
      /// Property Content (string)
      /// </summary>
      public virtual string Content
      {
         get;
         set;
      }



      /// <summary>
      /// First 55 words of the item's content (stripped of html tags)
      /// See http://codex.wordpress.org/Glossary#Excerpt
      /// </summary>
      public virtual string Teaser
      {
         get
         {
            return Content.StripHtml().GetFirstWords(55);
         }
      }



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


      //#region Implementation of IVersionableContent

      ///// <summary>
      ///// Returns paths to complex child properties that
      ///// should be versioned as well, e.g.
      ///// Foo
      ///// Foo.Bar
      ///// Foo.Bar.Baz
      ///// Where Foo is a property of the object implementing IVersionableContent
      ///// </summary>
      //public IList<string> CustomVersioningInfo
      //{
      //   get
      //   {
      //      throw new NotImplementedException();
      //   }
      //   set
      //   {
      //      throw new NotImplementedException();
      //   }
      //}

      //#endregion

   }
}
