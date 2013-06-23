namespace Arashi.Web.Mvc.Filters
{
   using System.Globalization;
   using System.Threading;
   using System.Web.Mvc;
   using Arashi.Core;
   using Arashi.Core.Domain;
   using Common.Logging;



   /// <summary>
   /// This action filter set the current thread culture as defined in the current Site.DefaultCulture
   /// </summary>
   /// <remarks>
   /// http://paldev.net/blogs/hafanah/archive/2009/07/12/asp-net-globalization-hint.aspx
   /// http://msdn.microsoft.com/en-us/library/bz9tc508%28VS.71%29.aspx
   /// </remarks>
   public class LocalizationFilter : ActionFilterAttribute
   {
      private ILog log = LogManager.GetCurrentClassLogger();
      private readonly IRequestContext context;

      
      
      /// <summary>
      /// Creates a new instance of the <see cref="LocalizationFilter"></see> class.
      /// </summary>
      /// <remarks>
      /// Lookup components via the static IoC resolver. It would be great if we could do this via dependency injection
      /// but filters cannot be managed via the container in the current version of ASP.NET MVC. 
      /// </remarks>
      public LocalizationFilter()
         : this(IoC.Resolve<IRequestContext>())
      {
      }


      /// <summary>
      /// Creates a new instance of the <see cref="LocalizationFilter"></see> class.
      /// </summary>
      /// <param name="context"></param>
      public LocalizationFilter(IRequestContext context)
      {
         this.context = context;
      }



      /// <summary>
      /// Set the current thread culture as the current Site.DefaultCulture
      /// </summary>
      /// <param name="filterContext"></param>
      public override void OnActionExecuting(ActionExecutingContext filterContext)
      {
         if (Thread.CurrentThread.CurrentCulture != null && context.CurrentSite != null)
         {
            CultureInfo siteSpecificCulture = CultureInfo.CreateSpecificCulture(context.CurrentSite.DefaultCulture);
            Thread.CurrentThread.CurrentCulture = siteSpecificCulture;
            Thread.CurrentThread.CurrentUICulture = siteSpecificCulture;

            log.DebugFormat("LocalizationFilter.OnActionExecuting: Setting Thread.CurrentThread.CurrentCulture = {0}", siteSpecificCulture.TwoLetterISOLanguageName);
         }
      
      }


   }
}