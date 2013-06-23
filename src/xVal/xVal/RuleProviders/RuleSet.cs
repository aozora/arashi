using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using xVal.Rules;

namespace xVal.RuleProviders
{
    public class RuleSet
    {
        public static readonly RuleSet Empty = new RuleSet(new object[] { }.ToLookup(x => (string) null, x => (Rule) null));

        private readonly ILookup<string, Rule> rules;

        public RuleSet(ILookup<string, Rule> rules)
        {
            if (rules == null) throw new ArgumentNullException("rules");
            this.rules = rules;
        }

        public RuleSet(IEnumerable<RuleSet> rulesetsToMerge)
        {
            if (rulesetsToMerge == null) throw new ArgumentNullException("rulesetsToMerge");
            var allRules = (from set in rulesetsToMerge
                            from key in set.Keys
                            from rule in set[key]
                            select new { key, rule });
            rules = allRules.ToLookup(x => x.key, x => x.rule);
        }

        public bool Contains(string key)
        {
            return rules.Contains(key);
        }

        public IEnumerable<Rule> this[string key]
        {
            get { return rules[key]; }
        }

        public IEnumerable<string> Keys
        {
            get { return rules.Select(x => x.Key); }
        }
    }
}