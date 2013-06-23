using System;
using System.IO;
using System.Reflection;
using System.Web;
using Arashi.Core;
using Arashi.Web.Components;
using Castle.Windsor;
using log4net;
using log4net.Config;

namespace Arashi.Web
{
   public class MvcApplication : HttpApplication, IContainerAccessor
   {
      private static readonly ILog log = LogManager.GetLogger(typeof(MvcApplication));
      private System.Threading.ReaderWriterLock _getModulesLock = new System.Threading.ReaderWriterLock();
      private static object lockObject = "";
      private static readonly AspNetHostingPermissionLevel trustLevel = GetCurrentTrustLevel();


      /// <summary>
      /// Gets the current trust level.
      /// </summary>
      public static AspNetHostingPermissionLevel CurrentTrustLevel
      {
         get
         {
            return trustLevel;
         }
      }


      #region Implementation of IContainerAccessor

      public IWindsorContainer Container
      {
         get
         {
            return IoC.Container;
         }
      }

      #endregion

      #region Application Start & End

      protected void Application_Start()
      {
         // Initialize log4net 
         XmlConfigurator.ConfigureAndWatch(new FileInfo(Server.MapPath("~/") + "config/logging.config"));
         log.Debug("MvcApplication.Application_Start: log4net initialized");

         // Configure log4net to log the current raw url
         log4net.GlobalContext.Properties["request_url"] = new Log4NetContextHelper();
         log.Debug("MvcApplication.Application_Start: Log4NetContextHelper initialized");

         // Set application level flags.
         // TODO: get rid of application variables.
         HttpContext.Current.Application.Lock();
         HttpContext.Current.Application["IsFirstRequest"] = true;
         HttpContext.Current.Application["ModulesLoaded"] = false;
         HttpContext.Current.Application["IsModuleLoading"] = false;
         HttpContext.Current.Application["IsInstalling"] = false;
         HttpContext.Current.Application["IsUpgrading"] = false;
         HttpContext.Current.Application.UnLock();

         //// Init Inversion Of Control
         //Bootstrapper.InitializeIoC();
         //log.Debug("MvcApplication.Application_Start: IoC initialized");

         // Attempt to peform first request initialization
         Bootstrapper.InitializeSystemForTheFirstTime();

         // NH Profiler
         //HibernatingRhinos.NHibernate.Profiler.Appender.NHibernateProfiler.Initialize();

         // Misc
         FixAppDomainRestartWhenTouchingFiles();
      }



      protected void Application_End()
      {
         Container.Dispose();
      }

      #endregion

      #region Begin Request

      /// <summary>
      /// Begin Request
      /// Here we launch the FirstRequestInitialization, due to the fact that under IIS7 Integrated Mode
      /// we cannot use the Application Start event
      /// </summary>
      /// <param name="source"></param>
      /// <param name="e"></param>
      void Application_BeginRequest(Object source, EventArgs e)
      {
         HttpApplication app = (HttpApplication)source;
         HttpContext context = app.Context;

         // Check if the system needs to be installed or upgraded
         //if (HttpContext.Current.Request.RawUrl.IndexOf("Install.aspx") == -1 ||
         //    HttpContext.Current.Request.RawUrl.IndexOf("/Resources/") == -1)
            //Bootstrapper.CheckSystemInstaller();
         if ((bool)HttpContext.Current.Application["IsFirstRequest"])
         {
            Bootstrapper.CheckSystemInstaller();
            HttpContext.Current.Application.Lock();
            HttpContext.Current.Application["IsFirstRequest"] = false;
            HttpContext.Current.Application.UnLock();
         }


         // Load widgets
         // To do this, the db must be already installed!
         if (!(bool)HttpContext.Current.Application["ModulesLoaded"]
            && !(bool)HttpContext.Current.Application["IsModuleLoading"]
            && !(bool)HttpContext.Current.Application["IsInstalling"]
            && !(bool)HttpContext.Current.Application["IsUpgrading"])
         {
            Bootstrapper.RegisterWidgets(true);
         }

      }

      #endregion

      #region Helpers

      private void FixAppDomainRestartWhenTouchingFiles()
      {
         if (CurrentTrustLevel == AspNetHostingPermissionLevel.Unrestricted)
         {
            // From: http://forums.asp.net/p/1310976/2581558.aspx
            // FIX disable AppDomain restart when deleting subdirectory
            // This code will turn off monitoring from the root website directory.
            // Monitoring of Bin, App_Themes and other folders will still be operational, so updated DLLs will still auto deploy.
            PropertyInfo p = typeof(HttpRuntime).GetProperty("FileChangesMonitor", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
            object o = p.GetValue(null, null);
            FieldInfo f = o.GetType().GetField("_dirMonSubdirs", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.IgnoreCase);
            object monitor = f.GetValue(o);
            MethodInfo m = monitor.GetType().GetMethod("StopMonitoring", BindingFlags.Instance | BindingFlags.NonPublic);
            m.Invoke(monitor, new object[] { });
         }
      }



      private static AspNetHostingPermissionLevel GetCurrentTrustLevel()
      {
         foreach (AspNetHostingPermissionLevel trustLevel in new AspNetHostingPermissionLevel[] {
                                                                   AspNetHostingPermissionLevel.Unrestricted,
                                                                   AspNetHostingPermissionLevel.High,
                                                                   AspNetHostingPermissionLevel.Medium,
                                                                   AspNetHostingPermissionLevel.Low,
                                                                   AspNetHostingPermissionLevel.Minimal 
                                                                  })
         {
            try
            {
               new AspNetHostingPermission(trustLevel).Demand();
            }
            catch (System.Security.SecurityException)
            {
               continue;
            }

            return trustLevel;
         }

         return AspNetHostingPermissionLevel.None;
      }

      #endregion


   }
}