using System.IO;
using System.Linq;
using Arashi.Services.Themes;

[assembly: WebActivator.PreApplicationStartMethod(typeof(Arashi.Web.Components.Bootstrapper), "PreApplicationStart")]
namespace Arashi.Web.Components
{
   #region Using

   using System;
   using System.Collections.Generic;
   using System.Collections.Specialized;
   using System.Configuration;
   using System.Reflection;
   using System.Web;
   using System.Web.Mvc;
   using System.Web.Routing;
   using Arashi.Core;
   using Arashi.Core.Domain;
   using Arashi.Core.Domain.Validation;
   using Arashi.Core.NHibernate;
   using Arashi.Services;
   using Arashi.Web.Mvc.Routing.Constraints;
   using Arashi.Web.Mvc.Validators;
   using Arashi.Web.Mvc.Windsor;
   using Castle.Components.Scheduler;
   using Castle.Core;
   using Castle.MicroKernel;
   using Castle.MicroKernel.Lifestyle;
   using Castle.MicroKernel.Registration;
   using Castle.Windsor;
   using Castle.Windsor.Configuration.Interpreters;
   using Common.Logging;
   using Microsoft.Web.Infrastructure.DynamicModuleHelper;

   #endregion

   /// <summary>
   /// This class is responsible to the system initialization
   /// </summary>
   public static class Bootstrapper
   {
      private static ILog log; // = LogManager.GetCurrentClassLogger();
      private static bool alreadyInitialized = false;
      private static readonly object _lock = new Object();
      private static readonly object lockObject = new Object();



      /// <summary>
      /// PreApplicationStart Method
      /// </summary>
      public static void PreApplicationStart()
      {
         // ----------------------------------------------------
         // [1] - Initialize NLog via Common.Logging
         // ----------------------------------------------------
         NameValueCollection properties = new NameValueCollection();
         properties["configType"] = "FILE";
         properties["configFile"] = "~/Config/NLog.config";

         LogManager.Adapter = new Common.Logging.NLog.NLogLoggerFactoryAdapter(properties);
         log = LogManager.GetCurrentClassLogger();
         log.Debug("Bootstrapper.PreApplicationStart: Log Provider Initialized");


         // ----------------------------------------------------
         // [2] - Register our HTTP Module
         // ----------------------------------------------------
         DynamicModuleUtility.RegisterModule(typeof(PerWebRequestLifestyleModule));
         DynamicModuleUtility.RegisterModule(typeof(Elmah.ErrorLogModule));
         DynamicModuleUtility.RegisterModule(typeof(Elmah.ErrorMailModule));
         DynamicModuleUtility.RegisterModule(typeof(Arashi.Web.Security.AuthenticationHttpModule));
         DynamicModuleUtility.RegisterModule(typeof(Arashi.Web.Components.NhibernateSessionModule));
         DynamicModuleUtility.RegisterModule(typeof(Arashi.Web.Components.TrackingHttpModule));
         DynamicModuleUtility.RegisterModule(typeof(Arashi.Web.Components.CloakHeaderHttpModule));
         DynamicModuleUtility.RegisterModule(typeof(ClientDependency.Core.Module.ClientDependencyModule));
         log.Debug("Bootstrapper.PreApplicationStart: HttpModules Registered");


         // ----------------------------------------------------
         // [3] - Init Inversion of Control
         // ----------------------------------------------------
         IWindsorContainer container = InitializeIoC();


         // ----------------------------------------------------
         // [4] - Init NHIBERNATE
         // ----------------------------------------------------
         container.Resolve<INHibernateHelper>().Configure();
         container.Register(Component.For<Arashi.Core.NHibernate.ISessionFactory>()
            .ImplementedBy<Arashi.Core.NHibernate.SessionFactory>()
            .Named("SessionFactory")
            .LifeStyle.Singleton
         );
         log.Debug("Bootstrapper.PreApplicationStart: NHIBERNATE Registered");

         // Init the NH Profiler
         if (ConfigurationManager.AppSettings["EnableNHProf"] == "true")
            HibernatingRhinos.Profiler.Appender.NHibernate.NHibernateProfiler.Initialize();


         // ----------------------------------------------------
         // [5] - Start Castle Scheduler
         // ----------------------------------------------------
         //RegisterThemes(container);
         //log.Debug("Bootstrapper.PreApplicationStart: Custom Themes Registered");


         // ----------------------------------------------------
         // [6] - Start Castle Scheduler
         // ----------------------------------------------------
         IScheduler scheduler = IoC.Resolve<IScheduler>();
         Trigger fiveMinutesTrigger = new PeriodicTrigger(DateTime.Today.AddDays(-1), new DateTime(2900, 1, 1), new TimeSpan(0, 5, 0), null);

         // email scheduler
         JobSpec emailJobSpec = new JobSpec("email_job", "Email scheduler job", "Jobs.EmailScheduler", fiveMinutesTrigger);
         scheduler.CreateJob(emailJobSpec, CreateJobConflictAction.Ignore);
         scheduler.Start();

         log.Debug("Bootstrapper.PreApplicationStart: Scheduler initialized");
      }




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

            RegisterThemes(IoC.Container);
            log.Debug("Bootstrapper.PreApplicationStart: Custom Themes Registered");


            // ----------------------------------------------------
            // Register Default Routes
            // ----------------------------------------------------
            RegisterRoutes(RouteTable.Routes);
            log.Debug("Bootstrapper.InitializeSystemForTheFirstTime: RouteTable registered");

            //RegisterGlobalFilters(GlobalFilters.Filters);


            // RouteDebugger 2: you can 'disable' it by setting Copy Local property of RouteDebugger.dll to false.

            // ----------------------------------------------------
            // Instanziate the ViewEngine 
            // ----------------------------------------------------
            // View engine
            InitViewEngine();
            log.Debug("Bootstrapper.InitializeSystemForTheFirstTime: AreaViewEngine initialized");


            // ----------------------------------------------------
            // Custom Validators
            // ----------------------------------------------------
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(EmailAttribute), typeof(EmailValidator));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(EqualToPropertyAttribute), typeof(EqualToPropertyValidator));


            alreadyInitialized = true;

            log.Debug("Bootstrapper.InitializeSystemForTheFirstTime: End");

            //// Re-load the requested page (to avoid conflicts with first-time configured NHibernate modules )
            //HttpContext.Current.Response.Redirect(HttpContext.Current.Request.RawUrl);
         }
      }



      /// <summary>
      /// Init the InversionOfControl Container
      /// </summary>
      public static IWindsorContainer InitializeIoC()
      {
         log.Debug("Bootstrapper.InitializeIoC: Start...");

         try
         {
            // Create the Container and add base components
            IWindsorContainer container = new WindsorContainer(new XmlInterpreter());

            // Register the ILog service
            container.Register(
                Component.For(typeof(ILog))
                    .Instance(log)
            );

            container.Register(Component.For<IHttpModule>()
               .ImplementedBy<NhibernateSessionModule>()
               .Named("NhibernateSessionModule")
               .LifeStyle.Singleton
            );
            container.Register(Component.For<IHttpModule>()
               .ImplementedBy<PerWebRequestLifestyleModule>()
               .Named("PerWebRequestLifestyleModule")
               .LifeStyle.Singleton
            );


            container.Register(Component.For<IControllerActivator>().ImplementedBy<WindsorControllerActivator>());
            
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

            // register the IoC wrapper class
            IoC.Initialize(container);


            // MVC 3: init IoC with the System.Web.Mvc.IDependencyResolver
            DependencyResolver.SetResolver(new WindsorDependencyResolver(container));

            // Windsor controller builder 
            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(container));

            // the following is now done by the WindsorControllerFactory implementation
            // Register into the container all the MVC IController
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
               if (typeof(IController).IsAssignableFrom(type))
               {
                  container.Register(Component.For(type).Named(type.Name.ToLower()).LifeStyle.Is(LifestyleType.Transient));

                  if (log.IsDebugEnabled)
                     log.DebugFormat("InitializeIoC: Registering IController of type {0}.{1}", type.AssemblyQualifiedName, type.Name);
               }
            }

            ////register controllers from themes
            //IThemeService themeService = IoC.Resolve<IThemeService>();
            //IList<Theme> themes = themeService.GetAll();

            //// cycle for every custom theme that has a cusotm assembly
            //log.Debug("InitializeIoC: searching for IController in Theme Assemblies...");
            //foreach (Theme theme in themes.Where(t => !string.IsNullOrEmpty(t.CustomOptionsAssembly)))
            //{
            //   log.DebugFormat("InitializeIoC: Found Theme {0} with assembly {1}", theme.Name, theme.CustomOptionsAssembly);

            //   string themeAssemblyPath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "bin");

            //   log.Debug(themeAssemblyPath);
            //   string[] files = Directory.GetFiles(themeAssemblyPath, theme.CustomOptionsAssembly + ".dll");
            //   log.DebugFormat("InitializeIoC: Found {0} assemblies", files.Length.ToString());

            //   foreach (string s in files)
            //      try
            //      {
            //         var themeAssembly = Assembly.LoadFrom(s);
            //         log.DebugFormat("InitializeIoC: Theme loaded from {0}", s);

            //         foreach (Type type in themeAssembly.GetTypes())
            //         {
            //            if (typeof(IController).IsAssignableFrom(type))
            //            {
            //               container.Register(Component.For(type).Named(type.Name.ToLower()).LifeStyle.Is(LifestyleType.Transient));

            //               if (log.IsDebugEnabled)
            //                  log.DebugFormat("InitializeIoC: [Theme {2}] Registering IController of type {0}.{1}", type.AssemblyQualifiedName, type.Name, theme.Name);
            //            }
            //         }

            //         // Load precompiled views from theme assembly
            //         //ApplicationPartRegistry.Register(themeAssembly);
            //         //log.Debug("InitializeIoC: Registered precompiled Razor Views from theme assembly!");
            //      }
            //      catch (Exception ex)
            //      {
            //         log.Error("Could not load the Theme Assembly", ex);
            //      }
            //}

            log.Debug("Bootstrapper.InitializeIoC: End");
            return container;
         }
         catch (Exception ex)
         {
            log.Error("Error initializing application.", ex);
            throw;
         }

      }



      /// <summary>
      /// Register IController implementations in custom theme assemblies
      /// Note: this MUST be called after NH init
      /// </summary>
      public static void RegisterThemes(IWindsorContainer container)
      {
         //register controllers from themes
         IThemeService themeService = IoC.Resolve<IThemeService>();
         IList<Theme> themes = themeService.GetAll();

         // cycle for every custom theme that has a cusotm assembly
         log.Debug("RegisterThemes: searching for IController in Theme Assemblies...");
         foreach (Theme theme in themes.Where(t => !string.IsNullOrEmpty(t.CustomOptionsAssembly)))
         {
            log.DebugFormat("RegisterThemes: Found Theme {0} with assembly {1}", theme.Name, theme.CustomOptionsAssembly);

            string themeAssemblyPath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "bin");

            log.Debug(themeAssemblyPath);
            string[] files = Directory.GetFiles(themeAssemblyPath, theme.CustomOptionsAssembly + ".dll");
            log.DebugFormat("RegisterThemes: Found {0} assemblies", files.Length.ToString());

            foreach (string s in files)
               try
               {
                  var themeAssembly = Assembly.LoadFrom(s);
                  log.DebugFormat("RegisterThemes: Theme loaded from {0}", s);

                  foreach (Type type in themeAssembly.GetTypes())
                  {
                     if (typeof(IController).IsAssignableFrom(type))
                     {
                        container.Register(Component.For(type).Named(type.Name.ToLower()).LifeStyle.Is(LifestyleType.Transient));

                        if (log.IsDebugEnabled)
                           log.DebugFormat("RegisterThemes: [Theme {2}] Registering IController of type {0}.{1}", type.AssemblyQualifiedName, type.Name, theme.Name);
                     }
                  }

               }
               catch (Exception ex)
               {
                  log.Error("RegisterThemes: Could not load the Theme Assembly", ex);
               }
         }
      }



      ///// <summary>
      ///// Register the global filters
      ///// </summary>
      ///// <param name="filters"></param>
      //public static void RegisterGlobalFilters(GlobalFilterCollection filters)
      //{
      //   filters.Add(new HandleErrorAttribute());
      //}


      /// <summary>
      /// Register the Routes
      /// </summary>
      /// <param name="routes"></param>
      public static void RegisterRoutes(RouteCollection routes)
      {
         routes.Clear();

         routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
         routes.IgnoreRoute("Admin/elmah.axd/{*pathInfo}");
         routes.IgnoreRoute("Resources/tiny_mce34/tiny_mce_gzip.ashx/{*pathInfo}");
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

         // Register the Admin Area 
         // The Admin routes are defined in the Arashi.Web.Areas.Admin.AdminAreaRegistration class
         AreaRegistration.RegisterAllAreas();


         //#region CustomThemes Routes

         //// register custom routes from Theme assemblies
         //IThemeService themeService = IoC.Resolve<IThemeService>();
         //IList<Theme> themes = themeService.GetAll();
         //// cycle for every custom theme that has a cusotm assembly
         //log.Debug("RegisterRoutes: searching for IController in Theme Assemblies...");
         //foreach (Theme theme in themes.Where(t => !string.IsNullOrEmpty(t.CustomOptionsAssembly)))
         //{
         //   log.DebugFormat("RegisterRoutes: Found Theme {0} with assembly {1}", theme.Name, theme.CustomOptionsAssembly);
         //   string themeAssemblyPath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "bin");

         //   log.Debug(themeAssemblyPath);
         //   string[] files = Directory.GetFiles(themeAssemblyPath, theme.CustomOptionsAssembly + ".dll");
         //   log.DebugFormat("RegisterRoutes: Found {0} assemblies", files.Length.ToString());

         //   foreach (string s in files)
         //      try
         //      {
         //         var themeAssembly = Assembly.LoadFrom(s);
         //         log.DebugFormat("RegisterRoutes: Theme loaded from {0}", s);

         //         foreach (Type type in themeAssembly.GetTypes())
         //         {
         //            if (typeof(IThemeRoutes).IsAssignableFrom(type))
         //            {
         //               //container.Register(Component.For(type).Named(type.Name.ToLower()).LifeStyle.Is(LifestyleType.Transient));
         //               ((IThemeRoutes)Activator.CreateInstance(type)).RegisterRoutes(routes);

         //               if (log.IsDebugEnabled)
         //                  log.DebugFormat("RegisterRoutes: [Theme {2}] Registered routes from type {0}.{1}", type.AssemblyQualifiedName, type.Name, theme.Name);
         //            }
         //         }
         //      }
         //      catch (Exception ex)
         //      {
         //         log.Error("RegisterRoutes: Could not load the Theme Assembly", ex);
         //      }
         //}


         //#endregion

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
         ViewEngines.Engines.Add(new RazorViewEngine());
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
               //kernel.AddComponent(moduleTypeKey, widgetTypeType);
               kernel.Register(Component.For(widgetTypeType).Named(moduleTypeKey));
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
