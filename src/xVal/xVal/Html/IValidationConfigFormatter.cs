using System.Linq;
using xVal.RuleProviders;
using xVal.Rules;

namespace xVal.Html
{
    public interface IValidationConfigFormatter
    {
        string FormatRules(RuleSet rules);
    }
}