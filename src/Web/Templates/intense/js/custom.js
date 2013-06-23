$(document).ready(function() {

// Animate buttons, move reflection and fade

//$(".slideinfo").hover(function() {
//    $(this).stop().animate({ marginTop: "0px" }, 300);
//	    $(this).parent().find("div.s-num").stop().animate({ marginBottom: "-55px", opacity: 0.25 }, 500);
//	    $(this).parent().find("p.icon").stop().animate({ marginTop: "-15px", opacity: 1 }, 300);		
//	    $(this).parent().find("span").stop().animate({ marginTop: "-15px", opacity: 1 }, 300);
//	},function(){
//	    $(this).stop().animate({ marginTop: "0px" }, 300);
//	    $(this).parent().find("div.s-num").stop().animate({ marginBottom: "0px", opacity: 1 }, 500);
//	    $(this).parent().find("p.icon").stop().animate({ marginTop: "0px", opacity: 1 }, 300);			
//	    $(this).parent().find("span").stop().animate({ marginTop: "0px", opacity: 1 }, 300);	
//	});
//});

$(".slideinfo").hover(function() {
    $(this).stop().animate({ marginTop: "0px" }, 300);
	    $(this).parent().find("div.s-num").stop().animate({ marginBottom: "-55px", opacity: 0.25 }, 500);
	    $(this).parent().find("p.icon").stop().animate({ marginTop: "-15px"}, 300);		
	    $(this).parent().find("span").stop().animate({ marginTop: "-15px", opacity: 1 }, 300);
	},function(){
	    $(this).stop().animate({ marginTop: "0px" }, 300);
	    $(this).parent().find("div.s-num").stop().animate({ marginBottom: "0px", opacity: 1 }, 500);
	    $(this).parent().find("p.icon").stop().animate({ marginTop: "0px"}, 300);			
	    $(this).parent().find("span").stop().animate({ marginTop: "0px", opacity: 1 }, 300);	
	});
});
