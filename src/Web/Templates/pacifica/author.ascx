<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.TemplateBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>
<% get_header(); %>

<div class="clear"></div>
<div id="content">

   <div class="from_title">
      <ul>
         <li class="page_title"><%= Html.Encode(Model.CurrentAuthor.DisplayName) %></li>
      </ul>
   </div>


   <!-- Main Content -->
   <div class="content_wrap">

   <div class="profile">
      <div class="avatar alignleft"><%= get_avatar(Model.CurrentAuthor.Email , 128) %></div>
      <div class="info">
         <p>
         <% if (string.IsNullOrEmpty(Model.CurrentAuthor.Description)) { %>
         <%= Resource("Author_NoBio") %>
         <% } else { %>
         <%= Html.Encode(Model.CurrentAuthor.Description) %>
         <% } %>
            <br />
            <a href="http://it.linkedin.com/in/azorasystem" >
               <img src="http://www.linkedin.com/img/webpromo/btn_viewmy_160x33.gif" width="160" height="33" border="0" alt="Visualizza il mio profilo su LinkedIn" />
            </a>
         </p>
         <% if (!string.IsNullOrEmpty(Model.CurrentAuthor.WebSite) && Model.CurrentAuthor.WebSite != "http://") { %> 
            <p class="im">Homepage: <a href='<%= Html.Encode(Model.CurrentAuthor.WebSite) %>'><%= Html.Encode(Model.CurrentAuthor.WebSite) %></a></p>
         <% }%>
         <div class="clear"></div>
      </div>
      <div class="clear"></div>
   </div>
   <br />
	
	<div class="entries">

	<ul><!-- BEGIN LOOP -->                 
      <% if (have_posts()) { %>
      <li>
         <h2 class="pagetitle">Posts by <%= Html.Encode(Model.CurrentAuthor.DisplayName) %></h2>
      </li>
     
      <% foreach (Post post in Model.Posts) { the_post(post); %>
	   <li <%= post_class() %>>
	      <div class="date"><p><%= the_time("M") %><span><%= the_time("j") %></span></p></div>
	      <div class="comments_tally"><a href="<%= the_permalink() %>"><%= comments_number(Resource("Comment_CommentsNumber_Zero"), Resource("Comment_CommentsNumber_One"), Resource("Comment_CommentsNumber_More")) %></a></div>

	      <div class="title">
	         <h2 id="post-<%= the_ID() %>"><a href='<%= the_permalink() %>' rel="bookmark" title="Permanent Link to <%= the_title_attribute() %>"><%= the_title() %></a></h2>
	         <h3><%= Resource("Post_PostedIn2") %>: <span><%= the_category(", ") %> | <%= the_tags("Tags: ", ", ", "") %></span></h3>
	      </div> 

	      <div class="entry"><%= the_content("Read More") %></div>
	      <!-- .post -->
	   </li>
   <% } %>
   </ul> <!-- END LOOP --> 

   <div class="nav"> 
		<div class="alignleft prev-entries"><%= next_posts_link(Resource("Pager_Simple_OlderEntries")) %></div>
		<div class="alignright next-entries"><%= previous_posts_link(Resource("Pager_Simple_NewerEntries")) %></div>
		<%--<%= wp_pagenavi()%>--%>
	</div>
   <% } %>
</div>

<% get_sidebar(); %>
<% get_footer(); %>
