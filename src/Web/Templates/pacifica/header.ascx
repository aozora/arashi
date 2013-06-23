<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.TemplateBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" lang="it-IT">
<head>
   <meta http-equiv="Content-Type" content='<%= bloginfo("html_type") %>; charset=<%= bloginfo("charset") %>' />
   <title><%= wp_title("&laquo;", true, "right") %></title>
   <%-- TODO: usare telerik to combine!!! --%>
   <link rel="stylesheet" href='<%= bloginfo("stylesheet_url") %>' type="text/css" media="screen" />
   <link rel="stylesheet" href='<%= bloginfo("stylesheet_directory") %>/pagenavi-css.css' type="text/css" media="screen" />
   <% if (is_contact()) { %>
   <link rel="stylesheet" href='<%= bloginfo("stylesheet_directory") %>/library/styles/form.css' type="text/css" media="screen" />
   <% } %>
   <link rel="shortcut icon" href='<%= bloginfo("template_directory") %>/library/img/page/favicon.gif' type="image/gif" />
   <link rel="stylesheet" href='<%= bloginfo("template_directory") %>/library/styles/custom/skyblue.css' type="text/css" media="screen" />
   <link rel="alternate" type="application/rss+xml" title='<%= bloginfo("name") %> RSS Feed' href='<%= bloginfo("rss2_url") %>' />
   <link rel="alternate" type="application/atom+xml" title='<%= bloginfo("name") %> Atom Feed' href='<%= bloginfo("atom_url") %>' />
   <link rel="pingback" href='<%= bloginfo("pingback_url") %>' />

   <%= wp_head() %>

   <!--[if lte IE 6]>
   <link rel="stylesheet" type="text/css" href='<%= bloginfo("template_directory") %>/library/styles/hacks/ie6.css' />
   <![endif]-->
   <script type="text/javascript" src='<%= bloginfo("template_directory") %>/library/scripts/jquery-1.3.2.min.js'></script>
   <script type="text/javascript" src='<%= bloginfo("template_directory") %>/library/scripts/jquery.pngFix.pack.js'></script>
   <% if (is_contact()) { %>
   <script type="text/javascript" src='<%= bloginfo("template_directory") %>/Resources/js/jquery.form.min.js'></script>
   <% } %>
   <%--
   <script type="text/javascript" src='<%= bloginfo("template_directory") %>/library/scripts/jquery.jcarousel.pack.js'></script>
   <script type="text/javascript" src='<%= bloginfo("template_directory") %>/library/scripts/jquery.prettyPhoto.js'></script>
   <script type="text/javascript" src='<%= bloginfo("template_directory") %>/library/scripts/jquery.flow.1.2.min.js'></script>
   --%>
   <script type="text/javascript" src='<%= bloginfo("template_directory") %>/library/scripts/pacifica.js'></script>
</head>
<body <%= body_class() %>>
   <!-- Start Navigation -->
   <div id="navwrap">
      <div class="navigation">
         <div class="border">
            <ul id="nav">
               <li class="first"><a href='<%= get_option("home") %>'>Home</a></li>
               <%--<li><a href='<%= get_option("home") %>/author/marcellop/'>Info</a></li>--%>
               <%= wp_list_pages("title_li=") %>
               <li><a href='<%= get_option("home") %>/contact/'>Contatti</a></li>
               <li>
                  <img class="icon" alt="subscribe" src='<%= bloginfo("template_directory") %>/library/img/navigation/subscribe_icon.jpg' />
                  <a style="float: left;" href='<%= bloginfo("rss2_url") %>'>Sottoscrivi news</a>
                  <%--<ul>
                     <li class="first"><a href='<%= bloginfo("rss2_url") %>' title="Subscribe To my Posts RSS Feed" target="_blank">Posts</a></li>
                     <li><a href='<%= bloginfo("comments_rss2_url") %>' title="Subscribe To my Comments RSS Feed" target="_blank">Comments</a></li>
                     <li><a href='<%= bloginfo("rss2_url") %>' title="Subscribe To my Posts RSS Feed" target="_blank">RSS Feeds</a></li>
                     <li><a href='<%= bloginfo("atom_url") %>' title="Subscribe To my Posts ATOM Feed" target="_blank">ATOM Feeds</a></li>
                  </ul>--%>
               </li>
		    	</ul>

            <% get_search_form(); %>
            <div class="clear"></div>
         </div>
      </div>
   </div>
   <!-- Stop Navigation -->
   
   <div id="wrap">
   
      <!-- Start Page -->
		<div id="page">
			<!-- Header -->
			<div class="header">
			   <% if (!is_single()) { %>
			      <h1>
			   <% } %>
			   <a title='<%= bloginfo("name") %>' href='<%= get_option("home") %>/'>
			      <img src='<%= bloginfo("template_directory") %>/library/img/page/logo_usabilityisafact.png' alt='<%= bloginfo("name") %>' />
			   </a>
			   <% if (!is_single()) { %>
			      </h1>
			   <% } %>
			</div>   
