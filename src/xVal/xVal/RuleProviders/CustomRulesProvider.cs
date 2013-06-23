using System;
using System.Collections.Generic;
using xVal.Rules;

namespace xVal.RuleProviders
{
    public class CustomRulesProvider : PropertyAttributeRuleProviderBase<Attribute>
    {
        protected override IEnumerable<Rule> MakeValidationRulesFromAttribute(Attribute att)
        {
            var customRuleAttribute = att as ICustomRule;
            if(customRuleAttribute != null)
                yield return customRuleAttribute.ToCustomRule();
        }
    }
}