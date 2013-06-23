<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Site.Master" Inherits="Arashi.Web.Mvc.Views.AdminViewPageBase<DashboardModel>" %>
<%@ Import Namespace="Arashi.Core.Domain.Dto"%>
<%@ Import Namespace="Arashi.Web.Areas.Admin.Models"%>
<%@ Import Namespace="Arashi.Core.Domain"%>
<asp:Content ContentPlaceHolderID="head" runat="server">
	<title>Control Panel</title>
<%--<% Html.Telerik().StyleSheetRegistrar()
                 .StyleSheets(css => css.AddGroup( "css.site.index",
                                                   group => group.Add("admin.layout2col-alt.css")
                                                                 //.Combined(true)
                                                  )); 
%>   --%>
</asp:Content>

<asp:Content ContentPlaceHolderID="ScriptPlaceHolder" runat="server">
	<script type="text/javascript">
	   var showWelcome = <%= Model.ShowWelcome.ToString().ToLowerInvariant() %>;
	   
	   $(function() {
         var tutorialLink = $("<a id='showTutorial' href='#'>Show the Tutorial</a>");

         $("#help-container").append(tutorialLink);

         $("#showTutorial").click(function(){
            return startTutorial();
         });
         
         if (showWelcome) {
            // Setup Welcome dialog
            $("#closeWelcomeDialog").live("click", function(){
               $.unblockUI();
            });
            
            $("#startTutorial").live("click", function(){
               return startTutorial();
            });
            
            $.blockUI({ 
                  title:    $("#welcome-dialog").attr("title"), 
                  message:  $("#welcome-dialog").html(), 
                  theme:    true, 
	               themedCSS: {
		               width:	'40%',
		               top:	'30%',
		               left:	'35%'
	               },
                  draggable: false, 
                  timeout:  0 
              }); 
         }        
	      
	      
	   });	
	</script>
</asp:Content>

<asp:Content ContentPlaceHolderID="BreadCrumbsPlaceHolder" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

   <div id="adminpagetitle">
      <img class="icon" src="/Resources/img/32x32/internet.png" alt="" />
      <h2>Control Panel</h2>
   </div>
   <div class="clear"></div>

   <fieldset id="stats" class="">
      <div class="controlpanelicon nohover ui-iconplustext ui-corner-all" >
         <img src='/Resources/img/32x32/word.png' alt='posts' />
         <em>
            <span class="ui-iconplustext-title">Posts</span>
            <% if (Model.ContentItemStats != null) {
                  foreach (ContentItemsCountByWorkflowStatus<Post> item in Model.ContentItemStats.PostCountByWorkflowStatus) { %>
                     <span><%= item.Status.ToString() %>:&nbsp;<strong><%= item.Count.ToString() %></strong></span>
            <%    }
               } %>
         </em>
      </div>
      <div class="controlpanelicon nohover ui-iconplustext ui-corner-all" >
         <img src='/Resources/img/32x32/comments.png' alt='comments' />
         <em>
            <span class="ui-iconplustext-title">Comments</span>
            <% if (Model.ContentItemStats != null) {
                  foreach (CommentsCountByWorkflowStatus item in Model.ContentItemStats.CommentsCountByStatus) { %>
                     <span><%= item.Status.ToString() %>:&nbsp;<strong><%= item.Count.ToString() %></strong></span>
            <%    }
               } %>
         </em>
      </div>
      <div class="controlpanelicon nohover ui-iconplustext ui-corner-all" >
         <img src='/Resources/img/32x32/pages.png' alt='pages' />
         <em>
            <span class="ui-iconplustext-title">Pages</span>
            <% if (Model.ContentItemStats != null) {
                  foreach (ContentItemsCountByWorkflowStatus<Arashi.Core.Domain.Page> item in Model.ContentItemStats.PageCountByWorkflowStatus) { %>
                     <span><%= item.Status.ToString() %>:&nbsp;<strong><%= item.Count.ToString() %></strong></span>
            <%    }
               } %>
         </em>
      </div>
      <div class="controlpanelicon nohover ui-iconplustext ui-corner-all" >
         <img src='/Resources/img/32x32/folder_green.png' alt='tags' />
         <em>
            <span class="ui-iconplustext-title">Categories</span>
            <span>Total:&nbsp;<strong><%= Model.ContentItemStats.CategoriesTotalCount.ToString() %></strong></span>
         </em>
      </div>
      <div class="controlpanelicon nohover ui-iconplustext ui-corner-all" >
         <img src='/Resources/img/32x32/tag.png' alt='comments' />
         <em>
            <span class="ui-iconplustext-title">Tags</span>
            <span>Total:&nbsp;<strong><%= Model.ContentItemStats.TagsTotalCount.ToString()%></strong></span>
         </em>
      </div>
   </fieldset>
   
   <br />
   
   <% foreach (var group in Model.ControlPanelModels) { %>
      <fieldset class="controlpanelgroup ui-widget ui-widget-content ui-corner-all">
         <legend class="ui-widget-header ui-corner-all"><%= Html.Encode(group.Category)%></legend>
         <% foreach (ControlPanelItem item in group.Items) { %>
            <a class="button controlpanelicon ui-iconplustext ui-button-text-icon button-icon-right ui-shadow" 
               href='<%= Url.Action(item.Action, item.Controller, new {siteid = RequestContext.ManagedSite.SiteId}) %>' >
               <img src='<%= item.ImageSrc %>' alt='<%= Html.Encode(item.ImageAlt) %>' />
               <em>
                  <span class="ui-iconplustext-title ui-title"><%= Html.Encode(item.Text) %></span>
                  <span><%= Html.Encode(item.Description) %></span>
               </em>
            </a>
         <% } %>
      </fieldset>
      <br />
   <% } %>
   
   <br />
   
   <div class="hidden">
      <div id="welcome-dialog" title="Welcome to the Arashi Control Panel">
            <img class="block-align-left" src="/Resources/img/48x48/tutorials.png" alt="tutorial" />
            <p>This is the main place from wich you can author & configure your site.</p>
            <p>To became familiar on how to use this screen I suggest to take a view to a very quick tutorial.</p>
            <p>In any case, you can start the tutorial anytime by clicking on the link in the top right of the Control Panel home.</p>
            <br />
            <p class="align-center">
               <a id="startTutorial" 
                  href="#"
                  class="button ui-state-default ui-priority-primary ui-corner-all call-to-action ui-state-active">
                  Start the quick tutorial
               </a>
               &nbsp;&nbsp;|&nbsp;&nbsp;
               <a id="closeWelcomeDialog" 
                  href="#"
                  class="ui-priority-secondary" >
                  Close
               </a>
            </p>
      </div>	
   </div>   
</asp:Content>

