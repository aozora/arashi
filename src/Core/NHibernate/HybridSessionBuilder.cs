using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using log4net;
using NHibernate;
using NHibernate.Cfg;
using System.Reflection;


namespace Arashi.Core.NHibernate
{
   /// <summary>
   /// Credits: original code from Tarantino.Infrastructure.Commons.DataAccess.ORMapper
   /// </summary>
   public class HybridSessionBuilder : ISessionBuilder
   {
      private static readonly ILog log = LogManager.GetLogger(typeof(HybridSessionBuilder));
      private static readonly Dictionary<string, ISessionFactory> sessionFactories = new Dictionary<string, ISessionFactory>();
      private static readonly Dictionary<string, ISession> currentSessions = new Dictionary<string, ISession>();
      private const string defaultConfigFileName = "hibernate.cfg.xml";
      private static object lockObject = "";



      public ISessionFactory GetSessionFactory()
      {
         log.Debug("HybridSessionBuilder.GetSessionFactory()");
         return GetSessionFactory(defaultConfigFileName);
      }



      public ISession GetSession()
      {
         log.Debug("HybridSessionBuilder.GetSession()");
         return GetSession(defaultConfigFileName);
      }



      public virtual ISession GetSession(string configurationFile)
      {
         log.DebugFormat("HybridSessionBuilder.GetSession(\"{0}\")", configurationFile);
         
         var factory = GetSessionFactory(configurationFile);
         var session = GetExistingOrNewSession(factory, configurationFile);
         
         log.DebugFormat("HybridSessionBuilder.GetSession: Using ISession {0}", session.GetHashCode());
         return session;
      }



      public IStatelessSession GetStatelessSession()
      {
         return GetStatelessSession(defaultConfigFileName);
      }



      public virtual IStatelessSession GetStatelessSession(string configurationFile)
      {
         var factory = GetSessionFactory(configurationFile);
         var session = factory.OpenStatelessSession();
         //Logger.Debug(this, string.Format("Using IStatelessSession {0}", session.GetHashCode()));
         return session;
      }



      public Configuration GetConfiguration()
      {
         log.DebugFormat("HybridSessionBuilder.GetConfiguration(\"{0}\")", defaultConfigFileName);
         return GetConfiguration(defaultConfigFileName);
      }



      public virtual Configuration GetConfiguration(string configurationFile)
      {
         log.InfoFormat("HybridSessionBuilder.GetConfiguration(\"{0}\")", defaultConfigFileName);
         
         Configuration configuration;

         // Add the configuration in the IOC Container
         if (!IoC.Container.Kernel.HasComponent(typeof(Configuration)))
         {
            //only one thread at a time
            System.Threading.Monitor.Enter(lockObject);

            try
            {
               configuration = new Configuration();
               configuration.Configure(GetFileName(configurationFile));

               IoC.Container.Kernel.AddComponentInstance<Configuration>(configuration);
            }
            finally
            {
               System.Threading.Monitor.Exit(lockObject);
            }

            //// this is bad I know, but I don't know how to correct the "item with same key" error
            //try
            //{
            //   configuration = new Configuration();
            //   configuration.Configure(GetFileName(configurationFile));

            //   IoC.Container.Kernel.AddComponentInstance<Configuration>(configuration);
            //}
            //catch (Exception ex)
            //{
            //   log.Debug(ex.ToString());
            //}
         }

         configuration = IoC.Resolve<Configuration>();

         log.InfoFormat("HybridSessionBuilder.GetConfiguration(\"{0}\"): end", defaultConfigFileName);
         return configuration;
      }



      public ISession GetExistingWebSession()
      {
         log.Debug("HybridSessionBuilder.GetExistingWebSession()");
         return GetExistingWebSession(defaultConfigFileName);
      }



      public virtual ISession GetExistingWebSession(string configurationFile)
      {
         log.DebugFormat("HybridSessionBuilder.GetExistingWebSession(\"{0}\")", configurationFile);
         return HttpContext.Current.Items[configurationFile] as ISession;
      }



      public static void ResetSession()
      {
         log.Debug("HybridSessionBuilder.ResetSession()");
         new HybridSessionBuilder().GetSession().Dispose();
      }



      public static void ResetSession(string configurationFile)
      {
         log.DebugFormat("HybridSessionBuilder.ResetSession(\"{0}\")", configurationFile);
         new HybridSessionBuilder().GetSession(configurationFile).Dispose();
      }



      public virtual ISessionFactory GetSessionFactory(string configurationFile)
      {
         log.DebugFormat("HybridSessionBuilder.GetSessionFactory(\"{0}\")", configurationFile);

         if (!sessionFactories.ContainsKey(configurationFile))
         {
            var configuration = GetConfiguration(configurationFile);
            sessionFactories[configurationFile] = configuration.BuildSessionFactory();
            log.Info("HybridSessionBuilder.GetSessionFactory: BuildSessionFactory done.");
         }

         log.DebugFormat("HybridSessionBuilder.GetSessionFactory(\"{0}\"): end", configurationFile);
         return sessionFactories[configurationFile];
      }



      private ISession GetExistingOrNewSession(ISessionFactory factory, string configurationFile)
      {
         log.DebugFormat("HybridSessionBuilder.GetExistingOrNewSession(factory, \"{0}\")", configurationFile);

         if (HttpContext.Current != null)
         {
            var session = GetExistingWebSession();

            if (session == null || !session.IsOpen)
            {
               session = OpenSessionAndAddToContext(factory, configurationFile);
            }

            return session;
         }

         var currentSession = currentSessions.ContainsKey(configurationFile) ? currentSessions[configurationFile] : null;
         if (currentSession == null || !currentSession.IsOpen)
         {
            currentSessions[configurationFile] = OpenSession(factory);
         }

         return currentSessions[configurationFile];
      }



      protected virtual ISession OpenSession(ISessionFactory factory)
      {
         log.Debug("HybridSessionBuilder.OpenSession()");
         return factory.OpenSession();
      }



      private ISession OpenSessionAndAddToContext(ISessionFactory factory, string configurationFile)
      {
         log.DebugFormat("HybridSessionBuilder.OpenSessionAndAddToContext(factory, \"{0}\")", configurationFile);
         
         var session = OpenSession(factory);
         HttpContext.Current.Items.Remove(configurationFile);
         HttpContext.Current.Items.Add(configurationFile, session);

         return session;
      }



      private static string GetFileName(string file)
      {
         log.DebugFormat("HybridSessionBuilder.GetFileName(\"{0}\")", file);

         var fileName = file;
         var fileExists = File.Exists(file);

         if (!fileExists && HttpContext.Current != null)
         {
            //var binPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "bin");
            // See http://mvolo.com/blogs/serverside/archive/2007/11/10/Integrated-mode-Request-is-not-available-in-this-context-in-Application_5F00_Start.aspx
            //var binPath = Path.Combine(HttpRuntime.AppDomainAppPath, "bin");
            //var binPath = HttpRuntime.AppDomainAppPath;
            //fileName = Path.Combine(binPath, fileName);
            fileName = Path.Combine(HttpRuntime.AppDomainAppPath, fileName);

            log.DebugFormat("HybridSessionBuilder.GetFileName: fileName = {0}", fileName);
         }

         if (!File.Exists(fileName))
         {
            var message =
               string.Format("Could not locate NHibernate configuration file at: {0}",
                             fileName);
            throw new ApplicationException(message);
         }

         return fileName;
      }
   }
}