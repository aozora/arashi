<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.TemplateBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>
<% get_header(); %>

	<div id="content" class="narrowcolumn">

		<h2 class="center"><%= Resource("_404_Title1") %></h2>

	</div>

<% get_sidebar(); %>
<% get_footer(); %>
