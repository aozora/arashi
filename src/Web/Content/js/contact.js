// requirements: jQuery Form Plugin
// ---------------------------------
$(document).ready(function () {

   contactClearResponseOutput();
	
   var url = jQuery('div.wpcf7 > form').attr('action');
   if (url.substr(url.length - 1, 1) == "/")
      url = url.substr(0, url.length - 1) + "ajax/";
   else
      url = url + "ajax/";


   try {
		jQuery('div.wpcf7 > form').ajaxForm({
         url: url,
         type: 'post',
			beforeSubmit: contactBeforeSubmit,
			dataType: 'json',
			success: contactProcessResponse
		});
	} catch (e) {
      if (window.console && window.console.error) 
         console.error(e);
	}

});



function contactClearResponseOutput() {
	jQuery('div.wpcf7-response-output')
      .hide()
      .empty()
      .removeClass('wpcf7-mail-sent-ok')
      .removeClass('wpcf7-mail-sent-ng')
      .removeClass('wpcf7-validation-errors')
      .removeClass('wpcf7-spam-blocked');
	jQuery('span.wpcf7-not-valid-tip').remove();
	jQuery('img.ajax-loader').css({ visibility: 'hidden' });
}



function contactBeforeSubmit(formData, jqForm, options) {
	contactClearResponseOutput();
	jQuery('img.ajax-loader', jqForm[0]).css({ visibility: 'visible' });

	return true;
}



function contactProcessResponse(data) {
	var wpcf7ResponseOutput = jQuery('div.wpcf7-response-output');
	contactClearResponseOutput();

   if ( $("#recaptcha_widget_div").is(':visible') )
      Recaptcha.reload();

   // setup css for errors
	if (!data.isvalid) {

//		jQuery.each(data.invalids, function(i, n) {
//			wpcf7NotValidTip(jQuery(data.into).find(n.into), n.message);
//		});
		wpcf7ResponseOutput.addClass('wpcf7-validation-errors');
	}

	if (data.message_sent) {
		jQuery('div.wpcf7 > form').resetForm().clearForm();
		wpcf7ResponseOutput.addClass('wpcf7-mail-sent-ok');
	} else {
		wpcf7ResponseOutput.addClass('wpcf7-mail-sent-ng');
	}

   // write the message
	wpcf7ResponseOutput.append(data.message).slideDown('fast');
}


