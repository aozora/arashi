<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="xVal.Rules"%>
<%@ Import Namespace="xVal.ClientSidePlugins.TestHelpers" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
    <head>
        <script type="text/javascript" src="<%= ResolveUrl("~/Tests/xVal.jQuery.Validate/jquery-1.3.2.js") %>"></script>
        <script type="text/javascript" src="<%= ResolveUrl("~/Tests/xVal.jQuery.Validate/jquery.validate.js") %>"></script>
        <script type="text/javascript" src="<%= ResolveUrl("~/ClientSidePlugins/xVal.jquery.validate.js?nocache=" + DateTime.Now.Ticks) %>"></script>
        <script type="text/javascript" src="<%= ResolveUrl("~/Messages/xVal.Messages.ForUnitTests.js") %>"></script>
        <script type="text/javascript">
            function EqualsFixedStringRule(value, element, params) {
                return (value == params.mustMatch);
            }
        </script>        
    </head>
    <body>
        <%= Html.ClientSideValidation("myprefix", SampleRuleSets.AllPossibleRules)
            .UseValidationSummary("myValidationSummary", "Client-generated validation summary header")
            .AddRule("RemotelyValidated_Field", new RemoteRule(Url.Action("EvaluateAbcRule")))%>
        
        <% using(Html.BeginForm()) { %>
            <div id="myValidationSummary">
                <%= Html.ValidationSummary("Server-generated validation summary header")%>
            </div>
        
            <table border="0">
                <% foreach(var fieldName in SampleRuleSets.AllPossibleRules.Keys) { %>
                    <tr>
                        <td><%= fieldName %></td>
                        <td><%= Html.TextBox("myprefix." + fieldName) %></td>
                    </tr>
                <% } %>
            </table>
            <input type="submit" id="submitButton" />
        <% } %>        
    </body>
</html>
