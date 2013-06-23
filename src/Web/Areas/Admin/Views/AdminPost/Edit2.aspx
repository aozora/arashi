﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Site.Master" Inherits="Arashi.Web.Mvc.Views.AdminViewPageBase<PostModel>" %>
<%@ Import Namespace="Arashi.Core.Domain.Extensions"%>
<%@ Import Namespace="Arashi.Core.Extensions"%>
<%@ Import Namespace="Arashi.Web.Areas.Admin.Models"%>
<%@ Import Namespace="Arashi.Core.Domain"%>
<asp:Content ContentPlaceHolderID="head" runat="server">
	<title>Edit Post</title>
   <script type="text/javascript">
      // global documents vars
      var urlSaveNewTag = '<%= Url.Action("SaveNew", "AdminTag", new { siteid = RequestContext.ManagedSite.SiteId }) %>';
      var urlSaveNewCategory = '<%= Url.Action("SaveNew", "AdminCategory", new { siteid = RequestContext.ManagedSite.SiteId }) %>';
      var urlGetTagCloud = '<%= Url.Action("GetTagsFormatted", "AdminTag", new { siteid = RequestContext.ManagedSite.SiteId }) %>';
      var urlGetTagId = '<%= Url.Action("GetTagId", "AdminTag", new { siteid = RequestContext.ManagedSite.SiteId }) %>';
      var urlUpdatePermalink = '<%= Url.Action("UpdatePermalink", "AdminContentItem", new { siteid = RequestContext.ManagedSite.SiteId }) %>';
      var urlBrowse = '<%= Url.Action("Browse", "MediaManager", new { siteid = RequestContext.ManagedSite.SiteId }) %>';
      var urlGetCustomFields = '<%= Url.Action("GetCustomFields", "AdminContentItem", new { siteid = RequestContext.ManagedSite.SiteId, contentItemId = Model.Post.Id }) %>';
      var urlSaveNewCustomField = '<%= Url.Action("SaveCustomField", "AdminContentItem", new { siteid = RequestContext.ManagedSite.SiteId }) %>';
      var urlUpdateCustomFields = '<%= Url.Action("UpdateCustomFields", "AdminContentItem", new { siteid = RequestContext.ManagedSite.SiteId, contentItemId = Model.Post.Id }) %>';
      var lastsel; 
	</script>
<% Html.Telerik().ScriptRegistrar()
                 .Scripts(script => script.AddGroup( "js.post.edit",
                                                     group => group.Add("admin.post.edit.js")
                                                   )
                                    ); 
%>
</asp:Content>

<asp:Content ContentPlaceHolderID="ScriptPlaceHolder" runat="server">
   <%= Html.HtmlEditorScripts() %>
</asp:Content>

<asp:Content ContentPlaceHolderID="BreadCrumbsPlaceHolder" runat="server">
   <%= Html.Breadcrumbs(b => b.AddHome("Home", Url.Action("Index", "Site", new {siteid = RequestContext.ManagedSite.SiteId}))
                              .Add("Posts", Url.Action("Index", "AdminPost", new {siteid = RequestContext.ManagedSite.SiteId}))
                              .AddCurrent("Edit Post")) %>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
<% using(Html.BeginForm("Update", "AdminPost", new { siteid = RequestContext.ManagedSite.SiteId, id = Model.Post.Id }, FormMethod.Post, new { id = "editpostform", @class = "_ui-form-default" })) { %>
   <%= Html.AntiForgeryToken() %>

   <div id="adminpagetitle">
      <img class="icon" src="/Resources/img/32x32/word.png" alt="edit post" />
      <h2>Edit Post</h2>

      <div id="post-edit-clouds" class="">
         <a id="post-edit-publish" href="#" class="button ui-iconplustext ui-button-text-icon button-icon-right ui-shadow">
            <img alt="tags" src="/Resources/img/32x32/date.png" />
            <em>
               <span class="ui-iconplustext-title ui-title">Published Status</span>
               <span>
                  <strong><%= Model.Post.WorkflowStatus.ToString() %></strong>
                  <% if (Model.Post.PublishedDate.HasValue && Model.Post.WorkflowStatus == WorkflowStatus.Published ) { %>
                     &nbsp;on&nbsp;<%= Model.Post.PublishedDate.Value.AdjustDateToTimeZone(RequestContext.CurrentUser.TimeZone).ToShortDateString() %>&nbsp;@&nbsp;<%= Model.Post.PublishedDate.Value.AdjustDateToTimeZone(RequestContext.CurrentUser.TimeZone).ToShortTimeString().Replace(" ", "&nbsp;") %>
                  <% } %>
               </span>
            </em>
            <span class="ui-button-icon-secondary ui-icon ui-icon-triangle-2-n-s"></span>
         </a>
         <a id="post-edit-tags" href="#" class="button ui-iconplustext ui-button-text-icon button-icon-right ui-shadow">
            <img alt="tags" src="/Resources/img/32x32/tag.png" />
            <em>
               <span class="ui-iconplustext-title ui-title">Tags</span>
               <span>
                  <% if (Model.Post.Tags.Count > 0) { %>
                     <%= Model.Post.Tags.Count.ToString() %>&nbsp;total tags
                  <% } else { %>
                     none
                  <% } %>
               </span>
            </em>
            <span class="ui-button-icon-secondary ui-icon ui-icon-triangle-2-n-s"></span>
         </a>
         <a id="post-edit-categories" href="#" class="button ui-iconplustext ui-button-text-icon button-icon-right ui-shadow">
            <img alt="tags" src="/Resources/img/32x32/folder_green.png" />
            <em>
               <span class="ui-iconplustext-title ui-title">Categories</span>
               <span>
                  <% if (Model.Post.Categories.Count > 0) { %>
                     <%= Model.Post.Categories.Count.ToString()%>&nbsp;total categories
                  <% } else { %>
                     none
                  <% } %>
               </span>
            </em>
            <span class="ui-button-icon-secondary ui-icon ui-icon-triangle-2-n-s"></span>
         </a>
         <a id="post-edit-customfields" href="#" class="button ui-iconplustext ui-button-text-icon button-icon-right ui-shadow">
            <img alt="tags" src="/Resources/img/32x32/lists.png" />
            <em>
               <span class="ui-iconplustext-title ui-title">Custom Fields</span>
               <span>
                  <% if (Model.Post.CustomFields.Count > 0) { %>
                     <%= Model.Post.CustomFields.Count.ToString()%>&nbsp;total fields
                  <% } else { %>
                     none
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
               <% if (Model.Post.PublishedDate.HasValue) { %>
	               <%= Html.DropDownList("Month", ViewData["MonthsList"] as SelectList)%>
	               <%= Html.TextBox("Day", Model.Post.PublishedDate.Value.AdjustDateToTimeZone(RequestContext.CurrentUser.TimeZone).Day.ToString().PadLeft(2, '0'), new {@class = "veryshorttext"}) %>
	               ,
	               <%= Html.TextBox("Year", Model.Post.PublishedDate.Value.AdjustDateToTimeZone(RequestContext.CurrentUser.TimeZone).Year, new {@class = "shorttext"}) %>
	               @
	               <%= Html.TextBox("Hour", Model.Post.PublishedDate.Value.AdjustDateToTimeZone(RequestContext.CurrentUser.TimeZone).Hour.ToString().PadLeft(2, '0'), new {@class = "veryshorttext"}) %>
	               :
	               <%= Html.TextBox("Minute", Model.Post.PublishedDate.Value.AdjustDateToTimeZone(RequestContext.CurrentUser.TimeZone).Minute.ToString().PadLeft(2, '0'), new {@class = "veryshorttext"}) %>
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
               <span>Current Status:&nbsp;<strong><%= Model.Post.WorkflowStatus.ToString() %></strong></span>
            </li>
            <li class="vertical">
               <span>Change status to:</span>&nbsp;
               <%= Html.DropDownList("WorkflowStatus", Model.WorkflowStatus, "")%>
            </li>
         </ol>
      </div>
      <div id="menu-tags" class="post-edit-megamenu ui-form-default ui-widget ui-state-default ui-corner-all ui-shadow">
         <div>
            <span><strong>Tags used by this post:</strong></span>
               
            <ul id="tagchecklist">
               <li id="tagchecklist-empty" class='<%= Model.Post.Tags.Count == 0 ? "" : "hidden" %>'>
                  <span><%= GlobalResource("Message_NoTagsAssociated") %></span>
               </li>
               <% foreach (Tag tag in Model.Post.Tags) { %>
               <li>
                  <div class="tag">
                     <%= Html.Encode(tag.Name) %>
                     <input id='tagid-<%= tag.TagId.ToString() %>' name="tagid" 
                              type="hidden" 
                              value='<%= tag.TagId.ToString() %>' />
                  </div>
                  <a title="Remove this tag" href="#" class="tag-remove" >
                     <span class="ui-icon ui-icon-circle-close" ></span>
                     remove
                  </a>
               </li>
               <% } %>
            </ul>
         </div>
         <hr />
         <div>
            <span><strong>Choose a tag from these:</strong></span>
               
            <div id="tab-tag-selection">
               <ul>
                  <li><a href="#tabPopular"><span>Popular</span></a></li>
                  <li><a href="#tabAll"><span>All</span></a></li>
                  <li><a href="#tabAdd"><span>Add New</span></a></li>
               </ul>
               <div id="tabPopular">
                  <span><%= GlobalResource("Message_NoTagsPopular") %></span>
               </div>
               <div id="tabAll">
                  <% if (Model.SiteTags.Count == 0) { %>
                     <span><%= GlobalResource("Message_NoTags") %></span>
                  <% } else { %>
                     <label>Select a tag from the list to add it to the current post:</label>
                     <br />
                     <select id="all-tags-select">
                        <% Html.RenderPartial("TagList", Model.SiteTags); %>
                     </select>
                  <% } %>
               </div>
               <div id="tabAdd">
      	         <div id="newtagform" class="ui-form-default">
                     <ol>
                        <li class="vertical">
                           <label>Tag name</label>
                           <%= Html.TextBox("TagName") %>
                           <span class="hint">Separate tags with commas</span>
                        </li>
                        <li class="align-right">
                           <a id="submitNewTagLink" 
                              class="button ui-shadow" 
                              href="#" >Add</a>
                        </li>
                     </ol>
                  </div>
               </div>
            </div>               
         </div>
      </div>
      <div id="menu-categories" class="post-edit-megamenu ui-form-default ui-widget ui-state-default ui-corner-all">
         <span><strong>Categories used by this post:</strong></span>
         <ol>
            <% foreach (Category category in Model.SiteCategories) { %>
               <li>
                  <% for (int level = 0; level < category.Level; level++) { %>
                     &nbsp;&nbsp;&nbsp;&nbsp;
                  <% } %>
			         <input id="category_<%= category.Id %>" name="categoryid" 
			                  type="checkbox"
			                  <% if (Model.Post.Categories.Contains(category)) { %>
			                  checked="checked" 
			                  <% } %>
			                  value="<%= category.Id %>" />
			         <label for="category_<%= category.Id %>"><%= category.Name %></label>
               </li>
            <% } %>
            <% if (Model.SiteCategories.Count() == 0) { %>
               <li>
                  <span><%= GlobalResource("Message_NoCategories") %></span>
               </li>
            <% } %>
         </ol>
         <hr />
         <a id="addNewCategoryLink" href="#" class="button-secondary" >Add a new category</a>
         <fieldset id="newCategoryPanel" class="margin-top hidden">
         	<div id="newcategoryform" class="ui-form-default">
               <ol>
                  <li class="vertical">
                     <label>Category name</label>
                     <%= Html.TextBox("CategoryName") %>
                  </li>
                  <li class="vertical">
                     <label>Parent category</label>
                     <%= Html.DropDownList("ParentCategory", Model.SiteCategoriesSelectList, "") %>
                  </li>
                  <li class="align-right">
                     <a id="submitNewCategoryLink" 
                        href="#"
                        class="button ui-shadow" >
                        Add
                     </a>
                  </li>
               </ol>
            </div>
         </fieldset>
      </div>
      <div id="menu-customfields" class="post-edit-megamenu ui-form-default ui-widget ui-state-default ui-corner-all">
         <ol>
            <li>
               <table id="table-customfields" >
                  <tbody><tr><td></td></tr></tbody>
               </table>
            </li>
            <% if (Model.Post.CustomFields.Count() == 0) { %>
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
                     <input type="hidden" id="contentitemid" name="contentitemid" value="<%= Model.Post.Id.ToString() %>" />
                     <a id="submitNewCustomFieldLink" 
                        href="#"
                        class="button" >
                        Add
                     </a>
                  </li>
               </ol>
            </div>
         </fieldset>
     </div>
   </div>
   <div class="clear"></div>

   <div class="ui-widget">
      <fieldset class="ui-widget ui-widget-content ui-corner-all">
         <legend class="ui-widget-header ui-corner-all">Title</legend>
	      <%= Html.TextBox("Title", Model.Post.Title, new {@class = "width-100perc"})%>
         <div id="post-permalink">
            <a id="post-permalink-edit" href="#">Edit Permalink</a>
            <div id="post-permalink-edit-container" class="hidden">
               <div class="ui-form-default">
                  <strong>Permalink:</strong>&nbsp;
                  <%= RequestContext.ManagedSite.DefaultUrl() %><%= Model.Post.PublishedDate.Value.Year %>/<%= Model.Post.PublishedDate.Value.Month.ToString().PadLeft(2, '0') %>/<% =Model.Post.PublishedDate.Value.Day.ToString().PadLeft(2, '0') %>/
                  <%= Html.TextBox("FriendlyName", Model.Post.FriendlyName, new {@class = "largetext"}) %>
                  &nbsp;
                  <a id="post-permalink-edit-submit" 
                     href="#" 
                     rel="<%= Model.Post.Id %>"
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
	      <%= Html.HtmlEditor("Content", Model.Post.Content)%>
      </fieldset>
      <br />
      <% if (string.IsNullOrEmpty(Model.Post.Summary)) { %>
         <a id="showSummaryLink" class="button-secondary" href="#">Do you want to specify the summary for this post?</a>
         <br />
      <% } %>
      <fieldset id="summaryFieldset" class='ui-widget ui-widget-content ui-corner-all <%= string.IsNullOrEmpty(Model.Post.Summary) ? "hidden" : "" %> '>
         <legend class="ui-widget-header ui-corner-all">Summary</legend>
	      <%=Html.TextArea("Summary", Model.Post.Summary, new {@class = "width-100perc"})%>
      </fieldset>
      <br />
      <fieldset class="ui-widget ui-widget-content ui-corner-all">
         <legend class="ui-widget-header ui-corner-all">
            Comments
            <% if (Model.Post.Comments.Count > 0) { %>
            &nbsp;(<%= Model.Post.Comments.Count.ToString() %>)
            <% } %>
         </legend>
         <%= Html.CheckBox("AllowComments", Model.Post.AllowComments) %>
         <label for="AllowComments">&nbsp;Allow comments on this post</label>
         <br />
         <%= Html.CheckBox("AllowPings", Model.Post.AllowPings) %>
         <label for="AllowPings">&nbsp;Allow trackbaks &amp; pingbacks on this post</label>
         <br />
         <% if (Model.Post.Comments.Count > 0) { %>
            <a id="showCommentsLink" class="button-secondary" href="#">Show comments</a>
         <% } %>               
      </fieldset>
   </div>

   <div id="adminpagefooter" class="ui-widget">
      <%= Html.SubmitUI(GlobalResource("Form_PostUpdate"), "call-to-action ui-state-active")%>
      &nbsp;|&nbsp;
      <a href='<%= Url.Action("Index", "AdminPost", new {siteid = RequestContext.ManagedSite.SiteId}) %>'>Back to Posts list</a>
   </div>

<% } %>
</asp:Content>
