<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.TemplateBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>
<% get_header(); %>
<div id="content">

<div class="content_wrap">
   <div class="full-entry">

	   <% if (have_posts()) { %>
         <% foreach (Post post in Model.Posts) { the_post(post); %>

            <div class="date"><p><%= the_time("M") %><span><%= the_time("j") %></span></p></div>
		      <div class="comments_tally"><%= comments_number(Resource("Comment_CommentsNumber_Zero"), Resource("Comment_CommentsNumber_One"), Resource("Comment_CommentsNumber_More")) %></div>

            <div class="title">
		         <h1 id="post-<%= the_ID() %>"><a href="<%= the_permalink() %>" rel="bookmark" title="Permanent Link to <%= the_title_attribute() %>"><%= the_title() %></a></h1>
		         <h3><%= Resource("Post_PostedBy") %> <%= the_author_posts_link(post.Author) %> <%= Resource("Post_PostedIn") %>: <span><%= the_category(", ") %> | <%= the_tags("Tags: ", ", ", "") %></span></h3>
		      </div> 

            <div class="post">
            <%= the_content("Read More") %>
            </div><!-- .post -->
            <% comments_template(); %>
         <% } %>
      <% } %>
   </div><!-- .content_wrap -->
<!--</div> .full-entry-->

<% get_sidebar(); %>
<% get_footer(); %>
