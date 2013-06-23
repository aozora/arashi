using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Arashi.Core.Util
{
   public class Serialization
   {
      #region Serialization Utils

      /// <summary>
      /// Method to convert a custom Object to XML string
      /// </summary>
      /// <param name="objectToSerialize">Object that is to be serialized to XML</param>
      /// <returns>XML string</returns>
      public static String SerializeObject(Object objectToSerialize)
      {
         try
         {
            String xmlizedString = null;
            MemoryStream memoryStream = new MemoryStream();
            XmlSerializer xs = new XmlSerializer(objectToSerialize.GetType());
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);

            xs.Serialize(xmlTextWriter, objectToSerialize);

            memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
            xmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());

            return xmlizedString;
         }
         catch (Exception ex)
         {
            Trace.WriteLine(ex.ToString());
            return null;
         }
      }



      /// <summary>
      /// To convert a Byte Array of Unicode values (UTF-8 encoded) to a complete String.
      /// </summary>
      /// <param name="characters">Unicode Byte Array to be converted to String</param>
      /// <returns>String converted from Unicode Byte Array</returns>
      private static String UTF8ByteArrayToString(Byte[] characters)
      {
         UTF8Encoding encoding = new UTF8Encoding();
         String constructedString = encoding.GetString(characters);
         return (constructedString);
      }



      /// <summary>
      /// Converts the String to UTF8 Byte array and is used in De serialization
      /// </summary>
      /// <param name="xmlString"></param>
      /// <returns></returns>
      private static Byte[] StringToUTF8ByteArray(String xmlString)
      {
         UTF8Encoding encoding = new UTF8Encoding();
         Byte[] byteArray = encoding.GetBytes(xmlString);
         return byteArray;
      }

      #endregion

   }
}
