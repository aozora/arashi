<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Site.Master" Inherits="Arashi.Web.Mvc.Views.AdminViewPageBase<NewSiteModel>" %>
<%@ Import Namespace="xVal.Rules"%>
<%@ Import Namespace="xVal.Html"%>
<%@ Import Namespace="Arashi.Web.Areas.Admin.Models"%>
<asp:Content ContentPlaceHolderID="head" runat="server">
	<title>New Site</title>
</asp:Content>

<asp:Content ContentPlaceHolderID="BreadCrumbsPlaceHolder" runat="server">
   <%= Html.Breadcrumbs(b => b.AddHome("Home", Url.Action("Index", "Site", new {siteid = RequestContext.ManagedSite.SiteId}))
                              .AddCurrent("New Site")) %>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
   <% using(Html.BeginForm("SaveNew", "Site", new { siteid = RequestContext.ManagedSite.SiteId }, FormMethod.Post, new { id = "newsiteform", @class = "ui-widget ui-form-default" })) { %>
      <div class="align-left">
         <img class="block-align wizard-img" src="/Resources/img/128x128/web_add.png" alt="webadd" />
         <div class="block-align">
            <h2>Create a new site</h2>
            <p>To create a new site fill the required fields below</p>
            <%= Html.AntiForgeryToken() %>
            <ol>
               <li>
                  <label for="Name">Name:</label>
                  <%= Html.TextBox("Name", Model.Name, new {maxlength = "70", @class = "mediumtext"}) %>
               </li>
               <li>
                  <label for="Description">Description:</label>
                  <%= Html.TextBox("Description", Model.Description, new {maxlength = "160", @class = "largetext"}) %>
               </li>
               <li>
                  <label for="DefaultHostName">Host name:</label>
                  http://<%= Html.TextBox("DefaultHostName", Model.DefaultHostName, new {maxlength = "100", @class = "mediumtext"}) %>
                  <span class="hint">Can be a domain name or an IP address, i.e. mywebsite.com, 192.198.0.1</span>
               </li>
               <li>
                  <br />
               </li>
               <li class="align-center">
                  <%= Html.SubmitUI("Create", "call-to-action ui-state-active") %>	
                  &nbsp;|&nbsp;
                  <a href='<%= Url.Action("Index", "Site", new {siteid = RequestContext.ManagedSite.SiteId}) %>'><%= GlobalResource("Form_BackToControlPanel") %></a>
               </li>
            </ol>

         </div>
      </div>
         
   <% } %>
   
<% Html.Telerik().ScriptRegistrar()
      .Scripts(script => script.AddGroup( "js.newsite.validation",
                                         group => group.Add("jquery.validate.js")
                                                       .Add("xVal.jquery.validate.js")
                                        ))
      .OnDocumentReady(() => { %>
                     <%= Html.ClientSideValidation<NewSiteModel>()
                              .AddRule("DefaultHostName", new RemoteRule(Url.Action("CheckIfSiteHostExists", "Site")))
                              .SuppressScriptTags()
                              .ToString() %>
      <%            }); %>

</asp:Content>