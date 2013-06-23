/// <summary>
/// admin.tutorial
/// Interactive Tutorial for the Control Panel
/// </summary>

var tutorialHtmlLoaded = false;

/// <summary>
/// Start the interactive tutorial
/// </summary>
function runTutorial() {

   // Close previous dialog
   $.unblockUI();
   
   if (!tutorialHtmlLoaded) {
      // load the external html with the tutorial texts   
      $.get("/Resources/help/tutorial.en-US.htm", function(data){
         $("body").append(data);
         
         tutorialHtmlLoaded = true;
         
         // init the click events of the buttons
         tutorialInit();
         
         // show the step 1
         tutorialStep1();
      });
   } else {
      // show the step 1
      tutorialStep1();
   }
   
}


/// <summary>
/// init the event bindings
/// </summary>
function tutorialInit() {
   
   // use blockUI with a completely transparent overlay
   $.blockUI({ 
      message:  null, 
      theme:     false, 
      overlayCSS: { backgroundColor: 'transparent' },
      draggable: false, 
      timeout:  0 
   }); 


   $("a.close-tutorial").live("click", function(){
      endTutorial();
      return false;
   });


   $("#tut1-next").live("click", function(){
      tutorialStep2();
      return false;
   });
   $("#tut2-next").live("click", function(){
      tutorialStep3();
      return false;
   });
   $("#tut3-next").live("click", function(){
      tutorialStep4();
      return false;
   });
   $("#tut4-next").live("click", function(){
      tutorialStep5();
      return false;
   });
   // Finish
   $("#endTutorial").live("click", function(){
      endTutorial();
      return false;
   });
}



/// <summary>
/// custom growl
/// </summary>
// position: top-left, top-right, bottom-left, bottom-right, center
function tutGrowl(txt, header, position, open) {
   $.jGrowl(txt, { 
      sticky: true, 
      header: header,
      position: position,
      theme: 'ui-state-highlight',
      closer: false,
		closeTemplate: '',
		closerTemplate: '',
      life: 7000,
      open: open
   });
}



/// <summary>
/// Tutorial Step 1
/// </summary>
function tutorialStep1() {
   
   tutGrowl( $("#tut-1").html(), $("#tut-1").attr("title"), 'center', function(e,m,o) {
	   $("#site-switcher a").effect('pulsate', {}, 500, function(){
	      //$("#site-switcher a").trigger("click");
         showSiteSwitcherMenu();
	   });
   });
  		   
}


/// <summary>
/// Tutorial Step 2
/// </summary>
function tutorialStep2() {

   // close the site switcher previously opened
   //$("#site-switcher a").click();
   showSiteSwitcherMenu();
   
   // close the previous growl
   $('.jGrowl-notification ').trigger("jGrowl.close");
   
   tutGrowl( $("#tut-2").html(), $("#tut-2").attr("title"), 'center', function(e,m,o) {
	   $('#header div.login-status').effect('pulsate', {}, 500);
   });
}



/// <summary>
/// Tutorial Step 3
/// </summary>
function tutorialStep3() {

   // close the previous growl
   $('.jGrowl-notification ').trigger("jGrowl.close");

   tutGrowl( $("#tut-3").html(), $("#tut-3").attr("title"), 'top-right', function(e,m,o) {
	   $("#stats").effect('pulsate', {}, 500);
   });
}



/// <summary>
/// Tutorial Step 4
/// </summary>
function tutorialStep4() {

   // close the previous growl
   $('.jGrowl-notification ').trigger("jGrowl.close");

   tutGrowl( $("#tut-4").html(), $("#tut-4").attr("title"), 'center', function(e,m,o) {
	   $("fieldset.controlpanelgroup").effect('pulsate', {}, 500);
   });
}



/// <summary>
/// Tutorial Step 5
/// </summary>
function tutorialStep5() {

   // close the previous growl
   $('.jGrowl-notification ').trigger("jGrowl.close");

   tutGrowl( $("#tut-5").html(), $("#tut-5").attr("title"), 'center', function(e,m,o) {
      // move the fake breadcrumb
      //$("#site-switcher").after($("#breadcrumb"));
      $("#site-switcher").after($("#sitemenu li.home"));
      
	   $("#breadcrumb li.home").effect('pulsate', {}, 500, function() {
         $("#sitemegamenu").show();  
         $("#sitemegamenu").css("width", "500px"); 
	   	$("#sitemegamenu").effect('pulsate', {}, 500);
	   });
   });
}



function endTutorial() {
      // close the previous growl
      $('.jGrowl-notification ').trigger("jGrowl.close");

      // hide the fake breadcrumbs
      $("#breadcrumb li.home").hide();
      
      // unblock the ui
      $.unblockUI();
}