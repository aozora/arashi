<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.TemplateBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>
<% get_header(); %>

	<div id="content">
		
			<div id="main">
						
                <h3 id="myWritings" class="replace">My Writings. My Thoughts.</h3>

					<% if (have_posts()) { %>
															
               <% foreach (Post post in Model.Posts) { the_post(post); %>

                    <div class="box1 clearfix">
                        <div class="post clearfix">
                            <h3 id="post-<%= the_ID() %>"><a href="<%= the_permalink() %>" rel="bookmark" title="Permanent Link to <%= the_title_attribute() %>"><%= the_title() %></a></h3>
                            <p class="txt0"><%= the_time("F jS, Y") %> // <%= comments_popup_link("No Comments &#187;", "1 Comment &#187;", "% Comments &#187;") %> // <%= the_category(", ") %></p>
                        
                           <%= the_content("<span class='continue'>Continue Reading</span>") %>
    
                        </div>
                    </div>
					
               <% } %>
					
					<div class="navigation nav clearfix">
						<div class="fl"><%= next_posts_link("&laquo; Older Entries") %></div>
						<div class="fr"><%= previous_posts_link("Newer Entries &raquo;") %></div>
					</div>
					<% } else { %>
					
					<h2 class='center'>No posts found</h2>
					
					<% } %>

	
            </div><!-- / #main -->
		
        <% get_sidebar(); %>
        
	</div><!-- / #content -->

<% get_footer(); %>
