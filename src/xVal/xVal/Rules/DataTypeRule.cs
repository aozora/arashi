namespace xVal.Rules
{
    public class DataTypeRule : Rule
    {
        public DataType Type { get; private set; }

        public DataTypeRule(DataType dataType) : base("DataType")
        {
            Type = dataType;
        }

        public enum DataType
        {
            Integer,
            Decimal,
            Date,
            DateTime,
            Currency,
            EmailAddress,
            CreditCardLuhn,
            Text
        }

        public override System.Collections.Generic.IDictionary<string, string> ListParameters()
        {
            var result = base.ListParameters();
            result.Add("Type", Type.ToString());
                return result;
        }
    }
}