using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;

namespace xVal.ServerSide
{
    public class RulesException : Exception
    {
        public RulesException(IEnumerable<ErrorInfo> errors)
        {
            Errors = errors;
        }

        public RulesException(string propertyName, string errorMessage)
            : this(propertyName, errorMessage, null) {}

        public RulesException(string propertyName, string errorMessage, object onObject)
        {
            Errors = new[] { new ErrorInfo(propertyName, errorMessage, onObject) };
        }

        public IEnumerable<ErrorInfo> Errors { get; private set; }

        public void AddModelStateErrors(ModelStateDictionary modelState, string prefix)
        {
            AddModelStateErrors(modelState, prefix, x => true);
        }

        public void AddModelStateErrors(ModelStateDictionary modelState, string prefix, Func<ErrorInfo, bool> errorFilter)
        {
            if (errorFilter == null) throw new ArgumentNullException("errorFilter");
            prefix = prefix == null ? "" : prefix + ".";
            foreach (var errorInfo in Errors.Where(errorFilter)) {
                var key = prefix + errorInfo.PropertyName;
                modelState.AddModelError(key, errorInfo.ErrorMessage);

                // Workaround for http://xval.codeplex.com/WorkItem/View.aspx?WorkItemId=1297 (ASP.NET MVC bug)
                // Ensure that some value object is registered in ModelState under this key
                ModelState existingModelStateValue;
                if(modelState.TryGetValue(key, out existingModelStateValue) && existingModelStateValue.Value == null)
                    existingModelStateValue.Value = new ValueProviderResult(null, null, null);
            }
        }
    }
}