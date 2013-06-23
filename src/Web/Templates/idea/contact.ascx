<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.TemplateBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>
<% get_header(); %>
	<!-- coda slider -->
		
        <div id="slider" class="blog">

            <div class="scroll blog">
			
 				<div id="blog_header_top">&nbsp;</div>
			
                <div class="scrollContainer blog">
				
					<div class="panel" id="panel_01">
						
						<div id="blog_header_midtop">&nbsp;</div>
					
						<h2 class="title main"><%= Resource("Contact_Title") %>
                     <span>Leave a message</span>
                  </h2>
						
						<hr />
					
						<div class="full_width">
						
                  <div class="full-entry wpcf7">
                     <form action='<%= get_option("home") %>/contact/savecontact/' method="post" id="contactform" class="wpcf7-form">
                        <%= Html.AntiForgeryToken("contact") %>
                        <p>
                           <label>
                              <%= Resource("Contact_Name") %>
                              <br />
                              <input type="text" name="author" id="author" value="" size="40" tabindex="1" />
                           </label>
                        </p>
                        <p>
                           <label>
                              <%= Resource("Contact_Email") %>
                              <br />
                              <input type="text" name="email" id="email" value="" size="40" tabindex="2" />
                           </label>
                        </p>
                        <p>
                           <label>
                              <%= Resource("Contact_Subject") %>
                              <br />
                              <input type="text" name="subject" id="url" value="" size="40" tabindex="3" />
                           </label>
                        </p>
                        <p>
                           <label>
                              <%= Resource("Contact_Message") %>
                              <br />
                              <textarea name="message" id="message" cols="40" rows="10" tabindex="4"></textarea>
                           </label>
                        </p>
                        <div>
                           <%= Html.GenerateCaptcha() %>
                        </div>
                        <br />
                        <p>
                           <input name="submit" type="submit" id="submit" class="submit_grey" tabindex="5" value='<%= Resource("Contact_Submit_Button") %>' />
                           <img src='<%= Url.Content("~/Content/images/loader.gif") %>' alt="ajax loader" class="ajax-loader" />
                        </p>
                     </form>
                     <% Html.RenderPartial("~/Views/ContactValidationMessage.ascx", this.ViewData); %>
                  </div> 

						
					</div>
					
				</div>
				
			</div>

        </div>
        </div>
		
		<div class="scrollBottom round">&nbsp;</div>
		
		<!-- / coda slider -->
		
		<!-- bottom container -->
		
		<%--<?php get_sidebar('bottom'); ?>	--%>
				
		<!-- / bottom container -->
				
<% get_footer(); %>
