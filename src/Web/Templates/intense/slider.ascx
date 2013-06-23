<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.TemplateBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>
<script type="text/javascript">
	$((function(){ 
		enterFunction = function(){
			$(this).html('ACTIVE');
		}
		leaveFunction = function(){
			$(this).html('inactive');
		}

		$('.accordion1').hSlides({
			totalWidth: 940, 
			totalHeight: 340, 
			minPanelWidth: 112, 
			maxPanelWidth: 490,
			speed: 800,
			eventHandler: 'hover',
			easing: 'easeOutQuad',
			activeClass: 'active'
		});
	}
));
</script>
<div class="accordion_bg">
	<div class="container_16 sliderbg">
		<div class="slider">
			<ul class="accordion accordion1">

				<li class="sh1">
					<div class="headerbox">
            		<h1><a title="Permanent link to About the project" href="#">About the project</a></h1>
		            <p>Arashi is a new open source application for web publishing, with strong focus on usability in order to let you became productive in no time!</p>
		            <p>The main goal of this project is to create a web platform even better than the popular WordPress.</p>
                  <%--<dl>
                     <dt><strong>1</strong></dt>
                     <dd>Vivamus et imperdiet enim.</dd>
                     <dt><strong>2</strong></dt>
                     <dd> Vestibulum mollis odio et </dd>
                     <dt><strong>3</strong> </dt>
                     <dd>sapien cursus mollis. </dd>
                  </dl>--%>
       			</div>
       			<!-- start:Slide Info-->
       			<div class="slideinfo">
         			<p class="icon">
         			   <img width="64" height="64" alt="Open Source Project" src='<%= bloginfo("template_url") %>/images/features/web.png' />
         			</p>

         			<span>About the project</span>
        	 			<div class="s-num">
					      <img width="60" height="30" alt="slide-1" src='<%= bloginfo("template_url") %>/images/s1.gif' />
					   </div>
				   </div>
       			<!-- end:Slide Info-->
				</li>	
				<li class="sh2">
					<div class="headerbox">
            	   <h1><a title="Permanent link to Write, click, and you are online" href="http://wpdemo.bannersmonster.com/intense/?p=118">Write, click, and you are online</a></h1>
		            <p>
		               <a href="#">
	                     <img width="300" height="130" alt="" src='<%= bloginfo("template_url") %>/images/features/write_publish-mini2.png' title="2" class="img_border alignnone size-full wp-image-87" />
		               </a>
		            </p>
                  <p>This is the accordion Slide using an image of width 300pixels and height 100pixels. If you want you can use a smaller size image of width 100×100 size</p>
       			</div>
       			<!-- start:Slide Info-->
       			<div class="slideinfo">
         			<p class="icon">
         			   <img width="64" height="64" alt="Write, click, and you are online" src='<%= bloginfo("template_url") %>/images/features/content.png' />
         			</p>

         			<span>Write, click, and you are online</span>
        	 			<div class="s-num">
					      <img width="60" height="30" alt="slide-2" src='<%= bloginfo("template_url") %>/images/s2.gif' />
					   </div>
				   </div>
       			<!-- end:Slide Info-->
				</li>	
			   <li class="sh3">
				   <div class="headerbox">
                  <h1>Powerfull &amp; simple Control Panel</h1>
		            <p>
		               <a href="http://wpdemo.bannersmonster.com/intense/wp-content/uploads/2010/01/21.jpg">
		                  <img width="300" height="101" alt="control panel" src='<%= bloginfo("template_url") %>/images/features/backend-mini1.png' title="2" class="img_border alignnone size-full wp-image-87" />
		               </a>
		            </p>
                  <p>Arashi features a very simple yet powerfull backend, that allows you to became immediately productive!</p>
    			   </div>
       			<!-- start:Slide Info-->
       			<div class="slideinfo">
         			<p class="icon">
         			   <img width="64" 
         			        height="64" 
         			        alt="Simple Control Panel" 
         			        src='<%= bloginfo("template_url") %>/images/features/config.png' />
         			</p>

         			<span>Powerfull &amp; simple Control Panel</span>
        	 			<div class="s-num">
					      <img width="60" height="30" alt="slide-3" src='<%= bloginfo("template_url") %>/images/s3.gif' />
					   </div>
				   </div>
       			<!-- end:Slide Info-->
				</li>	
				<li class="sh4">
					<div class="headerbox">
         			<h1>Rich Theme support</h1>
		            <p>
		               <a href="#" title="Theme">
	                     <img width="310" height="180" alt="wp themes" src='<%= bloginfo("template_url") %>/images/coverflow.png' title="2" class="img_border alignnone size-full wp-image-87" />
		               </a>
		            </p>
	            	<p>
	            	   Arashi can import most of the themes already available for the popular WordPress.
                  </p>
       			</div>
       			<!-- start:Slide Info-->
       			<div class="slideinfo">
         			<p class="icon">
         			   <img width="64" height="64" alt="Financial Analysis" src='<%= bloginfo("template_url") %>/images/features/looknfeel.png' />
         			</p>

         			<span>Themes compatibles with WordPress</span>
        	 			<div class="s-num">
					      <img width="60" height="30" alt="slide-4" src='<%= bloginfo("template_url") %>/images/s4.gif' />
					   </div>
				   </div>
       			<!-- end:Slide Info-->
				</li>	
				<li class="sh5">
					<div class="headerbox">
            		<h1><a title="Permanent link to Microsoft ASP.NET MVC" href="#">Developed with Microsoft&copy; technologies</a></h1>
		            <p>
                     <img width="142" height="58" alt="wp themes" src='<%= bloginfo("template_url") %>/images/features/mvc-logo.png' title="2" class="img_border alignnone size-full wp-image-87" />
		            </p>
		            <p>Donec rhoncus sagittis lacus nec porta. Donec vitae enim id magna elementum porttitor malesuada ut quam. Vestibulum consequat semper<br />
                     <strong>Duis facilisis,</strong> purus eget fermentum aliquam, lacus leo vehicula orci quis blandit justo lectus ornare enim
                  </p>
       			</div>
       			<!-- start:Slide Info-->
       			<div class="slideinfo">
         			<p class="icon">
         			   <img width="62" height="61" alt="Group Discussion" src='<%= bloginfo("template_url") %>/images/features/msdotnet-64x64.png' />
         			</p>

         			<span>Developed with Microsoft&copy; technologies</span>
        	 			<div class="s-num">
					      <img width="60" height="30" alt="slide-5" src='<%= bloginfo("template_url") %>/images/s5.gif' />
					   </div>
				   </div>
       			<!-- end:Slide Info-->
				</li>	
			</ul>
		</div>
	</div>
</div>

