<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.TemplateBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>
<% get_header(); %>

    <div id="content">

        <div id="main">

            <div class="box1 clearfix">
                 <div class="post clearfix">

                 <h2 class="hd-page"><%= Resource("Contact_Title") %></h2>
         
                  <div class="full-entry wpcf7">

                     <form class="form" action='<%= get_option("home") %>/contact/savecontact/' method="post" id="contactform">
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
                           <textarea name="message" id="message" cols="70%" rows="10" tabindex="4"></textarea>
                        </p>
                        <div>
                           <%= Html.GenerateCaptcha() %>
                        </div>
                        <p class="submit">
                           <input class="btinput" name="submit" type="submit" id="submit" tabindex="5" value='<%= Resource("Contact_Submit_Button") %>' />
                        </p>
                     </form>
                     <% if (ViewData["ContactValidationMessage"] != null)
                           Html.RenderPartial("~/Views/ContactValidationMessage.ascx", this.ViewData); 
                     %>
                  </div>

                </div>
            </div>

        </div><!-- / #main -->
        
      <% get_sidebar(); %>

    </div><!-- / #content -->

<% get_footer(); %>
