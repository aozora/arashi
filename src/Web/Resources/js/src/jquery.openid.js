//jQuery OpenID Plugin 1.1 Copyright 2009 Jarrett Vance http://jvance.com/pages/jQueryOpenIdPlugin.xhtml
$.fn.openid = function () {

//   // Load Facebook connect scripts if the FB logo is visible
//   if ($("#fb-logo").is(':visible')) {
//      $.getScript('http://connect.facebook.net/en_US/all.js', function () {
//         FB.init({ appId: $("#fb-appid").val(), status: true, cookie: true, xfbml: true });
//      });
//   }

   var $this = $(this);
   var $usr = $this.find('input[name=openid_username]');
   var $id = $this.find('input[name=openid_identifier]');
   var $front = $this.find('div:has(input[name=openid_username])>span:eq(0)');
   var $end = $this.find('div:has(input[name=openid_username])>span:eq(1)');
   var $usrfs = $this.find('ol:has(input[name=openid_username])');
   var $idfs = $this.find('ol:has(input[name=openid_identifier])');

   var submitusr = function () {
      if ($usr.val().length < 1) {
         $usr.focus();
         return false;
      }
      $id.val($front.text() + $usr.val() + $end.text());
      return true;
   };

   var submitid = function () {
      if ($id.val().length < 1) {
         $id.focus();
         return false;
      }
      return true;
   };

   // Yahoo & Google Providers
   $this.find('li.direct').click(function () {
      var $li = $(this);
      $li.parent().find('li').removeClass('highlight');
      $li.addClass('highlight');
      $usrfs.fadeOut();
      $idfs.fadeOut();

      $this.unbind('submit').submit(function () {
         $id.val($this.find("li.highlight span").text());
      });

      $this.submit();
      return false;
   });

   // OpenID Provider
   $this.find('li.openid').click(function () {
      var $li = $(this);
      $li.parent().find('li').removeClass('highlight');
      $li.addClass('highlight');
      $usrfs.hide();
      $idfs.show();
      $id.focus();
      $this.unbind('submit').submit(submitid);
      return false;
   });

   // Other OpenID Providers
   $this.find('li.username').click(function () {
      var $li = $(this);
      $li.parent().find('li').removeClass('highlight');
      $li.addClass('highlight');
      $idfs.hide();
      $usrfs.show();
      $this.find('label[for=openid_username] span').text($li.attr("title"));
      $front.text($li.find("span").text().split("username")[0]);
      $end.text("").text($li.find("span").text().split("username")[1]);
      $id.focus();
      $this.unbind('submit').submit(submitusr);
      return false;
   });

   // Facebook Provider
   $this.find('li.facebook').click(function () {
      var $li = $(this);
      $li.parent().find('li').removeClass('highlight');
      $li.addClass('highlight');
      $usrfs.hide();
      $idfs.hide();
      //$id.focus();

      // set the post action for facebook
      $this.attr("action", "/Admin/Login/FacebookConnect");
      $this.submit();

//      FB.init({ appId: $("#fb-appid").val(), status: true, cookie: true, xfbml: true });

//      FB.login(function (response) {
//         if (response.session) {
//            $this.unbind('submit').submit();
//         } else {
//            // user cancelled login
//            $("#loginbox div.validation-summary-errors").show().text("Can not logon to Facebook");
//         }
//      });

      return false;
   });


   $id.keypress(function (e) {
      if ((e.which && e.which == 13) || (e.keyCode && e.keyCode == 13)) {
         return submitid();
      }
   });
   $usr.keypress(function (e) {
      if ((e.which && e.which == 13) || (e.keyCode && e.keyCode == 13)) {
         return submitusr();
      }
   });

   $this.find('li span:not(.ui-button-text)').hide();
   $this.find('li').css('line-height', 0).css('cursor', 'pointer');
   //$this.find('li:eq(0)').click();
   $usrfs.hide();
   $idfs.hide();


   $("ul.providers li", $this).addClass("ui-button ui-widget ui-state-default ui-corner-all ui-shadow");

   // setup button widget
   $("ul.providers li", $this)
      .hover(
		   function () {
		      $(this).addClass("ui-state-hover");
		   },
		   function () {
		      $(this).removeClass("ui-state-hover");
		   }
	   );


   return this;
};


$(document).ready(function () {
   $("form.openid:eq(0)").openid();
});
