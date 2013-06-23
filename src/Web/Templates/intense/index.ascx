<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.TemplateBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>
<% get_header(); %>

<%--<?php
/**
 * @package WordPress
 * @subpackage Default_Theme
 */

 $slideroption=get_option("slideroption");
if($slideroption == "slider2")
{
 include (TEMPLATEPATH."/alternateslider.php"); 
}else{
include (TEMPLATEPATH."/slider.php"); 
}
?>--%>
<% Html.RenderPartial(Model.Site.Template.BasePath + "/slider.ascx"); %>
<div class="clear"></div>

<!-- start:middlepart -->
<div class="middlepart">
   <div class="container_12 ">
   
	   <!-- Start:column -->
	   <div class="smallbox">
		   <h2>Powered by jQuery</h2>
         <span>The frontend user experience is built with the incredible javascript libraries jQuery and jQuery UI</span>
			<p class="boxicon">
			   <a href="http://jquery.com/">
               <img width="130" height="32" alt="Built on jQuery" src='<%= bloginfo("template_url") %>/images/jquery_logo.png' />
            </a>
         </p>
			<p class="boxicon">
			   <a href="http://jqueryui.com/">
               <img width="130" height="32" alt="Built on jQuery" src='<%= bloginfo("template_url") %>/images/jqueryui_logo.png' />
            </a>
         </p>
	      <p>The Control Panel make strong use of the <a href="http://jqueryui.com/docs/Theming/API#The_jQuery_UI_CSS_Framework">jQuery CSS Framework</a></p>
	   </div>
	   <!-- end:column -->
   		
	   <!-- Start:column -->
      <div class="smallbox">
         <h2>Browser Compatibility</h2>
         <span>Arashi project is compatible with all modern browsers</span>
         <p class="boxicon">
            <img width="190" height="70" alt="Browser Compatibility" src='<%= bloginfo("template_url") %>/images/browsers.png' />
         </p>
         <p>However the Control Panel can run only with the latest versions of modern web browsers, like Firefox 3, Google Chrome 3, Safari 4 or Internet Explorer 8.</p>
         <p>Due to the fact that we use some CSS level 3 enhancements, users of Internet Explorer will be provided with a lesser look & feel</p>
	   </div>
	   <!-- end:column -->

	   <!-- Start:column -->
	   <div class="smallbox">
         <div class="subnav"><h3>Latest News</h3>
            <ul class="xoxo blogroll">
				   <%= wp_get_archives("type=postbypost&limit=5")%>
            
               <%--<li><a href="http://wordpress.org/development/">Development Blog</a></li>
               <li><a href="http://codex.wordpress.org/">Documentation</a></li>
               <li><a href="http://wordpress.org/extend/plugins/">Plugins</a></li>
               <li><a href="http://wordpress.org/extend/ideas/">Suggest Ideas</a></li>
               <li><a href="http://wordpress.org/support/">Support Forum</a></li>
               <li><a href="http://planet.wordpress.org/">WordPress Planet</a></li>--%>
            </ul>
         </div>
	   </div>
	   <!-- end:column -->

   </div>
</div>
<!-- end:middle part -->

<div class="clear"></div>

<!-- start:footer -->
<div class="sitefooter">
	<div class="container_16">
		<div class="grid_16">
			&copy; 2010 by Marcello Palmitessa & the Arashi Team<br />
			<%--<a href="#">Terms &amp; Conditions</a> | <a href="#">Privacy Policy</a>--%>
		</div>
	</div>
</div>

<%= Plugin.GoogleAnalytics() %>
<!-- end:footer -->
</body>
</html>

