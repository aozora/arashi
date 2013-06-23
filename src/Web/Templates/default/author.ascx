<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.TemplateBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>
<% get_header(); %>

<div id="content" class="narrowcolumn" >

   <h2 class="pagetitle"><%= Html.Encode(Model.CurrentAuthor.DisplayName) %></h2>


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
      <li>
         <h3 class="pagetitle">Posts by <%= Html.Encode(Model.CurrentAuthor.DisplayName) %></h3>
      </li>
     
      <% foreach (Post post in Model.Posts) { the_post(post); %>
	   <div <%= post_class() %>>
				<h3 id='post-<%= the_ID() %>'><a href='<%= the_permalink() %>' rel="bookmark" title='Permanent Link to <%= the_title_attribute() %>'><%= the_title() %></a></h3>
				<small><%= the_time("l, F jS, Y") %></small>
				<div class="entry">
					<%= the_content() %>
				</div>
				<p class="postmetadata"><%= the_tags("Tags: ", ", ", "<br />") %> Posted in <%= the_category(", ") %> | <%--<%= edit_post_link("Edit", "", " | ") %>--%>  <%= comments_popup_link("No Comments &#187;", "1 Comment &#187;", "% Comments &#187;") %></p>
	   </div>
   <% } %>

		<div class="navigation">
			<div class="alignleft"><%= next_posts_link("&laquo; Older Entries") %></div>
			<div class="alignright"><%= previous_posts_link("Newer Entries &raquo;") %></div>
			<%--<%= wp_pagenavi()%>--%>
		</div>
   <% } %>
</div>

<% get_sidebar(); %>
<% get_footer(); %>
