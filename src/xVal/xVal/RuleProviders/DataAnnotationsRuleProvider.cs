using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.RegularExpressions;
using xVal.Rules;
using System.Linq;

namespace xVal.RuleProviders
{
    public class DataAnnotationsRuleProvider : PropertyAttributeRuleProviderBase<ValidationAttribute>
    {        
        private static readonly Type[] NumericTypes = new[] { typeof(int), typeof(double), typeof(decimal), typeof(float) };
        private readonly RuleEmitterList<ValidationAttribute> ruleEmitters = new RuleEmitterList<ValidationAttribute>();

        public DataAnnotationsRuleProvider() : this(null)
        {
        }

        public DataAnnotationsRuleProvider(Func<Type, TypeDescriptionProvider> metadataProviderFactory) : base(metadataProviderFactory)
        {
            ruleEmitters.AddSingle<RequiredAttribute>(x => new RequiredRule());
            ruleEmitters.AddSingle<StringLengthAttribute>(x => new StringLengthRule(null, x.MaximumLength));
            ruleEmitters.AddSingle<RangeAttribute>(ConvertRangeAttribute);
            ruleEmitters.AddSingle<DataTypeAttribute>(ConvertDataTypeAttribute);
            ruleEmitters.AddSingle<RegularExpressionAttribute>(x => new RegularExpressionRule(x.Pattern));
        }

        protected override IEnumerable<Rule> GetRulesFromProperty(PropertyDescriptor propertyDescriptor)
        {
            return base.GetRulesFromProperty(propertyDescriptor)
                   .Union(GetNumericValueTypeRulesFromProperty(propertyDescriptor));
        }

        private static IEnumerable<Rule> GetNumericValueTypeRulesFromProperty(PropertyDescriptor propertyDescriptor)
        {
            // System.ComponentModel.DataAnnotations doesn't have any attribute to represent "int" or "double",
            // so we'll infer it directly from the property type
            Type unwrappedType = UnwrapIfNullable(propertyDescriptor.PropertyType);
            if (Array.IndexOf(NumericTypes, unwrappedType) >= 0) {
                if (unwrappedType == typeof(int))
                    yield return new DataTypeRule(DataTypeRule.DataType.Integer);
                else
                    yield return new DataTypeRule(DataTypeRule.DataType.Decimal);
            }
        }

        private static Type UnwrapIfNullable(Type type)
        {
            return Nullable.GetUnderlyingType(type) ?? type;
        }

        protected override IEnumerable<Rule> MakeValidationRulesFromAttribute(ValidationAttribute att)
        {
            var rules = ruleEmitters.EmitRules(att);
            foreach (var rule in rules)
                ApplyErrorMessage(att, rule);
            return rules;
        }

        private static void ApplyErrorMessage(ValidationAttribute att, Rule result)
        {
            if(att.ErrorMessage != null)
                result.ErrorMessage = att.ErrorMessage;
            else {
                result.ErrorMessageResourceType = att.ErrorMessageResourceType;
                result.ErrorMessageResourceName = att.ErrorMessageResourceName;
            }
        }

        private Rule ConvertDataTypeAttribute(DataTypeAttribute dt)
        {
            // Is this one that should be handled as a RegEx?
            string regEx = ToRegEx(dt.DataType);
            if (regEx != null)
                return new RegularExpressionRule(regEx, RegexOptions.IgnoreCase);
            // No, it must be one we have a native type for
            var xValDataType = ToXValDataType(dt.DataType);
            if (xValDataType != DataTypeRule.DataType.Text) // Ignore "text" - nothing to validate
                return new DataTypeRule(xValDataType);
            return null;
        }

        private static Rule ConvertRangeAttribute(RangeAttribute r)
        {
            if (r.OperandType == typeof (string))
                return new RangeRule(Convert.ToString(r.Minimum), Convert.ToString(r.Maximum));
            else if (r.OperandType == typeof (DateTime))
                return new RangeRule(r.Minimum == null ? (DateTime?) null : Convert.ToDateTime(r.Minimum), r.Maximum == null ? (DateTime?) null : Convert.ToDateTime(r.Maximum));
            else if (Array.IndexOf(NumericTypes, r.OperandType) >= 0)
                return new RangeRule(r.Minimum == null ? (decimal?) null : Convert.ToDecimal(r.Minimum), r.Maximum == null ? (decimal?) null : Convert.ToDecimal(r.Maximum));
            else // Can't compare any other type
                return null;
        }

        private string ToRegEx(DataType dataType)
        {
            switch (dataType) {
                case DataType.Time:
                    return RegularExpressionRule.Regex_Time;
                case DataType.Duration:
                    return RegularExpressionRule.Regex_Duration;
                case DataType.PhoneNumber:
                    return RegularExpressionRule.Regex_USPhoneNumber;
                case DataType.Url:
                    return RegularExpressionRule.Regex_Url;
                default:
                    return null;
            }
        }

        private static DataTypeRule.DataType ToXValDataType(DataType type)
        {
            switch(type) {
                case DataType.DateTime:
                    return DataTypeRule.DataType.DateTime;
                case DataType.Date:
                    return DataTypeRule.DataType.Date;
                case DataType.Currency:
                    return DataTypeRule.DataType.Currency;
                case DataType.EmailAddress:
                    return DataTypeRule.DataType.EmailAddress;
                case DataType.Custom:
                case DataType.Text:
                case DataType.Html:
                case DataType.MultilineText:
                case DataType.Password:
                    return DataTypeRule.DataType.Text;
                default:
                    throw new InvalidOperationException("Unknown data type: " + type.ToString());
            }
        }
    }
}