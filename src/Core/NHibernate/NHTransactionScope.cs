using System;
using System.Web;
using Arashi.Core.Repositories;
using NHibernate;
using System.Runtime.Remoting.Messaging;

namespace Arashi.Core.NHibernate
{
   /// <summary>
   /// NHTransactionScope
   /// Implements nested ITransaction for NH like System.Transaction.TransactionScope
   /// 
   /// Ref.: http://www.codeproject.com/aspnet/NHibernateArchitecture.asp
   /// NOTE: to check for non web apps
   /// 
   /// When your application completes all work it wants to perform in a transaction, 
   /// you should call the Complete method only once to inform that transaction manager that 
   /// it is acceptable to commit the transaction. Failing to call this method aborts the transaction.
   /// 
   /// A call to the Dispose method marks the end of the transaction scope. 
   /// Exceptions that occur after calling this method may not affect the transaction.
   /// </summary>
   public class NHTransactionScope : IDisposable
   {
      private bool isWeb = false;
      private bool voteCommit = false;
      private ITransaction transaction;

      /// <summary>
      /// Vote for a commit at the end of the current using block
      /// </summary>
      public void VoteCommit()
      {
         voteCommit = true;
      }


      internal ITransaction InternalTransaction
      {
         get
         {
            if (isWeb)
               return (ITransaction)HttpContext.Current.Items["THREAD_TRANSACTION"];
            else
               return (ITransaction)CallContext.GetData("THREAD_TRANSACTION");
         }
         set
         {
            if (isWeb)
               HttpContext.Current.Items["THREAD_TRANSACTION"] = value;
            else
               CallContext.SetData("THREAD_TRANSACTION", value);
         }
      }


      public NHTransactionScope()
      {
         object obj = new object();

         if (HttpContext.Current != null)
            isWeb = true;

         if (ActiveTransactions == 0)
         {
            transaction = InternalTransaction;

            if (transaction == null)
            {
               transaction = RepositoryHelper.GetSession().BeginTransaction();
               InternalTransaction = transaction;
            }
         }
         ActiveTransactions++;
      }


      private int ActiveTransactions
      {
         get
         {
            if (isWeb)
               return HttpContext.Current.Items["Active_Transactions"] == null ? 0 : Convert.ToInt32(HttpContext.Current.Items["Active_Transactions"]);
            else
               return CallContext.GetData("Active_Transactions") == null ? 0 : Convert.ToInt32(CallContext.GetData("Active_Transactions"));
         }
         set
         {
            if (isWeb)
               HttpContext.Current.Items["Active_Transactions"] = value;
            else
               CallContext.SetData("Active_Transactions", value);
         }
      }


      private void DisposeActiveTransacions()
      {
         if (isWeb)
            HttpContext.Current.Items["Active_Transactions"] = null;
         else
            CallContext.SetData("Active_Transactions", null);
      }


      public void Dispose()
      {
         ActiveTransactions--;

         ISession session = RepositoryHelper.GetSession();

         if (voteCommit)
         {
            if (voteCommit && ActiveTransactions == 0 && session.Transaction.IsActive)
            {
               DisposeActiveTransacions();

               try
               {
                  if (transaction != null && !transaction.WasCommitted && !transaction.WasRolledBack)
                  {
                     transaction.Commit();
                     InternalTransaction = null;
                  }
               }
               catch (HibernateException)
               {
                  InternalTransaction = null;

                  if (transaction != null && !transaction.WasCommitted && !transaction.WasRolledBack)
                  {
                     transaction.Rollback();
                  }

                  throw;
               }
            }
         }
         else if (RepositoryHelper.GetSession().Transaction.IsActive)
         {
            DisposeActiveTransacions();

            InternalTransaction = null;

            if (transaction != null && !transaction.WasCommitted && !transaction.WasRolledBack)
            {
               transaction.Rollback();
            }
         }
      }

   }
}