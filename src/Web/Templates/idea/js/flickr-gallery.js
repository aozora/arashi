$(document).ready(function () {
   $('#cbox a').colorbox();

   $("#cbox li a").append("<span class='portfolio_zoom'><img alt='' src='/Templates/idea/images/zoom.png'></span>");

   $("#cbox li").hover(
      function() {
         $(".portfolio_zoom", this).fadeIn('500');
	   }, 
      function() {
		   $(".portfolio_zoom", this).fadeOut('500');
	   }
   );

   // url called: http://api.flickr.com/services/feeds/photos_public.gne?id=50061359@N03&lang=en-us&format=json

//   $('#cbox').jflickrfeed({
//      limit: 20,
//      cleanDescription: false,
//      qstrings: {
//         id: '50061359@N03'
//      },
//      itemTemplate: '<li>' +
//                        '<div class="portfolio_box">' +
//                        '<a rel="colorbox" href="{{image_b}}" title="{{title}}">' +
//                           '<img src="{{image_m}}" alt="{{title}}" />' +
//                        '</a>' +
//                        '<div class="title">{{title}}</div>' +
//                        '</div>' +
//                     '</li>'
//   }, function (data) {
//      $('#cbox a').colorbox();

//      $("#cbox li a").append("<span class='portfolio_zoom'><img alt='' src='/Templates/idea/images/zoom.png'></span>");

//      $("#cbox li").hover(
//         function() {
//            $(".portfolio_zoom", this).fadeIn('500');
//	      }, 
//         function() {
//		      $(".portfolio_zoom", this).fadeOut('500');
//	      }
//      );

//   });


});
