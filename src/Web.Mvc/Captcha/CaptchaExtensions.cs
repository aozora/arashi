using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;
using Arashi.Web.Mvc.Models;

namespace Arashi.Web.Mvc.Captcha
{
   public static class CaptchaExtensions
   {
      public static string GenerateCaptcha(this HtmlHelper helper)
      {
         // Check the EnableCaptchaForComments setting if the captcha is enabled
         TemplateContentModel model = helper.ViewData.Model as TemplateContentModel;
         if (!model.Site.EnableCaptchaForComments)
            return string.Empty;

         // check if the keys are configured
         if (string.IsNullOrEmpty(model.Site.CaptchaPublicKey) ||
             string.IsNullOrEmpty(model.Site.CaptchaPrivateKey))
            return string.Empty;

         var captchaControl = new Recaptcha.RecaptchaControl
                                 {
                                    ID = "recaptcha",
                                    Theme = "white", 
                                    PublicKey = model.Site.CaptchaPublicKey,
                                    PrivateKey = model.Site.CaptchaPrivateKey
                                 };

         var htmlWriter = new HtmlTextWriter(new StringWriter());
         captchaControl.RenderControl(htmlWriter);

         return string.Format("<div>{0}</div>", htmlWriter.InnerWriter.ToString());
      }

   }
}
