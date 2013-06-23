<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Site.Master" Inherits="Arashi.Web.Mvc.Views.AdminViewPageBase<IPagedList<Arashi.Core.Domain.Page>>" %>
<%@ Import Namespace="Arashi.Core.Extensions"%>
<%@ Import Namespace="Arashi.Core.Domain"%>
<%@ Import Namespace="Arashi.Web.Mvc.Paging"%>
<asp:Content ContentPlaceHolderID="head" runat="server">
	<title>Pages</title>
</asp:Content>

<asp:Content ContentPlaceHolderID="BreadCrumbsPlaceHolder" runat="server">
   <%= Html.Breadcrumbs(b => b.AddHome("Home", Url.Action("Index", "Site", new {siteid = RequestContext.ManagedSite.SiteId}))
                              .AddCurrent("Pages")) %>
</asp:Content>

<asp:Content ContentPlaceHolderID="ScriptPlaceHolder" runat="server">
   <script type="text/javascript">
      var urlSortPages = '<%= Url.Action("Sort", "AdminPage", new { siteid = RequestContext.ManagedSite.SiteId }) %>';

      $(function(){
         $("#sortable").sortable({
            handle: 'span.handle',
			   placeholder: 'ui-state-highlight',
			   items: 'tr',
            update: function(event, ui) {
               // get a comma separated list of all sortable items
				   var ordereditems = $(this).sortable('toArray').toString();

               // send the new items positions to the server				   
               $.post(urlSortPages, {
                     ordereditems: ordereditems
                  }, function(data){
               });
				   
   			}
		   });
		   $("#sortable").disableSelection();
      });
            
   </script>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
   <div id="adminpagetitle">
      <img class="icon" src="/Resources/img/32x32/pages.png" alt="list pages" />
      <h2>Pages</h2>
   </div>
   <div class="clear"></div>

   <div class="ui-widget ui-helper-clearfix">
      <a id="newPageLink" 
             href='<%= Url.Action("NewPage", "AdminPage", new {siteid = RequestContext.ManagedSite.SiteId }) %>'  
             title="Create new page" 
             class="coolbutton ui-iconplustext ui-shadow" >
         <img src="/Resources/img/32x32/pages_add.png" alt="add page" />
         <em>
            <span class="ui-iconplustext-title ui-title">New Page</span>
            <span>Create a new page</span>
         </em>
      </a>
   </div>

   <br />
   
   <div>
      <div class="filter-actions ui-widget">
         View&nbsp;
         <a class='<%= string.IsNullOrEmpty((string)ViewData["WorkflowStatus_Current"]) ? "current" : string.Empty %>'
            href='<%= Url.Action("Index", "AdminPage", new {siteid = RequestContext.ManagedSite.SiteId}) %>'>
            All
         </a>
         <% foreach (KeyValuePair<string, string> kv in (IDictionary<string, string>)ViewData["WorkflowStatusDictionary"]) { %>
            |
            <a class='<%= (string)ViewData["WorkflowStatus_Current"] == kv.Key ? "current" : string.Empty %>'
               href='<%= Url.Action("Index", "AdminPage", new {siteid = RequestContext.ManagedSite.SiteId, status = kv.Key}) %>'>
               <%= kv.Value %>
            </a>
         <% } %>
      </div>
      
      <table class="grid ui-widget ui-widget-content ui-corner-all ui-shadow">
         <thead class="ui-widget-header ui-corner-top">
            <tr>
               <th colspan="2">Title</th>
               <th class="width-15perc">Author</th>
               <th class="">Status</th>
               <th>Position</th>
               <th class="width-15perc">Published Date</th>
            </tr>
         </thead>
         <tfoot class="ui-widget-header ui-corner-bottom">
            <tr>
               <th colspan="6">
                  <% if (Model != null){ %>
		               <%= Html.Pager(Model.PageSize, Model.PageNumber, Model.TotalItemCount )%>
                  <% } %>
               </th>
            </tr>
         </tfoot>
         <tbody id="sortable" class="ui-widget-content">
            <% if (Model != null){ %>
               <% foreach (Arashi.Core.Domain.Page p in Model){ %>
                  <tr id="<%= p.Id %>">
                     <td class="width-16">
                        <span class="handle" title="Drag up or down to change position"></span>
                     </td>
                     <td class="title">
                        <%= p.ParentPage == null ? string.Empty : "&mdash;&nbsp;" %>
                        <a href='<%= Url.Action("Edit", "AdminPage", new {siteid = RequestContext.ManagedSite.SiteId, id = p.Id}) %>'>
                           <%= string.IsNullOrEmpty(p.Title) ? "(no title)" : Html.Encode(p.Title) %>
                        </a>
                        <div class="hover-actions">
                           &nbsp;&nbsp;&nbsp;
                           <span class="ui-icon ui-icon-pencil"></span>
                           <%= Html.ActionLink("Edit", "Edit", "AdminPage", new {siteid = RequestContext.ManagedSite.SiteId, id = p.Id}, new {@class = "button-secondary"}) %>
                           <span class="separator">&nbsp;|&nbsp;</span>
                           <a href="#" class="ui-state-disabled" title="Sorry, this action is not yet implemented!" >Delete</a>
                           <span class="separator">&nbsp;|&nbsp;</span>
                           <a href="#" class="ui-state-disabled" title="Sorry, this action is not yet implemented!" >Preview</a>
                        </div>
                     </td>
                     <td><%= Html.Encode(p.Author.Email)%></td>
                     <td>
                        <%= p.WorkflowStatus.ToString() %>
                     </td>
                     <td class="align-right width-50">
                        <%= p.Position.ToString() %>
                     </td>
                     <td>
                        <% if (p.PublishedDate.HasValue) { %>
                              <% if (p.PublishedDate.Value > DateTime.Now.ToUniversalTime()) { %>
                                 Scheduled on<br />
                               <% } %>
                               <%= p.PublishedDate.Value.AdjustDateToTimeZone(RequestContext.CurrentUser.TimeZone).ToShortDateString() %>
                           <% } else { %>
                              (not published yet)
                           <% } %>
                     </td>
                  </tr>
               <% } %>
            <% } else { %>
            <!-- Empty Template -->
               <tr class="emptyrow">
                  <td colspan="4">No pages exists....</td>
               </tr>
            <% } %>
         </tbody>
      </table>
   </div>
   
   <div id="adminpagefooter" class="ui-widget">
      <a href='<%= Url.Action("Index", "Site", new {siteid = RequestContext.ManagedSite.SiteId}) %>'><%= GlobalResource("Form_BackToControlPanel") %></a>
   </div>
</asp:Content>

