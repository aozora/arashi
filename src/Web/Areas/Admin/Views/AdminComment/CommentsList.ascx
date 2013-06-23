<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.Views.AdminViewUserControlBase<IPagedList<Arashi.Core.Domain.Comment>>" %>
<%@ Import Namespace="Arashi.Web.Mvc.Gravatar"%>
<%@ Import Namespace="Arashi.Core.Extensions"%>
<%@ Import Namespace="Arashi.Core.Domain"%>
<%@ Import Namespace="Arashi.Web.Mvc.Paging"%>
<% if (Model != null){ %>
   <% foreach (Comment comment in Model){ %>
      <% Html.RenderPartial("~/Areas/Admin/Views/AdminComment/Comment.ascx", comment); %>
   <% } %>
      <% Html.RenderPartial("ReplyToComment");  %>
<% } else { %>
   <!-- Empty Template -->
   <tr class="emptyrow">
      <td colspan="5">No comments exists....</td>
   </tr>
<% } %>
