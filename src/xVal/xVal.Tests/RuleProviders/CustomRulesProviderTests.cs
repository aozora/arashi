using System;
using System.Linq;
using Xunit;
using xVal.RuleProviders;
using xVal.Rules;

namespace xVal.Tests.RuleProviders
{
    public class CustomRulesProviderTests
    {
        [Fact]
        public void IsACachingRuleProvider()
        {
            CachingRulesProvider instance = new CustomRulesProvider();
        }

        [Fact]
        public void ICustomRule_Has_ToCustomRule_Method()
        {
            Func<ICustomRule, CustomRule> test = x => x.ToCustomRule();
        }

        [Fact]
        public void Returns_Custom_Rules_From_Attributes()
        {
            var provider = new CustomRulesProvider();
            var rules = provider.GetRulesFromType(typeof (TestModel));
            Assert.Equal(1, rules.Keys.Count());
            var customRule = rules["Name"].Single() as CustomRule;
            Assert.Equal("myJSFunc", customRule.JavaScriptFunction);
            Assert.Equal("someParam", customRule.Parameters);
            Assert.Equal("My error", customRule.ErrorMessageOrResourceString);
        }

        private class TestModel
        {
            [TestCustomRule]
            public string Name { get; set; }
        }

        private class TestCustomRuleAttribute : Attribute, ICustomRule
        {
            public CustomRule ToCustomRule()
            {
                return new CustomRule("myJSFunc", "someParam", "My error");
            }
        }
    }
}