<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.TemplateBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>
<% get_header(); %>

    <div id="content">
    
        <div id="main">
        
            <div class="box1 clearfix">
                <div class="post clearfix">
            	 
                <% if (have_posts()) { %>
                  <% foreach (Post post in Model.Posts) { the_post(post); %>

                   <h2><%= the_title() %></h2> 
                   <p class="txt0"><%= the_time("F jS, Y")%> // <%= the_category(", ") %></p>
                           
                   <%= the_content("<span class='continue'>Continue Reading</span>")%>
                
                  <% } %>
               <% } %>

                </div>

            </div>
                            
   	      <% comments_template(); %>
            
        </div><!-- / #main -->
			
      <% get_sidebar(); %>
			
	</div><!-- / #content -->

<% get_footer(); %>
