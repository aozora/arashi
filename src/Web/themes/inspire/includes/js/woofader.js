/* WooFader
--------
Author: Foxinni.com
Version: 1.0.0
---------
*/

(function(jQuery) {
	jQuery.fn.woofader = function(input) {
	
		var defaults = {
			
			featured: '#featured',
			container: '#container',
			pagination: '#breadcrumb .pagination',
			nav_right: '#breadcrumb .right',
			nav_left: '#breadcrumb .left',
			slides: '.slide',
			right: 'right',
			speed: 200,
			timeout: 5000,
			animate: false,
			resize: false
	
		};
		
		
		return this.each(function() {
		
			//Crucial Inits
			var holder  = jQuery(this);
    		var options = jQuery.extend(defaults, input);  
    		
 			//Object Setup
 			var featured = jQuery(options.featured,holder);
  			var pagination = jQuery(options.pagination,holder);
			var nav = jQuery(options.nav,holder);
			var nav_right = jQuery(options.nav_right,holder);
			var nav_left = jQuery(options.nav_left,holder);
			var slides = jQuery(options.slides,holder);
			
   			//Animation Variables
   			var right = options.right;
   			var speed = options.speed; 
  			var timeout = options.timeout; 
			var animate = options.animate;
			var resize = options.resize; 
   			
    		//Working Variables
			var slideCount = slides.length;
			var count = 0;
			var nextItem;
			var nextItemHeight = featured.find('.slide:eq(0)').height();
			timeout = parseInt(timeout);
			
			//Height Setup
			if(resize){featured.css('height',nextItemHeight);}
				
				if(timeout > 0){
				
				function autoAnimate(){
					count++;
					slides.hide();
					if(count >= slideCount) { count = 0;}
					
					if(count == 0){
						//Slides 
						nextItem = featured.find('.slide:eq('+count+')');
						nextItem.fadeIn(speed);
						nextItemHeight = nextItem.height();
						if(animate){featured.animate({height:nextItemHeight});}
						else if(resize){featured.css('height',nextItemHeight);}
						//Pagination
						pagination.find('li').removeClass('active').end().find('li:first-child').addClass('active');
					} else {
						//Slides
						nextItem = featured.find('.slide:eq('+count+')');
						nextItem.fadeIn(speed);
						nextItemHeight = nextItem.height();
						if(animate){featured.animate({height:nextItemHeight});}
						else if(resize){featured.css('height',nextItemHeight);}
						//Pagination
						pagination.find('li').removeClass('active').end().find('li:eq('+count+')').addClass('active');
					}
					
				};
				doAutoAnimate = setInterval(autoAnimate,timeout);
				holder.click(function(){ clearInterval(doAutoAnimate); holder.addClass('stopped') }); // Clear Timeout
			}
			
			
			nav_right.add(nav_left).click(function(){
				
				slides.hide();
				if(timeout > 0){ clearInterval(doAutoAnimate); holder.addClass('stopped') };
				if(jQuery(this).hasClass('right')){
					count++;
					if(count >= slideCount) { count = 0; }
					var action = 'right';
				} else { 
					count--;
					if(count < 0) { count = (slideCount - 1);}
					var action = 'left'
				}
				if(count == 0){ 
					//Slides
					nextItem = featured.find('.slide:eq('+count+')');
					nextItem.fadeIn(speed);
					nextItemHeight = nextItem.height();
					if(animate){featured.stop().animate({height:nextItemHeight});}
					else if(resize){featured.css('height',nextItemHeight);}
					//Pagination
					pagination.find('li').removeClass('active').end().find('li:first-child').addClass('active');
				} else if(action == 'left'){
					//Slides
					nextItem = featured.find('.slide:eq('+count+')');
					nextItem.fadeIn(speed);
					nextItemHeight = nextItem.height();
					if(animate){featured.stop().animate({height:nextItemHeight});}
					else if(resize){featured.css('height',nextItemHeight);}
					//Pagination
					pagination.find('li').removeClass('active').end().find('li:eq('+count+')').addClass('active');
				} else {
					//Slides
					nextItem = featured.find('.slide:eq('+count+')');
					nextItem.fadeIn(speed);
					nextItemHeight = nextItem.height();
					if(animate){featured.stop().animate({height:nextItemHeight});}
					else if(resize){featured.css('height',nextItemHeight);}
					//Pagination
					pagination.find('li').removeClass('active').end().find('li:eq('+count+')').addClass('active');					
				}
				return false;
				
			});
					
			pagination.find('li').click(function(){
				//Slides
				slides.hide();
				var index = jQuery(this).index();
				if(timeout > 0){ clearInterval(doAutoAnimate); holder.addClass('stopped'); } // Clear Timeout
				nextItem = featured.find('.slide:eq('+index+')');
				nextItem.fadeIn(speed);
				nextItemHeight = nextItem.height();
				if(animate){featured.stop().animate({height:nextItemHeight});}
				else if(resize){featured.css('height',nextItemHeight);}
				//Pagination
				pagination.find('li').removeClass('active').end().find('li:eq('+index+')').addClass('active');

				
				count = index;
				return false;
			});
			
			holder.hover(
				function(){
						clearInterval(doAutoAnimate);
					},
				function(){
						if(holder.hasClass('stopped')){
							//Do not re-activate slider
						} else {
							if(timeout > 0){
								doAutoAnimate = setInterval(autoAnimate,timeout);
							}
						}
					}
					
			);
			
			

		});
	};
    
})(jQuery);