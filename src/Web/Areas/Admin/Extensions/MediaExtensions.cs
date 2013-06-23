namespace Arashi.Web.Areas.Admin.Extensions
{
   using System;
   using System.Drawing;
   using System.IO;
   using System.Web;
   using System.Web.Mvc;

   using Arashi.Core.Domain;
   using Arashi.Core.Extensions;
   using Arashi.Core.Util;

   using Common.Logging;

   using ApplicationException = Arashi.Core.Exceptions.ApplicationException;



   /// <summary>
   /// Extension Methods for media
   /// </summary>
   public static class MediaExtensions
   {
      private static ILog log = LogManager.GetCurrentClassLogger();

      /// <summary>
      /// Returns an img element for a thumnail image
      /// </summary>
      /// <param name="html"></param>
      /// <param name="relativePath"></param>
      /// <param name="name"></param>
      /// <returns></returns>
      public static IHtmlString MediaThumbnail(this HtmlHelper html, string relativePath, string name)
      {
         string img = "<img width=\"48\" title=\"{0}\" alt=\"\" class=\"attachment-80x60\" src=\"{1}\" />";

         UrlHelper urlHelper = new UrlHelper(html.ViewContext.RequestContext);
         string src = urlHelper.RouteUrl("GetThumbnail", new { path = relativePath.EncodeToBase64(), width = 48, height = 48 });

         return html.Raw(string.Format(img, name, src));
      }



      /// <summary>
      /// returns as img element with a given image
      /// </summary>
      /// <param name="html"></param>
      /// <param name="name"></param>
      /// <returns></returns>
      public static IHtmlString MediaImage(this HtmlHelper html, string name)
      {
         string img = "<img width=\"{0}\" height=\"{1}\" title=\"{2}\" alt=\"\" class=\"attachment-80x60\" src=\"{3}\" />";

         // if the file is an image, than create a html img with the image itself
         if (ImageHelper.IsImage(name))
         {
            UrlHelper urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            string src = urlHelper.Action("GetMedia", "MediaManager", new {name = name});

            try
            {
               Size size = GetScaledImageSize(html, name, 48, 48);
               return html.Raw(string.Format(img, size.Width, size.Height, name, src));
            }
            catch(Exception ex)
            {
               log.Error(ex.ToString());
               return html.Raw(string.Format(img, "48", "48", name, "/Resources/img/48x48/file.png"));
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

            return html.Raw(string.Format(img, "48", "48", name, "/Resources/img/48x48/" + noImageName));
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
      private static Size GetScaledImageSize(HtmlHelper helper, string fileName, int maxWidth, int maxHeight)
      {
         IRequestContext context = helper.ViewData["Context"] as IRequestContext;

         if (context != null)
         {
            string mediaRoot = context.ManagedSite.SiteDataPath + "media/";
            string mediaRootFullPath = helper.ViewContext.HttpContext.Server.MapPath(mediaRoot);

            return ImageHelper.ScaleImage(Path.Combine(mediaRootFullPath, fileName), maxWidth, maxHeight);
         }

         throw new ApplicationException("MediaExtensions.GetScaledImageSize: IRequestContext cannot be null");
      }


   }
}
