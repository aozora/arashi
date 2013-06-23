<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.TemplateBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>
<% get_header(); %>

 <div id="content">
 
   <div id="main">

      <div class="box1 clearfix">
         <div class="post clearfix">

            <h2 class="hd-page"><%= Html.Encode(Model.CurrentPage.Title) %></h2>
            <%= Model.CurrentPage.Content %>

         </div>
      </div>

   </div><!-- / #main -->
		
<% get_sidebar(); %>

</div><!-- / #content -->
<% get_footer(); %>
