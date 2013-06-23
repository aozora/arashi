using System.Collections.Generic;
using Arashi.Core.Domain;

namespace Arashi.Web.Mvc.Models
{
   public class ControlPanelModel
   {
      /// <summary>
      /// This is a simple counter
      /// </summary>
      public int CategoryId {get;set;}

      public string Category {get;set;}

      public IList<ControlPanelItem> Items {get;set;}

      public ControlPanelModel()
      {
         Items = new List<ControlPanelItem>();
      }

   }
}