<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Site.Master" Inherits="Arashi.Web.Mvc.Views.AdminViewPageBase<RoleModel>" %>
<%@ Import Namespace="Arashi.Web.Areas.Admin.Models"%>
<%@ Import Namespace="Arashi.Core.Domain.Extensions"%>
<%@ Import Namespace="Arashi.Core.Domain"%>
<%@ Import Namespace="Arashi.Web.Areas.Admin.Extensions"%>
<%@ Import Namespace="Arashi.Core.Extensions"%>
<asp:Content ContentPlaceHolderID="head" runat="server">
	<title>Edit Group</title>
</asp:Content>

<asp:Content ContentPlaceHolderID="ScriptPlaceHolder" runat="server">
   <script type="text/javascript">
      $(function(){
         $("#selectAllCheckBox").click(function(){
            var checked_status = this.checked;
            $("#rightsList input:checkbox").not($(this)).each(function()
            {
	            this.checked = checked_status;
            });         
         });
      });
   </script>
</asp:Content>

<asp:Content ContentPlaceHolderID="BreadCrumbsPlaceHolder" runat="server">
   <%= Html.Breadcrumbs(b => b.AddHome("Home", Url.Action("Index", "Site", new {siteid = RequestContext.ManagedSite.SiteId}))
                              .Add("Groups", Url.Action("Index", "Roles", new {siteid = RequestContext.ManagedSite.SiteId}))
                              .AddCurrent("Edit Group")) %>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
   <div id="adminpagetitle">
      <img class="icon" src="/Resources/img/32x32/password.png" alt="edit group" />
      <h2>Group Edit</h2>
   </div>
   <div class="clear"></div>
   
   
	<% using (Html.BeginForm("Update", "Roles", new {siteid = RequestContext.ManagedSite.SiteId, id = Model.Role.RoleId}, FormMethod.Post, new {id = "roleeditform", @class = "ui-widget ui-form-default"})) { %>
      
      <ol>
         <li>
            <label for="Name">Name:</label>
            <%= Html.TextBox("Name", Model.Role.Name) %>
         </li>
      </ol>
      
      <div><br /></div>
      <table class="grid ui-widget ui-widget-content ui-corner-all ui-shadow">
         <thead class="ui-widget-header ui-corner-top">
            <tr>
               <th colspan="3">&nbsp;&nbsp;Rights for group &#8216;<%= Model.Role.Name%>&#8217;</th>
            </tr>
         </thead>
         <tfoot class="ui-widget-header ui-corner-bottom">
            <tr>
               <th colspan="3"></th>
            </tr>
         </tfoot>
         <tbody id="rightsList" class="ui-widget-content">
            <tr>
               <td class="width-150"></td>
               <td class="col-check"><input type="checkbox" id="selectAllCheckBox" title="select all" /></td>
               <td><label class="hint" for="selectAllCheckBox">(Select All)</label></td>
            </tr>
            
		      <% string prevGroup = string.Empty; %>
		      <% foreach (Right right in Model.AllRights) { %>
		      <tr>
		         <td class="align-left">
			         <span class="title">
			            <% if (prevGroup != right.RightGroup) { %>
                        <%= Html.Encode(right.RightGroup) %>
                     <% } %>
		               <% prevGroup = right.RightGroup; %>
			         </span>
		         </td>
		         <td class="col-check">
			         <input type="checkbox" name="rightIds" id="right_<%= right.Id %>" value="<%= right.Id %>" <%= Model.Role.HasRight(right.Name) ? "checked" : String.Empty %> />
		         </td>
			      <td>
			         <span><%= Html.Encode(right.Description) %></span>
			      </td>
		      </tr>
		      <% } %>
         </tbody>
      </table>

      <div id="adminpagefooter" class="ui-widget">
         <%= Html.AntiForgeryToken() %>
	      <%= Html.SubmitUI(GlobalResource("Form_Save")) %>	
	      &nbsp;|&nbsp;
         <a href='<%= Url.Action("Index", "Roles", new {siteid = RequestContext.ManagedSite.SiteId}) %>'>Back to groups list</a>
	      &nbsp;|&nbsp;
         <a href='<%= Url.Action("Index", "Site", new {siteid = RequestContext.ManagedSite.SiteId}) %>'><%= GlobalResource("Form_BackToControlPanel") %></a>
      </div>
   <% } %>
</asp:Content>

