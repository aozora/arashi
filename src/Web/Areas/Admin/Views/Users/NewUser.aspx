<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Site.Master" Inherits="Arashi.Web.Mvc.Views.AdminViewPageBase<User>" %>
<%@ Import Namespace="Arashi.Web.Areas.Admin.Models"%>
<%@ Import Namespace="Arashi.Web.Areas.Admin.Extensions"%>
<%@ Import Namespace="Arashi.Core.Domain"%>
<%@ Import Namespace="xVal.Html" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
	<title>New User</title>
</asp:Content>

<asp:Content ContentPlaceHolderID="BreadCrumbsPlaceHolder" runat="server">
   <%= Html.Breadcrumbs(b => b.AddHome("Home", Url.Action("Index", "Site", new {siteid = RequestContext.ManagedSite.SiteId}))
                              .Add("Users", Url.Action("Index", "Users", new {siteid = RequestContext.ManagedSite.SiteId}))
                              .AddCurrent("New User")) %>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
   <div id="adminpagetitle">
      <img class="icon" src="/Resources/img/32x32/user.png" alt="user" />
      <h2><%= Html.Encode(RequestContext.ManagedSite.Name) %> - New User</h2>
   </div>
   <div class="clear"></div>

	<% using(Html.BeginForm("Create", "Users", new { siteid = RequestContext.ManagedSite.SiteId }, FormMethod.Post, new { id = "userform", @class = "ui-form-default" })) { %>
      <div class="ui-widget ui-widget-content ui-corner-all content-padding-all">
         <%= Html.AntiForgeryToken() %>
         
         <ol>
            <li>
               <label for="Email" class="validation-required" title="This field is required">Email:</label>
               <%= Html.TextBox("Email", Model.Email, new {maxlength = 100, @class = "largetext" }) %>
            </li>
            <li>
               <label for="DisplayName" >Display Name:</label>
               <%= Html.TextBox("DisplayName", Model.DisplayName, new {maxlength = 100, @class = "largetext" }) %>
               <span class="hint">This is the name displayed on each content you publish</span>
            </li>
            <li>
               <label for="FirstName">First Name:</label>
               <%= Html.TextBox("FirstName", Model.FirstName, new {maxlength = 100, @class = "largetext" }) %>
            </li>
            <li>
               <label for="LastName" >Last Name:</label>
               <%= Html.TextBox("LastName", Model.LastName, new {maxlength = 100, @class = "largetext" }) %>
            </li>
            <li>
               <label for="Password" class="validation-required" title="This field is required" >Password:</label>
               <%= Html.Password("Password", String.Empty, new {maxlength = 100, @class = "largetext" }) %>
            </li>
            <li>
               <label for="PasswordConfirmation" class="validation-required" title="This field is required">Password Confirmation:</label>
               <%= Html.Password("PasswordConfirmation", String.Empty, new {maxlength = 100, @class = "largetext" }) %>
            </li>
            <li>
               <label for="IsActive" >Is Active:</label>
               <%= Html.CheckBox("IsActive", Model.IsActive) %>
            </li>
            <li>
               <label for="WebSite" >Web Site:</label>
               <%= Html.TextBox("WebSite", Model.WebSite, new {maxlength = 100, @class = "largetext" }) %>
            </li>
            <li>
               <label for="TimeZone" >Time Zone:</label>
               <%= Html.DropDownList("TimeZone", ViewData["TimeZones"] as SelectList)%>
            </li>
            <li>
               <label for="AdminTheme" >Control Panel Theme:</label>
               <%= Html.DropDownList("AdminTheme", ViewData["AdminThemes"] as SelectList)%>
            </li>
            <li>
               <label for="AdminCulture">Language:</label>
               <%= Html.DropDownList("AdminCulture", ViewData["Cultures"] as SelectList, new {@class = "largetext"})%>
            </li>
            <li>
               <label for="Description">Biographical Notes:</label>
               <%= Html.TextArea("Description", Model.Description, 5, 40, new {@class = "largetext" }) %>
            </li>
            <li>
               <label>&nbsp;</label>
	            <% Html.RenderPartial("RolesListUserControl", Model, ViewData); %>
            </li>
         </ol>
      </div>

      <div id="adminpagefooter" class="ui-widget">
	      <%= Html.SubmitUI(GlobalResource("Form_Save")) %>	
	      &nbsp;|&nbsp;
         <a href='<%= Url.Action("Index", "Site", new {siteid = RequestContext.ManagedSite.SiteId}) %>'><%= GlobalResource("Form_BackToControlPanel") %></a>
      </div>
   <% } %>

<% Html.Telerik().ScriptRegistrar()
      .Scripts(script => script.AddGroup("js.usernew.validation",
                                         group => group.Add("jquery.validate.js")
                                                       .Add("xVal.jquery.validate.js")
                                        ))
      .OnDocumentReady(() => { %>
                     <%= Html.ClientSideValidation<User>()
                             .SuppressScriptTags().ToString() %>
      <%            }); %>

</asp:Content>
