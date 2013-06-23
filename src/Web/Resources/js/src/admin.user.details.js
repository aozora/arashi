/// <summary>
/// admin.user.details
/// </summary>
$(function () {

   $('#tabs').tabs();
   $("#auth-tabs").tabs();


   // setup button widget
   $("ul.providers li", "#providers-container").addClass("ui-button ui-widget ui-state-default ui-corner-all ui-shadow");
   $("ul.providers li", "#providers-container")
            .hover(
		         function () {
		            $(this).addClass("ui-state-hover");
		         },
		         function () {
		            $(this).removeClass("ui-state-hover");
		         }
	         );

   var uri = $("#current-provider-uri").val();


   if (uri != '') {
      // show the logo for the current provider (if selected)

      $("#providers-container ul.providers li").each(function () {
         var title = $(this).attr("title").toLowerCase();

         if (uri.indexOf(title) > -1) {
            var logo = $("img", $(this)).attr("src");
            $("#current-provider-logo").attr("src", logo);

            // hide the provider list
            $("#external-provider-container").hide();
         }
      });
   } else {
      // ------------- openid script -----------------

      // uri vars
      var externalFormAction = "/Admin/" + siteid + "/Users/" + "OpenIdConnect/" + userid;
      var facebookFormAction = "/Admin/" + siteid + "/Users/" + "FacebookConnect/" + userid;


//      // Load Facebook connect scripts if the FB logo is visible
//      if ($("#fb-logo").is(':visible')) {
//         $.getScript('http://connect.facebook.net/en_US/all.js', function () {
//            FB.init({ appId: $("#fb-appid").val(), status: true, cookie: true, xfbml: true });
//         });
//      }

      var $this = $("form#userdetailsform");
      var $usr = $("#external-provider-container", $this).find('input[name=openid_username]');
      var $id = $("#external-provider-container", $this).find('input[name=openid_identifier]');
      var $front = $("#external-provider-container", $this).find('div:has(input[name=openid_username])>span:eq(0)');
      var $end = $("#external-provider-container", $this).find('div:has(input[name=openid_username])>span:eq(1)');
      var $usrfs = $("#external-provider-container", $this).find('ol:has(input[name=openid_username])');
      var $idfs = $("#external-provider-container", $this).find('ol:has(input[name=openid_identifier])');

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
      $("#external-provider-container", $this).find('li.direct').click(function () {
         var $li = $(this);
         $li.parent().find('li').removeClass('highlight');
         $li.addClass('highlight');
         $usrfs.fadeOut();
         $idfs.fadeOut();

         // set the post action for facebook
         $this.attr("action", externalFormAction);

         $this.unbind('submit').submit(function () {
            $id.val($("#external-provider-container", $this).find("li.highlight span").text());
         });

         $this.submit();
         return false;
      });

      // OpenID Provider
      $("#external-provider-container", $this).find('li.openid').click(function () {
         var $li = $(this);
         $li.parent().find('li').removeClass('highlight');
         $li.addClass('highlight');
         $usrfs.hide();
         $idfs.show();
         $id.focus();
         $this.unbind('submit').submit(submitid);

         // set the post action for facebook
         $this.attr("action", externalFormAction);

         return false;
      });

      // Other OpenID Providers
      $("#external-provider-container", $this).find('li.username').click(function () {
         var $li = $(this);
         $li.parent().find('li').removeClass('highlight');
         $li.addClass('highlight');
         $idfs.hide();
         $usrfs.show();
         $("#external-provider-container", $this).find('label[for=openid_username] span').text($li.attr("title"));
         $front.text($li.find("span").text().split("username")[0]);
         $end.text("").text($li.find("span").text().split("username")[1]);
         $id.focus();
         $this.unbind('submit').submit(submitusr);

         // set the post action for openid
         $this.attr("action", externalFormAction);

         return false;
      });

      // Facebook Provider
      $("#external-provider-container", $this).find('li.facebook').click(function () {
         var $li = $(this);
         $li.parent().find('li').removeClass('highlight');
         $li.addClass('highlight');
         $usrfs.hide();
         $idfs.hide();
         //$id.focus();

         // set the post action for facebook
         $this.attr("action", facebookFormAction);
         $this.unbind('submit').submit();

//         FB.init({ appId: $("#fb-appid").val(), status: true, cookie: true, xfbml: true });

//         FB.login(function (response) {
//            if (response.session) {
//               $this.unbind('submit').submit();
//            } else {
//               // user cancelled login
//               $("#loginbox div.validation-summary-errors").show().text("Can not logon to Facebook");
//            }
//         });

         return false;
      });


      $("#external-provider-container", $this).find('li span:not(.ui-button-text)').hide();

      $usrfs.hide();
      $idfs.hide();
   }

});


/// Save the new site host
function removeExternalId() {
   return confirm("Are you sure to remove the association with the external provider? After confirming, this user can login to Arashi only with email/password.");
}