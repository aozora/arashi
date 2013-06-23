<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.TemplateBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>
<%@ Import Namespace="Arashi.Core.Extensions" %>
<%@ Import Namespace="Arashi.Core.Domain" %>
<% foreach (Comment comment in Model.RecentComments) { %>
   <li>
   	<%= Html.Encode(comment.AuthorName) %> said:&nbsp;
   	<a href="<%= get_permalink(comment.ContentItem) %>#comment-<%= comment.CommentId.ToString() %>" title="on <%= Html.Encode(comment.ContentItem.Title) %>">
   		<%= comment.CommentText.StripHtml() %>...
      </a>
      <div style="clear:both"></div>
   </li>
<% } %>
