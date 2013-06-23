<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.Views.AdminViewUserControlBase" %>
<%@ Import Namespace="Arashi.Core.Domain"%>
<%@ Import Namespace="Arashi.Core.Domain.Extensions"%>
<%@ Import Namespace="Arashi.Web.Areas.Admin.Models"%>
<div id="site-chooser" class="ui-widget ui-state-default ui-corner-bottom ui-corner-tr ui-shadow">
   <ul>
      <li>
         <a href='<%= RequestContext.ManagedSite.DefaultUrl() %>'>
            <img src="/Resources/img/32x32/browser_view.png" alt="view site" />
            View current site
         </a>
      </li>
      <li>
         <a href='<%= Url.Action("NewSite", "Site", new {siteid = RequestContext.ManagedSite.SiteId}) %>'>
            <img src="/Resources/img/32x32/browser_add.png" alt="newsite" />
            Create new site...
         </a>
      </li>
   </ul>
   <span>Switch to site:</span>
   <ul>
      <% foreach (Site site in ViewData["SitesList"] as IList<Site>) { %>
         <li>
            <a class="ui-corner-all" href='<%= Url.Action("Index", "Site", new {siteid = site.SiteId}) %>' >
            <img src="/Resources/img/32x32/browser.png" alt="newsite" />
               <strong><%= Html.Encode(site.Name) %></strong>
            </a>
         </li>
      <% } %>
   </ul>
</div>
