using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arashi.Core.Domain
{
   /// <summary>
   /// This class represent an registered widget type
   /// </summary>
   public class WidgetType
   {
      public virtual int WidgetTypeId {get; set;}
      public virtual string Name {get; set;}
      public virtual string AssemblyName {get; set;}
      public virtual string ClassName {get; set;}
      public virtual IDictionary<String, String> DefaultSettings {get; set;}


      public WidgetType()
      {
         WidgetTypeId = -1;
         DefaultSettings = new Dictionary<String, String>();
      }


      #region Equality

      public override bool Equals(object other)
      {
         if (this == other)
            return true;
         WidgetType w = other as WidgetType;
         if (w == null)
            return false;
         if (WidgetTypeId != w.WidgetTypeId)
            return false;
         return true;
      }



      public override int GetHashCode()
      {
         unchecked
         {
            int result = WidgetTypeId.GetHashCode();
            result = 29 * result;
            return result;
         }
      }

      #endregion


   }
}
