using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Arashi.Core.CallContext;
using NHibernate;

namespace Arashi.Core.NHibernate
{
   /// <summary>
   /// The implemenation of <see cref="ISessionFactory"/>.
   /// </summary>
   public class SessionFactory : ISessionFactory
   {
      internal const String SESSION_KEY = "{9D59CC3E-E68A-49B0-9874-5D76BBBC39A8}";
      internal const String TRANSACTION_KEY = "{346d94e4-6e90-4df4-a93f-4995de273ec3}";

      readonly ICallContextFactory callContextFactory;
      readonly INHibernateHelper nHibernateHelper;

      /// <summary>
      /// Initializes a new instance of the <see cref="SessionFactory"/> class.
      /// </summary>
      /// <param name="callContextFactory">The call context factory.</param>
      /// <param name="nHibernateHelper">The n hibernate helper.</param>
      public SessionFactory(ICallContextFactory callContextFactory, INHibernateHelper nHibernateHelper)
      {
         this.callContextFactory = callContextFactory;
         this.nHibernateHelper = nHibernateHelper;
      }



      /// <summary>
      /// Releases unmanaged resources and performs other cleanup operations before the
      /// <see cref="SessionFactory"/> is reclaimed by garbage collection.
      /// </summary>
      ~SessionFactory()
      {
         this.CloseCurrentSession(false);
      }

      #region ISessionFactory Members

      /// <summary>
      /// Create a session in the current context. If the context already created a
      /// session, then the active transaction is returned
      /// </summary>
      /// <returns></returns>
      public ISession GetSession()
      {
         ICallContext currentContext = callContextFactory.RetrieveCallContext();

         ISession session;

         if (currentContext[SESSION_KEY] == null)
         {
            session = CreateAndPutSessionInContext(currentContext);
         }
         else
         {
            //Session is already in context, but it could be disposed
            session = (ISession)currentContext[SESSION_KEY];
            if (!session.IsOpen)
            {
               session = CreateAndPutSessionInContext(currentContext);
            }
         }
         return session;
      }



      /// <summary>
      /// This call close the current opened session, and commit/rollback the
      /// transaction if the session was created in transaction.
      /// <remarks>In this first version of the session factory, the session is always
      /// in transaction.</remarks>
      /// </summary>
      /// <param name="commitIfInTransaction"></param>
      public void CloseCurrentSession(bool commitIfInTransaction)
      {
         ICallContext currentContext = callContextFactory.RetrieveCallContext();

         if (currentContext[SESSION_KEY] == null)
            return;

         using (var session = (ISession)currentContext[SESSION_KEY])
         {
            try
            {
               if (session.IsOpen)
                  if (session.Transaction != null && session.Transaction.IsActive)
                  {
                     if (commitIfInTransaction)
                        session.Transaction.Commit();
                     else
                        session.Transaction.Rollback();
                  }
                  else
                     session.Flush();
            }
            finally
            {
               currentContext[SESSION_KEY] = null;
               currentContext[TRANSACTION_KEY] = null;
            }
         }
      }

      #endregion

      /// <summary>
      /// Create the ISession with FlushMode.Commit and begin a new transaction
      /// </summary>
      /// <param name="callContext"></param>
      /// <returns></returns>
      internal ISession CreateAndPutSessionInContext(ICallContext callContext)
      {
         ISession session = nHibernateHelper.GetSession();
         session.FlushMode = FlushMode.Commit;
         callContext[SESSION_KEY] = session;
         callContext[TRANSACTION_KEY] = session.BeginTransaction(IsolationLevel.ReadCommitted);
         return session;
      }
   }
}
