#region license
// Copyright (c) 2005 - 2007 Ayende Rahien (ayende@ayende.com)
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
// 
//     * Redistributions of source code must retain the above copyright notice,
//     this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright notice,
//     this list of conditions and the following disclaimer in the documentation
//     and/or other materials provided with the distribution.
//     * Neither the name of Ayende Rahien nor the names of its
//     contributors may be used to endorse or promote products derived from this
//     software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE
// FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
// CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
// THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
#endregion

using System;
using System.Text.RegularExpressions;

namespace Arashi.Core.Util
{
   public static class Validation
   {

      /// <summary>
      ///   Verifica la validità di un url
      /// </summary>
      /// <param name="stringUrl">Stringa url, required=true</param>
      /// <returns></returns>
      public static Boolean IsUrl(String stringUrl)
      {
         return IsUrl(stringUrl, true);
      }



      public static Boolean IsUrl(String stringUrl, Boolean required)
      {
         if (!required && String.IsNullOrEmpty(stringUrl))
            return true;

         String strRegex = "^(https?://)"
                           + "?(([0-9a-z_!~*'().&=+$%-]+: )?[0-9a-z_!~*'().&=+$%-]+@)?" //user@ 
                           + @"(([0-9]{1,3}\.){3}[0-9]{1,3}" // IP- 199.194.52.184 
                           + "|" // allows either IP or domain 
                           + @"([0-9a-z_!~*'()-]+\.)*" // tertiary domain(s)- www. 
                           + @"([0-9a-z][0-9a-z-]{0,61})?[0-9a-z]\." // second level domain 
                           + "[a-z]{2,6})" // first level domain- .com or .museum 
                           + "(:[0-9]{1,4})?" // port number- :80 
                           + "((/?)|" // a slash isn't required if there is no file name 
                           + "(/[0-9a-z_!~*'().;?:@&=+$,%#-]+)+/?)$";
         Regex regex = new Regex(strRegex);
         return regex.IsMatch(stringUrl);
      }



      /// <summary>
      ///   Verifica la validità di un indirizzo email
      /// </summary>
      /// <param name="stringEmail">Stringa email, required=true</param>
      /// <returns></returns>
      public static Boolean IsEmail(String stringEmail)
      {

         String strRegex = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

         Regex regex = new Regex(strRegex);
         return regex.IsMatch(stringEmail);
      }



      /// <summary>
      ///   Controlla se la stringa contiene una data valida
      /// </summary>
      /// <param name="stringDate">Stringa data, required=true</param>
      /// <returns></returns>
      public static Boolean IsDate(String stringDate)
      {
         DateTime dateTimeOutput;
         return (DateTime.TryParse(stringDate, out dateTimeOutput) && stringDate.Length == 10);
      }



      /// <summary>
      ///   Controlla se la stringa contiene un numero valido
      /// </summary>
      /// <param name="stringNumeric">Stringa numerica, required=true</param>
      /// <returns></returns>
      public static Boolean IsNumeric(String stringNumeric)
      {
         int integerOutput;
         return (Int32.TryParse(stringNumeric, out integerOutput));
      }



      /// <summary>
      ///   Controlla se la stringa contiene un numero intero ow valido (cioè con separatori delle migliaia)
      /// </summary>
      /// <param name="stringIntegerNumber">Stringa numero intero, required=true, thousandSeparator='.'</param>
      /// <returns></returns>
      public static Boolean IsInteger(String stringIntegerNumber)
      {
         int integerOutput;
         return (Int32.TryParse(stringIntegerNumber, out integerOutput));
      }


      /// <summary>
      ///   Controlla se la stringa contiene un numero reale (double) ow valido (cioè con separatori delle migliaia e virgola decimale)
      /// </summary>
      /// <param name="stringDoubleNumber">Stringa numero reale, required=true, thousandSeparator='.', decimalPoint=','</param>
      /// <returns></returns>
      public static Boolean IsDouble(String stringDoubleNumber)
      {
         double doubleOutput;
         return (double.TryParse(stringDoubleNumber, out doubleOutput));
      }




      /// <summary>
      ///   Controlla se la stringa contiene un numero decimale valido
      /// </summary>
      /// <param name="stringDecimal">Stringa decimale, required=true</param>
      /// <returns></returns>
      public static Boolean IsDecimal(String stringDecimal)
      {
         Decimal decimalOutput;
         return (Decimal.TryParse(stringDecimal, out decimalOutput));
      }



      public static void DateRange(DateTime start, DateTime end)
      {
         if (start > end)
         {
            throw new ArgumentException("The start date cannot come after the end date");
         }
      }



      //public static void NotNullOrEmpty(String str, String name)
      //{
      //   if (String.IsNullOrEmpty(str))
      //      throw new ArgumentException("{0} must have a value", name);
      //}



      //public static void NotNull(object obj, String paramName)
      //{
      //   if (obj == null)
      //      throw new ArgumentNullException(paramName);
      //}



      public static void InRange(IComparable start, IComparable end, IComparable obj, String paramName)
      {
         if (start.CompareTo(obj) > 0 || end.CompareTo(obj) < 0)
            throw new ArgumentOutOfRangeException(paramName);
      }



      public static void InDateRange(DateTime innerStart, DateTime innerEnd,
                                     DateTime outerStart, DateTime outerEnd)
      {
         if (innerStart < outerStart || innerStart > outerEnd ||
             innerEnd > outerEnd || innerEnd < outerStart)
            throw
               new ArgumentOutOfRangeException(
                  String.Format("Date Ranges do not overlap, {0}-{1} does not contain {2}-{3}",
                                outerStart.ToShortDateString(), outerEnd.ToShortDateString(),
                                innerStart.ToShortDateString(), innerEnd.ToShortDateString()));
      }



      //public static void PositiveNumber(Int32 number, String name)
      //{
      //   if (number < 0)
      //      throw new ArgumentException(String.Format("{1} should be positive, but was {0}", number, name));
      //}

   }
}