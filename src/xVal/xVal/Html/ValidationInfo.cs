using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using xVal.Html.Options;
using xVal.RuleProviders;
using xVal.Rules;
using System.Linq;

namespace xVal.Html
{
    /// <summary>
    /// Represents a validation configuration that can be rendered to the client
    /// </summary>
    public class ValidationInfo
    {
        private const string clientSideOptionsKey_validationSummaryOptions = "ValidationSummary";
        private static readonly JavaScriptSerializer Serializer = new JavaScriptSerializer();
        public static IValidationConfigFormatter Formatter = new JsonValidationConfigFormatter();

        private enum RenderMode
        {
            ConfigOnly,          // Renders just the formatted validation config
            AttachScript,        // Renders xVal.AttachValidator(...config...)
            CompleteScriptBlock  // Renders <script>xVal.AttachValidator( ...config... )</script>
        }

        private RenderMode currentRenderMode;
        private RuleSet rules;
        private readonly List<KeyValuePair<string, Rule>> addedRules = new List<KeyValuePair<string, Rule>>();
        private readonly string elementPrefix;
        private readonly IDictionary<string, object> clientSideOptions = new Dictionary<string, object>();

        /// <summary>
        /// Constructs a ValidationInfo that simply format and render the rules
        /// </summary>
        public ValidationInfo(RuleSet rules)
        {
            if (rules == null) throw new ArgumentNullException("rules");
            this.rules = rules;
            currentRenderMode = RenderMode.ConfigOnly;
        }

        /// <summary>
        /// Constructs a ValidationInfo that will render JavaScript code that attaches the rules to DOM elements
        /// </summary>
        public ValidationInfo(RuleSet rules, string attachToElementPrefix) : this(rules)
        {
            currentRenderMode = RenderMode.CompleteScriptBlock;
            elementPrefix = attachToElementPrefix;
        }

        public ValidationInfo AddRule(string propertyName, Rule rule)
        {
            addedRules.Add(new KeyValuePair<string, Rule>(propertyName, rule));
            return this;
        }

        public ValidationInfo SuppressScriptTags()
        {
            if (currentRenderMode == RenderMode.CompleteScriptBlock)
                currentRenderMode = RenderMode.AttachScript;
            return this;
        }

        public ValidationInfo UseValidationSummary(string elementID)
        {
            return UseValidationSummary(elementID, null);
        }

        public ValidationInfo UseValidationSummary(string elementID, string message)
        {
            if (clientSideOptions.ContainsKey(clientSideOptionsKey_validationSummaryOptions))
                throw new InvalidOperationException("Validation summary has already been configured once for this helper - don't try to configure it twice.");
            clientSideOptions[clientSideOptionsKey_validationSummaryOptions] = new ValidationSummaryOptions(elementID, message);
            return this;
        }

        public override string ToString()
        {
            MergeAddedRulesIntoRuleSet();
            var formattedRules = Formatter.FormatRules(rules);
            if (currentRenderMode == RenderMode.ConfigOnly)
                return formattedRules;
            else {
                var elementPrefixOrNull = elementPrefix == null ? "null" : string.Format("\"{0}\"", elementPrefix);
                var optionsJson = Serializer.Serialize(clientSideOptions);
                var attachValidatorStatement = string.Format("xVal.AttachValidator({0}, {1}, {2})", elementPrefixOrNull, formattedRules, optionsJson);
                if (currentRenderMode == RenderMode.AttachScript)
                    return attachValidatorStatement;
                else if(currentRenderMode == RenderMode.CompleteScriptBlock)
                    return WrapInScriptTag(attachValidatorStatement);
                else {
                    throw new InvalidOperationException("Unknown render mode: " + currentRenderMode);
                }
            }
        }

        private static string WrapInScriptTag(string statement)
        {
            var tb = new TagBuilder("script");
            tb.MergeAttribute("type", "text/javascript");
            tb.InnerHtml = statement;
            return tb.ToString(TagRenderMode.Normal);
        }

        private void MergeAddedRulesIntoRuleSet()
        {
            if(addedRules.Count > 0) {
                var addedRuleSet = new RuleSet(addedRules.ToLookup(x => x.Key, x => x.Value));
                rules = new RuleSet(new[] { rules, addedRuleSet });
                addedRules.Clear();
            }
        }
    }
}