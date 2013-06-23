<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.TemplateBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>
<% get_header(); %>
		<!-- coda slider -->
		
        <div id="slider" class="blog">

            <div class="scroll blog">
			
				<div id="blog_header_top">&nbsp;</div>
			
                <div class="scrollContainer blog">
				
					<div class="panel" id="panel_01">

						<div id="blog_header_midtop">&nbsp;</div>
					
                  <% if (Model.CurrentPage.ParentPage != null) { %>
						   <h2 class="title main" id="title-<%= Html.Encode(Model.CurrentPage.ParentPage.FriendlyName) %>">
                        <%= Html.Encode(Model.CurrentPage.ParentPage.Title)%>
                        <% if (Model.CurrentPage.ParentPage.CustomFields.ContainsKey("desc")) { %>
                           <span><%= Html.Encode(this.get_post_meta(Model.CurrentPage.ParentPage.Id, "desc", false))%></span>
                        <% } %>
                     </h2>
                     <% if (Model.CurrentPage.ParentPage.ChildPages.Count > 0) { %>
                        <ul id="subtitle">
                           <%= wp_list_pages("title_li=&depth=2&child_of=" + Model.CurrentPage.ParentPage.Id)%>
                        </ul>
                     <% } %>
                  <% } else { %>
						   <h2 class="title main" id="title-<%= Html.Encode(Model.CurrentPage.FriendlyName) %>">
                        <%= Html.Encode(Model.CurrentPage.Title) %>
                        <% if (Model.CurrentPage.CustomFields.ContainsKey("desc")) { %>
                           <span><%= Html.Encode(this.get_post_meta(Model.CurrentPage.Id, "desc", false)) %></span>
                        <% } %>
                     </h2>
                     <% if (Model.CurrentPage.ChildPages.Count > 0) { %>
                        <ul id="subtitle">
                           <%= wp_list_pages("title_li=&depth=2&child_of=" + Model.CurrentPage.Id)%>
                        </ul>
                     <% } %>
                  <% } %>

						
						<hr />
					
						<div class="panel_left">
						
							<%--<?php if (have_posts()) : while (have_posts()) : the_post(); ?>--%>
						
							<div class="post">
							
								<%--<h3> <em><?php comments_popup_link('0', '1', '%'); ?><span> comments for post:</span></em> <strong>&quot;</strong><a href="<? the_permalink() ?>" class="post_link"><?php the_title(); ?></a><strong>&quot;</strong> <ins>|</ins> <span>Posted on <?php the_time('d. m.') ?> by	<a href="#author" class="author"><?php the_author() ?></a></span></h3>--%>
								
								<div class="content">
								
									<%--<?php the_post_thumbnail( 'blog-thumbnail', array('class' => "main" ) ); ?>--%>
									<%= Model.CurrentPage.Content %>
									
								</div>
								
								<%--<?php comments_template(); ?>--%>
							
							</div>
						
							<%--<?php endwhile; endif; ?>--%>
						
						</div>
						
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
