<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Site.Master" Inherits="Arashi.Web.Mvc.Views.AdminViewPageBase<IPagedList<Arashi.Core.Domain.Post>>" %>
<%@ Import Namespace="Arashi.Core.Extensions"%>
<%@ Import Namespace="Arashi.Core.Domain"%>
<%@ Import Namespace="Arashi.Web.Mvc.Paging"%>
<%@ Import Namespace="Arashi.Core.Domain.Extensions" %>
<asp:Content ContentPlaceHolderID="head" runat="server">
	<title>Posts</title>
<% Html.Telerik().ScriptRegistrar()
                 .Scripts(script => script.AddGroup( "js.post.index",
                                                     group => group.Add("admin.post.index.js")
                                                   )
                                    ); 
%>
</asp:Content>

<asp:Content ContentPlaceHolderID="BreadCrumbsPlaceHolder" runat="server">
   <%= Html.Breadcrumbs(b => b.AddHome("Home", Url.Action("Index", "Site", new {siteid = RequestContext.ManagedSite.SiteId}))
                              .AddCurrent("Posts")) %>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
   <div id="adminpagetitle">
      <img class="icon" src="/Resources/img/32x32/word.png" alt="list posts" />
      <h2>Posts</h2>
   </div>
   <div class="clear"></div>

   <div class="ui-widget ui-helper-clearfix">
      <a id="newPostLink" 
             href='<%= Url.Action("NewPost", "AdminPost", new {siteid = RequestContext.ManagedSite.SiteId }) %>'  
             title="Create new post" 
             class="coolbutton ui-iconplustext  ui-shadow" >
         <img src="/Resources/img/32x32/word_add.png" alt="new post" />
         <em>
            <span class="ui-iconplustext-title ui-title">New Post</span>
            <span>Create a new post</span>
         </em>
      </a>
   </div>

   <br />
   
   <div>
      <div class="filter-actions ui-widget">
      View&nbsp;
      <a class='<%= string.IsNullOrEmpty((string)ViewData["WorkflowStatus_Current"]) ? "current" : string.Empty %>'
         href='<%= Url.Action("Index", "AdminPost", new {siteid = RequestContext.ManagedSite.SiteId}) %>'>
         All
      </a>
      <% foreach (KeyValuePair<string, string> kv in (IDictionary<string, string>)ViewData["WorkflowStatusDictionary"]) { %>
         |
         <a class='<%= (string)ViewData["WorkflowStatus_Current"] == kv.Key ? "current" : string.Empty %>'
            href='<%= Url.Action("Index", "AdminPost", new {siteid = RequestContext.ManagedSite.SiteId, status = kv.Key}) %>'>
            <%= kv.Value %>
         </a>
      <% } %>
      </div>
      
      <table class="grid ui-widget ui-widget-content ui-corner-all ui-shadow">
         <thead class="ui-widget-header ui-corner-top">
            <tr>
               <th>Title</th>
               <th class="width-15perc">Author</th>
               <th class="width-15perc">Categories</th>
               <th class="width-15perc">Tags</th>
               <th class="width-70">Comments</th>
               <th class="">Status</th>
               <th class="width-15perc">Published Date</th>
            </tr>
         </thead>
         <tfoot class="ui-widget-header ui-corner-bottom">
            <tr>
               <th colspan="7">
                  <% if (Model != null){ %>
		               <%= Html.Pager(Model.PageSize, Model.PageNumber, Model.TotalItemCount )%>
                  <% } %>
               </th>
            </tr>
         </tfoot>
         <tbody class="ui-widget-content">
            <% if (Model != null){ %>
               <% foreach (Post post in Model){ %>
                  <tr>
                     <td class="title">
                        <a href='<%= Url.Action("Edit", "AdminPost", new {siteid = RequestContext.ManagedSite.SiteId, id = post.Id}) %>'>
                           <%= string.IsNullOrEmpty(post.Title) ? "(no title)" : Html.Encode(post.Title) %>
                        </a>
                        <div class="hover-actions">
                           &nbsp;&nbsp;&nbsp;
                           <span class="ui-icon ui-icon-pencil"></span>
                           <%= Html.ActionLink("Edit", "Edit", "AdminPost", new {siteid = RequestContext.ManagedSite.SiteId, id = post.Id}, new {@class = "button-secondary"}) %>
                           <span class="separator">&nbsp;|&nbsp;</span>
                           <%= Html.ActionLink("Delete", "Delete", "AdminPost", new { siteid = RequestContext.ManagedSite.SiteId, id = post.Id }, new { @class = "post-delete" })%>
                           <span class="separator">&nbsp;|&nbsp;</span>
                           <a href='<%= RequestContext.ManagedSite.DefaultUrl() + post.GetContentUrl() %>' >Preview</a>
                        </div>
                     </td>
                     <td><%= Html.Encode(post.Author.Email) %></td>
                     <td class="multitext">
                        <% for (int index = 0; index < post.Categories.Count; index++) { %>
                          <%= Html.ActionLink(post.Categories[index].Name, "Edit", "AdminCategory", new {id = RequestContext.ManagedSite.SiteId, categoryId = post.Categories[index].Id}, new {id = "post-" + post.Id + "category-" + post.Categories[index].Id} ) %>
                          <% if (index < (post.Categories.Count - 1)) { %>
                          ,
                          <% } %>
                        <% } %>
                     </td>
                     <td class="multitext">
                        <% for (int index = 0; index < post.Tags.Count; index++) {%>
                          <%= Html.ActionLink(post.Tags[index].Name, "Edit", "AdminTag", new {id = RequestContext.ManagedSite.SiteId, tagId = post.Tags[index].TagId}, new {id = "post-" + post.Id + "tag-" + post.Tags[index].TagId} ) %>
                          <% if (index < (post.Tags.Count - 1)) { %>
                          ,
                          <% } %>
                        <% } %>
                     </td>
                     <td class="width-70 align-center">
                        <span class="comment-count">
                           <%= post.Comments.Count.ToString() %>
                        </span>
                     </td>
                     <td>
                        <%= post.WorkflowStatus.ToString() %>
                     </td>
                     <td>
                        <% if (post.PublishedDate.HasValue) { %>
                              <% if (post.PublishedDate.Value > DateTime.Now.ToUniversalTime()) { %>
                                 Scheduled on<br />
                               <% } %>
                               <%= post.PublishedDate.Value.AdjustDateToTimeZone(RequestContext.CurrentUser.TimeZone).ToShortDateString() %>
                           <% } else { %>
                              (not published yet)
                           <% } %>
                     </td>
                  </tr>
               <% } %>
            <% } else { %>
            <!-- Empty Template -->
               <tr class="emptyrow">
                  <td colspan="7">No posts exists....</td>
               </tr>
            <% } %>
         </tbody>
      </table>
   </div>
   
   <div id="adminpagefooter" class="ui-widget">
      <a href='<%= Url.Action("Index", "Site", new {siteid = RequestContext.ManagedSite.SiteId}) %>'><%= GlobalResource("Form_BackToControlPanel") %></a>
   </div>
</asp:Content>

