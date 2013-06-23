using CookComputing.XmlRpc;

namespace Arashi.Web.Components.MetaWeblogApi.Domain
{
   [XmlRpcMissingMapping(MappingAction.Ignore)]
   public struct Source
   {
      public string name;
      public string url;
   }
}