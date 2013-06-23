using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace Arashi.Web.Tools.WordPressThemeConverter
{
   /// <summary>
   /// Converter utility to import WordPress PHP file templates
   /// and convert it to Arashi format (ASP.NET MVC)
   /// </summary>
   public class Converter
   {
      private static readonly ILog log = LogManager.GetLogger(typeof(Converter));
      string[] specialFunctions = new string[] {"get_header();", 
                                                "get_sidebar();", 
                                                "get_footer();", 
                                                "get_search_form();", 
                                                "comments_template();" };

      private const string netFileTemplateStart = "<%@ Control Language=\"C#\" Inherits=\"Arashi.Web.Mvc.TemplateEngine.TemplateBase<Arashi.Web.Mvc.Models.TemplateContentModel>\" %>\r\n";
      private const string phpOpen = @"<?php";
      private const string phpClose = @"?>";

      private const string phpIf = @"<?php if";
      private const string phpIfElse = @"<?php else : ?>";
      private const string phpIfEnd = @"<?php endif; ?>";

      private const string phpWhile = @"<?php while";
      private const string phpWhileEnd = @"<?php endwhile; ?>";

      private string sourcePath;
      private string destinationPath;

      // Rules:
      // 1 - substitute php If, While
      // 2 -      "     php generic open/close, removing ending ";" and adding starting "="
      // 3 - collect & validate functions, and check for matches in WordPressCompatibility
      //     via reflection



      /// <summary>
      /// Converter
      /// </summary>
      /// <param name="sourcePath"></param>
      /// <param name="destinationPath"></param>
      public Converter(string sourcePath, string destinationPath)
      {
         this.sourcePath = sourcePath;
         this.destinationPath = destinationPath;
      }




      /// <summary>
      /// Start the conversion
      /// </summary>
      /// <returns>
      /// This method will returns a dictionary with a list of warning/errors, where the key is the 
      /// template file and a row number (in the format, i.e. "index.ascx:14")
      /// </returns>
      public IDictionary<string, string> DoConvert()
      {
         // LIMITATIONS:
         // - scan only the root folder, not the subfolders
         //

         log.Debug("Converter.DoConvert: Start");

         IDictionary<string, string> convertLog = new Dictionary<string, string>();
         DirectoryInfo di = new DirectoryInfo(sourcePath);
         FileInfo[] phpSources = di.GetFiles("*.php");

         // parse each php file template
         foreach (FileInfo fileInfo in phpSources)
         {
            log.InfoFormat("Processing source file: {0}", fileInfo.FullName);

            StringBuilder php = new StringBuilder(File.ReadAllText(fileInfo.FullName));



            // 1 - insert dotnet control line
            php.Insert(0, netFileTemplateStart);

            // 1b - substitute double quotes that delimites server tags; php server tags are preserved
            php.Replace("\"<?", "'<?");
            php.Replace("?>\"", "?>'");


            // 2 - substitute php if
            php.Replace(phpIf, "<% if");
            php.Replace(phpIfElse, "<% } else { %>");
            php.Replace(phpIfEnd, "<% } %>");

            // 3 - substitute php while
            php.Replace(phpWhile, "<% foreach"); // TODO: URGENT: fix missing "{"
            php.Replace(phpWhileEnd, "<% } %>");

            foreach (string specialFunction in specialFunctions)
            {
               
            }

            // 5 - replace all generic php tags
            php.Replace(phpOpen, "<%=");
            php.Replace(phpClose, "%>");


            // Write the converted file in the destination
            string outputPath = Path.Combine(destinationPath, fileInfo.Name.Replace(".php", ".ascx"));
            log.InfoFormat("Writing output file: {0}", outputPath);
            
            File.WriteAllText(outputPath, php.ToString());

            log.InfoFormat("End Processing source file: {0}", fileInfo.FullName);
         }




         log.Debug("Converter.DoConvert: End");

         return convertLog;
      }

   }
}
