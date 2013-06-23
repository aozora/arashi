<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.TemplateBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>
<%--<% using(Html.BeginForm("Search", "Post", FormMethod.Get, new {role = "search", id = "searchform"})) { %>--%>
<form role="search" method="get" id="searchform" action='<%= get_option("home") %>/search/' >
<%--<%= Html.AntiForgeryToken("searchform") %>--%>
   <div>
      <label class="screen-reader-text" for="s">Search for:</label>
      <input type="text" value="" name="s" id="s" />
      <input type="submit" id="searchsubmit" value="Search" />
   </div>
</form>   
<%--<% } %>--%>

