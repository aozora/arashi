<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.TemplateBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>
<div id="tabs">

    <ul class="idTabs tabs clearfix">
        <li class="nav1"><a href="#comm"><img src='<%= bloginfo("template_directory") %>/images/ico-1.gif' alt="Comments" /></a></li>
        <li class="nav2"><a href="#pop"><img src='<%= bloginfo("template_directory") %>/images/ico-2.gif' alt="Popular" /></a></li>
        <li class="nav3"><a href="#tagcloud"><img src='<%= bloginfo("template_directory") %>/images/ico-5.gif' alt="Tags" /></a></li>												
    </ul>
    <div class="inside">

        <ul id="comm">
            <% Html.RenderPartial(Model.Site.Template.BasePath + "/includes/comments.ascx"); %>
        </ul>

        <ul id="pop">
            <% Html.RenderPartial(Model.Site.Template.BasePath + "/includes/popular.ascx"); %>
        </ul>

        <div id="tagcloud">
            <%= wp_tag_cloud("smallest=12&largest=20") %>
        </div>

    </div><!--inside-->

</div><!--tabs-->
<br />