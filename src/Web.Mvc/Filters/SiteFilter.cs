namespace Arashi.Web.Mvc.Filters
{
   using System;
   using Arashi.Core;
   using Arashi.Services.SiteStructure;
   using Arashi.Web.Helpers;
   using System.Web.Mvc;
   using Arashi.Core.Domain;

   using Common.Logging;

   /// <summary>
   /// This filter set the current and the managed site by resolving the current url host and finding a siteid in the url
   /// </summary>
   [Obsolete("Due to problems with MVC3 this filter is deprecated", true)]
   public class SiteFilter : ActionFilterAttribute
   {
      private readonly ILog log = LogManager.GetCurrentClassLogger();
      private readonly ISiteService siteService;
      private readonly IRequestContext context;



      /// <summary>
      /// Creates a new instance of the <see cref="SiteFilter"></see> class.
      /// </summary>
      /// <remarks>
      /// Lookup components via the static IoC resolver. It would be great if we could do this via dependency injection
      /// but filters cannot be managed via the container in the current version of ASP.NET MVC. 
      /// </remarks>
      public SiteFilter()
         : this(IoC.Resolve<ISiteService>(), IoC.Resolve<IRequestContext>())
      {
      }



      /// <summary>
      /// Creates a new instance of the <see cref="SiteFilter"></see> class.
      /// </summary>
      /// <param name="siteService"></param>
      /// <param name="context"></param>
      public SiteFilter(ISiteService siteService, IRequestContext context)
      {
         this.siteService = siteService;
         this.context = context;
      }



      public override void OnActionExecuting(ActionExecutingContext filterContext)
      {
         log.Debug("SiteFilter.OnActionExecuting - start");
         SetManagedSite(filterContext);

         SetCurrentSite(filterContext);
         log.Debug("SiteFilter.OnActionExecuting - end");
      }



      private void SetCurrentSite(ActionExecutingContext filterContext)
      {
         log.Debug("SiteFilter.SetCurrentSite");

         if (siteService == null)
            throw new InvalidOperationException("Unable to set the current site because the SiteService is unavailable");

         Site currentSite = siteService.GetSiteByHostName(WebHelper.GetHostName());

         if (currentSite == null)
         {
            log.Warn("SiteFilter.SetCurrentSite - currentSite == null");
            return;
         }

         context.SetCurrentSite(currentSite);
         context.CurrentSiteDataFolder = filterContext.HttpContext.Server.MapPath(currentSite.SiteDataPath);
         filterContext.HttpContext.Items["CurrentSite"] = currentSite;
         filterContext.HttpContext.Items["CurrentSiteDataFolder"] = context.CurrentSiteDataFolder;
      }



      private void SetManagedSite(ActionExecutingContext filterContext)
      {
         log.Debug("SiteFilter.SetManagedSite");

         if (siteService == null)
            throw new InvalidOperationException("Unable to set the current site because the SiteService is unavailable");

         UrlHelper urlHelper = new UrlHelper(filterContext.RequestContext);

         if (urlHelper.RequestContext.RouteData.Values.ContainsKey("siteid"))
         {
            string siteid = urlHelper.RequestContext.RouteData.Values["siteid"].ToString();
            log.DebugFormat("SiteFilter.SetManagedSite - siteid = {0}", siteid);

            if (!string.IsNullOrEmpty(siteid))
            {
               context.SetManagedSite(siteService.GetSiteById(Convert.ToInt32(siteid)));
               filterContext.HttpContext.Items["ManagedSite"] = context.ManagedSite;
            }
         }
      }

   }
}
