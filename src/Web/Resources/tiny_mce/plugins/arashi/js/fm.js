tinyMCEPopup.requireLangPack();

//var oldWidth, oldHeight, ed, url;

function init() {
	tinyMCEPopup.resizeToInnerSize();
	
	
	// load data
   $.get(urlBrowse, function(data){
      if (data != ''){
         $("#mediabrowser_panel").empty().append(data);
      }
   });
	

}



function insertMedia(url) {
   //var URL = document.my_form.my_field.value;
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
