using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using xVal.Rules;

namespace xVal.RuleProviders
{
    public abstract class PropertyAttributeRuleProviderBase<TAttribute> : CachingRulesProvider where TAttribute : Attribute
    {
        private readonly Func<Type, TypeDescriptionProvider> metadataProviderFactory; // Yes, it's a factory factory factory. Just trying to be consistent with the Dynamic Data API (http://mattberseth.com/blog/2008/08/dynamic_data_and_custom_metada.html)

        protected PropertyAttributeRuleProviderBase() : this(null) { }
        protected PropertyAttributeRuleProviderBase(Func<Type, TypeDescriptionProvider> metadataProviderFactory)
        {
            this.metadataProviderFactory = metadataProviderFactory ?? (x => new AssociatedMetadataTypeTypeDescriptionProvider(x));
        }

        protected override RuleSet GetRulesFromTypeCore(Type type)
        {
            var typeDescriptor = metadataProviderFactory(type).GetTypeDescriptor(type);
            var rules = (from prop in typeDescriptor.GetProperties().Cast<PropertyDescriptor>()
                         from rule in GetRulesFromProperty(prop)
                         select new KeyValuePair<string, Rule>(prop.Name, rule));
            return new RuleSet(rules.ToLookup(x => x.Key, x => x.Value));
        }

        protected virtual IEnumerable<Rule> GetRulesFromProperty(PropertyDescriptor propertyDescriptor)
        {
            return from att in propertyDescriptor.Attributes.OfType<TAttribute>()
                   from validationRule in MakeValidationRulesFromAttribute(att)
                   where validationRule != null
                   select validationRule;
        }

        protected abstract IEnumerable<Rule> MakeValidationRulesFromAttribute(TAttribute att);
    }
}