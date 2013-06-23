/// <summary>
/// admin.mediamanager.js
/// Media Manager
/// </summary>

$(function () {

   $("#uploader").plupload({
      // General settings
      runtimes: 'html5, flash',
      url: uploadHandlerName,
      max_file_size: uploadMaxFileSizeKB + 'kb',
      chunk_size: '1mb',
      unique_names: false,
      multipart: true,

      // Resize images on clientside if we can
      //resize: { width: 320, height: 240, quality: 90 },

      // Specify what files to browse for
      filters: [
			{ title: "Files", extensions: "jpg,gif,png,doc,docx,ppt,pptx,pps,xls,xlsx,pdf,htm,html,swf,zip,7z,rar,avi,mpg,wmv" }
		],

      // Flash settings
      flash_swf_url: '/Resources/flash/plupload.flash.swf',

      // Silverlight settings
//      silverlight_xap_url: '/ClientBin/plupload.silverlight.xap',

      // Post init events, bound after the internal events
      init: {
         StateChanged: function (up) {
            // Called when the state of the queue is changed
            if (up.state == plupload.DONE)
               location.reload(true);
         },
         Error: function (up, args) {
            // Called when a error has occured
            var msg;

            switch (args.code) {
               case plupload.FILE_EXTENSION_ERROR:
                  msg = 'FILE_EXTENSION_ERROR';
                  break;

               case plupload.FILE_SIZE_ERROR:
                  msg = 'FILE_EXTENSION_ERROR';
                  break;
               default:
                  msg = 'GENERIC ERROR';
            }

            growl(msg, false, true);
         }
      }

   });

});

