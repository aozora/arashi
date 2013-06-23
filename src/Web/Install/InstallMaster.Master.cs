using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Arashi.Core;
using Arashi.Services.Localization;

namespace Arashi.Web.Install
{
   public partial class InstallMaster : System.Web.UI.MasterPage
   {
      private ILocalizationService localizationService;


      public InstallMaster()
      {
         localizationService = IoC.Resolve<ILocalizationService>();
      }

      #region Localization Support

      /// <summary>
      /// Get a localized global resource
      /// </summary>
      /// <param name="token"></param>
      /// <returns></returns>
      protected string GlobalResource(string token)
      {
         return localizationService.ThemeResource(token, Thread.CurrentThread.CurrentUICulture);
      }



      /// <summary>
      /// Get a localized global resource filled with format parameters
      /// </summary>
      /// <param name="token"></param>
      /// <param name="args"></param>
      /// <returns></returns>
      protected string GlobalResource(string token, params object[] args)
      {
         return string.Format(GlobalResource(token), args);
      }

      #endregion

   }
}
