using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arashi.Web.Mvc.Captcha
{
   using System.Configuration;
   using System.Web;

   /// <summary>
   /// CaptchaValidator
   /// </summary>
   public class CaptchaValidator
   {
      public CaptchaValidator()
      {
      }



      /// <summary>
      /// validate a captcha challenge
      /// </summary>
      /// <param name="captchaChallengeValue"></param>
      /// <param name="captchaResponseValue"></param>
      /// <returns></returns>
      public static bool Validate(string privatekey, string captchaChallengeValue, string captchaResponseValue)
      {
         var captchaValidtor = new Recaptcha.RecaptchaValidator
         {
            PrivateKey = privatekey,
            RemoteIP = HttpContext.Current.Request.UserHostAddress,
            Challenge = captchaChallengeValue,
            Response = captchaResponseValue
         };

         var recaptchaResponse = captchaValidtor.Validate();
         return recaptchaResponse.IsValid;
      }


   }
}
