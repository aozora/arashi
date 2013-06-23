<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.Views.AdminViewUserControlBase<IList<MediaModel>>" %>
<%@ Import Namespace="Arashi.Core.Extensions"%>
<%@ Import Namespace="Arashi.Web.Areas.Admin.Models"%>
<table id="mediabrowser" class="grid ui-widget ui-widget-content ui-corner-all ui-shadow">
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
                  <span><%= Html.Encode(media.MimeType) %></span>
               </td>
               <td class="align-center width-100">
                  <div class="hover-actions">
                     &nbsp;&nbsp;&nbsp;
                     <a class="button ui-button-text-icon ui-state-default call-to-action ui-corner-all ui-shadow" 
                        href="#" 
                        onclick='insertMedia(<%= string.Format("\"{0}\"", Url.RouteUrl("media", new {name = media.Name})) %>)' >
                        <span class="ui-button-text">Select</span>
                     </a>
                  </div>
               </td>
            </tr>
         <% } %>
      <% } else { %>
         <!-- Empty Template -->
         <tr class="emptyrow">
            <td colspan="3">No media exists....</td>
         </tr>
      <% } %>
   </tbody>
</table>

