<%@ Page Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Site.Master" Inherits="Arashi.Web.Mvc.Views.AdminViewPageBase<IList<Site>>" %>
<%@ Import Namespace="Arashi.Core.Extensions"%>
<%@ Import Namespace="Arashi.Web.Areas.Admin.Models"%>
<%@ Import Namespace="Arashi.Core.Domain"%>
<%@ Import Namespace="Arashi.Core.Domain.Extensions"%>

<asp:Content ContentPlaceHolderID="head" runat="server">
   <title>Home Page</title>
   <style type="text/css">
   
   #intro,
   #siteslist {
      display: inline-block;
      width: 40%;
   }
   
   #intro {
      height: 100%;
      vertical-align: top;
      padding-right: 20px;
   }
   #intro h3 {
      line-height: 1.4em;
   }
   #intro #newsite-container {
      padding-left: 100px;
      font-size: 1.2em;
   }
   #intro #newsite-container .coolbutton-text {
      line-height: 32px;
   }

   li.item {
      margin: 10px 0;
      padding: 4px;
      border: solid 1px #AED0EA;
   }
   li.item a {
      text-decoration: none;
   }
   #siteslist {
   }
   #siteslist img {
      display: block;
      float: left;
      margin-right: 7px;
   }
   #siteslist p {
      display: block;
      text-align: left;
      margin-left: 42px;
      padding-left: 3px;
      min-height: 32px;
      border-left: 1px solid #A6C9E2;
      font-weight: normal; 
      text-align: left;
   }
   #siteslist span.sitename {
      font-size: 1.2em;
      font-weight: bold;
   }
   </style>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

   <div id="adminpagetitle">
      <img class="icon" src="/Resources/img/32x32/home.png" alt="home" />
      <h2>Welcome to the Control Panel</h2>
   </div>
   <div class="clear"></div>

   <br />
   
   <div>
      <div id="intro">
         <div>
            <h3>
            To start select a site from the list on the right, <br />
            otherwise use the button below to crete a new one.
            </h3>
            <br />
            <div class="clear"></div>
            <div id="newsite-container" class="ui-widget ui-helper-clearfix">
               <a href="#" class="coolbutton ui-state-default ui-corner-all ui-helper-clearfix ui-shadow" >
                  <div class="coolbutton-image">
                     <img src="/Resources/img/32x32/browser_add.png" />
                  </div>
                  <div class="coolbutton-text">
                     <span class="coolbutton-title">Create a new site</span>
                     <%--<span class="coolbutton-desc"></span>--%>
                  </div>
               </a>
            </div>
         </div>
      </div>

   <% if (Model.Count > 0){ %>
      <fieldset id="siteslist" class="width-500 controlpanelgroup ui-widget ui-widget-content ui-corner-all ui-shadow">
         <legend class="ui-widget-header ui-corner-all">Sites</legend>
            <ul class="content-padding">
            <% foreach (Site site in Model){ %>
               <li>
                  <a class="button width-400 controlpanelicon ui-iconplustext ui-button-text-icon button-icon-right ui-shadow" 
                     href='<%= Url.Action("Index", "Site", new {siteid = site.SiteId}) %>'
                     title='<%= Html.Encode(site.Description)%>' >
                     <img src="/Resources/img/32x32/internet.png" alt='site' />
                     <em>
                        <span class="ui-iconplustext-title ui-title"><%= Html.Encode(site.Name)%></span>
                        <span><%= Html.Encode(site.Description.Truncate(70, true))%></span>
                     </em>
                  </a>
               </li>
            <% } %>
         </ul>
      </fieldset>
   <% } %>
   </div>
   
   <br />
   
   <% if (RequestContext.CurrentUser.HasRight(Arashi.Services.Membership.Rights.SystemConfigurationView)) { %>
   <fieldset class="controlpanelgroup ui-widget ui-widget-content ui-corner-all">
      <legend class="ui-widget-header ui-corner-all">System</legend>
         <a class="button controlpanelicon ui-state-default ui-iconplustext ui-corner-all" 
            href='<%= Url.Action("Index", "SystemConfiguration") %>' >
            <img src="/Resources/img/32x32/burn.png" alt='system' />
            <em>
               <span class="ui-iconplustext-title">System Configuration</span>
               <span>Manage the system settings</span>
            </em>
         </a>
   </fieldset>
   <% } %>
</asp:Content>
