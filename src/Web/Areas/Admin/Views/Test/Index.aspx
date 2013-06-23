<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Site.Master" Inherits="Arashi.Web.Mvc.Views.AdminViewPageBase" %>
<%@ Import Namespace="Arashi.Core.Domain"%>
<%@ Import Namespace="Arashi.Web.Mvc.Partials"%>
<%@ Import Namespace="Arashi.Web.Areas.Admin.Extensions"%>

<asp:Content ContentPlaceHolderID="head" runat="server">
   <title>Test</title>
<%--<% Html.Telerik().StyleSheetRegistrar()
                 .StyleSheets(css => css.AddGroup( "css.admin.test",
                                                   group => group.Add("ui.daterangepicker.css")
                                                                 .Add("ui.slider.extras.css")
                                                                 .Version("v1")
                                                                 //.Combined(true)
                                                  )); 
%> --%>  
<%--<% Html.Telerik().ScriptRegistrar()
                 .Scripts(script => script.AddGroup( "js.admin.test",
                                                     group => group.Add("fg.daterangepicker.js")
                                                                   .Add("fg.selectToUISlider.js")
                                                                   .Version("v1")
                                                                   //.Combined(true)
                                                    )); 
%>--%>

   <style type="text/css">
      #tabs-1 h2 { clear: both; margin:0; padding:2em 0 .5em; font-size:1.5em; } 
		.strike { text-decoration: line-through; }
		
		/* DateRange Picker Demo */
		input#rangeA {width: 196px; height: 1.1em; display:block;}
		
		/* Slider Demo */
		#tabs-4 fieldset { border:0; margin: 6em; height: 12em;}	
		#tabs-4 label {font-weight: normal; float: left; margin-right: .5em; font-size: 1.1em;}
		#tabs-4 select {margin-right: 1em; float: left;}
		#tabs-4 .ui-slider {clear: both; top: 5em;}
   </style> 
   
   <style type="text/css">

      .button2 {
         -moz-border-radius:5px;
         -webkit-border-radius: 5px;
         -moz-box-shadow:0 1px 3px rgba(0, 0, 0, 0.25);
         -webkit-box-shadow: 0 1px 3px rgba(0,0,0, 0.25);
         background: url("/Resources/img/overlay-button.png") repeat-x scroll 0 0 #222222;
         border-bottom:1px solid rgba(0, 0, 0, 0.25);
         color:#FFFFFF !important;
         cursor:pointer;
         display:inline-block;
         font-family : Lucida Grande,Lucida Sans,Arial,sans-serif;
         font-size:13px;
         font-weight:bold;
         line-height:1;
         overflow:visible;
         padding:5px 15px 6px;
         position:relative;
         text-decoration:none;
         text-shadow:0 -1px 1px rgba(0, 0, 0, 0.25);
         width:auto;
      }
      .button2:hover {
         background-color:#111111;
         color:#FFFFFF;
      }
      .button2:active {
         top:1px;
      }
      
      .small.button2 {
         font-size:11px;
      }
      .large.button2 {
         font-size:14px;
         padding:8px 19px 9px;
      }
      .green.button2 {
         background-color:#91BD09;
      }
      .green.button2:hover {
         background-color:#749A02;
      }
      .blue.button2 {
         background-color:#AED0EA;
         /*background-color:#2DAEBF;*/
      }
      .blue.button2:hover {
         /*background-color:#007D9A;*/
         background-color:#74B2E2;
      }
      .red.button2 {
         background-color:#E33100;
      }
      .red.button2:hover {
         background-color:#872300;
      }
      .magenta.button2 {
         background-color:#A9014B;
      }
      .magenta.button2:hover {
         background-color:#630030;
      }
      .orange.button2 {
         background-color:#FF5C00;
      }
      .orange.button2:hover {
         background-color:#D45500;
      }
      .orangellow.button2 {
         background-color:#FFB515;
      }
      .orangellow.button2:hover {
         background-color:#FC9200;
      }
      .white.button2 {
         background-color:#FFFFFF;
         color:#666666 !important;
         font-weight:normal;
         text-shadow:0 1px 1px #FFFFFF;
      }
      .white.button2:hover {
      background-color:#EEEEEE;
      }
      
      
      .secondary.button2 {
         background-color:#CCCCCC;
         color:#555555 !important;
         text-shadow:0 1px 1px rgba(255, 255, 255, 0.5);
      }
      .secondary.button2:hover {
         background-color:#BBBBBB;
         color:#444444 !important;
      }
      
      
      .super.button2 {
/*         -moz-border-radius: 15px;
         -webkit-border-radius: 15px;*/
         background-image:url("/Resources/img/super-button-overlay.png");
         border:1px solid rgba(0, 0, 0, 0.25);
         font-size:13px;
         padding:0;
      }
      .super.button2 span {
         /*-moz-border-radius:14px 14px 14px 14px;*/
         border-top:1px solid rgba(255, 255, 255, 0.2);
         display:block;
         line-height:1;
         padding:4px 20px 6px;
      }
      body.ff3 .super.button2 span {
         padding:5px 20px;
         position:relative;
         top:-1px;
      }
      .small.super.button2 {
         -moz-border-radius:12px 12px 12px 12px;
         font-size:11px;
      }
      .small.super.button2 span {
         -moz-border-radius:11px 11px 11px 11px;
         padding:2px 12px 6px;
      }
      .small.white.super.button2 span {
         padding:3px 12px 5px;
      }
      body.ff3 .small.super.button2 span {
         padding:3px 12px;
      }
      .large.super.button2 {
         -moz-border-radius:18px 18px 18px 18px;
         background-position:left bottom;
      }
      .large.super.button2 span {
         -moz-border-radius:17px 17px 17px 17px;
         font-size:14px;
         padding:7px 20px 9px;
      }
      body.ff3 .large.super.button2 span {
         padding:8px 20px;
      }

   </style> 
</asp:Content>

<asp:Content ContentPlaceHolderID="ScriptPlaceHolder" runat="server">
   <script type="text/javascript">
      $(function() {
         // Tabs
         $('#tabs').tabs(2);

//         // DateRange Picker
//         $('#rangeA').daterangepicker();

//         //demo 1
//         $('select#speed').selectToUISlider();

//         //demo 2
//         $('select#valueA, select#valueB').selectToUISlider();

//         //demo 3
//         $('select#valueAA, select#valueBB').selectToUISlider({
//            labels: 12
//         });

//         $("#finderDialog").dialog({
//            title: 'File Manager',
//            width: 872,
//            autoOpen: false,
//            modal: true,
//            overlay: {
//               opacity: "0.5",
//               background: "black"
//            },
//            buttons: {
//               'Conferma': function() { return true; },
//               'Annulla': function() { $('#finderDialog').dialog('close'); }
//            }
//         });

//         $("#dialog_link").click(function() {
//            $("#finderDialog").load('<%= Url.Action("Index", "FileManager", new { siteid = 0}) %>', function() {

//               $(this).dialog("open");

//               // Initialize the finder!
//               initFinder();

//            });
//         });

         // Bouncing buttons ("a la skype")
         $(".button-left-hover").hover(function(){
             $("img", this)
                .animate({top:"-14px"}, 200).animate({top:"-8px"}, 200) // first jump
                .animate({top:"-11px"}, 100).animate({top:"-8px"}, 100) // second jump
                .animate({top:"-10px"}, 100).animate({top:"-8px"}, 100); // the last jump
         });


//         $("div.tooltip").tooltip({
//            trigger: '#tooltiplink'
//         });

      });
   </script>
</asp:Content>

<asp:Content ContentPlaceHolderID="BreadCrumbsPlaceHolder" runat="server">
<%--   <%= Html.Breadcrumbs(b => b.AddHome("Home", Url.Action("Index", "Site", new {siteid = RequestContext.ManagedSite.SiteId}))
                              .AddCurrent("Test Page")) %>--%>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
   <div id="contentPageHeader">
      <h2>Test</h2>
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
   
   <div id="tabs">
      <ul>
         <li><a href="#tabs-1">Full Filament Demo</a></li>
         <li><a href="#tabs-2">Buttons</a></li>
         <li><a href="#tabs-3">File Manager</a></li>
         <li><a href="#tabs-4">FilamentGroup Extensions</a></li>
         <li><a href="#tabs-5">Encryption</a></li>
         <li><a href="#tabs-6">Exceptions</a></li>
         <li><a href="#tabs-7">Route Data</a></li>
         <li><a href="#tabs-8">Search Index Rebuild</a></li>
         <li><a href="#tabs-9">UI Form</a></li>
         <li><a href="#tabs-10">Pings</a></li>
         <li><a href="#tabs-11">Bulk Insert</a></li>
      </ul>
      
      <div id="tabs-1">
         
         <h2>Buttons CSS3</h2>
         <a href="#" class="medium orangellow button2">Anchor</a>
         &nbsp;&nbsp;&nbsp;
         <button class="medium orangellow button2" type="button">Button</button>
         &nbsp;&nbsp;&nbsp;
         &nbsp;&nbsp;&nbsp;
         <a href="#" class="medium secondary orangellow button2">secondary Anchor</a>
         &nbsp;&nbsp;&nbsp;
         <button class="medium secondary orangellow button2" type="button">secondary Button</button>
         <br />
         <br />
      
         <a href="#" class="medium orangellow button2"><span>Anchor with text in span</span></a>
         &nbsp;&nbsp;&nbsp;
         <button class="medium orangellow button2" type="button"><span>Button with text in span</span></button>
         &nbsp;&nbsp;&nbsp;
         &nbsp;&nbsp;&nbsp;
         <a href="#" class="medium secondary  button2"><span>secondary Anchor with text in span</span></a>
         &nbsp;&nbsp;&nbsp;
         <button class="medium secondary  button2" type="button"><span>secondary Button with text in span</span></button>
         <br />
      
      
      
         <h2>Basic button from different types of markup</h2>
         <a href="#" class="button ui-state-default ui-corner-all">Link</a>
         <button class="button ui-state-default ui-corner-all" type="button">Button element</button>
         <span href="#" class="button ui-state-default ui-corner-all">Span</span>

         <h2>Buttons with priority & disabled</h2>
         <button class="button ui-state-default ui-priority-primary ui-corner-all">Primary</button>
         <button class="button ui-state-default ui-priority-secondary ui-corner-all">Secondary</button>
         <a href="#" class="button-secondary">Secondary</a>

         <button class="button ui-state-default ui-state-disabled ui-corner-all">Disabled</button> 

         <h2>Toggle buttons</h2>	
         <button class="button ui-state-default button-toggleable ui-corner-all">Toggle</button>
         <button class="button ui-state-default button-toggleable ui-corner-all">Toggle</button>
         <button class="button ui-state-default button-toggleable ui-corner-all">Toggle</button> 
         				
         <h2>Icons in buttons</h2>
         <a href="#" class="button ui-state-default button-icon-left ui-corner-all"><span class="ui-icon ui-icon-circle-plus"></span>Previous</a>
         <a href="#" class="button ui-state-default button-icon-right ui-corner-all"><span class="ui-icon ui-icon-circle-plus"></span>Next</a>
         				
         <h2>Special Icons in buttons</h2>
         <a href="#" class="button ui-state-default button-icon-left-big ui-corner-all">
            <img src="/Resources/img/32x32/plugins.png" alt="">
            <span>Button</span>
         </a>


         <h2>Radio button style toggle buttons set</h2>
         <div class="buttonset buttonset-single">
	         <button class="button ui-state-default ui-priority-primary ui-corner-left">Visual</button>
	         <button class="button ui-state-default ui-priority-primary">Code</button>
	         <button class="button ui-state-default ui-priority-primary">Split</button>
	         <button class="button ui-state-default ui-priority-primary ui-corner-right">Preview</button>
         </div>

         <h2>Multiple select buttons set</h2>
         <div class="buttonset buttonset-multi">
	         <button class="button ui-state-default ui-corner-left"><b>B</b></button>
	         <button class="button ui-state-default"><i>I</i></button>
	         <button class="button ui-state-default"><u>U</u></button>
	         <button class="button ui-state-default ui-corner-right"><span class="strike">S</span></button>
         </div>

         <h2>Buttons in a Toolbar</h2>
         <div class="toolbar ui-widget ui-helper-clearfix">
	         <div class="buttonset ui-helper-clearfix">
		         <a href="#" class="button ui-state-default button-icon-solo ui-corner-all" title="Open"><span class="ui-icon ui-icon-folder-open"></span> Open</a>
		         <a href="#" class="button ui-state-default button-icon-solo  ui-corner-all" title="Save"><span class="ui-icon ui-icon-disk"></span> Save</a>
		         <a href="#" class="button ui-state-default button-icon-solo  ui-corner-all" title="Delete"><span class="ui-icon ui-icon-trash"></span> Delete</a>

	         </div>
	         <div class="buttonset buttonset-multi">
		         <button class="button ui-state-default ui-corner-left"><b>B</b></button>
		         <button class="button ui-state-default"><i>I</i></button>
		         <button class="button ui-state-default  ui-corner-right"><u>U</u></button>
	         </div>
	         <div class="buttonset ui-helper-clearfix">

		         <a href="#" class="button ui-state-default button-icon-solo  ui-corner-all" title="Print"><span class="ui-icon ui-icon-print"></span> Print</a>
		         <a href="#" class="button ui-state-default button-icon-solo  ui-corner-all" title="Email"><span class="ui-icon ui-icon-mail-closed"></span> Email</a>
	         </div>
	         <div class="buttonset buttonset-single ui-helper-clearfix">
		         <button class="button ui-state-default ui-state-active ui-priority-primary ui-corner-left">Edit</button>
		         <button class="button ui-state-default ui-priority-primary ui-corner-right">View</button>

	         </div>
         </div>
      </div>
      
      <div id="tabs-2">
         <br />
         
         <div class="ui-widget ui-helper-clearfix">
            <a class="coolbutton ui-widget-content ui-corner-all ui-helper-clearfix" href="#" >
               <div class="coolbutton-image">
                  <img src="/Resources/img/32x32/settings.png" />
               </div>
               <div class="coolbutton-text">
                  <span class="coolbutton-title">Link Title</span>
                  <span class="coolbutton-desc">Short decriptions</span>
               </div>
            </a>
            <a class="coolbutton ui-widget-content ui-corner-all ui-helper-clearfix" href="#" >
               <div class="coolbutton-image">
                  <img src="/Resources/img/32x32/archive-32.png" />
               </div>
               <div class="coolbutton-text">
                  <span class="coolbutton-title">Link Title</span>
                  <span class="coolbutton-desc">Short decriptions</span>
               </div>
            </a>
         </div>
      
      
         <br />
         <button class="button ui-state-default ui-corner-all" type="button">Button 1</button>
         
         <br />
         <br />
         <a href="#" class="button ui-state-default ui-corner-all">
            <span class="ui-icon-circle-triangle-e">Button Link</span>
         </a>
         
         <br />
         <br />
         <a href="#" class="button ui-state-default ui-corner-all" onclick="blockUserInterface();" >
            <span class="ui-icon-circle-triangle-e">Block UI</span>
         </a>
         <br />
         <br />
         
         <p>Here is an ImageLink</p>
         <%= Html.ImageLink("Index", "Site", "/Resources/img/32x32/bookmark.png", "bookmark") %>
         <br />
         <br />
         
         <p>Here is a jQueryUI button (Full with ui-icon-circle-plus)</p>
         <%= Html.ActionLinkUI("Test ActionLinkUI", "Index", "Site", "ui-icon-circle-plus", true, false, null, null)%>
         <br />
         <br />
         
         <p>Here is a jQueryUI button (simplest)</p>
         <%= Html.ActionLinkUI("Test ActionLinkUI", "Index", "Site", null, null)%>
         <br />
         <br />
         
         <p>Here is a jQueryUI button (icon on the right)</p>
         <%= Html.ActionLinkUI("Test ActionLinkUI", "Index", "Site", "ui-icon-circle-plus", false, false, null, null)%>
         <br />
         <br />
         
         <p>Here is a jQueryUI button (icon only without text)</p>
         <%= Html.ActionLinkUI("Test ActionLinkUI", "Index", "Site", "ui-icon-circle-plus", true, true, null, null)%>
         <br />
         <br />
         
         <p>Here is a jQueryUI button with bouncing hover image</p>
         <br />
         <a href="#" class="button button-left-hover ui-state-default ui-corner-all">
            <img src="/Resources/img/32x32/info.png" alt="info" />
            Info
         </a>
         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
         <a href="#" class="button button-left-hover ui-state-default ui-corner-all">
            <img src="/Resources/img/32x32/apply.png" alt="apply" />
            Apply
         </a>
         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
         <%= Html.ActionLinkUIWithOverIcon("Test ActionLinkUI", "/Resources/img/32x32/apply.png", "apply", "Index", "Site", null)%>
         <br />
         <br />
         <button class="button button-left-hover ui-state-default ui-corner-all">
            <img src="/Resources/img/32x32/apply.png" alt="apply" />
            Apply
         </button>
      </div>
      
      <div id="tabs-3">
         <br />
         <p>
            <a id="dialog_link" href="#" class="button ui-state-default button-icon-left ui-corner-all" >
               <span class="ui-icon ui-icon-newwin" ></span>
               Open Dialog
            </a>
         </p>         

         <div id="finderDialog">
            <%--<% Html.RenderPartialRequest("FileManagerControl"); %>--%>
         </div>
         
         <br />
      
      </div>
      
      <div id="tabs-4">
		   <br />
         <div class="ui-helper-clearfix"></div>
  
	      <input id="rangeA" type="text" value="Choose a Date" />	

		   <br />
		   <form action="#">
		      <br />
		      <br />
		      <!-- demo 1 -->
		      <fieldset>
			      <label for="speed">Select a Speed:</label>
			      <select name="speed" id="speed">
				      <option value="Slower">Slower</option>
				      <option value="Slow">Slow</option>
				      <option value="Med" selected="selected">Med</option>
				      <option value="Fast">Fast</option>
				      <option value="Faster">Faster</option>
			      </select>
		      </fieldset>
		
		      <!-- demo 2 -->
		      <fieldset>
			      <label for="valueA">From:</label>
			      <select name="valueA" id="valueA">
				      <option value="6am">6:00</option>
				      <option value="7am">7:00</option>
				      <option value="8am">8:00</option>
				      <option value="9am" selected="selected">9:00</option>
				      <option value="10am">10:00</option>
				      <option value="11am">11:00</option>
				      <option value="Noon">Noon</option>
				      <option value="1pm">1:00</option>
				      <option value="2pm">2:00</option>
				      <option value="3pm">3:00</option>
				      <option value="4pm">4:00</option>
				      <option value="5pm">5:00</option>
				      <option value="6pm">6:00</option>
				      <option value="7pm">7:00</option>
				      <option value="8pm">8:00</option>
				      <option value="9pm">9:00</option>
				      <option value="10pm">10:00</option>
				      <option value="11pm">11:00</option>
				      <option value="12pm">12:00</option>
			      </select>
      	
			      <label for="valueB">To:</label>
			      <select name="valueB" id="valueB">
				      <option value="6am">6:00</option>
				      <option value="7am">7:00</option>
				      <option value="8am">8:00</option>
				      <option value="9am">9:00</option>
				      <option value="10am">10:00</option>
				      <option value="11am">11:00</option>
				      <option value="Noon">Noon</option>
				      <option value="1pm">1:00</option>
				      <option value="2pm">2:00</option>
				      <option value="3pm">3:00</option>
				      <option value="4pm">4:00</option>
				      <option value="5pm">5:00</option>
				      <option value="6pm">6:00</option>
				      <option value="7pm">7:00</option>
				      <option value="8pm">8:00</option>
				      <option value="9pm" selected="selected">9:00</option>
				      <option value="10pm">10:00</option>
				      <option value="11pm">11:00</option>
				      <option value="12pm">12:00</option>
			      </select>
		      </fieldset>
		
		
		      <!-- demo 3 -->
   		   <fieldset>
			   <label for="valueAA">From:</label>
			   <select name="valueAA" id="valueAA">
				   <optgroup label="2003">
					   <option value="01/03">Jan 03</option>
					   <option value="02/03">Feb 03</option>
					   <option value="03/03">Mar 03</option>
					   <option value="03/03">Apr 03</option>
					   <option value="03/03">May 03</option>
					   <option value="06/03">Jun 03</option>
					   <option value="07/03">Jul 03</option>
					   <option value="08/03" selected="selected">Aug 03</option>
					   <option value="09/03">Sep 03</option>
					   <option value="10/03">Oct 03</option>
					   <option value="11/03">Nov 03</option>
					   <option value="12/03">Dec 03</option>
				   </optgroup>
				   <optgroup label="2004">
					   <option value="01/04">Jan 04</option>
					   <option value="02/04">Feb 04</option>
					   <option value="03/04">Mar 04</option>
					   <option value="04/04">Apr 04</option>
					   <option value="04/04">May 04</option>
					   <option value="06/04">Jun 04</option>
					   <option value="07/04">Jul 04</option>
					   <option value="08/04">Aug 04</option>
					   <option value="09/04">Sep 04</option>
					   <option value="10/04">Oct 04</option>
					   <option value="11/04">Nov 04</option>
					   <option value="12/04">Dec 04</option>
				   </optgroup>
				   <optgroup label="2005">
					   <option value="01/05">Jan 05</option>
					   <option value="02/05">Feb 05</option>
					   <option value="03/05">Mar 05</option>
					   <option value="04/05">Apr 05</option>
					   <option value="05/05">May 05</option>
					   <option value="06/05">Jun 05</option>
					   <option value="07/05">Jul 05</option>
					   <option value="08/05">Aug 05</option>
					   <option value="09/05">Sep 05</option>
					   <option value="10/05">Oct 05</option>
					   <option value="11/05">Nov 05</option>
					   <option value="12/05">Dec 05</option>
				   </optgroup>
				   <optgroup label="2006">
					   <option value="01/06">Jan 06</option>
					   <option value="02/06">Feb 06</option>
					   <option value="03/06">Mar 06</option>
					   <option value="04/06">Apr 06</option>
					   <option value="06/06">May 06</option>
					   <option value="06/06">Jun 06</option>
					   <option value="07/06">Jul 06</option>
					   <option value="08/06">Aug 06</option>
					   <option value="09/06">Sep 06</option>
					   <option value="10/06">Oct 06</option>
					   <option value="11/06">Nov 06</option>
					   <option value="12/06">Dec 06</option>
				   </optgroup>
				   <optgroup label="2007">
					   <option value="01/07">Jan 07</option>
					   <option value="02/07">Feb 07</option>
					   <option value="03/07">Mar 07</option>
					   <option value="04/07">Apr 07</option>
					   <option value="07/07">May 07</option>
					   <option value="07/07">Jun 07</option>
					   <option value="07/07">Jul 07</option>
					   <option value="08/07">Aug 07</option>
					   <option value="09/07">Sep 07</option>
					   <option value="10/07">Oct 07</option>
					   <option value="11/07">Nov 07</option>
					   <option value="12/07">Dec 07</option>
				   </optgroup>
				   <optgroup label="2008">
					   <option value="01/08">Jan 08</option>
					   <option value="02/08">Feb 08</option>
					   <option value="03/08">Mar 08</option>
					   <option value="04/08">Apr 08</option>
					   <option value="08/08">May 08</option>
					   <option value="08/08">Jun 08</option>
					   <option value="08/08">Jul 08</option>
					   <option value="08/08">Aug 08</option>
					   <option value="09/08">Sep 08</option>
					   <option value="10/08">Oct 08</option>
					   <option value="11/08">Nov 08</option>
					   <option value="12/08">Dec 08</option>
				   </optgroup>
			   </select>
   	
			   <label for="valueBB">To:</label>
			   <select name="valueBB" id="valueBB">
				   <optgroup label="2003">
					   <option value="01/03">Jan 03</option>
					   <option value="02/03">Feb 03</option>
					   <option value="03/03">Mar 03</option>
					   <option value="03/03">Apr 03</option>
					   <option value="03/03">May 03</option>
					   <option value="06/03">Jun 03</option>
					   <option value="07/03">Jul 03</option>
					   <option value="08/03">Aug 03</option>
					   <option value="09/03">Sep 03</option>
					   <option value="10/03">Oct 03</option>
					   <option value="11/03">Nov 03</option>
					   <option value="12/03">Dec 03</option>
				   </optgroup>
				   <optgroup label="2004">
					   <option value="01/04">Jan 04</option>
					   <option value="02/04">Feb 04</option>
					   <option value="03/04">Mar 04</option>
					   <option value="04/04">Apr 04</option>
					   <option value="04/04">May 04</option>
					   <option value="06/04">Jun 04</option>
					   <option value="07/04">Jul 04</option>
					   <option value="08/04">Aug 04</option>
					   <option value="09/04">Sep 04</option>
					   <option value="10/04">Oct 04</option>
					   <option value="11/04">Nov 04</option>
					   <option value="12/04">Dec 04</option>
				   </optgroup>
				   <optgroup label="2005">
					   <option value="01/05">Jan 05</option>
					   <option value="02/05">Feb 05</option>
					   <option value="03/05">Mar 05</option>
					   <option value="04/05">Apr 05</option>
					   <option value="05/05">May 05</option>
					   <option value="06/05">Jun 05</option>
					   <option value="07/05">Jul 05</option>
					   <option value="08/05">Aug 05</option>
					   <option value="09/05">Sep 05</option>
					   <option value="10/05">Oct 05</option>
					   <option value="11/05">Nov 05</option>
					   <option value="12/05">Dec 05</option>
				   </optgroup>
				   <optgroup label="2006">
					   <option value="01/06">Jan 06</option>
					   <option value="02/06">Feb 06</option>
					   <option value="03/06">Mar 06</option>
					   <option value="04/06">Apr 06</option>
					   <option value="06/06">May 06</option>
					   <option value="06/06">Jun 06</option>
					   <option value="07/06">Jul 06</option>
					   <option value="08/06">Aug 06</option>
					   <option value="09/06">Sep 06</option>
					   <option value="10/06">Oct 06</option>
					   <option value="11/06">Nov 06</option>
					   <option value="12/06">Dec 06</option>
				   </optgroup>
				   <optgroup label="2007">
					   <option value="01/07">Jan 07</option>
					   <option value="02/07">Feb 07</option>
					   <option value="03/07">Mar 07</option>
					   <option value="04/07">Apr 07</option>
					   <option value="07/07">May 07</option>
					   <option value="07/07">Jun 07</option>
					   <option value="07/07">Jul 07</option>
					   <option value="08/07" selected="selected">Aug 07</option>
					   <option value="09/07">Sep 07</option>
					   <option value="10/07">Oct 07</option>
					   <option value="11/07">Nov 07</option>
					   <option value="12/07">Dec 07</option>
				   </optgroup>
				   <optgroup label="2008">
					   <option value="01/08">Jan 08</option>
					   <option value="02/08">Feb 08</option>
					   <option value="03/08">Mar 08</option>
					   <option value="04/08">Apr 08</option>
					   <option value="08/08">May 08</option>
					   <option value="08/08">Jun 08</option>
					   <option value="08/08">Jul 08</option>
					   <option value="08/08">Aug 08</option>
					   <option value="09/08">Sep 08</option>
					   <option value="10/08">Oct 08</option>
					   <option value="11/08">Nov 08</option>
					   <option value="12/08">Dec 08</option>
				   </optgroup>
			   </select>
		   </fieldset>
		   </form>

      </div>
      
      <div id="tabs-5">
         <% using (Html.BeginForm("TestSHA2Encryption", "Test", new { siteid = ((IRequestContext)ViewData["Context"]).ManagedSite.SiteId }, FormMethod.Post, new { id = "encryptform" })){ %>
         <div class="form-item">
            <label for="plainText">Text to encrypt (SHA2):</label>
            <input type="text" id="plainText" name="plainText" maxlength="255" />
         </div>
         <div class="form-item">
            <label>&nbsp;</label>
	         <%= Html.SubmitUI(GlobalResource("Form_Save")) %>	
         </div>
         <% } %>
         
         <hr />
         
         <% if (ViewData["encryptedText"] != null){ %>
            <textarea id="encryptedText" name="encryptedText" rows="4" cols="40" >
               <%= ViewData["encryptedText"] %>
            </textarea>
         <% } %>
      </div>
      
      <div id="tabs-6">
            <% using (Html.BeginForm("TestException", "Test", new { siteid = ((IRequestContext)ViewData["Context"]).ManagedSite.SiteId }, FormMethod.Post, new { id = "testexceptionform" })){ %>
            <div class="form-item">
               <label for="plainText">Status Code:</label>
               <select id="statuscode" name="statuscode">
                  <option value="NOHTTP">Non Http Exception</option>
                  <option value="404">404 - Page Not Found</option>
                  <option value="503">503 - Url invalid or site down</option>
                  <option value="403">403 - Access forbidden</option>
                  <option value="500">500 - Unexpected error</option>
               </select>
            </div>
            <div class="form-item">
               <label>&nbsp;</label>
	            <%= Html.SubmitUI(GlobalResource("Form_Save")) %>	
            </div>
            <% } %>
      </div>

      <div id="tabs-7">
         <div class="content-padding">
            <%= ViewData["RouteData"] %>
            <br />
            <br />
	         <% using (Html.BeginForm("TestRouting", "Test", null, FormMethod.Post, new {id = "testroutingform"})) { %>
	            <div class="form-item">
	               <label>Action</label>
	               <%= Html.TextBox("actionName", "", new {@class = "mediumtext"}) %>
	            </div>
	            <div class="form-item">
	               <label>Controller</label>
	               <%= Html.TextBox("controllerName", "", new {@class = "mediumtext"}) %>
	            </div>
	            <div class="form-item">
	               <label>Area</label>
	               <%= Html.TextBox("areaName", "", new {@class = "mediumtext"}) %>
	            </div>
	            <div class="form-item">
	               <label>Id</label>
	               <%= Html.TextBox("idValue", "", new {@class = "mediumtext"}) %>
	            </div>
               <div id="adminpagefooter" class="ui-widget">
	               <%= Html.SubmitUI(GlobalResource("Form_Save")) %>	
               </div>
	         <% } %>
	         <br />
	         <fieldset>
	            <legend>Generated Url</legend>
	            <p><%= ViewData["GeneratedUrl"] %></p>
	         </fieldset>
	         
	         <br />
	         <br />
	         
         </div>
      </div>

      <div id="tabs-8">
         <div class="content-padding">
            <h2>Search Index Rebuild</h2>
            <br />
            
            <%= Html.ActionLinkUI("Rebuild Search Index", "RebuildIndex", "Test", "ui-icon-lightbulb", null, null)%>
            
            <br />
         </div>
      </div>

      <div id="tabs-9">
         <div class="content-padding width-400">
            <form action="#" class="ui-form-default">
               <div class="ui-form-item">
                  <label>Label 1</label>
                  <input type="text" class="required mediumtext" />
               </div>
               <div class="ui-form-item">
                  <label>Label 2</label>
                  <input type="text" class="required" />
               </div>
               <div class="form-item align-right">
                  <%= Html.SubmitUI("Update")%>
               </div>
            </form>
         </div>
      </div>
      
      <div id="tabs-10">
         <div class="content-padding width-400 ui-form-default">
            <div class="ui-form-item">
               <label>Response:</label>
               <%= Html.TextArea("response", ViewData["TestTrackback"] == null ? "" : ViewData["TestTrackback"].ToString() , 10, 40, new {@class = "largetext"}) %>
            </div>
            <div class="ui-form-item">
               <%= Html.ActionLinkUI("Send Trackback", "TestTrackback", "Test", "ui-icon-lightbulb", null, null)%>
            </div>
         </div>
      </div>
      
      <div id="tabs-11">
         <div class="content-padding width-400 ui-form-default">
            <% using (Html.BeginForm("DoPostBulkInsert", "Test", new { siteid = ((IRequestContext)ViewData["Context"]).ManagedSite.SiteId }, FormMethod.Post, new { id = "bulkinsertform" })){ %>
            <div class="ui-form-item">
               <label>Number of post to insert:</label>
               <input type="text" name="numberOfPost" class="shorttext" value="500" />
            </div>
            <div class="ui-form-item">
               <%= Html.SubmitUI("Builk Insert!") %>
            </div>
            <% } %>
         </div>
      </div>
      
      
      
   </div>

      
   
</asp:Content>
