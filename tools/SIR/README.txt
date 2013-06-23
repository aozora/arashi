README: Application Gallery SIR (Sniffer, Installer, Reporter)
Version 1.0.0.1 - 2009-07-30
==============================================================

ABSTRACT
========
SIR is an automated testing utility for evaluating the structure and
functionality of an application package created for the Windows Web
Application Gallery.  (http://www.microsoft.com/web/gallery).  

The guide for creating these packages can be found at:

http://learn.iis.net/page.aspx/578/application-packaging-guide-for-the-windows-web-application-gallery.

NOTE - This tool is Microsoft Intellectual Property and for use by
Microsoft Employees only.  Do not provide this software, or any 
portion of it, to any third party.


FUNCTIONS
=========

1. Sniffer - SIR will examine the application package using a variety
of techniques.  SIR is looking for potential errors in syntax,
structure, naming conventions, formatting, and others.  Each test will
log an entry for the report.

2. Installer - If the Sniffer doesn't find any blocking errors, SIR
will attempt to install the application on the local machine.  If the
installation is successful, SIR will try to retrieve the application's
default page.  Applications are installed to the system's Default Web
Site.  The URL for the application will be http://localhost/appname.
When SIR installs and verifies an application, the application will
remain installed.

3. Reporter - SIR generates an XML format report of all of the actions
it was able to take.  There is an XSLT avaailable for viewing this
report in a browser.


PREREQUISITES
=============

1. Windows XP (except Home), Windows Server 2003, Windows Vista,
Windows Server 2008, Windows 7 or Windows Server 2008 R2.

2. IIS Installed and running.

3. The Web Deployment Tool installed.

4. Database and connection tools for the database that the application
uses.  

4a. MySQL needs the MySQL Server installed and running, and the .NET
Connector for MySQL 5.2.5. 

4b. SQL needs Either SQL Express or SQL Server installed and running
and the SQL Management Objects installed.


NOTE - The best way to get all of the prerequisites after the OS
installed and configured is to use the Web Platform Installer to
install an application from the Web Application Gallery that has
similar requirements to your application.


CONFIGURATION
=============

SIR uses a configuration.xml file that is installed in the directory
that SIR's executable was installed to.  The following items can be
configured using this file:

* Report folder - This is the default path that reports are placed in
  after SIR Runs.  The default is "C:\Reports".  This directory must
  exist prior to running SIR.

* Report file name - This should be left blank, as SIR will set up a
  good default file name.  If you have a specific need for a
  consistent file name, this is the place to set it.

* parameterSchema - By default, SIR looks for the parameters.xsd file
  in the directory that SIR was installed into.  If you need to use a
  different parameters.xsd file, you can override the default here.

* Required and Optional Providers - Do not change the items in this
  list.  These represent the official list of required and optional
  providers in the manifest.xml file that are allowed in packages for
  the Web Application Gallery.

* Valid Tags - Do not change the items in this list.  This list
  represents the official list of known and valid tags that can be
  used with parameters in the parameters.xml file.

* dbInfo - These parameters are used by SIR when performing a test
  installation of your application.  The admin userid and password,
  and the instance specified will be used to build the connection
  string used by the Web Deployment tool to execute any SQL files that
  are included with the database provider.


EXECUTION
=========

SIR must be run from am elevated command line.  The syntax for calling
SIR can be found by running 'AppGallerySIR.exe -h'.  The output from
this command is:

Microsoft SIR (Sniffer, Installer and Reporter) for Web Application
               Gallery Packages
Version 1.0.0.1
Copyright (c) Microsoft Corporation. All rights reserved.


AppGallerySIR <-source:<path>> [args ...]
  -s:|-source:<path>              Required parameter -
                                  The absolute physical path to a .zip file
                                  (or) the absolute physical path to folder
                                  containing zip files
                                  (or) a HTTP URL that points to a .zip file
  -r:|-reportFolderPath:<path>    The path to the directory where reports will
                                  be placed.  This can be an absolute path or
                                  a relative path.
  -w:|-reportFileName:<name>      name of the report file
  -p:|-parameterSchema:<path>     The absolute path to the parameters.xsd file
  -skipInstallation               Skip installation of the package using
                                  msdeploy
  -help                           Show this help



For example:

AppGallerySIR -s:C:\Users\Fred\Documents\Packages\application.zip -r:reports

* This command will run SIR against the application package in the path
  above.  
* It will look to see if there is a directory at
  C:\Users\Fred\Documents\Packages\reports\ for the report.  If the
  reports directory does not exist, SIR will inform the user and then
  exit.
* SIR will attempt to install the application under the local IIS
  install's "Default Web Site".   If the Default Web Site does not
  exist, then SIR will show an installation failure.


GENERAL NOTES
=============

1.  The specified source for SIR must always be an absolute path to a
    package file, a directory with multiple package files, or an URL.

2.  The specified location for the reports folder can be an absolute
    path, or a path relative to the directory that contains the source.

3.  SIR must be run in the directory that has the Configuration.xml
    file, or you need to sepcify its locations on the command line.

4.  To view the SIR report, open up the XML report file in a browser
    that supports XML / XSL combinations (most modern browsers).


FAQ
===

Q - If My package passes all of SIR's tests does that mean it's ready
for the Gallery?

A - Not yet.  SIR catches a number of potential issues, but not all of
them.  All application packages need to be evaluated by the
Application Gallery Team before they can be included. 


Q - SIR says something's wrong with my application package, but I
can't figure out how to fix it.  Who can I ask?

A - You can always e-mail appgal@microsoft.com with any questions
about building an application package.  If you have a report from SIR,
please include it with the e-mail.


Q - Why does SIR need to run from an elevated command prompt?

A - SIR calls the Web Deployment Tool to install the application.  The
WDT needs elevated privileges to be able to instal the application
into IIS.


Q - Where do I report bugs?

A - In Product Studio - IIS OOB/AppGal/Tools/SIR.


Q - What if I have a question that isn't answered here?

A - Send it to wagt-sir@microsoft.com.







