using System;
using System.Web;
using log4net.Repository.Hierarchy;

namespace Arashi.Core.NHibernate
{
   /// <summary>
   /// Credits: original code from Tarantino.Infrastructure.Commons.DataAccess.ORMapper
   /// </summary>
   public class NHibernateSessionModule : IHttpModule
   {
      public void Init(HttpApplication context)
      {
         context.EndRequest += Context_EndRequest;
      }



      private void Context_EndRequest(object sender, EventArgs e)
      {
         var builder = new HybridSessionBuilder();

         var session = builder.GetExistingWebSession();
         if (session != null)
         {
            //Logger.Debug(this, "Disposing of ISession " + session.GetHashCode());
            session.Dispose();
         }
      }



      public void Dispose()
      {
      }
   }
}