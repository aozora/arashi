<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.Views.AdminViewUserControlBase" %>
<%@ Import Namespace="Arashi.Web.Mvc.Gravatar"%>
<div class="login-status ui-state-default ui-corner-bottom">
   <div id="user-avatar">
      <input id="user-gravatar-url" type="hidden" value='<%= Html.GravatarUrl(RequestContext.CurrentUser.Email, 32) %>' />
      <img id="current-user-gravatar"
           src="/Resources/img/gravatar-default-32x32.png" 
           class="avatar"
           width="32" 
           height="32"
           alt="Gravatar" />
   </div>
   <div id="user-info">
      <p>
         <span><%= GlobalResource("LoggedUserLabel") %></span>
         &nbsp;
         <a href='<%= Url.Action("Details", "Users", new {siteid = RequestContext.CurrentSite.SiteId, id = RequestContext.CurrentUser.UserId}) %>' >
            <%= Html.Encode(RequestContext.CurrentUser.DisplayName)%>
         </a>
      </p>
      <p>
      <%= Html.ActionLink("Log Off", "Logout", "Login") %>
      </p>
   </div>
</div>
