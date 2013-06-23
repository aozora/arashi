using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Components.Validator;
using Xunit;
using xVal.RuleProviders;
using xVal.Rules;
using xVal.RulesProviders.CastleValidator;
using xVal.Tests.TestHelpers;

namespace xVal.Tests.RuleProviders
{
    public class CastleValidatorRulesProviderTests
    {
        [Fact]
        public void ImplementsIRuleProvider()
        {
            IRulesProvider instance = new CastleValidatorRulesProvider();
        }

        [Fact]
        public void Converts_ValidateNonEmptyAttribute_To_RequiredRule()
        {
            TestConversion<ValidateNonEmptyAttribute, RequiredRule>();
        }

        [Fact]
        public void Obtains_Custom_Error_Message_From_Attribute()
        {
            var rule = TestConversion<ValidateNonEmptyAttribute, RequiredRule>("Custom Message");
            Assert.Equal("Custom Message", rule.ErrorMessage);
        }

        [Fact]
        public void Converts_ValidateEmailAttribute_To_DataTypeRule_Email()
        {
            var rule = TestConversion<ValidateEmailAttribute, DataTypeRule>();
            Assert.Equal(DataTypeRule.DataType.EmailAddress, rule.Type);
        }

        [Fact]
        public void Converts_ValidateCreditCardAttribute_To_DataType_CreditCard()
        {
            var rule = TestConversion<ValidateCreditCardAttribute, DataTypeRule>();
            Assert.Equal(DataTypeRule.DataType.CreditCardLuhn, rule.Type);
        }

        [Fact]
        public void Converts_ValidateDateTimeAttribute_To_DataType_DateTime_Plus_Required()
        {
            var rules = TestConversionToMultipleRules<ValidateDateTimeAttribute, Rule>();
            Assert.Equal(2, rules.Count());
            var dataTypeRule = rules.OfType<DataTypeRule>().Single();
            Assert.Equal(DataTypeRule.DataType.DateTime, dataTypeRule.Type);
            Assert.NotNull(rules.OfType<RequiredRule>().SingleOrDefault());
        }

        [Fact]
        public void Converts_ValidateDateAttribute_To_DataType_Date_Plus_Required()
        {
            var rules = TestConversionToMultipleRules<ValidateDateAttribute, Rule>();
            Assert.Equal(2, rules.Count());
            var dataTypeRule = rules.OfType<DataTypeRule>().Single();
            Assert.Equal(DataTypeRule.DataType.Date, dataTypeRule.Type);
            Assert.NotNull(rules.OfType<RequiredRule>().SingleOrDefault());
        }

        [Fact]
        public void Converts_ValidateDecimalAttribute_To_DataType_Decimal_Plus_Required()
        {
            var rules = TestConversionToMultipleRules<ValidateDecimalAttribute, Rule>();
            Assert.Equal(2, rules.Count());
            var dataTypeRule = rules.OfType<DataTypeRule>().Single();
            Assert.Equal(DataTypeRule.DataType.Decimal, dataTypeRule.Type);
            Assert.NotNull(rules.OfType<RequiredRule>().SingleOrDefault());
        }

        [Fact]
        public void Converts_ValidateDoubleAttribute_To_DataType_Decimal_Plus_Required() // No point distinguishing between decimal and double on client
        {
            var rules = TestConversionToMultipleRules<ValidateDoubleAttribute, Rule>();
            Assert.Equal(2, rules.Count());
            var dataTypeRule = rules.OfType<DataTypeRule>().Single();
            Assert.Equal(DataTypeRule.DataType.Decimal, dataTypeRule.Type);
            Assert.NotNull(rules.OfType<RequiredRule>().SingleOrDefault());
        }

        [Fact]
        public void Converts_ValidateSingleAttribute_To_DataType_Decimal_Plus_Required() // No point distinguishing between decimal and single on client
        {
            var rules = TestConversionToMultipleRules<ValidateSingleAttribute, Rule>();
            Assert.Equal(2, rules.Count());
            var dataTypeRule = rules.OfType<DataTypeRule>().Single();
            Assert.Equal(DataTypeRule.DataType.Decimal, dataTypeRule.Type);
            Assert.NotNull(rules.OfType<RequiredRule>().SingleOrDefault());
        }

        [Fact]
        public void Converts_ValidateIntegerAttribute_To_DataType_Integer_Plus_Required()
        {
            var rules = TestConversionToMultipleRules<ValidateIntegerAttribute, Rule>();
            Assert.Equal(2, rules.Count());
            var dataTypeRule = rules.OfType<DataTypeRule>().Single();
            Assert.Equal(DataTypeRule.DataType.Integer, dataTypeRule.Type);
            Assert.NotNull(rules.OfType<RequiredRule>().SingleOrDefault());
        }

        [Fact]
        public void Converts_ValidateLengthAttribute_To_StringLength_Exact()
        {
            var rule = TestConversion<ValidateLengthAttribute, StringLengthRule>(73);
            Assert.Equal(73, rule.MinLength);
            Assert.Equal(73, rule.MaxLength);
        }

        [Fact]
        public void Converts_ValidateLengthAttribute_To_StringLength_Range()
        {
            var rule = TestConversion<ValidateLengthAttribute, StringLengthRule>(3, 16);
            Assert.Equal(3, rule.MinLength);
            Assert.Equal(16, rule.MaxLength);
        }

        [Fact]
        public void Converts_ValidateRangeAttribute_To_Range_Integer()
        {
            var rule = TestConversion<ValidateRangeAttribute, RangeRule>(15, 19);
            Assert.Equal(15, Convert.ToInt32(rule.Min));
            Assert.Equal(19, Convert.ToInt32(rule.Max));
        }

        [Fact]
        public void Converts_ValidateRangeAttribute_To_Range_Decimal()
        {
            var rule = TestConversion<ValidateRangeAttribute, RangeRule>(RangeValidationType.Decimal, 1.3, 2.48);
            Assert.Equal(1.3m, Convert.ToDecimal(rule.Min));
            Assert.Equal(2.48m, Convert.ToDecimal(rule.Max));
        }

        [Fact]
        public void Converts_ValidateRangeAttribute_To_Range_String()
        {
            var rule = TestConversion<ValidateRangeAttribute, RangeRule>("bob", "tarzan");
            Assert.Equal("bob", Convert.ToString(rule.Min));
            Assert.Equal("tarzan", Convert.ToString(rule.Max));
        }

        // Omitted: Converts_ValidateRangeAttribute_To_Range_DateTime
        // It isn't possible in .NET to pass a non-constant expression (such as a DateTime) to a custom
        // attribute, so I'm not sure how ValidateRangeAttribute(DateTime, DateTime) is supposed to be used.
        // Nonetheless, ConvertToXValRules should cope if you did manage to send it one.


        [Fact]
        public void Converts_ValidateRegularExpressionAttribute_To_RegularExpression()
        {
            var rule = TestConversion<ValidateRegExpAttribute, RegularExpressionRule>("mypattern");
            Assert.Equal("mypattern", rule.Pattern);
        }

        [Fact]
        public void Converts_ValidateSameAsAttribute_To_Comparison_Equals()
        {
            var rule = TestConversion<ValidateSameAsAttribute, ComparisonRule>("myProp");
            Assert.Equal("myProp", rule.PropertyToCompare);
            Assert.Equal(ComparisonRule.Operator.Equals, rule.ComparisonOperator);
        }

        [Fact]
        public void Converts_ValidateNotSameAsAttribute_To_Comparison_DoesNotEqual()
        {
            var rule = TestConversion<ValidateNotSameAsAttribute, ComparisonRule>("myProp");
            Assert.Equal("myProp", rule.PropertyToCompare);
            Assert.Equal(ComparisonRule.Operator.DoesNotEqual, rule.ComparisonOperator);
        }

        private static TRule TestConversion<TAttribute, TRule>(params object[] attributeConstructorParams)
            where TAttribute : AbstractValidationAttribute
            where TRule : Rule
        {
            return RulesProviderTestHelpers.TestConversion<TAttribute, TRule>(new CastleValidatorRulesProvider(), attributeConstructorParams);
        }

        private static IEnumerable<TRule> TestConversionToMultipleRules<TAttribute, TRule>(params object[] attributeConstructorParams)
            where TAttribute : AbstractValidationAttribute
            where TRule : Rule
        {
            return RulesProviderTestHelpers.TestConversionToMultipleRules<TAttribute, TRule>(new CastleValidatorRulesProvider(), attributeConstructorParams);
        }
    }
}