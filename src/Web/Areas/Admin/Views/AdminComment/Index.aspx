<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Site.Master" Inherits="Arashi.Web.Mvc.Views.AdminViewPageBase<IPagedList<Arashi.Core.Domain.Comment>>" %>
<%@ Import Namespace="Arashi.Web.Mvc.Gravatar"%>
<%@ Import Namespace="Arashi.Core.Extensions"%>
<%@ Import Namespace="Arashi.Core.Domain"%>
<%@ Import Namespace="Arashi.Web.Mvc.Paging"%>
<asp:Content ContentPlaceHolderID="head" runat="server">
	<title>Comments</title>
   <script type="text/javascript">
      var urlSubmitReply = '<%= Url.Action("Reply", "AdminComment", new { siteid = RequestContext.ManagedSite.SiteId }) %>';
	</script>
<% Html.Telerik().ScriptRegistrar()
                 .Scripts(script => script.AddGroup( "js.comments",
                                                     group => group.Add("admin.comments.js")
                                                    )); 
%>
</asp:Content>

<asp:Content ContentPlaceHolderID="BreadCrumbsPlaceHolder" runat="server">
   <%= Html.Breadcrumbs(b => b.AddHome("Home", Url.Action("Index", "Site", new {siteid = RequestContext.ManagedSite.SiteId}))
                              .AddCurrent("Comments")) %>
</asp:Content>


<asp:Content ContentPlaceHolderID="MainContent" runat="server">
   <div id="adminpagetitle">
      <img class="icon" src="/Resources/img/32x32/comments.png" alt="list comments" />
      <h2>Comments</h2>
   </div>
   <div class="clear"></div>

   <div>
      <div class="filter-actions ui-widget">
         View&nbsp;
         <a class='<%= string.IsNullOrEmpty((string)ViewData["CommentStatus_Current"]) ? "current" : string.Empty %>'
            href='<%= Url.Action("Index", "AdminComment", new {siteid = RequestContext.ManagedSite.SiteId}) %>'>
            All
         </a>
         <% foreach (KeyValuePair<string, string> kv in (IDictionary<string, string>)ViewData["CommentStatusDictionary"]) { %>
            |
            <a class='<%= (string)ViewData["CommentStatus_Current"] == kv.Key ? "current" : string.Empty %>'
               href='<%= Url.Action("Index", "AdminComment", new {siteid = RequestContext.ManagedSite.SiteId, status = kv.Key}) %>'>
               <%= kv.Value %>
            </a>
         <% } %>
      </div>
      
      <table id="comments" class="grid ui-widget ui-widget-content ui-corner-all ui-shadow">
         <thead class="ui-widget-header ui-corner-top">
            <tr>
               <th class="width-20perc">Author</th>
               <th>Comment</th>
               <th>Type</th>
               <th>Status</th>
               <th class="width-15perc">Post</th>
            </tr>
         </thead>
         <tfoot class="ui-widget-header ui-corner-bottom">
            <tr>
               <th colspan="5">
                  <% if (Model != null){ %>
		               <%= Html.Pager(Model.PageSize, Model.PageNumber, Model.TotalItemCount )%>
                  <% } %>
               </th>
            </tr>
         </tfoot>
         <tbody class="ui-widget-content">
            <% Html.RenderPartial("~/Areas/Admin/Views/AdminComment/CommentsList.ascx", Model); %>
         </tbody>
      </table>
   </div>
   
   <div id="adminpagefooter" class="ui-widget">
      <a href='<%= Url.Action("Index", "Site", new {siteid = RequestContext.ManagedSite.SiteId}) %>'><%= GlobalResource("Form_BackToControlPanel") %></a>
   </div>
</asp:Content>

