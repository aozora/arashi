// PrettyPhoto (lightbox)
jQuery(document).ready(function($){
	$("a[rel^='prettyPhoto']").prettyPhoto();
});

// Portfolio thumbnail hover effect
jQuery(document).ready(function () {
	jQuery('#portfolio .block img').mouseover(function() {
		jQuery(this).stop().fadeTo(300, 0.5);
	});
	jQuery('#portfolio .block img').mouseout(function() {
		jQuery(this).stop().fadeTo(400, 1.0);
	});
});

// Portfolio tag sorting
jQuery(document).ready(function(){
								
	jQuery('.port-cat a').click(function(evt){
		var clicked_cat = jQuery(this).attr('rel');
		if(clicked_cat == 'all'){
			jQuery('#portfolio .post').hide().fadeIn(200);
		} else {
			jQuery('#portfolio .post').hide()
			jQuery('#portfolio .' + clicked_cat).fadeIn(400);
		 }
		//eq_heights();
		evt.preventDefault();
	})	

	// Thanks @johnturner, I owe you a beer!
	var postMaxHeight = 0;
	jQuery("#portfolio .post").each(function (i) {
		 var elHeight = jQuery(this).height();
		 if(parseInt(elHeight) > postMaxHeight){
			 postMaxHeight = parseInt(elHeight);
		 }
	});
	jQuery("#portfolio .post").each(function (i) {
		jQuery(this).css('height',postMaxHeight+'px');
	});
														
});
