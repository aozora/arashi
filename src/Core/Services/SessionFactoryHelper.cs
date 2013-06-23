using System.Collections.Generic;
using System.Reflection;
using Arashi.Core.Exceptions;
using Arashi.Core.NHibernate;
using Castle.MicroKernel;
using log4net;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Mapping;

namespace Arashi.Core.Services
{
   /// <summary>
   /// Provides utility methods to maintain the <see cref="NHibernate"/> SessionFactory.
   /// </summary>
   public class SessionFactoryHelper
   {
      private static readonly ILog log = LogManager.GetLogger(typeof(SessionFactoryHelper));
      private IKernel kernel;

      /// <summary>
      /// Default constructor.
      /// </summary>
      /// <param name="kernel"></param>
      public SessionFactoryHelper(IKernel kernel)
      {
         this.kernel = kernel;
      }



      /// <summary>
      /// Add a new assembly to the configuration and build a new SessionFactory.
      /// </summary>
      /// <param name="assembly"></param>
      public void AddAssembly(Assembly assembly)
      {
         AddAssemblies(new List<Assembly>(){assembly});
      }



      /// <summary>
      /// Add multiple assembly at once to the configuration and build a new SessionFactory.
      /// </summary>
      /// <param name="assemblies"></param>
      public void AddAssemblies(IList<Assembly> assemblies)
      {
         Configuration nhConfiguration = this.kernel[typeof(Configuration)] as Configuration;
         
         //ISessionBuilder sessionBuilder = IoC.Resolve<ISessionBuilder>();
         //Configuration nhConfiguration = sessionBuilder.GetConfiguration();

         if (nhConfiguration == null)
            throw new ApplicationException("Allarm: I can't find the NHibernate configuration file");

         foreach (Assembly assembly in assemblies)
         {
            nhConfiguration.AddAssembly(assembly);
            log.DebugFormat("SessionFactoryHelper: AddAssembly {0}", assembly.FullName);
         }

         // Rebuild the section factory
         ISessionFactory newSessionFactory = nhConfiguration.BuildSessionFactory();
         log.Debug("SessionFactoryHelper: Rebuilding SessionFactory");

         foreach (var c in nhConfiguration.ClassMappings)
         {
            log.DebugFormat("nhConfiguration.ClassMappings \t{0}", c.ClassName);
         }

         // replace the sessionfactory in the IOC container
         this.kernel.RemoveComponent("nhibernate.factory");
         this.kernel.AddComponentInstance("nhibernate.factory", newSessionFactory);
      }


   }
}