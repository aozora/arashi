<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Site.Master" Inherits="Arashi.Web.Mvc.Views.AdminViewPageBase<PostModel>" %>
<%@ Import Namespace="Arashi.Core.Extensions"%>
<%@ Import Namespace="Arashi.Web.Areas.Admin.Models"%>
<%@ Import Namespace="Arashi.Core.Domain"%>
<asp:Content ContentPlaceHolderID="head" runat="server">
	<title>New Post</title>
   <script type="text/javascript">
      // global documents vars
      var urlSaveNewTag = '<%= Url.Action("SaveNew", "AdminTag", new { siteid = RequestContext.ManagedSite.SiteId }) %>';
      var urlSaveNewCategory = '<%= Url.Action("SaveNew", "AdminCategory", new { siteid = RequestContext.ManagedSite.SiteId }) %>';
      var urlGetTagCloud = '<%= Url.Action("GetTagsFormatted", "AdminTag", new { siteid = RequestContext.ManagedSite.SiteId }) %>';
      var urlBrowse = '<%= Url.Action("Browse", "MediaManager", new { siteid = RequestContext.ManagedSite.SiteId }) %>';
      
      // this is required for ckeditor skin config
      var ckeSkin = '<%= RequestContext.CurrentUser.AdminTheme.ToString().ToLowerInvariant() %>';
	</script>
<% Html.Telerik().ScriptRegistrar()
                 .Scripts(script => script.AddGroup( "js.post.edit",
                                                     group => group.Add("admin.post.new.js")
                                                    )); 
%>
</asp:Content>

<asp:Content ContentPlaceHolderID="ScriptPlaceHolder" runat="server">
   <%= Html.HtmlEditorScripts() %>
</asp:Content>

<asp:Content ContentPlaceHolderID="BreadCrumbsPlaceHolder" runat="server">
   <%= Html.Breadcrumbs(b => b.AddHome("Home", Url.Action("Index", "Site", new {siteid = RequestContext.ManagedSite.SiteId}))
                              .Add("Posts", Url.Action("Index", "AdminPost", new {siteid = RequestContext.ManagedSite.SiteId}))
                              .AddCurrent("New Post")) %>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
<% using(Html.BeginForm("SaveNew", "AdminPost", new { siteid = RequestContext.ManagedSite.SiteId }, FormMethod.Post, new { id = "newpostform", @class = "_ui-form-default" })) { %>
<%= Html.AntiForgeryToken() %>
   <div id="adminpagetitle">
      <img class="icon" src="/Resources/img/32x32/word_add.png" alt="new post" />
      <h2>New Post</h2>
 
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
<%--         <a id="post-edit-customfields" href="#" class="button ui-iconplustext ui-button-text-icon button-icon-right">
            <img alt="tags" src="/Resources/img/32x32/lists.png" />
            <em>
               <span class="ui-iconplustext-title">Custom Fields</span>
               <span>
                  <% if (Model.Post.CustomFields.Count > 0) { %>
                     <%= Model.Post.CustomFields.Count.ToString()%>&nbsp;total fields
                  <% } else { %>
                     none
                  <% } %>
               </span>
            </em>
            <span class="ui-button-icon-secondary ui-icon ui-icon-triangle-2-n-s"></span>
         </a>--%>
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
      <div id="menu-categories" class="post-edit-megamenu ui-form-default ui-widget ui-state-default ui-corner-all ui-shadow">
         <span><strong>Categories used by this post:</strong></span>
         <ol>
            <% foreach (Category category in Model.SiteCategories)
               { %>
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
      <div id="menu-customfields" class="post-edit-megamenu ui-form-default ui-widget ui-state-default ui-corner-all ui-shadow">
      </div>
   </div>
   <div class="clear"></div>

   <div class="ui-widget">
      <fieldset class="ui-widget ui-widget-content ui-corner-all">
         <legend class="ui-widget-header ui-corner-all">Title</legend>
	      <%=Html.TextBox("Title", Model.Post.Title, new {@class = "width-100perc"})%>
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
      <a id="showSummaryLink" class="button-secondary" href="#">Do you want to specify the summary for this post?</a>
      <br />
      <fieldset class="ui-widget ui-widget-content ui-corner-all hidden">
         <legend class="ui-widget-header ui-corner-all">Summary</legend>
	      <%=Html.TextArea("Summary", Model.Post.Summary, new {@class = "width-100perc"})%>
      </fieldset>
      <br />
      <fieldset class="ui-widget ui-widget-content ui-corner-all">
         <legend class="ui-widget-header ui-corner-all">
            Comments
         </legend>
         <%= Html.CheckBox("AllowComments", Model.Post.AllowComments) %>
         <label for="AllowComments">&nbsp;Allow comments on this post</label>
         <br />
         <%= Html.CheckBox("AllowPings", Model.Post.AllowPings) %>
         <label for="AllowPings">&nbsp;Allow trackbaks &amp; pingbacks on this post</label>
         <br />
      </fieldset>
   </div>

   <div id="adminpagefooter" class="ui-widget">
      <%= Html.SubmitUI(GlobalResource("Form_Save"), "call-to-action ui-state-active") %>	
      &nbsp;|&nbsp;
      <a href='<%= Url.Action("Index", "AdminPost", new {siteid = RequestContext.ManagedSite.SiteId}) %>'>Back to Posts list</a>
   </div>

<% } %>
</asp:Content>

