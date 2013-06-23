<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.TemplateBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>
<% get_header(); %>

	<div id="content" class="narrowcolumn">

	<% if (have_posts()) { %>

		<h2 class="pagetitle">Search Results</h2>

		<div class="navigation">
			<div class="alignleft"><%= next_posts_link("&laquo; Older Entries") %></div>
			<div class="alignright"><%= previous_posts_link("Newer Entries &raquo;") %></div>
			<%--<%= wp_pagenavi()%>--%>
		</div>


      <% foreach (Post post in Model.Posts) { the_post(post); %>

			<div <%= post_class() %> >
				<h3 id='post-<%= the_ID() %>'><a href='/<%= the_permalink() %>' rel="bookmark" title='Permanent Link to <%= the_title_attribute() %>'><%= the_title() %></a></h3>
				<small><%= the_time("l, F jS, Y") %></small>

				<p class="postmetadata"><%= the_tags("Tags: ", ", ", "<br />") %> Posted in <%= the_category(", ") %> | <%= comments_popup_link("No Comments &#187;", "1 Comment &#187;", "% Comments &#187;") %></p>
			</div>

      <% } %>

		<div class="navigation">
			<%--<div class="alignleft"><?php next_posts_link('&laquo; Older Entries') ?></div>
			<div class="alignright"><?php previous_posts_link('Newer Entries &raquo;') ?></div>--%>
		</div>

	<% } else { %>

		<h2 class="center">No posts found. Try a different search?</h2>
		<% get_search_form(); %>

	<% } %>

	</div>

<% get_sidebar(); %>
<% get_footer(); %>
