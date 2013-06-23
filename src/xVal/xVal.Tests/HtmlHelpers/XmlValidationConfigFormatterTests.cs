using System.Collections.Generic;
using System.Web.Routing;
using Xunit;
using xVal.Html;
using xVal.RuleProviders;
using xVal.Rules;
using System.Linq;
using xVal.Tests.TestHelpers;

namespace xVal.Tests.HtmlHelpers
{
    public class XmlValidationConfigFormatterTests
    {
        [Fact]
        public void Empty_Ruleset_Formatted_With_Name()
        {
            // Arrange
            var formatter = new XmlValidationConfigFormatter();

            // Act
            var result = formatter.FormatRules(RuleSet.Empty);

            // Assert
            Assert.Equal("<xval:ruleset />", result);
        }

        [Fact]
        public void Single_Rule()
        {
            // Arrange
            var formatter = new XmlValidationConfigFormatter();
            var rules = RuleSetHelpers.MakeTestRuleSet(new Dictionary<string, IDictionary<string, object>> {
                { 
                    "myprop", new Dictionary<string, object> {
                        { "somerule", new { param1 = "param1value", param2 = "param2value" } }    
                    } 
                }
            });
            rules["myprop"].First().ErrorMessage = "My error message";

            // Act
            var result = formatter.FormatRules(rules);

            // Assert
            Assert.Equal(@"<xval:ruleset>
    <somerule errmsg=""My error message"" forfield=""myprop"" param1=""param1value"" param2=""param2value"" />
</xval:ruleset>", result);
        }

        [Fact]
        public void Multiple_Rules()
        {
            // Arrange
            var formatter = new XmlValidationConfigFormatter();
            var rules = RuleSetHelpers.MakeTestRuleSet(new Dictionary<string, IDictionary<string, object>> {
                { 
                    "screenplay", new Dictionary<string, object> {
                        { "copyright", new { author = "Wm. Shakespeare", year = "1668" } },
                        { "description", new { language = "Welsh", grammar = "perfect", length = "long" } }    
                    }
                },
                {
                    "petname", new Dictionary<string, object> {
                        { "required", new { } },
                        { "DataType", new { @type = "EmailAddress", domainSuffix = ".co.uk" } },
                        { "LengthConstraint", new { max = "150" } }
                    }
                }
            });

            // Act
            var result = formatter.FormatRules(rules);

            // Assert
            // Notice that the attributes are re-ordered (into alphabetical order)
            // This test code is a bit flaky because the attributes could be ordered differently in future
            // Not sure if there's a better way to test this (maybe will parse the output as XML then inspect independently of order)
            Assert.Equal(@"<xval:ruleset>
    <copyright author=""Wm. Shakespeare"" forfield=""screenplay"" year=""1668"" />
    <description forfield=""screenplay"" grammar=""perfect"" language=""Welsh"" length=""long"" />
    <required forfield=""petname"" />
    <DataType domainSuffix="".co.uk"" forfield=""petname"" type=""EmailAddress"" />
    <LengthConstraint forfield=""petname"" max=""150"" />
</xval:ruleset>", result);
        }

    }
}