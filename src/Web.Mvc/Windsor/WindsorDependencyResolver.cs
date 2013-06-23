namespace Arashi.Web.Mvc.Windsor
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Web.Mvc;

   using Castle.Windsor;

   using Common.Logging;



   /// <summary>
   /// IDependencyResolver implementation for Castle Windsor
   /// </summary>
   public class WindsorDependencyResolver : IDependencyResolver
   {
      private ILog log = LogManager.GetCurrentClassLogger();
      private readonly IWindsorContainer container;



      public WindsorDependencyResolver(IWindsorContainer container)
      {
         this.container = container;
         log.Debug("WindsorDependencyResolver.ctor()");
      }



      public object GetService(Type serviceType)
      {
         return container.Kernel.HasComponent(serviceType) ? container.Resolve(serviceType) : null;
      }



      public IEnumerable<object> GetServices(Type serviceType)
      {
         return container.Kernel.HasComponent(serviceType) ? container.ResolveAll(serviceType).Cast<object>() : new object[] { };
      }

   }
}
