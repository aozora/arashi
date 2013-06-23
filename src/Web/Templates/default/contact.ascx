<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.TemplateBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>
<% get_header(); %>

<div id="content" class="narrowcolumn" >

   <h2 class="pagetitle"><%= Resource("Contact_Title") %></h2>

   <!-- Main Content -->
   <div class="full-entry wpcf7">

      <form action='<%= get_option("home") %>/contact/savecontact/' method="post" id="contactform">
         <%= Html.AntiForgeryToken("contact") %>

         <p>
            <%= Resource("Contact_Name") %>
            <br />
            <input type="text" name="author" id="author" value="" size="40" tabindex="1" />
         </p>
         <p>
            <%= Resource("Contact_Email") %>
            <br />
            <input type="text" name="email" id="email" value="" size="40" tabindex="2" />
         </p>
         <p>
            <%= Resource("Contact_Subject") %>
            <br />
            <input type="text" name="subject" id="url" value="" size="40" tabindex="3" />
         </p>
         <p>
            <%= Resource("Contact_Message") %>
            <br />
            <textarea name="message" id="message" cols="40" rows="10" tabindex="4"></textarea>
         </p>
         <div>
            <%= Html.GenerateCaptcha() %>
         </div>
         <p>
            <input name="submit" type="submit" id="submit" tabindex="5" value='<%= Resource("Contact_Submit_Button") %>' />
         </p>
      </form>
      <% if (ViewData["ContactValidationMessage"] != null)
            Html.RenderPartial("~/Views/ContactValidationMessage.ascx", this.ViewData); 
      %>
   </div>
 </div>
<% get_sidebar(); %>
<% get_footer(); %>
  