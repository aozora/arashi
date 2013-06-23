using System;
using System.Collections.Generic;
using System.Reflection;
using Castle.Components.Validator;
using System.Linq;
using xVal.RuleProviders;
using xVal.Rules;

namespace xVal.RulesProviders.CastleValidator
{
    public class CastleValidatorRulesProvider : CachingRulesProvider
    {
        private readonly IValidatorRegistry registry;
        private readonly ValidatorRunner runner;
        private readonly RuleEmitterList<IValidator> ruleEmitters = new RuleEmitterList<IValidator>();

        public CastleValidatorRulesProvider()
        {
            registry = new CachedValidationRegistry();
            runner = new ValidatorRunner(registry);
            ruleEmitters.AddSingle<NonEmptyValidator>(x => new RequiredRule());
            ruleEmitters.AddSingle<CreditCardValidator>(x => new DataTypeRule(DataTypeRule.DataType.CreditCardLuhn));
            ruleEmitters.AddMultiple<DateTimeValidator>(x => new Rule[] {
                new RequiredRule(),
                new DataTypeRule(DataTypeRule.DataType.DateTime)
            });
            ruleEmitters.AddMultiple<DateValidator>(x => new Rule[] {
                new RequiredRule(),
                new DataTypeRule(DataTypeRule.DataType.Date)
            });
            ruleEmitters.AddMultiple<IntegerValidator>(x => new Rule[] {
                new RequiredRule(),
                new DataTypeRule(DataTypeRule.DataType.Integer)
            });
            ruleEmitters.AddMultiple<IValidator>(x => {
                if ((x is DecimalValidator) || (x is DoubleValidator) || (x is SingleValidator)) {
                    return new Rule[] {
                        new RequiredRule(),
                        new DataTypeRule(DataTypeRule.DataType.Decimal)
                    };
                } else
                    return null;
            });
            ruleEmitters.AddSingle<LengthValidator>(ConstructStringLengthRule);
            ruleEmitters.AddSingle<RangeValidator>(ConstructRangeRule);
            ruleEmitters.AddSingle<EmailValidator>(x => new DataTypeRule(DataTypeRule.DataType.EmailAddress));
            ruleEmitters.AddSingle<RegularExpressionValidator>(x => new RegularExpressionRule(x.Expression, x.RegexRule.Options));
            ruleEmitters.AddSingle<SameAsValidator>(x => new ComparisonRule(x.PropertyToCompare, ComparisonRule.Operator.Equals));
            ruleEmitters.AddSingle<NotSameAsValidator>(x => new ComparisonRule(x.PropertyToCompare, ComparisonRule.Operator.DoesNotEqual));
        }

        protected override RuleSet GetRulesFromTypeCore(Type type)
        {
            var validators = registry.GetValidators(runner, type, RunWhen.Everytime);
            var allRules = from val in validators
                           from rule in ConvertToXValRules(val)
                           select new KeyValuePair<string, Rule>(val.Property.Name, rule);
            return new RuleSet(allRules.ToLookup(x => x.Key, x => x.Value));
        }

        private IEnumerable<Rule> ConvertToXValRules(IValidator val)
        {
            var rules = ruleEmitters.EmitRules(val);
            if (!string.IsNullOrEmpty(val.ErrorMessage))
                foreach (var rule in rules)
                    rule.ErrorMessage = val.ErrorMessage;
            return rules;
        }

        private static StringLengthRule ConstructStringLengthRule(LengthValidator lengthValidator)
        {
            if(lengthValidator.ExactLength != int.MinValue)
                return new StringLengthRule(lengthValidator.ExactLength, lengthValidator.ExactLength);
            else if((lengthValidator.MinLength != int.MaxValue) || (lengthValidator.MaxLength != int.MaxValue)) {
                return new StringLengthRule(
                    /* Min length */ lengthValidator.MinLength == int.MinValue ? (int?)null : lengthValidator.MinLength,
                    /* Max length */ lengthValidator.MaxLength == int.MaxValue ? (int?)null : lengthValidator.MaxLength
                );
            }
            return null;
        }

        // Due to an annoying inconsistency in Castle Validator, there's no way to determine the 
        // min and max values for a RangeValidator other than by peeking at a private field.
        // (All other validators expose a useful public description of themselves.)
        private readonly static FieldInfo rangeValidatorMinField = typeof(RangeValidator).GetField("min", BindingFlags.Instance | BindingFlags.NonPublic);
        private readonly static FieldInfo rangeValidatorMaxField = typeof(RangeValidator).GetField("max", BindingFlags.Instance | BindingFlags.NonPublic);
        private static RangeRule ConstructRangeRule(RangeValidator validator)
        {
            object minValue = rangeValidatorMinField.GetValue(validator);
            object maxValue = rangeValidatorMaxField.GetValue(validator);
            switch (validator.Type) {
                    // RangeValidator's convention is to use type.MinValue/type.MaxValue/type.Empty to
                    // signal "no boundary at this end", whereas xVal uses null.
                case RangeValidationType.Integer:
                    var minInt = (int) minValue == int.MinValue ? (int?) null : (int) minValue;
                    var maxInt = (int) maxValue == int.MaxValue ? (int?) null : (int) maxValue;
                    return new RangeRule(minInt, maxInt);
                case RangeValidationType.Decimal:
                    var minDec = (decimal)minValue == decimal.MinValue ? (decimal?)null : (decimal)minValue;
                    var maxDec = (decimal)maxValue == decimal.MaxValue ? (decimal?)null : (decimal)maxValue;
                    return new RangeRule(minDec, maxDec);
                case RangeValidationType.DateTime:
                    var minDateTime = (DateTime)minValue == DateTime.MinValue ? (DateTime?)null : (DateTime)minValue;
                    var maxDateTime = (DateTime)maxValue == DateTime.MaxValue ? (DateTime?)null : (DateTime)maxValue;
                    return new RangeRule(minDateTime, maxDateTime);
                case RangeValidationType.String:
                    var minString = (string)minValue == string.Empty ? null : (string)minValue;
                    var maxString = (string)maxValue == string.Empty ? null : (string)maxValue;
                    return new RangeRule(minString, maxString);
            }
            return null; // Ignore unknown RangeValidationType
        }
    }
}