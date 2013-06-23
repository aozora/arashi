using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Arashi.Web.Areas.Admin.Models
{
   /// <summary>
   /// Model for the upload
   /// </summary>
   public class UploadModel
   {
      #region Public Properties

      /// <summary>
      /// File size in KBs
      /// </summary>
      public int MaxFileSizeKB {get; set;}

      /// <summary>
      /// Maximum number of simultaneous uploads
      /// </summary>
      public int MaxUploads {get; set;}

      /// <summary>
      /// File filter, for example ony jpeg use: FileFilter=Jpeg (*.jpg) |*.jpg
      /// </summary>
      public string FileFilter {get; set;}

      /// <summary>
      /// Your custom parameter
      /// </summary>
      public string CustomParam {get; set;}
      
      /// <summary>
      /// The default color for the control, for example: LightBlue
      /// </summary>
      public string DefaultColor {get; set;}

      /// <summary>
      /// If true, it will use the HttpUploadHandler.ashx to upload the file.
      /// Since version 3 this is the default. If set to false, it will use the WCF service
      /// </summary>
      public bool HttpUploader {get; set;}

      /// <summary>
      /// Custom specified name of the HttpUploadHandler, 
      /// for example this can be "PHPUpload.php" to use the PHP upload handler.
      /// </summary>
      public string UploadHandlerName {get; set;}

      /// <summary>
      /// Uri of the optional image for the select files button
      /// </summary>
      public string SelectFileButtonImageSource {get; set;}

      #endregion

      /// <summary>
      /// Constructor
      /// </summary>
      /// <param name="urlHelper"></param>
      public UploadModel(UrlHelper urlHelper)
      {
         // set defaults
         this.HttpUploader = true;
         this.MaxFileSizeKB = 102400; // 50Mb
         this.MaxUploads = 4;
         this.DefaultColor = "White";

         //"jpeg,jpg,gif,png,tif,tiff,ico,psd,doc,xls,docx,ppt,pps,pdf,xps,txt,log,csv,swf,zip,rar,7z";
         this.FileFilter = @"All allowed files|*.jpg;*.gif;*.png;*.doc;*.docx;*.ppt;*.pptx;*.pps;*.xls;*.xlsx;*.pdf;*.htm;*.html;*.swf;*.zip;*.7z;*.rar;*.avi;*.mpg;*.wmv|Jpeg (*.jpg)|*.jpg|Gif (*.gif)|*.gif|Png (*.png)|*.png|Microsoft Word (*.doc;*.docx)|*.doc;*.docx|Microsoft Powerpoint (*.ppt;*.pptx;*.pps)|*.ppt;*.pptx;*.pps|Microsoft Excel (*.xls;*.xlsx)|*.xls;*.xlsx|PDF (*.pdf)|*.pdf|Html (*.htm;*.html)|*.htm;*.html|Flash (*.swf)|*.swf|Compressed file (*.zip;*.7z;*.rar)|*.zip;*.7z;*.rar|Videos (*.avi;*.mpg;*.wmv)|*.avi;*.mpg;*.wmv";
         //this.FileFilter = @"All allowed files|*.jpg;*.gif;*.png;*.doc;*.docx;*.ppt;*.pptx;*.pps;*.xls;*.xlsx;*.pdf;*.htm;*.html;*.swf;*.zip;*.7z;*.rar;*.avi;*.mpg;*.wmv";

         string uploadUrl = urlHelper.Action("Upload", "MediaManager");
         this.UploadHandlerName = uploadUrl.StartsWith("/") ? uploadUrl.Substring(1) : uploadUrl;
         
      }
   }
}
