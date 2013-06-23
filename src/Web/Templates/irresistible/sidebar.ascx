<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.TemplateBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>
<div id="sidebar" >

   <!-- Sidebar Tabs -->
   <% Html.RenderPartial(Model.Site.Template.BasePath + "/includes/tabs.ascx"); %>

   <!-- Add you sidebar manual code here to show above the widgets -->
   <div class="widgetized">
      <% dynamic_sidebar("<div class=\"\" id=\"%1$s\">", "</div></div>", "<h3>", "</h3><div class=\"list3 box1\">"); %>
   </div>	           
   <!-- Add you sidebar manual code here to show below the widgets -->	

<%--
   <!-- Sidebar Video -->    
   <?php if ( get_option('woo_video') == 'false' ) include ( TEMPLATEPATH . "/includes/video.php" ); ?>
--%>


    <!-- Add you sidebar manual code here to show above the widgets -->
    <div class="widgetized">
		<%--<?php if (function_exists('dynamic_sidebar') && dynamic_sidebar(1) )  ?>--%>	
    </div>	           
    <!-- Add you sidebar manual code here to show below the widgets -->			
                
</div><!-- / #sidebar -->
