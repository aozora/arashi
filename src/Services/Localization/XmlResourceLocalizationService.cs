using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using Common.Logging;

namespace Arashi.Services.Localization
{
   public class XmlResourceLocalizationService : ILocalizationService
   {
      #region Private Fields

      private ILog log;

      /// <summary>
      /// ResourceManager for the global resources
      /// </summary>
      private ResourceManager globalResourceManager;

      /// <summary>
      /// ResourceManager for the validation strings of the Control Panel
      /// </summary>
      private ResourceManager validationResourceManager;

      /// <summary>
      /// ResourceManager for the themes/templates files
      /// </summary>
      private ResourceManager themeResourceManager;
      private const string globalBaseName = "Arashi.Web.App_GlobalResources.GlobalResources";
      private const string validationBaseName = "Arashi.Web.App_GlobalResources.ValidationResources";
      private const string templateBaseName = "Arashi.Web.App_GlobalResources.ThemeResources";

      #endregion

      #region Constructor

      public XmlResourceLocalizationService(ILog log)
      {
         this.log = log;

         Assembly assembly = Assembly.Load("Arashi.Web");
         globalResourceManager = new ResourceManager(globalBaseName, assembly);
         //validationResourceManager = new ResourceManager(validationBaseName, assembly);
         themeResourceManager = new ResourceManager(templateBaseName, assembly);
      }

      #endregion

      #region Implementation of ILocalizationService

      /// <summary>
      /// Get a literal string from the global resources with the given resource name (token).
      /// If the localized string is empty (resource not found) return the token itself.
      /// </summary>
      /// <param name="token"></param>
      /// <param name="cultureInfo"></param>
      /// <returns></returns>
      public string GlobalResource(string token, CultureInfo cultureInfo)
      {
         try
         {
            string tokenValue = globalResourceManager.GetString(token, cultureInfo);
            return string.IsNullOrEmpty(tokenValue) ? token : tokenValue;
         }
         catch (MissingManifestResourceException ex)
         {
            log.WarnFormat("XmlResourceLocalizationService.GlobalResource: MissingManifestResourceException for token [{0}] and culture [{1}]", token, cultureInfo.TwoLetterISOLanguageName);
            log.Warn(ex.ToString());
            return token;
         }
      }



      /// <summary>
      /// Get a literal string from the validation resources with the given resource name (token)
      /// </summary>
      /// <param name="token"></param>
      /// <param name="cultureInfo"></param>
      /// <returns></returns>
      public string ValidationResource(string token, CultureInfo cultureInfo)
      {
         throw new NotImplementedException();
      }



      /// <summary>
      /// Get a literal string from the theme/template resources with the given resource name (token)
      /// </summary>
      /// <param name="token"></param>
      /// <param name="cultureInfo"></param>
      /// <returns></returns>
      public string ThemeResource(string token, CultureInfo cultureInfo)
      {
         try
         {
            return themeResourceManager.GetString(token, cultureInfo);
         }
         catch (MissingManifestResourceException ex)
         {
            log.WarnFormat("XmlResourceLocalizationService.ThemeResource: MissingManifestResourceException for token [{0}] and culture [{1}]", token, cultureInfo.TwoLetterISOLanguageName);
            log.Warn(ex.ToString());
            return token;
         }
      }

      #endregion
   }
}
