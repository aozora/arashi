<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Site.Master" Inherits="Arashi.Web.Mvc.Views.AdminViewPageBase<PageModel>" %>
<%@ Import Namespace="Arashi.Core.Domain.Extensions"%>
<%@ Import Namespace="Arashi.Web.Areas.Admin.Extensions"%>
<%@ Import Namespace="Arashi.Core.Extensions"%>
<%@ Import Namespace="Arashi.Web.Areas.Admin.Models"%>
<%@ Import Namespace="Arashi.Core.Domain"%>
<asp:Content ContentPlaceHolderID="head" runat="server">
	<title>Edit Page</title>
   <script type="text/javascript">
      var urlUpdatePermalink = '<%= Url.Action("UpdatePermalink", "AdminContentItem", new { siteid = RequestContext.ManagedSite.SiteId }) %>';
      var urlBrowse = '<%= Url.Action("Browse", "MediaManager", new { siteid = RequestContext.ManagedSite.SiteId }) %>';
      var urlGetCustomFields = '<%= Url.Action("GetCustomFields", "AdminContentItem", new { siteid = RequestContext.ManagedSite.SiteId, contentItemId = Model.Page.Id }) %>';
      var urlUpdateCustomFields = '<%= Url.Action("UpdateCustomFields", "AdminContentItem", new { siteid = RequestContext.ManagedSite.SiteId, contentItemId = Model.Page.Id }) %>';
      var urlSaveNewCustomField = '<%= Url.Action("SaveCustomField", "AdminContentItem", new { siteid = RequestContext.ManagedSite.SiteId }) %>';
      var lastsel; 
	</script>
<% Html.Telerik().ScriptRegistrar()
                 .Scripts(script => script.AddGroup("js.page.edit",
                                                     group => group.Add("admin.page.edit.js")
                                                    )); 
%>
</asp:Content>

<asp:Content ContentPlaceHolderID="ScriptPlaceHolder" runat="server">
   <%= Html.HtmlEditorScripts() %>
</asp:Content>

<asp:Content ContentPlaceHolderID="BreadCrumbsPlaceHolder" runat="server">
   <%= Html.Breadcrumbs(b => b.AddHome("Home", Url.Action("Index", "Site", new {siteid = RequestContext.ManagedSite.SiteId}))
                              .Add("Pages", Url.Action("Index", "AdminPage", new {siteid = RequestContext.ManagedSite.SiteId}))
                              .AddCurrent("Edit Page")) %>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
<% using(Html.BeginForm("Update", "AdminPage", new { siteid = RequestContext.ManagedSite.SiteId, id = Model.Page.Id }, FormMethod.Post, new { id = "editpageform", @class = "_ui-form-default" })) { %>
   <div id="adminpagetitle">
      <%= Html.AntiForgeryToken() %>
      <img class="icon" src="/Resources/img/32x32/page.png" alt="edit page" />
      <h2>Edit Page</h2>

      <div id="post-edit-clouds" class="">
         <a id="post-edit-publish" href="#" class="button ui-iconplustext ui-button-text-icon button-icon-right ui-shadow">
            <img alt="publish" src="/Resources/img/32x32/date.png" />
            <em>
               <span class="ui-iconplustext-title ui-title">Published Status</span>
               <span>
                  <strong><%= Model.Page.WorkflowStatus.ToString()%></strong>
                  <% if (Model.Page.PublishedDate.HasValue && Model.Page.WorkflowStatus == WorkflowStatus.Published ) { %>
                     &nbsp;on&nbsp;<%= Model.Page.PublishedDate.Value.AdjustDateToTimeZone(RequestContext.CurrentUser.TimeZone).ToShortDateString()%>&nbsp;@&nbsp;<%= Model.Page.PublishedDate.Value.AdjustDateToTimeZone(RequestContext.CurrentUser.TimeZone).ToShortTimeString().Replace(" ", "&nbsp;") %>
                  <% } %>
               </span>
            </em>
            <span class="ui-button-icon-secondary ui-icon ui-icon-triangle-2-n-s"></span>
         </a>
         <a id="post-edit-customfields" href="#" class="button ui-iconplustext ui-button-text-icon button-icon-right ui-shadow">
            <img alt="customfields" src="/Resources/img/32x32/lists.png" />
            <em>
               <span class="ui-iconplustext-title ui-title">Custom Fields</span>
               <span>
                  <% if (Model.Page.CustomFields.Count > 0) { %>
                     <%= Model.Page.CustomFields.Count.ToString()%>&nbsp;total fields
                  <% } else { %>
                     none
                  <% } %>
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
               <% if (Model.Page.PublishedDate.HasValue) { %>
	               <%= Html.DropDownList("Month", ViewData["MonthsList"] as SelectList)%>
	               <%= Html.TextBox("Day", Model.Page.PublishedDate.Value.AdjustDateToTimeZone(RequestContext.CurrentUser.TimeZone).Day.ToString().PadLeft(2, '0'), new { @class = "veryshorttext" })%>
	               ,
	               <%= Html.TextBox("Year", Model.Page.PublishedDate.Value.AdjustDateToTimeZone(RequestContext.CurrentUser.TimeZone).Year, new { @class = "shorttext" })%>
	               @
	               <%= Html.TextBox("Hour", Model.Page.PublishedDate.Value.AdjustDateToTimeZone(RequestContext.CurrentUser.TimeZone).Hour.ToString().PadLeft(2, '0'), new { @class = "veryshorttext" })%>
	               :
	               <%= Html.TextBox("Minute", Model.Page.PublishedDate.Value.AdjustDateToTimeZone(RequestContext.CurrentUser.TimeZone).Minute.ToString().PadLeft(2, '0'), new { @class = "veryshorttext" })%>
               <% } else { %>
	               <%= Html.DropDownList("Month", ViewData["MonthsList"] as SelectList)%>
	               <%= Html.TextBox("Day", DateTime.Now.AdjustDateToTimeZone(RequestContext.CurrentUser.TimeZone).Day.ToString().PadLeft(2, '0'), new {@class = "veryshorttext"}) %>
	               ,
	               <%= Html.TextBox("Year", DateTime.Now.AdjustDateToTimeZone(RequestContext.CurrentUser.TimeZone).Year, new {@class = "shorttext"}) %>
	               @
	               <%= Html.TextBox("Hour", DateTime.Now.AdjustDateToTimeZone(RequestContext.CurrentUser.TimeZone).Hour.ToString().PadLeft(2, '0'), new {@class = "veryshorttext"}) %>
	               :
	               <%= Html.TextBox("Minute", DateTime.Now.AdjustDateToTimeZone(RequestContext.CurrentUser.TimeZone).Minute.ToString().PadLeft(2, '0'), new {@class = "veryshorttext"}) %>
               <% } %>
            </li>
            <%--<li class="vertical">
               <label>Published until</label>
	            <%=Html.TextBox("PublishedUntil", Model.Post.PublishedUntil.HasValue ? Model.Post.PublishedUntil.Value.AdjustDateToTimeZone(RequestContext.CurrentUser.TimeZone) : Model.Post.PublishedUntil, new {@class = "datepicker date"})%>
            </li>--%>
            <li class="vertical">
               <span>Current Status:&nbsp;<strong><%= Model.Page.WorkflowStatus.ToString()%></strong></span>
            </li>
            <li class="vertical">
               <span>Change status to:</span>&nbsp;
               <%= Html.DropDownList("WorkflowStatus", Model.WorkflowStatus, "")%>
            </li>
         </ol>
      </div>
      <div id="menu-customfields" class="post-edit-megamenu ui-form-default ui-widget ui-state-default ui-corner-all ui-shadow">
         <ol>
            <li>
               <table id="table-customfields" >
                  <tbody><tr><td></td></tr></tbody>
               </table>
            </li>
            <% if (Model.Page.CustomFields.Count() == 0) { %>
               <li>
                  <span><%= GlobalResource("Message_NoCustomField")%></span>
               </li>
            <% } %>
        </ol>
         <hr />
         <a id="addNewCustomFieldLink" href="#" class="button-secondary" >Add a new custom field</a>
         <fieldset id="newCustomFieldPanel" class="margin-top hidden">
         	<div id="customfield-form" class="ui-form-default">
               <ol>
                  <li class="vertical">
                     <label>Key</label>
                     <%= Html.TextBox("Key", string.Empty, new { @class = "mediumtext" })%>
                  </li>
                  <li class="vertical">
                     <label>Value</label>
                     <%= Html.TextArea("Value", string.Empty, 2, 20, new {@class="mediumtext"}) %>
                  </li>
                  <li class="align-right">
                     <input type="hidden" id="contentitemid" name="contentitemid" value="<%= Model.Page.Id.ToString() %>" />
                     <a id="submitNewCustomFieldLink" 
                        href="#"
                        class="button ui-shadow" >
                        Add
                     </a>
                  </li>
               </ol>
            </div>
         </fieldset>
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
               <%= Html.ParentPageDropDownList("ParentPage", Model.ParentPages, Model.Page.ParentPage)%>
           </li>
         </ol>
      </div>
   </div>
   <div class="clear"></div>

   <div class="ui-widget">
      <fieldset id="post-title-container" class="ui-widget ui-widget-content ui-corner-all">
         <legend class="ui-widget-header ui-corner-all">Title</legend>
	      <%=Html.TextBox("Title", Model.Page.Title, new {@class = "width-100perc"})%>
         <div id="post-permalink">
            <a id="post-permalink-edit" href="#">Edit Permalink</a>
            <div id="post-permalink-edit-container" class="hidden">
               <div class="ui-form-default">
                  <strong>Permalink:</strong>&nbsp;
                  <%= RequestContext.ManagedSite.DefaultUrl() %>page/
                  <%= Html.TextBox("FriendlyName", Model.Page.FriendlyName, new {@class = "largetext"}) %>
                  &nbsp;
                  <a id="post-permalink-edit-submit" 
                     href="#" 
                     rel="<%= Model.Page.Id %>"
                     class="button">
                     <%= GlobalResource("Form_Save") %>
                  </a>
                  &nbsp;|&nbsp;
                  <a id="post-permalink-edit-cancel" href="#" class="ui-priority-secondary">Cancel</a>
               </div>
            </div>
         </div>
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
      <%= Html.SubmitUI(GlobalResource("Form_PostUpdate"), "call-to-action ui-state-active")%>	
      &nbsp;|&nbsp;
      <a href='<%= Url.Action("Index", "AdminPage", new {siteid = RequestContext.ManagedSite.SiteId}) %>'>Back to Pages list</a>
   </div>

<% } %>
</asp:Content>

