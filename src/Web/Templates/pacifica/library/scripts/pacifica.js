$(document).ready(function(){
						   
//	// Dropdown Script	   
//	function mainmenu(){
//	$(" #nav ul ").css({display: "none"}); // Opera Fix
//	$(" #nav li").hover(function(){
//			$(this).find('ul:first').css({visibility: "visible",display: "none"}).show(400);
//			},function(){
//			$(this).find('ul:first').css({visibility: "hidden"});
//			});
//	}
//	
//	 $(document).ready(function(){					
//		mainmenu();
//	});
 
//        // Activate PrettyPhoto Lightbox handle
//		$("a[rel^='gallery']").prettyPhoto({
//				animationSpeed: 'normal', /* fast/slow/normal */
//				padding: 30, /* padding for each side of the picture */
//				opacity: 0.85, /* Value betwee 0 and 1 */
//				showTitle: false, /* true/false */
//				allowresize: true, /* true/false */
//				counter_separator_label: '/', /* The separator for the gallery counter 1 "of" 2 */
//				theme: 'light_rounded', /* light_rounded / dark_rounded / light_square / dark_square */
//				hideflash: false, /* Hides all the flash object on a page, set to TRUE if flash appears over prettyPhoto */
//				modal: false, /* If set to true, only the close button will close the window */
//				changepicturecallback: function(){}, /* Called everytime an item is shown/changed */
//				callback: function(){} /* Called when prettyPhoto is closed */
//			});

	
	// PNG Fix
	$(document).pngFix(); 	
//	
//	// Activate jCarousel
//	function mycarousel_initCallback(carousel)
//	{
//    	// Disable autoscrolling if the user clicks the prev or next button.
//    	carousel.buttonNext.bind('click', function() {
//    	    carousel.startAuto(0);
//    	});
//		
//    	carousel.buttonPrev.bind('click', function() {
//    	    carousel.startAuto(0);
//    	});
//		
//    	// Pause autoscrolling if the user moves with the cursor over the clip.
//    	carousel.clip.hover(function() {
//    	    carousel.stopAuto();
//    	}, function() {
//    	    carousel.startAuto();
//    	});
//	};


//    	jQuery('#mycarousel').jcarousel({
//    	    auto: 8,
//    	    wrap: 'last',
//    	    animation: 'slow',
//    	    scroll: 2,
//    	    initCallback: mycarousel_initCallback
//    });

});