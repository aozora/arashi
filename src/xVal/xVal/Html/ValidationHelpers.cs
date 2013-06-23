using System;
using System.Web.Mvc;
using System.Xml.Linq;
using xVal.RuleProviders;

namespace xVal.Html
{
    public static class ValidationHelpers
    {
        public static ValidationInfo ClientSideValidationRules(this HtmlHelper html, Type modelType)
        {
            if (modelType == null) throw new ArgumentNullException("modelType");
            return new ValidationInfo(ActiveRuleProviders.GetRulesForType(modelType));
        }

        public static ValidationInfo ClientSideValidationRules<TModel>(this HtmlHelper html)
        {
            return ClientSideValidationRules(html, typeof(TModel));
        }

        public static ValidationInfo ClientSideValidationRules(this HtmlHelper html, RuleSet rules)
        {
            return new ValidationInfo(rules);
        }

        public static ValidationInfo ClientSideValidation(this HtmlHelper html, string elementPrefix, RuleSet rules)
        {
            return new ValidationInfo(rules, elementPrefix);
        }

        public static ValidationInfo ClientSideValidation(this HtmlHelper html, string elementPrefix, Type modelType)
        {
            return ClientSideValidation(html, elementPrefix, ActiveRuleProviders.GetRulesForType(modelType));
        }

        public static ValidationInfo ClientSideValidation(this HtmlHelper html, Type modelType)
        {
            return ClientSideValidation(html, null, modelType);
        }

        public static ValidationInfo ClientSideValidation<TModel>(this HtmlHelper html, string elementPrefix)
        {
            return ClientSideValidation(html, elementPrefix, typeof (TModel));
        }

        public static ValidationInfo ClientSideValidation<TModel>(this HtmlHelper html)
        {
            return ClientSideValidation(html, null, typeof(TModel));
        }
    }
}