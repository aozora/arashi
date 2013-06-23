using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using Arashi.Web.Mvc.Models;

namespace Arashi.Web.Mvc.Plugins
{
   public class PluginHelper
   {
      public PluginHelper(ViewContext viewContext, IViewDataContainer viewDataContainer)
         : this(viewContext, viewDataContainer, RouteTable.Routes)
      {
      }



      public PluginHelper(ViewContext viewContext, IViewDataContainer viewDataContainer, RouteCollection routeCollection)
      {
         if (viewContext == null)
         {
            throw new ArgumentNullException("viewContext");
         }
         if (viewDataContainer == null)
         {
            throw new ArgumentNullException("viewDataContainer");
         }
         if (routeCollection == null)
         {
            throw new ArgumentNullException("routeCollection");
         }
         this.ViewContext = viewContext;
         this.ViewDataContainer = viewDataContainer;
         this.Model = ViewContext.ViewData.Model as TemplateContentModel;
         this.RouteCollection = routeCollection;
      }



      public ViewContext ViewContext
      {
         get;
         private set;
      }



      public TemplateContentModel Model
      {
         get;
         private set;
      }



      public ViewDataDictionary ViewData
      {
         get
         {
            return this.ViewDataContainer.ViewData;
         }
      }



      public IViewDataContainer ViewDataContainer
      {
         get;
         private set;
      }


      public RouteCollection RouteCollection
      {
         get;
         private set;
      }

   }
}
