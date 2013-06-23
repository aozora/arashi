<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Site.Master" Inherits="Arashi.Web.Mvc.Views.AdminViewPageBase" %>
<%@ Import Namespace="Arashi.Core.Extensions"%>
<%@ Import Namespace="Arashi.Web.Areas.Admin.Extensions"%>
<%@ Import Namespace="Arashi.Web.Mvc.Paging"%>
<%@ Import Namespace="Arashi.Core.Domain"%>

<asp:Content ContentPlaceHolderID="head" runat="server">
	<title>Widgets</title>
</asp:Content>

<asp:Content ContentPlaceHolderID="BreadCrumbsPlaceHolder" runat="server">
   <%= Html.Breadcrumbs(b => b.AddHome("Home", Url.Action("Index", "Site", new {siteid = RequestContext.ManagedSite.SiteId}))
                              .AddCurrent("Widgets")) %>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
   <div id="adminpagetitle">
      <img class="icon" src="/Resources/img/32x32/widgets.png" alt="Widgets list" />
      <h2>Widgets</h2>
   </div>
   <div class="clear"></div>

   <div class="ui-widget">
      <div style="margin: 20px auto; width: 350px">
         <h3>Sorry, this page is under construction.</h3>
         <h4>Please come back later...</h4>
         <img src="/Resources/img/under_construction_page.png" alt="under construction" />
      </div>
   </div>

   <div class="clear"></div>
   <br />
   
   
   <div id="adminpagefooter" class="ui-widget">
      <a href='<%= Url.Action("Index", "Site", new {siteid = RequestContext.ManagedSite.SiteId}) %>'><%= GlobalResource("Form_BackToControlPanel") %></a>
   </div>
   
</asp:Content>
