using System;
using System.Configuration;
using System.Web.Mvc;
using Arashi.Core.Domain;

namespace Arashi.Web.Mvc.Captcha
{
   public class CaptchaValidatorAttribute : ActionFilterAttribute
   {
      private const string CHALLENGE_FIELD_KEY = "recaptcha_challenge_field";
      private const string RESPONSE_FIELD_KEY = "recaptcha_response_field";

      public override void OnActionExecuting(ActionExecutingContext filterContext)
      {
         Site currentSite = filterContext.HttpContext.Items["CurrentSite"] as Site;

         // Check if the CAPTCHA is enabled for the current site
         if (currentSite != null && currentSite.EnableCaptchaForComments)
         {
            filterContext.ActionParameters["captchaValid"] = CaptchaValidator.Validate(currentSite.CaptchaPrivateKey, filterContext.HttpContext.Request.Form[CHALLENGE_FIELD_KEY], filterContext.HttpContext.Request.Form[RESPONSE_FIELD_KEY]);
         }
         else
         {
            filterContext.ActionParameters["captchaValid"] = true;
         }

         base.OnActionExecuting(filterContext);

         // Add string to Trace for testing
         //filterContext.HttpContext.Trace.Write("Log: OnActionExecuting", String.Format("Calling {0}", filterContext.ActionDescriptor.ActionName));
      }
   }
}
