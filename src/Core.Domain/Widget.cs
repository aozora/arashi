using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arashi.Core.Domain
{
   /// <summary>
   /// This is an instance of a activated widget for a specific Site.
   /// </summary>
   public class Widget
   {
      public virtual int WidgetId {get; set;}
      public virtual WidgetType Type {get; set;}
      public virtual Site Site {get; set;}
      public virtual string Title {get; set;}
      public virtual WidgetPlaceHolder PlaceHolder {get; set;}
      public virtual int Position {get; set;}
      public virtual IDictionary<String, String> Settings {get; set;}


      public Widget()
      {
         WidgetId = -1;
         Position = 0;
         Settings = new Dictionary<String, String>();
      }


      #region Equality

      public override bool Equals(object other)
      {
         if (this == other)
            return true;
         Widget w = other as Widget;
         if (w == null)
            return false;
         if (WidgetId != w.WidgetId)
            return false;
         return true;
      }



      public override int GetHashCode()
      {
         unchecked
         {
            int result = WidgetId.GetHashCode();
            result = 29 * result;
            return result;
         }
      }

      #endregion

   }
}
