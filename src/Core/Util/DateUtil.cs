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
using System.Collections.Generic;
using System.Globalization;

namespace Arashi.Core.Util
{
   public static class DateUtil
   {
      /// <summary>
      /// Converte una stringa in una data; se la stringa è null o invalida restituisce null
      /// </summary>
      /// <param name="dateString"></param>
      /// <returns></returns>
      public static DateTime? Convert(string dateString)
      {
         if (string.IsNullOrEmpty(dateString))
            return null;

         DateTime d;
         DateTime.TryParse(dateString, out d);
         return d;
      }



      /// <summary>
      /// Combine e date with time info into a unique DateTime object
      /// </summary>
      /// <param name="datePart"></param>
      /// <param name="timePart"></param>
      /// <returns></returns>
      public static DateTime Combine(DateTime datePart, DateTime timePart)
      {
         return new DateTime(datePart.Year, datePart.Month, datePart.Day,
                             timePart.Hour, timePart.Minute, timePart.Second, timePart.Millisecond);
      }


      /// <summary>
      /// Get the datetime corresponding to the first day of a month
      /// </summary>
      /// <param name="date"></param>
      /// <returns></returns>
      public static DateTime StartMonth(DateTime date)
      {
         return new DateTime(date.Year, date.Month, 1);
      }



      /// <summary>
      /// Get the datetime corresponding to the last day of a month
      /// </summary>
      /// <param name="date"></param>
      /// <returns></returns>
      public static DateTime EndMonth(DateTime date)
      {
         return StartMonth(date).AddMonths(1).AddDays(-1);
      }


      /// <summary>
      /// Get the datetime corresponding to the next day of a week
      /// </summary>
      /// <param name="dayOfWeek"></param>
      /// <returns></returns>
      public static DateTime Next(DayOfWeek dayOfWeek)
      {
         if (dayOfWeek <= DateTime.Today.DayOfWeek)
         {
            return DateTime.Today.Date.AddDays((dayOfWeek - DateTime.Today.DayOfWeek) + 7);
         }

         return DateTime.Today.Date.AddDays((dayOfWeek - DateTime.Today.DayOfWeek));
      }



      /// <summary>
      /// Get the datetime corresponding to the previous day of a week
      /// </summary>
      /// <param name="dayOfWeek"></param>
      /// <returns></returns>
      public static DateTime Previous(DayOfWeek dayOfWeek)
      {
         if (dayOfWeek >= DateTime.Today.DayOfWeek)
         {
            return DateTime.Today.Date.AddDays((dayOfWeek - DateTime.Today.DayOfWeek) - 7);
         }

         return DateTime.Today.Date.AddDays((dayOfWeek - DateTime.Today.DayOfWeek));
      }


      /// <summary>
      /// Get the datetime corresponding to the first day of a week
      /// </summary>
      /// <param name="time"></param>
      /// <returns></returns>
      public static DateTime StartWeek(DateTime time)
      {
         return time.Date.AddDays(DayOfWeek.Sunday - time.DayOfWeek);
      }



      /// <summary>
      /// Get the datetime corresponding to the last day of a week
      /// </summary>
      /// <param name="time"></param>
      /// <returns></returns>
      public static DateTime EndWeek(DateTime time)
      {
         return time.Date.AddDays(DayOfWeek.Saturday - time.DayOfWeek);
      }



      public static IDictionary<int, String> MonthNames(CultureInfo cultureInfo, bool abbreviatedNames)
      {
         IDictionary<int, String> list = new Dictionary<int, string>();

         DateTime start = new DateTime(DateTime.Now.Year, 1, 1);

         for (int index = 0; index < 12; index++)
         {
            if (abbreviatedNames)
               list.Add(index + 1, cultureInfo.DateTimeFormat.GetAbbreviatedMonthName(start.AddMonths(index).Month));
            else
               list.Add(index + 1, cultureInfo.DateTimeFormat.GetMonthName(start.AddMonths(index).Month));
         }

         return list;
      }

   }
}