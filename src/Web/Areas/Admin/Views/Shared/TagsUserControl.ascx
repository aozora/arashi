<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IList<TagDTO>>" %>
<%@ Import Namespace="Arashi.Core.Domain.Dto"%>
<%-- TODO: Rename in AdminTagCloud.ascx --%>
<% foreach (TagDTO dto in Model) { %>
   <li value='<%= dto.Count.ToString() %>'>
      <a href="#" rel='<%= dto.TagId.ToString() %>' >
         <%= Html.Encode(dto.Name) %>
      </a>
   </li>     
<% } %>