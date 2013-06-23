var timeBetweenSlides = 8000;
var ActualPromo = 1;
var SwitchID = 0;


$(document).ready(function () { // activates the promo slider

   if (document.getElementById("HomepagePromo") != null) {
      showPromo();
   }

});



// start positions for promo thumbnails - now are setted to be ousite of its container in the right side

function setPositions(PromoNr) {
   xMiddle = parseInt(document.body.clientWidth / 2);
   //wypos = parseInt(document.getElementById("Header").offsetTop);

   document.getElementById('BigThumbnail' + PromoNr).style.left = 950 + 'px';
   document.getElementById('BigThumbnail' + PromoNr).style.top = 43 + 'px';

   document.getElementById('MediumThumbnail' + PromoNr).style.left = 950 + 'px';
   document.getElementById('MediumThumbnail' + PromoNr).style.top = 133 + 'px';
}



function switchPromo(PromoNr) {
   if (PromoNr != ActualPromo) {
      // if somebody clicks very fast on promo buttons the queue is cleared first
      $('#PromoInfo' + ActualPromo).clearQueue();
      $('#MediumThumbnail' + ActualPromo).clearQueue();
      $('#BigThumbnail' + ActualPromo).clearQueue();
      clearTimeout(SwitchID);

      // here starts the transition between slides
      hidePromo(ActualPromo);
      $('#PButton' + ActualPromo).removeClass('PromoButton_on').addClass('PromoButton');
      ActualPromo = PromoNr;
      $('#PButton' + ActualPromo).removeClass('PromoButton').addClass('PromoButton_on');
      showPromo(PromoNr);
   }
}


function showPromo(PromoNr) {

   if (!PromoNr) {
      PromoNr = ActualPromo;
   }

   setPositions(PromoNr);

   $('#MediumThumbnail' + PromoNr).delay(600);
   $('#MediumThumbnail' + PromoNr).show(0);
   $('#BigThumbnail' + PromoNr).delay(900);
   $('#BigThumbnail' + PromoNr).show(0);

   $('#PromoInfo' + ActualPromo).delay(900);
   $('#PromoInfo' + ActualPromo).fadeIn(500); // change this to animate the showing of promo text in another style

   $('#MTC' + ActualPromo).show(0);
   $('#BTC' + ActualPromo).show(0);
   $('#MTC' + ActualPromo).fadeIn(0);
   $('#BTC' + ActualPromo).fadeIn(0);


   $('#MediumThumbnail' + PromoNr).animate({ left: '-=420' }, 1000); // change this to animate the hiding of smaller thumbnail in another style
   $('#BigThumbnail' + PromoNr).animate({ left: '-=300' }, 1000); // change this to animate the hiding of bigger thumbnail in another style


   PromoNr = ActualPromo + 1;

   if (document.getElementById('PButton' + PromoNr) == null) {
      PromoNr = 1;
   }

   SwitchID = setTimeout("switchPromo(" + PromoNr + ")", timeBetweenSlides);

}

function hidePromo(PromoNr) {

   if (!PromoNr) {
      PromoNr = ActualPromo;
   }

   xMiddle = parseInt(document.body.clientWidth / 2);

   $('#PromoInfo' + ActualPromo).delay(600);
   $('#PromoInfo' + ActualPromo).fadeOut(500); // change this to animate the hiding of promo text in another style

   $('#MediumThumbnail' + PromoNr).animate({ left: '+=420' }, 300); // change this to animate the hiding of smaller thumbnail in another style
   $('#BigThumbnail' + PromoNr).delay(200);
   $('#BigThumbnail' + PromoNr).animate({ left: '+=300' }, 300); // change this to animate the hiding of bigger thumbnail in another style

   $('#MTC' + ActualPromo).delay(600);
   $('#BTC' + ActualPromo).delay(600);
   $('#MTC' + ActualPromo).fadeOut(0)
   $('#BTC' + ActualPromo).fadeOut(0)

   $('#BigThumbnail' + PromoNr).hide(0);
   $('#MediumThumbnail' + PromoNr).hide(0);

}

var mousex;
var mousey;

jQuery(document).ready(function () {
   $(document).mousemove(function (e) {
      mousex = e.pageX;
      mousey = e.pageY;
   });
})



function show_details(obj) {
   $('#' + obj).show(0);
}


function popup_details(obj) {
   document.getElementById(obj).style.left = mousex + 'px';
   document.getElementById(obj).style.top = mousey + 'px';
}


function hide_details(obj) {
   $('#' + obj).hide(0);
}


//function changeHomepageTabs(obj, buttobj) {

//   $('#UserSelectedArticles').fadeOut(200);
//   $('#LatestArticles').fadeOut(200);
//   $('#LatestTopics').fadeOut(200);

//   $('#Butt1').removeClass('CurrentPagedepth1').addClass('depth1');
//   $('#Butt2').removeClass('CurrentPage');
//   $('#Butt3').removeClass('CurrentPagedepth3').addClass('depth3');

//   if (buttobj == 'Butt1') {
//      $('#' + buttobj).removeClass('depth1').addClass('CurrentPagedepth1');
//   }
//   if (buttobj == 'Butt2') {
//      $('#' + buttobj).addClass('CurrentPage');
//   }
//   if (buttobj == 'Butt3') {
//      $('#' + buttobj).removeClass('depth3').addClass('CurrentPagedepth3');
//   }

//   $('#' + obj).delay(200);
//   $('#' + obj).fadeIn(200);

//}
