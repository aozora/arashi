<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.TemplateBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
   <title><%= wp_title("&laquo;", true, "right") %></title>
   <meta http-equiv="Content-Type" content='<%= bloginfo("html_type") %>; charset=<%= bloginfo("charset") %>' />
   <link rel="stylesheet" href='<%= bloginfo("stylesheet_url") %>' type="text/css" media="screen" />
   <!--[if lte IE 7]><link rel="stylesheet" type="text/css" href="<%= bloginfo("stylesheet_directory") %>/ie.css" /><![endif]-->
   <script type="text/javascript" src="<%= bloginfo("stylesheet_directory") %>/includes/js/jquery-1.3.2.min.js"></script>
   <script type="text/javascript" src="<%= bloginfo("stylesheet_directory") %>/includes/js/tabs.js"></script>
   <script type="text/javascript" src="<%= bloginfo("stylesheet_directory") %>/includes/js/superfish.js"></script>
   <!--[if IE 6]>
   <script src="<%= bloginfo("stylesheet_directory") %>/includes/js/pngfix.js"></script>
   <script src="<%= bloginfo("stylesheet_directory") %>/includes/js/menu.js"></script>
   <![endif]-->

   <link rel="alternate" type="application/rss+xml" title='<%= bloginfo("name") %> RSS Feed' href='<%= bloginfo("rss2_url") %>' />
   <link rel="pingback" href='<%= bloginfo("pingback_url") %>' />

   <%= wp_head() %>

   <script type="text/javascript">
      jQuery("#idTabs").idTabs(); 
   </script>            

</head>

<body id="woothemes">

	<div id="wrap">

		<div id="header">
			
            <ul id="nav" class="nav">
   				<li<% if ( is_home() ) { %> class="current_page_item"<% } %>><a href="<%= get_option("home") %>/">Home</a></li>
   				<%=  /*if (get_option('woo_nav') == 'true' ) 
                       wp_list_categories('sort_column=menu_order&depth=3&title_li=&exclude='); 
                   else*/
                       wp_list_pages("sort_column=menu_order&depth=3&title_li=&exclude=")
                   %>
               <li><a href='<%= get_option("home") %>/contact/'>Contact us</a></li>
   			</ul>

			<form id="topSearch" class="search" method="get" action="<%= get_option("home") %>/search/">
					
				<p class="fields">
					<input type="text" value="search" name="s" id="s" onfocus="if (this.value == 'search') {this.value = '';}" onblur="if (this.value == '') {this.value = 'search';}" />
					<button class="replace" type="submit" name="submit">Search</button>
				</p>

			</form>
			
            <div id="logo">
                <a href="<%= bloginfo("url") %>" title="<%= bloginfo("description") %>"><img class="title" src="<%= bloginfo("template_directory") %>/images/logo.png" alt="<%= bloginfo("name") %>" /></a>
                <h1 class="replace"><a href="<%= bloginfo("url") %>"><%= bloginfo("name") %></a></h1>	
            </div>
            
			<div id="hi">
			
				<p class="nomargin">This is the personal blog and thoughts of Woo Member, who lives in Never Neverland. Browse around to see my stuff or follow my thoughts…</p>
				
				<%--<p><a href="<?php echo stripslashes( get_option( 'woo_aboutlink' ) ); ?>"><?php _e('read more',woothemes); ?></a></p>--%>
			
			</div>
			
		</div>
		