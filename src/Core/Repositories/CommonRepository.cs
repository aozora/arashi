using Arashi.Core.NHibernate;
using Castle.Facilities.NHibernateIntegration;
using NHibernate;

namespace Arashi.Core.Repositories
{
   /// <summary>
   /// Simple class for easily get an ISession and ITransaction objects
   /// </summary>
   public class CommonRepository
   {
      private readonly ISessionManager sessionManager;
      //private readonly ISessionBuilder sessionManager;

      public CommonRepository(ISessionManager sessionManager)
      {
         this.sessionManager = sessionManager;
      }
      //public CommonRepository(ISessionBuilder sessionManager)
      //{
      //   this.sessionManager = sessionManager;
      //}



      public ISession GetSession()
      {
         return this.sessionManager.OpenSession();
         //return this.sessionManager.GetSession();
      }

   }
}