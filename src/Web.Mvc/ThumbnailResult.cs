using System;
using System.Drawing;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Arashi.Core.Util;
using log4net;
using ApplicationException=Arashi.Core.Exceptions.ApplicationException;

namespace Arashi.Web.Mvc
{
   public class ThumbnailResult : ActionResult
   {
      private static readonly ILog log = LogManager.GetLogger(typeof(ThumbnailResult));

      #region Public Properties

      public Image Source {get; set;}
      public int ThumbnailWidth {get; set;}
      public int ThumbnailHeight {get; set;}
      public bool MaintainAspect {get; set;}

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

         try
         {
            outputImage = ImageHelper.CreateThumbnail(Source as Bitmap, ThumbnailWidth, ThumbnailHeight, MaintainAspect);

            // Trasfomo in array di bytes
            byte[] b = ImageHelper.ConvertImageToByteArray(outputImage);

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
