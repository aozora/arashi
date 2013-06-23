<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.TemplateEngine.TemplateBase<Arashi.Web.Mvc.Models.TemplateContentModel>" %>
<div class="search">
   <form method="post" action='<%= get_option("home") %>/search/' >
      <fieldset>
         <input type="text" class="field" name="s" value="" />
         <input type="submit" class="button" name="send" value="" />
      </fieldset>
   </form>
</div>
