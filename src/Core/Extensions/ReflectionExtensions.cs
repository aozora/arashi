using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Arashi.Core.Extensions
{
   public static class ReflectionExtensions
   {
      /// <summary>
      /// 	Gets the property value.
      /// </summary>
      /// <typeparam name = "T"></typeparam>
      /// <param name = "obj">The obj.</param>
      /// <param name = "propertyName">Name of the property.</param>
      /// <returns></returns>
      public static T GetPropertyValue<T>(this Object obj, String propertyName)
      {
         PropertyInfo pi = obj.GetType().GetProperty(propertyName,
                                                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
         return (T)pi.GetValue(obj, new object[]
			                                 	{
			                                 	});
      }

   }
}
