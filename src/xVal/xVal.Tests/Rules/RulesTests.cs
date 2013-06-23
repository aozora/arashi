using System;
using System.Text.RegularExpressions;
using Xunit;
using xVal.Rules;

namespace xVal.Tests.Rules
{
    public class RulesTests
    {
        [Fact]
        public void RequiredRule_Needs_No_Parameters()
        {
            var rule = new RequiredRule();
            Assert.Empty(rule.ListParameters());
        }

        [Fact]
        public void StringLengthRule_Takes_Min_And_Max_Lengths()
        {
            var rule = new StringLengthRule(1, 5);
            var parameters = rule.ListParameters();
            Assert.Equal(2, parameters.Count);
            Assert.Equal("1", parameters["MinLength"]);
            Assert.Equal("5", parameters["MaxLength"]);
        }

        [Fact]
        public void StringLengthRule_Requires_At_Least_One_Of_Min_Or_Max_Length()
        {
            Assert.Throws<ArgumentException>(delegate
            {
                var rule = new StringLengthRule(null, null);
            });
        }

        [Fact]
        public void RangeRule_Takes_Min_And_Max_Ints()
        {
            var rule = new RangeRule(1, 5);
            var parameters = rule.ListParameters();
            Assert.Equal(3, parameters.Count);
            Assert.Equal("1", parameters["Min"]);
            Assert.Equal("5", parameters["Max"]);
            Assert.Equal("integer", parameters["Type"]);
        }

        [Fact]
        public void RangeRule_Takes_Min_And_Max_Decimals()
        {
            var rule = new RangeRule(1.2m, 5.4m);
            var parameters = rule.ListParameters();
            Assert.Equal(3, parameters.Count);
            Assert.Equal("1.2", parameters["Min"]);
            Assert.Equal("5.4", parameters["Max"]);
            Assert.Equal("decimal", parameters["Type"]);
        }

        [Fact]
        public void RangeRule_Takes_Min_And_Max_Strings()
        {
            var rule = new RangeRule("abc", "xyz");
            var parameters = rule.ListParameters();
            Assert.Equal(3, parameters.Count);
            Assert.Equal("abc", parameters["Min"]);
            Assert.Equal("xyz", parameters["Max"]);
            Assert.Equal("string", parameters["Type"]);
        }

        [Fact]
        public void RangeRule_Takes_Min_And_Max_DateTimes()
        {
            var rule = new RangeRule(new DateTime(2001,10, 20, 01, 02, 03), new DateTime(2003, 3, 5));
            var parameters = rule.ListParameters();
            Assert.Equal(13, parameters.Count);
            Assert.Equal("2001", parameters["MinYear"]);
            Assert.Equal("10", parameters["MinMonth"]);
            Assert.Equal("20", parameters["MinDay"]);
            Assert.Equal("1", parameters["MinHour"]);
            Assert.Equal("2", parameters["MinMinute"]);
            Assert.Equal("3", parameters["MinSecond"]);
            Assert.Equal("2003", parameters["MaxYear"]);
            Assert.Equal("3", parameters["MaxMonth"]);
            Assert.Equal("5", parameters["MaxDay"]);
            Assert.Equal("0", parameters["MaxHour"]);
            Assert.Equal("0", parameters["MaxMinute"]);
            Assert.Equal("0", parameters["MaxSecond"]);
            Assert.Equal("datetime", parameters["Type"]);
        }

        [Fact]
        public void RangeRule_Requires_At_Least_One_Of_Min_Or_Max()
        {
            Assert.Throws<ArgumentException>(delegate {
                var rule = new RangeRule((string)null, (string)null);
            });
        }

        [Fact]
        public void DataTypeRule_Accepts_Formats()
        {
            TestDataTypeRuleFormat(DataTypeRule.DataType.Integer, "Integer");
            TestDataTypeRuleFormat(DataTypeRule.DataType.Decimal, "Decimal");
            TestDataTypeRuleFormat(DataTypeRule.DataType.Date, "Date");
            TestDataTypeRuleFormat(DataTypeRule.DataType.DateTime, "DateTime");
            TestDataTypeRuleFormat(DataTypeRule.DataType.Currency, "Currency");
            TestDataTypeRuleFormat(DataTypeRule.DataType.EmailAddress, "EmailAddress");
            TestDataTypeRuleFormat(DataTypeRule.DataType.CreditCardLuhn, "CreditCardLuhn");
        }

        private void TestDataTypeRuleFormat(DataTypeRule.DataType constructorParam, string expectedTypeDescription)
        {
            var rule = new DataTypeRule(constructorParam);
            var parameters = rule.ListParameters();
            Assert.Equal(1, parameters.Count);
            Assert.Equal(expectedTypeDescription, parameters["Type"]);
        }

        [Fact]
        public void RegularExpressionRule_Takes_Pattern()
        {
            var rule = new RegularExpressionRule("myPattern");
            var parameters = rule.ListParameters();
            Assert.Equal(1, parameters.Count);
            Assert.Equal("myPattern", parameters["Pattern"]);
        }

        [Fact]
        public void RegularExpressionRule_Takes_Pattern_Plus_Options()
        {
            var rule = new RegularExpressionRule("myPattern", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            var parameters = rule.ListParameters();
            Assert.Equal(2, parameters.Count);
            Assert.Equal("myPattern", parameters["Pattern"]);
            Assert.Equal("im", parameters["Options"]);
        }

        [Fact]
        public void ComparisonRule_Takes_PropertyToCompare_And_Type()
        {
            var rule = new ComparisonRule("password1", ComparisonRule.Operator.Equals);
            var parameters = rule.ListParameters();
            Assert.Equal(2, parameters.Count);
            Assert.Equal("password1", parameters["PropertyToCompare"]);
            Assert.Equal("Equals", parameters["ComparisonOperator"]);
        }

        [Fact]
        public void CustomRule_Takes_JavaScriptFunction_Params_And_ErrorMessageString()
        {
            var jsParam = new { key = "value", another = "something else" };
            var rule = new CustomRule("myJsFunction", jsParam, "My error");
            var parameters = rule.ListParameters();
            Assert.Equal(2, parameters.Count);
            Assert.Equal("myJsFunction", parameters["Function"]);
            Assert.Equal(@"{""key"":""value"",""another"":""something else""}", parameters["Parameters"]);
            Assert.Equal("My error", rule.ErrorMessageOrResourceString);
        }

        [Fact]
        public void CustomRule_Takes_JavaScriptFunction_Params_And_ErrorMessageAccessor()
        {
            var jsParam = new { key = "value", another = "something else" };
            string error = "";
            Func<string> errorAccessor = () => error;
            var rule = new CustomRule("myJsFunction", jsParam, errorAccessor);
            var parameters = rule.ListParameters();
            Assert.Equal(2, parameters.Count);
            Assert.Equal("myJsFunction", parameters["Function"]);
            Assert.Equal(@"{""key"":""value"",""another"":""something else""}", parameters["Parameters"]);
            error = "Updated error";
            Assert.Equal("Updated error", rule.ErrorMessageOrResourceString);
        }

        [Fact]
        public void CustomRule_Takes_JavaScriptFunction_Params_And_ErrorMessage_Resource_Info()
        {
            var jsParam = new { key = "value", another = "something else" };
            var rule = new CustomRule("myJsFunction", jsParam, typeof(MyResources), "SomeMessage");
            var parameters = rule.ListParameters();
            Assert.Equal(2, parameters.Count);
            Assert.Equal("myJsFunction", parameters["Function"]);
            Assert.Equal(@"{""key"":""value"",""another"":""something else""}", parameters["Parameters"]);
            Assert.Equal("Hello from MyResources", rule.ErrorMessageOrResourceString);
        }

        [Fact]
        public void RemoteRule_Takes_Url()
        {
            var remoteRule = new RemoteRule("/some/test/url");
            var parameters = remoteRule.ListParameters();            
            Assert.Equal(1, parameters.Count);
            Assert.Equal("/some/test/url", parameters["url"]);
        }

        private static class MyResources
        {
            public static string SomeMessage { get { return "Hello from MyResources"; } }
        }
    }
}