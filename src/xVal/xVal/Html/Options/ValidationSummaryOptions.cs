using System;

namespace xVal.Html.Options
{
    internal class ValidationSummaryOptions
    {
        public string ElementID { get; private set; }
        public string HeaderMessage { get; private set; }

        public ValidationSummaryOptions(string elementId, string headerMessage)
        {
            if (string.IsNullOrEmpty(elementId))
                throw new ArgumentException("Cannot be null or empty", "elementId");
            if (headerMessage == string.Empty)
                throw new ArgumentException("headerMessage cannot be empty (pass null if you don't want to display a header message)");

            ElementID = elementId;
            HeaderMessage = headerMessage;
        }
    }
}