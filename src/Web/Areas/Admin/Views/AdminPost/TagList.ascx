<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IList<Tag>>" %>
<%@ Import Namespace="Arashi.Core.Domain"%>
<option value=""></option>
<% foreach (Tag tag in Model) { %>
   <option value='<%= tag.TagId.ToString() %>'><%= Html.Encode(tag.Name) %></option>
<% } %>
