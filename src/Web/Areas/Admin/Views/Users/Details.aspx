<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Site.Master" Inherits="Arashi.Web.Mvc.Views.AdminViewPageBase<UserModel>" %>
<%@ Import Namespace="xVal.Rules"%>
<%@ Import Namespace="Arashi.Web.Areas.Admin.Models"%>
<%@ Import Namespace="Arashi.Web.Areas.Admin.Extensions"%>
<%@ Import Namespace="Arashi.Core.Extensions"%>
<%@ Import Namespace="xVal.Html"%>
<%@ Import Namespace="Arashi.Core.Domain" %>
<asp:Content ContentPlaceHolderID="head" runat="server">
	<title>Details</title>
</asp:Content>

<asp:Content ContentPlaceHolderID="ScriptPlaceHolder" runat="server">
	<script type="text/javascript">
	   $(function(){
	      $('#tabs').tabs();
	      
	      $("#changePasswordLink").click(function() {
	         $("#changePasswordFormContainer").fadeIn();
	         $("#changePasswordLink").addClass("ui-state-disabled");
	         $("#Password").focus();
	      });
	      $("#changePasswordCancel").click(function() {
	         $("#changePasswordFormContainer").fadeOut();
	         $("#changePasswordLink").removeClass("ui-state-disabled");
	      });
	      
	   });
	   
	</script>
</asp:Content>

<asp:Content ContentPlaceHolderID="BreadCrumbsPlaceHolder" runat="server">
   <%= Html.Breadcrumbs(b => b.AddHome("Home", Url.Action("Index", "Site", new {siteid = RequestContext.ManagedSite.SiteId}))
                              .Add("Users", Url.Action("Index", "Users", new {siteid = RequestContext.ManagedSite.SiteId}))
                              .AddCurrent("User Details")) %>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
   <div id="adminpagetitle">
      <img class="icon" src="/Resources/img/32x32/user.png" alt="list users" />
      <h2>User Details</h2>
   </div>
   <div class="clear"></div>

   <div class="ui-widget ui-helper-clearfix">
      <a id="changePasswordLink" href="#" class="coolbutton ui-iconplustext ui-shadow" >
         <img src="/Resources/img/32x32/password.png" alt="change password" />
         <em>
            <span class="ui-iconplustext-title ui-title">Change Password</span>
            <span>Change the password for the user</span>
         </em>
      </a>
   </div>   
   <div id="changePasswordFormContainer" class="ui-widget ui-widget-content ui-corner-all ui-helper-hidden">
      <% using (Html.BeginForm("ChangePassword", "Users", new {siteid = RequestContext.ManagedSite.SiteId, id = Model.User.UserId}, FormMethod.Post, new {id = "newpageform", @class = "ui-form-default"})){ %>
         <div>
               <%= Html.AntiForgeryToken("changepassword") %>
         </div>
         <ol>
            <li>
               <label for="Password" >Password:</label>
               <%= Html.Password("Password", String.Empty, new {maxlength = 100, @class = "mediumtext" }) %>
            </li>
            <li>
               <label for="PasswordConfirmation" >Password Confirmation:</label>
               <%= Html.Password("PasswordConfirmation", String.Empty, new {maxlength = 100, @class = "mediumtext" }) %>
            </li>
            <li class="align-center">
	            <%= Html.SubmitUI(GlobalResource("Form_Save")) %>	
               &nbsp;|&nbsp;
               <a id="changePasswordCancel" href="#" class="button-secondary" ><%= GlobalResource("Form_Cancel") %></a>
            </li>
         </ol>
      <% } %>
   </div>
   <br />

   
	<% using (Html.BeginForm("Update", "Users", new {siteid = RequestContext.ManagedSite.SiteId, id = ViewData.Model.User.UserId}, FormMethod.Post, new {id = "userdetailsform", @class = "ui-form-default"})) { %>
      <div id="tabs">
         <%= Html.AntiForgeryToken("update") %>
         <ul>
            <li><a href="#tabUserData"><span>General Info</span></a></li>
            <li><a href="#tabUserRoles"><span>Roles</span></a></li>
            <li><a href="#tabAdmin"><span>Control Panel Preferencies</span></a></li>
         </ul>

         <div id="tabUserData">
            <%--<div class="form-legend">
               <div class="ui-widget ui-widget-content ui-corner-all">
                  <span ></span>
                  &nbsp;indicates required field&nbsp;
               </div>
            </div>--%>
                  
            <ol>
               <li>
                  <label for="Email" class="validation-required" title="This field is required">Email:</label>
                  <%= Html.TextBox("Email", Model.User.Email, new {maxlength = 100, @class = "largetext" }) %>
               </li>
               <li>
                  <label for="DisplayName" >Display Name:</label>
                  <%= Html.TextBox("DisplayName", Model.User.DisplayName, new {maxlength = 100, @class = "largetext" }) %>
                  <span class="hint">This is the name displayed on each content you publish</span>
               </li>
               <li>
                  <label for="FirstName" class="" >First Name:</label>
                  <%= Html.TextBox("FirstName", Model.User.FirstName, new {maxlength = 100, @class = "largetext" }) %>
               </li>
               <li>
                  <label for="LastName" class="" >Last Name:</label>
                  <%= Html.TextBox("LastName", Model.User.LastName, new {maxlength = 100, @class = "largetext" }) %>
               </li>
               <li>
                  <label for="IsActive" >Is Active:</label>
                  <%= Html.CheckBox("IsActive", Model.User.IsActive) %>
               </li>
               <li>
                  <label for="WebSite" >Web Site:</label>
                  <%= Html.TextBox("WebSite", Model.User.WebSite, new {maxlength = 100, @class = "largetext" }) %>
               </li>
               <li>
                  <label for="TimeZone" >Time Zone:</label>
	               <%= Html.DropDownList("TimeZone", Model.TimeZones)%>
               </li>
               <li>
                  <label for="Description">Biographical Notes:</label>
                  <%= Html.TextArea("Description", Model.User.Description, 5, 40, new {@class = "largetext" }) %>
               </li>
               <li>
                  <label>Last Login:</label>
                  <%= Html.Encode(Model.User.LastLogin.HasValue ? Model.User.LastLogin.Value.AdjustDateToTimeZone(Model.User.TimeZone).ToString() : string.Empty) %>
               </li>
               <li>
                  <label>Last IP Address:</label>
                  <%= Html.Encode(Model.User.LastIp) %>
               </li>
               <li>
                  <label>Created at:</label>
                  <%= Html.Encode(Model.User.CreatedDate.AdjustDateToTimeZone(Model.User.TimeZone).ToString()) %>
               </li>
               <li>
                  <label>Last Update at:</label>
                  <%= Html.Encode(Model.User.UpdatedDate.HasValue ? Model.User.UpdatedDate.Value.AdjustDateToTimeZone(Model.User.TimeZone).ToString() : string.Empty) %>
               </li>
            </ol>
         </div>
         
         <div id="tabUserRoles">
		      <%--<% Html.RenderPartial("RolesListUserControl", Model.User, ViewData.Add("Roles", Model.Roles) ); %>--%>
            <table class="grid ui-widget ui-widget-content ui-corner-all ui-shadow width-auto">
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
	               <% foreach (Role role in Model.Roles) { %>
	               <tr>
	                  <td class="width-200">
		                  <label for="Role_<%= role.RoleId %>"><%= role.Name %></label>  
	                  </td>
	                  <td class="width-40">
		                  <input type="checkbox" id="Role_<%= role.RoleId %>" name="roleIds" 
		                         value="<%= role.RoleId %>" 
		                         <%= Model.User.IsInRole(role.Name) ? "checked" : String.Empty %> />
	                  </td>
	               </tr>
	               <% } %>
               </tbody>
            </table>
         </div>
         
         <div id="tabAdmin">
            <ol>
               <li>
                  <label for="AdminTheme" >Control Panel Theme:</label>
	               <%= Html.DropDownList("AdminTheme", Model.AdminThemes)%>
               </li>
               <li>
                  <label for="AdminCulture">Language:</label>
                  <%= Html.DropDownList("AdminCulture", Model.Cultures, new {@class = "largetext"})%>
               </li>
            </ol>
         </div>
      </div>

      <div id="adminpagefooter" class="ui-widget">
	      <%= Html.SubmitUI(GlobalResource("Form_Save")) %>	
	      &nbsp;|&nbsp;
         <a href='<%= Url.Action("Index", "Site", new {siteid = RequestContext.ManagedSite.SiteId}) %>'><%= GlobalResource("Form_BackToControlPanel") %></a>
      </div>
   <% } %>

<% Html.Telerik().ScriptRegistrar()
      .Scripts(script => script.AddGroup( "js.userdetail.validation",
                                         group => group.Add("jquery.validate.js")
                                                       .Add("xVal.jquery.validate.js")
                                        ))
           .OnDocumentReady(() => { %>
                     <%= Html.ClientSideValidation<User>().SuppressScriptTags().ToString() %>
      <%--<% }).OnDocumentReady(() => { %>
                     <%= Html.ClientSideValidation<ChangePasswordModel>("User").SuppressScriptTags().ToString() %>--%>
      <%            }); %>
   
</asp:Content>

