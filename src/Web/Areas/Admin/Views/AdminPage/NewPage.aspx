<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Site.Master" Inherits="Arashi.Web.Mvc.Views.AdminViewPageBase<PageModel>" %>
<%@ Import Namespace="Arashi.Core.Extensions"%>
<%@ Import Namespace="Arashi.Web.Areas.Admin.Models"%>
<%@ Import Namespace="Arashi.Core.Domain"%>
<asp:Content ContentPlaceHolderID="head" runat="server">
	<title>New Page</title>
   <script type="text/javascript">
      var urlBrowse = '<%= Url.Action("Browse", "MediaManager", new { siteid = RequestContext.ManagedSite.SiteId }) %>';
	</script>
<% Html.Telerik().ScriptRegistrar()
                 .Scripts(script => script.AddGroup( "js.page.new",
                                                     group => group.Add("admin.page.new.js")
                                                    )); 
%>
</asp:Content>

<asp:Content ContentPlaceHolderID="ScriptPlaceHolder" runat="server">
   <%= Html.HtmlEditorScripts() %>
</asp:Content>

<asp:Content ContentPlaceHolderID="BreadCrumbsPlaceHolder" runat="server">
   <%= Html.Breadcrumbs(b => b.AddHome("Home", Url.Action("Index", "Site", new {siteid = RequestContext.ManagedSite.SiteId}))
                              .Add("Pages", Url.Action("Index", "AdminPage", new {siteid = RequestContext.ManagedSite.SiteId}))
                              .AddCurrent("New Page")) %>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
<% using(Html.BeginForm("SaveNew", "AdminPage", new { siteid = RequestContext.ManagedSite.SiteId }, FormMethod.Post, new { id = "newpageform", @class = "_ui-form-default" })) { %>
   <div id="adminpagetitle">
      <%= Html.AntiForgeryToken() %>
      <img class="icon" src="/Resources/img/32x32/page_add.png" alt="new page" />
      <h2>New Page</h2>
 
      <div id="post-edit-clouds" class="">
         <a id="post-edit-publish" href="#" class="button ui-iconplustext ui-button-text-icon button-icon-right ui-shadow">
            <img alt="tags" src="/Resources/img/32x32/date.png" />
            <em>
               <span class="ui-iconplustext-title ui-title">Published Status</span>
               <span>
                  <strong><%= Model.WorkflowStatus.SelectedValue %></strong>
                     &nbsp;on&nbsp;<%= DateTime.Now.AdjustDateToTimeZone(RequestContext.CurrentUser.TimeZone).ToShortDateString()%>&nbsp;@&nbsp;<%= DateTime.Now.AdjustDateToTimeZone(RequestContext.CurrentUser.TimeZone).ToShortTimeString().Replace(" ", "&nbsp;") %>
               </span>
            </em>
            <span class="ui-button-icon-secondary ui-icon ui-icon-triangle-2-n-s"></span>
         </a>
         <a id="post-edit-customtemplatefile" href="#" class="button ui-iconplustext ui-button-text-icon button-icon-right ui-shadow">
            <img alt="templatefile" src="/Resources/img/32x32/template.png" />
            <em>
               <span class="ui-iconplustext-title ui-title">Custom Template</span>
               <span>
                  <% if (!string.IsNullOrEmpty(Model.Page.CustomTemplateFile)) { %>
                     <%= Model.Page.CustomTemplateFile %>
                  <% } else { %>
                  none
                  <% } %>
               </span>
            </em>
            <span class="ui-button-icon-secondary ui-icon ui-icon-triangle-2-n-s"></span>
         </a>
         <a id="post-edit-parentpage" href="#" class="button ui-iconplustext ui-button-text-icon button-icon-right ui-shadow">
            <img alt="templatefile" src="/Resources/img/32x32/page-parent.png" />
            <em>
               <span class="ui-iconplustext-title ui-title">Parent Page</span>
               <span>
                  <% if (Model.Page.ParentPage == null) { %>
                     none
                  <% } else { %>
                     <%= Model.Page.ParentPage.Title %>
                  <% } %>
               </span>
            </em>
            <span class="ui-button-icon-secondary ui-icon ui-icon-triangle-2-n-s"></span>
         </a>
      </div>
      <div id="menu-publish" class="post-edit-megamenu ui-form-default ui-widget ui-state-default ui-corner-all ui-shadow">
         <ol>
            <li class="vertical">
               <label>Published on</label>
               <%= Html.DropDownList("Month", ViewData["MonthsList"] as SelectList)%>
               <%= Html.TextBox("Day", DateTime.Now.AdjustDateToTimeZone(RequestContext.CurrentUser.TimeZone).Day.ToString().PadLeft(2, '0'), new {@class = "veryshorttext"}) %>
               ,
               <%= Html.TextBox("Year", DateTime.Now.AdjustDateToTimeZone(RequestContext.CurrentUser.TimeZone).Year, new {@class = "shorttext"}) %>
               @
               <%= Html.TextBox("Hour", DateTime.Now.AdjustDateToTimeZone(RequestContext.CurrentUser.TimeZone).Hour.ToString().PadLeft(2, '0'), new {@class = "veryshorttext"}) %>
               :
               <%= Html.TextBox("Minute", DateTime.Now.AdjustDateToTimeZone(RequestContext.CurrentUser.TimeZone).Minute.ToString().PadLeft(2, '0'), new {@class = "veryshorttext"}) %>
            </li>
            <%--<li class="vertical">
               <label>Published until</label>
	            <%=Html.TextBox("PublishedUntil", Model.Post.PublishedUntil.HasValue ? Model.Post.PublishedUntil.Value.AdjustDateToTimeZone(RequestContext.CurrentUser.TimeZone) : Model.Post.PublishedUntil, new {@class = "datepicker date"})%>
            </li>--%>
            <li class="vertical">
               <span>Save as:</span>&nbsp;
               <%= Html.DropDownList("WorkflowStatus", Model.WorkflowStatus)%>
            </li>
         </ol>
      </div>
      <div id="menu-customtemplatefile" class="post-edit-megamenu ui-form-default ui-widget ui-state-default ui-corner-all ui-shadow">
         <ol>
            <li class="vertical">
               <% if (Model.CustomTemplateFiles.Count() > 0) { %>
               <label>Template File:</label>
	            <%= Html.DropDownList("CustomTemplateFile", Model.CustomTemplateFiles, "", new {@class = "mediumtext"})%>
               <% } else { %>
                  No custom template files available for the current theme.
               <% } %>
            </li>
         </ol>
      </div>
      <div id="menu-parentpage" class="post-edit-megamenu ui-form-default ui-widget ui-state-default ui-corner-all ui-shadow">
         <ol>
            <li class="vertical">
               <label>Parent page:</label>
	            <%= Html.DropDownList("ParentPage", Model.ParentPages, "", new {@class = "mediumtext"})%>
           </li>
         </ol>
      </div>
   </div>
   <div class="clear"></div>

   <div class="ui-widget">
      <fieldset id="post-title-container" class="ui-widget ui-widget-content ui-corner-all">
         <legend class="ui-widget-header ui-corner-all">Title</legend>
	      <%=Html.TextBox("Title", Model.Page.Title, new {@class = ""})%>
      </fieldset>
      <br />
      <fieldset id="post-content-container" class="ui-widget ui-widget-content ui-corner-all">
         <legend class="ui-widget-header ui-corner-all">Content</legend>
         <%--<div class="align-right">
            <a href="javascript:toggleEditor('Content');"><span class="hint" >Toggle editor</span></a>
         </div>--%>
	      <%= Html.HtmlEditor("Content", Model.Page.Content)%>
      </fieldset>
      <br />
   </div>

   <div id="adminpagefooter" class="ui-widget">
      <%= Html.SubmitUI(GlobalResource("Form_Save"), "call-to-action ui-state-active")%>	
      &nbsp;|&nbsp;
      <a href='<%= Url.Action("Index", "AdminPage", new {siteid = RequestContext.ManagedSite.SiteId}) %>'>Back to Pages list</a>
   </div>
<% } %>
</asp:Content>

