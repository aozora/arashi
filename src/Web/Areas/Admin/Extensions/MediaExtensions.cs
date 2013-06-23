using System.Drawing;
using System.IO;
using System.Web.Mvc;
using Arashi.Core.Domain;
using Arashi.Core.Util;
using ApplicationException=Arashi.Core.Exceptions.ApplicationException;

namespace Arashi.Web.Areas.Admin.Extensions
{
   /// <summary>
   /// Extension Methods for media
   /// </summary>
   public static class MediaExtensions
   {
      public static string MediaImage(this HtmlHelper html, string name)
      {
         string img = "<img width=\"{0}\" height=\"{1}\" title=\"{2}\" alt=\"\" class=\"attachment-80x60\" src=\"{3}\" />";

         // if the file is an image, than create a html img with the image itself
         if (ImageHelper.IsImage(name))
         {
            UrlHelper urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            string src = urlHelper.Action("GetMedia", "MediaManager", new {name = name});

            Size size = GetScaledImageSize(html, name, 48, 48);

            return string.Format(img, size.Width, size.Height, name, src);
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
