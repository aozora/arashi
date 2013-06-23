<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.Views.AdminViewUserControlBase<Arashi.Core.Domain.Comment>" %>
<%@ Import Namespace="Arashi.Core.Domain"%>
<%@ Import Namespace="Arashi.Web.Mvc.Gravatar"%>
<%@ Import Namespace="Arashi.Core.Extensions"%>
<tr>
   <td class="col-author">
      <%= Html.GravatarImage(Model.Email, 32) %>
      <div>
         <a href='<%= Url.Action("Edit", "AdminComment", new {siteid = RequestContext.ManagedSite.SiteId, id = Model.CommentId}) %>'>
            <%= Html.Encode(Model.AuthorName) %>
         </a>
         <br />
         <a href='mailto:<%= Model.Email %>'><%= Model.Email %></a>
         <br />
         <span><%= Model.UserIp %></span>
      </div>
   </td>
   <td class="multitext">
      Submitted on&nbsp;
      <a href='<%= Model.ContentItem.GetContentUrl() + "#comment-" + Model.CommentId.ToString() %>'>
         <%= Model.CreatedDate.AdjustDateToTimeZone(RequestContext.CurrentUser.TimeZone).ToShortDateString() %>
         &nbsp;at&nbsp;
         <%= Model.CreatedDate.AdjustDateToTimeZone(RequestContext.CurrentUser.TimeZone).ToShortTimeString() %>
      </a>
      <br />
      <p><%= Html.Encode(Model.CommentText) %></p>
      <div class="hover-actions">
         &nbsp;&nbsp;&nbsp;
         <% if (Model.Status == CommentStatus.Approved) { %>
            <span class="ui-icon ui-icon-cancel"></span>
            <%= Html.ActionLink("Unapprove", "ChangeStatus", "AdminComment", new {siteid = RequestContext.ManagedSite.SiteId, id = Model.CommentId, status = CommentStatus.Unapproved}, new {@class = "button-secondary change-status"}) %>
         <% } else { %>
            <span class="ui-icon ui-icon-check"></span>
            <%= Html.ActionLink("Approve", "ChangeStatus", "AdminComment", new {siteid = RequestContext.ManagedSite.SiteId, id = Model.CommentId, status = CommentStatus.Approved}, new {@class = "button-secondary change-status"}) %>
         <% } %>
         <span class="separator">&nbsp;|&nbsp;</span>
         <% if (Model.Status == CommentStatus.Spam) { %>
            <span class="ui-icon ui-icon-star"></span>
            <%= Html.ActionLink("This is not spam", "ChangeStatus", "AdminComment", new {siteid = RequestContext.ManagedSite.SiteId, id = Model.CommentId, status = CommentStatus.Approved}, new {@class = "button-secondary change-status"}) %>
         <% } else { %>
            <span class="ui-icon ui-icon-trash"></span>
            <%= Html.ActionLink("Mark as Spam", "ChangeStatus", "AdminComment", new {siteid = RequestContext.ManagedSite.SiteId, id = Model.CommentId, status = CommentStatus.Spam}, new {@class = "button-secondary change-status"}) %>
         <% } %>
         <span class="separator">&nbsp;|&nbsp;</span>
         <span class="ui-icon ui-icon-circle-close"></span>
         <%= Html.ActionLink("Delete", "Delete", "AdminComment", new {siteid = RequestContext.ManagedSite.SiteId, id = Model.CommentId}, new {@class = "button-secondary delete"}) %>
         <span class="separator">&nbsp;|&nbsp;</span>
         <span class="ui-icon ui-icon ui-icon-arrowreturnthick-1-e"></span>
         <a class="reply-link" 
            href="#"
            rel='<%= Model.CommentId %>' >Reply</a>
      </div>
   </td>
   <td>
      <span>
         <%= Model.Type.ToString() %>
      </span>
   </td>
   <td>
      <span>
         <%= Model.Status.ToString() %>
      </span>
   </td>
   <td class="multitext">
      <a href='<%= Model.ContentItem.GetContentUrl() %>' >
         <%= Html.Encode(Model.ContentItem.Title)%>
      </a>
   </td>
</tr>
