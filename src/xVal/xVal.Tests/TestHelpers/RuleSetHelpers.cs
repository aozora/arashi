using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using xVal.RuleProviders;
using xVal.Rules;

namespace xVal.Tests.TestHelpers
{
    public static class RuleSetHelpers
    {
        /// <summary>
        /// Utility function to give a quick syntax for instantiating a complete RuleSet
        /// </summary>
        public static RuleSet MakeTestRuleSet(IDictionary<string, IDictionary<string, object>> data)
        {
            var rules = from propName in data.Keys
                        from ruleName in data[propName].Keys
                        select new { propName, rule = (Rule)new TestRule(ruleName, data[propName][ruleName]) };
            return new RuleSet(rules.ToLookup(x => x.propName, x => x.rule));
        }

        private class TestRule : Rule
        {
            public IDictionary<string, string> Parameters { get; set; }

            public TestRule(string ruleName, object parameters)
                : base(ruleName)
            {
                Parameters = new RouteValueDictionary(parameters).ToDictionary(x => x.Key, x => x.Value.ToString());
            }

            public override IDictionary<string, string> ListParameters()
            {
                return Parameters;
            }
        }
    }
}