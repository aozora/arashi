<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.TemplateBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>

	<!--
   // Do not delete these lines
	if (!empty($_SERVER['SCRIPT_FILENAME']) && 'comments.php' == basename($_SERVER['SCRIPT_FILENAME']))
		die ('Please do not load this page directly. Thanks!');

	if ( post_password_required() ) { ?>
		<p class="nocomments"><?php _e('This post is password protected. Enter the password to view comments.',woothemes); ?></p>
	<?php
		return;
	}
   -->

<% if (have_comments()) { %>
   <h4 class="txt1"><%= comments_number("No Responses", "One Response", "% Responses")%> to &#8220;<%=the_title()%>&#8221;</h4>

	<ol class="commentlist">
   <%= wp_list_comments("avatar_size=48") %>
	</ol>
	<div class="navigation">
		<div class="alignleft"><%--<?php previous_comments_link() ?>--%></div>
		<div class="alignright"><%--<?php next_comments_link() ?>--%></div>
		<div class="fix"></div>
	</div>
<% } else { %>
	<% if ( comments_open() ) { %>
		<!-- If comments are open, but there are no comments. -->

	 <% } else { %>
	   <%--// comments are closed--%>
      <div id="comments_wrap">
      		<!-- If comments are closed. -->
      		<p class="nocomments">Comments are closed.</p>
      </div> <!-- end #comments_wrap -->
	<% } %>
<% } %>


<div id="respond">
   <h4 class="txt1"><%= comment_form_title( "Leave a Reply", "Leave a Reply to %s" ) %></h4>

   <%--
   TODO: manage comment registration (see http://codex.wordpress.org/Option_Reference )
   <?php if ( get_option('comment_registration') && !is_user_logged_in() ) : ?>
   <p>You must be <a href="<?php echo wp_login_url( get_permalink() ); ?>">logged in</a> to post a comment.</p>
   <?php else : ?>--%>

   <form class="form" action='<%= post_url() %>savecomment/' method="post" id="comments">

   <% if ( is_user_logged_in() ) {%>

<%--      <div class="cancel-comment-reply">
      	<small><?php cancel_comment_reply_link(); ?></small>
      </div>
      <p class="logged-in">Logged in as  <a href="<?php echo get_option('siteurl'); ?>/wp-admin/profile.php"><?php echo $user_identity; ?></a>. <a href="<?php echo get_option('siteurl'); ?>/wp-login.php?action=logout" title="Log out of this account"><?php _e('Logout &raquo;',woothemes); ?></a></p>
      <p><textarea class="textarea" name="comment" id="comment" cols="100%" rows="10" tabindex="4"></textarea></p>
--%>

   <% } else { %>
      <ol class="fieldset">
      	<li class="field">
            <label for="author"><small>Name (required)</small></label>
            <input type="text" name="author" id="author" value="" size="22" tabindex="1" />
      	</li>
      	<li class="field">
            <label for="email"><small>Mail (will not be published) (required)</small></label>
            <input type="text" name="email" id="email" value="" size="22" tabindex="2" />
      	</li>
      	<li class="field">
            <label for="url"><small>Website</small></label>
            <input type="text" name="url" id="url" value="" size="22" tabindex="3" />
      	</li>
      	<li class="field">
      		<%--<label for="comment">Comments</label>--%>
            <textarea name="comment" id="comment" cols="100%" rows="10" tabindex="4"></textarea>
      	</li>
      	<li class="field">
            <%= Html.GenerateCaptcha() %>
      	</li>
      </ol>
   <% } %>
      <!--<p><small><strong>XHTML:</strong> You can use these tags: <code><?php echo allowed_tags(); ?></code></small></p>-->

      <p class="submit">
         <input class="btinput" name="submit" type="submit" id="submit" tabindex="5" value="Submit" />
      </p>
      <%--   <input type="hidden" name="comment_post_ID" value="<?php echo $id; ?>" />--%>
      <%= comment_id_fields() %>
   </form>

</div> <!-- end #respond -->

