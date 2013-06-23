<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.TemplateBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>
<div id="sidebar">
	<ul>
		<% /* Widgetized sidebar, if you have the plugin installed. */
			if ( !dynamic_sidebar()) { %>
   			<li>
   				<% get_search_form(); %>
   			</li>

   			<!-- Author information is disabled per default. Uncomment and fill in your details if you want to use it.
   			<li><h2>Author</h2>
   			<p>A little something about you, the author. Nothing lengthy, just an overview.</p>
   			</li>
   			-->
   			
   		   <!-- see original .... some php code -->
            
   			<%--<?php wp_list_pages('title_li=<h2>Pages</h2>' ); ?>--%>

   			<li><h2>Archives</h2>
   				<ul>
   				<%= wp_get_archives("type=monthly") %>
   				</ul>
   			</li>

   			<%= wp_list_categories("show_count=1&title_li=<h2>Categories</h2>") %>
   
            <li><h2>Popular tags</h2>
               <ul>
                  <li>
                     <%= wp_tag_cloud(null) %>
                  </li>
               </ul>
            </li>
   
   			<% /* If this is the frontpage */ if ( is_home() || is_page() ) { %>
   				<%= wp_list_bookmarks() %>

   				<li><h2>Meta</h2>
   				<ul>
   				<%--<?php wp_register(); ?>
   					<li><?php wp_loginout(); ?></li>--%>
   			      <li><a href='<%= bloginfo("rss2_url") %>'>Entries (RSS)</a></li>
   			      <li><a href='<%= bloginfo("atom_url") %>'>Entries (ATOM)</a></li>
   					<li><a href="http://validator.w3.org/check/referer" title="This page validates as XHTML 1.0 Transitional">Valid <abbr title="eXtensible HyperText Markup Language">XHTML</abbr></a></li>
   					<li><a href="http://gmpg.org/xfn/"><abbr title="XHTML Friends Network">XFN</abbr></a></li>
   					<%--<?php wp_meta(); ?>--%>
   				</ul>
   				</li>
   			   <%--<?php } ?--%>

   			<% } %>
   
      <% } %><%-- End Static sidebar --%>
   </ul>
</div>