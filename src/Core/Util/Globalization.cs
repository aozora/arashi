using System;
using System.Linq;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;

namespace Arashi.Core.Util
{
   /// <summary>
   /// Utilty class for Globalization stuff.
   /// </summary>
   public class Globalization
   {
#if !SILVERLIGHT

      /// <summary>
      /// Get a sortedlist of all installed cultures, ordered by display name.
      /// </summary>
      /// <returns></returns>
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
      public static IEnumerable<CultureInfo> GetOrderedCultures()
      {
         IEnumerable<CultureInfo> orderedCultures = from c in CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                                                    orderby c.DisplayName
                                                    select c;
         return orderedCultures;
      }
#endif


      /// <summary>
      /// Get the description of a language from a culture.
      /// </summary>
      /// <param name="culture"></param>
      /// <returns></returns>
      public static string GetNativeLanguageTextFromCulture(string culture)
      {
         CultureInfo ci = new CultureInfo(culture);
         string languageAsText = ci.NativeName.Substring(0, ci.NativeName.IndexOf("(") - 1);

         return languageAsText;
      }



      /// <summary>
      /// Get the country part from the culture string.
      /// </summary>
      /// <param name="culture"></param>
      /// <returns></returns>
      public static string GetCountryFromCulture(string culture)
      {
         return culture.Substring(3);
      }

      /// <summary>
      /// Get the two-letter ISO language name of the given culture.
      /// </summary>
      /// <param name="culture"></param>
      /// <returns></returns>
      public static string GetLanguageFromCulture(string culture)
      {
         CultureInfo ci = new CultureInfo(culture);
         return ci.TwoLetterISOLanguageName;
      }
   }
}