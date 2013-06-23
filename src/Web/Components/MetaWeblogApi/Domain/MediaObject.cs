using CookComputing.XmlRpc;

namespace Arashi.Web.Components.MetaWeblogApi.Domain
{
   [XmlRpcMissingMapping(MappingAction.Ignore)]
   public struct MediaObject
   {
      public byte[] bits;
      public string name;
      public string type;
   }
}