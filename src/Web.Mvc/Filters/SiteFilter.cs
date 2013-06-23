using System;
using Arashi.Core;
using Arashi.Services.SiteStructure;
using Arashi.Web.Helpers;
using System.Web.Mvc;
using Arashi.Core.Domain;

namespace Arashi.Web.Mvc.Filters
{
   public class SiteFilter : ActionFilterAttribute
   {
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
         SetManagedSite(filterContext);

         SetCurrentSite(filterContext);
      }



      private void SetCurrentSite(ActionExecutingContext filterContext)
      {
         if (siteService == null)
            throw new InvalidOperationException("Unable to set the current site because the SiteService is unavailable");

         Site currentSite = siteService.GetSiteByHostName(WebHelper.GetHostName());

         if (currentSite == null)
            return;

         context.SetCurrentSite(currentSite);
         context.CurrentSiteDataFolder = filterContext.HttpContext.Server.MapPath(currentSite.SiteDataPath);
         filterContext.HttpContext.Items["CurrentSite"] = currentSite;
      }



      private void SetManagedSite(ActionExecutingContext filterContext)
      {
         if (siteService == null)
            throw new InvalidOperationException("Unable to set the current site because the SiteService is unavailable");

         UrlHelper urlHelper = new UrlHelper(filterContext.RequestContext);

         if (urlHelper.RequestContext.RouteData.Values.ContainsKey("siteid"))
         {
            if (!string.IsNullOrEmpty(urlHelper.RequestContext.RouteData.Values["siteid"].ToString()))
               context.SetManagedSite(siteService.GetSiteById(Convert.ToInt32(urlHelper.RequestContext.RouteData.Values["siteid"])));
         }
      }

   }
}
