/// <summary>
/// admin.mediamanager.edit.js
/// Media Edit
/// </summary>

var originalW, originalY; // these are the original width/height of the image
var ratio;

$(function () {

   // calculate the image aspect ratio
   originalW = $('#resizewidth').val();
   originalY = $('#resizeheight').val();

   ratio = originalW / originalY;

   $("#tabs").tabs();

   $("#actions-panel").accordion({
      collapsible: true,
      autoHeight: false,
      change: function (event, ui) {
         var $header = $(ui.newHeader);

         if ($header.has("#crop-header")) {

         }

      }
   }).accordion("activate", false);


   // client validation
   $("#actions-panel form").each(function(){
      $(this).validate({
         errorClass: 'input-validation-error',
         submitHandler: function (form) {
            $(form).ajaxSubmit({
               target: '#edited',
               beforeSubmit: beforeMediaSubmit,  // pre-submit callback 
               success: successeMediaSubmit  // post-submit callback
            });
         }
      });
   
   });


   // enable Jcrop
   $("#original").Jcrop({
      onChange: cropShowCoords,
      onSelect: cropShowCoords,
      bgColor: 'transparent'
   });

});


// common functions
function beforeMediaSubmit() {
   pageLoading(false);
   return true;
}
function successeMediaSubmit() {
   
   // update the history
   var $h = $("<input type='hidden' name='history' />");
   $h.val($("#media").attr("src"));

   $("#history").append($h);

   // remove the page loader
   pageLoading(true);
}


function cropShowCoords(c) {
   $('#croptop').val(c.x);
   $('#cropleft').val(c.y);
   $('#cropwidth').val(c.w);
   $('#cropheight').val(c.h);

   $('#crop-sel-left').text(c.x);
   $('#crop-sel-top').text(c.y);
   $('#crop-sel-width').text(c.w);
   $('#crop-sel-height').text(c.h);
}


function scaleChanged(x) {
   var w = $('#resizewidth');
   var h = $('#resizeheight');
   var w1 = '', h1 = '';

	if ( x ) {
		h1 = (w.val() != '') ? intval( w.val() / ratio ) : '';
		h.val( h1 );
	} else {
		w1 = (h.val() != '') ? intval( h.val() * ratio ) : '';
		w.val( w1 );
	}

}

function intval(f) {
   return f | 0;
}