using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Arashi.Core;
using Arashi.Core.Domain;
using Arashi.Core.Services;
using Arashi.Services;
using Arashi.Web.Mvc.Routing.Constraints;
using Arashi.Web.Mvc.Windsor;
using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using log4net;

namespace Arashi.Web.Components
{
   using Castle.Components.Scheduler;

   /// <summary>
   /// This class is responsible to the system initialization
   /// </summary>
   public static class Bootstrapper
   {
      private static readonly ILog log = LogManager.GetLogger(typeof(Bootstrapper));
      private static bool alreadyInitialized = false;
      private static object _lock = new Object();
      private static object lockObject = new Object();


      /// <summary>
      /// Initialize the system.
      /// This method is called ONLY on the first request after application start.
      /// </summary>
      public static void InitializeSystemForTheFirstTime()
      {
         log.Debug("Bootstrapper.InitializeSystemForTheFirstTime: Start...");

         lock (_lock)
         {
            if (alreadyInitialized)
            {
               log.Debug("Bootstrapper.InitializeSystemForTheFirstTime: alreadyInitialized = true ==> return");
               return;
            }

            // ----------------------------------------------------
            // [1] - Init Inversion of Control
            // ----------------------------------------------------
            InitializeIoC();

            // ----------------------------------------------------
            // [2] - Register Default Routes
            // ----------------------------------------------------
            RegisterRoutes(RouteTable.Routes);
            log.Debug("Bootstrapper.InitializeSystemForTheFirstTime: RouteTable registered");


            // Check if the RouteDebug is enabled
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["EnableRouteDebug"]))
               RouteDebug.RouteDebugger.RewriteRoutesForTesting(RouteTable.Routes);


            // ----------------------------------------------------
            // [3] - Instanziate the ViewEngine 
            // ----------------------------------------------------
            // View engine
            InitViewEngine();
            log.Debug("Bootstrapper.InitializeSystemForTheFirstTime: AreaViewEngine initialized");


            // ----------------------------------------------------
            // [4] - Start Scheduler
            // ----------------------------------------------------
            IScheduler scheduler = IoC.Resolve<IScheduler>();
            Trigger fiveMinutesTrigger = new PeriodicTrigger(DateTime.Today.AddDays(-1), new DateTime(2900, 1, 1), new TimeSpan(0, 5, 0), null);

            // email scheduler
            JobSpec emailJobSpec = new JobSpec("email_job", "Email scheduler job", "Jobs.EmailScheduler", fiveMinutesTrigger);
            scheduler.CreateJob(emailJobSpec, CreateJobConflictAction.Ignore);
            scheduler.Start();

            log.Debug("Bootstrapper.InitializeSystemForTheFirstTime: Scheduler initialized");


            alreadyInitialized = true;

            log.Debug("Bootstrapper.InitializeSystemForTheFirstTime: End");

            //// Re-load the requested page (to avoid conflicts with first-time configured NHibernate modules )
            //HttpContext.Current.Response.Redirect(HttpContext.Current.Request.RawUrl);
         }
      }



      /// <summary>
      /// Init the InversionOfControl Container
      /// </summary>
      public static void InitializeIoC()
      {
         log.Debug("Bootstrapper.InitializeIoC: Start...");

         try
         {
            IWindsorContainer container = new WindsorContainer(new XmlInterpreter());
            container.AddComponent("core.sessionfactoryhelper", typeof(SessionFactoryHelper));
            IoC.Initialize(container);


            // Add IRequestContext to the container.
            container.Register(Component.For<IRequestContext>()
               .ImplementedBy<RequestContext>()
               .Named("core.context")
               .LifeStyle.PerWebRequest
            );

            // Add IRequestContextProvider to the container.
            container.Register(Component.For<IRequestContextProvider>()
               .ImplementedBy<RequestContextProvider>()
            );


            // Windsor controller builder (MVCContrib)
            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(container));

            // Register into the container all the MVC IController
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
               if (typeof(IController).IsAssignableFrom(type))
               {
                  container.Kernel.AddComponent(type.Name.ToLower(), type, LifestyleType.Transient);
                  if (log.IsDebugEnabled)
                     log.DebugFormat("InitializeIoC: Registering IController of type {0}", type.Name.ToLower());
               }
            }

         }
         catch (Exception ex)
         {
            log.Error("Error initializing application.", ex);
            throw;
         }

         log.Debug("Bootstrapper.InitializeIoC: End");
      }



      /// <summary>
      /// Register the Routes
      /// </summary>
      /// <param name="routes"></param>
      public static void RegisterRoutes(RouteCollection routes)
      {
         routes.Clear();

         routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
         routes.IgnoreRoute("Admin/elmah.axd/{*pathInfo}");
         routes.IgnoreRoute("{*allaspx}", new
         {
            allaspx = @".*\.aspx(/.*)?"
         });
         routes.IgnoreRoute("{*allashx}", new
         {
            allashx = @".*\.ashx(/.*)?"
         });
         routes.IgnoreRoute("{*favicon}", new
         {
            favicon = @"(.*/)?favicon.ico(/.*)?"
         });

         #region ControlPanel Routes

         // Register the Admin Area 
         // The Admin routes are defined in the Arashi.Web.Areas.Admin.AdminAreaRegistration class
         AreaRegistration.RegisterAllAreas();

         #endregion

         #region FrontEnd Routes

         // FrontEnd routes
         //routes.CreateArea("Content", "Arashi.Web.Content.Controllers",
            routes.MapRoute("media", "media/{name}", new
            {
               controller = "Media",
               action = "GetMedia"
            });
            routes.MapRoute("feed-rss", "feed/", new
            {
               controller = "Feed",
               action = "RssEntries"
            });
            routes.MapRoute("feed-atom", "feed/atom/", new
            {
               controller = "Feed",
               action = "AtomEntries"
            });
            //routes.MapRoute("feeds", "comments/feed", new
            //{
            //   controller = "Feed",
            //   action = "Comments"
            //});
            routes.MapRoute("author", "author/{name}/", new
            {
               controller = "Post",
               action = "Author",
               name = ""
            });
            routes.MapRoute("page", "page/{name}/", new
            {
               controller = "Post",
               action = "Page",
               name = ""
            });
            routes.MapRoute("contact", "contact/", new
            {
               controller = "Post",
               action = "Contact"
            });
            routes.MapRoute("contact-save", "contact/savecontact/", new
            {
               controller = "Post",
               action = "SaveContact"
            },
               new
               {
                  httpmethod = new HttpMethodConstraint("POST")
               }
            );
            routes.MapRoute("contact-save-ajax", "contact/savecontactajax/", new
            {
               controller = "Post",
               action = "SaveContactAjax"
            },
               new
               {
                  httpmethod = new HttpMethodConstraint("POST")
               }
            );
            routes.MapRoute("category", "category/{name}/", new
            {
               controller = "Post",
               action = "Category",
               name = ""
            });
            routes.MapRoute("tag", "tag/{name}/", new
            {
               controller = "Post",
               action = "Tag",
               name = ""
            });
            routes.MapRoute("search", "search/", new
            {
               controller = "Post",
               action = "Search"
            });
            routes.MapRoute("post-comment", "{year}/{month}/{day}/{name}/savecomment/", new
            {
               controller = "Post",
               action = "SaveComment"
            },
               new
               {
                  year = new YearRouteConstraint(),
                  month = new MonthRouteConstraint(),
                  day = new DayRouteConstraint()
               });
            routes.MapRoute("single", "{year}/{month}/{day}/{name}/", new
            {
               controller = "Post",
               action = "Single"
            },
            new
            {
               year = new YearRouteConstraint(),
               month = new MonthRouteConstraint(),
               day = new DayRouteConstraint()
            });

            // Archives
            routes.MapRoute("archive-day", "{year}/{month}/{day}/", new
            {
               controller = "Post",
               action = "Archive"
            },
               new
               {
                  year = new YearRouteConstraint(),
                  month = new MonthRouteConstraint(),
                  day = new DayRouteConstraint()
               });
            routes.MapRoute("archive-month", "{year}/{month}/", new
            {
               controller = "Post",
               action = "Archive"
            },
               new
               {
                  year = new YearRouteConstraint(),
                  month = new MonthRouteConstraint()
               });
            routes.MapRoute("archive-year", "{year}/", new
            {
               controller = "Post",
               action = "Archive"
            },
               new
               {
                  year = new YearRouteConstraint()
               });

            routes.MapRoute("302", "302/", new
            {
               controller = "Maintenance",
               action = "Index"
            });

            routes.MapRoute("home", "", new
            {
               controller = "Post",
               action = "Index"
            });

            // WARNING: the following generic route MUST BE DISABLED. 
            //          if not, the modules routes that are added later will never be hit!
            //// Default route
            //,routes.MapRoute(null, "{controller}/{action}/{id}", new
            //{
            //   action = "Index",
            //   controller = "Home",
            //   id = ""
            //})

         //);
         #endregion

      }



      /// <summary>
      /// Instantiate the ViewEngine
      /// </summary>
      public static void InitViewEngine()
      {
         ViewEngines.Engines.Clear();
         //ViewEngines.Engines.Add(new AreaViewEngine());
         ViewEngines.Engines.Add(new WebFormViewEngine());
      }



      /// <summary>
      /// Register in the IoC all the IWidgetComponents
      /// </summary>
      public static void RegisterWidgets(bool redirectAfterRegistration)
      {
         log.Debug("Bootstrapper.RegisterWidgets: start");

         HttpContext.Current.Application.Lock();
         HttpContext.Current.Application["IsModuleLoading"] = true;
         HttpContext.Current.Application.UnLock();

         IKernel kernel = IoC.Container.Kernel;

         Arashi.Services.Widget.IWidgetService widgetService = IoC.Resolve<Arashi.Services.Widget.IWidgetService>();

         // Get all widget types registered in the system
         IList<WidgetType> types = widgetService.GetWidgetTypes();
         log.DebugFormat("Bootstrapper.RegisterWidgets: found {0} widget types", types.Count.ToString());

         foreach (WidgetType widgetType in types)
         {
            //only one thread at a time
            System.Threading.Monitor.Enter(lockObject);

            string assemblyQualifiedName = widgetType.ClassName + ", " + widgetType.AssemblyName;

            log.DebugFormat("Bootstrapper: registering widget component for widget type: {0} - {1}", widgetType.Name, assemblyQualifiedName);

            // First, try to get the CLR module type
            Type widgetTypeType = Type.GetType(assemblyQualifiedName);
            if (widgetTypeType == null)
               throw new Exception("Could not find module: " + assemblyQualifiedName);

            try
            {
               // check if the component is already registered
               if (kernel.HasComponent(widgetTypeType))
                  return;

               //Register the widget component
               string moduleTypeKey = "widgetcomponent." + widgetTypeType.FullName;
               kernel.AddComponent(moduleTypeKey, widgetTypeType);
            }
            finally
            {
               System.Threading.Monitor.Exit(lockObject);
            }
         }

         HttpContext.Current.Application.Lock();
         HttpContext.Current.Application["ModulesLoaded"] = true;
         HttpContext.Current.Application["IsModuleLoading"] = false;
         HttpContext.Current.Application.UnLock();

         log.Debug("Bootstrapper.RegisterWidgets: end. Redirecting to self");

         // Re-load the requested page (to avoid conflicts with first-time configured NHibernate modules )
         if (redirectAfterRegistration)
            HttpContext.Current.Response.Redirect(HttpContext.Current.Request.RawUrl);
      }




      /// <summary>
      /// Initialize Arashi (check installer).
      /// </summary>
      /// <remarks>
      /// Because we might perform some redirects, it's not possible to call this method from Application_Start 
      /// in Global.asax.cs (on IIS 7).
      /// </remarks>
      public static void CheckSystemInstaller()
      {
         if (HttpContext.Current.Request.RawUrl.Contains("Install") ||
             HttpContext.Current.Request.RawUrl.Contains("Resources/"))
            return;

         log.Debug("Bootstrapper.CheckSystemInstaller: start");

         try
         {
            // Check version and redirect to install pages if neccessary.
            DatabaseInstaller dbInstaller = new DatabaseInstaller(HttpContext.Current.Server.MapPath("~/Install/Core"),
                                                                  Assembly.Load("Arashi.Core"));
            // if the db connection is ok...
            if (dbInstaller.TestDatabaseConnection())
            {
               if (dbInstaller.CanUpgrade)
               {
                  HttpContext.Current.Application.Lock();
                  HttpContext.Current.Application["IsUpgrading"] = true;
                  HttpContext.Current.Application.UnLock();
                  log.Debug("Bootstrapper.CheckSystemInstaller: IsUpgrading = true");

                  HttpContext.Current.Response.Redirect("~/Install/Upgrade.aspx");
               }
               else if (dbInstaller.CanInstall)
               {
                  HttpContext.Current.Application.Lock();
                  HttpContext.Current.Application["IsInstalling"] = true;
                  HttpContext.Current.Application.UnLock();
                  log.Debug("Bootstrapper.CheckSystemInstaller: IsInstalling = true");

                  HttpContext.Current.Response.Redirect("~/Install/Install.aspx");
               }
            }
            else
            {
               throw new Exception("Arashi can't connect to the database. Please check your application settings.");
            }
         }finally
         {
            log.Debug("Bootstrapper.CheckSystemInstaller: end");
         }
      }

   }
}
