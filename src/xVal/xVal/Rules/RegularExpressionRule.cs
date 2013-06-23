using System;
using System.Text.RegularExpressions;

namespace xVal.Rules
{
    public class RegularExpressionRule : Rule
    {
        // Todo: check these regexes and add tests for them (especially Regex_Duration - how is one supposed to format a duration?)
        public const string Regex_Time = @"((([0]?[1-9]|1[0-2])(:|\.)[0-5][0-9]((:|\.)[0-5][0-9])?( )?(am|pm))|(([0]?[0-9]|1[0-9]|2[0-3])(:|\.)[0-5][0-9]((:|\.)[0-5][0-9])?))";
        public const string Regex_Duration = @"(\d\d\:){0,2}\d\d";
        public const string Regex_USPhoneNumber = @"([0-9]( |-)?)?(\(?[0-9]{3}\)?|[0-9]{3})( |-)?([0-9]{3}( |-)?[0-9]{4}|[a-zA-Z0-9]{7})";
        public const string Regex_Url = @"(http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?";

        public string Pattern { get; private set; }
        public RegexOptions Options { get; private set; }
        public RegularExpressionRule(string pattern) : this(pattern, RegexOptions.None) { }

        public RegularExpressionRule(string pattern, RegexOptions options) : base("RegEx")
        {
            if (pattern == null) throw new ArgumentNullException("pattern");
            Pattern = pattern;
            Options = options;
        }

        public override System.Collections.Generic.IDictionary<string, string> ListParameters()
        {
            var result = base.ListParameters();
            result.Add("Pattern", Pattern);

            string options = "";
            if ((Options & RegexOptions.IgnoreCase) == RegexOptions.IgnoreCase) options += "i";
            if ((Options & RegexOptions.Multiline) == RegexOptions.Multiline) options += "m";
            if(options != "")
                result.Add("Options", options);
            
            return result;
        }
    }
}