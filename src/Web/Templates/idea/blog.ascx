<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.TemplateBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>
<% get_header(); %>
		<!-- coda slider -->
		
        <div id="slider" class="blog">

            <div class="scroll blog">
			
				<div id="blog_header_top">&nbsp;</div>
			
                <div class="scrollContainer blog">
				
					<div class="panel" id="panel_01">

						<div id="blog_header_midtop">&nbsp;</div>
					
						<h2 class="title main" id="title-<%= Html.Encode(Model.CurrentPage.FriendlyName) %>">
                     <%= Html.Encode(Model.CurrentPage.Title) %>
                     <% if (Model.CurrentPage.CustomFields.ContainsKey("desc")) { %>
                        <span><%= Html.Encode(this.get_post_meta(Model.CurrentPage.Id, "desc", false)) %></span>
                     <% } %>
                  </h2>
						
						<hr />
					
                  <% if (have_posts()) { %>
						<div class="panel_left">
						
                     <% foreach (Post post in Model.Posts) { the_post(post); %>
						
							<div class="post">
							
								<h3> <em><%= comments_popup_link("0", "1", "%") %><span> comments for post:</span></em> <strong>&quot;</strong><a href="<%= the_permalink() %>" class="post_link"><%= the_title() %></a><strong>&quot;</strong> <ins>|</ins> <span>Posted on <%= the_time("d. m.") %> by	<a href="#author" class="author"><%= the_author() %></a></span></h3>
								
								<div class="content">
								
									<%--<?php the_post_thumbnail( 'blog-thumbnail', array('class' => "main" ) ); ?>--%>
									<%= the_content("") %>
									
									<div class="meta">Posted in <%= the_category(", ") %></div>
                        </div>
							</div>

                     <% } %>
								
							<%--<?php if(function_exists('wp_pagenavi')): ?>--%>
							<%= wp_pagenavi()%>
							<%--<div class="navigation">
			               <div class="alignleft"><%= next_posts_link("&laquo; Older Entries") %></div>
			               <div class="alignright"><%= previous_posts_link("Newer Entries &raquo;") %></div>
							</div>--%>
							
						</div>
                  <% } else { %>
					
							<h2 class="center">Not Found</h2>
							<p class="center">Sorry, but you are looking for something that isn't here.</p>
							<% get_search_form(); %>
					
                  <% } %>
						
						
						
						<div class="panel_right">
						
							<div id="sidebar">
							
								<div id="sidebar_top">&nbsp;</div>
								
								<div id="sidebar_main">
								
									<%--<?php get_sidebar('blog'); ?>--%>
                           <% get_sidebar(); %>
									
								</div>
								
								<div id="sidebar_bottom">&nbsp;</div>
							
							</div>
						
						</div>
					
					</div>
					
				</div>
				
			</div>

        </div>
        
        <div class="scrollBottom round">&nbsp;</div>
		
		<!-- / coda slider -->
		
		<!-- bottom container -->
		
		<%--<?php get_sidebar('bottom'); ?>--%>
				
		<!-- / bottom container -->
		
<% get_footer(); %>
