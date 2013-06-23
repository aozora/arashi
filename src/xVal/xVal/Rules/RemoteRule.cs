using System.Collections.Generic;

namespace xVal.Rules
{
    public class RemoteRule : Rule
    {
        private readonly string _url;

        public RemoteRule(string url) : base("Remote")
        {
            _url = url;
        }

        public override IDictionary<string, string> ListParameters()
        {
            var result = base.ListParameters();
            result.Add("url", _url);
            return result;
        }
    }
}
