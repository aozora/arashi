namespace xVal.Rules
{
    public class ComparisonRule : Rule
    {
        public Operator ComparisonOperator { get; private set; }
        public string PropertyToCompare { get; private set; }

        public enum Operator
        {
            Equals, DoesNotEqual // Will consider adding LessThan/GreaterThan (+/- OrEquals) in due course. Not right now because of complexity of comparing dates.
        }

        public ComparisonRule(string propertyToCompare, Operator comparisonOperator) : base("Comparison")
        {
            PropertyToCompare = propertyToCompare;
            ComparisonOperator = comparisonOperator;
        }

        public override System.Collections.Generic.IDictionary<string, string> ListParameters()
        {
            var result = base.ListParameters();
            result.Add("PropertyToCompare", PropertyToCompare);
            result.Add("ComparisonOperator", ComparisonOperator.ToString());
            return result;
        }
    }
}