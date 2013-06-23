using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using xVal.RuleProviders;
using System.ComponentModel.DataAnnotations;
using xVal.Rules;

namespace xVal.Tests.RuleProviders
{
    public class ActiveRuleProvidersTests
    {
        [Fact]
        public void GetRulesForType_Concatenates_Output_From_All_Providers()
        {
            // Arrange
            var arbitraryType = typeof(int);
            var someOtherType = typeof(string);
            var mockProvider1 = MakeMockRuleProvider(arbitraryType, "prop1a", "prop1b");
            var mockProvider2 = MakeMockRuleProvider(arbitraryType, "prop2");
            var mockProvider3 = MakeMockRuleProvider(arbitraryType, "prop3a", "prop3b", "prop3c");
            var mockProvider4 = MakeMockRuleProvider(someOtherType, "this_should_not_be_output");
            ActiveRuleProviders.Providers.Clear();
            ActiveRuleProviders.Providers.Add(mockProvider1);
            ActiveRuleProviders.Providers.Add(mockProvider2);
            ActiveRuleProviders.Providers.Add(mockProvider3);
            ActiveRuleProviders.Providers.Add(mockProvider4);

            // Act
            var rules = ActiveRuleProviders.GetRulesForType(arbitraryType);

            // Assert
            var totalNumberOfRules = rules.Keys.Sum(x => rules[x].Count());
            Assert.Equal(6, totalNumberOfRules);
            Assert.NotEmpty("prop1a");
            Assert.NotEmpty("prop1b");
            Assert.NotEmpty("prop2");
            Assert.NotEmpty("prop3a");
            Assert.NotEmpty("prop3b");
            Assert.NotEmpty("prop3c");
        }

        [Fact]
        public void GetRulesForType_Can_Handle_Provider_Returning_NULL()
        {
            // Arrange
            var mockProvider = new Moq.Mock<IRulesProvider>();
            mockProvider.Expect(x => x.GetRulesFromType(typeof(double)))
                        .Returns((RuleSet)null);
            ActiveRuleProviders.Providers.Clear();
            ActiveRuleProviders.Providers.Add(mockProvider.Object);

            // Act
            var rules = ActiveRuleProviders.GetRulesForType(typeof (double));

            // Assert
            Assert.NotNull(rules);
            Assert.Empty(rules.Keys);
        }

        private static IRulesProvider MakeMockRuleProvider(Type forModelType, params string[] rulePropertyNames)
        {
            var ruleset = new RuleSet(rulePropertyNames.ToLookup(x => x, x => (Rule)new RequiredRule()));
            var mockProvider = new Moq.Mock<IRulesProvider>();
            mockProvider.Expect(x => x.GetRulesFromType(forModelType)).Returns(ruleset);
            return mockProvider.Object;
        }
    }
}