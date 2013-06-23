<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.TemplateBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
   <title><%= wp_title("&laquo;", true, "right") %></title>
   <meta http-equiv="Content-Type" content='<%= bloginfo("html_type") %>; charset=<%= bloginfo("charset") %>' />
   <link rel="stylesheet" href='<%= bloginfo("stylesheet_url") %>' type="text/css" media="screen" />
   <!--[if lte IE 7]><link rel="stylesheet" type="text/css" href="<%= bloginfo("stylesheet_directory") %>/ie.css" /><![endif]-->
   <script type="text/javascript" src="<%= bloginfo("stylesheet_directory") %>/includes/js/jquery-1.3.2.min.js"></script>
   <script type="text/javascript" src="<%= bloginfo("stylesheet_directory") %>/includes/js/tabs.js"></script>
   <script type="text/javascript" src="<%= bloginfo("stylesheet_directory") %>/includes/js/superfish.js"></script>
   <!--[if IE 6]>
   <script src="<%= bloginfo("stylesheet_directory") %>/includes/js/pngfix.js"></script>
   <script src="<%= bloginfo("stylesheet_directory") %>/includes/js/menu.js"></script>
   <![endif]-->

   <%= wp_head() %>

   <script type="text/javascript">
      jQuery("#idTabs").idTabs(); 
   </script>            
   <style type="text/css">
      #main {width: 960px}     
      #maintenance-container {
         display: block;
         height: 200px;
         text-align: center;
         margin: 0 20%;
      }

      #maintenance-container span.title {
         font-weight: bold;
         font-size: 22px;
      }
      #maintenance-container span.description {
         font-weight: normal;
         font-size: 20px;
      }
      
   </style>

</head>

<body id="woothemes">

	<div id="wrap">

		<div id="header">
			
            <ul id="nav" class="nav">
   			</ul>

<%--			   <form id="topSearch" class="search" method="get" action="<%= get_option("home") %>/search/">
   					
				   <p class="fields">
					   <input type="text" value="search" name="s" id="s" onfocus="if (this.value == 'search') {this.value = '';}" onblur="if (this.value == '') {this.value = 'search';}" />
					   <button class="replace" type="submit" name="submit">Search</button>
				   </p>

			   </form>
--%>			
            <div id="logo">
                <a href="#" title="<%= bloginfo("description") %>"><img class="title" src="<%= bloginfo("template_directory") %>/images/logo.png" alt="<%= bloginfo("name") %>" /></a>
                <h1 class="replace"><a href="<%= bloginfo("url") %>"><%= bloginfo("name") %></a></h1>	
            </div>
            
			<div id="hi">
			
				<p class="nomargin">This is the personal blog and thoughts of Woo Member, who lives in Never Neverland. Browse around to see my stuff or follow my thoughts…</p>
			
			</div>
			
		</div>
		
		
    <div id="content">

        <div id="main">

            <div class="box1 clearfix">
               <div id="maintenance-container" >
                  <p style="text-align:center;">
                     <br />
                     <br />
                     <span class="description">
                        This site is temporarily closed for maintenance.
                        <br />
                        <br />
                        Please come back later...
                     </span>
                     <br />
                     <br />
                  </p>
               </div>
            </div>

        </div><!-- / #main -->
        
      <%--<% get_sidebar(); %>--%>

    </div><!-- / #content -->


	</div><!-- / #wrap -->

	<div id="footer">
		
		<div id="footerWrap">
		
			<p id="copy">Copyright &copy; <%= date("Y") %> <a href="#"><%= bloginfo("name") %></a>. All rights reserved.</p>
			
			<ul id="footerNav">
				<li><a href="http://www.woothemes.com" title="Irresistible Theme by WooThemes"><img src="<%= bloginfo("template_directory") %>/images/img_woothemes.jpg" width="87" height="21" alt="WooThemes" /></a></li>			
			</ul>
		
		</div><!-- / #footerWrap -->
	
	</div><!-- / #footer -->

</body>
</html>
