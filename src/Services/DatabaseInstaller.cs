using System;
using System.Reflection;
using System.IO;
using System.Collections;
using Arashi.Core.Domain;
using log4net;

namespace Arashi.Services
{
   /// <summary>
   /// The DatabaseInstaller class is responsible for installing, upgrading and uninstalling
   /// database tables and records. 
   /// </summary>
   public class DatabaseInstaller
   {
      #region Private Fields

      private static readonly ILog log = LogManager.GetLogger(typeof(DatabaseInstaller));

      private string installRootDirectory;
      private string databaseScriptsDirectory;
      private Assembly assembly;
      private DatabaseType databaseType;
      private string installScriptFile;
      private string uninstallScriptFile;
      private ArrayList upgradeScriptVersions;
      private Version currentVersionInDatabase;

      #endregion

      #region Public Properties

      /// <summary>
      /// The database type of the current database;
      /// </summary>
      public DatabaseType DatabaseType
      {
         get
         {
            return this.databaseType;
         }
      }



      /// <summary>
      /// The current version of the module/assembly in the database.
      /// </summary>
      public Version CurrentVersionInDatabase
      {
         get
         {
            return this.currentVersionInDatabase;
         }
      }



      /// <summary>
      /// The version of the assembly that is to be installed or upgraded.
      /// </summary>
      public Version NewAssemblyVersion
      {
         get
         {
            return this.assembly.GetName().Version;
         }
      }



      /// <summary>
      /// Indicates if a module or assembly can be installed from the given location.
      /// </summary>
      public bool CanInstall
      {
         get
         {
            return CheckCanInstall();
         }
      }



      /// <summary>
      /// Indicates if a module or assembly can be upgraded from the given location.
      /// </summary>
      public bool CanUpgrade
      {
         get
         {
            return CheckCanUpgrade();
         }
      }



      /// <summary>
      /// Indicates if a module or assembly can be uninstalled from the given location.
      /// </summary>
      public bool CanUninstall
      {
         get
         {
            return CheckCanUninstall();
         }
      }

      #endregion


      /// <summary>
      /// Default constructor.
      /// </summary>
      /// <param name="installRootDirectory">
      /// The physical path of the directory where the install
      /// scripts are located. This is the root install directory without 'Database/DatabaseType'.
      /// </param>
      /// <param name="assembly">The (optional) assembly that has to be upgraded or uninstalled.</param>
      public DatabaseInstaller(string installRootDirectory, Assembly assembly)
      {
         log.DebugFormat("DatabaseInstaller.ctor(): installRootDirectory = {0}, assembly = {1}", installRootDirectory, assembly.FullName);

         this.installRootDirectory = installRootDirectory;
         this.assembly = assembly;
         databaseType = DatabaseUtil.GetCurrentDatabaseType();
         string databaseSubDirectory = Path.Combine("Database", this.databaseType.ToString().ToLower());
         databaseScriptsDirectory = Path.Combine(installRootDirectory, databaseSubDirectory);

         upgradeScriptVersions = new ArrayList();
         CheckDatabaseScripts();

         // Sort the versions in ascending order. This way it's easy to iterate through the scripts
         // when upgrading.
         upgradeScriptVersions.Sort();

         if (this.assembly != null)
         {
            CheckCurrentVersionInDatabase();
         }
      }



      /// <summary>
      /// Check if the database connection is valid.
      /// </summary>
      /// <returns></returns>
      public bool TestDatabaseConnection()
      {
         return DatabaseUtil.TestDatabaseConnection();
      }



      /// <summary>
      /// Install the database part of a Cuyaghoga component.
      /// </summary>
      public void Install()
      {
         if (CanInstall)
         {
            log.Info("Installing module with " + this.installScriptFile);
            DatabaseUtil.ExecuteSqlScript(this.installScriptFile);
         }
         else
         {
            throw new InvalidOperationException("Can't install assembly from: " + this.installRootDirectory);
         }
      }



      /// <summary>
      /// Upgrade the database part of a Cuyahoga component to higher version.
      /// </summary>
      public void Upgrade()
      {
         if (CanUpgrade)
         {
            log.Info("Upgrading " + this.assembly.GetName().Name);
            // Iterate through the sorted versions that are extracted from the upgrade script names.
            foreach (Version version in this.upgradeScriptVersions)
            {
               // Only run the script if the version is higher than the current database version
               if (version > this.currentVersionInDatabase)
               {
                  string upgradeScriptPath = Path.Combine(this.databaseScriptsDirectory, version.ToString(3) + ".sql");
                  log.Info("Running upgrade script " + upgradeScriptPath);
                  DatabaseUtil.ExecuteSqlScript(upgradeScriptPath);
                  this.currentVersionInDatabase = version;
               }
            }
         }
         else
         {
            throw new InvalidOperationException("Can't upgrade assembly from: " + this.installRootDirectory);
         }
      }



      /// <summary>
      /// Uninstall the database part of a Cuyaghoga component.
      /// </summary>
      public void Uninstall()
      {
         if (CanUninstall)
         {
            log.Info("Uninstalling module with " + this.installScriptFile);
            DatabaseUtil.ExecuteSqlScript(this.uninstallScriptFile);
         }
         else
         {
            throw new InvalidOperationException("Can't uninstall assembly from: " + this.installRootDirectory);
         }
      }



      /// <summary>
      /// Check if the install sql scripts exists in the correct folder.
      /// The scripts searched must be named as:
      ///   - Full installation script:      install.sql
      ///   - Full uninstallation script:    uninstall.sql
      ///   - Incremental upgrade scripts:   {major}.{minor}.{patch}.sql
      /// </summary>
      private void CheckDatabaseScripts()
      {
         DirectoryInfo directory = new DirectoryInfo(this.databaseScriptsDirectory);

         if (directory.Exists)
         {
            foreach (FileInfo file in directory.GetFiles("*.sql"))
            {
               log.DebugFormat("DatabaseInstaller.CheckDatabaseScripts: found {0}", file.FullName);

               if (file.Name.ToLower() == "install.sql")
               {
                  this.installScriptFile = file.FullName;
               }
               else if (file.Name.ToLower() == "uninstall.sql")
               {
                  this.uninstallScriptFile = file.FullName;
               }
               else
               {
                  // Extract the version from the script filename.
                  // NOTE: these filenames have to be in the major.minor.patch.sql format
                  string[] extractedVersion = file.Name.Split('.');
                  if (extractedVersion.Length == 4)
                  {
                     Version version = new Version(
                        Int32.Parse(extractedVersion[0]),
                        Int32.Parse(extractedVersion[1]),
                        Int32.Parse(extractedVersion[2]));
                     this.upgradeScriptVersions.Add(version);
                  }
                  else
                  {
                     log.Warn(String.Format("Invalid SQL script file found in {0}: {1}", this.databaseScriptsDirectory, file.Name));
                  }
               }
            }
         }
      }



      private void CheckCurrentVersionInDatabase()
      {
         if (this.assembly != null)
         {
            this.currentVersionInDatabase = DatabaseUtil.GetAssemblyVersion(this.assembly.GetName().Name);
         }
      }



      private bool CheckCanInstall()
      {
         return this.currentVersionInDatabase == null && this.installScriptFile != null;
      }



      private bool CheckCanUpgrade()
      {
         if (this.assembly != null)
         {
            if (this.currentVersionInDatabase != null && this.upgradeScriptVersions.Count > 0)
            {
               // Upgrade is possible if the script with the highest version number
               // has a number higher than the current database version AND when the
               // assembly version number is equal or higher than the script with
               // the highest version number.
               Version highestScriptVersion = (Version)this.upgradeScriptVersions[this.upgradeScriptVersions.Count - 1];

               if (this.currentVersionInDatabase < highestScriptVersion
                   && this.assembly.GetName().Version >= highestScriptVersion)
               {
                  return true;
               }
            }
         }
         return false;
      }



      private bool CheckCanUninstall()
      {
         return (this.assembly != null && this.uninstallScriptFile != null);
      }
   }
}