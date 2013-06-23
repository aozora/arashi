<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.TemplateBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>
<% get_header(); %>

	<div id="content">
	
		<div id="main">

		<h2 class="title">Search Results</h2>
		
	<% if (have_posts()) { %>
      <% foreach (Post post in Model.Posts) { the_post(post); %>

			<div class="box1 clearfix" >
				<div class="post">
				   <h3 id='post-<%= the_ID() %>'><a href='/<%= the_permalink() %>' rel="bookmark" title='Permanent Link to <%= the_title_attribute() %>'><%= the_title() %></a></h3>
				   <p><%= the_excerpt() %></p>
			   </div>
			</div>

      <% } %>

		<div class="navigation nav clearfix">
			<%--<div class="alignleft"><?php next_posts_link('&laquo; Older Entries') ?></div>
			<div class="alignright"><?php previous_posts_link('Newer Entries &raquo;') ?></div>--%>
		</div>

	<% } else { %>

		<h2 class="center">No posts found. Try a different search?</h2>

	<% } %>

	</div><!-- / #main -->

   <% get_sidebar(); %>

</div><!-- / #content -->

<% get_footer(); %>
