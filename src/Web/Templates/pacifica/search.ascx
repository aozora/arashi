<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.SearchTemplateBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>
<%@ Import Namespace="Arashi.Services.Search"%>
<% get_header(); %>

<div class="clear"></div>
<div id="content">
   <div class="from_title">
      <ul>
         <li class="page_title"><%= Resource("Search_Title").ToUpper() %></li>
      </ul>
   </div>

   <div class="content_wrap sidebar_push">
      <div class="full-entry">

	   <% if (have_posts()) { %>
                
         <% for (int index = 0; index < Model.SearchResult.Count; index++) { the_post(Model.SearchResult[index]); %>
			   <div>
               <h2 id="post-<%= the_ID() %>"><a href="<%= the_permalink() %>" rel="bookmark" title="Permanent Link to <%= the_title_attribute() %>"><%= the_title() %></a></h2>
               <p class="postmetadata">Posted on: <%= the_time("F jS, Y") %><br />
               <%= Resource("Post_PostedBy") %> <%= Model.SearchResult[index].Author%> <%= Resource("Post_PostedIn") %> <%= Model.SearchResult[index].Category%> | <%= Model.SearchResult[index].Tag%></p>
               <span style="width: 540px; overflow: hidden;"><%= the_excerpt() %></span>
            </div>
         <% } %>

         <div class="clear"></div>
		   <div class="nav">
			   <div class="alignleft prev-entries"><%= next_posts_link(Resource("Pager_Simple_OlderEntries")) %></div>
			   <div class="alignright next-entries"><%= previous_posts_link(Resource("Pager_Simple_NewerEntries")) %></div>
			   <%--<%= wp_pagenavi()%>--%>
		   </div>

	   <% } else { %>
		   <h2 class="center"><%= Resource("Search_NoItemsFound") %></h2>
	   <% } %>

		</div><!-- .content-wrapper-->


<% get_sidebar(); %>
<% get_footer(); %>
