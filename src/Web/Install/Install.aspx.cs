namespace Arashi.Web.Install
{
   using System;
   using System.IO;
   using System.Reflection;
   using System.Threading;
   using System.Web;
   using System.Web.Routing;
   using Arashi.Core;
   using Arashi.Core.Domain;
   using Arashi.Core.Domain.Extensions;
   using Arashi.Core.NHibernate;
   using Arashi.Services;
   using Arashi.Services.File;
   using Arashi.Services.Membership;
   using Arashi.Services.SiteStructure;
   using Arashi.Services.Themes;
   using Arashi.Web.Components;
   using Common.Logging;



   /// <summary>
   /// Summary description for Install.
   /// </summary>
   public partial class Install : InstallPageBase
   {
      private ILog log = LogManager.GetCurrentClassLogger();
      private readonly ISiteService siteService;
      private readonly IUserService userService;
      private readonly IThemeService templateService;
      private readonly IFileService fileService;

      #region Constructor

      /// <summary>
      /// Constructor.
      /// </summary>
      public Install()
      {
         siteService = IoC.Resolve<ISiteService>();
         userService = IoC.Resolve<IUserService>();
         templateService = IoC.Resolve<IThemeService>();
         fileService = IoC.Resolve<IFileService>();

         this.Load += new EventHandler(Page_Load);
      }

      #endregion


      /// <summary>
      /// Page Load
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      protected void Page_Load(object sender, EventArgs e)
      {
         log.Debug("Install.aspx.Page_Load: Start");

         ErrorPanel.Visible = false;

         if (!IsPostBack)
         {
            try
            {
               // Check installable state. Check Arashi.Core.Domain
               bool canInstall = false;

               // Core
               DatabaseInstaller dbInstaller = new DatabaseInstaller(Server.MapPath("~/Install/Core"), Assembly.Load("Arashi.Core"));

               if (dbInstaller.CanInstall)
               {
                  lblCoreAssembly.Text = "<ul><li>Arashi Core " + dbInstaller.NewAssemblyVersion.ToString(3) + "</li></ul>";

                  //// Core cms
                  //DatabaseInstaller coreCmsInstaller = new DatabaseInstaller(Server.MapPath("~/Install/Cms"), Assembly.Load("Arashi.Core.Cms"));
                  //if (coreCmsInstaller.CanInstall)
                  //{
                  canInstall = true;
                  //   lblModulesAssembly.Text = "Arashi Core Cms " + coreCmsInstaller.NewAssemblyVersion.ToString(3);
                  //}
               }

               if (canInstall)
               {
                  //DatabasePanel.Visible = true;
                  IntroPanel.Visible = true;
               }
               else
               {
                  // Check if we perhaps need to add an admin
                  if (userService.CountAllUsersForAllSites() == 0)
                     AdminPanel.Visible = true;
                  else
                     ShowError("Can't install Arashi. Check if the install.sql file exists in the /Install/Core|Modules/Database/DATABASE_TYPE/ directory and that there isn't already a version installed.");
               }

               log.Debug("Install.Page_Load: End");
            }
            catch (Exception ex)
            {
               log.Error(ex.ToString());
               ShowError("An error occurred: <br/><br/>" + ex.ToString());
            }
         }

         log.Debug("Install.aspx.Page_Load: End");
      }


      #region Start

      /// <summary>
      /// StartInstallButton_Click
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      protected void StartInstallButton_Click(object sender, EventArgs e)
      {
         IntroPanel.Visible = false;
         PrerequisitesPanel.Visible = true;
      }

      #endregion


      #region Prerequisites Check

      protected void PrerequisitesCheckButton_Click(object sender, EventArgs e)
      {
         log.Debug("Install.PrerequisitesCheckButton_Click: Start");


         string siteDataRoot = Server.MapPath("~/Sites/");
         DirectoryInfo di = new DirectoryInfo(siteDataRoot);
         bool isFSPermissionOk = fileService.CheckIfDirectoryIsWritable(di.Parent.FullName);
         
         DatabaseInstaller dbInstaller = new DatabaseInstaller(Server.MapPath("~/Install/Core"), Assembly.Load("Arashi.Core"));
         bool isDbOk = dbInstaller.TestDatabaseConnection();

         if (isDbOk && isFSPermissionOk)
         {
            lblCheckResults.Text = "The prerequisites check is successfull, please proceed to the next step.";
            GoToStep2Button.Enabled = true;
            PrerequisitesCheckButton.Visible = false;
         }
         else
         {
            string results = "";

            if (!isDbOk)
               results = "<p class=\"field-validation-error\">There is a problem trying to connect to the database, please check the connection string in the web.config file!</p>";
          
            if (!isFSPermissionOk)
               results = "<p class=\"field-validation-error\">The identity used by the application, as configured in the application pool under IIS, doesn't have the write permissions!</p>";

            lblCheckResults.Text = results;
         }

         log.Debug("Install.PrerequisitesCheckButton_Click: End");
      }



      protected void GoToStep2Button_Click(object sender, EventArgs e)
      {
         DatabasePanel.Visible = true;
         PrerequisitesPanel.Visible = false;
      }

      #endregion

      #region Install Database

      protected void InstallDatabaseButton_Click(object sender, EventArgs e)
      {
         log.Debug("Install.InstallDatabaseButton_Click: Start");

         DatabaseInstaller dbInstaller = new DatabaseInstaller(Server.MapPath("~/Install/Core"), Assembly.Load("Arashi.Core"));
         log.Debug("Install.InstallDatabaseButton_Click: DatabaseInstaller initialized");

         try
         {

            dbInstaller.Install();
            log.Debug("Install.InstallDatabaseButton_Click: Database installed successfully");

            DatabasePanel.Visible = false;
            CreateSitePanel.Visible = true;

            ShowMessage("Database successfully created.");

            log.Debug("Install.InstallDatabaseButton_Click: End");
         }
         catch (Exception ex)
         {
            log.Error(ex.ToString());
            ShowError("An error occurred while installing the database tables: <br/>" + ex.ToString());
         }
      }

      #endregion

      #region Site creation code

      /// <summary>
      /// Start the site creation
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      protected void CreateSiteButton_Click(object sender, EventArgs e)
      {
         log.Debug("Install.CreateSiteButton_Click: Start");

         if (!IsValid)
         {
            log.Debug("Install.CreateSiteButton_Click: Page is not valid!");
            return;
         }

         try
         {
            Site site = CreateSite();
            log.Debug("Install.CreateSiteButton_Click: Site created successfully");

            SiteIdHidden.Value = site.SiteId.ToString();

            CreateSitePanel.Visible = false;
            AdminPanel.Visible = true;

            ShowMessage("Site successfully created.");
            log.Debug("Install.CreateSiteButton_Click: End");
         }
         catch (Exception ex)
         {
            log.Error(ex.ToString());
            ShowError("An error occurred while creating the site: <br/>" + ex.ToString());
         }
      }



      /// <summary>
      /// Create a default site
      /// THe administrator must edit it after installation complete
      /// </summary>
      /// <returns></returns>
      private Site CreateSite()
      {
         // Site
         Site site = new Site
                        {
                           Name = "Demo",
                           Description = "Arashi Sample Site",
                           AllowRegistration = false,
                           EnableCaptchaForComments = false,
                           AllowPasswordRetrieval = true,
                           DefaultCulture = "en-US",
                           TimeZone = 0,
                           Email = "webmaster@localhost.com",
                           MaxPostsPerPage = 10,
                           MaxCommentsPerPage = 50,
                           MaxLinksInComments = 3,
                           MaxSyndicationFeeds = 10,
                           AllowComments = true,
                           AllowCommentsOnlyForRegisteredUsers = false,
                           SortCommentsFromOlderToNewest = true,
                           FeedUseSummary = true,
                           ShowAvatars =  true,
                           CreatedDate = DateTime.Now.ToUniversalTime()
                        };


         // Template
         Theme defaultTheme = templateService.GetById(0);
         site.Theme = defaultTheme;

         // Site Host
         SiteHost host = new SiteHost
         {
            HostName = SiteHostTextBox.Text,
            IsDefault = true,
            Site = site,
            CreatedDate = DateTime.Now.ToUniversalTime()
         };

         site.Hosts.Add(host);

         // Site Host for Control Panel
         SiteHost cpHost = new SiteHost
         {
            HostName = "admin." + SiteHostTextBox.Text,
            IsDefault = false,
            Site = site,
            CreatedDate = DateTime.Now.ToUniversalTime()
         };

         site.Hosts.Add(cpHost);

         SeoSettings seo = new SeoSettings
                              {
                                 Site = site,
                                 PostTitleFormat = "%post_title% | %blog_title%",
                                 PageTitleFormat = "%page_title% | %blog_title%",
                                 CategoryTitleFormat = "%category_title% | %blog_title%",
                                 TagTitleFormat = "%tag% | %blog_title%",
                                 SearchTitleFormat = "%search% | %blog_title%",
                                 ArchiveTitleFormat = "%date% | %blog_title%",
                                 Page404TitleFormat = "Nothing found for %request_words%",
                                 DescriptionFormat = "%description%",
                                 UseCategoriesForMeta = true,
                                 GenerateKeywordsForPost = true,
                                 UseNoIndexForArchives = true,
                                 UseNoIndexForCategories = true,
                                 UseNoIndexForTags = false,
                                 GenerateDescriptions = true,
                                 CapitalizeCategoryTitles = true,
                                 RewriteTitles = true,
                                 HomeDescription = "",
                                 HomeKeywords = "",
                                 HomeTitle = ""
                              };
         //site.SeoSettings = seo;


         //using (NHTransactionScope tx = new NHTransactionScope())
         //{
            log.Debug("Install.CreateSite: Saving new site...");

            // Save the new site
            siteService.SaveSite(site);

            // this is ...odd, but if I comment the following 3 lines I get an error
            // from NH: Could not perform Save for Site ---> NHibernate.PropertyValueException: not-null property references a null or transient valueArashi.Core.Domain.SeoSettings.Site
            site.SeoSettings = seo;
            siteService.SaveSeoSettings(seo);
            siteService.SaveSite(site);


            log.Debug("Install.CreateSite: Site saved...");

            log.Debug("Install.CreateSite: Creating default roles...");

            // create default roles
            userService.CreateDefaultRoles(site);

            //tx.VoteCommit();
            log.Debug("Install.CreateSite: Transaction commit");
         //}

         log.Debug("Install.CreateSite: Creating Site folder structure - Start");

         // Create SiteData folder structure
         string siteDataRoot = Server.MapPath(site.SiteDataPath);
         log.Debug("Install.CreateSite: siteDataRoot = " + siteDataRoot);

         DirectoryInfo di = new DirectoryInfo(siteDataRoot);

         // Check if the parent folder is writable
         if (di.Parent != null && !fileService.CheckIfDirectoryIsWritable(di.Parent.FullName))
				throw new IOException(string.Format("Unable to create the site because the directory {0} is not writable.", siteDataRoot));

         string siteDataPhysicalDirectory = siteDataRoot; //Path.Combine(siteDataRoot, site.SiteId.ToString());

         fileService.CreateDirectory(siteDataPhysicalDirectory);
			//CreateDirectory(Path.Combine(siteDataPhysicalDirectory, "UserFiles"));
         fileService.CreateDirectory(Path.Combine(siteDataPhysicalDirectory, "index"));

         log.Debug("Install.CreateSite: Creating Site folder structure - End");

         return site;
      }


      #endregion

      #region Create Admin Account

      /// <summary>
      /// Create the Administrator account
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      protected void AdminButton_Click(object sender, EventArgs e)
      {
         log.Debug("Install.AdminButton_Click: Start");

         if (!IsValid)
         {
            log.Debug("Install.AdminButton_Click: Page is not valid!");
            return;
         }


         // Only create an admin if there are really NO users.
         if (userService.CountAllUsersForAllSites() > 0)
         {
            string msg = "There is already a user in the database. For security reasons Arashi won't add a new user!";
            ShowError(msg);
            return;
         }


         // Retrieve the previous saved Site
         Arashi.Core.Domain.Site site = siteService.GetSiteById(Convert.ToInt32(SiteIdHidden.Value));

         User newAdmin = new User
         {
            DisplayName = "Admin",
            Email = EmailTextBox.Text,
            FirstName = "Admin",
            IsActive = true,
            TimeZone = 0,
            Site = site,
            AdminTheme = "arashi",
            AdminCulture = Thread.CurrentThread.CurrentCulture.Name,
            CreatedDate = DateTime.UtcNow
         };

         newAdmin.Password = newAdmin.HashPassword(PasswordTextBox.Text);


         try
         {
            Role adminRole = userService.GetRoleByName(site, "Administrators");
            newAdmin.Roles.Add(adminRole);
            userService.SaveUser(newAdmin);

            ShowMessage("User successfully created.");
            log.Debug("Install.AdminButton_Click: Admin user created!");

            AdminPanel.Visible = false;
            pnlFinished.Visible = true;

            RemoveInstallLock();

            //at the moment, this is the last step of the installation, so 
            //call the finishing routine here
            FinishInstall();
         }
         catch (Exception ex)
         {
            log.Error(ex.ToString());
            ShowError("An error occurred while creating the administrator: <br/>" + ex.ToString());
         }

         log.Debug("Install.AdminButton_Click: End");
      }

      #endregion 

      /// <summary>
      /// Complete the install calling the Bootstrapper.Initialize
      /// </summary>
      private void FinishInstall()
      {
         log.Debug("Install.FinishInstall: Start");

         // Set the welcome cookie, used by the Dashboard to show a welcome message (and tutorial)
         HttpCookie cookie = new HttpCookie("ShowWelcome", "true")
         {
            HttpOnly = true,
            Path = "/",
            Secure = false,
            Expires = DateTime.Now.AddDays(2)
         };
         HttpContext.Current.Response.Cookies.Add(cookie);

         //upon first install, the registration (on app startup) will have no effect,
         //so make sure this happens after installation
         Bootstrapper.RegisterRoutes(RouteTable.Routes);
         Bootstrapper.InitViewEngine();
         Bootstrapper.RegisterWidgets(false); // the "false" argument is set to prevent the redirect

         log.Debug("Install.FinishInstall: End");
      }


      #region Private Helpers

      /// <summary>
      /// Show the error message
      /// </summary>
      /// <param name="message"></param>
      private void ShowError(string message)
      {
         ErrorLabel.Text = message;
         ErrorPanel.Visible = true;
         MessagePanel.Visible = false;
      }



      /// <summary>
      /// Show a generic message
      /// </summary>
      /// <param name="message"></param>
      private void ShowMessage(string message)
      {
         MessageLiteral.Text = message;
         MessagePanel.Visible = true;
         ErrorPanel.Visible = false;
      }



      /// <summary>
      /// Remove the "install" lock flag
      /// </summary>
      private void RemoveInstallLock()
      {
         HttpContext.Current.Application.Lock();
         HttpContext.Current.Application["IsInstalling"] = false;
         HttpContext.Current.Application.UnLock();
      }

      #endregion

      //private void BindOptionalModules()
      //{
      //   // Retrieve the modules that are already installed.
      //   IList availableModules = _commonDao.GetAll(typeof(ModuleType), "Name");
      //   // Retrieve the available modules from the filesystem.
      //   string moduleRootDir = HttpContext.Current.Server.MapPath("~/Modules");
      //   DirectoryInfo[] moduleDirectories = new DirectoryInfo(moduleRootDir).GetDirectories();
      //   // Go through the directories and add the modules that can be installed
      //   ArrayList installableModules = new ArrayList();
      //   foreach (DirectoryInfo di in moduleDirectories)
      //   {
      //      // Skip hidden directories.
      //      bool shouldAdd = (di.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden;
      //      foreach (ModuleType moduleType in availableModules)
      //      {
      //         if (moduleType.Name == di.Name)
      //         {
      //            shouldAdd = false;
      //            break;
      //         }
      //      }
      //      if (shouldAdd)
      //      {
      //         // Check if the module can be installed
      //         DatabaseInstaller moduleInstaller = new DatabaseInstaller(Path.Combine(Server.MapPath("~/Modules/" + di.Name), "Install"), null);
      //         if (moduleInstaller.CanInstall)
      //         {
      //            installableModules.Add(di.Name);
      //         }
      //      }
      //   }
      //   rptModules.DataSource = installableModules;
      //   rptModules.DataBind();
      //}




      //protected void btnInstallModules_Click(object sender, EventArgs e)
      //{
      //   try
      //   {
      //      InstallOptionalModules();
      //      pnlModules.Visible = false;
      //      pnlAdmin.Visible = true;
      //   }
      //   catch (Exception ex)
      //   {
      //      ShowError("An error occured while installing additional modules: <br/>" + ex.ToString());
      //   }
      //}


      //private void InstallOptionalModules()
      //{
      //   foreach (RepeaterItem ri in rptModules.Items)
      //   {
      //      CheckBox chkInstall = ri.FindControl("chkInstall") as CheckBox;
      //      if (chkInstall != null && chkInstall.Checked)
      //      {
      //         Literal litModuleName = (Literal) ri.FindControl("litModuleName");
      //         string moduleName = litModuleName.Text;
      //         DatabaseInstaller moduleInstaller = new DatabaseInstaller(Path.Combine(Server.MapPath("~/Modules/" + moduleName), "Install"), null);
      //         moduleInstaller.Install();
      //      }
      //   }
      //}


      //protected void btnSkipInstallModules_Click(object sender, EventArgs e)
      //{
      //   pnlModules.Visible = false;
      //   pnlAdmin.Visible = true;
      //}


      //protected void btnSkipCreateSite_Click(object sender, EventArgs e)
      //{
      //   //at the moment, this is the last step of the installation, so 
      //   //call the finishing routine here
      //   FinishInstall();
      //   pnlCreateSite.Visible = false;
      //   pnlFinished.Visible = true;
      //   RemoveInstallLock();
      //}


   }
}
