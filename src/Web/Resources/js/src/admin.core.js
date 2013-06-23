/// <summary>
/// admin.core
/// Core functions for the Control Panel
/// </summary>

// Global fields
var defaultSearchText; 

/// <summary>
/// Run First!
/// </summary>

// Blocca subito la UI durante il caricamento...
// blockUserInterface();
// ---------------------------------------------------------------------------
 
 
/// <summary>
/// On Document Ready
/// </summary>
$(document).ready(function () {

   // Hover of nav menu
   $("#menu > li").not(".nohover")
	   .hover(
		   function () {
		      $(this).addClass("ui-state-active");
		      if ($("a", $(this)).hasClass("drop"))
		         $(this).addClass("ui-corner-top");
		      else
		         $(this).addClass("ui-corner-all");
		   },
		   function () {
		      $(this).removeClass("ui-state-active");
		      if ($("a", $(this)).hasClass("drop"))
		         $(this).removeClass("ui-corner-top");
		      else
		         $(this).removeClass("ui-corner-all");
		   }
	   );


   // Form elements hover
   $(".ui-form-default ol li input, .ui-form-default ol li select, .ui-form-default ol li textarea")
	   .hover(
		   function () {
		      $(this).addClass("ui-form-hover");
		   },
		   function () {
		      $(this).removeClass("ui-form-hover");
		   }
	   );

   // setup button widget
   $(".button, .call-to-action, .coolbutton").button();



   // Search box
   $("#searchtext").focus(function () {
      $(this).addClass("ui-state-highlight");
      defaultSearchText = $(this).attr("value");
      //if ($(this).attr("value") == searchBox2Default)
         $(this).attr("value", "");
   });
   $("#searchtext").blur(function () {
      $(this).removeClass("ui-state-highlight");
      if ($(this).attr("value") == "")
         $(this).attr("value", defaultSearchText);
   });
   $("#search-submit")
   	.hover(
		   function () {
		      $(this).addClass("ui-state-hover");
		   },
		   function () {
		      $(this).removeClass("ui-state-hover");
		   }
	   );



   // hover on the tables row
   $("table.grid tbody tr:not(.emptyrow, #reply-container)")
      .hover(
		   function () {
		      $(this).addClass("ui-state-highlight ui-state-hover-row");
		   },
		   function () {
		      $(this).removeClass("ui-state-highlight ui-state-hover-row");
		   }
      );


   // Site MegaMenu under breadcrumbs
   if ($("#breadcrumb").is(':visible')) {
      // setup hoverIntent for Site Mega DropDown Menu
      var config = {
         sensitivity: 2, // number = sensitivity threshold (must be 1 or higher)
         interval: 100, // number = milliseconds for onMouseOver polling interval
         over: megaHoverOver, // function = onMouseOver callback (REQUIRED)
         timeout: 500, // number = milliseconds delay before onMouseOut
         out: megaHoverOut // function = onMouseOut callback (REQUIRED)
      };

      $("#breadcrumb li.home").hoverIntent(config);
   }

   // block when ajax activity starts & unblock when stop
   //   $().ajaxStart(function() {
   //      blockUserInterface();
   //   })
   //      .ajaxStop(function() {
   //         $.unblockUI();
   //      });

   // Render messages with Growl
   if ($("#message-container").contents("div").length > 0) {
      showMessage('', false);
   }


   // ajax error common handler
   $(document).ajaxError(function (request, settings) {
      $.unblockUI();

      // Log su Firebug se presente
      if (window.console && window.console.error) {
         console.error(request);
         console.error(settings);
         console.trace();

         // check if the error was caused by a HTTP 403
         if (settings != null && settings.status == 403)
            alert("Sorry, you are not authorized to perform this operation.");
         else
            alert(request);
      }
      else {
         if (settings != null && settings.status == 403)
            alert("Sorry, you are not authorized to perform this operation.");
         else
            alert("Ooops, sorry but an unexptected error occured.\nRetry, but if the problem persist contact us!");
      }
   });


   // init jGrowl
   closer: false;

   // async load of the gravatar image
   //   loadCurrentUserGravatar();

   // Closing Loading-mask
   //   $.unblockUI();
});



function startTutorial() {
   // dinamically load the tutorial script
   $.getScript('/Resources/js/src/admin.tutorial.js', function() {
      runTutorial();
   });      

   return false;
}



/// <summary>
/// Display a loading message (from jquery mobile)
/// </summary>
function pageLoading(done, message) {
   var $html = $("html");

   if (done) {
      $html.removeClass("ui-loading");
   } 
   else 
   {
      if (message != '') 
      {
         var $loader = $("#ui-loader");
         $loader.css({ top: $(window).scrollTop() + 75 });
         $("h1", $loader).text(message);
      }
      $html.addClass("ui-loading");
   }
};




function getBrowserLanguage() {
   return normaliseLang(navigator.language /* Mozilla */ ||	navigator.userLanguage /* IE */);
}

/// <summary>
/// Ensure language code is in the format aa-AA
/// </summary>
function normaliseLang(lang) {
	lang = lang.replace(/_/, '-').toLowerCase();
	if (lang.length > 3) {
		lang = lang.substring(0, 3) + lang.substring(3).toUpperCase();
	}
	return lang;
}



/// <summary>
/// Show a growl message. 
/// The message html passed as argument will be put in the #message-container element
/// </summary>
function showMessage(msg, sticky) {

   if (!_isNull(sticky))
      sticky = true;

   if (!_isNull(msg))
      $("#message-container").empty().append(msg);

   var iserror = $("#message-container div.message div").hasClass("ui-state-error") ? true : false;
   growl( $("#message-container .message").html(), iserror, sticky );
}



function growl(msg, iserror, sticky) {

   var stateClass = iserror ? "ui-state-error" : "ui-state-highlight";
   
   if (iserror)
      sticky = true;
   
   $.jGrowl(msg, { 
      sticky: sticky,
      theme: "ui-widget " + stateClass + " ui-shadow",
      life: 7000,
      closeTemplate: "<img src='/Resources/img/16x16/cross.png' alt='' />Close"
   });
}





//On Hover Over
function megaHoverOver(){
   var megamenuIndex = "#sitemegamenu";

   var rowWidth = 0;
   //Calculate width based on the contained ul width
   $(megamenuIndex).find("ul").each(function () {
      rowWidth += parseInt($(this).css('width'));
   });

   // set position
   $(megamenuIndex).css({ 'left': $(this).offset().left - (rowWidth / 2) });
   
   $(megamenuIndex).css({'width': (rowWidth + 42)});
   $(megamenuIndex).stop().fadeTo('fast', 1).show(); //Find sub and fade it in
   $(this).addClass("ui-state-default ui-corner-top");
}


//On Hover Out
function megaHoverOut() {
   //$(this).removeClass("ui-state-hover");
   $(this).removeClass("ui-state-default ui-corner-top");

   var megamenuIndex = "#sitemegamenu";
   
   if (megamenuIndex == '#')
      return false;
      
   $(megamenuIndex).stop().fadeTo('fast', 0, function() { //Fade to 0 opactiy
      $(this).hide();  //after fading, hide it
   });
}



/// <summary>
/// IsNull
/// </summary>
function _isNull(i) {
   return (i == null || i == 'null' || i == '' || i == 'undefined');
}



/// <summary>
/// Load in background the Gravatar image
/// </summary>
function loadCurrentUserGravatar() {

   var img = new Image();
   
   // use the 'load' event to run code after the image has been loaded
   $(img).load( function() {
      // hide first
      $(this).css('display','none'); // since .hide() failed in safari
      $("#current-user-gravatar").after(this);
      $("#current-user-gravatar").fadeOut(function(){
         $(img).fadeIn();
      });
   })
   .error(function(xhr, textStatus, errorThrown) {
	   $(this).attr("src", "/Resources/img/gravatar-default-32x32.png");
   })
   .attr('src', $("#user-gravatar-url").val()); // this will load the image
    
}

