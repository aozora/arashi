<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.TemplateBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>
<% if (!is_404()){ %>		
		<!-- footer -->
		
		<div id="footer">
		
			<div class="float_left">Copyright © <%= date("Y") %> Arashi Project. All rights reserved.&nbsp;&nbsp;&nbsp;Powered by Arashi v1.1.2</div>
			
			<div class="float_right">
            <a href='<%= get_option("home") %>/'>Home</a>
			   <%--<?php i_menu('menu_footer', false); ?>--%>
			</div>
		
		</div>
		
		<!-- / footer -->
	</div>
	

	
	<%--<script src='<%= bloginfo("stylesheet_directory") %>/js/imgpreview.0.22.jquery.js' type="text/javascript"></script>
	<script type="text/javascript">
		jQuery(document).ready(function(){
			jQuery('.imgpreview').imgPreview();
		});
	</script>--%>
<% } %>
<%= wp_footer() %>
<%= Plugin.GoogleAnalytics() %>
	
</body>
</html>
