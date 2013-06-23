using System;
using CookComputing.XmlRpc;

namespace Arashi.Web.Components.MetaWeblogApi.Domain
{
   [XmlRpcMissingMapping(MappingAction.Ignore)]
   public struct Post
   {
      [XmlRpcMissingMapping(MappingAction.Error)]
      [XmlRpcMember(Description = "Required when posting.")]
      public string title;
	
      [XmlRpcMissingMapping(MappingAction.Error)]
      [XmlRpcMember(Description = "Required when posting.")]
      public string[] categories;

      [XmlRpcMissingMapping(MappingAction.Error)]
      [XmlRpcMember(Description = "Required when posting.")]
      public DateTime dateCreated;

      [XmlRpcMissingMapping(MappingAction.Error)]
      [XmlRpcMember(Description = "Required when posting.")]
      public string description;

      public string userid;
      public string permalink;
      public object postid;
      public string wp_slug;
   }
}