<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.TemplateBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>
<% get_header(); %>

<!-- Start Gallery - Refer to the documentation for the exact use of this. -->
<!-- End Gallery -->

<div class="clear"></div>
<div id="content">

   <!-- Start Info Bar -->
   <!-- End Info Bar -->

   <!-- Main Content -->
	<div class="content_wrap">
		<div class="entries">

         <% if (have_posts()) { %>
		   <ul><!-- BEGIN LOOP -->                 
            <% foreach (Post post in Model.Posts) { the_post(post); %>
		         <li <%= post_class() %>>
		            <div class="date"><p><%= the_time("M") %><span><%= the_time("j") %></span></p></div>
		            <div class="comments_tally"><a href="<%= the_permalink() %>"><%= comments_number(Resource("Comment_CommentsNumber_Zero"), Resource("Comment_CommentsNumber_One"), Resource("Comment_CommentsNumber_More")) %></a></div>

		            <div class="title">
		               <h2 id="post-<%= the_ID() %>"><a href='<%= the_permalink() %>' rel="bookmark" title="Permanent Link to <%= the_title_attribute() %>"><%= the_title() %></a></h2>
		               <h3><%= Resource("Post_PostedBy") %> <%= the_author_posts_link(post.Author) %> <%= Resource("Post_PostedIn") %>: <span><%= the_category(", ") %> | <%= the_tags("Tags: ", ", ", "") %></span></h3>
		            </div> 

		            <div class="entry"><%= the_content(Resource("Post_ReadMore")) %></div>
		            <!-- .post -->
		         </li>
            <% } %>
         </ul> <!-- END LOOP --> 

         <div class="nav">
			   <div class="alignleft prev-entries"><%= next_posts_link(Resource("Pager_Simple_OlderEntries")) %></div>
			   <div class="alignright next-entries"><%= previous_posts_link(Resource("Pager_Simple_NewerEntries")) %></div>
			   <%--<%= wp_pagenavi() %>--%>
			   <div class="clear"></div>
		   </div>
         <% } %>
      </div>

   <!-- End Main Content -->

<% get_sidebar(); %>
<% get_footer(); %>
