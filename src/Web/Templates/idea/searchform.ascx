<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.TemplateBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>
<form role="search" method="get" id="searchform" action='<%= get_option("home") %>/search/' >
<%--<%= Html.AntiForgeryToken("searchform") %>--%>
	<div>
		<input type="text" name="s" id="Text1" value="" />
		<input type="submit" id="Submit1" value="GO" />
	</div>
</form>   
