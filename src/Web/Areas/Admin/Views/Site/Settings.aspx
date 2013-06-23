<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Site.Master" Inherits="Arashi.Web.Mvc.Views.AdminViewPageBase<Arashi.Core.Domain.Site>" %>
<%@ Import Namespace="Arashi.Web.Areas.Admin.Extensions"%>
<%@ Import Namespace="Arashi.Core.Domain" %>
<asp:Content ContentPlaceHolderID="head" runat="server">
	<title>Configurations</title>
<% Html.Telerik().ScriptRegistrar()
                 .Scripts(script => script.AddGroup("js.site.settings",
                                                     group => group.Add("admin.site.settings.js")
                                                    )); 
%>
</asp:Content>

<asp:Content ContentPlaceHolderID="ScriptPlaceHolder" runat="server">
	<script type="text/javascript">
      var urlGetHosts = '<%= Url.Action("GetHosts", "Site", new { siteid = RequestContext.ManagedSite.SiteId }) %>';
      var urlSaveHost = '<%= Url.Action("SaveHost", "Site", new { siteid = RequestContext.ManagedSite.SiteId }) %>';
      var urlAddHost = '<%= Url.Action("AddHost", "Site", new { siteid = RequestContext.ManagedSite.SiteId }) %>';
      var lastsel; 
	</script>
</asp:Content>

<asp:Content ContentPlaceHolderID="BreadCrumbsPlaceHolder" runat="server">
   <%= Html.Breadcrumbs(b => b.AddHome("Home", Url.Action("Index", "Site", new {siteid = RequestContext.ManagedSite.SiteId}))
                              .AddCurrent("Configurations")) %>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
   <div id="adminpagetitle">
      <img class="icon" src="/Resources/img/32x32/settings.png" alt="" />
      <h2>Configurations</h2>
   </div>
   <div class="clear"></div>

<% using (Html.BeginForm("Save", "Site", new { siteid = RequestContext.ManagedSite.SiteId }, FormMethod.Post, new { id = "sitesettingsform", @class = "ui-form-default" })){ %>

   <div id="tabs">
      <%= Html.AntiForgeryToken() %>
      <ul>
         <li><a href="#tabGeneral"><span>General</span></a></li>
         <li><a href="#tabHosts"><span>Domains</span></a></li>
         <li><a href="#tabPosts"><span>Posts</span></a></li>
         <li><a href="#tabComments"><span>Comments</span></a></li>
         <li><a href="#tabCaptcha"><span>Spam Protection</span></a></li>
         <li><a href="#tabAnalytics"><span>Google Analytics</span></a></li>
      </ul>
      
      <div id="tabGeneral">
         <ol>
            <li>
               <label for="Name">Name:</label>
               <%= Html.TextBox("Name", Model.Name, new {maxlength = "70", @class = "largetext"}) %>
            </li>
            <li>
               <label for="Description">Description:</label>
               <%= Html.TextBox("Description",Model.Description, new {maxlength = "160", @class = "largetext"}) %>
            </li>
            <li>
               <label for="Email">Email:</label>
               <%= Html.TextBox("Email", Model.Email, new {maxlength = "100", @class = "largetext"}) %>
            </li>
            <li>
               <label for="TimeZone" >Time Zone:</label>
               <%= Html.DropDownList("TimeZone", ViewData["TimeZones"] as SelectList)%>
            </li>
<%-- TODO: to implement!
            <li>
               <label for="DateFormat">* Nome:</label>
               <%= Html.TextBox("Description",Model.Description, new {maxlength = "100", @class = "largetext"}) %>
            </li>
            <li>
               <label for="TimeFormat">* Nome:</label>
               <%= Html.TextBox("Description",Model.Description, new {maxlength = "100", @class = "largetext"}) %>
            </li>
--%>
<%-- Status is no more supported - Replaced by the MaintenanceFilter 
            <li>
               <label for="Status">Site Status:</label>
               <%= Html.DropDownList("Status", ViewData["SiteStatus"] as SelectList, new {@class = "largetext"}) %>
               &nbsp;
               <% if (Model.Status != SiteStatus.Online) { %>
                  <img src="/Resources/img/16x16/exclamation_diamond_frame.png" alt="alert, site not online!" />
               <% } %>
            </li>--%>
            <li>
               <label for="DefaultCulture">Default language:</label>
               <%= Html.DropDownList("DefaultCulture", ViewData["Cultures"] as SelectList, new {@class = "largetext"})%>
            </li>
         </ol>
<%--         
         <fieldset>
            <legend>Multi language support</legend>
            <div class="form-item">
               <label for="IsCultureSupportEnabledCheckbox">Enable multi language support:</label>
               <%= Html.CheckBox("IsCultureSupportEnabledCheckbox", Model.IsCultureSupportEnabled, new {@class = "checkbox"}) %>
            </div>            
         </fieldset>--%>
      </div>
      
      <div id="tabHosts">
         <p>Here are listed the domains associated to the current site. Each site <strong>must have</strong> at least 1 domain.</p>
         <br />
         <span class="hint">Click on a row to edit, press Enter to save or Esc to cancel</span>
         <br />
         <table id="hosts-table" >
            <tbody><tr><td></td></tr></tbody>
         </table>
         <br />
         <fieldset class="ui-widget ui-widget-content ui-corner-all ui-form-default width-450">
            <legend class="ui-widget-header ui-corner-all">New Domain</legend>
            <ol>
               <li>
                  <label for="">New domain:</label>
                  <input type="text" id="newhostname" name="newhostname" class="mediumtext" />
               </li>
               <li>
                  <label>&nbsp;</label>
                  <a id="newsitehost-button" class="button ui-shadow" href="#"><%= GlobalResource("Form_Save") %></a>
               </li>
            </ol>
         </fieldset>
      </div>
      
      <div id="tabPosts">
         <ol>
            <li>
               <label>Home page shows:</label>
               <input type="radio" id="home-latestpost" name="homepage" value="latest-post" <%= Model.DefaultPage == null ? "checked='checked'" : string.Empty %> />
               <%--<%= Html.RadioButton("HomePage", false, (Model.DefaultPage == null ), new {id = "home-latestpost"})%>--%>
               <label for="home-latestpost">Latest posts</label>
            </li>
            <li>
               <label>&nbsp;</label>
               <input type="radio" id="home-page" name="homepage" value="page" <%= Model.DefaultPage != null ? "checked='checked'" : string.Empty %> />
               <label id="defaultpage-label" for="PagesForHomePage">Specific page:</label>
               <%= Html.DropDownList("PagesForHomePage", ViewData["Pages"] as SelectList, "", new {@class = "mediumtext"}) %>
            </li>
            <li>
               <label for="MaxPostsPerPage">Blog pages show at most:</label>
               <%= Html.TextBox("MaxPostsPerPage", Model.MaxPostsPerPage, new {maxlength = "2", @class = "shorttext"}) %>
            </li>
            <li>
               <label for="MaxSyndicationFeeds">Syndication feeds show the most recent:</label>
               <%= Html.TextBox("MaxSyndicationFeeds", Model.MaxSyndicationFeeds, new {maxlength = "2", @class = "shorttext"}) %>
            </li>
            <li>
               <label>For each article in a feed, show:</label>
               <%--<input type="radio" id="feed-full" name="FeedUseSummary" value="false" <%= Model.FeedUseSummary ? "checked=\"checked\"" : "" %> />--%>
               <%= Html.RadioButton("FeedUseSummary", false, Model.FeedUseSummary, new {id = "feed-full"})%>
               <label for="feed-full">Full content</label>
            </li>
            <li>
               <label>&nbsp;</label>
               <%--<input type="radio" id="feed-summary" name="FeedUseSummary" value="true" <%= Model.FeedUseSummary ? "checked=\"checked\"" : "" %> />--%>
               <%= Html.RadioButton("FeedUseSummary", true, Model.FeedUseSummary, new {id = "feed-summary"})%>
               <label for="feed-summary">Summary</label>
            </li>
         </ol>
      </div>
      
      <div id="tabComments">
         <ol>
            <li>
               <label for="AllowPings" >Allow link notifications from other blogs (pingbacks and trackbacks.):</label>
               <%= Html.CheckBox("AllowPings", Model.AllowPings)%>
            </li>
            <li>
               <label for="AllowComments" >Allow people to post comments on the article:</label>
               <%= Html.CheckBox("AllowComments", Model.AllowComments)%>
            </li>
            <li>
               <label for="AllowCommentsOnlyForRegisteredUsers" >Users must be registered and logged in to comment:</label>
               <%= Html.CheckBox("AllowCommentsOnlyForRegisteredUsers", Model.AllowCommentsOnlyForRegisteredUsers)%>
            </li>
            <li>
               <label for="MaxCommentsPerPage">Show comments in block of :</label>
               <%= Html.TextBox("MaxCommentsPerPage", Model.MaxCommentsPerPage, new {maxlength = "2", @class = "shorttext"}) %>
            </li>
            <li>
               <label for="SortCommentsFromOlderToNewest">Display comments from older to newest:</label>
               <%= Html.CheckBox("SortCommentsFromOlderToNewest", Model.SortCommentsFromOlderToNewest)%>
            </li>
            <li>
               <label for="SendEmailForNewComment">Send me an email when anyone post a new comment:</label>
               <%= Html.CheckBox("SendEmailForNewComment", Model.SendEmailForNewComment)%>
            </li>
            <li>
               <label for="SendEmailForNewModeration">Send me an email when a comment is held for moderation:</label>
               <%= Html.CheckBox("SendEmailForNewModeration", Model.SendEmailForNewModeration)%>
            </li>
            <li>
               <label for="ShowAvatars">Show avatars:</label>
               <%= Html.CheckBox("ShowAvatars", Model.ShowAvatars)%>
            </li>
         </ol>
         
         <br />
         
         <fieldset class="ui-widget-content ui-corner-all">
            <legend class="ui-widget-header ui-corner-all">Moderation</legend>
            <ol>
               <li>
                  <label for="MaxLinksInComments">Max number of links in comments:</label>
                  <%= Html.TextBox("MaxLinksInComments", Model.MaxLinksInComments, new {maxlength = "2", @class = "shorttext"}) %>
               </li>
               <li>
                  <label for="ModerationKeys">Moderation keywords:</label>
                  <%= Html.TextArea("ModerationKeys", Model.ModerationKeys, 6, 30, new {@class = "largetext"}) %>
                  <span class="hint">Separate each word with a new line</span>
               </li>
               <li>
                  <label for="BlacklistKeys">Blacklist keywords:</label>
                  <%= Html.TextArea("BlacklistKeys", Model.BlacklistKeys, 6, 30, new {@class = "largetext"}) %>
                  <span class="hint">Separate each word with a new line</span>
               </li>
            </ol>
         </fieldset>
      </div>
      
      <div id="tabCaptcha">
         <p class="block-align width-600">
            In order to protect your site from spam enable the use of the reCaptcha service, 
            it will help you to greatly reduce the number of undesired spam comments and contacts.
            <br />
            To use this service, you must register on <a href="http://recaptcha.net/" title="Go to the reCaptcha site">reCaptcha site</a> 
            in order to obtain the couple of keys that must be inserted in the fields below.
         </p>
         <a class="block-align" href="http://recaptcha.net/" title="Go to the reCaptcha site">
            <img src="/Resources/img/recaptcha_logo.png" alt="recaptcha logo" />
         </a>
         <ol> 
            <li>
               <label for="EnableCaptchaForComments">Enable captcha for comments:</label>
               <%= Html.CheckBox("EnableCaptchaForComments", Model.EnableCaptchaForComments)%>
               <span class="hint">For security reasons, it's safe to leave this option checked</span>
            </li>
         </ol>
         <br />
         <fieldset class="ui-widget-content ui-corner-all">
            <legend class="ui-widget-header ui-corner-all">reCaptcha settings:</legend>
            <ol>
               <li>
                  <label for="CaptchaPrivateKey">Private Key:</label>
                  <%= Html.TextBox("CaptchaPrivateKey", Model.CaptchaPrivateKey, new {@class = "largetext"}) %>
               </li>
               <li>
                  <label for="CaptchaPublicKey">Public Key:</label>
                  <%= Html.TextBox("CaptchaPublicKey", Model.CaptchaPublicKey, new {@class = "largetext"}) %>
               </li>
            </ol>
         </fieldset>         
      </div>
      
      <div id="tabAnalytics">
         <p>Insert here your Google Analytics tracking code:</p>
         <ol>
            <li>
               <label for="TrackingCode">&nbsp;</label>
               <%= Html.TextBox("TrackingCode", Model.TrackingCode, new {@class = "mediumtext"}) %>
            </li>
         </ol>
      </div>
   </div>
   
   <div id="adminpagefooter" class="ui-widget">
      <%= Html.SubmitUI(GlobalResource("Form_Save")) %>	
      &nbsp;|&nbsp;
      <a href='<%= Url.Action("Index", "Site", new {siteid = RequestContext.ManagedSite.SiteId}) %>'><%= GlobalResource("Form_BackToControlPanel") %></a>
   </div>
   
<% } %> 
   
</asp:Content>
