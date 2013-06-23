<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Site.Master" Inherits="Arashi.Web.Mvc.Views.AdminViewPageBase<Arashi.Core.Domain.SeoSettings>" %>
<%@ Import Namespace="Arashi.Core.Domain" %>
<asp:Content ContentPlaceHolderID="head" runat="server">
	<title>Search Index Rebuild</title>
</asp:Content>

<asp:Content ContentPlaceHolderID="BreadCrumbsPlaceHolder" runat="server">
   <%= Html.Breadcrumbs(b => b.AddHome("Home", Url.Action("Index", "Site", new {siteid = RequestContext.ManagedSite.SiteId}))
                              .AddCurrent("Search Index Rebuild")) %>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
   <div id="adminpagetitle">
      <img class="icon" src="/Resources/img/32x32/searchdb.png" alt="" />
      <h2>Search Index Rebuild</h2>
   </div>
   <div class="clear"></div>

   <% using (Html.BeginForm("Rebuild", "SearchIndex", new { siteid = RequestContext.ManagedSite.SiteId }, FormMethod.Post, new { id = "rebuildsearchindexform", @class = "ui-widget ui-form-default" })){ %>
      <p>
         In this page you can force the rescan and rebuild of the search index for all the published content.
         <br />
         Do this in order to resolve issues like corrupted index files or content missing from the search result.
      </p> 
      <p>
         <%= Html.AntiForgeryToken() %>
         <%= Html.SubmitUI("Rebuild!") %>	
         <br />
         <br />
         <br />
         <br />
      </p>
      
      <div id="adminpagefooter" class="ui-widget">
         <a href='<%= Url.Action("Index", "Site", new {siteid = RequestContext.ManagedSite.SiteId}) %>'><%= GlobalResource("Form_BackToControlPanel") %></a>
      </div>
       
   <% } %>

</asp:Content>