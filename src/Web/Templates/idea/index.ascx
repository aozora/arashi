<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.TemplateBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>
<% get_header(); %>

      <div id="slider">
         <div class="scroll">
            <div class="scrollContainer">

               <div id="HomepagePromo" class="HomepagePromo">
                  <div id="PromoContentBg">
                     <div id="PromoContent">
                        <div class="MediumThumbnail" id="MediumThumbnail1" style="display: none;">
                           <span id="MTC1" class="Content" style='display: none; background: url(<%= bloginfo("stylesheet_directory") %>/images/promo/mtc1_login2.png) no-repeat center center #FFFFFF;'>
                              <a href="#"></a>
                           </span>
                        </div>
                        <div class="BigThumbnail" id="BigThumbnail1" style="display: none;">
                           <span id="BTC1" class="Content" style='display: none; background: url(<%= bloginfo("stylesheet_directory") %>/images/promo/btc1_home.png) no-repeat center center #FFFFFF;'>
                              <a href="#"></a></span>
                        </div>

                        <div class="MediumThumbnail" id="MediumThumbnail2" style="display: none;">
                           <span id="MTC2" class="Content" style='display: none; background: url(<%= bloginfo("stylesheet_directory") %>/images/promo/mtc2_post.png) no-repeat center center #FFFFFF;'>
                              <a href="#"></a></span>
                        </div>
                        <div class="BigThumbnail" id="BigThumbnail2" style="display: none;">
                           <span id="BTC2" class="Content" style='display: none; background: url(<%= bloginfo("stylesheet_directory") %>/images/promo/btc2_post.png) no-repeat center center #FFFFFF;'>
                              <a href="#"></a></span>
                        </div>

                        <div class="MediumThumbnail" id="MediumThumbnail3" style="display: none;">
                           <span id="MTC3" class="Content" style='display: none; background: url(<%= bloginfo("stylesheet_directory") %>/images/promo/mtc3_themes.png) no-repeat center center #FFFFFF;'>
                              <a href="#"></a></span>
                        </div>
                        <div class="BigThumbnail" id="BigThumbnail3" style="display: none;">
                           <span id="BTC3" class="Content" style='display: none; background: url(<%= bloginfo("stylesheet_directory") %>/images/promo/btc3_themes.png) no-repeat center center #FFFFFF;'>
                              <a href="#"></a></span>
                        </div>

                        <div class="PromoInfo" id="PromoInfo1">
                           <div class="PromoText" id="PromoTitle1">
                              <h2>The project</h2>
                              <p>Arashi is a new open source application for web publishing, with strong focus on usability in order to let you became productive in less time!</p>
                              <p>The main goal of this project is to create a web platform even better than the popular WordPress.</p>
                              <p class="promo-buttons">
                                 <a class="button green features" href='<%= get_option("home") %>/page/features'>See the features</a>
                                 <a class="button green download" href='<%= get_option("home") %>/page/download'>DOWNLOAD</a>
                                 <span>It's free!</span>
                                 <br />
                                 <img src='<%= bloginfo("stylesheet_directory") %>/images/video.png' alt="promo video" />
                                 <a class="" href='<%= get_option("home") %>/page/download'>
                                    VIEW THE VIDEO TEASER
                                 </a>
                             </p>
                           </div>
                        </div>
                        <div class="PromoInfo" id="PromoInfo2" style="display: none;">
                           <div class="PromoText" id="PromoTitle2">
                              <h2>Easy content publishing</h2>
                              <p>The Control Panel is simple &amp; powerfull, it offers various configuration options in order to adapt it to your requirements.</p>
                              <ul>
                                 <li>Rich blog engine (comments moderation, syndication, pings)</li>
                                 <li><acronym title="Search Engine Optimization">SEO</acronym> friendly</li>
                                 <li>Support multiple site and multiple domains</li>
                                 <li>Powerfull theming engine, compatible with the themes of popular Wordpress</li>
                                 <li>Content indexing</li>
                              </ul>
                           </div>
                        </div>
                        <div class="PromoInfo" id="PromoInfo3" style="display: none;">
                           <div class="PromoText" id="PromoTitle3">
                              <h2>Rich Theme support</h2>
                              <p>
	            	               Arashi can import most of the themes already available for the popular WordPress.
                                 <br />
                                 <br />
                                 We don't reinvent the wheel, the team has choosen the road to reuse the template files of the popular web platform Wordpress.
                              </p>
                           </div>
                        </div>
                     </div>
                  </div>
                  <div id="PromoButtons" class="PromoButtons">
                     <div class="PromoButtonsContainer">
                        <span></span>
                        <a id="PButton1" href="#" class="PromoButton" onclick="switchPromo(1); return false;">
                           <span></span>
                        </a>
                        <span></span>
                        <a id="PButton2" href="#" class="PromoButton" onclick="switchPromo(2); return false;">
                           <span></span>
                        </a>
                        <span></span>
                        <a id="PButton3" href="#" class="PromoButton" onclick="switchPromo(3); return false;">
                           <span></span>
                        </a>
                        <span></span>
            			</div>
                  </div>

 <%--                 <object width="560" height="340">
                     <param name="movie" value="http://www.youtube.com/v/vnNmS4gHn5s?fs=1&amp;hl=en_US&amp;rel=0&amp;color1=0x006699&amp;color2=0x54abd6">
                     </param>
                     <param name="allowFullScreen" value="true"></param>
                     <param name="allowscriptaccess" value="always"></param>
                     <embed src="http://www.youtube.com/v/vnNmS4gHn5s?fs=1&amp;hl=en_US&amp;rel=0&amp;color1=0x006699&amp;color2=0x54abd6"
                        type="application/x-shockwave-flash" allowscriptaccess="always" allowfullscreen="true"
                        width="560" height="340"></embed>
                  </object>--%>
               </div>

				</div>
			</div>
      </div>

		<div class="scrollBottom round">&nbsp;</div>
			
		<!-- / coda slider -->
		
		<!-- bottom container -->
		<%--<?php get_sidebar('bottom'); ?>	--%>
      
      <div id="bottom_container">
		
			<div id="bottom_container_top">&nbsp;</div>
         <div id="bottom_container_main">
            <div id="bottom_container_main_inner">
               <div class="column">
                <h2>Powered by</h2>
                  <p>Arashi is built with <a href="http://www.asp.net/mvc/"><strong>Microsoft ASP.NET MVC</strong></a> that give you the best reliability &amp; performance.</p>
                  <p class="align_center">
                     <img width="142" height="58" alt="Built on Microsoft ASP.NET MVC" src='<%= bloginfo("template_url") %>/images/mvc-logo-landing-page.png' />
                  </p>
                  <br />
                  <p>The administrative backend user experience is enhanced by the incredible javascript libraries <a href="http://jquery.com"><strong>jQuery</strong></a> and <a href="http://jqueryui.com"><strong>jQuery UI</strong></a>, and it make strong use of the <a href="http://jqueryui.com/docs/Theming/API#The_jQuery_UI_CSS_Framework">jQuery CSS Framework</a>.</p>
                  <p class="align_center">
                     <img width="130" height="32" alt="Built on jQuery" src='<%= bloginfo("template_url") %>/images/jquery_logo.png' />
                     &nbsp;
                     <img width="132" height="32" alt="Built on jQuery" src='<%= bloginfo("template_url") %>/images/jqueryui_logo.png' />
                  </p>
               </div>
               <div class="column">
                  <h2>Browser support</h2>
                  <p>Arashi project is compatible with all modern browsers.</p>
                  <p class="align_center"><img width="190" height="70" alt="Browser Compatibility" src='<%= bloginfo("template_url") %>/images/browsers.png' /></p>
                  <p>However the Control Panel can run only with the latest versions of modern web browsers, like Firefox 3, Google Chrome 3, Safari 4 or Internet Explorer 8.</p>
                  <p>Due to the fact that we use some CSS level 3 enhancements, users of Internet Explorer will be provided with a poor look &amp; feel</p>
               </div>
               <div class="column twitter-widget">
                  <h2>Twitter Updates</h2>
                  <div id="twitter-followme">
                     <a href="http://twitter.com/arashiproject" title="Click here to follow me on Twitter!"></a>
                  </div>

                  <ul id="twitter-roll">
                     <li></li>
                  </ul>

               </div>
            </div>
         </div>
         <div id="bottom_container_bottom">&nbsp;</div>
		
		</div>      
      
		<!-- / bottom container -->
		
<% get_footer(); %>
