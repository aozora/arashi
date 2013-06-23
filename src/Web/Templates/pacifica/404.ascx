<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.TemplateBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>
<% get_header(); %>

<div class="clear"></div>
<div id="content">
<div class="from_title">
<ul>
<li class="page_title"><%= Resource("_404_Title3") %></li>
</ul>
</div>

<div class="content_wrap">
<div class="full-entry">
<%= Resource("_404_Message3") %>
<%= Resource("_404_Message4") %>
</div><!-- .content-wrap-->
</div><!-- .full-entry-->

<% get_sidebar(); %>
<% get_footer(); %>
