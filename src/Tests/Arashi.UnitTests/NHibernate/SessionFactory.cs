using NHibernate;
using ISessionFactory = Arashi.Core.NHibernate.ISessionFactory;

namespace Arashi.UnitTests.NHibernate
{
   public class SessionFactory : ISessionFactory
   {
      readonly ISession session;

      #region Implementation of ISessionFactory

      /// <summary>
      /// Create a session in the current context. If the context already created a 
      /// session, then the active transaction is returned
      /// </summary>
      /// <returns></returns>
      public ISession GetSession()
      {
         return session;
      }



      /// <summary>
      /// This call close the current opened session, and commit/rollback the
      /// transaction if the session was created in transaction.
      /// <remarks>
      /// In this first version of the session factory, the session is always
      /// in transaction.
      /// </remarks>
      /// </summary>
      public void CloseCurrentSession(bool commitIfInTransaction)
      {
         if (session.Transaction != null && session.Transaction.IsActive && commitIfInTransaction)
         {
            session.Transaction.Commit();
         }

         session.Close();
      }

      #endregion
   }
}
