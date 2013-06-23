/// <summary>
/// admin.newpost
/// Manage the NewPost view
/// </summary>

$(function() {

   // Post edit megamenu
   $("#post-edit-tags a").button();

   $("#post-edit-publish").click(function () {
      showPostEditMenu($("#post-edit-publish"), $("#menu-publish"));
   });
   $("#post-edit-tags").click(function () {
      showPostEditMenu($("#post-edit-tags"), $("#menu-tags"));
   });
   $("#post-edit-categories").click(function () {
      showPostEditMenu($("#post-edit-categories"), $("#menu-categories"));
   });
// custom fields are disabled for new post, only for edit!
//   $("#post-edit-customfields").click(function () {
//      showPostEditMenu($("#post-edit-customfields"), $("#menu-customfields"));
//   });


  
   $("#showSummaryLink").click(function(){
      $(this).hide();
      $("#summaryFieldset").slideToggle();
   });
   
   $("#addNewCategoryLink").click(function(){
      $("#newCategoryPanel").slideToggle();
   });
   
   $("#submitNewTagLink").click(function(){
      postNewTag();
   });

   $("#submitNewCategoryLink").click(function(){
      postNewCategory();
   });

   // hover effect on tagchecklist
   setTagCheckListEvents();

   $("#tab-tag-selection").tabs();
   
   // barba-trick: this is for the em font size problem
   $("#tab-tag-selection").removeClass("ui-widget");

   // combobox for add tags from the "all" list
   $("#all-tags-select").change(function(){
      var $selected = $("option:selected", this);
      
      if ($selected.val() != '') {
         addTagToPost( $selected.val(), $selected.text() );
         $(this).val(0);
      }
   });

   
   getTagCloud();
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



function postNewTag() {
   $.post(urlSaveNewTag, {Name: $("#newtagform input#TagName").val()}, function(data){
      showMessage(data);
      
      // reset form
      $("#newtagform input#TagName").val("");
      
      getTagCloud();
   });
}



function postNewCategory() {

   var name = $("#newcategoryform input#CategoryName").val();
   var parentCategoryId = $("#newcategoryform select#ParentCategory").val();

   if (_isNull(name)) {
      growl('To add a new category you must specify the name!', true, true);
      return false;
   }

   $.post(urlSaveNewCategory, {
      Name: name,
      parentCategoryId: parentCategoryId
   }, function (data) {

      showMessage(data);

      // reset form
      $("#newcategoryform input#CategoryName").val("");
      $("#newcategoryform select#ParentCategory").val("");

      // should also update the categories list
      // ...
   });
}


   
/// Add a tag widget to the tagchecklist
function addTagToPost( value, text ) {
   
   // check if the tag has already been added previously
   if ( $("#tagchecklist").find("li .tag input[value=" + value + "]").length > 0 )
      return;

   var inputId = "tagid-" + value;
   var $li = $("<li></li>");
   var $input = $("<input type='hidden' id='" + inputId + "' name='tagid' value='" + value + "'/>");
   var $tag = $("<div class='tag'>" + text + "</div>");
   var $delete = $("<a class='tag-remove' href='#' title='Remove this tag'><span class='ui-icon ui-icon-circle-close'></span>remove</a>");
   
   $tag.append($input);
   $li.append($tag)
      .append($delete);
   
   // add the tag widget to the dom
   $("#tagchecklist").append($li);
   
   // hide the empty template
   $("#tagchecklist-empty").hide();
   
   // reset event handlers
   setTagCheckListEvents();
   
      // + live x remove
}



function getTagCloud() {
   // update Tag Cloud
   $.get(urlGetTagCloud, function(data){
      if (data != ''){
         $("#tabPopular").empty().append(data);
         
         $("#tabPopular a").unbind();
         $("#tabPopular a").click(function(){
            addTagToPost( $(this).attr("rel"), $(this).text() );
         });
      }
   });
}



// unbind e re-set the hover event on the tagchecklist
// and on the remove links
function setTagCheckListEvents() {
   $("#tagchecklist li").unbind();
   
   $("#tagchecklist li")
      .hover(
         function() {
            $("a.tag-remove", this).show();
         },
         function() {
            $("a.tag-remove", this).hide();
         }
      );
      
   $("#tagchecklist li a.tag-remove").unbind();
   $("#tagchecklist li a.tag-remove").click(function(){
      $(this).parent().remove();
   });
}