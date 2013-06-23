<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.TemplateBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>
<% get_header(); %>

    <div id="content">

        <div id="main">

            <div class="box1 clearfix">
            
                <h2 class="hd-page"><%= Resource("_404_Title2") %></h2>
                
                <p><%= Resource("_404_Message1") %></p>
                <p><%= Resource("_404_Message2") %> <a href='<%= bloginfo("url") %>' title='<%= bloginfo("name") %>'>Home</a></p>
                
            </div>

        </div><!-- / #main -->
        
      <% get_sidebar(); %>

    </div><!-- / #content -->

<% get_footer(); %>
