/// <summary>
/// admin.page.edit
/// Manage the edit of a page
/// </summary>

$(function () {
   $("#tabs").tabs();
});



///// <summary>
///// Show hide the mega menu on click event
///// </summary>
//function showPostEditMenu($button, $menu) {
//   // determine if already opened
//   if ($menu.is(':hidden')) {

//      // IE Bug: must reset the position
//      $menu.css({ top: 0, left: 0 });

//      // set the position
//      $menu.position({
//         my: 'left top',
//         at: 'left bottom',
//         of: $button,
//         offset: '0 0'
//      });

//      $menu.show();
//      $button.removeClass("ui-corner-all");
//      $button.addClass("ui-corner-top");
//      $menu.removeClass("ui-corner-all");
//      $menu.addClass("ui-corner-bottom");
//      $menu.addClass("ui-corner-tr");
//   } else {
//      $menu.hide();
//      $button.removeClass("ui-corner-top");
//      $button.addClass("ui-corner-all");
//      $menu.removeClass("ui-corner-bottom");
//      $menu.removeClass("ui-corner-tr");
//      $menu.addClass("ui-corner-all");
//   }
//}

