using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Arashi.Core.Util;
using log4net;

namespace Arashi.Services.File
{
   /// <summary>
   /// This class has common methods to manage files & folders
   /// </summary>
   public class FileService : IFileService
   {
      private static readonly ILog log = LogManager.GetLogger(typeof(FileService));


      #region IFileService Members

      /// <summary>
      /// Check if a folder is writable
      /// </summary>
      /// <param name="physicalDirectory"></param>
      /// <returns></returns>
      public bool CheckIfDirectoryIsWritable(string physicalDirectory)
      {
         try
         {
            bool isWritable = IOUtil.CheckIfDirectoryIsWritable(physicalDirectory);

            if (!isWritable)
               log.WarnFormat("Checking access to physical directory {0} resulted in no access.", physicalDirectory);

            return isWritable;
         }
         catch (Exception ex)
         {
            log.ErrorFormat("An unexpected error occurred while checking write access for the directory {0}.", physicalDirectory);
            log.Error(ex.ToString());
            throw;
         }
      }



      /// <summary>
      /// Create a folder if it not exists
      /// </summary>
      /// <param name="physicalDirectory"></param>
      public void CreateDirectory(string physicalDirectory)
      {
         if (!Directory.Exists(physicalDirectory))
            Directory.CreateDirectory(physicalDirectory);
      }



      /// <summary>
      /// Check if a folder exists, otherwise create it
      /// </summary>
      /// <param name="physicalDirectory"></param>
      public void EnsureDirectoryExists(string physicalDirectory)
      {
         CreateDirectory(physicalDirectory);
      }



      /// <summary>
      /// Returns a list of <see cref="FileInfo"/> for all the files of a given folder
      /// </summary>
      /// <param name="path"></param>
      /// <returns></returns>
      public IList<FileInfo> GetFiles(string path)
      {
         DirectoryInfo dirInfo = new DirectoryInfo(path);
         FileInfo[] files = dirInfo.GetFiles();

         return files.ToList();
      }



      /// <summary>
      /// Save the contents of the Stream to a file
      /// </summary>
      /// <param name="stream"></param>
      /// <param name="fs"></param>
      public void SaveFileStream(Stream stream, FileStream fs)
      {
         byte[] buffer = new byte[4096];
         int bytesRead;

         while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
         {
            fs.Write(buffer, 0, bytesRead);
         }
      }




      /// <summary>
      /// Permanently delete a file from the file system
      /// </summary>
      /// <param name="fullFilePath"></param>
      public void Delete(string fullFilePath)
      {
         System.IO.File.Delete(fullFilePath);
      }




      /// <summary>
      /// Check the filesystem to ensure that the given file is unique, otherwise
      /// rename it automatically with a progressive integer (like Windows Explorer)
      /// </summary>
      /// <param name="fullPathName"></param>
      /// <returns></returns>
      public string EnsureUniqueFileName(string fullPathName)
      {
         if (!System.IO.File.Exists(fullPathName))
            return fullPathName;

         return UniqueFileName(fullPathName);
      }



      private string UniqueFileName(string fullPathName)
      {
         string newFileName = string.Empty;
         int i = 0;
         int dotPosition;

         while (System.IO.File.Exists(fullPathName))
         {
            dotPosition = fullPathName.LastIndexOf(".");
            dotPosition = dotPosition == -1 ? fullPathName.Length - 1 : dotPosition;

            newFileName = fullPathName.Insert(dotPosition, " (" + (++i).ToString() + ")");
         }

         return newFileName;
      }



      #endregion
   }
}
