<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Login/Login.Master" Inherits="System.Web.Mvc.ViewPage<LoginModel>" %>
<%@ Import Namespace="xVal.Html"%>
<%@ Import Namespace="Arashi.Web.Areas.Admin.Extensions"%>
<%@ Import Namespace="Arashi.Web.Areas.Admin.Models"%>

<asp:Content ContentPlaceHolderID="head" runat="server">
	<title>Login</title>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
   <div id="loginbox" class="section ui-widget">
      <div id="loginbox-inner" class="ui-widget-content ui-corner-all ui-shadow">
         <div id="loginbox-title" class="ui-corner-all">
	         <h2 class="align-center">
               <img alt="logo" src="/Resources/img/logo/arashi-project-logo-h43.png"/>
	         </h2>
	         <% if (ViewData["AuthenticationFailed"] != null) { %>
	            <div class="validation-summary-errors ui-state-error ui-corner-all">
	               <%= Html.Encode(ViewData["AuthenticationFailed"].ToString()) %>
	            </div>
	         <% } %>
         </div>
         <div class="section-body">
		      <% using (Html.BeginForm("Authenticate", "Login", FormMethod.Post, new { id = "loginform", @class = "ui-form-default" })) { %>
			         <ol>
			            <li class="vertical">
				            <label for="Email">Email</label>
				            <%= Html.TextBox("Email", String.Empty, new {@class = "logintext"}) %>
			               <%= Html.ValidationMessage("Email")%>
			            </li>
			            <li class="vertical">
                        <label for="Password">Password</label>
				            <%= Html.Password("Password", String.Empty, new {@class = "logintext"}) %>
			               <%= Html.ValidationMessage("Password")%>
			            </li>
			            <li class="align-center">
                        <%= Html.AntiForgeryToken() %>
         			      <%= Html.Hidden("ReturnUrl", ViewData["ReturnUrl"]) %>
			               <%--<%= Html.SubmitUI("Login", "button ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only")%>--%>
                        <button type="submit" class="button ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only ui-shadow">
                           <span class="ui-button-text">Login</span>
                        </button>
			            </li>
			            <li class="align-center">
                        <%= Html.CheckBox("RememberMe", true) %> <label for="RememberMe" class="loginbox-rememberme">Remember Me</label>
			            </li>
			         </ol>
		      <% } %>
	      </div>
      </div>
      <br />
      &nbsp;&nbsp;&nbsp;
      <a href='<%= Url.Action("ResetPassword", "Login") %>'>
         Lost your password?
      </a>
   </div>
</asp:Content>
