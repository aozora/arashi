using NHibernate;
using ISessionFactory = Arashi.Core.NHibernate.ISessionFactory;

namespace Arashi.Services
{
   using Common.Logging;


   /// <summary>
   /// Base class for services
   /// </summary>
   public abstract class ServiceBase
   {
      /// <summary>
      /// ctor
      /// </summary>
      /// <param name="sessionFactory"></param>
      /// <param name="log"></param>
      protected ServiceBase(ISessionFactory sessionFactory, ILog log)
      {
         SessionFactory = sessionFactory;
         this.log = log;
      }



      protected ISessionFactory SessionFactory
      {
         get;
         private set;
      }



      protected ISession Session
      {
         get
         {
            return SessionFactory.GetSession();
         }
      }


      protected ILog log
      {
         get;
         private set;
      }



   }
}
