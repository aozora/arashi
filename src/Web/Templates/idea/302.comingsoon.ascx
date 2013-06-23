<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.TemplateBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>
<% get_header(); %>
<!-- BEGIN 404 SCREEN-->
<div id="screen-404" class="comingsoon">
   <h2 class="center">
      Coming soon
   </h2>
   <p class="custom">
      Arashi Project is a new CMS framework for the Microsoft .NET platform developed on top of Microsoft ASP.NET MVC, NHibernate, jQuery.
      It aims to be like WordPress.
      <br />
      <br />
      <strong>We are working hard to finish some minor details, stay tuned!</strong>
   </p>
   <div id="twitter-followme">
      <a href="http://twitter.com/arashiproject" title="Click here to follow me on Twitter!"></a>
   </div>

   <ul class="custom">
   </ul>
</div>
<!-- END 404 SCREEN-->
<% get_footer(); %>
