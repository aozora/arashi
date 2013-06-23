
namespace Arashi.Web.Areas.Admin.Controllers
{
   using Arashi.Services.Localization;
   using Arashi.Services.Membership;
   using Arashi.Services.SiteStructure;
   using System.Drawing;
   using Arashi.Core.Extensions;
   using Arashi.Web.Mvc;
   using Arashi.Web.Mvc.Controllers;
   using Common.Logging;

   public class ImagesController : SecureControllerBase
   {
      private ILog log;

      #region Constructor

      public ImagesController(ILog log, ILocalizationService localizationService, IUserService userService, ISiteService siteService)
         : base(log, localizationService, userService, siteService)
      {
         this.log = log;
      }

      #endregion


      /// <summary>
      /// Put an image thumbnail in the output stream of the current http response
      /// </summary>
      /// <param name="path">This MUST be a virtual path encoded with Base64</param>
      /// <param name="width"></param>
      /// <param name="height"></param>
      /// <returns></returns>
      public ThumbnailResult Thumbnail(string path, int width, int height)
      {
         string mappedPath = Server.MapPath(path.DecodeFromBase64());
         Image source = Image.FromFile(mappedPath);

         ThumbnailResult result = new ThumbnailResult()
                                     {
                                        OriginalFullFilePath = mappedPath,
                                        Source = source,
                                        ThumbnailWidth = width,
                                        ThumbnailHeight = height,
                                        MaintainAspect = true
                                     };

         return result;
      }

   }
}