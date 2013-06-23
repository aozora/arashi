using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace xVal.Rules
{
    public abstract class Rule
    {
        public string RuleName { get; private set; }
        private string errorMessage;
        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                if((errorMessageResourceType != null) || (errorMessageResourceName != null))
                    throw new InvalidOperationException("Can't set ErrorMessage: this Rule is already in resource mode");
                errorMessage = value;
            }
        }

        private Type errorMessageResourceType;
        public Type ErrorMessageResourceType
        {
            get { return errorMessageResourceType; }
            set
            {
                if(errorMessage != null)
                    throw new InvalidOperationException("Can't set ErrorMessageResourceType: this Rule is already in fixed-string mode");
                errorMessageResourceType = value;
            }
        }

        private string errorMessageResourceName;
        public string ErrorMessageResourceName
        {
            get { return errorMessageResourceName; }
            set
            {
                if (errorMessage != null)
                    throw new InvalidOperationException("Can't set ErrorMessageResourceName: this Rule is already in fixed-string mode");
                errorMessageResourceName = value;
            }
        }

        private readonly static Type[] EmptyTypeArray = new Type[] {};
        public virtual string ErrorMessageOrResourceString
        {
            get
            {
                if(errorMessage != null)
                    return errorMessage;
                else if ((errorMessageResourceType != null) && (errorMessageResourceName != null)) {
                    var property = errorMessageResourceType.GetProperty(errorMessageResourceName, BindingFlags.Public | BindingFlags.Static, null, typeof(string), EmptyTypeArray, null);
                    return (string)property.GetValue(null, null);
                }
                else
                    return null;
            }
        }

        protected Rule(string ruleName)
        {
            RuleName = ruleName;
        }

        public virtual IDictionary<string, string> ListParameters()
        {
            return new Dictionary<string, string>();
        }
    }
}