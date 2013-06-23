/// <summary>
/// arashi.tinymce
/// Initialize the html editor
/// </summary>
$(function() {
   $('textarea.htmleditor').tinymce({
      //script_url : '/Resources/tiny_mce34/tiny_mce.js',
      script_url: '/Resources/tiny_mce34/tiny_mce_gzip.ashx', // Use Compressor
      // General options
      theme : 'advanced',
      skin : 'arashi',
      //skin : 'thebigreason',
      language: __cultureInfo["name"].substr(0, 2),
      width: '98%',
      height: '260',
      plugins : 'arashilink,pagebreak,style,layer,advhr,advlist,autolink,lists,advimage,emotions,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,template,inlinepopups',

      accessibility_warnings: false,
      document_base_url: '/',
      relative_urls: false,
      extended_valid_elements: "object[classid|codebase|width|height|align|type|data],param[id|name|type|value|valuetype<DATA?OBJECT?REF]",
      convert_newlines_to_brs : false,

      // Custom File Browser
      file_browser_callback : 'arashiFileBrowser',
            
      // Theme options
      theme_advanced_buttons1: 'bold,italic,underline,strikethrough,|,bullist,numist,blockquote,|,outdent,indent,|,justifyleft,justifycenter,justifyright,justifyfull,|,formatselect,fontsizeselect,|,pasteword,|,arashilink,|,anchor,image,media,charmap,emotions,|,code,',
      theme_advanced_buttons2 : '',
      theme_advanced_buttons3 : '',
      theme_advanced_buttons4 : '',
      theme_advanced_toolbar_location : 'top',
      theme_advanced_toolbar_align : 'left',
      theme_advanced_statusbar_location : 'bottom',
      theme_advanced_resizing : false,

   });
});



function arashiFileBrowser (field_name, url, type, win) {

   // Firebug debugging...
   if (window.console && window.console.error) {
      console.log("Field_Name: " + field_name + "\nURL: " + url + "\nType: " + type + "\nWin: " + win + "\nCurrent Url: " + window.location.toString());
   }

   /* If you work with sessions in PHP and your client doesn't accept cookies you might need to carry
      the session name and session ID in the request string (can look like this: "?PHPSESSID=88p0n70s9dsknra96qhuk6etm5").
      These lines of code extract the necessary parameters and add them back to the filebrowser URL again. */

   //var cmsURL = window.location.toString();    // script URL - use an absolute path!
   var cmsURL = jQuery.url.attr("protocol") + '://' + jQuery.url.attr("host")
   
   if (jQuery.url.attr("port") != '')
      cmsURL = cmsURL + ':' + jQuery.url.attr("port")
      
   cmsURL = cmsURL + '/resources/tiny_mce34/plugins/arashi/fm.htm?urlget=' + urlBrowse;
   
   if (window.console && window.console.error) 
      console.log(cmsURL);


   
   if (cmsURL.indexOf("?") < 0) {
      //add the type as the only query parameter
      cmsURL = cmsURL + "?type=" + type;
   }
   else {
      //add the type as an additional query parameter
      // (PHP session ID is now included if there is one at all)
      cmsURL = cmsURL + "&type=" + type;
   }

   tinyMCE.activeEditor.windowManager.open({
      file : cmsURL,
      title : 'Media Browser',
      width : 620,  
      height : 480,
      resizable : "no",
      inline : "yes",  // This parameter only has an effect if you use the inlinepopups plugin!
      close_previous : "no"
   }, {
      window : win,
      input : field_name
   });
   
   return false;
}



function toggleEditor(id) {
   if (!tinyMCE.get(id))
      tinyMCE.execCommand('mceAddControl', false, id);
   else
      tinyMCE.execCommand('mceRemoveControl', false, id);
}