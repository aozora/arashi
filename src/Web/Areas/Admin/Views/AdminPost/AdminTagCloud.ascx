<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IList<TagDTO>>" %>
<%@ Import Namespace="Arashi.Core.Domain.Dto"%>
<%= Html.TagCloud(Model) %>