namespace Arashi.Web.Areas.Admin.Models
{
   using System;
   using System.Collections.Generic;


   public class MediaManagerModel
   {
      public int TotalRecordCount { get; set; }
      public int CurrentPageIndex { get; set; }
      public int PageSize { get; set; }
      public string CurrentSearchPattern { get; set; }

      public IEnumerable<MediaModel> Medias { get; set; }
   }
}