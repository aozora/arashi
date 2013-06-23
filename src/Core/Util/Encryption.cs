using System;
using System.Text;
using System.Security.Cryptography;

namespace Arashi.Core.Util
{
   /// <summary>
   /// Helper class that contains static methods for data encryption.
   /// </summary>
   public sealed class Encryption
   {
      private Encryption()
      {
      }

      #region SHA Encryption (http://dotnetrush.blogspot.com/2007/04/c-sha-2-cryptography-sha-256-sha-384.html)

      /// <summary>
      /// Convert a string in a SHA2 hash.
      /// </summary>
      /// <param name="phrase"></param>
      /// <returns></returns>
      public static string SHA256Encrypt(string phrase)
      {
         UTF8Encoding encoder = new UTF8Encoding();
         SHA256Managed sha256hasher = new SHA256Managed();
         byte[] hashedDataBytes = sha256hasher.ComputeHash(encoder.GetBytes(phrase));
         return ByteArrayToString(hashedDataBytes);
      }



      public static string SHA384Encrypt(string phrase)
      {
         UTF8Encoding encoder = new UTF8Encoding();
         SHA384Managed sha384hasher = new SHA384Managed();
         byte[] hashedDataBytes = sha384hasher.ComputeHash(encoder.GetBytes(phrase));
         return ByteArrayToString(hashedDataBytes);
      }



      public static string SHA512Encrypt(string phrase)
      {
         UTF8Encoding encoder = new UTF8Encoding();
         SHA512Managed sha512hasher = new SHA512Managed();
         byte[] hashedDataBytes = sha512hasher.ComputeHash(encoder.GetBytes(phrase));
         return ByteArrayToString(hashedDataBytes);
      }



      public static string ByteArrayToString(byte[] inputArray)
      {
         StringBuilder output = new StringBuilder("");
         for (int i = 0; i < inputArray.Length; i++)
         {
            output.Append(inputArray[i].ToString("X2"));
         }
         return output.ToString();
      }

      #endregion


      /// <summary>
      /// Calculates the MD5 of a given string.
      /// </summary>
      /// <param name="inputString"></param>
      /// <returns>The (hexadecimal) string representatation of the MD5 hash.</returns>
      /// <remarks>
      /// Not raccomanded for sensitive info
      /// </remarks>
      public static string StringToMD5Hash(string inputString)
      {
         byte[] encryptedBytes;

         using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
         {
            encryptedBytes = md5.ComputeHash(Encoding.ASCII.GetBytes(inputString));
         }

         StringBuilder sb = new StringBuilder();

         for (int i = 0; i < encryptedBytes.Length; i++)
         {
            sb.AppendFormat("{0:x2}", encryptedBytes[i]);
         }

         return sb.ToString();
      }

   }
}