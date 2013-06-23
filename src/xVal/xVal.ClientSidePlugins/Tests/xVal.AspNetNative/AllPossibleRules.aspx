<%@ Page Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="xVal.ClientSidePlugins.TestHelpers"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <script type="text/javascript" src="<%= ClientScript.GetWebResourceUrl(typeof(System.Web.UI.Page), "WebForms.js") %>"></script>
    <script type="text/javascript" src="<%= ClientScript.GetWebResourceUrl(typeof(System.Web.UI.Page), "WebUIValidation.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/ClientSidePlugins/xVal.AspNetNative.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Messages/xVal.Messages.ForUnitTests.js") %>"></script>
    <script type="text/javascript">
        function EqualsFixedStringRule(value, element, params) {
            return (value == params.mustMatch);
        }
    </script>       
</head>
<body>
    <%= Html.ClientSideValidation("myprefix", SampleRuleSets.AllPossibleRules) %>

    <% using(Html.BeginForm()) { %>
        Generated at <%= DateTime.Now.ToLongTimeString() %>
        
        <table border="0">
            <% foreach(var fieldName in SampleRuleSets.AllPossibleRules.Keys) { %>
                <tr>
                    <td><%= fieldName %></td>
                    <td><%= Html.TextBox("myprefix." + fieldName) %></td>
                </tr>
            <% } %>
        </table>        
        
        <input id="submitButton" type="submit" value="Post now" />
    <% } %>
</body>
</html>
