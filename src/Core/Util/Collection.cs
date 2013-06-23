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
using System.ComponentModel;
using System.Collections;
using System.Runtime.Serialization;

namespace Arashi.Core.Util
{
   [Serializable]
   public class ElementNotfoundException : Exception
   {
      public ElementNotfoundException()
      {
      }

      public ElementNotfoundException(string message)
         : base(message)
      {
      }

      public ElementNotfoundException(String message, Exception exception)
         : base(message, exception)
      {
      }


      public ElementNotfoundException(SerializationInfo info, StreamingContext context)
         :base( info, context)
      {
      }

   }



   public static class Collection
   {

      /// <summary>
      /// Get the first T type in the collection
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="collection"></param>
      /// <returns></returns>
      public static T First<T>(ICollection<T> collection)
      {
         IList<T> list = collection as IList<T>;
         if (list != null)
            return list[0];
         foreach (T item in collection)
         {
            return item;
         }
         throw new ElementNotfoundException();
      }



      /// <summary>
      /// Get the last T type in the collection
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="collection"></param>
      /// <returns></returns>
      public static T Last<T>(ICollection<T> collection)
      {
         IList<T> list = collection as IList<T>;
         if (list != null)
            return list[list.Count - 1];
         T last = default(T);
         bool set = false;
         foreach (T item in collection)
         {
            last = item;
            set = true;
         }
         if (set)
            return last;
         throw new ElementNotfoundException();
      }



      public static ICollection<T> SelectAll<T>(ICollection<T> collection, Predicate<T> predicate)
      {
         return SelectInternal(true, collection, predicate);
      }



      public static T Find<T>(ICollection<T> items, Predicate<T> pred)
      {
         foreach (T item in items)
         {
            if (pred(item))
               return item;
         }
         return default(T);
      }



      private static ICollection<T> SelectInternal<T>(bool addIfTrue, ICollection<T> collection, Predicate<T> predicate)
      {
         ICollection<T> results = new List<T>();
         foreach (T item in collection)
         {
            if (predicate(item))
            {
               if (addIfTrue)
                  results.Add(item);
            }
            else if (addIfTrue == false)
            {
               results.Add(item);
            }
         }
         return results;
      }



      public static ICollection<T> SelectAllNot<T>(ICollection<T> collection, Predicate<T> predicate)
      {
         return SelectInternal(false, collection, predicate);
      }



      public static void ForEach<T>(ICollection<T> collection, Action<T> action)
      {
         foreach (T item in collection)
         {
            action(item);
         }
      }


      /// <summary>
      /// Convert the collection to a generic T array
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="list"></param>
      /// <returns></returns>
      public static T[] ToArray<T>(object list)
      {
         return ToArray<T>((IList)list);
      }



      /// <summary>
      /// Convert the IList to a generic T array
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="list"></param>
      /// <returns></returns>
      public static T[] ToArray<T>(IList list)
      {
         T[] arr = new T[list.Count];
         list.CopyTo(arr, 0);
         return arr;
      }



      public static BindingList<T> ToBindingList<T>(IList list)
      {
         return new BindingList<T>(ToArray<T>(list));
      }



      public static IDictionary<T, ICollection<K>> GroupBy<K, T>(ICollection<K> collection, Converter<K, T> converter)
      {
         Dictionary<T, ICollection<K>> dic = new Dictionary<T, ICollection<K>>();
         foreach (K k in collection)
         {
            T key = converter(k);
            if (dic.ContainsKey(key) == false)
            {
               dic[key] = new List<K>();
            }
            dic[key].Add(k);
         }
         return dic;
      }



      public static object Single(ICollection collection)
      {
         if (collection.Count == 0)
            return null;
         if (collection.Count > 1)
            throw new InvalidOperationException("Collection does not have exactly one item");
         IEnumerator enumerator = collection.GetEnumerator();
         enumerator.MoveNext();
         return enumerator.Current;
      }


      public static ICollection<T> ToUniqueCollection<T>(ICollection<T> collection)
      {
         List<T> result = new List<T>(collection.Count);
         foreach (T item in collection)
         {
            if (!result.Contains(item))
               result.Add(item);
         }
         return result;
      }



      /// <summary>
      /// Obtains the entry and remove it if found.
      /// </summary>
      /// <param name="attributes">The attributes.</param>
      /// <param name="key">The key.</param>
      /// <param name="defaultValue">The default value.</param>
      /// <returns>the entry value or the default value</returns>
      public static string ObtainEntryAndRemove(IDictionary attributes, string key, string defaultValue)
      {
         string value = ObtainEntryAndRemove(attributes, key);

         return value ?? defaultValue;
      }



      /// <summary>
      /// Obtains the entry and remove it if found.
      /// </summary>
      /// <param name="attributes">The attributes.</param>
      /// <param name="key">The key.</param>
      /// <returns>the entry value or null</returns>
      public static string ObtainEntryAndRemove(IDictionary attributes, string key)
      {
         string value = null;

         if (attributes != null && attributes.Contains(key))
         {
            value = (String)attributes[key];

            attributes.Remove(key);
         }

         return value;
      }


   }
}