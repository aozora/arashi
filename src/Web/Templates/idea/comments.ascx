<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.TemplateBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>

<%--	<!--if ( post_password_required() ) { 
	TODO: manage post password
		<p class="nocomments">This post is password protected. Enter the password to view comments.</p>
	<php
		return;
	}-->--%>


<% if (have_comments()){ %>
	
	<div id="comments">
	
	<h3><%= comments_popup_link("0", "1", "%") %> Shouts to "<%= the_title() %>" :</h3>
	
		<ul class="comments">
			<%= wp_list_comments() %>
		</ul>
										
	</div>

 <% } else { %>

	<% if ( comments_open() ) { %>
		<!-- If comments are open, but there are no comments. -->

	 <% } else { %>
	   <%--// comments are closed--%>
	   
		<!-- If comments are closed. -->
		<p class="nocomments">Comments are closed.</p>

	<% } %>
<% } %>


<% if ( comments_open() ) { %>

	<div id="respond">
	
		<h3><%= comment_form_title( "Leave a Reply", "Leave a Reply to %s" ) %></h3>
		
		<%--<div class="cancel-comment-reply">
			<small><% cancel_comment_reply_link() %></small>
		</div>--%>
		
		<% if ( get_option("comment_registration") == "1" && !is_user_logged_in() ) { %>
			<p>You must be <a href="<%= wp_login_url() %>">logged in</a> to post a comment.</p>
		<% } else { %>
			
			<form action='<%= post_url() %>savecomment/' method="post" id="comment_form">
				
				<div>
			
					<% if ( is_user_logged_in() ) { %>
			
               <p>Logged in as <a href='/<%= get_option("siteurl") %>wp-admin/profile.php'><%--<?php echo $user_identity; ?>--%></a>. 
					<%--<p>Logged in as <a href="<?php echo get_option('siteurl'); ?>/wp-admin/profile.php"><?php echo $user_identity; ?></a>. <a href="<?php echo wp_logout_url(get_permalink()); ?>" title="Log out of this account">Log out &raquo;</a></p>--%>

					<% } else { %>
							<label for="author">Name</label>
							<input type="text" name="author" id="author" value="" tabindex="1" />
							<label for="email">E-mail</label>
							<input type="text" name="email" id="email" value="" tabindex="2" />
							<label for="url">Website</label>
							<input type="text" name="url" id="url" value="" tabindex="3" />
					<% } %>
				
					<label for="comment">Message</label>
					<textarea name="comment" id="comment" cols="58" rows="10" tabindex="4"></textarea><br />
					<input class="submit_green" type="submit" value="SUBMIT" />
				</div>
         <%= comment_id_fields() %>
			<%--<?php do_action('comment_form', $post->ID); ?>--%>
				
            <div>
            <%= Html.GenerateCaptcha() %>
            </div>
				
			</form>
			
		<% } %>
		
	</div>		

<% } %>
