<%@ Import Namespace="Arashi.Web.Mvc.Models"%>
<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.Views.AdminViewUserControlBase<MessageModel>" %>
<!--msg-<%= Model.UIState %>-->
<div class="message ui-widget <%= Model.CssClass %>">
   <div class="ui-state-<%= Model.UIState %> ui-helper-clearfix ui-corner-all"> 
      <p>
         <span style="float: left; margin-right: 0.3em;" class="ui-icon ui-icon-<%= Model.Icon.ToString().ToLower() %>"></span>
         <%= Model.FullText %>
      </p>
      <% if (Model.IsClosable){ %>
         <a href="#" class="message-close ui-corner-all" title='<%= GlobalResource("Message_Close") %>' >
            <span class="ui-icon ui-icon-closethick" >close</span>
         </a>
      <% } %>
   </div>
</div>

