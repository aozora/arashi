using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using xVal.RuleProviders;
using xVal.Rules;

namespace xVal.Html
{
    public class XmlValidationConfigFormatter : IValidationConfigFormatter
    {
        private const string TagName = "xval:ruleset";
        private const string NewLine = "\r\n";
        private const string Indent = "    ";

        public string FormatRules(RuleSet rules)
        {
            var tb = new TagBuilder(TagName);
            var rulesBuilder = new StringBuilder();
            var allRules = from key in rules.Keys
                           from rule in rules[key]
                           select new { key, rule };

            if (allRules.Any())
                rulesBuilder.Append(NewLine);
            foreach (var item in allRules) {
                rulesBuilder.Append(Indent);
                rulesBuilder.Append(FormatSingleRule(item.rule, item.key));
                rulesBuilder.Append(NewLine);
            }

            tb.InnerHtml = rulesBuilder.ToString();
            return tb.ToString(allRules.Any() ? TagRenderMode.Normal : TagRenderMode.SelfClosing);
        }

        private static string FormatSingleRule(Rule rule, string forField)
        {
            var tb = new TagBuilder(rule.RuleName);
            tb.MergeAttributes(rule.ListParameters());
            tb.MergeAttribute("forfield", forField);
            var errorMessage = rule.ErrorMessageOrResourceString;
            if(errorMessage != null)
                tb.MergeAttribute("errmsg", errorMessage);
            return tb.ToString(TagRenderMode.SelfClosing);
        }
    }
}