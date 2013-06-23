tinyMCEPopup.requireLangPack();
var defaultSearchText;

function init() {
	tinyMCEPopup.resizeToInnerSize();

	pageLoading(false);
	
	// load data
	$.get(urlBrowse, function (data) {
	   if (data != '') {
	      $("#mediabrowser_panel").empty().append(data);

	      $("form#searchform").ajaxForm({
	         target: '#mediabrowser_panel'
	      });

         // live binding on pager clicks
	      $(".wp-pagenavi a").live("click", function(event){
            event.preventDefault();

            pageLoading(false);

            $.get($(this).attr("href"), function (data) {
               $("#mediabrowser_panel").empty().append(data);
               pageLoading(true);
            });
         });

	      pageLoading(true);
	   }
	});


   // Search box
	$("#searchtext").focus(function () {
	   $(this).addClass("ui-state-highlight");
	   defaultSearchText = $(this).attr("value");
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

}



function insertMedia(url) {
   var win = tinyMCEPopup.getWindowArg("window");

   var fullUrl = jQuery.url.attr("protocol") + '://' + jQuery.url.attr("host")
   if (jQuery.url.attr("port") != '')
      fullUrl = fullUrl + ':' + jQuery.url.attr("port")

   fullUrl = fullUrl + url;

   // insert information now
   win.document.getElementById(tinyMCEPopup.getWindowArg("input")).value = fullUrl;

   // are we an image browser
   if (typeof(win.ImageDialog) != "undefined") {
      // we are, so update image dimensions...
      if (win.ImageDialog.getImageData)
          win.ImageDialog.getImageData();

      // ... and preview if necessary
      if (win.ImageDialog.showPreviewImage)
          win.ImageDialog.showPreviewImage(fullUrl);
   }

   // close popup window
   tinyMCEPopup.close();
}

tinyMCEPopup.onInit.add(init);

