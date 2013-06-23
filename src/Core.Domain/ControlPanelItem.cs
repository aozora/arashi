using System.Collections;
using System.Collections.Generic;
namespace Arashi.Core.Domain
{
   public class ControlPanelItem
   {
      #region Private Fields

      private int controlPanelItemId;

      #endregion

      #region Public Properties

      public virtual int ControlPanelItemId
      {
         get
         {
            return controlPanelItemId;
         }
         set
         {
            controlPanelItemId = value;
         }
      }

      public virtual int ViewOrder { get; set; }

      public virtual string Category {get; set;}

      public virtual string Text { get; set; }

      public virtual string Description { get; set; }

      public virtual string ImageSrc { get; set; }

      public virtual string LittleImageSrc { get; set; }

      public virtual string ImageAlt { get; set; }

      public virtual string Controller { get; set; }
      public virtual string Action { get; set; }

      public virtual string Parameters { get; set; }


      #endregion


      public ControlPanelItem()
      {
         controlPanelItemId = -1;
      }


      #region Equality

      public override bool Equals(object other)
      {
         if (this == other)
            return true;
         ControlPanelItem cpi = other as ControlPanelItem;
         if (cpi == null)
            return false;
         if (ControlPanelItemId != cpi.ControlPanelItemId)
            return false;
         return true;
      }



      public override int GetHashCode()
      {
         unchecked
         {
            int result;
            result = ControlPanelItemId.GetHashCode();
            result = 29 * result;
            return result;
         }
      }

      #endregion

   }
}
