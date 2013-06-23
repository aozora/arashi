using System;
using System.Collections.Generic;
using System.Linq;
using xVal.RuleProviders;
using xVal.Rules;

namespace xVal
{
    public static class ActiveRuleProviders
    {
        public static IList<IRulesProvider> Providers = new List<IRulesProvider> {
            new DataAnnotationsRuleProvider(),
            new CustomRulesProvider()
        };

        public static RuleSet GetRulesForType(Type type)
        {
            var allRuleSets = from p in Providers
                              select p.GetRulesFromType(type) ?? RuleSet.Empty;
            return new RuleSet(allRuleSets);
        }
    }
}