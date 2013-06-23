**************************************************************
 
                  A R A S H I    P R O J E C T

**************************************************************
REQUIREMENTS:
- Microsoft .NET 3.5 Framework
- Microsoft SQL Server 2005/8
- Microsoft IIS 7.x 
   (IIS 6 should be fine, but I don't support it. 
    For more info see http://haacked.com/archive/2008/11/26/asp.net-mvc-on-iis-6-walkthrough.aspx )
**************************************************************

**************************************************************
      INDEX
      =====
      
   1) Full Install
   2) Upgrade
**************************************************************



===============
1) FULL INSTALL
===============

These are the instruction on how to install Arashi from source.

1. Unzip the files to a local directory.

2. Create an empty database. Supported databases are:
   - SQL Server 2005-2008 / SQL Server Express 2005-2008

3. Building with VS 2008 (only if you have downloaded and unzipped the binary version)
   - Make sure that you have VS.NET 2008 SP1 and ASP.NET MVC 1.0 installed
   - Go to the /src directory and Open ArashiPortal.sln with VS.
   - Build Solution.
      
4. Configure database connection.      
   Change the connection string in Web/Web.config (connectionString)
   to the database that you have created in step 2.
     
   Example:
      <add name="arashi-db" connectionString="Server=localhost;Database=Arashi;uid=sa;pwd=password;MultipleActiveResultSets=true" providerName="System.Data.SqlClient" />
     
   NOTE: it is important to leave the attributes 'name="arashi-db"' and the "MultipleActiveResultSets=true" argument in the connection string.
       
   IMPORTANT: Make sure that the account that connects to the database ***for the first time*** has enough 
   permissions to create the database during the setup!
      
5. Configure the web server.
   By default, Arashi is configured to run under IIS as web site; running as virtual directory is not supported.
   - Open the IIS management tool, and create a new web site with the default settings, for now set the port to 8080 or 41.
     If you run IIS v7.x use the default integrated application pool. Arashi supports both classic and integrated pipeline.
   
   - Also, assign write permission to the user NETWORK SERVICE on the folders \Web\log, \Web\Sites
   
   
   Now you are ready to run the application. 
   In a web browser (for a better experience, use Firefox or Google Chrome, don't use IE (please, do that for umanity sake)) 
   open the default url you assigned under IIS, i.e. http://localhost:8080 or http://localhost:41.
   With that, the setup page will appear, follow the instructions to setup the installation.
   
   When everything is finished click on the "Log In" button to access the Control Panel.
   The Control Panel will be always accessible with the url: http://myurl/Admin/Login/



===============
2) UPGRADE
===============

0. Close the browser

1. Make a backup of the folder that contains the isntalled files

2. Make a backup of the installed database

3. In the web root folder, delete the following folders:
   - all subfolders of \Resources
   - all files and folders inside \bin
   - \Admin
   - \Content

4. unzip the files of the latest version and when asked overwrite all

5. Check the connection string in the web.config file.

6. Open the browser, and clear the cookies.
   This is necessary in order to prevent an error after the code migration to v2 of ASP.NET MVC; for details see the following links:
      http://jwwishart.wordpress.com/2010/03/24/upgrade-to-mvc-2-causes-unable-to-cast-object-of-type-system-web-ui-triplet-to-type-system-object-relating-to-antiforgerytoken/
      http://weblogs.asp.net/james_crowley/archive/2010/03/18/beware-upgrade-to-asp-net-mvc-2-0-with-care-if-you-use-antiforgerytoken.aspx
      
7. Run the application, it should detect the upgraded files and after a confirmation it will run a script to upgrade the database.

   
   
Hints and tips:
- It's possible run the database install scripts manually if the installation fails for some
  reason. They are located in the /Web/Install/Database directory.
- The /Sites directory must be writable for the ASP.NET user (NETWORK SERVICE on IIS). This is the directory
  where the content files and search index.

Install Troubleshooting:
If something goes worng during the installation, it is best to erase the db and restart from the begin.
To erase the db, run the script "uninstall.sql" that you find in the folder \Install\Core\Database\MsSql.

