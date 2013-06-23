<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.TemplateBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>
<% get_header(); %>
<!-- BEGIN 404 SCREEN-->
<div id="screen-404">
   <h2 class="center">
      <%= Resource("_404_Title2") %>
   </h2>
   <p class="custom">
      <%= Resource("_404_Message1")%>
      <br />
      <%= Resource("_404_Message2")%>
      <br />
      <strong><%= Resource("_404_Message5")%>:</strong>
   </p>
   <ul>
      <li class="page_item page_item-home"><a href='<%= get_option("home") %>/'>Home</a> </li>
      <%= wp_list_pages("sort_column=menu_order&depth=3&title_li=&exclude=")%>
   </ul>
   <ul class="custom">
      <%--<?php i_menu('menu_404'); ?>--%>
   </ul>
</div>
<!-- END 404 SCREEN-->
<% get_footer(); %>
