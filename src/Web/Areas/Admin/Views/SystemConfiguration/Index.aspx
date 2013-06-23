<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Site.Master" Inherits="Arashi.Web.Mvc.Views.AdminViewPageBase<Arashi.Core.Domain.SystemConfiguration>" %>
<%@ Import Namespace="Arashi.Core.Domain" %>
<asp:Content ContentPlaceHolderID="head" runat="server">
	<title>System Configuration</title>
</asp:Content>

<asp:Content ContentPlaceHolderID="ScriptPlaceHolder" runat="server">
	<script type="text/javascript">
      $(function(){
         $("#tabs").tabs();
	   });
   </script>
</asp:Content>

<asp:Content ContentPlaceHolderID="BreadCrumbsPlaceHolder" runat="server">
   <%= Html.Breadcrumbs(b => b.AddHome("Home", Url.Action("Index", "Site", new {siteid = RequestContext.ManagedSite.SiteId}))
                              .AddCurrent("System Configuration")) %>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
   <div id="adminpagetitle">
      <img class="icon" src="/Resources/img/32x32/burn.png" alt="" />
      <h2>System Configuration</h2>
   </div>
   <div class="clear"></div>

   <% using (Html.BeginForm("Save", "SystemConfiguration", null, FormMethod.Post, new { id = "sysconfigform", @class = "ui-widget ui-form-default" })){ %>

      <div id="tabs">
         <%= Html.AntiForgeryToken() %>
         <ul>
            <li><a href="#tabSmtp"><span>SMTP Settings</span></a></li>
         </ul>
         
         <div id="tabSmtp">
            <ol>
               <li> <!-- ADD Annotations -->
                  <label for="SmtpHost">Smtp Host:</label>
                  <%= Html.TextBox("SmtpHost", Model.SmtpHost, new {maxlength = "50", @class = "largetext"}) %>
               </li>
               <li>
                  <label for="SmtpHostPort">Smtp Host Port Number:</label>
                  <%= Html.TextBox("SmtpHostPort", Model.SmtpHostPort, new {maxlength = "4", @class = "shorttext align-right"}) %>
               </li>
               <li>
                  <label for="SmtpRequireSSL">Smtp RequireSSL:</label>
                  <%= Html.CheckBox("SmtpRequireSSL", Model.SmtpRequireSSL, new {@class = "checkbox"}) %>
               </li>
               <li>
                  <label for="SmtpUserName">Smtp UserName:</label>
                  <%= Html.TextBox("SmtpUserName", Model.SmtpUserName, new {maxlength = "255", @class = "largetext"}) %>
               </li>
               <li>
                  <label for="SmtpUserPassword">Smtp User Password:</label>
                  <%= Html.TextBox("SmtpUserPassword", Model.SmtpUserPassword, new {maxlength = "255", @class = "largetext"}) %>
               </li>
               <li>
                  <label for="SmtpDomain">Smtp Domain:</label>
                  <%= Html.TextBox("SmtpDomain", Model.SmtpDomain, new {maxlength = "255", @class = "largetext"}) %>
               </li>
            </ol>
         </div>
      </div>

      <br />
      
      <div id="adminpagefooter" class="ui-widget">
         <%= Html.SubmitUI(GlobalResource("Form_Save")) %>	
         &nbsp;|&nbsp;
         <a href='<%= Url.Action("Index", "Site") %>'>Back to home</a>
      </div>
       
   <% } %>

</asp:Content>