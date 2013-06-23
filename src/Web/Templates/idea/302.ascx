<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.TemplateBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>
<% get_header(); %>
<!-- BEGIN 404 SCREEN-->
<div id="screen-404" class="status-302">
   <h2 class="center">
      <%= Resource("_302_Title") %>
   </h2>
   <p class="custom">
      <strong><%= Resource("_302_Message1")%></strong>
   </p>

   <ul class="custom">
   </ul>
</div>
<!-- END 404 SCREEN-->
<% get_footer(); %>
