<%@ Page Language="C#" Inherits="Arashi.Web.Mvc.Views.AdminViewPageBase<System.Web.Mvc.HandleErrorInfo>" %>
<%@ Import Namespace="Arashi.Web.Mvc.Partials"%>
<%@ Import Namespace="Arashi.Web.Areas.Admin.Extensions"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
   <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<% Html.Telerik().StyleSheetRegistrar()
                 .StyleSheets(css => css.AddGroup("css.default",
                                                   group => group.Add("admin.reset.css")
                                                                 .Add("admin.layout.css")
                 ))
                 .StyleSheets(css => css.AddGroup("css.theme",
                                                   group => group.DefaultPath("~/Resources/css/themes/arashi")
                                                                 .Add("jquery-ui.css")
                                                                 .Add("arashi-ui.css")
                 ))
                 .StyleSheets(css => css.AddGroup("css.theme.common",
                                                   group => group
                                                                 .Add("admin.form.css")
                                                                 .Add("admin.style.css")
                                                                 .Add("admin.ui.common.css")
                 )).Render();
%>   
   <!--[if lte IE 7]>
   <script type="text/javascript">
      try {document.execCommand("BackgroundImageCache", false, true);} catch(err) {};
   </script>
   <![endif]--> 
   <title>Oooops!</title>
   <style type="text/css">
      html,body {background: transparent url(/Resources/img/bkg-error.gif) repeat-x 0 0}
      #content-wrap {background: transparent;}
   </style>
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
                     <% if (ViewData["SiteId"] != null) { %>
                     <a href='<%= Url.Action("Index", "Site", new {siteid = Convert.ToInt32(ViewData["SiteId"])}) %>' 
                        class="button button-icon-left ui-state-default ui-corner-all ui-shadow">
                        <span class="ui-icon ui-icon-home"></span>
                        <%= GlobalResource("Form_BackToControlPanel") %>
                     </a>
                     <% } %>
               </div>
               <br />
               <% if (!string.IsNullOrEmpty(ViewData["DebugInfo"].ToString())) { %>
               <div class="ui-widget ui-widget-content ui-corner-all">
                  <%= ViewData["DebugInfo"] %>
               </div>
               <% } %>
               <br />
               <br />
            </div>
         </div>
      </div>
   </div>
</body>
</html>
