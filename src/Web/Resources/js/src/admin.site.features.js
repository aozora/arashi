/// <summary>
/// admin.site.features
/// </summary>
var lastsel;

$(function () {

   // Setup the datepicker
   $.datepicker.setDefaults($.datepicker.regional[""]);

   $("#show-sitefeatures-link").click(function () {
      if ($("#Site").val() != '')
         GetSiteFeatures();
   });

});



function GetSiteFeatures()
{
   $("#enable-all-link")
      .show()
      .attr("href", urlEnableAllSiteFeatures + "?sid=" + $("#Site").val());

   $("#sitefeatures-table").jqGrid({
      url: urlGetSiteFeatures + "?sid=" + $("#Site").val(),
      editurl: urlSaveSiteFeatures + "?sid=" + $("#Site").val(),
      width: 600,
      height: 500,
      shrinkToFit: true,
      datatype: 'json',
      mtype: 'POST',
      colNames: ['Id', 'Name', 'Enabled', 'Start Date', 'End Date'],
      colModel: [
         { name: 'id',
            key: true,
            hidden: true
         },
         { name: 'name',
            index: 'name',
            fixed: true,
            width: 340,
            align: 'left',
            editable: false,
            sortable: false,
            resizable: false
         },
         { name: 'enabled',
            fixed: true,
            width: 60,
            edittype: "checkbox",
            formatter:'checkbox',
            editoptions: { value: "True:False" },
            align: 'center',
            editable: true,
            sortable: false,
            resizable: false
         },
         { name: 'startdate',
            fixed: true,
            align: 'center',
            width: '80',
            editable: true,
            sortable: false,
            resizable: false
         },
         { name: 'enddate',
            fixed: true,
            align: 'center',
            width: '90',
            editable: true,
            sortable: false,
            resizable: false
         }],
         onSelectRow: function(id){
		      if(id && id!==lastsel){
			      jQuery('#sitefeatures-table').jqGrid('restoreRow',lastsel);
			      // jQuery("#grid_id").jqGrid('editRow',rowid, keys, oneditfunc, succesfunc, url, extraparam, aftersavefunc,errorfunc, afterrestorefunc)
			      jQuery('#sitefeatures-table').jqGrid('editRow', id, true, onEditPickDates, editRowSuccess,null, null, null, editRowError);
			      lastsel = id;
		      }
	      }
   });

}



function onEditPickDates(id) {
   jQuery("#" + id + "_startdate", "#sitefeatures-table").datepicker($.datepicker.regional[__cultureInfo["name"].substr(0, 2)]);
   jQuery("#" + id + "_enddate", "#sitefeatures-table").datepicker($.datepicker.regional[__cultureInfo["name"].substr(0, 2)]);
}



function editRowSuccess(data) {
   growl('Feature saved!', false, false);
   $("#sitefeatures-table").trigger("reloadGrid");
   return true;
}


function editRowError(rowid, response) {
   growl('Sorry, an error occured!', true, false);
}