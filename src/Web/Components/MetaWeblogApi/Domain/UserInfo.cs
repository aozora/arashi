using System;

namespace Arashi.Web.Components.MetaWeblogApi.Domain
{
   [Serializable]
   public struct UserInfo
   {
      public string email;
      public string firstname;
      public string lastname;
      public string nickname;
      public string url;
      public string userid;
   }
}