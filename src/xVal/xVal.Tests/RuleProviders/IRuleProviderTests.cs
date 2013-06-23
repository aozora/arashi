using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Xunit;
using xVal.RuleProviders;
using xVal.Rules;

namespace xVal.Tests.RuleProviders
{
    public class IRuleProviderTests
    {
        [Fact]
        public void IRuleProvider_Has_GetRulesFromType_Method()
        {
            // Arrange
            var arbitraryType = this.GetType();
            var mockRuleProvider = new Moq.Mock<IRulesProvider>();
            mockRuleProvider.Expect(x => x.GetRulesFromType(arbitraryType))
                            .Returns(RuleSet.Empty);

            // Act
            var actualResult = mockRuleProvider.Object.GetRulesFromType(arbitraryType);

            // Assert
            Assert.Same(RuleSet.Empty, actualResult);
        }
    }
}