using System;
using System.Collections.Generic;

namespace xVal.Rules
{
    public class RangeRule : Rule
    {
        public object Min { get; private set; }
        public object Max { get; private set; }
        private Type BoundType { get; set; }

        public RangeRule(int? min, int? max) : this((object)min, (object)max) { }
        public RangeRule(decimal? min, decimal? max) : this((object)min, (object)max) { }
        public RangeRule(string min, string max) : this((object)min, (object)max) { }
        public RangeRule(DateTime? min, DateTime? max) : this((object)min, (object)max) { }

        private RangeRule(object min, object max) : base("Range")
        {
            if((min == null) && (max == null))
                throw new ArgumentException("Specify min or max or both");
            Min = min;
            Max = max;
            BoundType = (min ?? max).GetType();
        }

        public override IDictionary<string, string> ListParameters()
        {
            var result = base.ListParameters();

            if (BoundType == typeof(DateTime))
            {
                if(Min != null)
                    AddDateTimePartsToDictionary(result, ((DateTime?)Min).Value, "Min");
                if(Max != null)
                    AddDateTimePartsToDictionary(result, ((DateTime?)Max).Value, "Max");
            }
            else {
                if (Min != null) result.Add("Min", Min.ToString());
                if (Max != null) result.Add("Max", Max.ToString());
            }
            result.Add("Type", MakeTypeDescription(BoundType));

            return result;
        }

        private static string MakeTypeDescription(Type type)
        {
            if (type == typeof(int)) return "integer";
            if (type == typeof(decimal)) return "decimal";
            if (type == typeof(string)) return "string";
            if (type == typeof(DateTime)) return "datetime";
            throw new ArgumentException("Unexpected type");
        }

        private static void AddDateTimePartsToDictionary(IDictionary<string, string> collection, DateTime value, string prefix)
        {
            collection.Add(prefix + "Year", value.Year.ToString());
            collection.Add(prefix + "Month", value.Month.ToString());
            collection.Add(prefix + "Day", value.Day.ToString());
            collection.Add(prefix + "Hour", value.Hour.ToString());
            collection.Add(prefix + "Minute", value.Minute.ToString());
            collection.Add(prefix + "Second", value.Second.ToString());
        }
    }
}