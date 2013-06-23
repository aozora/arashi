<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.TemplateBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>
<% get_header(); %>

<!-- Start Gallery - Refer to the documentation for the exact use of this. -->
<!-- End Gallery -->

<div class="clear"></div>
<div id="content">
   <div class="from_title">
      <ul>
         <li class="page_title">
            <% /* If this is a category archive */ if (is_category()) { %>
            <%= ResourceFormat("Archive_ForCategory", single_cat_title())%>
            <% /* If this is a tag archive */ } else if ( is_tag() ) { %>
            <%= ResourceFormat("Archive_ForTag", single_tag_title())%>
            <% /* If this is a daily archive */ } else if (is_day()) { %>
            <%= ResourceFormat("Archive_ForDay", the_time("jS F Y"))%>
            <% /* If this is a monthly archive */ } else if (is_month()) { %>
            <%= ResourceFormat("Archive_ForMonth", the_time("F Y"))%>
            <% /* If this is a yearly archive */ } else if (is_year()) { %>
            <%= ResourceFormat("Archive_ForYear", the_time("Y"))%>
            <% /* If this is an author archive */ } else if (is_author()) { %>
            <%= Resource("Archive_ForAuthor") %>
            <%--<% /* If this is a paged archive */ } else if (isset($_GET['paged']) && !empty($_GET['paged'])) { %>
            Blog Archives--%>
            <% } %>
         </li>
      </ul>
   </div>

<!-- Main Content -->
	<div class="content_wrap">
		<div class="entries">

	      <% if (have_posts()) { %>
         <ul><!-- BEGIN LOOP --> 
            <% foreach (Post post in Model.Posts) { the_post(post); %>
		         <li <% post_class(); %>>
	               <div class="date"><p><%= the_time("M") %><span><%= the_time("j") %></span></p></div>
	               <div class="comments_tally"><%= comments_number(Resource("Comment_CommentsNumber_Zero"), Resource("Comment_CommentsNumber_One"), Resource("Comment_CommentsNumber_More")) %></div>

		            <div class="title">
		            <h2 id="post-<%= the_ID() %>"><a href='<%= the_permalink() %>' rel="bookmark" title="Permanent Link to <%= the_title_attribute() %>"><%= the_title() %></a></h2>
		            <h3><%= Resource("Post_PostedBy") %> <%= the_author_posts_link(post.Author) %> <%= Resource("Post_PostedIn") %>: <span><%= the_category(", ") %> | <%= the_tags("Tags: ", ", ", "") %></span></h3>
		            </div> 

		            <div class="entry"><%= the_content("Read More | Comments") %></div>
		            <!-- .post -->
		         </li>
            <% } %>
         </ul> <!-- END LOOP --> 

         <div class="nav"><!-- BEGIN NEXT/PREV -->  
			   <div class="alignleft prev-entries"><%= next_posts_link(Resource("Pager_Simple_OlderEntries")) %></div>
			   <div class="alignright next-entries"><%= previous_posts_link(Resource("Pager_Simple_NewerEntries")) %></div>
			   <%--<%= wp_pagenavi()%>--%>
		   </div>
         <% } else { %>
   			<h4 class="center">Non ci sono articoli per questo archivio.</h4>
         <% } %>
      </div>
<!-- End Main Content -->

<% get_sidebar(); %>

<% get_footer(); %>
