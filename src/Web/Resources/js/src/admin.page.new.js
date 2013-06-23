/// <summary>
/// admin.page.edit
/// Manage the edit of a page
/// </summary>

$(function () {

   // Post edit megamenu
   $("#post-edit-tags a").button();

   $("#post-edit-publish").click(function () {
      showPostEditMenu($("#post-edit-publish"), $("#menu-publish"));
   });
   $("#post-edit-customtemplatefile").click(function () {
      showPostEditMenu($("#post-edit-customtemplatefile"), $("#menu-customtemplatefile"));
   });
   $("#post-edit-parentpage").click(function () {
      showPostEditMenu($("#post-edit-parentpage"), $("#menu-parentpage"));
   });
});



/// <summary>
/// Show hide the mega menu on click event
/// </summary>
function showPostEditMenu($button, $menu) {
   // determine if already opened
   if ($menu.is(':hidden')) {

      // IE Bug: must reset the position
      $menu.css({ top: 0, left: 0 });

      // set the position
      $menu.position({
         my: 'left top',
         at: 'left bottom',
         of: $button,
         offset: '0 0'
      });

      $menu.show();
      $button.removeClass("ui-corner-all");
      $button.addClass("ui-corner-top");
      $menu.removeClass("ui-corner-all");
      $menu.addClass("ui-corner-bottom");
      $menu.addClass("ui-corner-tr");
   } else {
      $menu.hide();
      $button.removeClass("ui-corner-top");
      $button.addClass("ui-corner-all");
      $menu.removeClass("ui-corner-bottom");
      $menu.removeClass("ui-corner-tr");
      $menu.addClass("ui-corner-all");
   }
}

