using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arashi.Web.Mvc
{
   using System.Web.Helpers;
   using System.Web.Mvc;

   public class WebImageResult : ActionResult
   {
      private readonly WebImage image;
      private readonly string format;

      public WebImageResult(WebImage image)
         : this(image, null)
      { }



      public WebImageResult(WebImage image, string format)
      {
         this.image = image;
         this.format = format;
      }



      public WebImage WebImage
      {
         get { return this.image; }
      }



      public string Format
      {
         get { return this.format; }
      }



      public override void ExecuteResult(ControllerContext context)
      {
         if (this.format == null)
            this.image.Write(this.format);
         else
            this.image.Write(this.format);
      }
   }
}
