using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Windsor;
using Common.Logging;

namespace Arashi.Web.Mvc.Windsor
{
   using System.Collections.Generic;

   /// <summary>
   /// Controller Factory class for instantiating controllers using the Windsor IoC container.
   /// Copied from MvcContrib (http://mvccontrib.googlecode.com/svn/trunk). Not using MvcContrib anymore.
   /// </summary>
   public class WindsorControllerFactory : DefaultControllerFactory
   {
      private ILog log = LogManager.GetCurrentClassLogger();
      private readonly IWindsorContainer container;


      // Decomment this only for debug
      // ---------------------------------------------------------------------------------------------------------------
      //public override IController CreateController(RequestContext requestContext, string controllerName)
      //{
      //   log.DebugFormat("WindsorControllerFactory.CreateController - controllerName = {0}", controllerName);
      //
      //   Type type;
      //   object obj;
      //   requestContext.RouteData.DataTokens.TryGetValue("Namespaces", out obj);
      //   IEnumerable<string> source = obj as IEnumerable<string>;
      //
      //   HashSet<string> namespaces = new HashSet<string>(source, StringComparer.OrdinalIgnoreCase);
      //   //type = this.GetControllerTypeWithinNamespaces(requestContext.RouteData.Route, controllerName, namespaces);
      //
      //   return base.CreateController(requestContext, controllerName);
      //}





      /// <summary>
      /// Creates a new instance of the <see cref="WindsorControllerFactory"/> class.
      /// </summary>
      /// <param name="container">The Windsor container instance to use when creating controllers.</param>
      public WindsorControllerFactory(IWindsorContainer container)
      {
         if (container == null)
            throw new ArgumentNullException("container");

         this.container = container;
      }



      protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
      {
         log.Debug("WindsorControllerFactory.GetControllerInstance");

         if (controllerType == null)
         {
            HttpException ex = new HttpException(404, string.Format("The controller for path '{0}' could not be found or it does not implement IController.", requestContext.HttpContext.Request.Path));
            log.Error(ex.Message, ex);
            throw ex;
         }

         return (IController)container.Resolve(controllerType);
      }



      public override void ReleaseController(IController controller)
      {
         var disposable = controller as IDisposable;

         if (disposable != null)
         {
            disposable.Dispose();
         }

         container.Release(controller);
      }
   }
}