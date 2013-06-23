namespace Arashi.Web.Components
{
   using System;
   using System.Web;
   using Arashi.Core;
   using Arashi.Core.NHibernate;



   /// <summary>
   /// The HttpModule used for the lifecycle "PerWebRequest" to optimize the NHibernate.ISession uses.
   /// </summary>
   public class NhibernateSessionModule : IHttpModule
   {
      ISessionFactory SessionFactory
      {
         get;
         set;
      }

      #region IHttpModule Members

      /// <summary>
      /// Disposes of the resources (other than memory) used by the module that implements <see cref="T:System.Web.IHttpModule"/>.
      /// </summary>
      public void Dispose()
      {
      }


      /// <summary>
      /// Initializes a module and prepares it to handle requests.
      /// </summary>
      /// <param name="context">An <see cref="T:System.Web.HttpApplication"/> that provides access to the methods, properties, and events common to all application objects within an ASP.NET application</param>
      public void Init(HttpApplication context)
      {
         context.Error += ContextError;
         context.EndRequest += ContextEndRequest;
         SessionFactory = IoC.Resolve<ISessionFactory>();
      }

      #endregion

      /// <summary>
      /// Contexts the end request.
      /// </summary>
      /// <param name="sender">The sender.</param>
      /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
      void ContextEndRequest(object sender, EventArgs e)
      {
         SessionFactory.CloseCurrentSession(true);
      }

      /// <summary>
      /// Contexts the error.
      /// </summary>
      /// <param name="sender">The sender.</param>
      /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
      void ContextError(object sender, EventArgs e)
      {
         SessionFactory.CloseCurrentSession(false);
      }
   }
}