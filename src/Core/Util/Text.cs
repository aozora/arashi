using System;
using System.Text.RegularExpressions;

namespace Arashi.Core.Util
{
   /// <summary>
   /// The Text class contains helper methods for text manipulation.
   /// </summary>
   public sealed class Text
   {
      private Text()
      {
      }


      /// <summary>
      /// Truncate a given text to the given number of characters. 
      /// Also any embedded html is stripped.
      /// </summary>
      /// <param name="fullText"></param>
      /// <param name="numberOfCharacters"></param>
      /// <returns></returns>
      public static string TruncateText(string fullText, int numberOfCharacters)
      {
         return TruncateText(fullText, numberOfCharacters, false);
      }

      public static string TruncateText(string fullText, int numberOfCharacters, bool addEllipses)
      {
         return TruncateText(fullText, numberOfCharacters, addEllipses, false);
      }


      /// <summary>
      /// Truncate a given text to the given number of characters. 
      /// Also any embedded html is stripped.
      /// </summary>
      /// <param name="fullText"></param>
      /// <param name="numberOfCharacters"></param>
      /// <param name="addEllipses">add ellipses to the end</param>
      /// <param name="ensureWord">ensure that the last word is keep safe</param>
      /// <returns></returns>
      public static string TruncateText(string fullText, int numberOfCharacters, bool addEllipses, bool ensureWord)
      {
         string text;
         if (fullText.Length > numberOfCharacters)
         {
            int spacePos;
            // se non devo troncare l'ultima parola
            if (ensureWord)
               spacePos = fullText.IndexOf(" ", numberOfCharacters, StringComparison.OrdinalIgnoreCase);
            else
               spacePos = numberOfCharacters;

            if (spacePos > -1)
            {
               text = fullText.Substring(0, spacePos);
               if (addEllipses)
                  text += "...";
            }
            else
            {
               text = fullText;
            }
         }
         else
         {
            text = fullText;
         }
         Regex regexStripHTML = new Regex("<[^>]+>", RegexOptions.IgnoreCase | RegexOptions.Compiled);
         text = regexStripHTML.Replace(text, " ");
         return text;
      }




      /// <summary>
      /// Ensure that the given string has a trailing slash.
      /// </summary>
      /// <param name="stringThatNeedsTrailingSlash"></param>
      /// <returns></returns>
      public static string EnsureTrailingSlash(string stringThatNeedsTrailingSlash)
      {
         if (!stringThatNeedsTrailingSlash.EndsWith("/", StringComparison.OrdinalIgnoreCase))
         {
            return stringThatNeedsTrailingSlash + "/";
         }
         else
         {
            return stringThatNeedsTrailingSlash;
         }
      }



      /// <summary>
      /// Remove all html tags from a given string, but living the inner text
      /// </summary>
      /// <param name="htmlString"></param>
      /// <returns></returns>
      public static string StripHTML(string htmlString)
      {
         //This pattern Matches everything found inside html tags;
         //(.|\n) - > Look for any character or a new line
         // *?  -> 0 or more occurences, and make a non-greedy search meaning
         //That the match will stop at the first available '>' it sees, and not at the last one
         //(if it stopped at the last one we could have overlooked
         //nested HTML tags inside a bigger HTML tag..)
         // Thanks to Oisin and Hugh Brown for helping on this one...
         string pattern = @"<(.|\n)*?>";

         return Regex.Replace(htmlString, pattern, string.Empty);
      }

   }
}