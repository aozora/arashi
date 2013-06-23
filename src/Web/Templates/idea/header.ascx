<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.TemplateBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
   <meta http-equiv="Content-Type" content='<%= bloginfo("html_type") %>; charset=<%= bloginfo("charset") %>' />
   <meta http-equiv="Content-Type" content='<%= bloginfo("html_type") %>; charset=<%= bloginfo("charset") %>' />
   <title><%= wp_title("|", true, "right") %></title>
   <link rel="shortcut icon" href='<%= bloginfo("stylesheet_directory") %>/images/favicon.gif' type="image/gif" />
   <link rel="pingback" href='<%= bloginfo("pingback_url") %>' />
   <link rel="alternate" type="application/rss+xml" title='<%= bloginfo("name") %> RSS Feed' href='<%= bloginfo("rss2_url") %>' />

   <% wp_enqueue_style("style", "/style.css"); %>   

   <%--<link rel="stylesheet" href='<%= bloginfo("stylesheet_url") %>' type="text/css" />--%>
   <!--[if IE 6]>
	   <link rel="stylesheet" href='<%= bloginfo("stylesheet_directory") %>/ie6.css' type="text/css" />
   <![endif]-->
   <!--[if IE 7]>
	   <link rel="stylesheet" href='<%= bloginfo("stylesheet_directory") %>/ie7.css' type="text/css" />
   <![endif]-->
   <%--<script src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js" type="text/javascript"></script>--%>
   <% wp_enqueue_script("jquery"); %>

<% if (is_home()) { %>
   <% wp_enqueue_script("promo", "/js/promo.js"); %>
   <% wp_enqueue_script("jtwitter", "/js/jquery.jtwitter.min.js"); %>
<% } %>

<% if (is_page("screenshots")) { %>
   <% wp_enqueue_style("style",  "/js/colorbox/colorbox.css"); %>   
   <% wp_enqueue_script("colorbox", "/js/colorbox/jquery.colorbox-min.js"); %>
   <% wp_enqueue_script("flickr-gallery", "/js/flickr-gallery.js"); %>
<% } %>

<% if (is_contact()) { %>
   <% wp_enqueue_script("jquery.form"); %>
   <% wp_enqueue_script("contact"); %>
<% } %>
   
   <!--[if IE 6]>
   <script src='<%= bloginfo("stylesheet_directory") %>/js/DD_belatedPNG_0.0.7a.js' type="text/javascript"></script>
   <script src='<%= bloginfo("stylesheet_directory") %>/js/png_fix_elements.js' type="text/javascript"></script>
   <![endif]-->

   <%= wp_head() %>
   <% wp_enqueue_style_render(); %>
</head>

<body <%= is_home() ? "class='coda'" : ""  %> >

<% if (!is_404()) { %>
	<div id="main_container">
		<h1 class="logo"><a id="logo" href='<%= get_option("home") %>/'><%= get_option("name")%> - <%= bloginfo("description")%></a></h1>
		<ul id="main_menu">
		   <li class="page_item page_item-home">
		      <a href='<%= get_option("home") %>/'>Home</a>
		   </li>
		   <%= wp_list_pages("sort_column=menu_order&depth=1&title_li=&exclude=")%>
         
         <li>
            <a href="/contact" class="">Contact Us</a>
         </li>
      </ul>
		
<%--		<div id="login_form_container">
			<div id="login_form_left"></div>
			<div id="login_form_right">
			
				<form id="login" action="">
					<div><strong>Login: </strong>
					<input type="text" name="username" id="username" value="username" />
					<input type="password" name="password" id="password" value="password" />
					<input type="button" value="GO" id="login_submit" /></div>
				</form>
			</div>
		</div>--%>
<% } %>
