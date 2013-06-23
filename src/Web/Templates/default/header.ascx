<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.TemplateBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" lang="en-US">

<head>
<meta http-equiv="Content-Type" content='<%= bloginfo("html_type") %>; charset=<%= bloginfo("charset") %>' />
<title><%= wp_title("&laquo;", true, "right") %></title>
<link rel="stylesheet" href='<%= bloginfo("stylesheet_url") %>' type="text/css" media="screen" />
<link rel="stylesheet" href='<%= bloginfo("stylesheet_directory") %>/pagenavi-css.css' type="text/css" media="screen" />
<% if (is_contact()) { %>
<link rel="stylesheet" href='<%= bloginfo("stylesheet_directory") %>/form.css' type="text/css" media="screen" />
<% } %>
<link rel="alternate" type="application/rss+xml" title='<%= bloginfo("name") %> RSS Feed' href='<%= bloginfo("rss2_url") %>' />
<link rel="alternate" type="application/atom+xml" title='<%= bloginfo("name") %> Atom Feed' href='<%= bloginfo("atom_url") %>' />
<link rel="pingback" href='<%= bloginfo("pingback_url") %>' />
<% if (is_contact()) { %>
<script type="text/javascript" src='<%= bloginfo("template_directory") %>/Resources/js/jquery.min.js'></script>
<script type="text/javascript" src='<%= bloginfo("template_directory") %>/Resources/js/jquery.form.min.js'></script>
<% } %>

<style type="text/css" media="screen">

<% if ( !is_single() ) { %>
	#page { background: url('<%= bloginfo("stylesheet_directory") %>/images/kubrickbg-<%= bloginfo("text_direction") %>.jpg') repeat-y top; border: none; }
<% } else { %>
	#page { background: url('<%= bloginfo("stylesheet_directory") %>/images/kubrickbgwide.jpg') repeat-y top; border: none; }
<% } %>

</style>

<%--<php if ( is_singular() ) wp_enqueue_script( 'comment-reply' ); >--%>

<%= wp_head() %>
</head>
<body <%= body_class() %>>
<div id="page">


<div id="header">
	<div id="headerimg">
		<h1><a href='<%= get_option("home") %>/'><%= bloginfo("name") %></a></h1>
		<div class="description"><%= bloginfo("description") %></div>
	</div>
</div>
<hr />