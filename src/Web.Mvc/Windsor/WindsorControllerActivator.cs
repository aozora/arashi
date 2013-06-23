namespace Arashi.Web.Mvc.Windsor
{
   using System;
   using System.Web.Mvc;
   using System.Web.Routing;

   /// <summary>
   /// 
   /// </summary>
   public class WindsorControllerActivator : IControllerActivator
   {
      #region Implementation of IControllerActivator

      public IController Create(RequestContext requestContext, Type controllerType)
      {
         return DependencyResolver.Current.GetService(controllerType) as IController;   
      }

      #endregion
   }
}
