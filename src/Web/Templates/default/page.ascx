<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.TemplateBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>
<% get_header(); %>

   <div id="content" class="narrowcolumn" >
	
		<div class="post" id="post-<%= Model.CurrentPage.Id.ToString() %>">
		<h2><%= Html.Encode(Model.CurrentPage.Title) %></h2>
			<div class="entry">
				<%= Model.CurrentPage.Content %>
			</div>
		</div>
   </div>

<% get_sidebar(); %>
<% get_footer(); %>
