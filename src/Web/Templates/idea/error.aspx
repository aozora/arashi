<%@ Page Language="C#" AutoEventWireup="false"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
   <meta http-equiv="Content-Type" content='text/html; charset=iso-8859-1' />
   <title>Arashi Project</title>
   <link rel="stylesheet" href='/Templates/idea/style.css' type="text/css" media="screen" />
   <link rel="shortcut icon" href='/Templates/idea/images/favicon.gif' type="image/gif" />

   <meta name="description" content="Arashi is a new open source application for web publishing, with strong focus on usability in order to let you became productive in no time" />
   <meta name="keywords" content="cms, asp.net, mvc, nhibernate, .NET Framework, wordpress, usability" />

   <!--[if IE 6]>
	   <link rel="stylesheet" href="/Templates/idea/ie6.css" type="text/css" />
   <![endif]-->
   <!--[if IE 7]>
	   <link rel="stylesheet" href="/Templates/idea/ie7.css" type="text/css" />
   <![endif]-->
   
   <!--[if IE 6]>
   <script src="/Templates/idea/js/DD_belatedPNG_0.0.7a.js" type="text/javascript"></script>
   <script src="/Templates/idea/js/png_fix_elements.js" type="text/javascript"></script>
   <![endif]-->
   
</head>
<body>
   <div id="screen-error">
      <div id="screen-error-titles" >
         <h2>Sorry</h2>
         <h3>An error occurred...</h3>
      </div>
      <div id="screen-error-desc" >
         <p class="custom">
            <span><strong><%= ViewData["ErrorTitle"] %></strong></span>
            <span><%= ViewData["ErrorMessage"] %></span>
         </p>
         <ul>
            <li>
               <a href="/">Return to Home</a>
            </li>
         </ul>
      </div>
   </div>
</body>

</html>
