<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.TemplateBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>

	</div><!-- / #wrap -->

	<div id="footer">
		
		<div id="footerWrap">
		
			<p id="copy">Copyright &copy; <%= date("Y") %> <a href="#"><%= bloginfo("name") %></a>. All rights reserved.</p>
			
			<ul id="footerNav">
			
				<%= wp_list_pages("sort_column=menu_order&title_li=&depth=1") %>
				<li><a href="http://www.woothemes.com" title="Irresistible Theme by WooThemes"><img src="<%= bloginfo("template_directory") %>/images/img_woothemes.jpg" width="87" height="21" alt="WooThemes" /></a></li>			
			</ul>
		
		</div><!-- / #footerWrap -->
	
	</div><!-- / #footer -->

<% wp_footer(); %>
<%= Plugin.GoogleAnalytics() %>

</body>
</html>
