
namespace Arashi.Web.Services
{
   using System.ServiceModel.DomainServices.Hosting;
   using System.ServiceModel.DomainServices.Server;

   using NHibernate;

   [EnableClientAccess()]
   public class NHibernateDomainService : DomainService
   {
      protected ISession Session;

      public NHibernateDomainService(ISessionFactory sessionFactory)
      {
         this.Session = sessionFactory.OpenSession();
      }



      protected override void Dispose(bool disposing)
      {
         Session.Dispose();
         base.Dispose(disposing);

      }



      protected override bool ExecuteChangeSet()
      {
         using (var trans = Session.BeginTransaction())
         {
            base.ExecuteChangeSet();
            trans.Commit();
            return true;
         }
      }

   }
}


