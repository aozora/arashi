<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.TemplateBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>
<% foreach (Post post in Model.RecentPosts) { %>
   <li>
      <a href="<%= get_permalink(post) %>" title="<%= Html.Encode(post.Title) %>"><%= Html.Encode(post.Title) %></a>
      <div style="clear:both"></div>
   </li>
<% } %>
