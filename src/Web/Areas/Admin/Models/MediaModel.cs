using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Arashi.Web.Areas.Admin.Models
{
   public class MediaModel
   {
      public string Name {get; set;}
      
      public string MimeType {get; set;}
      
      public string RelativePath {get; set;}

      public DateTime LastModifiedDate {get; set;}
   }
}
