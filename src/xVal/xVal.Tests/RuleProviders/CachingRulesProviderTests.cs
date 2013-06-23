using System;
using System.Collections.Generic;
using Moq;
using Xunit;
using xVal.RuleProviders;
using xVal.Rules;
using System.Linq;

namespace xVal.Tests.RuleProviders
{
    public class CachingRulesProviderTests
    {
        [Fact]
        public void Returns_Rules_Given_By_Subclass()
        {
            // Arrange
            var expectedStringResult = new RuleSet(new Rule[] {}.ToLookup(x => "test1", x => x));
            var expectedDateTimeResult = new RuleSet(new Rule[] {}.ToLookup(x => "test2", x => x));
            var provider = new TestRulesProvider(t => {
                if(t == typeof(string))
                    return expectedStringResult;
                if (t == typeof(DateTime))
                    return expectedDateTimeResult;
                throw new ArgumentException();
            });

            // Act
            var stringResult = provider.GetRulesFromType(typeof (string));
            var dateTimeResult = provider.GetRulesFromType(typeof(DateTime));

            // Assert
            Assert.Same(expectedStringResult, stringResult);
            Assert.Same(expectedDateTimeResult, dateTimeResult);
        }

        [Fact]
        public void Only_Calls_GetRulesFromTypeCore_Once_Per_Type()
        {
            // Arrange
            int numCalls = 0;
            var expectedStringResult = new RuleSet(new Rule[] { }.ToLookup(x => "test1", x => x));
            var expectedDateTimeResult = new RuleSet(new Rule[] { }.ToLookup(x => "test2", x => x));
            var provider = new TestRulesProvider(t =>
            {
                numCalls++;
                if (t == typeof(string))
                    return expectedStringResult;
                if (t == typeof(DateTime))
                    return expectedDateTimeResult;
                throw new ArgumentException();
            });

            // Act / Assert
            var stringResult = provider.GetRulesFromType(typeof(string));
            Assert.Same(expectedStringResult, stringResult);
            Assert.Equal(1, numCalls);

            var dateTimeResult = provider.GetRulesFromType(typeof(DateTime));
            Assert.Same(expectedDateTimeResult, dateTimeResult);
            Assert.Equal(2, numCalls);

            var stringResult2 = provider.GetRulesFromType(typeof(string));
            Assert.Same(expectedStringResult, stringResult2);
            Assert.Equal(2, numCalls);

            var dateTimeResult2 = provider.GetRulesFromType(typeof(DateTime));
            Assert.Same(expectedDateTimeResult, dateTimeResult2);
            Assert.Equal(2, numCalls);
        }

        private class TestRulesProvider : CachingRulesProvider
        {
            private readonly Func<Type, RuleSet> rulesToReturn;

            public TestRulesProvider(Func<Type, RuleSet> rulesToReturn)
            {
                this.rulesToReturn = rulesToReturn;
            }

            protected override RuleSet GetRulesFromTypeCore(Type type)
            {
                return rulesToReturn(type);
            }
        }
    }
}