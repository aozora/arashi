namespace Arashi.Web.Mvc
{
   using System;
   using System.Drawing;
   using System.Web;
   using System.Web.Caching;
   using System.Web.Mvc;
   using Arashi.Core.Util;

   using Common.Logging;

   using ApplicationException=Arashi.Core.Exceptions.ApplicationException;


   /// <summary>
   /// A custom ActionResult that render an image thumbnails, with cache header expiration of 10minutes by default
   /// </summary>
   public class ThumbnailResult : ActionResult
   {
      private readonly ILog log = LogManager.GetCurrentClassLogger();

      #region Public Properties

      public Image Source {get; set;}
      public int ThumbnailWidth {get; set;}
      public int ThumbnailHeight {get; set;}
      public bool MaintainAspect {get; set;}

      /// <summary>
      /// The full file system path of the original image
      /// </summary>
      public string OriginalFullFilePath { get; set; }

      #endregion


      public override void ExecuteResult(ControllerContext context)
      {
         RenderThumbnail(context);
      }



      private void RenderThumbnail(ControllerContext context)
      {
         if (Source == null)
            throw new ApplicationException("ThumbnailResult.RenderThumbnail: property Source cannot be null!");

         Image outputImage = null;
         TimeSpan defaultClientCacheExpiration = new TimeSpan(0, 10, 0); // 10 minutes

         try
         {
            outputImage = ImageHelper.CreateThumbnail(Source as Bitmap, ThumbnailWidth, ThumbnailHeight, MaintainAspect);

            // Trasfomo in array di bytes
            byte[] b = ImageHelper.ConvertImageToByteArray(outputImage);

            // Http Caching & Expires
            context.HttpContext.Response.Cache.SetCacheability(HttpCacheability.Public);
            context.HttpContext.Response.Cache.SetExpires(Cache.NoAbsoluteExpiration);
            context.HttpContext.Response.Cache.SetLastModifiedFromFileDependencies();
            // add a file dependency on the original file
            context.HttpContext.Response.AddFileDependency(OriginalFullFilePath);
            context.HttpContext.Response.Cache.SetExpires(DateTime.Now + defaultClientCacheExpiration);


            context.HttpContext.Response.ContentType = "image/jpeg"; // the thumb is always jpeg
            context.HttpContext.Response.OutputStream.Write(b, 0, b.Length);
            //outputImage.Save(context.HttpContext.Response.OutputStream, ImageFormat.Jpeg);
         }
         catch (Exception ex)
         {
            log.Error(ex.ToString());
            throw;
         }
         finally
         {
            if (outputImage != null)
               outputImage.Dispose();
         }

      }

   }
}
