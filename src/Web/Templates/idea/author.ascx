<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.TemplateBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>
<% get_header(); %>
		<!-- coda slider -->
		
        <div id="slider" class="blog">

            <div class="scroll blog">
			
				<div id="blog_header_top">&nbsp;</div>
			
                <div class="scrollContainer blog">

					<div class="panel" id="panel_01">

						<div id="blog_header_midtop">&nbsp;</div>
					
						<h2 class="title main" ><%= Html.Encode(Model.CurrentAuthor.DisplayName)%></h2>
						
						<hr />
					
						<div class="panel_left">
						
                     <div class="profile">
                        <div class="avatar alignleft"><%= get_avatar(Model.CurrentAuthor.Email , 128) %></div>
                        <div class="info">
                           <p>
                           <% if (string.IsNullOrEmpty(Model.CurrentAuthor.Description)) { %>
                           This user hasn't shared any biographical information
                           <% } else { %>
                           <%= Html.Encode(Model.CurrentAuthor.Description) %>
                           <% } %>
                           </p>
                           <% if (!string.IsNullOrEmpty(Model.CurrentAuthor.WebSite) && Model.CurrentAuthor.WebSite != "http://") { %> 
                              <p class="im">Homepage: <a href='<%= Html.Encode(Model.CurrentAuthor.WebSite) %>'><%= Html.Encode(Model.CurrentAuthor.WebSite) %></a></p>
                           <% }%>
                           <div class="clear"></div>
                        </div>
                        <div class="clear"></div>
                     </div>

						
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
