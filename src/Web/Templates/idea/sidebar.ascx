<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.TemplateBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>
<%--   <div class="block">

	   <h2>Categories</h2>
   	
	   <ul id="category_list">
		   <%= wp_list_categories("title_li=") %>									
	   </ul>
   	
   </div>

   <div class="block">

	   <h2>Search</h2>
   	
	   <% get_search_form(); %>
   		
   </div>

   <div class="block">

	   <h2>Tag cloud</h2>
   	
	   <div id="tag_cloud">
   	   <%= wp_tag_cloud("smallest=10&largest=28&unit=px") %>
	   </div>
   	
   </div>--%>

   <% if (dynamic_sidebar("<div class=\"block\">", "</div>", "", "")){} %>
   
   <div class="block last">
	   &nbsp;										
   </div>
