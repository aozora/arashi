<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Site.Master" Inherits="Arashi.Web.Mvc.Views.AdminViewPageBase<IPagedList<Arashi.Core.Domain.User>>" %>
<%@ Import Namespace="Arashi.Core.Extensions"%>
<%@ Import Namespace="Arashi.Web.Areas.Admin.Extensions"%>
<%@ Import Namespace="Arashi.Web.Mvc.Paging"%>
<%@ Import Namespace="Arashi.Core.Domain"%>

<asp:Content ContentPlaceHolderID="head" runat="server">
	<title>Index</title>
</asp:Content>

<asp:Content ContentPlaceHolderID="BreadCrumbsPlaceHolder" runat="server">
   <%= Html.Breadcrumbs(b => b.AddHome("Home", Url.Action("Index", "Site", new {siteid = RequestContext.ManagedSite.SiteId}))
                              .AddCurrent("Users")) %>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
   <div id="adminpagetitle">
      <img class="icon" src="/Resources/img/32x32/users.png" alt="list users" />
      <h2>Users</h2>
   </div>
   <div class="clear"></div>

   <div class="ui-widget ui-helper-clearfix">
      <a id="newUserLink" 
             href='<%= Url.Action("NewUser", "Users", new {siteid = RequestContext.ManagedSite.SiteId }) %>'  
             title="Create new user" 
             class="coolbutton ui-iconplustext ui-shadow" >
         <img src="/Resources/img/32x32/user_add.png" alt="add user" />
         <em>
            <span class="ui-iconplustext-title ui-title">Add User</span>
            <span>Create a new user</span>
         </em>
      </a>
   </div>   

   <br />

   <table class="grid ui-widget ui-widget-content ui-corner-all ui-shadow">
      <thead class="ui-widget-header ui-corner-top">
         <tr>
            <th>Name</th>
            <th>Email</th>
            <th>Active</th>
            <th>Last Login</th>
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
            <% foreach (User user in Model){ %>
               <tr>
                  <td class="title">
                     <a href='<%= Url.Action("Details", "Users", new {siteid = RequestContext.ManagedSite.SiteId, id = user.UserId}) %>' >
                        <%= Html.Encode(user.DisplayName) %>
                     </a>
                  </td>
                  <td>
                     <%= Html.Encode(user.Email) %>
                  </td>
                  <td>
                     <input type="checkbox" disabled="disabled" <%= (user.IsActive ? "checked='checked'" : "") %> />
                  </td>
                  <td>
                  <% if (user.LastLogin.HasValue)
                     { %>
                     <%= user.LastLogin.Value.AdjustDateToTimeZone(user.TimeZone).ToShortDateString() %>
                  <% } else { %>
                     never logged
                  <% } %>
                  </td>
               </tr>
            <% } %>
         <% } else { %>
         <!-- Empty Template -->
            <tr class="emptyrow">
               <td colspan="4">
                  <% Html.RenderMessage("No users exists. {0}",
                                         "info",
                                         Url.Action("AddUser", "User"),
                                         "Create new one",
                                         "margin-topbottom", 
                                         false); %>
               </td>
            </tr>
         <% } %>
      </tbody>
   </table>
   
   <div id="adminpagefooter" class="ui-widget">
      <a href='<%= Url.Action("Index", "Site", new {siteid = RequestContext.ManagedSite.SiteId}) %>'><%= GlobalResource("Form_BackToControlPanel") %></a>
   </div>
   
</asp:Content>
