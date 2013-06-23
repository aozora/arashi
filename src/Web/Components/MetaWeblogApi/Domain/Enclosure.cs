using CookComputing.XmlRpc;


namespace Arashi.Web.Components.MetaWeblogApi.Domain
{
   [XmlRpcMissingMapping(MappingAction.Ignore)]
   public struct Enclosure
   {
      public int length;
      public string type;
      public string url;
   }
}