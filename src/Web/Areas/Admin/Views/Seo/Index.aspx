<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Site.Master" Inherits="Arashi.Web.Mvc.Views.AdminViewPageBase<Arashi.Core.Domain.SeoSettings>" %>
<%@ Import Namespace="Arashi.Core.Domain" %>
<asp:Content ContentPlaceHolderID="head" runat="server">
	<title>SEO Settings</title>
</asp:Content>

<asp:Content ContentPlaceHolderID="BreadCrumbsPlaceHolder" runat="server">
   <%= Html.Breadcrumbs(b => b.AddHome("Home", Url.Action("Index", "Site", new {siteid = RequestContext.ManagedSite.SiteId}))
                              .AddCurrent("SEO Settings")) %>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
   <div id="adminpagetitle">
      <img class="icon" src="/Resources/img/32x32/settings.png" alt="" />
      <h2>SEO Settings</h2>
   </div>
   <div class="clear"></div>

   <% using (Html.BeginForm("Save", "Seo", new { siteid = RequestContext.ManagedSite.SiteId }, FormMethod.Post, new { id = "seosettingsform", @class = "ui-widget ui-form-default" })){ %>
      <ol>
         <li>
            <label for="HomeTitle">Home Title:</label>
            <%= Html.TextArea("HomeTitle", Model.HomeTitle, new {@class = "largetext"}) %>
         </li>
         <li>
            <label for="HomeDescription">Home Description:</label>
            <%= Html.TextArea("HomeDescription", Model.HomeDescription, new {@class = "largetext"}) %>
         </li>
         <li>
            <label for="HomeKeywords">Home Keywords:</label>
            <%= Html.TextArea("HomeKeywords", Model.HomeKeywords, new {@class = "largetext"}) %>
         </li>
         <li>
            <label for="RewriteTitles">Rewrite Titles:</label>
            <%= Html.CheckBox("RewriteTitles", Model.RewriteTitles)%>
         </li>
         <li>
            <label for="PostTitleFormat">Post Title Format:</label>
            <%= Html.TextBox("PostTitleFormat", Model.PostTitleFormat, new {maxlength = "50", @class = "largetext"}) %>
         </li>
         <li>
            <label for="PageTitleFormat">Page Title Format:</label>
            <%= Html.TextBox("PageTitleFormat", Model.PageTitleFormat, new {maxlength = "50", @class = "largetext"}) %>
         </li>
         <li>
            <label for="CategoryTitleFormat">Category Title Format:</label>
            <%= Html.TextBox("CategoryTitleFormat", Model.CategoryTitleFormat, new {maxlength = "50", @class = "largetext"}) %>
         </li>
         <li>
            <label for="TagTitleFormat">Tag Title Format:</label>
            <%= Html.TextBox("TagTitleFormat", Model.TagTitleFormat, new {maxlength = "50", @class = "largetext"}) %>
         </li>
         <li>
            <label for="SearchTitleFormat">Search Title Format:</label>
            <%= Html.TextBox("SearchTitleFormat", Model.SearchTitleFormat, new {maxlength = "50", @class = "largetext"}) %>
         </li>
         <li>
            <label for="ArchiveTitleFormat">Archive Title Format:</label>
            <%= Html.TextBox("ArchiveTitleFormat", Model.ArchiveTitleFormat, new {maxlength = "50", @class = "largetext"}) %>
         </li>
         <li>
            <label for="Page404TitleFormat">404 Title Format:</label>
            <%= Html.TextBox("Page404TitleFormat", Model.Page404TitleFormat, new {maxlength = "50", @class = "largetext"}) %>
         </li>
         <li>
            <label for="DescriptionFormat">Description Format:</label>
            <%= Html.TextBox("DescriptionFormat", Model.DescriptionFormat, new {maxlength = "50", @class = "largetext"}) %>
         </li>
         <li>
            <label for="UseCategoriesForMeta">Use Categories for Meta:</label>
            <%= Html.CheckBox("UseCategoriesForMeta", Model.UseCategoriesForMeta)%>
         </li>
         <li>
            <label for="GenerateKeywordsForPost">Generate Keywords For Post:</label>
            <%= Html.CheckBox("GenerateKeywordsForPost", Model.GenerateKeywordsForPost)%>
         </li>
         <li>
            <label for="UseNoIndexForCategories">Use NoIndex For Categories:</label>
            <%= Html.CheckBox("UseNoIndexForCategories", Model.UseNoIndexForCategories)%>
         </li>
         <li>
            <label for="UseNoIndexForArchives">Use NoIndex For Archives:</label>
            <%= Html.CheckBox("UseNoIndexForArchives", Model.UseNoIndexForArchives)%>
         </li>
         <li>
            <label for="UseNoIndexForTags">Use NoIndex For Tags:</label>
            <%= Html.CheckBox("UseNoIndexForTags", Model.UseNoIndexForTags)%>
         </li>
         <li>
            <label for="GenerateDescriptions">Generate Descriptions:</label>
            <%= Html.CheckBox("GenerateDescriptions", Model.GenerateDescriptions)%>
         </li>
         <li>
            <label for="CapitalizeCategoryTitles">Capitalize Category Titles:</label>
            <%= Html.CheckBox("CapitalizeCategoryTitles", Model.CapitalizeCategoryTitles)%>
         </li>
      </ol>  
   
      <div id="adminpagefooter" class="ui-widget">
         <%= Html.AntiForgeryToken() %>
         <%= Html.SubmitUI(GlobalResource("Form_Save")) %>	
         &nbsp;|&nbsp;
         <a href='<%= Url.Action("Index", "Site", new {siteid = RequestContext.ManagedSite.SiteId}) %>'><%= GlobalResource("Form_BackToControlPanel") %></a>
      </div>
       
   <% } %>

</asp:Content>