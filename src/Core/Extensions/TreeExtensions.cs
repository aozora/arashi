namespace Arashi.Core.Extensions
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Text;

   /// <summary>
   /// For details see http://www.claassen.net/geek/blog/2009/06/searching-tree-of-objects-with-linq.html
   /// </summary>
   public static class TreeExtensions
   {
      /// <summary>
      /// The algorithm will dig continue to dig down a nodes children until it reaches a leaf node (a node without children), 
      /// before considering the next child of the current parent node
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="head"></param>
      /// <param name="childrenFunc"></param>
      /// <returns></returns>
      public static IEnumerable<T> AsDepthFirstEnumerable<T>(this T head, Func<T, IEnumerable<T>> childrenFunc)
      {
         yield return head;
         foreach (var node in childrenFunc(head))
         {
            foreach (var child in AsDepthFirstEnumerable(node, childrenFunc))
            {
               yield return child;
            }
         }
      }



      /// <summary>
      /// The algorithm will return all nodes at a particular depth first before considering the children at the next level. 
      /// I.e. First return all the nodes from level 1, then all nodes from level 2, etc.
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="head"></param>
      /// <param name="childrenFunc"></param>
      /// <returns></returns>
      public static IEnumerable<T> AsBreadthFirstEnumerable<T>(this T head, Func<T, IEnumerable<T>> childrenFunc)
      {
         yield return head;
         var last = head;
         foreach (var node in AsBreadthFirstEnumerable(head, childrenFunc))
         {
            foreach (var child in childrenFunc(node))
            {
               yield return child;
               last = child;
            }
            if (last.Equals(node))
               yield break;
         }
      }

   }
}
