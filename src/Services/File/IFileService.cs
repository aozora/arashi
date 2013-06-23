using System;
using System.Collections.Generic;
using System.IO;

namespace Arashi.Services.File
{
   /// <summary>
   /// This class has common methods to manage files & folders
   /// </summary>
   public interface IFileService
   {
      /// <summary>
      /// Check if a folder is writable
      /// </summary>
      /// <param name="physicalDirectory"></param>
      /// <returns></returns>
      bool CheckIfDirectoryIsWritable(string physicalDirectory);


      /// <summary>
      /// Create a folder if it not exists
      /// </summary>
      /// <param name="physicalDirectory"></param>
      void CreateDirectory(string physicalDirectory);



      /// <summary>
      /// Check if a folder exists, otherwise create it
      /// </summary>
      /// <param name="physicalDirectory"></param>
      void EnsureDirectoryExists(string physicalDirectory);



      /// <summary>
      /// Save the contents of the Stream to a file
      /// </summary>
      /// <param name="stream"></param>
      /// <param name="fs"></param>
      void SaveFileStream(Stream stream, FileStream fs);


      /// <summary>
      /// Permanently delete a file from the file system
      /// </summary>
      /// <param name="fullFilePath"></param>
      void Delete(string fullFilePath);



      /// <summary>
      /// Returns a list of <see cref="FileInfo"/> for all the files of a given folder
      /// </summary>
      /// <param name="path"></param>
      /// <returns></returns>
      IList<FileInfo> GetFiles(string path);



      /// <summary>
      /// Check the filesystem to ensure that the given file is unique, otherwise
      /// rename it automatically with a progressive integer (like Windows Explorer)
      /// </summary>
      /// <param name="fullPathName"></param>
      /// <returns></returns>
      string EnsureUniqueFileName(string fullPathName);
   }
}
