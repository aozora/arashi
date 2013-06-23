<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.TemplateBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>
<% get_header(); %>
		<!-- coda slider -->
		
        <div id="slider" class="blog">

            <div class="scroll blog">
			
				<div id="blog_header_top">&nbsp;</div>
			
                <div class="scrollContainer blog">
				
					<div class="panel" id="panel_01">

						<div id="blog_header_midtop">&nbsp;</div>
					
						<h2 class="title main" id="title-<%= Html.Encode(Model.CurrentPage.FriendlyName) %>">
                     <%= Html.Encode(Model.CurrentPage.Title) %>
                     <% if (Model.CurrentPage.CustomFields.ContainsKey("desc")) { %>
                        <span><%= Html.Encode(this.get_post_meta(Model.CurrentPage.Id, "desc", false)) %></span>
                     <% } %>
                  </h2>
						
						<hr />
					
						<div class="full_width">
                     <div class="float_left half_width">
						      <div class="feature img float_left">
                           <img src='<%= bloginfo("stylesheet_directory") %>/images/features/cnr.png' alt="" />
                        </div>
                        <div class="feature desc float_left">
                           <h3>Simple administration</h3>
                           <p>The administration backend is thinked to be as simple as possible; to do that we continuously apply our studies on usability to improve the user experience and increase the productivity.
                              <br />
                              The administration backend is themable with the incredible <a href="http://jqueryui.com/themeroller/">jQuery UI ThemeRoller</a>, and is built using the <a href="http://jqueryui.com/docs/Theming/API#The_jQuery_UI_CSS_Framework">jQuery UI CSS Framework</a>. That ensures that our user may use the backend with a comformtable look &amp; feel.
                           </p>
                        </div>
                     </div>
                     <div class="float_left half_width">
						      <div class="feature img float_left">
                           <img src='<%= bloginfo("stylesheet_directory") %>/images/features/search.png' alt="" />
                        </div>
                        <div class="feature desc float_left">
                          <h3>Search Engine Optimization</h3>
                          <p>Arashi features content filters that All the content published with Arashi is automatically optimized for Search Engines: title are optmized, the meta tags are generated automatically, there are checks to avoid duplicated content, the content <acronym title="Uniform Resource Locator">URL</acronym>s are in canonical format; in the administration backend you can fine-tune all the options.</p>
                        </div>
                     </div>
						</div>
						<div class="full_width">
                     <div class="float_left half_width">
						      <div class="feature img float_left">
                           <img src='<%= bloginfo("stylesheet_directory") %>/images/features/kword.png' alt="" />
                        </div>
                        <div class="feature desc float_left">
                           <h3>Powerfull Blog Engine</h3>
                           <p>You can easily publish your content, adding images &amp; videos, and use these advanced features:</p>
                           <ul>
                              <li>organize content with tags and categories</li>
                              <li>enable remote pings</li>
                              <li>feed syndication (ATOM, &nbsp;RSS)</li>
                              <li>anti-spam check with <a href="http://recaptcha.net/">ReCaptcha</a></li>
                              <li>comments moderation</li>
                              <li>per-post custom fields</li>
                              <li>content indexing</li>
                              <li>per-page custom templates</li>
                           </ul>
                        </div>
                     </div>
                     <div class="float_left half_width">
						      <div class="feature img float_left">
                           <img src='<%= bloginfo("stylesheet_directory") %>/images/features/colors.png' alt="" />
                        </div>
                        <div class="feature desc float_left">
                          <h3>Rich theme support</h3>
                          <p>Arashi has a per-view theme engine that is compatible with the one used by the popular <a href="http://wordpress.org/">WordPress</a> platform. 
                          <br />
                          This enable Arashi to use the variety of themes already available for WordPress, with minimal modifications(*).
                          <br />
                          <br />
                          (*) Due to the fact that WordPress and Arashi uses differents technologies (the former uses PHP, the latter uses .NET) a very little effort is needed to convert the PHP language tags in every template files.
                          <br />
                          There is no need to alter the static resources like pure HTML, stylesheets and javascript.
                          </p>
                        </div>
                     </div>
						</div>
						<div class="full_width">
                     <div class="float_left half_width">
						      <div class="feature img float_left">
                           <img src='<%= bloginfo("stylesheet_directory") %>/images/features/wizard.png' alt="" />
                        </div>
                        <div class="feature desc float_left">
                           <h3>Install &amp; Upgrade Wizard</h3>
                           <p>On the first launch a simple wizard will be displayed, in order to quickly setup the database &amp; the basic configuration. The same applies after each code upgrade.</p>
                        </div>
                     </div>
                     <div class="float_left half_width">
						      <div class="feature img float_left">
                           <img src='<%= bloginfo("stylesheet_directory") %>/images/features/web.png' alt="" />
                        </div>
                        <div class="feature desc float_left">
                           <h3>Server Requirements</h3>
                           <p>These are the requirements to implement Arashi on your web server:</p>
                           <ul>
                              <li>Microsoft .NET Framework v3.5 SP1</li>
                              <li>Microsoft IIS v7.x</li>
                              <li>Microsoft SQL Server 2005/2008, and other database server supported by <a href="http://nhforge.org/doc/nh/en/index.html#configuration-optional-dialects">NHibernate</a></li>
                           </ul>
                        </div>
                     </div>
						</div>
						<div class="full_width">
                     <div class="float_left half_width">
						      <div class="feature img float_left">
                           <img src='<%= bloginfo("stylesheet_directory") %>/images/features/evolution_steps.png' alt="" />
                        </div>
                        <div class="feature desc float_left">
                           <h3>Still evolving...</h3>
                           <p>The Arashi Project is a platform still evolving, with every new release we'll delivery new features, side by side with the evolution of the web.</p>
                        </div>
                     </div>
                     <div class="float_left half_width">
                     </div>
						</div>
					
					</div>
					
				</div>
				
			</div>

        </div>
        
        <div class="scrollBottom round">&nbsp;</div>
		
		<!-- / coda slider -->
		
		<!-- bottom container -->
		
		<%--<?php get_sidebar('bottom'); ?>--%>
				
		<!-- / bottom container -->
		
<% get_footer(); %>
