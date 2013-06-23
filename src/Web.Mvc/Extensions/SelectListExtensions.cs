namespace Arashi.Web.Mvc.Extensions
{
   using System;
   using System.Linq;
   using System.Web.Mvc;


   
   public static class SelectListExtensions
   {
      /// <summary>
      /// Convert an enumeration to a <see>System.Web.Mvc.SelectList</see>
      /// The SelectedValue is also set.
      /// </summary>
      /// <typeparam name="TEnum"></typeparam>
      /// <param name="enumObj"></param>
      /// <returns></returns>
      public static SelectList ToSelectList<TEnum>(this TEnum enumObj)
      {
         var values = from TEnum e in Enum.GetValues(typeof(TEnum))
                      select new
                      {
                         Id = e,
                         Name = e.ToString()
                      };

         return new SelectList(values, "Id", "Name", enumObj);
      }


   }
}
