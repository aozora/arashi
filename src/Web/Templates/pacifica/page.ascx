<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.TemplateBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>
<% get_header(); %>

<!-- Start Gallery - Refer to the documentation for the exact use of this. -->
<!-- End Gallery -->

<div class="clear"></div>
<div id="content">
   <div class="from_title">
      <ul>
         <li class="page_title">
            <h2>
            <% if (Model.CurrentPage.ParentPage != null) { %>
               <a href='<%= GetAbsoluteUrl(Model.CurrentPage.ParentPage.GetContentUrl()) %>' title='<%= Html.Encode(Model.CurrentPage.ParentPage.Title) %>'><%= Html.Encode(Model.CurrentPage.ParentPage.Title) %></a>
            <% } else { %>
               <%= Html.Encode(Model.CurrentPage.Title) %>
            <% } %>
            </h2>
         </li>

   <%--<?php
   if($post->post_parent)
   $children = wp_list_pages("title_li=&child_of=".$post->post_parent."&echo=0");
   else
   $children = wp_list_pages("title_li=&child_of=".$post->ID."&echo=0");
   if ($children) {?>
   <?php echo $children;?>
   <?php }?>--%>

      </ul>
   </div>

   <div class="sidebar_push"> 
      <div class="content_wrap">
         <div class="full-entry">
            <div class="post">
            <%= Model.CurrentPage.Content %>
            </div>
         </div>
      </div><!-- End Full-Entry -->

<% get_sidebar(); %>
<% get_footer(); %>
