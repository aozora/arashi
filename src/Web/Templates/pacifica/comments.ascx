<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.TemplateBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>
<div class="comment_list">

	<!--if ( post_password_required() ) { 
	TODO: manage post password
		<p class="nocomments">This post is password protected. Enter the password to view comments.</p>
	<php
		return;
	}-->

<h2><%= comments_number(Resource("Comment_CommentsNumber_Zero"), Resource("Comment_CommentsNumber_One"), Resource("Comment_CommentsNumber_More")) %>
	<% if ( comments_open() ) { %>
	<a href="#postcomment" title='<%= Resource("Comment_LeaveAComment") %>'></a>
   <% } %>
</h2>

<% if (have_comments()) { %>
<ul>
<% int index = 0;
   foreach (Comment comment in Model.Posts[0].Comments.Where(c => c.Status == CommentStatus.Approved).OrderBy(c => c.CreatedDate)) { the_comment(comment); %>
	<li <%= comment_class(index) %> id="comment-<%= comment.CommentId.ToString() %>">
	<%= get_avatar(comment.Email, 50 ) %>
        <p><cite><%= comment_type("", "", "") %> <%= Resource("Comment_By")%> <%= comment_author_link() %> &#8212; <%= comment_date() %> @ <a href="#comment-<%= comment.CommentId.ToString() %>"><%= comment_time() %></a></cite></p>	
        <%= Html.Encode(comment.CommentText) %>
	</li>
<%    index++;
   } %>
</ul>

 <% } else { // If there are no comments yet %>
	<p><%= Resource("Comment_NoComments") %></p>
<% } %>

<p class="comments_meta"><%--<%= post_comments_feed_link("<abbr title=\"Really Simple Syndication\">RSS</abbr> feed for comments on this post.") %>--%>
<% if ( pings_open() ) { %>
	<a href="<%= trackback_url() %>" rel="trackback">TrackBack <abbr title="Universal Resource Locator">URL</abbr></a>
	<%= trackback_rdf() %>
<% } %>
</p>

<% if ( comments_open() ) { %>
   <h2 id="postcomment"><%= Resource("Comment_LeaveAComment") %></h2>

   <%--
   <?php if ( get_option('comment_registration') && !is_user_logged_in() ) : ?>
   <p><?php printf(__('You must be <a href="%s">logged in</a> to post a comment.'), wp_login_url( get_permalink() ) );?></p>
   <% } else { %>
   --%>


<form action='<%= post_url() %>savecomment/' method="post" id="commentform">

<% if ( is_user_logged_in() ) {%>

   <p>Logged in as <a href='<%= get_option("siteurl") %>wp-admin/profile.php'><%--<?php echo $user_identity; ?>--%></a>. </p>
   <%-- TODO:
   <p><?php printf(__('Logged in as %s.'), '<a href="'.get_option('siteurl').'/wp-admin/profile.php">'.$user_identity.'</a>'); ?> <a href="<?php echo wp_logout_url(get_permalink()); ?>" title="<?php _e('Log out of this account') ?>"><?php _e('Log out &raquo;'); ?></a></p>
   --%>
<% } else { %>

   <p>
      <input type="text" name="author" id="author" value="" size="22" tabindex="1" />
      <label for="author"><%= Resource("Comment_Name") %></label>
   </p>

   <p>
      <input type="text" name="email" id="email" value="" size="22" tabindex="2" />
      <label for="email"><%= Resource("Comment_Email") %></label>
   </p>

   <p>
      <input type="text" name="url" id="url" value="" size="22" tabindex="3" />
      <label for="url"><%= Resource("Comment_Website") %></label>
   </p>
   
<% } %>

<!--<p><small><strong>XHTML:</strong> <?php printf(__('You can use these tags: %s'), allowed_tags()); ?></small></p>-->
<p>
   <textarea name="comment" id="comment" cols="70%" rows="10" tabindex="4"></textarea>
</p>
<div>
<%= Html.GenerateCaptcha() %>
</div>
<p>
   <input name="submit" type="submit" id="submit" tabindex="5" value='<%= Resource("Comment_Submit") %>' />
   <%= comment_id_fields() %>
</p>

</form>

<%--<php endif; // If registration required and not logged in >--%>

<% } else { // Comments are closed %>
   <p><%= Resource("Comment_Closed") %></p>
<% } %>
</div>
