using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Arashi.Core.Util
{
   using System.Drawing.Drawing2D;

   public class ImageHelper
   {

      /// <summary>
      /// A better alternative to Image.GetThumbnail. Higher quality but slightly slower
      /// </summary>
      /// <param name="source"></param>
      /// <param name="thumbWi"></param>
      /// <param name="thumbHi"></param>
      /// <param name="maintainAspect"></param>
      /// <returns></returns>
      public static Bitmap CreateThumbnail(Bitmap source, int thumbWi, int thumbHi, bool maintainAspect)
      {
         // return the source image if it's smaller than the designated thumbnail
         if (source.Width < thumbWi && source.Height < thumbHi)
            return source;

         Bitmap ret = null;
         
         try
         {
            int wi = thumbWi;
            int hi = thumbHi;

            if (maintainAspect)
            {
               // maintain the aspect ratio despite the thumbnail size parameters
               if (source.Width > source.Height)
               {
                  wi = thumbWi;
                  hi = (int)(source.Height * ((decimal)thumbWi / source.Width));
               }
               else
               {
                  hi = thumbHi;
                  wi = (int)(source.Width * ((decimal)thumbHi / source.Height));
               }
            }

            // original code that creates lousy thumbnails
            // System.Drawing.Image ret = source.GetThumbnailImage(wi,hi,null,IntPtr.Zero);
            ret = new Bitmap(wi, hi);
            using (Graphics g = Graphics.FromImage(ret))
            {
               g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
               g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
               g.PixelOffsetMode = PixelOffsetMode.HighQuality; 

               g.FillRectangle(Brushes.White, 0, 0, wi, hi);
               g.DrawImage(source, 0, 0, wi, hi);
            }
         }
         catch
         {
            ret = null;
         }

         return ret;
      }



      /// <summary>
      /// Get a System.Drawing.Imaging.ImageCodecInfo with a quality ratio of 75
      /// </summary>
      /// <returns></returns>
      public static ImageCodecInfo GetJpegImageCodecInfo()
      {
         return GetJpegImageCodecInfo(75);
      }



      /// <summary>
      /// Get a System.Drawing.Imaging.ImageCodecInfo
      /// </summary>
      /// <returns></returns>
      public static ImageCodecInfo GetJpegImageCodecInfo(long compressionQuality)
      {
         //Configure JPEG Compression Engine
         EncoderParameters encoderParams = new EncoderParameters();
         long[] quality = new long[1];
         quality[0] = compressionQuality;
         EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
         encoderParams.Param[0] = encoderParam;

         ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
         ImageCodecInfo jpegICI = null;
         for (int x = 0; x < arrayICI.Length; x++)
         {
            if (arrayICI[x].FormatDescription.Equals("JPEG"))
            {
               jpegICI = arrayICI[x];
               break;
            }
         }
         return jpegICI;
      }



      /// <summary>
      /// Calculate a new size for an image and returns a Size object with the new scaled dimensions
      /// </summary>
      /// <param name="img">Image</param>
      /// <param name="maxW">Max thumbnail width</param>
      /// <param name="maxH">Max thumbnail height</param>
      public static Size ScaleImage(Image img, int maxW, int maxH)
      {
         // Calcolo width e height per far stare l'img nel thumbnbail
         // Ho 3 casi principali:
         // - (w >  max) && (h <= max)
         // - (w <= max) && (h >  max)
         // - (w >  max) && (h >  max)

         Size size = new Size();
         int width = 0;
         int height = 0;

         if ((img.Width > maxW) && (img.Height <= maxH))
         {
            width = maxW;
            height = Convert.ToInt32((img.Height * maxW) / img.Width);
         }
         else if ((img.Width <= maxW) && (img.Height > maxH))
         {
            width = Convert.ToInt32((img.Width * maxH) / img.Height);
            height = maxH;
         }
         else if ((img.Width > maxW) && (img.Height > maxH))
         {
            // se sia W che H sono più grandi di max, ridimensiona in propozione al piu grandedi W/H
            if (img.Width > img.Height)
            {
               width = maxW;
               height = Convert.ToInt32((img.Height * maxW) / img.Width);
            }
            else if (img.Width > img.Height)
            {
               width = Convert.ToInt32((img.Width * maxH) / img.Height);
               height = maxH;
            }
            else if (img.Width == img.Height)
            {
               width = maxW;
               height = maxH;
            }
         }
         else
         {
            width = img.Width;
            height = img.Height;
         }

         size.Width = width;
         size.Height = height;

         return size;
      }



      /// <summary>
      /// Calculate a new size for an image and returns a Size object with the new scaled dimensions
      /// </summary>
      /// <param name="imageFullPath">Full physical path of the image file</param>
      /// <param name="maxW"></param>
      /// <param name="maxH"></param>
      /// <returns></returns>
      public static Size ScaleImage(string imageFullPath, int maxW, int maxH)
      {
         using (Image img = Image.FromFile(imageFullPath))
         {
            return ScaleImage(img, maxW, maxH);
         }
      }



      /// <summary>
      /// Return a Size object with the dimensions of the image
      /// </summary>
      /// <param name="image"></param>
      /// <returns></returns>
      public static Size GetSize(Image image)
      {
         return image.Size;
      }


      /// <summary>
      /// Return a Size object with the dimensions of the image
      /// </summary>
      /// <param name="imageFullPath"></param>
      /// <returns></returns>
      public static Size GetSize(string imageFullPath)
      {
         using (Image image = Image.FromFile(imageFullPath))
         {
            return image.Size;
         }
      }



      /// <summary>
      /// Convert an image to a byte array
      /// </summary>
      /// <param name="image"></param>
      /// <returns></returns>
      public static byte[] ConvertImageToByteArray(Image image)
      {
         ImageCodecInfo codec = null;

         foreach (ImageCodecInfo e in ImageCodecInfo.GetImageEncoders())
         {
            if (e.MimeType == "image/png")
            {
               codec = e;
               break;
            }
         }

         using (EncoderParameters ep = new EncoderParameters())
         {
            ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);

            using (MemoryStream ms = new MemoryStream())
            {
               image.Save(ms, codec, ep);
               return ms.ToArray();
            }
         }
      }



      /// <summary>
      /// Check if a file is of type image; this method evaluate the extension of the file name;
      /// Files with extension .jpg, .jpeg, .gif, .png, .bmp, 
      /// </summary>
      /// <param name="fileName"></param>
      /// <returns></returns>
      public static bool IsImage(string fileName)
      {
         //const string imageExtensions = "jpg|jpeg|gif|png|bmp";
         string fileExtension = fileName.Substring(fileName.LastIndexOf('.') + 1);

         return MimeTypes.IsImage(fileExtension);

         //return (imageExtensions.IndexOf(fileExtension) > -1);
      }



      #region Rotate Image

      /// <summary>
      /// Rotates the input image by rotationAngle degrees around center
      /// </summary>
      /// <param name="bmpSrc"></param>
      /// <param name="rotationAngle"></param>
      /// <returns></returns>
      public static Bitmap RotateImage(Bitmap bmpSrc, float rotationAngle)
      {
         Matrix mRotate = new Matrix();
         mRotate.Translate(bmpSrc.Width / -2, bmpSrc.Height / -2, MatrixOrder.Append);
         mRotate.RotateAt(rotationAngle, new Point(0, 0), MatrixOrder.Append);
         
         using (GraphicsPath gp = new GraphicsPath())
         {  
            // transform image points by rotation matrix
            gp.AddPolygon(new System.Drawing.Point[] { new System.Drawing.Point(0, 0), new System.Drawing.Point(bmpSrc.Width, 0), new System.Drawing.Point(0, bmpSrc.Height) });
            gp.Transform(mRotate);
            System.Drawing.PointF[] pts = gp.PathPoints;

            // create destination bitmap sized to contain rotated source image
            Rectangle bbox = BoundingBox(bmpSrc, mRotate);
            Bitmap bmpDest = new Bitmap(bbox.Width, bbox.Height);

            using (Graphics gDest = Graphics.FromImage(bmpDest))
            {  // draw source into dest
               Matrix mDest = new Matrix();
               mDest.Translate(bmpDest.Width / 2, bmpDest.Height / 2, MatrixOrder.Append);
               gDest.Transform = mDest;
               gDest.DrawImage(bmpSrc, pts);
               return bmpDest;
            }
         }
      }



      private static Rectangle BoundingBox(Image img, Matrix matrix)
      {
         GraphicsUnit gu = new GraphicsUnit();
         Rectangle rImg = Rectangle.Round(img.GetBounds(ref gu));

         // Transform the four points of the image, to get the resized bounding box.
         System.Drawing.Point topLeft = new System.Drawing.Point(rImg.Left, rImg.Top);
         System.Drawing.Point topRight = new System.Drawing.Point(rImg.Right, rImg.Top);
         System.Drawing.Point bottomRight = new System.Drawing.Point(rImg.Right, rImg.Bottom);
         System.Drawing.Point bottomLeft = new System.Drawing.Point(rImg.Left, rImg.Bottom);
         System.Drawing.Point[] points = new System.Drawing.Point[] { topLeft, topRight, bottomRight, bottomLeft };
         GraphicsPath gp = new GraphicsPath(points, new byte[] { (byte)PathPointType.Start, (byte)PathPointType.Line, (byte)PathPointType.Line, (byte)PathPointType.Line });
         gp.Transform(matrix);
         
         return Rectangle.Round(gp.GetBounds());
      }

      #endregion

   }
}