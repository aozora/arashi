using System;
using System.Linq;
using System.Text.RegularExpressions;
using NHibernate.Validator;
using NHibernate.Validator.Engine;
using Xunit;
using xVal.RuleProviders;
using xVal.Rules;
using xVal.RulesProviders.NHibernateValidator;
using xVal.Tests.TestHelpers;

namespace xVal.Tests.RuleProviders
{
    public class NHibernateValidatorRulesProviderTests
    {
        [Fact]
        public void ImplementsIRuleProvider()
        {
            new NHibernateValidatorRulesProvider(ValidatorMode.UseXml);
        }

        [Fact]
        public void Detects_Attributes_On_Public_Properties()
        {
            // Arrange
            var provider = new NHibernateValidatorRulesProvider(ValidatorMode.UseAttribute);

            // Act
            var rules = provider.GetRulesFromType(typeof (TestModel));

            // Assert
            Assert.Equal(1, rules.Keys.Count());
            var lengthRule = rules["Name"].First() as StringLengthRule;
            Assert.Equal(3, lengthRule.MinLength);
            Assert.Equal(6, lengthRule.MaxLength);
            Assert.Equal("MyMessage", lengthRule.ErrorMessage);
        }

        [Fact]
        public void Converts_LengthAttribute_Max_To_StringLengthRule()
        {
            var rule = TestConversion<LengthAttribute, StringLengthRule>(17);
            Assert.Equal(0, rule.MinLength);
            Assert.Equal(17, rule.MaxLength);
        }

        [Fact]
        public void Converts_LengthAttribute_MinMax_To_StringLengthRule()
        {
            var rule = TestConversion<LengthAttribute, StringLengthRule>(4, 6);
            Assert.Equal(4, rule.MinLength);
            Assert.Equal(6, rule.MaxLength);
        }


        [Fact]
        public void Converts_MinAttribute_To_RangeRule()
        {
            var rule = TestConversion<MinAttribute, RangeRule>((long)31);
            Assert.Equal(31, (decimal)rule.Min);
            Assert.Null(rule.Max);
        }

        [Fact]
        public void Converts_MaxAttribute_To_RangeRule()
        {
            var rule = TestConversion<MaxAttribute, RangeRule>((long)-150);
            Assert.Null(rule.Min);
            Assert.Equal(-150, (decimal)rule.Max);
        }

        [Fact]
        public void Converts_RangeAttribute_To_RangeRule()
        {
            var rule = TestConversion<RangeAttribute, RangeRule>((long)3, (long)4);
            Assert.Equal(3, (decimal)rule.Min);
            Assert.Equal(4, (decimal)rule.Max);
        }

        [Fact]
        public void Converts_NotEmptyAttribute_To_RequiredRule()
        {
            var rule = TestConversion<NotEmptyAttribute, RequiredRule>();
        }

        [Fact]
        public void Converts_NotNullNotEmptyAttribute_To_RequiredRule()
        {
            var rule = TestConversion<NotNullNotEmptyAttribute, RequiredRule>();
        }

        [Fact]
        public void Converts_PatternAttribute_To_RegExRule()
        {
            var rule = TestConversion<PatternAttribute, RegularExpressionRule>("myregex\\d+", RegexOptions.IgnoreCase);
            Assert.Equal("myregex\\d+", rule.Pattern);
            Assert.Equal(RegexOptions.IgnoreCase, rule.Options);
        }

        [Fact]
        public void Converts_EmailAttribute_To_DataTypeRule_EmailAddress()
        {
            var rule = TestConversion<EmailAttribute, DataTypeRule>();
            Assert.Equal(DataTypeRule.DataType.EmailAddress, rule.Type);
        }

        [Fact]
        public void Converts_DigitsAttribute_IntegralOnly_To_RegEx()
        {
            var rule = TestConversion<DigitsAttribute, RegularExpressionRule>(4);
            Assert.Equal(@"\d{0,4}", rule.Pattern);
        }

        [Fact]
        public void Converts_DigitsAttribute_WithFractional_To_RegEx()
        {
            var rule = TestConversion<DigitsAttribute, RegularExpressionRule>(4, 6);
            Assert.Equal(@"\d{0,4}(\.\d{1,6})?", rule.Pattern);
        }

        private class TestModel
        {
            [Length(3, 6, Message = "MyMessage")]
            public string Name { get; set; }
        }

        private static TRule TestConversion<TAttribute, TRule>(params object[] attributeConstructorParams)
            where TAttribute : Attribute
            where TRule : Rule
        {
            return RulesProviderTestHelpers.TestConversion<TAttribute, TRule>(new NHibernateValidatorRulesProvider(ValidatorMode.UseAttribute), attributeConstructorParams);
        }

    }
}