<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.TemplateBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>
<div class="clear"></div>
</div><!-- Stop Content WRAP -->
</div><!-- End Content -->

<!-- Footer NAV  Navigator -->
<div class="footer_nav">
<div class="list">

<div class="cen">
   <% if (!is_home()) { %>
   <a href='<%= bloginfo("home") %>'><%= Resource("Link_ReturnToTheHomePage") %></a>
   <% } %>
</div>

</div>
<div class="clear"></div>
</div>

</div>
<!-- End Page -->
		
<!-- Start Footer -->
<div id="footer">
<!-- Start Footer Wrap -->
<div class="footerWrap">

	<%--<?php if (get_option('pacifica_footerads') == 'yes' ) {?>
	<div class="footerAds"> 
	<a href="<?php echo get_option('pacifica_footerad_long_url'); ?>"><img src="<?php echo get_option('pacifica_footerad_long'); ?>" alt="Ad" /></a>
	<a href="<?php echo get_option('pacifica_footerad_1_url'); ?>"><img src="<?php echo get_option('pacifica_footerad_1'); ?>" alt="Ad" class="square" /></a>
	<a href="<?php echo get_option('pacifica_footerad_2_url'); ?>"><img src="<?php echo get_option('pacifica_footerad_2'); ?>" alt="Ad" class="square" /></a>
	</div>
	<?php } ?>--%>

	<div class="copyright">
	&#169; 2010 Marcello Palmitessa. All Rights Reserved. 
	</div>

	<div class="footer_links">
	   <ul>                
	   <%--
	      <li <?php if (is_front_page()){echo 'class="current_page_item"';} ?> ><a href="<?php echo get_settings('home'); ?>">Home</a></li>
	      <?php wp_list_pages('title_li=&sort_column=menu_order&include='.get_option('pacifica_footer_include')); ?>
	   --%>
	      <li>
		      &quot;<%= bloginfo("name") %>&quot; is powered by&nbsp;<a href="http://www.arashi-project.com/"><strong>Arashi</strong></a>
	      </li>
	   </ul>
	</div>
		
</div>
<!-- End Footer Wrap -->

</div>
<!-- End Footer -->
</div>
<!-- End Global Wrap -->
	

<%= wp_footer() %>
<%= Plugin.GoogleAnalytics() %>
</body>
</html>
