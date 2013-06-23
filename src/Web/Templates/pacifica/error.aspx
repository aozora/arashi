<%@ Page Language="C#" AutoEventWireup="false"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" lang="it-IT">
<head>
   <meta http-equiv="Content-Type" content='text/html; charset=iso-8859-1' />
   <title>Usability is a fact</title>
   <link rel="stylesheet" href='/Templates/pacifica/style.css' type="text/css" media="screen" />
   <link rel="shortcut icon" href='/Templates/pacifica/library/img/page/favicon.gif' type="image/gif" />
   <link rel="stylesheet" href='/Templates/pacifica/library/styles/custom/skyblue.css' type="text/css" media="screen" />

   <meta name="description" content="Usability is a fact and not an opinion. Suggestions and guidelines on creating usable applications for everyone." />
   <meta name="keywords" content="usability, usabilità, user experience, ux, design, web design, guidelines, user interface, interfacce utente" />
	<meta name="author" content="Marcello Palmitessa" />

   <!--[if lte IE 6]>
   <link rel="stylesheet" type="text/css" href='/Templates/pacifica/library/styles/hacks/ie6.css' />
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
<body>
   <!-- Start Navigation -->
   <div id="navwrap">
      <div class="navigation">
         <div class="border">
            <div class="clear"></div>
         </div>
      </div>
   </div>
   <!-- Stop Navigation -->
   
   <div id="wrap">
		<div id="page">
			<!-- Header -->
			<div class="header">
		      <h1>
		         <a title='Usability is a fact' href='/'>
		            <img src='/Templates/pacifica/library/img/page/logo_usabilityisafact.png' alt='Usability is a fact' />
		         </a>
		      </h1>
			</div>   

         <div class="clear"></div>
         <div id="content">
	         <div class="content_wrap">

               <div id="maintenance-container" >
                  <p style="text-align:center;">
                     <img class="alignleft" src="/Resources/img/64x64/alert.png" alt="alert" />
                     <div>
                        <span class="title" style="color: #333;" ><%= ViewData["ErrorTitle"] %> </span>
                        <span class="description" style="color: #666;"><%= ViewData["ErrorMessage"] %></span>
                        <br />
                        <br />
                        <span class="" style="color: #0388C0;">Use the browser buttons to go back...</span>
                        <br />
                        <br />
                     </div>
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
            &#169; 2010 by Marcello Palmitessa. All Rights Reserved.
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
