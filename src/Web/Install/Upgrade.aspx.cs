using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Arashi.Core;
using Arashi.Services;
using Arashi.Services.File;
using Arashi.Services.Membership;
using Arashi.Services.SiteStructure;
using Arashi.Services.Themes;
using Common.Logging;

namespace Arashi.Web.Install
{
   public class AssemblyInfo
   {
      public string AssemblyName { get; set; }
      public Version CurrentVersion { get; set; }
      public Version NewVersion { get; set; }
      public bool CanUpgrade { get; set; }
   }



	/// <summary>
	/// Summary description for Upgrade.
	/// </summary>
	public partial class Upgrade : Page
	{
      private ILog log = LogManager.GetCurrentClassLogger();
      private readonly ISiteService siteService;
      private readonly IUserService userService;
      private readonly IThemeService templateService;
      private readonly IFileService fileService;


      #region Constructor

      public Upgrade()
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
         log.Debug("Upgrade.aspx.Page_Load: Start");

         ErrorPanel.Visible = false;

         if (!IsPostBack)
         {
            try
            {
               // Check upgradable state. Check both Cuyahoga.Core and Cuyahoga.Modules.
               bool canUpgrade = false;
               StringBuilder sb = new StringBuilder();
               DatabaseInstaller dbInstaller;

               // get a list of all arashi assemblies
               AppDomain arashiDomain = AppDomain.CurrentDomain;
               var arashiLoadedAssemblies = from a in arashiDomain.GetAssemblies()
                                            where a.FullName.StartsWith("Arashi.")
                                            select a;

               IList<AssemblyInfo> infos = new List<AssemblyInfo>();

               foreach (Assembly assembly in arashiLoadedAssemblies)
               {
                  log.DebugFormat("Loaded Assembly Version: {0} {1}", assembly.FullName, assembly.GetName().Version);
              
                  // Check upgradability for each assembly
                  dbInstaller = new DatabaseInstaller(Server.MapPath("~/Install/Core"), assembly);

                  if (dbInstaller.CanUpgrade)
                     canUpgrade = true;

                  AssemblyInfo info = new AssemblyInfo();
                  info.AssemblyName = assembly.GetName().Name;
                  info.CurrentVersion = dbInstaller.CurrentVersionInDatabase;
                  info.NewVersion = dbInstaller.NewAssemblyVersion;
                  info.CanUpgrade = dbInstaller.CanUpgrade;
                  infos.Add(info);
               }

               UpgradeInforepeater.DataSource = infos;
               UpgradeInforepeater.DataBind();


               //// Core
               //DatabaseInstaller dbInstaller = new DatabaseInstaller(Server.MapPath("~/Install/Core"), Assembly.Load("Arashi.Core"));

               //if (dbInstaller.CanUpgrade)
               //{
               //   canUpgrade = true;
               //   lblCoreAssemblyCurrent.Text = "Arashi Core Domain" + dbInstaller.CurrentVersionInDatabase.ToString(3);
               //   lblCoreAssemblyNew.Text = "Arashi Core Domain" + dbInstaller.NewAssemblyVersion.ToString(3);
               //   // Core modules
               //   //DatabaseInstaller moduleDbInstaller = new DatabaseInstaller(Server.MapPath("~/Install/Modules"), Assembly.Load("Cuyahoga.Modules"));
               //   //lblModulesAssemblyCurrent.Text = "Cuyahoga Core Modules " + moduleDbInstaller.CurrentVersionInDatabase.ToString(3);
               //   //if (moduleDbInstaller.CanUpgrade)
               //   //{
               //   //   lblModulesAssemblyNew.Text = "Cuyahoga Core Modules " + moduleDbInstaller.NewAssemblyVersion.ToString(3);
               //   //}
               //   //else
               //   //{
               //   //   lblModulesAssemblyNew.Text = "Cuyahoga Core Modules - no upgrade available";
               //   //}
               //}

               if (canUpgrade)
               {
                  IntroPanel.Visible = true;
               }
               else
               {
                  ShowError("Nothing to upgrade.");
               }

               log.Debug("Upgrade.Page_Load: End");
            }
            catch (Exception ex)
            {
               log.Error(ex.ToString());
               ShowError("An error occurred: <br/><br/>" + ex.ToString());
            }
         }

         log.Debug("Upgrade.Page_Load: End");
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



      ///// <summary>
      ///// Remove the "install" lock flag
      ///// </summary>
      //private void RemoveInstallLock()
      //{
      //   HttpContext.Current.Application.Lock();
      //   HttpContext.Current.Application["IsInstalling"] = false;
      //   HttpContext.Current.Application.UnLock();
      //}

      #endregion


      /// <summary>
      /// Start the upgrade
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      protected void StartUpgradeButton_Click(object sender, EventArgs e)
		{
         DatabaseInstaller dbInstaller = new DatabaseInstaller(Server.MapPath("~/Install/Core"), Assembly.Load("Arashi.Core"));

			try
			{
				dbInstaller.Upgrade();
            //if (modulesDbInstaller.CanUpgrade)
            //{
            //   modulesDbInstaller.Upgrade();
            //}
            IntroPanel.Visible = false;
            UpgradeCompletedPanel.Visible = true;

				// Remove the IsUpgrading flag, so users can view the pages again.
				HttpContext.Current.Application.Lock();
				HttpContext.Current.Application["IsUpgrading"] = false;
				HttpContext.Current.Application.UnLock();
			}
			catch (Exception ex)
			{
			   log.Error(ex.ToString());
            ShowError("An error occured while upgrading the database tables: <br/>" + ex.ToString());
			}
		}


	}
}
