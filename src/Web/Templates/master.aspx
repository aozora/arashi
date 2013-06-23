<%@ Page Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.ThemeMasterViewPageBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>
<% Html.RenderPartial(Model.Site.Template.BasePath + "/" + GetTemplateFile()); %>
