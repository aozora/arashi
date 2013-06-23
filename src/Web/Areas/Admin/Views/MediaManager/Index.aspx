<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Site.Master" Inherits="Arashi.Web.Mvc.Views.AdminViewPageBase<IList<MediaModel>>" %>
<%@ Import Namespace="Arashi.Web.Areas.Admin.Models"%>
<%@ Import Namespace="Arashi.Core.Extensions"%>
<%@ Import Namespace="Arashi.Core.Domain"%>
<%@ Import Namespace="Arashi.Web.Mvc.Paging"%>
<%@ Import Namespace="Arashi.Core.Domain.Extensions" %>
<asp:Content ContentPlaceHolderID="head" runat="server">
	<title>Media Manager</title>
   <script type="text/javascript">
      var urlDeleteMedia = '<%= Url.Action("Delete", "MediaManager", new { siteid = RequestContext.ManagedSite.SiteId }) %>';
	</script>
<% Html.Telerik().ScriptRegistrar()
                 .Scripts(script => script.AddGroup( "js.mediamanager",
                                                     group => group.Add("Silverlight.js")
                                                                   .Add("admin.mediamanager.js")
                                                    )); 
%>
</asp:Content>

<asp:Content ContentPlaceHolderID="BreadCrumbsPlaceHolder" runat="server">
   <%= Html.Breadcrumbs(b => b.AddHome("Home", Url.Action("Index", "Site", new {siteid = RequestContext.ManagedSite.SiteId}))
                              .AddCurrent("Media Manager")) %>
</asp:Content>


<asp:Content ContentPlaceHolderID="MainContent" runat="server">
   <div id="adminpagetitle">
      <img class="icon" src="/Resources/img/32x32/archive-32.png" alt="list comments" />
      <h2>Media Manager</h2>
   </div>
   <div class="clear"></div>

   <div class="ui-widget ui-helper-clearfix">
      <%--<a id="newMediaLink" 
             href='#'  
             title="Add a new media" 
             class="coolbutton ui-iconplustext ui-shadow" >
         <img src="/Resources/img/32x32/archive-add.png" alt="add media" />
         <em>
            <span class="ui-iconplustext-title">Add Media</span>
            <span>Add a new media file</span>
         </em>
      </a>--%>

      <% UploadModel uploadModel = new UploadModel(Url); %>
      <object data="data:application/x-silverlight-2," 
              type="application/x-silverlight-2" 
              width="200" 
              height="50" id="slmfu">
         <param name="source" value="/ClientBin/mpost.SilverlightMultiFileUpload.Lite.xap" />
         <param name="onload" value="pluginLoaded" />
         <param name="onError" value="onSilverlightError" />
         <param name="background" value="white" />
         <param name="minRuntimeVersion" value="4.0.50401.0" />
         <param name="autoUpgrade" value="true" />
         <param name="windowless" value="true" />
         <param name="initParams" value="HttpUploader=true,UploadHandlerName=<%= uploadModel.UploadHandlerName %>,MaxFileSizeKB=<%= uploadModel.MaxFileSizeKB %>,MaxUploads=<%= uploadModel.MaxUploads %>,FileFilter='<%= uploadModel.FileFilter %>',CustomParam=<%= uploadModel.CustomParam %>,DefaultColor=<%= uploadModel.DefaultColor %>,SelectFileButtonImageSource=<%= RequestContext.ManagedSite.DefaultUrl() %>Resources/img/32x32/archive-add.png" />
         <a href="http://go.microsoft.com/fwlink/?LinkID=149156&v=4.0.50401.0" style="text-decoration:none">
            <img src="http://go.microsoft.com/fwlink/?LinkId=108181" alt="Get Microsoft Silverlight" style="border-style:none"/>
         </a>
      </object>
      <iframe id="_sl_historyFrame" style='visibility: hidden; height: 0; width: 0; border: 0px'></iframe>

   </div>

   <br />
   
   <div>
      <div class="filter-actions ui-widget">
         <%--<a href="#">Details</a>
         |
         <a href="#">Thumbnails</a>--%>
<%--         View&nbsp;
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
--%>
      </div>
      
      <table id="media" class="grid ui-widget ui-widget-content ui-corner-all ui-shadow">
         <thead class="ui-widget-header ui-corner-top">
            <tr>
               <th class="width-50"></th>
               <th>File</th>
               <th class="width-15perc">Date</th>
            </tr>
         </thead>
         <tfoot class="ui-widget-header ui-corner-bottom">
            <tr>
               <th colspan="3">
                  <%--<% if (Model != null){ %>
		               <%= Html.Pager(Model.PageSize, Model.PageNumber, Model.TotalItemCount )%>
                  <% } %>--%>
               </th>
            </tr>
         </tfoot>
         <tbody class="ui-widget-content">
            <% if (Model != null && Model.Count > 0) { %>
               <% foreach (MediaModel media in Model) { %>
                  <tr>
                     <td class="width-50">
                        <%= Html.MediaImage(media.Name) %>
                     </td>
                     <td class="multitext">
                        <strong><%= Html.Encode(media.Name) %></strong>
                        <br />
                        <span><%= media.MimeType %></span>
                        <div class="hover-actions">
                           &nbsp;&nbsp;&nbsp;
                           <%--<span class="ui-icon ui-icon-cancel"></span>
                           <a href="#" class="ui-state-disabled">View</a>
                           
                           <span class="separator">&nbsp;|&nbsp;</span>
                           
                           <span class="ui-icon ui-icon-star"></span>
                           <a href="#" class="ui-state-disabled">Edit</a>
                           
                           <span class="separator">&nbsp;|&nbsp;</span>--%>
                           
                           <span class="ui-icon ui-icon-circle-close"></span>
                           <a class="delete-media" href="#" >Delete permanently</a>
                           <input class="media-name" type="hidden" value="<%= media.Name %>" />
                        </div>
                     </td>
                     <td>
                        <%= media.LastModifiedDate.AdjustDateToTimeZone(RequestContext.CurrentUser.TimeZone).ToShortDateString() %>
                     </td>
                  </tr>
               <% } %>
            <% } else { %>
               <!-- Empty Template -->
               <tr class="emptyrow">
                  <td colspan="4">No media exists....</td>
               </tr>
            <% } %>
         </tbody>
      </table>
   </div>
   
   <div id="adminpagefooter" class="ui-widget">
      <a href='<%= Url.Action("Index", "Site", new {siteid = RequestContext.ManagedSite.SiteId}) %>'><%= GlobalResource("Form_BackToControlPanel") %></a>
   </div>
   
   <% Html.RenderPartial("~/Areas/Admin/Views/Shared/Upload.ascx", uploadModel); %>
</asp:Content>

