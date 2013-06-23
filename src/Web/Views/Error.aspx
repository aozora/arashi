<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
   <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<% Html.Telerik().StyleSheetRegistrar()
                 .StyleSheets(css => css.AddGroup("css.default",
                                                   group => group.Add("admin.reset.css")
                                                                 .Add("admin.layout.css")
                 ))
                 .StyleSheets(css => css.AddGroup("css.theme",
                                                   group => group.DefaultPath("~/Resources/css/themes/light")
                                                                 .Add("admin.ui.css")
                                                                 .Add("admin.form.css")
                                                                 .Add("admin.style.css")
                 )).Render();
%>  
   <title>Oooops!</title>
</head>

<body>
   <div id="wrap">
      <div id="content-wrap">
         <div id="content" class='status<%= ViewData["ErrorCode"] %>'>
            <div id='errorbox-container'>
               <br />
               <br />
               <br />
               <div id="errorbox" class="ui-widget ui-widget-content ui-corner-all align-center">
                     <img src="/Resources/img/64x64/alert.png" alt="" />
                     <h1><%= ViewData["ErrorTitle"] %></h1>
                     <p><strong><%= ViewData["ErrorMessage"] %></strong></p>
                     <br />
                     <%--<a href="#" onclick="history.go(-1);" class="button button-icon-left ui-state-default ui-corner-all">
                        <span class="ui-icon ui-icon ui-icon-circle-arrow-w"></span>
                        Go Back
                     </a>--%>
                     Use the browser buttons to go back...
               </div>
               <br />
               <br />
               <br />
            </div>
         </div>
      </div>
   </div>
</body>
</html>
