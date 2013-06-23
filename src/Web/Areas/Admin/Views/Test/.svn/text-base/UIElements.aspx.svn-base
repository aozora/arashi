<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="Arashi.Web.Areas.Admin.Extensions"%>
<asp:Content ContentPlaceHolderID="head" runat="server">
   <title>UI Elements Test Page</title>

   <script type="text/javascript">
      $(function() {
         // Tabs
         $('#tabs').tabs(2);

	      $("a.controlpanelicon:not(.ui-state-disabled, .nohover)").hover(
		      function() {
		         $(this).addClass("ui-state-hover");
		      },
		      function() {
		         $(this).removeClass("ui-state-hover");
		      }
	      );

      });
   </script>
</asp:Content>

<asp:Content ContentPlaceHolderID="BreadCrumbsPlaceHolder" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
   <div id="contentPageHeader">
      <h2>UI Elements Test Page</h2>
   </div>

   <div class="message ui-widget margin-top-high">
	   <div class="ui-state-highlight ui-helper-clearfix ui-corner-all"> 
		   <p>
		      <span style="float: left; margin-right: 0.3em;" class="ui-icon ui-icon-info"></span>
		      <strong>Hey!</strong> Sample ui-state-highlight style.
		   </p>
	   </div>
   </div>
   
   <div class="message ui-widget margin-top-high">
		<div class="ui-state-error ui-helper-clearfix ui-corner-all"> 
			<p>
			   <span style="float: left; margin-right: 0.3em;" class="ui-icon ui-icon-alert"></span>
			   <strong>Alert:</strong> Sample ui-state-error style. This message is closable!
			</p>
         <a href="#" class="message-close ui-corner-all" >
            <span class="ui-icon ui-icon-closethick" >close</span>
         </a>
		</div>
	</div>
   
   <br />
   
   <fieldset class="ui-widget ui-widget-content ui-corner-all">
      <legend class="ui-widget-header ui-corner-all">Legend</legend>
      <div>
         <p>
            This is the legend content.
            Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.
         </p>
      </div>
   </fieldset>
   
   <br class="clear" />
   
   <div class="width-550 ui-widget">
      <form action="#" class="ui-helper-reset ui-widget ui-widget-content ui-form-default">
         <ol>
            <li>
               <label>Label 1</label>
               <input type="text" class="mediumtext" />
               <span class="hint">This is a hint!</span>
            </li>
            <li>
               <!-- In a validation error occure, the input element take the "required" class,
                    and a span.ui-state-error with the validation message appears after. -->
               <label>Label 2</label>
               <input type="text" class="required" />
               <span class="ui-state-error ui-corner-all">This is a validation message</span>
            </li>
            <li>
               <label>This is a very very very long long long long label for test</label>
               <select class="mediumtext">
                  <option selected="selected" value="0">Option 0</option>
                  <option value="1">Option 1</option>
                  <option value="2">Option 2</option>
                  <option value="3">Option 3</option>
               </select>
            </li>
            <li>
               <label>Label 2</label>
               <textarea class="mediumtext"></textarea>
            </li>
            <li class="align-center">
               <%= Html.SubmitUI("Update")%>
               &nbsp;|&nbsp;
               <a href="#" class="ui-priority-secondary">Cancel action</a>
            </li>
         </ol>
      </form>
   </div>
  
   <br />
   
   <div id="tabs">
      <ul>
         <li><a href="#tabs-1">Form in tab</a></li>
         <li><a href="#tabs-2">...</a></li>
      </ul>
      
      <div id="tabs-1">
         <div class="content-padding width-550">
            <form action="#" class="ui-form-default">
               <ol>
                  <li>
                     <label>Label 1</label>
                     <input type="text" class="required mediumtext" />
                  </li>
                  <li>
                     <label>Label 2</label>
                     <input type="text" class="required" />
                  </li>
                  <li class="align-center">
                     <%= Html.SubmitUI("Update")%>
                  </li>
               </ol>
            </form>
         </div>
      </div>

      <div id="tabs-2">
         <br />
         <br />
         <br />
      </div>
   </div>
   
   <br />
   
   <!-- Grid -->
   <table class="grid ui-widget ui-state-default ui-corner-all">
      <thead>
         <tr>
            <th>Column 1</th>
            <th>Column 2</th>
            <th>Column 3</th>
            <th>Column 4</th>
            <th>Column 5</th>
            <th></th>
         </tr>
      </thead>
      <tbody class="ui-widget-content">
         <tr>
            <td class="title">
               <a href="#">
                  <span class="handle ui-icon ui-icon-arrowthick-2-n-s"></span>
                  Page title
               </a>
            </td>
            <td>Text</td>
            <td>Text</td>
            <td>Text</td>
            <td>Text</td>
            <td>
               <a class="button ui-state-default button-icon-left ui-corner-all" href="#">
                  <span class="ui-icon ui-icon-circle-plus"></span>
                  Action
               </a>
            </td>
         </tr>
         <tr>
            <td class="title">
               <a href="#">
                  <span class="handle ui-icon ui-icon-arrowthick-2-n-s"></span>
                  Page title
               </a>
            </td>
            <td>Text</td>
            <td>Text</td>
            <td>Text</td>
            <td>Text</td>
            <td>
               <a class="button ui-state-default button-icon-left ui-corner-all" href="#">
                  <span class="ui-icon ui-icon-circle-plus"></span>
                  Action
               </a>
            </td>
         </tr>
         <tr>
            <td class="title">
               <a href="#">
                  <span class="handle ui-icon ui-icon-arrowthick-2-n-s"></span>
                  Page title
               </a>
            </td>
            <td>Text</td>
            <td>Text</td>
            <td>Text</td>
            <td>Text</td>
            <td>
               <a class="button ui-state-default button-icon-left ui-corner-all" href="#">
                  <span class="ui-icon ui-icon-circle-plus"></span>
                  Action
               </a>
            </td>
         </tr>
         <tr>
            <td class="title">
               <a href="#">
                  <span class="handle ui-icon ui-icon-arrowthick-2-n-s"></span>
                  Page title
               </a>
            </td>
            <td>Text</td>
            <td>Text</td>
            <td>Text</td>
            <td>Text</td>
            <td>
               <a class="button ui-state-default button-icon-left ui-corner-all" href="#">
                  <span class="ui-icon ui-icon-circle-plus"></span>
                  Action
               </a>
            </td>
         </tr>
         <!-- Empty Template -->
         <tr class="emptyrow">
            <td colspan="6">Empty template....</td>
         </tr>
      </tbody>
      <tfoot class="ui-state-default ui-corner-bottom">
         <tr>
            <th colspan="6">
               <div class="pager">
                  <a href="#">&lt;</a>
                  <a href="#">1</a>
                  <a href="#">2</a>
                  <span class="current">3</span>
                  <a href="#">4</a>
                  <a href="#">5</a>
                  <span class="disabled">&gt;</span>
               </div>
            </th>
         </tr>
      </tfoot>
   </table>
   
   <br />
 
   <!-- Panels -->

   
   <br />
   <!-- Generic Icon + Text element -->
   <div class="ui-iconplustext">
      <img src="/Resources/img/32x32/empty.png" />
      <div>
         <span class="ui-iconplustext-title">New empty page</span>
         <span class="">Create a new empty page</span>
      </div>
   </div>
   
   <br />
   
   <!-- Action Icons -->
   <!-- TODO: usare display: inline-block invece dei float -->
   
   <div class="ui-widget ui-helper-clearfix">
      <a class="coolbutton ui-state-default ui-corner-all ui-iconplustext ui-helper-clearfix" href="#" >
         <img src="/Resources/img/32x32/empty.png" />
         <em>
            <span class="ui-iconplustext-title">New empty page</span>
            <span>Create a new empty page</span>
         </em>
      </a>
      <a class="coolbutton ui-state-default ui-corner-all ui-iconplustext ui-helper-clearfix" href="#" >
         <img src="/Resources/img/32x32/copy.png" />
         <em>
            <span class="ui-iconplustext-title">Copy</span>
            <span>Make a copy of existing</span>
         </em>
      </a>
      <a class="coolbutton ui-state-default ui-corner-all ui-iconplustext ui-helper-clearfix" href="#" >
         <img src="/Resources/img/32x32/doc.png" />
         <em>
            <span class="ui-iconplustext-title">Preconfigured</span>
            <span>New preconfigured page</span>
         </em>
      </a>
   </div>
   
   <br />
   
   <!-- ControlPanel Actions/Panels -->
   <fieldset class="ui-widget ui-widget-content ui-corner-all">
      <legend class="ui-widget-header ui-corner-all">Blog Statistics</legend>
      <div class="controlpanelitems-category-container">
         <div class="controlpanelitems-container-inner">
         
            <div class="controlpanelicon nohover ui-widget ui-iconplustext ui-corner-all" >
               <img src='/Resources/img/32x32/comments.png' alt='comments' />
               <em>
                  <span class="ui-iconplustext-title">New Comments</span>
                  <a href="#">Pending:&nbsp;<strong>8</strong></a>
               </em>
            </div>
            <div class="controlpanelicon nohover ui-widget ui-iconplustext ui-corner-all" >
               <img src='/Resources/img/32x32/word.png' alt='comments' />
               <em>
                  <span class="ui-iconplustext-title">New Post</span>
                  <a href="#">Pending:&nbsp;<strong>3</strong></a>
               </em>
            </div>
            <div class="controlpanelicon nohover ui-widget ui-iconplustext ui-corner-all" >
               <img src='/Resources/img/32x32/users2.png' alt='comments' />
               <em>
                  <span class="ui-iconplustext-title">New Users</span>
                  <a href="#">Pending:&nbsp;<strong>2</strong></a>
               </em>
            </div>
            
         </div>
      </div>
   </fieldset>

   <br />
   
   <fieldset class="ui-widget ui-widget-content ui-corner-all">
      <legend class="ui-widget-header ui-corner-all">Categories</legend>

      <a class="controlpanelicon ui-widget ui-iconplustext ui-corner-all" href="#" >
         <img src="/Resources/img/32x32/themes.png" alt="" />
         <em>
            <span class="ui-iconplustext-title">Title</span>
            <span>Description of the item</span>
         </em>
      </a>
      <a class="controlpanelicon ui-widget ui-iconplustext ui-corner-all" href="#" >
         <img src="/Resources/img/32x32/settings.png" alt="" />
         <em>
            <span class="ui-iconplustext-title">Title</span>
            <span>Description of the item</span>
         </em>
      </a>
      <a class="controlpanelicon ui-widget ui-iconplustext ui-corner-all" href="#" >
         <img src="/Resources/img/32x32/themes.png" alt="" />
         <em>
            <span class="ui-iconplustext-title">Title</span>
            <span>Description of the item</span>
         </em>
      </a>
      <a class="controlpanelicon ui-widget ui-iconplustext ui-corner-all" href="#" >
         <img src="/Resources/img/32x32/settings.png" alt="" />
         <em>
            <span class="ui-iconplustext-title">Title</span>
            <span>Description of the item</span>
         </em>
      </a>
      <a class="controlpanelicon ui-widget ui-iconplustext ui-corner-all" href="#" >
         <img src="/Resources/img/32x32/themes.png" alt="" />
         <em>
            <span class="ui-iconplustext-title">Title</span>
            <span>Description of the item</span>
         </em>
      </a>
      <a class="controlpanelicon ui-widget ui-iconplustext ui-corner-all" href="#" >
         <img src="/Resources/img/32x32/settings.png" alt="" />
         <em>
            <span class="ui-iconplustext-title">Title</span>
            <span>Description of the item</span>
         </em>
      </a>
      <a class="controlpanelicon ui-widget ui-iconplustext ui-corner-all" href="#" >
         <img src="/Resources/img/32x32/themes.png" alt="" />
         <em>
            <span class="ui-iconplustext-title">Title</span>
            <span>Description of the item</span>
         </em>
      </a>
      <a class="controlpanelicon ui-widget ui-iconplustext ui-corner-all" href="#" >
         <img src="/Resources/img/32x32/settings.png" alt="" />
         <em>
            <span class="ui-iconplustext-title">Title</span>
            <span>Description of the item</span>
         </em>
      </a>

   </fieldset>
   
   <br />
 
   <!-- DDM -->
   
   <br />
   <br />
   <br />

   
</asp:Content>
