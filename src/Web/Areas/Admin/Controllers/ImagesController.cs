using System.Drawing;
using Arashi.Core.Extensions;
using Arashi.Web.Mvc;
using Arashi.Web.Mvc.Controllers;

namespace Arashi.Web.Areas.Admin.Controllers
{
   public class ImagesController : SecureControllerBase
   {

      /// <summary>
      /// Put an image thumbnail in the output stream of the current http response
      /// </summary>
      /// <param name="path">This MUST be a virtual path encoded with Base64</param>
      /// <param name="width"></param>
      /// <param name="height"></param>
      /// <returns></returns>
      public ThumbnailResult Thumbnail(string path, int width, int height)
      {
         Image source = Image.FromFile(Server.MapPath(path.DecodeFromBase64()));

         ThumbnailResult result = new ThumbnailResult()
                                     {
                                        Source = source,
                                        ThumbnailWidth = width,
                                        ThumbnailHeight = height,
                                        MaintainAspect = true
                                     };

         return result;
      }

   }
}