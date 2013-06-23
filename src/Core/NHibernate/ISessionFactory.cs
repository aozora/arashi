using NHibernate;

namespace Arashi.Core.NHibernate
{
   using System;

   /// <summary>
   /// Abstract creation of the Nhibernate session.
   /// </summary>
   public interface ISessionFactory
   {
      /// <summary>
      /// Create a session in the current context. If the context already created a 
      /// session, then the active transaction is returned
      /// </summary>
      /// <returns></returns>
      ISession GetSession();



      /// <summary>
      /// This call close the current opened session, and commit/rollback the
      /// transaction if the session was created in transaction.
      /// <remarks>
      /// In this first version of the session factory, the session is always
      /// in transaction.
      /// </remarks>
      /// </summary>
      void CloseCurrentSession(Boolean commitIfInTransaction);
   }
}
