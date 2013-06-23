using System;

namespace Arashi.Services.ControlPanel
{
   using System.Collections.Generic;
   using Arashi.Core.Domain;
   using Arashi.Core.Repositories;
   using Common.Logging;


   public class ControlPanelService : ServiceBase, IControlPanelService
   {
      public ControlPanelService(Arashi.Core.NHibernate.ISessionFactory sessionFactory, ILog log)
         : base(sessionFactory, log)
      {
      }

      #region IControlPanelService Members

      public void SaveControlPanelItem(ControlPanelItem controlPanelItem)
      {
         //using (NHTransactionScope tx = new NHTransactionScope())
         //{
         Repository<ControlPanelItem>.Save(controlPanelItem);
         //   tx.VoteCommit();
         //}
      }



      public void DeleteControlPanelItem(ControlPanelItem controlPanelItem)
      {
         //using (NHTransactionScope tx = new NHTransactionScope())
         //{

            Repository<ControlPanelItem>.Delete(controlPanelItem);

            //foreach (ControlPanelItemRole controlPanelItemRole in controlPanelItem.ControlPanelItemRoles)
            //   DeleteControlPanelItemRole(controlPanelItemRole);

            //controlPanelItem.ControlPanelItemRoles.Clear();
         //   tx.VoteCommit();
         //}

      }



      public ControlPanelItem GetControlPanelItemById(Int32 controlPanelItemId)
      {
         return Repository<ControlPanelItem>.FindById(controlPanelItemId);
      }



      //public IList<ControlPanelItemRole> GetControlPanelItemRole(ControlPanelItem controlPanelItem, Role role)
      //{
      //   String hql = "from ControlPanelItemRole controlPanelItemRole where controlPanelItemRole.ControlPanelItem = :controlPanelItem and controlPanelItemRole.Role = :role ";
      //   IQuery query = Session.CreateQuery(hql);
      //   query.SetEntity("controlPanelItem", controlPanelItem);
      //   query.SetEntity("role", role);

      //   IList<ControlPanelItemRole> controlPanelItemRoles = query.List<ControlPanelItemRole>();

      //   return controlPanelItemRoles;

      //}



      //public void SaveControlPanelItemRole(ControlPanelItemRole controlPanelItemRole)
      //{
      //   using (NHTransactionScope tx = new NHTransactionScope())
      //   {
      //      Repository<ControlPanelItemRole>.Save(controlPanelItemRole);
      //      tx.VoteCommit();
      //   }
      //}



      //public void DeleteControlPanelItemRole(ControlPanelItemRole controlPanelItemRole)
      //{
      //   using (NHTransactionScope tx = new NHTransactionScope())
      //   {
      //      Repository<ControlPanelItemRole>.Delete(controlPanelItemRole);
      //      tx.VoteCommit();
      //   }
      //}


      public IList<ControlPanelItem> GetControlPanelItems()
      {
         //using (NHTransactionScope tx = new NHTransactionScope())
         //{
            //return Session.CreateQuery("from ControlPanelItem cpi order by cpi.ViewOrder")
            //   .List<ControlPanelItem>();
            return Session.CreateQuery("from ControlPanelItem order by ViewOrder ASC")
               .List<ControlPanelItem>();
         //}
      }

      #endregion
   }
}