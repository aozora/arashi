<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.TemplateBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" lang="it-IT">
<head>
<meta http-equiv="Content-Type" content='<%= bloginfo("html_type") %>; charset=<%= bloginfo("charset") %>' />
<title><%= wp_title("&laquo;", true, "right") %></title>
<%-- TODO: usare telerik to combine!!! --%>
<link rel="stylesheet" href='<%= bloginfo("stylesheet_url") %>' type="text/css" media="screen" />
<link rel="stylesheet" href='<%= bloginfo("stylesheet_directory") %>/pagenavi-css.css' type="text/css" media="screen" />
<link rel="shortcut icon" href='<%= bloginfo("template_directory") %>/library/img/page/favicon.gif' type="image/gif" />
<link rel="stylesheet" href='<%= bloginfo("template_directory") %>/library/styles/custom/skyblue.css' type="text/css" media="screen" />

<%= wp_head() %>

<!--[if lte IE 6]>
<link rel="stylesheet" type="text/css" href='<%= bloginfo("template_directory") %>/library/styles/hacks/ie6.css' />
<![endif]-->
<style type="text/css">
   /* Style Overrides */
   #navwrap,
   #footer {background: none;}
   
   #page .header img {margin: 35px auto 0 auto;}
   
   #wrap {
      -moz-border-radius: 6px;
      -webkit-border-radius: 6px;
      -moz-box-shadow: 6px 6px 6px #000;
      -webkit-box-shadow: 6px 6px 6px #000;
      width: 699px;
   }
   #page .header,
   #page #content,
   .footer_nav {width: 699px;}
   
   #page {
      -moz-border-radius: 6px;
      -webkit-border-radius: 6px;
      -moz-box-shadow: 6px 6px 6px #000;
      -webkit-box-shadow: 6px 6px 6px #000;
      background: #FFF url(/Resources/img/bkg_wave.jpg) no-repeat scroll center center;
   }
   
   #content .content_wrap {
      margin: 0;
      padding: 60px 0;
   }
   
   .footer_nav .cen {color: #B3B3B3}
   .footer_nav .list {border: 0}
   
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
<body <%= body_class() %>>
   <!-- Start Navigation -->
   <div id="navwrap">
      <div class="navigation">
         <div class="border">
            <%--
            <ul id="nav">
		    	</ul>
            <div class="search">
            </div>
            --%>
            <div class="clear"></div>
         </div>
      </div>
   </div>
   <!-- Stop Navigation -->
   
   <div id="wrap">
   
      <!-- Start Page -->
		<div id="page">
			<!-- Header -->
			<div class="header">
			      <h1>
			   <a title='<%= bloginfo("name") %>' href='<%= get_option("home") %>/'>
			      <img src='<%= bloginfo("template_directory") %>/library/img/page/logo_usabilityisafact.png' alt='<%= bloginfo("name") %>' />
			   </a>
			      </h1>
			</div>   


         <div class="clear"></div>
         <div id="content">

         <div class="content_wrap">

            <div id="maintenance-container" >
               <p style="text-align:center;">
                  <!--
                  <img  class="alignleft" src="/Templates/pacifica/library/img/under_construction_page.png" alt="under construction" />
                  -->
                  <span class="description" style="color: #666;">
                     Suggerimenti per creare applicazioni usabili a prova di &#8216;utonto&#8217;.
                     <br />
                     Perchè l'usabilità non è un opinione!
                     <%--Al momento sono occupato per un aggiornamento, se sopravvivo senza conseguenze sarò di nuovo online a breve!--%>
                  </span>
                  <br />
                  <br />
                  <br />
                  <br />
                  <span class="title" style="color: #333;" >Coming </span>
                  <span class="title" style="color: #0388C0;">Soon!</span>
               </p>
            </div>

            <div class="clear"></div>
         </div>

         </div>
         <!-- End Content -->

      <!-- Footer NAV  Navigator -->
      <div class="footer_nav">
         <div class="list">
            <div class="cen">
            &#169; 2009 by Marcello Palmitessa. All Rights Reserved.
            </div>
         </div>
         <div class="clear"></div>
         </div>
      </div>
   </div>
   
   <div id="footer">
      <div class="footerWrap">
      </div>
      <!-- End Footer Wrap -->
   </div>

</body>

</html>
