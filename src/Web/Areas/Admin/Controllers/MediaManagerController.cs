namespace Arashi.Web.Areas.Admin.Controllers
{
   using System;
   using System.Collections;
   using System.Collections.Generic;
   using System.Drawing;
   using System.IO;
   using System.Linq;
   using System.Web;
   using System.Web.Helpers;
   using System.Web.Mvc;
   using System.Web.Routing;
   using System.Web.UI.WebControls;

   using Arashi.Core.Domain;
   using Arashi.Core.Extensions;
   using Arashi.Core.Util;
   using Arashi.Services.File;
   using Arashi.Services.Membership;
   using Arashi.Web.Areas.Admin.Models;
   using Arashi.Web.Mvc;
   using Arashi.Web.Mvc.Controllers;
   using Arashi.Web.Mvc.Filters;
   using Arashi.Web.Mvc.Models;
   using Common.Logging;

   using Arashi.Services.Localization;
   using Arashi.Services.SiteStructure;

   using Image = System.Drawing.Image;



   /// <summary>
   /// Manage the comment moderation
   /// </summary>
   public class MediaManagerController : SecureControllerBase
   {
      #region Private Fields

      private readonly ILog log;
      private const int pageSize = 20;
      private readonly IFileService fileService;

      #endregion

      #region Constructor

      /// <summary>
      /// Controller Constructor
      /// </summary>
      /// <param name="fileService"></param>
      public MediaManagerController(ILog log, IFileService fileService, ILocalizationService localizationService, IUserService userService, ISiteService siteService)
         : base(log, localizationService, userService, siteService)
      {
         this.log = log;
         this.fileService = fileService;
      }

      #endregion

      #region Media Manager

      /// <summary>
      /// Show the list of comments
      /// </summary>
      /// <param name="page"></param>
      /// <param name="s">Search pattern</param>
      /// <returns></returns>
      [PermissionFilter(RequiredRights = Rights.FilesView)]
      public ActionResult Index(int? page, string s)
      {
         string mediaRoot = Context.ManagedSite.SiteDataPath + "media/";
         string mediaRootFullPath = this.ControllerContext.HttpContext.Server.MapPath(mediaRoot);

         // ensure the "media" folder exists
         fileService.EnsureDirectoryExists(mediaRootFullPath);

         // get all the files in the root
         IList<FileInfo> files = fileService.GetFiles(mediaRootFullPath);
         MediaManagerModel model = new MediaManagerModel();

         if (!string.IsNullOrEmpty(s))
            files = files.Where(f => f.Name.Contains(s)).ToList();

         model.CurrentSearchPattern = s;
         model.PageSize = pageSize;
         model.CurrentPageIndex = page.HasValue ? page.Value : 1;
         model.TotalRecordCount = files.Count;

         model.Medias = (from file in files
                         select new MediaModel()
                           {
                              Name = file.Name,
                              MimeType = MimeTypes.GetMimeTypeName(string.IsNullOrEmpty(file.Extension) ? string.Empty : file.Extension.Substring(1)),
                              RelativePath = mediaRoot + file.Name,
                              LastModifiedDate =  file.LastWriteTimeUtc,
                              IsAudio = MimeTypes.IsAudio(string.IsNullOrEmpty(file.Extension) ? string.Empty : file.Extension.Substring(1)),
                              IsImage = MimeTypes.IsImage(string.IsNullOrEmpty(file.Extension) ? string.Empty : file.Extension.Substring(1)),
                              IsMovie = MimeTypes.IsMovie(string.IsNullOrEmpty(file.Extension) ? string.Empty : file.Extension.Substring(1))
                           }
                        ).Skip((model.CurrentPageIndex - 1) * model.PageSize)
                         .Take(model.PageSize);

         return View("Index", model);
      }



      public JsonResult GetMediaData()
      {
         string mediaRoot = Context.ManagedSite.SiteDataPath + "media/";
         string mediaRootFullPath = this.ControllerContext.HttpContext.Server.MapPath(mediaRoot);

         // ensure the "media" folder exists
         fileService.EnsureDirectoryExists(mediaRootFullPath);

         // get all the files in the root
         IList<FileInfo> files = fileService.GetFiles(mediaRootFullPath);
         IList<MediaModel> list = new List<MediaModel>();

         foreach (FileInfo file in files)
         {
            MediaModel mediaModel = new MediaModel
            {
               Name = file.Name,
               MimeType = MimeTypes.GetMimeTypeName(string.IsNullOrEmpty(file.Extension) ? string.Empty : file.Extension.Substring(1)),
               RelativePath = mediaRoot + file.Name,
               LastModifiedDate = file.LastWriteTimeUtc
            };

            list.Add(mediaModel);
         }


         var data = (from m in list
                    select new
                    {
                       img = this.GetImgElement(m.Name),
                       file = m.Name, 
                       date = m.LastModifiedDate.AdjustDateToTimeZone(Context.CurrentUser.TimeZone).ToShortDateString()
                    }
                    ).ToArray();

         //var jsonData = new
         //{
         //   data = (from m in list
         //           select new
         //           {
         //              img = m.RelativePath,
         //              file = m.Name, 
         //              date = m.LastModifiedDate.AdjustDateToTimeZone(Context.CurrentUser.TimeZone).ToShortDateString()
         //           }
         //           ).ToArray()
         //};

         return Json(data, JsonRequestBehavior.AllowGet);
      }



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

      #endregion

      #region Media Browser Control

      /// <summary>
      /// Show the partial view for the media browser, used to offer a media list for selection
      /// </summary>
      /// <returns></returns>
      [PermissionFilter(RequiredRights = Rights.FilesView)]
      public ActionResult Browse(int? page, string s)
      {
         //string mediaRoot = Context.ManagedSite.SiteDataPath + "media/";
         //string mediaRootFullPath = this.ControllerContext.HttpContext.Server.MapPath(mediaRoot);

         //// ensure the "media" folder exists
         //fileService.EnsureDirectoryExists(mediaRootFullPath);

         //// get all the files in the root
         //IList<FileInfo> files = fileService.GetFiles(mediaRootFullPath);
         //IList<MediaModel> model = new List<MediaModel>();

         //foreach (FileInfo file in files)
         //{
         //   MediaModel mediaModel = new MediaModel
         //   {
         //      Name = file.Name,
         //      MimeType = MimeTypes.GetMimeTypeName(file.Extension.Substring(1)),
         //      RelativePath = mediaRoot + file.Name,
         //      LastModifiedDate = file.LastWriteTimeUtc
         //   };

         //   model.Add(mediaModel);
         //}

         string mediaRoot = Context.ManagedSite.SiteDataPath + "media/";
         string mediaRootFullPath = this.ControllerContext.HttpContext.Server.MapPath(mediaRoot);

         // ensure the "media" folder exists
         fileService.EnsureDirectoryExists(mediaRootFullPath);

         // get all the files in the root
         IList<FileInfo> files = fileService.GetFiles(mediaRootFullPath);
         MediaManagerModel model = new MediaManagerModel();

         if (!string.IsNullOrEmpty(s))
            files = files.Where(f => f.Name.Contains(s)).ToList();

         model.CurrentSearchPattern = s;
         model.PageSize = pageSize;
         model.CurrentPageIndex = page.HasValue ? page.Value : 1;
         model.TotalRecordCount = files.Count;

         model.Medias = (from file in files
                         select new MediaModel()
                         {
                            Name = file.Name,
                            MimeType = MimeTypes.GetMimeTypeName(string.IsNullOrEmpty(file.Extension) ? string.Empty : file.Extension.Substring(1)),
                            RelativePath = mediaRoot + file.Name,
                            LastModifiedDate = file.LastWriteTimeUtc,
                            IsAudio = MimeTypes.IsAudio(string.IsNullOrEmpty(file.Extension) ? string.Empty : file.Extension.Substring(1)),
                            IsImage = MimeTypes.IsImage(string.IsNullOrEmpty(file.Extension) ? string.Empty : file.Extension.Substring(1)),
                            IsMovie = MimeTypes.IsMovie(string.IsNullOrEmpty(file.Extension) ? string.Empty : file.Extension.Substring(1))
                         }
                        ).Skip((model.CurrentPageIndex - 1) * model.PageSize)
                         .Take(model.PageSize);

         return PartialView("~/Areas/Admin/Views/MediaManager/MediaBrowser.cshtml", model);
      }

      #endregion

      #region Upload

      /// <summary>
      /// Show the upload page
      /// </summary>
      /// <returns></returns>
      [AcceptVerbs(HttpVerbs.Get)]
      public ActionResult Upload()
      {
         return View("~/Areas/Admin/Views/MediaManager/Upload.cshtml");
      }



      /// <summary>
      /// Receive uploads of media files
      /// </summary>
      /// <returns></returns>
      [AcceptVerbs(HttpVerbs.Post)]
      public ActionResult Upload(int? chunk, string name)
      {
         try
         {
            log.Debug("MediaManagerController.Upload: Start");

            string mediaRoot = Context.ManagedSite.SiteDataPath + "media/";
            string mediaRootFullPath = this.ControllerContext.HttpContext.Server.MapPath(mediaRoot);

            // ensure the "media" folder exists
            fileService.EnsureDirectoryExists(mediaRootFullPath);

            // ensure that there is no files with the same name
            string uploadedFileFullPath = fileService.EnsureUniqueFileName(Path.Combine(mediaRootFullPath, name));

            chunk = chunk ?? 0;

            log.DebugFormat("MediaManagerController.Upload: chunks #{0}", Request["chunks"]);

            //Write the file
            log.DebugFormat("MediaManagerController.Upload: Write data to disk FOLDER: {0}", mediaRootFullPath);

            //Is it the first chunk? Prepare by deleting any existing files with the same name
            if (chunk == 0)
            {
               log.Debug("MediaManagerController.Upload: First chunk arrived");

               //Delete target file
               if (System.IO.File.Exists(uploadedFileFullPath))
                  System.IO.File.Delete(uploadedFileFullPath);
            }

            using (FileStream fs = System.IO.File.Open(uploadedFileFullPath, chunk == 0 ? FileMode.Create : FileMode.Append))
            {
               // Use "Request.Files[0].InputStream" instead of "Request.InputStream" else all the multipart headers and sections are written to the file.
               fileService.SaveFileStream(Request.Files[0].InputStream, fs);
               fs.Close();
            }

            log.Debug("MediaManagerController.Upload: Write data to disk SUCCESS");

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

      #region Get Media Img Element

      // TODO: refactor the following functions, they are a duplicate of the same in MediaExtension

      public string GetImgElement(string name)
      {
         string img = "<img width='{0}' height='{1}' title='{2}' alt='' class='attachment-80x60' src='{3}' />";

         // if the file is an image, than create a html img with the image itself
         if (ImageHelper.IsImage(name))
         {
            //UrlHelper urlHelper = new UrlHelper(ControllerContext);
            //string src = urlHelper.Action("GetMedia", "MediaManager", new { name = name });
            string src = UrlHelper.GenerateUrl(null, "GetMedia", "MediaManager", new RouteValueDictionary(new { name = name }), RouteTable.Routes, this.ControllerContext.RequestContext, true);
            //return GenerateUrl(routeName, actionName, controllerName, routeValues, this.RouteCollection, this.RequestContext, true);

            try
            {
               Size size = GetScaledImageSize(name, 48, 48);
               return string.Format(img, size.Width, size.Height, name, src);
            }
            catch (Exception ex)
            {
               log.Error(ex.ToString());
               return string.Format(img, "48", "48", name, "/Resources/img/48x48/file.png");
            }

         }
         else // otherwise use a default image for each file type
         {
            string extension = name.Substring(name.LastIndexOf('.') + 1);
            string noImageName;

            switch (extension)
            {
               case "doc":
               case "docx":
                  noImageName = "doc.png";
                  break;
               case "xls":
               case "xlsx":
                  noImageName = "dkspread_kspoc.png";
                  break;
               case "ppt":
               case "pptx":
               case "pps":
                  noImageName = "pps.png";
                  break;
               case "pdf":
                  noImageName = "pdf.png";
                  break;
               case "txt":
                  noImageName = "txt.png";
                  break;
               case "swf":
                  noImageName = "swf.png";
                  break;
               case "htm":
               case "html":
                  noImageName = "html.png";
                  break;
               case "zip":
               case "7z":
               case "rar":
                  noImageName = "tar.png";
                  break;
               case "mov":
               case "avi":
               case "mpg":
               case "mkv":
                  noImageName = "video.png";
                  break;
               default:
                  noImageName = "file.png";
                  break;
            }

            return string.Format(img, "48", "48", name, "/Resources/img/48x48/" + noImageName);
         }

      }



      /// <summary>
      /// Returns a System.Drawing.Size object with the scaled size of a image
      /// </summary>
      /// <param name="helper"></param>
      /// <param name="fileName"></param>
      /// <param name="maxWidth"></param>
      /// <param name="maxHeight"></param>
      /// <returns></returns>
      private Size GetScaledImageSize(string fileName, int maxWidth, int maxHeight)
      {
         //IRequestContext context = helper.ViewData["Context"] as IRequestContext;

         //if (context != null)
         //{
            string mediaRoot = Context.ManagedSite.SiteDataPath + "media/";
            string mediaRootFullPath = ControllerContext.HttpContext.Server.MapPath(mediaRoot);

            return ImageHelper.ScaleImage(Path.Combine(mediaRootFullPath, fileName), maxWidth, maxHeight);
         //}

         //throw new ApplicationException("MediaExtensions.GetScaledImageSize: IRequestContext cannot be null");
      }

      #endregion

      #region Media Edit

      /// <summary>
      /// Show the edit media page
      /// </summary>
      /// <param name="name"></param>
      /// <returns></returns>
      [PermissionFilter(RequiredRights = Rights.FilesView)]
      public ActionResult Edit(string name)
      {
         string mediaRoot = Context.ManagedSite.SiteDataPath + "media/";
         string mediaRootFullPath = this.ControllerContext.HttpContext.Server.MapPath(mediaRoot);

         FileInfo file = fileService.GetFile(Path.Combine(mediaRootFullPath, name));

         MediaModel model = new MediaModel()
         {
            Name = file.Name,
            MimeType = MimeTypes.GetMimeTypeName(string.IsNullOrEmpty(file.Extension) ? string.Empty : file.Extension.Substring(1)),
            RelativePath = mediaRoot + file.Name,
            LastModifiedDate = file.LastWriteTimeUtc,
            IsAudio = MimeTypes.IsAudio(string.IsNullOrEmpty(file.Extension) ? string.Empty : file.Extension.Substring(1)),
            IsImage = MimeTypes.IsImage(string.IsNullOrEmpty(file.Extension) ? string.Empty : file.Extension.Substring(1)),
            IsMovie = MimeTypes.IsMovie(string.IsNullOrEmpty(file.Extension) ? string.Empty : file.Extension.Substring(1))
         };
         
         // Image size info
         Size originalSize = ImageHelper.GetSize(file.FullName);
         ViewBag.OriginalWidth = originalSize.Width;
         ViewBag.OriginalHeight = originalSize.Height;


         // ViewBag for effect properties
         var installedFonts = new System.Drawing.Text.InstalledFontCollection();
         IEnumerable<SelectListItem> fonts = installedFonts.Families.Select(f => new SelectListItem() { Text = f.Name, Value = f.Name, Selected = (f.Name.ToLower() == "arial" ? true : false) });
         ViewBag.Fonts = fonts;

         IEnumerable<SelectListItem> styles = Enum.GetNames(typeof(FontStyle)).Select(s => new SelectListItem() { Text = s, Value = s, Selected = (s.ToLower() == "regular" ? true : false) });
         ViewBag.Styles = styles;

         IEnumerable<SelectListItem> colorNames = Enum.GetNames(typeof(KnownColor)).Select(s => new SelectListItem() { Text = s, Value = s, Selected = (s.ToLower() == "black" ? true : false) }).OrderBy(s => s.Text);
         ViewBag.ColorNames = colorNames;

         IEnumerable<SelectListItem> horizontalAlign = Enum.GetNames(typeof(HorizontalAlign)).Select(s => new SelectListItem() { Text = s, Value = s, Selected = (s.ToLower() == "center" ? true : false) });
         ViewBag.HorizontalAlign = horizontalAlign;

         IEnumerable<SelectListItem> verticalAlign = Enum.GetNames(typeof(VerticalAlign)).Select(s => new SelectListItem() { Text = s, Value = s, Selected = (s.ToLower() == "middle" ? true : false) });
         ViewBag.VerticalAlign = verticalAlign;
         

         return View("Edit", model);
      }

      #region Resize

      /// <summary>
      /// Resize an image
      /// </summary>
      /// <param name="name"></param>
      /// <param name="resizewidth"></param>
      /// <param name="resizeheight"></param>
      /// <returns></returns>
      [AcceptVerbs(HttpVerbs.Post)]
      [ValidateAntiForgeryToken(Salt = "resize")]
      [PermissionFilter(RequiredRights = Rights.FilesEdit)]
      public ActionResult Resize(string name, int resizewidth, int resizeheight)
      {
         ViewBag.SiteId = Context.ManagedSite.SiteId;
         ViewBag.Name = name;
         ViewBag.Width = resizewidth;
         ViewBag.Height = resizeheight;

         return this.PartialView("Resized");
      }



      /// <summary>
      /// Render a resized image
      /// </summary>
      /// <param name="name"></param>
      /// <param name="resizewidth"></param>
      /// <param name="resizeheight"></param>
      /// <returns></returns>
      public ImageResult GetResized(string name, int resizewidth, int resizeheight)
      {
         string mediaRootFullPath = this.ControllerContext.HttpContext.Server.MapPath(Context.ManagedSite.SiteDataPath + "media/");
         string imageFullPath = Path.Combine(mediaRootFullPath, name);

         using (Image source = Image.FromFile(imageFullPath))
         {
            Bitmap resizedImage = ImageHelper.CreateThumbnail(source as Bitmap, resizewidth, resizeheight, true);

            ImageResult result = new ImageResult()
            {
               OriginalFullFilePath = imageFullPath,
               Source = resizedImage
            };

            return result;
         }
      }

      #endregion

      #region Rotate

      /// <summary>
      /// Rotate Partial Action
      /// </summary>
      /// <param name="name"></param>
      /// <param name="rotate"></param>
      /// <returns></returns>
      [AcceptVerbs(HttpVerbs.Post)]
      [ValidateAntiForgeryToken(Salt = "rotate")]
      [PermissionFilter(RequiredRights = Rights.FilesEdit)]
      public ActionResult Rotate(string name, string rotate)
      {
         ViewBag.SiteId = Context.ManagedSite.SiteId;
         ViewBag.Name = name;
         ViewBag.Action = string.Concat("GetRotate", rotate.Capitalize()); // GetRotateLeft or GetRotateRight
         return this.PartialView("RotateFlip");
      }




      public ImageResult GetRotateRight(string name)
      {
         string mediaRootFullPath = this.ControllerContext.HttpContext.Server.MapPath(Context.ManagedSite.SiteDataPath + "media/");
         string imageFullPath = Path.Combine(mediaRootFullPath, name);

         using (Image source = Image.FromFile(imageFullPath))
         {
            Bitmap rotatedImage = ImageHelper.RotateImage(source as Bitmap, -90);

            ImageResult result = new ImageResult()
            {
               OriginalFullFilePath = imageFullPath,
               Source = rotatedImage
            };

            return result;
         }
         //new WebImage(imageFullPath)
         //    .RotateRight()
         //    .Write();
      }


      public ImageResult GetRotateLeft(string name)
      {
         string mediaRootFullPath = this.ControllerContext.HttpContext.Server.MapPath(Context.ManagedSite.SiteDataPath + "media/");
         string imageFullPath = Path.Combine(mediaRootFullPath, name);

         using (Image source = Image.FromFile(imageFullPath))
         {
            Bitmap rotatedImage = ImageHelper.RotateImage(source as Bitmap, 90);

            ImageResult result = new ImageResult()
            {
               OriginalFullFilePath = imageFullPath,
               Source = rotatedImage
            };

            return result;
         }
         //new WebImage(imageFullPath)
         //    .RotateLeft()
         //    .Write();
      }


      #endregion

      #region Flip

      /// <summary>
      /// Flip action
      /// </summary>
      /// <param name="name"></param>
      /// <param name="flip"></param>
      /// <returns></returns>
      [AcceptVerbs(HttpVerbs.Post)]
      [ValidateAntiForgeryToken(Salt = "flip")]
      [PermissionFilter(RequiredRights = Rights.FilesEdit)]
      public ActionResult Flip(string name, string flip)
      {
         ViewBag.SiteId = Context.ManagedSite.SiteId;
         ViewBag.Name = name;
         ViewBag.Action = (flip == "h" ? "GetFlipHorizontal" : "GetFlipVertical");
         return this.PartialView("RotateFlip");
      }



      public ImageResult GetFlipHorizontal(string name)
      {
         string mediaRootFullPath = this.ControllerContext.HttpContext.Server.MapPath(Context.ManagedSite.SiteDataPath + "media/");
         string imageFullPath = Path.Combine(mediaRootFullPath, name);

         using (Image source = Image.FromFile(imageFullPath))
         {
            source.RotateFlip(RotateFlipType.RotateNoneFlipX);
            Image flippedImage = source.Clone() as Image;

            ImageResult result = new ImageResult()
            {
               OriginalFullFilePath = imageFullPath,
               Source = flippedImage
            };

            return result;
         }
      }



      public ImageResult GetFlipVertical(string name)
      {
         string mediaRootFullPath = this.ControllerContext.HttpContext.Server.MapPath(Context.ManagedSite.SiteDataPath + "media/");
         string imageFullPath = Path.Combine(mediaRootFullPath, name);

         using (Image source = Image.FromFile(imageFullPath))
         {
            source.RotateFlip(RotateFlipType.RotateNoneFlipY);
            Image flippedImage = source.Clone() as Image;

            ImageResult result = new ImageResult()
            {
               OriginalFullFilePath = imageFullPath,
               Source = flippedImage
            };

            return result;
         }
      }

      #endregion

      #region Text Watermak

      /// <summary>
      /// Text Watermak
      /// </summary>
      /// <param name="name"></param>
      /// <param name="text"></param>
      /// <param name="color"></param>
      /// <param name="font"></param>
      /// <param name="size"></param>
      /// <param name="style"></param>
      /// <param name="halign"></param>
      /// <param name="valign"></param>
      /// <returns></returns>
      [AcceptVerbs(HttpVerbs.Post)]
      [ValidateAntiForgeryToken(Salt = "textwatermark")]
      [PermissionFilter(RequiredRights = Rights.FilesEdit)]
      public ActionResult TextWatermark(string name, string color, string text, string font, string size, string style, string halign, string valign)
      {
         ViewBag.SiteId = Context.ManagedSite.SiteId;
         ViewBag.Name = name;
         ViewBag.Color = color;
         ViewBag.Text = text;
         ViewBag.Font = font;
         ViewBag.Size = size;
         ViewBag.Style = style;
         ViewBag.HorizontalAlign = halign;
         ViewBag.VerticalAlign = valign;
         return this.PartialView("TextWatermark");
      }



      /// <summary>
      /// Render the image with the text watermark
      /// </summary>
      /// <param name="name"></param>
      /// <param name="color"></param>
      /// <param name="text"></param>
      /// <param name="font"></param>
      /// <param name="size"></param>
      /// <param name="style"></param>
      /// <param name="halign"></param>
      /// <param name="valign"></param>
      public void GetTextWatermark(string name, string color, string text, string font, string size, string style, string halign, string valign)
      {
         string mediaRootFullPath = this.ControllerContext.HttpContext.Server.MapPath(Context.ManagedSite.SiteDataPath + "media/");
         string imageFullPath = Path.Combine(mediaRootFullPath, name);

         new WebImage(imageFullPath)
               .AddTextWatermark(text, color, Int32.Parse(size), style, font, halign, valign)
               .Write();
      }

      #endregion

      #region Crop

      /// <summary>
      /// Crop action
      /// </summary>
      /// <param name="name"></param>
      /// <param name="x"></param>
      /// <param name="y"></param>
      /// <param name="width"></param>
      /// <param name="height"></param>
      /// <returns></returns>
      [AcceptVerbs(HttpVerbs.Post)]
      [ValidateAntiForgeryToken(Salt = "crop")]
      [PermissionFilter(RequiredRights = Rights.FilesEdit)]
      public ActionResult Crop(string name, int croptop, int cropleft, int cropwidth, int cropheight)
      {
         ViewBag.SiteId = Context.ManagedSite.SiteId;
         ViewBag.Name = name;
         ViewBag.X = cropleft;
         ViewBag.Y = croptop;
         ViewBag.Width = cropwidth;
         ViewBag.Height = cropheight;

         return this.PartialView("Crop");
      }



      public ImageResult GetCrop(string name, int croptop, int cropleft, int cropwidth, int cropheight)
      {
         string mediaRootFullPath = this.ControllerContext.HttpContext.Server.MapPath(Context.ManagedSite.SiteDataPath + "media/");
         string imageFullPath = Path.Combine(mediaRootFullPath, name);

         using (Image source = Image.FromFile(imageFullPath))
         {
            Rectangle cropArea = new Rectangle(cropleft, croptop, cropwidth, cropheight);

            Bitmap bmpImage = new Bitmap(source);
            Bitmap bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);

            ImageResult result = new ImageResult()
            {
               OriginalFullFilePath = imageFullPath,
               Source = bmpCrop
            };

            return result;
         }
      }

      #endregion


      /// <summary>
      /// Save the changes 
      /// </summary>
      /// <returns></returns>
      [AcceptVerbs(HttpVerbs.Post)]
      [ValidateAntiForgeryToken(Salt = "save")]
      [PermissionFilter(RequiredRights = Rights.FilesEdit)]
      public ActionResult Save(string name, string[] history)
      {
         log.Debug("MediaManagerController.Save - Start");

         string mediaRootFullPath = this.ControllerContext.HttpContext.Server.MapPath(Context.ManagedSite.SiteDataPath + "media/");
         string imageFullPath = Path.Combine(mediaRootFullPath, name);
         Bitmap source;

         // clone the original bitmap: this prevent I/O error for saving due to locks of System.Drawing
         using (Image original = Image.FromFile(imageFullPath))
         {
            source = original.Clone() as Bitmap;
         }

         // add "restore original"

         try
         {
            // apply transformation to the source image
            for (int index = 0; index < history.Length; index++)
            {
               log.DebugFormat("MediaManagerController.Save - transformation: {0}", history[index]);

               string v = string.Concat(this.ControllerContext.HttpContext.Request.Url.GetLeftPart(UriPartial.Authority).ToString(), history[index]);
               UriBuilder uriBuilder = new UriBuilder(v);
               IEnumerable<KeyValuePair<string, string>> queryParams = uriBuilder.GetQueryParams();

               if (history[index].IndexOf("GetResized") > -1)
               {
                  int resizewidth = Convert.ToInt32(queryParams.Where(q => q.Key == "resizewidth").Select(q => q.Value).First());
                  int resizeheight = Convert.ToInt32(queryParams.Where(q => q.Key == "resizeheight").Select(q => q.Value).First());
                  source = ImageHelper.CreateThumbnail(source, resizewidth, resizeheight, true);
               }
               else if (history[index].IndexOf("GetRotateLeft") > -1)
               {
                  source = ImageHelper.RotateImage(source, 90);
               }
               else if (history[index].IndexOf("GetRotateRight") > -1)
               {
                  source = ImageHelper.RotateImage(source, -90);
               }
               else if (history[index].IndexOf("GetFlipHorizontal") > -1)
               {
                  source.RotateFlip(RotateFlipType.RotateNoneFlipX);
               }
               else if (history[index].IndexOf("GetFlipVertical") > -1)
               {
                  source.RotateFlip(RotateFlipType.RotateNoneFlipY);
               }
               else if (history[index].IndexOf("GetCrop") > -1)
               {
                  int cropleft = Convert.ToInt32(queryParams.Where(q => q.Key == "cropleft").Select(q => q.Value).First());
                  int croptop = Convert.ToInt32(queryParams.Where(q => q.Key == "croptop").Select(q => q.Value).First());
                  int cropwidth = Convert.ToInt32(queryParams.Where(q => q.Key == "cropwidth").Select(q => q.Value).First());
                  int cropheight = Convert.ToInt32(queryParams.Where(q => q.Key == "cropheight").Select(q => q.Value).First());

                  Rectangle cropArea = new Rectangle(cropleft, croptop, cropwidth, cropheight);
                  source = source.Clone(cropArea, source.PixelFormat);
               }
               else if (history[index].IndexOf("GetTextWatermark") > -1)
               {
                  string text = queryParams.Where(q => q.Key == "text").Select(q => q.Value).First();
                  string color = queryParams.Where(q => q.Key == "color").Select(q => q.Value).First();
                  string size = queryParams.Where(q => q.Key == "size").Select(q => q.Value).First();
                  string style = queryParams.Where(q => q.Key == "style").Select(q => q.Value).First();
                  string font = queryParams.Where(q => q.Key == "font").Select(q => q.Value).First();
                  string halign = queryParams.Where(q => q.Key == "halign").Select(q => q.Value).First();
                  string valign = queryParams.Where(q => q.Key == "valign").Select(q => q.Value).First();

                  WebImage wi = new WebImage(imageFullPath).AddTextWatermark(text, color, Int32.Parse(size), style, font, halign, valign);

                  MemoryStream ms = new MemoryStream(wi.GetBytes(wi.ImageFormat));
                  source = Image.FromStream(ms) as Bitmap;
               }
            }

            // now the source object is the final transformed image
            // WARNING: this overwrite the original
            //          (make backup? --> system settings?)
            source.Save(imageFullPath);

            MessageModel message = new MessageModel
            {
               Text = "Image saved",
               Icon = MessageModel.MessageIcon.Info,
            };
            RegisterMessage(message, true);


            return RedirectToAction("Index");
         }
         catch (Exception ex)
         {
            log.Error(ex.ToString());

            MessageModel message = new MessageModel
            {
               Text = GlobalResource("Message_GenericError"),
               Icon = MessageModel.MessageIcon.Alert,
            };
            RegisterMessage(message, true);

            return RedirectToAction("Edit", new RouteValueDictionary() { { "name", name } });
         }
         finally
         {
            log.Debug("MediaManagerController.Save - End");
         }

      }

      #endregion
   }
}
