using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Arashi.Services.Localization
{
   public interface ILocalizationService
   {

      #region Methods

      /// <summary>
      /// Get a literal string from the global resources with the given resource name (token)
      /// </summary>
      /// <param name="token"></param>
      /// <param name="cultureInfo"></param>
      /// <returns></returns>
      string GlobalResource(string token, CultureInfo cultureInfo);



      /// <summary>
      /// Get a literal string from the validation resources with the given resource name (token)
      /// </summary>
      /// <param name="token"></param>
      /// <param name="cultureInfo"></param>
      /// <returns></returns>
      string ValidationResource(string token, CultureInfo cultureInfo);



      /// <summary>
      /// Get a literal string from the theme/template resources with the given resource name (token)
      /// </summary>
      /// <param name="token"></param>
      /// <param name="cultureInfo"></param>
      /// <returns></returns>
      string ThemeResource(string token, CultureInfo cultureInfo);

      #endregion

   }
}
