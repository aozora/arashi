<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.TemplateBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>
<div id="sidebar">
<div class="sidebar_wrap">
	<ul class="xoxo">
	<% if ( !dynamic_sidebar(null, null, "<h3 class=\"widgettitle\">", "</h3>")) { %>
		<%--<?php wp_list_pages('title_li=<h2>Pages</h2>' ); ?>--%>

		<li><h2><%= Resource("Label_Archives") %></h2>
			<ul>
			<%= wp_get_archives("type=monthly") %>
			</ul>
		</li>

		<%= wp_list_categories("show_count=1&title_li=<h2>Categories</h2>") %>

      <li><h2><%= Resource("Label_Tags") %></h2>
         <ul>
            <li>
               <%= wp_tag_cloud(null) %>
            </li>
         </ul>
      </li>

		<% /* If this is the frontpage */ if ( is_home() || is_page() ) { %>
			<%= wp_list_bookmarks() %>

			<li><h2><%= Resource("Label_Meta") %></h2>
			<ul>
			<%--<?php wp_register(); ?>
				<li><?php wp_loginout(); ?></li>--%>
		      <li><a href='<%= bloginfo("rss2_url") %>'>Feed (RSS)</a></li>
		      <li><a href='<%= bloginfo("atom_url") %>'>Feed (ATOM)</a></li>
				<li><a href="http://validator.w3.org/check/referer" title="This page validates as XHTML 1.0 Transitional">Valid <abbr title="eXtensible HyperText Markup Language">XHTML</abbr></a></li>
				<%--<?php wp_meta(); ?>--%>
			</ul>
			</li>
		   <%--<?php } ?--%>

		<% } %>

   <% } %>
      <li><h3>Usability Blogroll</h3>
         <ul class="xoxo blogroll">
            <li>
               <a href="http://www.useit.com/" title="useit.com: usable information technology ">useit.com</a>
            </li>
            <li>
               <a href="http://www.uxmatters.com/" title=">UXmatters :: Insights and inspiration for the user experience community">UXmatters</a>
            </li>
            <li>
               <a href="http://www.lukew.com/ff/index.asp" title="Functioning Form - Interface Design Blog">Functioning Form</a>
            </li>
            <li>
               <a href="http://www.alistapart.com/" title="A List Apart: for people who make websites">A List Apart</a>
            </li>
            <li>
               <a href="http://www.inspireux.com/" title="inspireUX - words to inspire user experience designers">inspireUX</a>
            </li>
            <li>
               <a href="http://konigi.com/" title="Konigi">Konigi</a>
            </li>
            <li>
               <a href="http://iloveusability.com/" title="I Love Usability">I Love Usability</a>
            </li>
            <li>
               <a href="http://www.usability.gov/" title="">Usability.gov</a>
            </li>
         </ul>
      </li>
      <li><h3>Friends</h3>
         <ul class="xoxo blogroll">
            <li>
               <a href="http://www.fugini.net/" title="internet e dintorni">internet e dintorni</a>
            </li>
            <li>
               <a href="http://www.devleap.it/" title="DevLeap - Bridge the gap!">DevLeap</a>
            </li>
            <li>
               <a href="http://blogs.ugidotnet.org/" title="UGIdotNET">UGIdotNET</a>
            </li>
            <li>
               <a href="http://www.asp.net/mVC/" title="Microsoft ASP.NET MVC">ASP.NET MVC</a>
            </li>
         </ul>
      </li>
	</ul>
</div>
</div>
