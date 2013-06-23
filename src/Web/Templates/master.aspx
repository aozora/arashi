<%@ Page Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.ThemeMasterViewPageBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>
<%@ Import Namespace=Arashi.Web.Mvc.TemplateEngine %>
<%@ Import Namespace=Arashi.Web.Mvc.Models %>
<% Html.RenderPartial(base.GetTemplateFile()); %>
