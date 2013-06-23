using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using Arashi.Core.Util;
using Arashi.Services.File;
using Arashi.Services.Membership;
using Arashi.Web.Areas.Admin.Models;
using Arashi.Web.Mvc.Controllers;
using Arashi.Web.Mvc.Filters;
using Arashi.Web.Mvc.Models;
using log4net;

namespace Arashi.Web.Areas.Admin.Controllers
{
   /// <summary>
   /// Manage the comment moderation
   /// </summary>
   public class MediaManagerController : SecureControllerBase
   {
      #region Private Fields

      private static readonly ILog log = LogManager.GetLogger(typeof(MediaManagerController));
      private const int pageSize = 20;
      private readonly IFileService fileService;

      #endregion

      #region Constructor

      /// <summary>
      /// Controller Constructor
      /// </summary>
      /// <param name="fileService"></param>
      public MediaManagerController(IFileService fileService)
      {
         this.fileService = fileService;
      }

      #endregion

      /// <summary>
      /// Show the list of comments
      /// </summary>
      /// <param name="page"></param>
      /// <returns></returns>
      [PermissionFilter(RequiredRights = Rights.FilesView)]
      public ActionResult Index(int? page)
      {
         string mediaRoot = Context.ManagedSite.SiteDataPath + "media/";
         string mediaRootFullPath = this.ControllerContext.HttpContext.Server.MapPath(mediaRoot);

         // ensure the "media" folder exists
         fileService.EnsureDirectoryExists(mediaRootFullPath);

         // get all the files in the root
         IList<FileInfo> files = fileService.GetFiles(mediaRootFullPath);
         IList<MediaModel> model = new List<MediaModel>();

         foreach (FileInfo file in files)
         {
            MediaModel mediaModel = new MediaModel
            {
               Name = file.Name,
               MimeType = MimeTypes.GetMimeTypeName(file.Extension.Substring(1)),
               RelativePath = mediaRoot + file.Name,
               LastModifiedDate =  file.LastWriteTimeUtc
            };

            model.Add(mediaModel);
         }

         return View("Index", model);
      }




      /// <summary>
      /// Show the partial view for the media browser, used to offer a media list for selection
      /// </summary>
      /// <returns></returns>
      [PermissionFilter(RequiredRights = Rights.FilesView)]
      public ActionResult Browse()
      {
         string mediaRoot = Context.ManagedSite.SiteDataPath + "media/";
         string mediaRootFullPath = this.ControllerContext.HttpContext.Server.MapPath(mediaRoot);

         // ensure the "media" folder exists
         fileService.EnsureDirectoryExists(mediaRootFullPath);

         // get all the files in the root
         IList<FileInfo> files = fileService.GetFiles(mediaRootFullPath);
         IList<MediaModel> model = new List<MediaModel>();

         foreach (FileInfo file in files)
         {
            MediaModel mediaModel = new MediaModel
            {
               Name = file.Name,
               MimeType = MimeTypes.GetMimeTypeName(file.Extension.Substring(1)),
               RelativePath = mediaRoot + file.Name,
               LastModifiedDate = file.LastWriteTimeUtc
            };

            model.Add(mediaModel);
         }

         return View("~/Areas/Admin/Views/MediaManager/MediaBrowser.ascx", model);
      }




      /// <summary>
      /// Return a <see cref="FileResult"/> that map to a physical file on the file system
      /// </summary>
      /// <param name="name"></param>
      /// <returns></returns>
      public FileResult GetMedia(string name)
      {
         string mediaRoot = Context.ManagedSite.SiteDataPath + "media/";
         string mediaRootFullPath = this.ControllerContext.HttpContext.Server.MapPath(mediaRoot);

         string contentType = MimeTypes.GetMimeTypeName(name.Substring(name.LastIndexOf('.') + 1));

         return base.File(Path.Combine(mediaRootFullPath, name), contentType);
      }

      #region Upload

      /// <summary>
      /// Receive uploads of media files
      /// </summary>
      /// <returns></returns>
      [AcceptVerbs(HttpVerbs.Post)]
      public ActionResult Upload()
      {
         try
         {
            log.Debug("MediaManagerController.Upload: Start");

             string _tempExtension = "_temp";
             string _fileName;
             string _parameters;
             bool _lastChunk;
             bool _firstChunk;
             long _startByte;


            string mediaRoot = Context.ManagedSite.SiteDataPath + "media/";
            string mediaRootFullPath = this.ControllerContext.HttpContext.Server.MapPath(mediaRoot);

            // ensure the "media" folder exists
            fileService.EnsureDirectoryExists(mediaRootFullPath);


            _fileName = Request.QueryString["file"];
            _parameters = Request.QueryString["param"];
            _lastChunk = string.IsNullOrEmpty(Request.QueryString["last"]) ? true : bool.Parse(Request.QueryString["last"]);
            _firstChunk = string.IsNullOrEmpty(Request.QueryString["first"]) ? true : bool.Parse(Request.QueryString["first"]);
            _startByte = string.IsNullOrEmpty(Request.QueryString["offset"]) ? 0 : long.Parse(Request.QueryString["offset"]);


            string uploadFolder = mediaRootFullPath;
            string tempFileName = _fileName + _tempExtension;

            //Is it the first chunk? Prepare by deleting any existing files with the same name
            if (_firstChunk)
            {
               log.Debug("MediaManagerController.Upload: First chunk arrived");

               //Delete temp file
               if (System.IO.File.Exists(mediaRootFullPath + tempFileName))
                  System.IO.File.Delete(mediaRootFullPath + tempFileName);

               //Delete target file
               if (System.IO.File.Exists(mediaRootFullPath + _fileName))
                  System.IO.File.Delete(mediaRootFullPath + _fileName);

            }

            //Write the file
            log.DebugFormat("MediaManagerController.Upload: Write data to disk FOLDER: {0}", uploadFolder);

            using (FileStream fs = System.IO.File.Open(mediaRootFullPath + tempFileName, FileMode.Append))
            {
               fileService.SaveFileStream(Request.InputStream, fs);
               fs.Close();
            }

            log.Debug("MediaManagerController.Upload: Write data to disk SUCCESS");

            //Is it the last chunk? Then finish up...
            if (_lastChunk)
            {
               log.Debug("MediaManagerController.Upload: Last chunk arrived");

               // ensure that there is no files with the same name
               string uploadedFileFullPath = fileService.EnsureUniqueFileName(mediaRootFullPath + _fileName);

               //Rename file to original file
               System.IO.File.Move(mediaRootFullPath + tempFileName, mediaRootFullPath + _fileName);

               //Finish stuff....
               FinishedFileUpload(_fileName, _parameters);
            }

         }
         catch (Exception ex)
         {
            log.Error(ex.ToString());
            throw;
         }
         finally
         {
            log.Debug("MediaManagerController.Upload: End");
         }

         return new EmptyResult();
      }





      /// <summary>
      /// Do your own stuff here when the file is finished uploading
      /// </summary>
      /// <param name="fileName"></param>
      /// <param name="parameters"></param>
      protected virtual void FinishedFileUpload(string fileName, string parameters)
      {
      }

      #endregion


      /// <summary>
      /// Permanently delete a media file
      /// </summary>
      /// <param name="name"></param>
      /// <returns></returns>
      [AcceptVerbs(HttpVerbs.Post)]
      [PermissionFilter(RequiredRights = Rights.CommentsEdit)]
      public ActionResult Delete(string name)
      {
         string mediaRoot = Context.ManagedSite.SiteDataPath + "media/";
         string mediaRootFullPath = this.ControllerContext.HttpContext.Server.MapPath(mediaRoot);

         try
         {
            fileService.Delete(Path.Combine(mediaRootFullPath, name));

            MessageModel message = new MessageModel
            {
               Text = "The selected media file has been deleted!",
               Icon = MessageModel.MessageIcon.Info,
            };

            return View("MessageUserControl", message);
         }
         catch (Exception ex)
         {
            log.Error("MediaManagerController.Delete", ex);

            MessageModel message = new MessageModel
            {
               Text = GlobalResource("Message_GenericError"),
               Icon = MessageModel.MessageIcon.Alert,
            };

            return View("MessageUserControl", message);
         }

      }


   }
}
