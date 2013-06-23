using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using NHibernate.Validator;
using NHibernate.Validator.Cfg;
using NHibernate.Validator.Cfg.MappingSchema;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Mappings;
using xVal.RuleProviders;
using xVal.Rules;

namespace xVal.RulesProviders.NHibernateValidator
{
    public class NHibernateValidatorRulesProvider : CachingRulesProvider
    {
        private readonly ValidatorMode configMode;
        private readonly RuleEmitterList<IRuleArgs> ruleEmitters = new RuleEmitterList<IRuleArgs>();

        public NHibernateValidatorRulesProvider(ValidatorMode configMode)
        {
            this.configMode = configMode;

            ruleEmitters.AddSingle<LengthAttribute>(x => new StringLengthRule(x.Min, x.Max));
            ruleEmitters.AddSingle<MinAttribute>(x => new RangeRule(x.Value, null));
            ruleEmitters.AddSingle<MaxAttribute>(x => new RangeRule(null, x.Value));
            ruleEmitters.AddSingle<RangeAttribute>(x => new RangeRule(x.Min, x.Max));
            ruleEmitters.AddSingle<NotEmptyAttribute>(x => new RequiredRule());
            ruleEmitters.AddSingle<NotNullNotEmptyAttribute>(x => new RequiredRule());
            ruleEmitters.AddSingle<PatternAttribute>(x => new RegularExpressionRule(x.Regex, x.Flags));
            ruleEmitters.AddSingle<EmailAttribute>(x => new DataTypeRule(DataTypeRule.DataType.EmailAddress));
            ruleEmitters.AddSingle<DigitsAttribute>(MakeDigitsRule);
        }

        protected override RuleSet GetRulesFromTypeCore(Type type)
        {
            var classMapping = ClassMappingFactory.GetClassMapping(type, configMode);

            var rules = from member in type.GetMembers()
                        where member.MemberType == MemberTypes.Field || member.MemberType == MemberTypes.Property
                        from att in classMapping.GetMemberAttributes(member).OfType<IRuleArgs>() // All NHibernate Validation validators attributes must implement this interface
                        from rule in ConvertToXValRules(att)
                        where rule != null
                        select new { MemberName = member.Name, Rule = rule };

            return new RuleSet(rules.ToLookup(x => x.MemberName, x => x.Rule));
        }

        private IEnumerable<Rule> ConvertToXValRules(IRuleArgs att)
        {
            foreach (var rule in ruleEmitters.EmitRules(att)) {
                if(rule != null) {
                    rule.ErrorMessage = MessageIfSpecified(att.Message);
                    yield return rule;
                }
            }
        }

        private string MessageIfSpecified(string message)
        {
            // We don't want to display the default {validator.*} messages
            if ((message != null) && !message.StartsWith("{validator."))
                return message;
            return null;
        }

        private RegularExpressionRule MakeDigitsRule(DigitsAttribute att)
        {
            if (att == null) throw new ArgumentNullException("att");
            string pattern;
            if (att.FractionalDigits < 1)
                pattern = string.Format(@"\d{{0,{0}}}", att.IntegerDigits);
            else
                pattern = string.Format(@"\d{{0,{0}}}(\.\d{{1,{1}}})?", att.IntegerDigits, att.FractionalDigits);
            return new RegularExpressionRule(pattern);
        }

        private static class ClassMappingFactory
        {
            public static IClassMapping GetClassMapping(Type type, ValidatorMode mode)
            {
                IClassMapping result = null;
                switch (mode) {
                    case ValidatorMode.UseAttribute:
                        break;
                    case ValidatorMode.UseXml:
                        result = new XmlClassMapping(GetXmlDefinitionFor(type));
                        break;
                    case ValidatorMode.OverrideAttributeWithXml:
                        var xmlDefinition = GetXmlDefinitionFor(type);
                        if (xmlDefinition != null)
                            result = new XmlOverAttributeClassMapping(xmlDefinition);
                        break;
                    case ValidatorMode.OverrideXmlWithAttribute:
                        var xmlDefinition2 = GetXmlDefinitionFor(type);
                        if (xmlDefinition2 != null)
                            result = new AttributeOverXmlClassMapping(xmlDefinition2);
                        break;
                }
                return result ?? new ReflectionClassMapping(type);
            }

            private static NhvmClass GetXmlDefinitionFor(Type type)
            {
                var mapp = MappingLoader.GetMappingFor(type);
                return mapp != null && mapp.@class.Length > 0 ? mapp.@class[0] : null;
            }
        }
    }
}