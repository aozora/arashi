using System;

namespace Arashi.Web.Components.MetaWeblogApi.Domain
{
   [Serializable]
   public struct CategoryInfo
   {
      public string categoryid;
      public string description;
      public string htmlurl;
      public string rssUrl;
      public string title;
   }
}