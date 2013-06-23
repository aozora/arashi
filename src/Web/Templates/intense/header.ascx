<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.TemplateBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
   <meta http-equiv="Content-Type" content='<%= bloginfo("html_type") %>; charset=<%= bloginfo("charset") %>' />
   <title><%= wp_title("&laquo;", true, "right") %> <%= bloginfo("name") %></title>
   <link rel="shortcut icon" href='<%= bloginfo("template_url") %>/favicon.gif' type="image/gif" />
   <link rel="stylesheet" type="text/css" media="all" href='<%= bloginfo("template_url") %>/css/reset.css' />
   <link rel="stylesheet" type="text/css" media="all" href='<%= bloginfo("template_url") %>/css/960.css' />
   <link rel="stylesheet" type="text/css" href='<%= bloginfo("template_url") %>/css/superfish.css' media="screen" />
   <%--<link rel="stylesheet" href='<%= bloginfo("stylesheet_url") %>' type="text/css" media="screen" /> --%>
   <link rel="stylesheet" type="text/css" href='<%= bloginfo("template_directory") %>/css/lightstyle.css' media="screen" /> 

   <%-- TODO: <%= if ( is_singular() ) wp_enqueue_script( 'comment-reply' ); %>--%>
   
   <%= wp_head() %>

   <script type="text/javascript" src='<%= bloginfo("template_url") %>/js/jquery.min.js'></script>
   <script type="text/javascript" src='<%= bloginfo("template_url") %>/js/jquery.easing.min.js'></script>
   <script type="text/javascript" src='<%= bloginfo("template_url") %>/js/jquery.hoverIntent.min.js'></script>
   <script type="text/javascript" src='<%= bloginfo("template_url") %>/js/jquery.hslides.min.js'></script>
   <script type="text/javascript" src='<%= bloginfo("template_url") %>/js/custom.js'></script>

   <!--[if lt IE 7]>
   <script src='<%= bloginfo("template_url") %>/DD_belatedPNG.js'></script>
   <script>
     /* EXAMPLE */
     DD_belatedPNG.fix('div,img, a img, *');
     
     /* string argument can be any CSS selector */
     /* .png_bg example is unnecessary */
     /* change it to what suits you! */
   </script>
   <![endif]-->
</head>

<body>
<!-- start:header -->

<div class="header">

	<!-- start:container_16 -->
	<div class="container_16">
    	<!-- start:logo -->
		<div class="grid_4">
         <h1><a href='<%= get_option("home") %>/' class="logo">Intense</a></h1>
		</div>
    	<!-- end:logo -->
    	<!-- start:header right -->
    	<div class="grid_12">
       	<!-- start:top-menu -->		
         <div class="top-menu">
        		<ul class="sf-menu">
          	   <li>
          	      <a href='<%= get_option("home") %>/'>
          	         Home
          	         <span>Main Homepage</span>
          	      </a>
          	   </li>
          	   <li>
          	      <a href='<%= get_option("home") %>/page/features/'>
          	         Features
          	         <span>See what's hot</span>
          	      </a>
          	   </li>
          	   <li>
          	      <a href='<%= get_option("home") %>/page/download/'>
          	         Download
          	         <span>Get the files</span>
          	      </a>
          	   </li>
          	   <li>
          	      <a href='<%= get_option("home") %>/page/about/'>
          	         About
          	         <span>Who we are</span>
          	      </a>
          	   </li>
          	   <li>
          	      <a href='<%= get_option("home") %>/contact/'>
          	         Contact
          	         <span>Get in touch</span>
          	      </a>
          	   </li>
            </ul>
         </div>
   		<!-- end:top-menu -->
      </div>
    	<!-- end:header right -->
	</div>
	<!-- end:container_16 -->
</div>
<!-- end:header -->
<div class="clear"></div>
