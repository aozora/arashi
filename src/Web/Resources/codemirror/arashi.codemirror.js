/// <summary>
/// arashi.codemirror
/// Initialize the code editor
/// </summary>

var editor;

$(function() {

   editor = CodeMirror.fromTextArea("Content", {
      width: '96%',
      height: '350px', 
      path: '/Resources/codemirror/js/',
      parserfile: ["parsexml.js", "parsecss.js", "tokenizejavascript.js", "parsejavascript.js", "parsehtmlmixed.js"], 
      stylesheet: ["/Resources/codemirror/css/xmlcolors.css", "/Resources/codemirror/css/jscolors.css", "/Resources/codemirror/css/csscolors.css"], 
      continuousScanning: 500, 
      lineNumbers: true,
      indentUnit: 3,
      tabMode: 'spaces',
      textWrapping: false
   });

   createToolbar();

   $("body").append("<div id='mediamanager-dialog' class='hidden'></div>");

   $.get(urlBrowse, function(data){
      $("#mediamanager-dialog").append(data);
   });
   

});



function createToolbar(){

   var $toolbar = $("<div id='post-content-toolbar' class='ui-state-default'></div>");
   var $media = $("<a id='addMediaLink' class='media' href='#'><span>Add Media...</span></a>");
   var $more = $("<a id='addMoreLink' class='more' href='#'><span>More</span></a>");

   $toolbar
      .append($media)
      .append($more);

   $("#post-content-container legend").after($toolbar);

   $("#post-content-toolbar a").hover(
      function() {
         $(this).addClass("ui-state-hover ui-corner-all");
      },
      function() {
         $(this).removeClass("ui-state-hover ui-corner-all");
      }
   );

   $("#addMediaLink").click(function(){
      showMediaBrowser();
   });
   $("#addMoreLink").click(function(){
      editor.replaceSelection("<!--more-->");
   });

}



/// <summary>
/// Show the Media Browser ui dialog
/// </summary>
function showMediaBrowser() {
   $("#mediamanager-dialog").dialog({
      title: 'Media Browser',
      width: 620,
      height: 480,
      resizable: false,
      modal: true,
      buttons: {
				      Cancel: function() {
					      $(this).dialog('close');
				      }
			      }
   });

}





/// <summary>
/// Callback function used when a media file is selected
/// This must insert a tag with the correct href in the code editor
/// </summary>
function insertMedia(url) {

   //editor.replaceSelection("<img src=\"/media/image.jpg\" alt=\"\" />");
   editor.replaceSelection("<img src=\"" + url + "\" alt=\"\" />");
   
   // close the dialog
   $("#mediamanager-dialog").dialog('close');
}