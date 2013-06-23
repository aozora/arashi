/// <summary>
/// admin.mediamanager.js
/// Media Manager
/// </summary>

//var grid;
//var data = [];
//var loader;

//var fileFormatter = function (row, cell, value, columnDef, dataContext) {
//   return "<img width='80' height='60' title=''" + dataContext["file"] + "'' alt='' class=\"attachment-80x60\" src='" + dataContext["img"] + "' />";
//   //return "<b><a href='" + dataContext["link"] + "' target=_blank>" + dataContext["title"] + "</a></b><br/>" + dataContext["description"];
//};


//var columns = [
//			{ id: "img", name: "", field: "img", width: 50, cssClass: "width-50", resizable: false, sortable: false },
//			{ id: "file", name: "File", field: "file", minWidth: 400, cssClass: "multitext", resizable: false, sortable: true },
//			{ id: "date", name: "Date", field: "date", width: 200, resizable: false, sortable: true }
//		];

//var options = {
//   height: 500,
//   rowHeight: 64,
//   editable: false,
//   enableAddRow: false,
//   enableCellNavigation: false,
//   forceFitColumns: true,
//   multiSelect: false
//};

//var loadingIndicator = null;
////var dataView;


$(function () {
   //loader = new Slick.Data.RemoteModel();



   //   $.getJSON('/Admin/0/MediaManager/GetMediaData', function (data) {

   //      //dataView = new Slick.Data.DataView();
   //      //grid = new Slick.Grid($("#media-grid"), dataView, columns, options);
   //      grid = new Slick.Grid($("#media-grid"), data, columns, options);


   ////      // initialize the model after all the events have been hooked up
   ////      dataView.beginUpdate();
   ////      dataView.setItems(data);
   ////      dataView.setFilter(myFilter);
   ////      dataView.endUpdate();



   ////      grid.onSort = function (sortCol, sortAsc) {
   ////         sortdir = sortAsc ? 1 : -1;
   ////         sortcol = sortCol.field;

   ////         if (sortAsc == true)
   ////            data.sort(compare);
   ////         else
   ////            data.reverse(compare);

   ////         grid.render();
   ////      };
   //   });


   //   $("#search-submit").button({
   //      icons: {
   //         primary: "ui-icon-search"
   //      },
   //      text: false
   //   }).removeClass("ui-corner-all").addClass("ui-corner-right");

   $("#media a.colorbox").colorbox({ 
      minWidth: 550, 
      minHeight: 300,
      current: _cb_current,
      previous: _previous,
		next: _next,
		close: _close
   });

   $("#search-submit")
   	.hover(
		   function () {
		      $(this).addClass("ui-state-hover");
		   },
		   function () {
		      $(this).removeClass("ui-state-hover");
		   }
	   );


   $("a.delete-media").click(function () {
      if (confirm("Are you sure to permanently delete this media file ?")) {

         var $link = $(this);

         $.post(urlDeleteMedia, {
            name: $("input.media-name", $link.parent()).val()
         }, function (data) {

            // if the delete was successfull delete the file row from the DOM
            if (data.indexOf("<!--msg-alert") > -1) {
               showMessage(data);
            } else {
               // remove the row of current file
               $link.parents('tr').eq(0).remove();
               showMessage(data);
            }
         });

      }
   });


});



