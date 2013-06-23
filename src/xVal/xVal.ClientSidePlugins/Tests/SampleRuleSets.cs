using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using xVal.RuleProviders;
using xVal.Rules;
using System.Linq;

namespace xVal.ClientSidePlugins.TestHelpers
{
    public static class SampleRuleSets
    {
        public static RuleSet Person
        {
            get
            {
                var rules = new List<KeyValuePair<string, Rule>> {
                    new KeyValuePair<string, Rule>("Name", new RequiredRule { ErrorMessage = "State your name"}),
                    new KeyValuePair<string, Rule>("Age", new RangeRule(0, 150) { ErrorMessage = "Age must be within range 0-150"})
                };
                return new RuleSet(rules.ToLookup(x => x.Key, x => x.Value));
            }
        }

        public static RuleSet AllPossibleRules
        {
            get
            {
                var rules = new List<KeyValuePair<string, Rule>> {
                    new KeyValuePair<string, Rule>("RequiredField", new RequiredRule { ErrorMessage = "This is a custom error message for a required field"}),
                    new KeyValuePair<string, Rule>("DataType_EmailAddress_Field", new DataTypeRule(DataTypeRule.DataType.EmailAddress)),
                    new KeyValuePair<string, Rule>("DataType_CreditCardLuhn_Field", new DataTypeRule(DataTypeRule.DataType.CreditCardLuhn)),
                    new KeyValuePair<string, Rule>("DataType_Integer_Field", new DataTypeRule(DataTypeRule.DataType.Integer)),
                    new KeyValuePair<string, Rule>("DataType_Decimal_Field", new DataTypeRule(DataTypeRule.DataType.Decimal)),
                    new KeyValuePair<string, Rule>("DataType_Date_Field", new DataTypeRule(DataTypeRule.DataType.Date)),
                    new KeyValuePair<string, Rule>("DataType_DateTime_Field", new DataTypeRule(DataTypeRule.DataType.DateTime)),
                    new KeyValuePair<string, Rule>("DataType_Currency_Field", new DataTypeRule(DataTypeRule.DataType.Currency)),
                    new KeyValuePair<string, Rule>("Regex_Field", new RegularExpressionRule("[A-Z]\\d{3}") { ErrorMessage = "Enter a value of the form 'X123'"}),
                    new KeyValuePair<string, Rule>("Regex_CaseInsensitive_Field", new RegularExpressionRule("[A-Z]{3}", RegexOptions.IgnoreCase) { ErrorMessage = "Enter a value of the form 'aBc'"}),
                    new KeyValuePair<string, Rule>("Range_Int_Field", new RangeRule((int)5, (int)10)),
                    new KeyValuePair<string, Rule>("Range_Decimal_Field", new RangeRule(null, 10.98m)),
                    new KeyValuePair<string, Rule>("Range_String_Field", new RangeRule("aardvark", "antelope")),
                    new KeyValuePair<string, Rule>("Range_DateTime_Min_Field", new RangeRule(new DateTime(2001, 2, 19, 17, 04, 59), (DateTime?)null)),
                    new KeyValuePair<string, Rule>("Range_DateTime_Max_Field", new RangeRule((DateTime?)null, new DateTime(2001, 2, 19, 17, 04, 59))),
                    new KeyValuePair<string, Rule>("Range_DateTime_Range_Field", new RangeRule(new DateTime(2001, 2, 19, 17, 04, 59), new DateTime(2003, 4, 15, 6, 20, 25))),
                    new KeyValuePair<string, Rule>("StringLength_Min_Field", new StringLengthRule(3, null)),
                    new KeyValuePair<string, Rule>("StringLength_Max_Field", new StringLengthRule(null, 6)),
                    new KeyValuePair<string, Rule>("StringLength_Range_Field", new StringLengthRule(4, 7)),
                    new KeyValuePair<string, Rule>("Comparison_Equals", new ComparisonRule("RequiredField", ComparisonRule.Operator.Equals)),
                    new KeyValuePair<string, Rule>("Comparison_DoesNotEqual", new ComparisonRule("RequiredField", ComparisonRule.Operator.DoesNotEqual)),
                    new KeyValuePair<string, Rule>("Custom", new CustomRule("EqualsFixedStringRule", new { mustMatch = "hello" }, "Please enter the string 'hello'" )),
                    new KeyValuePair<string, Rule>("RemotelyValidated_Field", null)
                };
                return new RuleSet(rules.ToLookup(x => x.Key, x => x.Value));
            }
        }
    }
}