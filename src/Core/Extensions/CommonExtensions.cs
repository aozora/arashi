using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;
using Arashi.Core.Util;

namespace Arashi.Core.Extensions
{
   public static class CommonExtensions
   {
      /// <summary>
      /// Return a string encrypted with SHA2 algoritm
      /// </summary>
      /// <param name="plainString"></param>
      /// <returns></returns>
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "string"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "SHA")]
      public static string EncryptToSHA2(this string plainString)
      {
         return Encryption.SHA256Encrypt(plainString);
      }



      /// <summary>
      /// Return a string encrypted with MD5 algoritm
      /// </summary>
      /// <param name="plainString"></param>
      /// <returns></returns>
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "string")]
      public static string EncryptToMD5(this string plainString)
      {
         return Encryption.StringToMD5Hash(plainString);
      }


      /// <summary>
      /// Adjust a date to the timezone of the current user.
      /// </summary>
      /// <param name="dt"></param>
      /// <param name="timeZone"></param>
      /// <returns></returns>
      public static DateTime AdjustDateToTimeZone(this DateTime dt, int timeZone)
      {
         return TimeZoneUtil.AdjustDateToTimeZone(dt, timeZone);
      }



      /// <summary>
      /// Adjust a date from the timezone of the current user to the timezone of
      /// the server.
      /// </summary>
      /// <param name="dt"></param>
      /// <param name="userTimeZone"></param>
      /// <returns></returns>
      public static DateTime AdjustDateToServerTimeZone(this DateTime dt, int userTimeZone)
      {
         return TimeZoneUtil.AdjustDateToServerTimeZone(dt, userTimeZone);
      }






      /// <summary>
      /// Restituisce una stringa vuota se la string è null
      /// </summary>
      /// <param name="s"></param>
      /// <returns></returns>
      public static string EmptyIfNull(this object s)
      {
         return (s == null ? string.Empty : s.ToString());
      }



      /// <summary>
      /// Restituisce null se la tringa è vuota o null
      /// </summary>
      /// <param name="s"></param>
      /// <returns></returns>
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1820:TestForEmptyStringsUsingStringLength")]
      public static string NullIfEmpty(this string s)
      {
         return (s.Trim() == string.Empty ? null : s);
      }



      #region String Extensions

      /// <summary>
      /// Create a sanitized lowercase version of a string, removing illegal chars, stripping tags
      /// and replacing white spaces with dashes.
      /// </summary>
      /// <param name="s"></param>
      /// <returns></returns>
      public static string Sanitize(this string s)
      {
         if (String.IsNullOrEmpty(s))
            return string.Empty;

         // to lowercase, trim extra spaces
         string temp = s.ToLowerInvariant().Trim();

         // remove entities
         temp = Regex.Replace(temp, @"&\w+;", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Compiled);

         // remove diacritics (accents)
         temp = temp.RemoveDiacritics();

         // remove anything that is not letters, numbers, dash, or space
         temp = Regex.Replace(temp, @"[^A-Za-z0-9\-\s]", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Compiled);

         var len = temp.Length;
         var sb = new StringBuilder(len);
         bool prevdash = false;

         for (int i = 0; i < temp.Length; i++)
         {
            char c = temp[i];

            if (c == ' ' || c == ',' || c == '.' || c == '/' || c == '\\' || c == '-')
            {
               if (!prevdash)
               {
                  sb.Append('-');
                  prevdash = true;
               }
            }
            else if ((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9'))
            {
               sb.Append(c);
               prevdash = false;
            }

            if (i == 80)
               break;
         }

         temp = sb.ToString();
         
         // remove trailing dash, if there is one
         if (temp.EndsWith("-", StringComparison.OrdinalIgnoreCase))
            temp = temp.Substring(0, temp.Length - 1);
         
         return temp;
      }



      /// <summary>
      /// This method remove diacritics (accents) from a string
      /// </summary>
      /// <param name="s"></param>
      /// <returns></returns>
      public static string RemoveDiacritics(this string s)
      {
         string stFormD = s.Normalize(NormalizationForm.FormD);
         StringBuilder sb = new StringBuilder();

         for (int ich = 0; ich < stFormD.Length; ich++)
         {
            UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
            if (uc != UnicodeCategory.NonSpacingMark)
            {
               sb.Append(stFormD[ich]);
            }
         }

         return (sb.ToString().Normalize(NormalizationForm.FormC));
      }




      /// <summary>
      /// Make sure that a string has the ending web slash
      /// </summary>
      /// <param name="s"></param>
      /// <returns></returns>
      public static string EnsureEndingSlash(this string s)
      {
         if (!s.EndsWith("/", StringComparison.InvariantCultureIgnoreCase))
            return string.Concat(s, "/");

         return s;
      }



      /// <summary>
      /// Get the given text with the first char in upper case and the others in lower case
      /// </summary>
      /// <param name="s"></param>
      /// <returns></returns>
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
      public static string Capitalize(this string s)
      {
         return s = (!string.IsNullOrEmpty(s) && s.Length > 0) ? s.Substring(0, 1).ToUpperInvariant() + s.Substring(1).ToLowerInvariant() : s;
      }



      /// <summary>
      /// Lefts the specified s.
      /// </summary>
      /// <param name="s">The s.</param>
      /// <param name="length">The length.</param>
      /// <returns></returns>
      public static string Left(this string s, int length)
      {
         s.Validate();

         if (length < 0)
            throw new ArgumentOutOfRangeException("length");

         if ((length == 0) || string.IsNullOrEmpty(s))
            return string.Empty;

         if (length <= s.Length)
            return s.Substring(0, length);

         return s;
      }



      /// <summary>
      /// Rights the specified s.
      /// </summary>
      /// <param name="s">The s.</param>
      /// <param name="length">The length.</param>
      /// <returns></returns>
      public static string Right(this string s, int length)
      {
         s.Validate();

         if (length < 0)
            throw new ArgumentOutOfRangeException("length");

         if ((length == 0) || string.IsNullOrEmpty(s))
            return string.Empty;

         if (length <= s.Length)
            return s.Substring(s.Length - length, length);

         return s;
      }



      /// <summary>
      /// Validates the specified text.
      /// </summary>
      /// <param name="text">The text.</param>
      public static void Validate(this string text)
      {
         if (string.IsNullOrEmpty(text))
            throw new ArgumentNullException("text");
      }




      /// <summary>
      /// Get a text without a separator char ('/' or '/') at the end
      /// </summary>
      /// <param name="s"></param>
      /// <returns></returns>
      public static string WithoutEndSeparatorChar(this string s)
      {
         if (s.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.OrdinalIgnoreCase) ||
             s.EndsWith(Path.AltDirectorySeparatorChar.ToString(), StringComparison.OrdinalIgnoreCase))
            return s.Substring(0, s.Length - 1);
         else
            return s;
      }



      /// <summary>
      /// Enclose a string between "<code><![CDATA[]]></code>" tag
      /// </summary>
      /// <param name="source"></param>
      /// <returns></returns>
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "CDATA")]
      public static string EncloseWithCDATA(this string source)
      {
         return string.Concat("<![CDATA[", source, "]]>");
      }



      /// <summary>
      /// Quotes the specified string with single quotes
      /// </summary>
      /// <returns>A quoted string</returns>
      public static string Quote(this string stringToQuote)
      {
         return "\'" + stringToQuote + "\'";
      }



      /// <summary>
      /// Get the number of occurences of the substring into a string
      /// </summary>
      /// <param name="s"></param>
      /// <param name="substring"></param>
      /// <returns></returns>
      public static Int32 Occurs(this string s, string substring)
      {
         if (s == null || substring.Length == 0)
            return 0;
         Int32 occurs = 0;
         for (Int32 i = 0; i < s.Length - substring.Length + 1; i++)
         {
            if (s.Substring(i, substring.Length) == substring)
            {
               occurs++;
               i = i + substring.Length - 1;
            }
         }
         return occurs;
      }



      /// <summary>
      /// Truncate a given text to the given number of characters. 
      /// Also any embedded html is stripped.
      /// </summary>
      /// <param name="s"></param>
      /// <param name="numberOfCharacters"></param>
      /// <returns></returns>
      public static string Truncate(this string s, int numberOfCharacters)
      {
         return Text.TruncateText(s, numberOfCharacters);
      }

      /// <summary>
      /// Truncate a given text to the given number of characters. 
      /// Also any embedded html is stripped.
      /// </summary>
      /// <param name="s"></param>
      /// <param name="numberOfCharacters"></param>
      /// <param name="addEllipses"></param>
      /// <returns></returns>
      public static string Truncate(this string s, int numberOfCharacters, bool addEllipses)
      {
         return Text.TruncateText(s, numberOfCharacters, addEllipses);
      }



      /// <summary>
      /// Truncate a given text to the given number of characters. 
      /// Also any embedded html is stripped.
      /// </summary>
      /// <param name="s"></param>
      /// <param name="numberOfCharacters"></param>
      /// <param name="addEllipses"></param>
      /// <param name="ensureWord"></param>
      /// <returns></returns>
      public static string Truncate(this string s, int numberOfCharacters, bool addEllipses, bool ensureWord)
      {
         return Text.TruncateText(s, numberOfCharacters, addEllipses, ensureWord);
      }


      #endregion


      ///// <summary>
      ///// "Pulisce" una stringa sostituendo tutti i caratteri speciali html con le apposite
      ///// versioni che non diano problemi con jquery.Taconite
      ///// </summary>
      ///// <param name="source"></param>
      ///// <returns></returns>
      //public static string SafeTaconite(this string source)
      //{
      //   StringBuilder sb = new StringBuilder(source);
      //   IDictionary<string, string> specials = new Dictionary<string, string>
      //                                             {
      //                                                {"&nbsp;", "&#160;"},
      //                                                {" ", "&#160;"},
      //                                                {"&copy;", "&#169;"},
      //                                                //{"&", "&#38;"},
      //                                                {"&amp;", "&#38;"},
      //                                                {"@", "&#64;"},
      //                                                {"<", "&#60;"},
      //                                                {"&lt;", "&#60;"},
      //                                                {">", "&#62;"},
      //                                                {"&gt;", "&#62;"},
      //                                                {"'", "&#39;"},
      //                                                {"&quot;", "&#34;"},
      //                                                {"\"", @"&#34;"},
      //                                                {"&reg;", "&#174;"},
      //                                                {"®", "&#174;"},
      //                                                {"&trade;", "&#x2122;"},
      //                                                {"™", "&#x2122;"}
      //                                             };


      //   foreach (string special in specials.Keys)
      //   {
      //      sb.Replace(special, specials[special]);
      //   }

      //   string output = sb.ToString();
      //   IList<int> indexToChange = new List<int>();

      //   for (int i = 0; i < output.Length; i++)
      //   {
      //      if (output[i] == '&' && output[i + 1] != '#')
      //         indexToChange.Add(i);
      //   }

      //   // very special: sostituisce tutti i "&" che non siano codici speciali
      //   // ...da rivedere....genera una nuova stringa ad ogni ciclo.... :(
      //   foreach (int i in indexToChange)
      //   {
      //      output = output.Insert(i + 1, "#38;");
      //   }


      //   return output.ToString();
      //}


      /// <summary>
      /// Remove all html tags from a given string, but living the inner text
      /// </summary>
      /// <param name="text"></param>
      /// <returns></returns>
      public static string StripHtml(this string text)
      {
         return Text.StripHTML(text);
      }



      /// <summary>
      /// Count the number of words in a given string
      /// </summary>
      /// <param name="text"></param>
      /// <returns></returns>
      public static int WordCount(this string text)
      {
         return text.Split(new char[] { ' ', '.', ';', '?' }, StringSplitOptions.RemoveEmptyEntries).Length;
      }



      /// <summary>
      /// Returns the first 'N' words of a given string
      /// </summary>
      /// <param name="text"></param>
      /// <param name="wordCount"></param>
      /// <returns></returns>
      public static string GetFirstWords(this string text, int wordCount)
      {
         string[] words = text.Split(new char[] { ' ', '.', ';', '?' }, StringSplitOptions.RemoveEmptyEntries);
         return string.Join(" ", words, 0, words.Length < wordCount ? words.Length - 1 : wordCount - 1);
      }



      #region Collection Extensions

      /// <summary>
      /// Return true if the object exists in a given collection
      /// </summary>
      /// <param name="o"></param>
      /// <param name="c"></param>
      /// <example>if (product.In(productList))</example>
      /// <returns></returns>
      public static bool In(this Object o, IEnumerable c)
      {
         foreach (object i in c)
         {
            if (i.Equals(o))
               return true;
         }
         return false;
      }



      /// <summary>
      /// Each
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="values"></param>
      /// <returns></returns>
      /// <remarks>
      /// Credits:
      /// http://grabbagoft.blogspot.com/2007/12/ruby-style-array-methods-in-c-30.html
      /// </remarks>
      /// <example>
      /// <![CDATA[
      /// string[] myVitamins = { "b-12", "c", "riboflavin" };
      /// 
      /// myVitamins.Each().Do(
      ///     (vitamin) =>
      ///     {
      ///         Console.WriteLine("{0} is tasty", vitamin);
      ///     }
      /// );
      /// 
      /// var myOtherVitamins = new List<string>() { "b-12", "c", "riboflavin" };
      /// 
      /// myOtherVitamins.Each().Do(
      ///     (vitamin) =>
      ///     {
      ///         Console.WriteLine("{0} is very tasty", vitamin);
      ///     }
      /// );
      /// ]]>
      /// </example>
      public static EachIterator<T> Each<T>(this IEnumerable<T> values)
      {
         return new EachIterator<T>(values);
      }


      /// <summary>
      /// Used by the Each<T> extension method
      /// </summary>
      /// <typeparam name="T"></typeparam>
      public class EachIterator<T>
      {
         private readonly IEnumerable<T> values;

         internal EachIterator(IEnumerable<T> values)
         {
            this.values = values;
         }


         public void Do(Action<T> action)
         {
            if (values != null)
            {
               foreach (var item in values)
               {
                  action(item);
               }
            }
         }
      }



      /// <summary>
      /// EachWithIndex
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="values"></param>
      /// <returns></returns>
      /// <remarks>
      /// Credits:
      /// http://grabbagoft.blogspot.com/2007/12/ruby-style-array-methods-in-c-30.html
      /// </remarks>
      /// <example>
      /// string[] myVitamins = { "b-12", "c", "riboflavin" };
      /// 
      /// myVitamins.EachWithIndex().Do(
      ///     (vitamin, index) =>
      ///     {
      ///         Console.WriteLine("{0} cheers for {1}!", index, vitamin);
      ///     }
      /// );
      /// 
      /// var myOtherVitamins = new List<string>() { "b-12", "c", "riboflavin" };
      /// 
      /// myOtherVitamins.EachWithIndex().Do(
      ///     (vitamin, index) =>
      ///     {
      ///         Console.WriteLine("{0} cheers for {1}!", index, vitamin);
      ///     }
      /// );
      /// 
      /// This now outputs:
      /// 
      /// 0 cheers for b-12!
      /// 1 cheers for c!
      /// 2 cheers for riboflavin!
      /// 0 cheers for b-12!
      /// 1 cheers for c!
      /// 2 cheers for riboflavin!
      /// </example>
      public static EachWithIndexIterator<T> EachWithIndex<T>(this IEnumerable<T> values)
      {
         return new EachWithIndexIterator<T>(values);
      }



      public class EachWithIndexIterator<T>
      {
         private readonly IEnumerable<T> values;

         internal EachWithIndexIterator(IEnumerable<T> values)
         {
            this.values = values;
         }

         public void Do(Action<T, int> action)
         {
            int i = 0;
            foreach (var item in values)
            {
               action(item, i++);
            }
         }
      }


      #endregion

   }
}










































