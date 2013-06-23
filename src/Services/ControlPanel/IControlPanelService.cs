using System;
using System.Collections.Generic;
using Arashi.Core.Domain;

namespace Arashi.Services.ControlPanel
{
   public interface IControlPanelService
   {



      /// <summary>
      /// Salva l'item
      /// </summary>
      /// <param name="controlPanelItem">controlPanelItem da salvare</param>
      void SaveControlPanelItem(ControlPanelItem controlPanelItem);


      /// <summary>
      /// Elimina l'item
      /// </summary>
      /// <param name="controlPanelItem">controlPanelItem da eliminare</param>
      void DeleteControlPanelItem(ControlPanelItem controlPanelItem);


      /// <summary>
      /// Ritorna l'item identificato da controlPanelItemId
      /// </summary>
      /// <param name="controlPanelItemId">Id ricercato</param>
      /// <returns>Item del pannello di controllo</returns>
      ControlPanelItem GetControlPanelItemById(Int32 controlPanelItemId);



      
      ///// <summary>
      ///// Ritorna l'item identificato da controlPanelItem
      ///// </summary>
      ///// <param name="controlPanelItem">controlPanelItem ricercato</param>
      ///// <param name="role">role ricercato</param>
      ///// <returns>ItemRole del pannello di controllo</returns>
      //IList<ControlPanelItemRole> GetControlPanelItemRole(ControlPanelItem controlPanelItem, Role role);



      
      ///// <summary>
      ///// Salva l'item identificato da controlPanelItemRole
      ///// </summary>
      ///// <param name="controlPanelItemRole">controlPanelItemRole da salvare</param>
      //void SaveControlPanelItemRole(ControlPanelItemRole controlPanelItemRole);



      
      ///// <summary>
      ///// Elimina l'item identificato da controlPanelItemRole
      ///// </summary>
      ///// <param name="controlPanelItemRole">controlPanelItemRole da eliminare</param>
      //void DeleteControlPanelItemRole(ControlPanelItemRole controlPanelItemRole);


      IList<ControlPanelItem> GetControlPanelItems();


      /// <summary>
      /// Ritorna i controlPanelItems del servizio in funzione dei ruoli dell'utente
      /// </summary>
      /// <param name="service">Servizio di cui si vogliono ottenere gli item del pannello di controllo</param>
      /// <param name="userRoles">Ruoli dell'utente corrente</param>
      /// <returns>Lista degli item del pannello di controllo</returns>
      //IList<ControlPanelItem> GetControlPanelItemsByServiceAndRoles(Service service, IList<Role> userRoles);



   }
}