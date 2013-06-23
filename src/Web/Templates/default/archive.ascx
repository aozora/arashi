<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.TemplateBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>
<% get_header(); %>

	<div id="content" class="narrowcolumn" role="main">

	<% if (have_posts()) { %>

 	  <%--<?php $post = $posts[0]; // Hack. Set $post so that the_date() works. ?>--%>
 	  <% /* If this is a category archive */ if (is_category()) { %>
		<h2 class="pagetitle">Archive for the &#8216;<%= single_cat_title() %>&#8217; Category</h2>
 	  <% /* If this is a tag archive */ } else if ( is_tag() ) { %>
		<h2 class="pagetitle">Posts Tagged &#8216;<%= single_tag_title() %>&#8217;</h2>
 	  <% /* If this is a daily archive */ } else if (is_day()) { %>
		<h2 class="pagetitle">Archive for <%= the_time("F jS, Y") %></h2>
 	  <% /* If this is a monthly archive */ } else if (is_month()) { %>
		<h2 class="pagetitle">Archive for <%= the_time("F, Y") %></h2>
 	  <% /* If this is a yearly archive */ } else if (is_year()) { %>
		<h2 class="pagetitle">Archive for <%= the_time("Y") %></h2>
	  <% /* If this is an author archive */ } else if (is_author()) { %>
		<h2 class="pagetitle">Author Archive</h2>
 	  <%--<% /* If this is a paged archive */ } else if (isset($_GET['paged']) && !empty($_GET['paged'])) { %>
		<h2 class="pagetitle">Blog Archives</h2>--%>
 	  <% } %>


		<div class="navigation">
			<div class="alignleft"><%= next_posts_link("&laquo; Older Entries") %></div>
			<div class="alignright"><%= previous_posts_link("Newer Entries &raquo;") %></div>
			<%--<%= wp_pagenavi()%>--%>
		</div>
	
      <% foreach (Post post in Model.Posts) { the_post(post); %>

			<div <% post_class(); %> >
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
      
   <% } else { %>

<%--		if ( is_category() ) { // If this is a category archive
			printf("<h2 class='center'>Sorry, but there aren't any posts in the %s category yet.</h2>", single_cat_title('',false));
		} else if ( is_date() ) { // If this is a date archive
			echo("<h2>Sorry, but there aren't any posts with this date.</h2>");
		} else if ( is_author() ) { // If this is a category archive
			$userdata = get_userdatabylogin(get_query_var('author_name'));
			printf("<h2 class='center'>Sorry, but there aren't any posts by %s yet.</h2>", $userdata->display_name);
		} else { --%>
			<h2 class="center">No posts found.</h2>
		<%--}
		get_search_form();

	endif;
--%>
   <% } %>

	</div>

<% get_sidebar(); %>

<% get_footer(); %>
