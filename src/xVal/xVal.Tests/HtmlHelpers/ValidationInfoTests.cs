using System.Text;
using Xunit;
using xVal.Html;
using xVal.RuleProviders;
using System.Linq;
using xVal.Rules;

namespace xVal.Tests.HtmlHelpers
{
    public class ValidationInfoTests
    {
        [Fact]
        public void Can_Add_Rules_To_ValidationInfo()
        {
            ValidationInfo.Formatter = new TestRulesFormatter();
            
            var validationInfo = new ValidationInfo(RuleSet.Empty);
            Assert.Equal("", validationInfo.ToString());

            validationInfo.AddRule("prop1", new RequiredRule());
            Assert.Equal("prop1=Required,", validationInfo.ToString());

            validationInfo.AddRule("prop2", new StringLengthRule(2, 3));
            Assert.Equal("prop1=Required,prop2=StringLength,", validationInfo.ToString());
        }

        private class TestRulesFormatter : IValidationConfigFormatter
        {
            public string FormatRules(RuleSet rules)
            {
                var allRules = from key in rules.Keys
                               from rule in rules[key]
                               select new { key, rule };
                var result = new StringBuilder();
                foreach (var pair in allRules)
                    result.AppendFormat("{0}={1},", pair.key, pair.rule.RuleName);
                return result.ToString();
            }
        }
    }
}