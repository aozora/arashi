using System.Globalization;
using System.Threading;
using System.Web.Mvc;
using Arashi.Core;
using Arashi.Core.Domain;
using log4net;

namespace Arashi.Web.Mvc.Filters
{
   /// <summary>
   /// This action filter set the current thread culture as defined in the current user AdminCulture.
   /// This filter is used ONLY for the control panel controllers.
   /// </summary>
   /// <remarks>
   /// http://paldev.net/blogs/hafanah/archive/2009/07/12/asp-net-globalization-hint.aspx
   /// http://msdn.microsoft.com/en-us/library/bz9tc508%28VS.71%29.aspx
   /// </remarks>
   public class AdminLocalizationFilter : ActionFilterAttribute
   {
      private static readonly ILog log = LogManager.GetLogger(typeof(LocalizationFilter));
      private readonly IRequestContext context;

      
      
      /// <summary>
      /// Creates a new instance of the <see cref="LocalizationFilter"></see> class.
      /// </summary>
      /// <remarks>
      /// Lookup components via the static IoC resolver. It would be great if we could do this via dependency injection
      /// but filters cannot be managed via the container in the current version of ASP.NET MVC. 
      /// </remarks>
      public AdminLocalizationFilter()
         : this(IoC.Resolve<IRequestContext>())
      {
      }



      /// <summary>
      /// Creates a new instance of the <see cref="LocalizationFilter"></see> class.
      /// </summary>
      /// <param name="context"></param>
      public AdminLocalizationFilter(IRequestContext context)
      {
         this.context = context;
      }



      /// <summary>
      /// Set the current thread culture as the current user AdminCulture
      /// </summary>
      /// <param name="filterContext"></param>
      public override void OnActionExecuting(ActionExecutingContext filterContext)
      {
         //if (Thread.CurrentThread.CurrentCulture != null)

         // if there is a logged user, set the current thread culture like the user one.
         if (context.CurrentUser != null)
         {
            CultureInfo siteSpecificCulture = CultureInfo.CreateSpecificCulture(context.CurrentUser.AdminCulture);
            Thread.CurrentThread.CurrentCulture = siteSpecificCulture;
            Thread.CurrentThread.CurrentUICulture = siteSpecificCulture;

            log.DebugFormat("LocalizationFilter.OnActionExecuting: Setting Thread.CurrentThread.CurrentCulture = {0}", siteSpecificCulture.TwoLetterISOLanguageName);
         }
      
      }


   }
}