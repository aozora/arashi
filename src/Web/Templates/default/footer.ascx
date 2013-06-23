<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.TemplateBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>
   <hr />
   <div id="footer">
	   <p>
		   <%= bloginfo("name") %> is powered by
		   <a href="http://www.arashi-project.com/">Arashi</a>
		   <br /><a href='<%= bloginfo("rss2_url") %>'>Entries (RSS)</a>
		   <%--and <a href='<% bloginfo("comments_rss2_url"); %>'>Comments (RSS)</a>.--%>
	   </p>
   </div>
</div>
<%= Plugin.GoogleAnalytics() %>
</body>
</html>
