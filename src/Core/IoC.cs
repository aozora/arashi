using System;
using System.Collections;
using Castle.Windsor;

namespace Arashi.Core
{
   public static class IoC
   {
      private static IWindsorContainer container;


      public static void Initialize(IWindsorContainer windsorContainer)
      {
         InternalContainer = windsorContainer;
      }


      #region Resolve Method (Non Generic)

      public static object Resolve(Type serviceType)
      {
         return Container.Resolve(serviceType);
      }


      public static object Resolve(Type serviceType, IDictionary arguments)
      {
         return Container.Resolve(serviceType, arguments);
      }



      public static object Resolve(Type serviceType, string serviceName)
      {
         return Container.Resolve(serviceName, serviceType);
      }


      public static object Resolve(Type serviceType, string serviceName, IDictionary arguments)
      {
         return Container.Resolve(serviceName, serviceType, arguments);
      }

      #endregion

      #region Resolve Method (Generic)

      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
      public static T Resolve<T>()
      {
         return Container.Resolve<T>();
      }


      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
      public static T Resolve<T>(IDictionary arguments)
      {
         return Container.Resolve<T>(arguments);
      }



      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
      public static T Resolve<T>(string name)
      {
         return Container.Resolve<T>(name);
      }


      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
      public static T Resolve<T>(string name, IDictionary arguments)
      {
         return Container.Resolve<T>(name, arguments);
      }



      #endregion


      public static IWindsorContainer Container
      {
         get
         {
            //IWindsorContainer result = InternalContainer;
            return InternalContainer;
         }
      }


      public static bool IsInitialized
      {
         get
         {
            return InternalContainer != null;
         }
      }



      internal static IWindsorContainer InternalContainer
      {
         get
         {
            return container;
         }
         set
         {
            container = value;
         }
      }


      public static void Reset(IWindsorContainer containerToReset)
      {
         if (containerToReset == null)
            return;

         if (ReferenceEquals(InternalContainer, containerToReset))
         {
            InternalContainer = null;
         }
      }



      public static void Reset()
      {
         IWindsorContainer windsorContainer = InternalContainer;
         Reset(windsorContainer);
      }
   }
}