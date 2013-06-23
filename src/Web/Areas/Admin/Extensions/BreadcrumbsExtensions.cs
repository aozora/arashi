using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using Arashi.Core.Domain;
using Arashi.Core.Domain.Extensions;
using Arashi.Web.Mvc.Models;
using ApplicationException=Arashi.Core.Exceptions.ApplicationException;

namespace Arashi.Web.Areas.Admin.Extensions
{
   /// <summary>
   /// Create and render the classic "breadcrumbs"
   /// </summary>
   public static class BreadcrumbsExtensions
   {

      public static string Breadcrumbs(this HtmlHelper helper, Action<BreadcrumbBuilder> action)
      {
         IRequestContext context = helper.ViewData["Context"] as IRequestContext;

         Breadcrumbs b = new Breadcrumbs();

         action(new BreadcrumbBuilder(b));

         // Render
         StringBuilder html = new StringBuilder();
         //html.Append("<div id=\"breadcrumb\" class=\"ui-widget ui-widget-content ui-corner-all\">");
         //html.Append("<ul>");

         foreach (BreadcrumbNode node in b.Items)
         {
            if (node.IsHome)
               html.Append("<li class=\"home\">");
            else
               html.Append("<li>");

            // if the node don't have the href specified
            if (node.IsHome)
            {
               html.AppendFormat("<a class=\"home\" href=\"{0}\">", node.Href);
               html.Append("<img src=\"/Resources/img/16x16/home.png\" alt=\"home\" /><span class=\"ui-icon ui-icon-triangle-2-n-s\"></span>&nbsp;");
               html.Append(node.Text);
               html.Append("</a>");

               // if I have a siteid in the request (we are in the context of a Site specific view)
               // then I can show the site menu as a megadropdown of the "home" breadcrumb node. 
               //if (helper.ViewContext.RequestContext.RouteData.Values.ContainsKey("siteid"))
               if (context != null && context.ManagedSite != null)
               {
                  //int siteId = Convert.ToInt32(helper.ViewContext.RequestContext.RouteData.Values["siteid"]);
                  html.Append(GetSiteMegaMenu(helper, context.ManagedSite));
               }

            }
            else if (node.IsCurrent)
            {
               html.AppendFormat("<strong>{0}</strong>", node.Text);
            }
            else
            {
               html.AppendFormat("<a href=\"{0}\">{1}</a>", node.Href, node.Text);
            }

            html.Append("</li>");
         }

         //html.Append("</ul>");
         //html.Append("</div>");

         return html.ToString();
      }




      public static string GetSiteMegaMenu(HtmlHelper helper, Site managedSite)
      {
         StringBuilder html = new StringBuilder();

         // create a new instance of the UrlHelper
         UrlHelper urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
         IRequestContext context = helper.ViewData["Context"] as IRequestContext;

         IList<ControlPanelModel> model = helper.ViewData["ControlPanelModel"] as IList<ControlPanelModel>;

         if (model == null)
            throw new ApplicationException("BreadcrumbsExtensions.GetSiteMegaMenu - ViewData[\"ControlPanelModel\"] == null !!!");

         html.Append("<div id=\"sitemegamenu\" class=\"ui-widget ui-state-default ui-corner-bottom ui-corner-all ui-shadow\">");

         //html.Append("<div id=\"viewcurrentsite-container\" class=\"ui-state-hover-border-bottom\">");
         //html.Append("<img src=\"/Resources/img/16x16/arrow.png\" alt=\"view current site\" />");
         //html.AppendFormat("<a href=\"{0}\">View Current Site</a>", managedSite.DefaultUrl());
         //html.Append("</div>");

         foreach (var group in model)
         {
            html.Append("<ul>");
            html.Append("<li>");
            html.AppendFormat("<h2>{0}</h2>", helper.Encode(group.Category));
            html.Append("</li>");

            foreach (ControlPanelItem item in group.Items)
            {
               html.Append("<li>");
               html.AppendFormat("<img src=\"{0}\" alt=\"{1}\" />", item.LittleImageSrc, helper.Encode(item.ImageAlt) );
               html.AppendFormat("<a href=\"{0}\" title=\"{1}\" >", urlHelper.Action(item.Action, item.Controller, new {siteid = managedSite.SiteId}), helper.Encode(item.Description) );
               html.Append(helper.Encode(item.Text));
               html.Append("</a>");
               html.Append("</li>");
            }

            html.Append("</ul>");
         }

         html.Append("</div>");

         return html.ToString();
      }


   }



   /// <summary>
   /// Breadcrumb model that has a collection of BreadcrumbNodes
   /// </summary>
   public class Breadcrumbs
   {
      private IList<BreadcrumbNode> nodes;

      public Breadcrumbs()
      {
         nodes = new List<BreadcrumbNode>();
      }


      public IList<BreadcrumbNode> Items
      {
         get
         {
            return nodes;
         }
         set
         {
            nodes = value;
         }
      }

   }




   /// <summary>
   /// A single node of a bredcrumbs
   /// </summary>
   public class BreadcrumbNode
   {
      public string Text {get; set;}
      public string Href {get; set;}
      public bool IsHome {get; set;}
      public bool IsCurrent {get; set;}

      public BreadcrumbNode()
      {
         IsHome = false;
         IsCurrent = false;
      }

   }



   /// <summary>
   /// Helper used by the Breadcrumb extension
   /// </summary>
   public class BreadcrumbBuilder
   {
      private Breadcrumbs breadcrumbs;
 
      public BreadcrumbBuilder(Breadcrumbs breadcrumbs)
      {
         this.breadcrumbs = breadcrumbs;
      }


      public BreadcrumbBuilder Add(BreadcrumbNode node)
      {
         this.breadcrumbs.Items.Add(node);
         return this;
      }

      public BreadcrumbBuilder Add(string text, string href)
      {
         BreadcrumbNode node = new BreadcrumbNode
         {
            Text = text,
            Href = href
         };

         this.breadcrumbs.Items.Add(node);
         return this;
      }



      public BreadcrumbBuilder AddHome(string text, string href)
      {
         BreadcrumbNode node = new BreadcrumbNode
         {
            Text = text,
            Href = href,
            IsHome = true
         };

         this.breadcrumbs.Items.Add(node);
         return this;
      }



      public BreadcrumbBuilder AddCurrent(string text)
      {
         BreadcrumbNode node = new BreadcrumbNode
         {
            Text = text,
            Href = string.Empty,
            IsHome = false,
            IsCurrent = true
         };

         this.breadcrumbs.Items.Add(node);
         return this;
      }


   }

}
