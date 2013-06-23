<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.TemplateBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>
<% get_header(); %>

	<div id="content">
		
		<div id="main">
						
         <h3 id="myWritings" class="replace"><%= Html.Encode(Model.CurrentAuthor.DisplayName) %></h3>
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
         <br />

			<% if (have_posts()) { %>
														
            <% foreach (Post post in Model.Posts) { the_post(post); %>

                 <div class="box1 clearfix">
                     <div class="post clearfix">
                         <h3 id="H1"><a href="<%= the_permalink() %>" rel="bookmark" title="Permanent Link to <%= the_title_attribute() %>"><%= the_title() %></a></h3>
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
