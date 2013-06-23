<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Site.Master" Inherits="Arashi.Web.Mvc.Views.AdminViewPageBase<IPagedList<Arashi.Core.Domain.Message>>" %>
<%@ Import Namespace="Arashi.Web.Mvc.Gravatar"%>
<%@ Import Namespace="Arashi.Core.Extensions"%>
<%@ Import Namespace="Arashi.Core.Domain"%>
<%@ Import Namespace="Arashi.Web.Mvc.Paging"%>
<asp:Content ContentPlaceHolderID="head" runat="server">
	<title>Messages</title>
<% Html.Telerik().ScriptRegistrar()
                 .Scripts(script => script.AddGroup( "js.messages.index",
                                                     group => group.Add("admin.messages.js")
                                                   )
                                    ); 
%>
</asp:Content>

<asp:Content ContentPlaceHolderID="BreadCrumbsPlaceHolder" runat="server">
   <%= Html.Breadcrumbs(b => b.AddHome("Home", Url.Action("Index", "Site", new {siteid = RequestContext.ManagedSite.SiteId}))
                              .AddCurrent("Messages")) %>
</asp:Content>


<asp:Content ContentPlaceHolderID="MainContent" runat="server">
   <div id="adminpagetitle">
      <img class="icon" src="/Resources/img/32x32/mailbox.png" alt="notification messages" />
      <h2>Messages</h2>
   </div>
   <div class="clear"></div>

   <div>
      <%--<div class="filter-actions ui-widget">
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
      </div>--%>
      
      <table id="comments" class="grid ui-widget ui-widget-content ui-corner-all ui-shadow">
         <thead class="ui-widget-header ui-corner-top">
            <tr>
               <th>From</th>
               <th>To</th>
               <th>Subject</th>
               <th>Type</th>
               <th>Status</th>
               <th>Attempts</th>
               <th class="align-center">Created<br />Date</th>
               <th class="align-center">Updated<br />Date</th>
            </tr>
         </thead>
         <tfoot class="ui-widget-header ui-corner-bottom">
            <tr>
               <th colspan="8">
                  <% if (Model != null){ %>
		               <%= Html.Pager(Model.PageSize, Model.PageNumber, Model.TotalItemCount )%>
                  <% } %>
               </th>
            </tr>
         </tfoot>
         <tbody class="ui-widget-content">
            <% if (Model != null){ %>
               <% foreach (Message message in Model) { %>
                  <tr>
                     <td class="multitext">
                        <%= Html.Encode(message.From) %>
                        <div class="hover-actions">
                           &nbsp;&nbsp;&nbsp;
                           <span class="ui-icon ui-icon-search"></span>
                           <%= Html.ActionLink("View", "GetMessageBody", "Messages", new {siteid = RequestContext.ManagedSite.SiteId, id = message.MessageId}, new {@class = "button-secondary view"}) %>
                           <span class="separator">&nbsp;|&nbsp;</span>
                           <% if (message.Status != MessageStatus.Queued && message.Status != MessageStatus.Sent) { %>
                              <span class="ui-icon ui-icon-refresh"></span>
                              <%= Html.ActionLink("Send again", "ResetMessage", "Messages", new {siteid = RequestContext.ManagedSite.SiteId, id = message.MessageId}, new {@class = "button-secondary reset"}) %>
                           <% } else { %>
                              <span class="ui-icon ui-icon-stop"></span>
                              <%= Html.ActionLink("Block", "BlockMessage", "Messages", new {siteid = RequestContext.ManagedSite.SiteId, id = message.MessageId}, new {@class = "button-secondary reset"}) %>
                           <% } %>
                           <span class="separator">&nbsp;|&nbsp;</span>
                           <span class="ui-icon ui-icon-circle-close"></span>
                           <%= Html.ActionLink("Delete", "Delete", "Messages", new {siteid = RequestContext.ManagedSite.SiteId, id = message.MessageId}, new {@class = "button-secondary delete"}) %>
                        </div>
                     </td>
                     <td>
                        <%= Html.Encode(message.To) %>
                     </td>
                     <td>
                        <%= Html.Encode(message.Subject.Truncate(200, true)) %>
                     </td>
                     <td>
                        <img src='/Resources/img/16x16/<%= message.Type == MessageType.Email ? "mail.png" : "balloon.png" %>' alt="status" />&nbsp;
                        <%= message.Type.ToString() %>
                     </td>
                     <td>
                        <% switch (message.Status)
                           {
                              case MessageStatus.Queued: %>
                                 <img src="/Resources/img/16x16/hourglass.png" alt="queued" />
                        <%       break;
                              case MessageStatus.Sending:%>
                                 <img src="/Resources/img/16x16/arrow_circle_double_135.png" alt="sending" />
                        <%       break;
                              case MessageStatus.NotSent:%>
                                 <img src="/Resources/img/16x16/cross_circle_frame.png" alt="sending" />
                        <%       break;
                              case MessageStatus.Sent:%>
                                 <img src="/Resources/img/16x16/tick.png" alt="sending" />
                        <%       break;
                           } %>
                        <%= message.Status.ToString() %>
                     </td>
                     <td class="align-center">
                        <%= message.AttemptsCount.ToString() %>
                     </td>
                     <td class="align-right">
                        <%= message.CreatedDate.AdjustDateToTimeZone(RequestContext.CurrentUser.TimeZone).ToShortDateString() %>
                     </td>
                     <td class="align-right">
                        <% if (message.UpdatedDate.HasValue) { %>
                           <%= message.UpdatedDate.Value.AdjustDateToTimeZone(RequestContext.CurrentUser.TimeZone).ToShortDateString()%>
                        <% } else { %>
                           &nbsp;
                        <% } %>
                     </td>
                  </tr>
               <% } %>
            <% } else { %>
            <!-- Empty Template -->
               <tr class="emptyrow">
                  <td colspan="7">No messages exists....</td>
               </tr>
            <% } %>
         </tbody>
      </table>
   </div>
   
   <div id="adminpagefooter" class="ui-widget">
      <a href='<%= Url.Action("Index", "Site", new {siteid = RequestContext.ManagedSite.SiteId}) %>'><%= GlobalResource("Form_BackToControlPanel") %></a>
   </div>
</asp:Content>

