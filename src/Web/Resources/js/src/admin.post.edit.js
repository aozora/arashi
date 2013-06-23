/// <summary>
/// admin.post.edit
/// Manage the edit of a post
/// </summary>

$(function () {

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
   $("#post-edit-customfields").click(function () {
      showPostEditMenu($("#post-edit-customfields"), $("#menu-customfields"));
   });


   // Edit permalink events
   $("#post-permalink-edit").click(function () {
      $(this).hide();
      $("#post-permalink-edit-container").fadeIn();
   });

   $("#post-permalink-edit-cancel").click(function () {
      $("#post-permalink-edit-container").fadeOut();
      $("#post-permalink-edit").show();
   });

   $("#post-permalink-edit-submit").click(function () {
      postPermalinkUpdate();
   });


   $("#showSummaryLink").click(function () {
      $(this).hide();
      $("#summaryFieldset").slideToggle();
   });

   $("#addNewCategoryLink").click(function () {
      $("#newCategoryPanel").slideToggle();
   });
   $("#submitNewTagLink").click(function () {
      postNewTag();
   });

   $("#addNewCustomFieldLink").click(function () {
      $("#newCustomFieldPanel").slideToggle();
   });
   $("#submitNewCustomFieldLink").click(function () {
      postNewCustomField();
   });


   $("#submitNewCategoryLink").click(function () {
      postNewCategory();
   });

   // hover effect on tagchecklist
   setTagCheckListEvents();

   $("#tab-tag-selection").tabs();

   // barba-trick: this is for the em font size problem
   $("#tab-tag-selection").removeClass("ui-widget");

   // combobox for add tags from the "all" list
   $("#all-tags-select").change(function () {
      var $selected = $("option:selected", this);

      if ($selected.val() != '') {
         addTagToPost($selected.val(), $selected.text());
         $(this).val(0);
      }
   });


   getTagCloud();


   $("#table-customfields").jqGrid({
      url: urlGetCustomFields,
      editurl: urlUpdateCustomFields,
      width: 340,
      height: 'auto',
      shrinkToFit: true,
      datatype: 'json',
      mtype: 'POST',
      colNames: ['Key', 'Value', ''],
      colModel: [
         { name: 'key',
            fixed: true,
            key: true,
            align: 'left',
            width: '100',
            editable: false,
            sortable: false,
            resizable: false
         },
         { name: 'value',
            fixed: true,
            align: 'left',
            editable: true,
            sortable: false,
            resizable: false
         },
         { name: 'action',
            fixed: true,
            align: 'center',
            width: '40',
            editable: false,
            sortable: false,
            resizable: false
         }],
      onSelectRow: function (id) {
         if (id && id !== lastsel) {
            jQuery('#table-customfields').restoreRow(lastsel);
            jQuery('#table-customfields').editRow(id, true, null,
                                           function (data) {
                                              growl('Custom field saved!', false, false);
                                              return true;
                                           },
                                           null, null, null,
                                           function (rowid, response) {
                                              growl('Sorry, an error occured!', true, false);
                                           });
            lastsel = id;
         }
      }, // end onSelectRow
      gridComplete: function () {
         var ids = jQuery("#table-customfields").jqGrid('getDataIDs');
         for (var i = 0; i < ids.length; i++) {
            //var cl = ids[i];
            html = "<button type='button' class='table-customfields-deletebutton' title='Delete this custom field' ><img src='/Resources/img/16x16/cross_circle_frame.png' alt='' /></button>";
            jQuery("#table-customfields").jqGrid('setRowData', ids[i], { action: html });
         }
         $("#table-customfields button.table-customfields-deletebutton").click(function () {
            var gr = jQuery("#table-customfields").jqGrid('getGridParam', 'selrow');
            if (gr != null) 
               jQuery("#table-customfields").jqGrid('delGridRow', gr, { reloadAfterSubmit: false });
         });

      }
   });

});



/// <summary>
/// Show hide the mega menu on click event
/// </summary>
function showPostEditMenu( $button, $menu ) {
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



/// <summary>
/// Create a new tag
/// </summary>
function postNewTag() {
   var tagName = $("#newtagform input#TagName").val();
   
   $.post(urlSaveNewTag, {Name: tagName}, function(data){
      showMessage(data);
      
      // reset form
      $("#newtagform input#TagName").val("");
      
      getTagCloud();
      
      // add in the post tag list with tagid
      $.get(urlGetTagId, {name: tagName}, function(data){
         if (data != '')
            addTagToPost( data, tagName );
      });      
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
      }, function(data){
      
      showMessage(data);
      
      // reset form
      $("#newcategoryform input#CategoryName").val("");
      $("#newcategoryform select#ParentCategory").val("");
   
      // should also update the categories list
      // ...
   });
}



function postNewCustomField() {
   var key = $("#customfield-form input#Key").val();
   var value = $("#customfield-form textarea#Value").val();

   if (_isNull(key) || _isNull(value)) {
      growl('To add a new custom field you must specify the key and the value!', true, true);
      return false;
   }

   $.post(urlSaveNewCustomField, {
      contentitemid: $("#contentitemid").val(),
      key: key,
      value: value
   }, function (data) {

      showMessage(data);

      // reset form
      $("#customfield-form input#Key").val("");
      $("#customfield-form textarea#Value").val("");

      // reload the grid
      $("#table-customfields").trigger("reloadGrid");
   });
}


   
/// <summary>
/// Add a tag widget to the tagchecklist
/// </summary>
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


/// <summary>
/// Post the updated permalink for the current post
/// <summary>
function postPermalinkUpdate(){

   $.post(urlUpdatePermalink, {
         permalink: $("#FriendlyName").val(),
         id: $("#post-permalink-edit-submit").attr("rel")
      }, function(data){
      
      showMessage(data);
      
      // hide the permalink edit panel
      $("#post-permalink-edit-container").fadeOut();
      $("#post-permalink-edit").show();
   });
}