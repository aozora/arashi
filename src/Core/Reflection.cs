using System.Reflection;

namespace Arashi.Core
{
   /// <summary>
   /// Reflection - Utility for dynamic method invocation
   /// </summary>
   public class Reflection
   {
      /// <summary>
      /// Use reflection to dynamically invoke a method
      /// </summary>
      /// <param name="instance"></param>
      /// <param name="methodName"></param>
      /// <param name="parameters">'null' for no parameter for the function, or we can pass the array of parameters</param>
      public static void InvokeMethod(object instance, string methodName, object[] parameters)
      {
         //Getting the method information using the method info class
         MethodInfo mi = instance.GetType().GetMethod(methodName);

         //invoking the method
         //null- no parameter for the function [or] we can pass the array of parameters
         mi.Invoke(instance, parameters);
      }



   }
}