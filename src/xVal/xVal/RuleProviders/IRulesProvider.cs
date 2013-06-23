using System;

namespace xVal.RuleProviders
{
    public interface IRulesProvider
    {
        RuleSet GetRulesFromType(Type type);
    }
}