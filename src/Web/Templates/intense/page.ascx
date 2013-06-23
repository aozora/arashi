<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.TemplateBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>
<% get_header(); %>

<div class="subheader_bg">
 	<!-- start:container_16 -->
	   <div class="container_16 subheader">
		   <div class="grid_4">
			   <h2><%= Html.Encode(Model.CurrentPage.Title) %></h2>
		   </div>
		   <div class="grid_12">
			   <p>
			      <%--<?php $values = get_post_custom_values('pagedesc'); echo $values[0]; ?>--%>
			   </p>
		   </div>
	   </div>
  	<!-- end:container_16 -->
</div>
<!-- end:subheader -->

<div class="clear"></div>

<!-- start:middlepart -->
<div class="middlepart">
	<!-- start:container 12 -->
	<div class="container_12">
	   <div class="grid_3">
         <% get_sidebar(); %>

	      <div class="grid_9">
    		   <!-- start:right content -->	
			   <div class="content">
		         <div class="post" id="post-<%= Model.CurrentPage.Id.ToString() %>">
			         <div class="entry">
				         <%= Model.CurrentPage.Content %>
			         </div>
		         </div>
	         </div>
	      </div>
    		<!-- end:right content -->			
		</div>
		
	</div>
	<!-- end:container 12 -->	
</div>
<!-- end:middlepart -->

<div class="clear"></div>

<% get_footer(); %>
