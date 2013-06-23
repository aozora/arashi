using System;
using System.Collections.Generic;
using Xunit;
using xVal.Html;
using xVal.RuleProviders;
using xVal.Tests.TestHelpers;
using System.Linq;

namespace xVal.Tests.HtmlHelpers
{
    public class JsonValidationConfigFormatterTests
    {
        [Fact]
        public void EmptyRulesetFormatting()
        {
            // Arrange
            var formatter = new JsonValidationConfigFormatter();
            var rules = RuleSet.Empty;

            // Act
            var result = formatter.FormatRules(rules);

            // Assert
            Assert.Equal(@"{""Fields"":[]}", result);
        }

        [Fact]
        public void Single_Rule()
        {
            // Arrange
            var formatter = new JsonValidationConfigFormatter();
            var rules = RuleSetHelpers.MakeTestRuleSet(new Dictionary<string, IDictionary<string, object>> {
                { 
                    "myprop", new Dictionary<string, object> {
                        { "somerule", new { param1 = "param1value", param2 = "param2value" } }    
                    } 
                }
            });
            rules["myprop"].First().ErrorMessage = "My_error_message";

            // Act
            var result = formatter.FormatRules(rules);

            // Assert
            Assert.Equal(@"{
    ""Fields"": [
        {""FieldName"" :""myprop"",
         ""FieldRules"":[
            {""RuleName"":""somerule"",
             ""RuleParameters"":{
                ""param1"":""param1value"",
                ""param2"":""param2value""
                },
             ""Message"":""My_error_message""
            }
        ]}
    ]
}".Replace(" ", "").Replace(Environment.NewLine, ""), result);
        }
    }
}