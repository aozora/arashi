<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Site.Master" Inherits="Arashi.Web.Mvc.Views.AdminViewPageBase<IPagedList<Arashi.Core.Domain.Role>>" %>
<%@ Import Namespace="Arashi.Core.Extensions"%>
<%@ Import Namespace="Arashi.Web.Areas.Admin.Extensions"%>
<%@ Import Namespace="Arashi.Web.Mvc.Paging"%>
<%@ Import Namespace="Arashi.Core.Domain"%>

<asp:Content ContentPlaceHolderID="head" runat="server">
	<title>Groups</title>
</asp:Content>

<asp:Content ContentPlaceHolderID="BreadCrumbsPlaceHolder" runat="server">
   <%= Html.Breadcrumbs(b => b.AddHome("Home", Url.Action("Index", "Site", new {siteid = RequestContext.ManagedSite.SiteId}))
                              .AddCurrent("Groups")) %>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
   <div id="adminpagetitle">
      <img class="icon" src="/Resources/img/32x32/password.png" alt="list roles" />
      <h2>Roles</h2>
   </div>
   <div class="clear"></div>

   <div class="ui-widget ui-helper-clearfix">
      <a id="newRoleLink" 
             href='<%= Url.Action("NewRole", "Roles", new {siteid = RequestContext.ManagedSite.SiteId }) %>'  
             title="Create new user" 
             class="coolbutton ui-iconplustext ui-shadow" >
         <img src="/Resources/img/32x32/password_add.png" alt="add role" />
         <em>
            <span class="ui-iconplustext-title ui-title">Add Group</span>
            <span>Create a new users group</span>
         </em>
      </a>
   </div>   

   <br />

   <table class="grid ui-widget ui-widget-content ui-corner-all ui-shadow">
      <thead class="ui-widget-header ui-corner-bottom">
         <tr>
            <th class="width-200">Name</th>
            <th>Rights</th>
         </tr>
      </thead>
      <tfoot class="ui-widget-header ui-corner-bottom">
         <tr>
            <th colspan="4">
               <% if (Model != null){ %>
	               <div class="pager">
		               <%= Html.Pager(Model.PageSize, Model.PageNumber, Model.TotalItemCount )%>
	               </div>
               <% } %>
            </th>
         </tr>
      </tfoot>
      <tbody class="ui-widget-content">
         <% if (Model != null){ %>
            <% foreach (Role role in Model){ %>
               <tr>
                  <td class="title width-200">
                     <a href='<%= Url.Action("Edit", "Roles", new {siteid = RequestContext.ManagedSite.SiteId, id = role.RoleId}) %>' >
                        <%= Html.Encode(role.Name) %>
                     </a>
                  </td>
                  <td>
                     <%= Html.Encode(role.RightsString) %>
                  </td>
               </tr>
            <% } %>
         <% } else { %>
         <!-- Empty Template -->
            <tr class="emptyrow">
               <td colspan="5">
                  <span>No roles exists...</span>
               </td>
            </tr>
         <% } %>
      </tbody>
   </table>
   
   <div id="adminpagefooter" class="ui-widget">
      <a href='<%= Url.Action("Index", "Site", new {siteid = RequestContext.ManagedSite.SiteId}) %>'><%= GlobalResource("Form_BackToControlPanel") %></a>
   </div>
   
</asp:Content>
