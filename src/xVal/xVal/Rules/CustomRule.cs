using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace xVal.Rules
{
    public class CustomRule : Rule
    {
        public string JavaScriptFunction { get; private set; }
        public object Parameters { get; private set; }
        private Func<string> ErrorMessageAccessor { get; set; }

        public static readonly JavaScriptSerializer Serializer = new JavaScriptSerializer();

        public CustomRule(string javaScriptFunction, object parameters, string errorMessage) : this(javaScriptFunction, parameters)
        {
            ErrorMessage = errorMessage;
        }

        public CustomRule(string javaScriptFunction, object parameters, Type errorMessageResourceType, string errorMessageResourceName)
            : this(javaScriptFunction, parameters)
        {
            ErrorMessageResourceType = errorMessageResourceType;
            ErrorMessageResourceName = errorMessageResourceName;
        }

        public CustomRule(string javaScriptFunction, object parameters, Func<string> errorMessageAccessor) : this(javaScriptFunction, parameters)
        {
            ErrorMessageAccessor = errorMessageAccessor;
        }

        private CustomRule(string javaScriptFunction, object parameters) : base("Custom")
        {
            JavaScriptFunction = javaScriptFunction;
            Parameters = parameters;
        }

        public override IDictionary<string, string> ListParameters()
        {
            var result = base.ListParameters();
            result.Add("Function", JavaScriptFunction);
            result.Add("Parameters", Serializer.Serialize(Parameters));
            return result;
        }

        public override string ErrorMessageOrResourceString
        {
            get
            {
                if(ErrorMessageAccessor != null)
                    return ErrorMessageAccessor();
                else
                    return base.ErrorMessageOrResourceString;
            }
        }
    }
}