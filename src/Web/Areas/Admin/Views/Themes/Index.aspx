<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Site.Master" Inherits="Arashi.Web.Mvc.Views.AdminViewPageBase<Arashi.Web.Areas.Admin.Models.ThemesModel>" %>
<%@ Import Namespace="Arashi.Core.Extensions"%>
<%@ Import Namespace="Arashi.Web.Areas.Admin.Extensions"%>
<%@ Import Namespace="Arashi.Web.Mvc.Paging"%>
<%@ Import Namespace="Arashi.Core.Domain"%>

<asp:Content ContentPlaceHolderID="head" runat="server">
	<title>Themes</title>
</asp:Content>

<asp:Content ContentPlaceHolderID="BreadCrumbsPlaceHolder" runat="server">
   <%= Html.Breadcrumbs(b => b.AddHome("Home", Url.Action("Index", "Site", new {siteid = RequestContext.ManagedSite.SiteId}))
                              .AddCurrent("Themes")) %>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
   <div id="adminpagetitle">
      <img class="icon" src="/Resources/img/32x32/themes.png" alt="themes list" />
      <h2>Themes</h2>
   </div>
   <div class="clear"></div>

   <div class="ui-widget">
      <ul id="theme-selector">
         <% if (Model.Templates != null){ %>
            <% foreach (Template template in Model.Templates) { %>
            <li>
               <div class="ui-widget-content ui-corner-all">
                  <h3>
                     <%= template.Name%>
                  </h3>
                  <div class="theme-thumbnail">
                     <img width="240px" src='<%= Url.RouteUrl("GetThumbnail", new {path = template.EncodedThumbnailVirtualPath, width = 240, height = 240})  %>' alt="template thumbnail" />
                  </div>
                  <div class="theme-button">
                     <% if (Model.CurrentTemplate != template) { %>
                        <a href='<%= Url.Action("Change", "Themes", new {siteid = RequestContext.ManagedSite.SiteId, id = template.TemplateId})  %>'
                           class="button ui-shadow" >Use this template</a>
                     <% } else { %>
                        <div class="current ui-state-highlight">
                           <span class="">&nbsp;In Use&nbsp;</span>
                        </div>
                     <% } %>
                  </div>
               </div>
            </li>
            <% } %>
         <% } else { %>
               <span>No theme exists...</span>
         <% } %>
    </ul>
   </div>

   <br />
   
   <div id="adminpagefooter" class="ui-widget">
      <a href='<%= Url.Action("Index", "Site", new {siteid = RequestContext.ManagedSite.SiteId}) %>'><%= GlobalResource("Form_BackToControlPanel") %></a>
   </div>
   
</asp:Content>
