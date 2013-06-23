using System;
using System.Text;

namespace Arashi.Core.Extensions
{
   public static class Base64Extensions
   {
      public static string EncodeToBase64(this string input)
      {
         byte[] bytes = new byte[input.Length];
         bytes = Encoding.UTF8.GetBytes(input);

         return Convert.ToBase64String(bytes);
      }



      public static string DecodeFromBase64(this string input)
      {
         Decoder decoder = new UTF8Encoding().GetDecoder();

         byte[] bytes = Convert.FromBase64String(input);
         int charCount = decoder.GetCharCount(bytes, 0, bytes.Length);

         char[] chars = new char[charCount];
         decoder.GetChars(bytes, 0, bytes.Length, chars, 0);

         return new String(chars);
      }
   }
}