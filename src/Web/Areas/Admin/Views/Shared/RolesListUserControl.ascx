<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<User>" %>
<%@ Import Namespace="Arashi.Web.Areas.Admin.Models"%>
<%@ Import Namespace="Arashi.Core.Domain"%>
<%@ Import Namespace="Arashi.Core.Domain.Extensions"%>
<table class="grid ui-widget ui-widget-content ui-corner-all width-auto ui-shadow">
   <thead class="ui-widget-header ui-corner-top">
      <tr>
         <th class="width-200">Role</th>
         <th class="width-40"></th>
      </tr>
   </thead>
   <tfoot class="ui-widget-header ui-corner-bottom">
      <tr>
         <th colspan="2"></th>
      </tr>
   </tfoot>
   <tbody class="ui-widget-content">
		<% foreach (Role role in (ViewData["Roles"] as IList<Role>)) { %>
		<tr>
		   <td class="width-200">
			   <label for="Role_<%= role.RoleId %>"><%= role.Name %></label>  
		   </td>
		   <td class="width-40">
			   <input type="checkbox" id="Role_<%= role.RoleId %>" name="roleIds" 
			          value="<%= role.RoleId %>" 
			          <%= Model.IsInRole(role) ? "checked" : String.Empty %> />
		   </td>
		</tr>
		<% } %>
   </tbody>
</table>

