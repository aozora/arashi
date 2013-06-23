using System;
using System.Collections.Generic;
using xVal.Rules;
using System.Linq;

namespace xVal.RuleProviders
{
    // Note: RuleEmitters should return null if they don't want to match the input
    public class RuleEmitterList<TInputBase>
    {
        public delegate IEnumerable<Rule> RuleEmitter(TInputBase item);

        private readonly List<RuleEmitter> ruleEmitters = new List<RuleEmitter>();

        public void AddSingle<TSource>(Func<TSource, Rule> emitter) where TSource : TInputBase
        {
            ruleEmitters.Add(x => {
                if (x is TSource) {
                    Rule rule = emitter((TSource)x);
                    return rule == null ? null : new[] {rule};
                } else {
                    return null;
                }
            });
        }

        public void AddMultiple<TSource>(RuleEmitter emitter) where TSource : TInputBase
        {
            ruleEmitters.Add(x => (x is TSource) ? emitter((TSource)x) : null);
        }

        public IEnumerable<Rule> EmitRules(TInputBase item)
        {
            foreach (var converter in ruleEmitters) {
                var converterResult = converter(item);
                if (converterResult != null)
                    return converterResult;
            }
            return new Rule[] {}; // No matching converter, so return empty set of rules
        }
    }
}