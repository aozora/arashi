namespace Arashi.Web.Mvc
{
   using System;
   using System.Drawing;
   using System.Web;
   using System.Web.Caching;
   using System.Web.Mvc;
   using Arashi.Core.Util;
   using ApplicationException=Arashi.Core.Exceptions.ApplicationException;


   /// <summary>
   /// A custom ActionResult that render an image
   /// </summary>
   public class ImageResult : ActionResult
   {

      #region Public Properties

      public Image Source {get; set;}


      /// <summary>
      /// The full file system path of the original image
      /// </summary>
      public string OriginalFullFilePath { get; set; }

      #endregion


      public override void ExecuteResult(ControllerContext context)
      {
         RenderImage(context);
      }



      private void RenderImage(ControllerContext context)
      {
         if (Source == null)
            throw new ApplicationException("ThumbnailResult.RenderThumbnail: property Source cannot be null!");

         try
         {
            // Trasfomo in array di bytes
            byte[] b = ImageHelper.ConvertImageToByteArray(Source);

            // Http Caching & Expires
            context.HttpContext.Response.Cache.SetCacheability(HttpCacheability.Public);
            context.HttpContext.Response.Cache.SetExpires(Cache.NoAbsoluteExpiration);
            //context.HttpContext.Response.Cache.SetLastModifiedFromFileDependencies();
            //// add a file dependency on the original file
            //context.HttpContext.Response.AddFileDependency(OriginalFullFilePath);
            //context.HttpContext.Response.Cache.SetExpires(DateTime.Now + defaultClientCacheExpiration);

            context.HttpContext.Response.ContentType = MimeTypes.GetMimeTypeName(Source.RawFormat.ToString());
            context.HttpContext.Response.OutputStream.Write(b, 0, b.Length);
         }
         catch (Exception)
         {
            throw;
         }
         //finally
         //{
         //   if (outputImage != null)
         //      outputImage.Dispose();
         //}

      }

   }
}
