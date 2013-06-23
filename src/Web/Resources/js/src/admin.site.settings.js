/// <summary>
/// admin.site.settings
/// </summary>

$(function () {

   $("#tabs").tabs();

   $("#defaultpage-label").click(function () {
      $("#home-page").attr("checked", "checked");
   });
   $("#PagesForHomePage").change(function () {
      $("#home-page").attr("checked", "checked");
   });

   $("#newsitehost-button").click(function () {
      postNewSiteHost();
   });

   $("#hosts-table").jqGrid({
      url: urlGetHosts,
      editurl: urlSaveHost,
      width: 460,
      shrinkToFit: true,
      datatype: 'json',
      mtype: 'POST',
      colNames: ['Id', 'Domain', 'Default', ''],
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
            editable: true,
            sortable: false,
            resizable: false
         },
         { name: 'isdefault',
            fixed: true,
            width: 60,
            edittype:"checkbox",
            editoptions: {value:"Yes:No"},
            align: 'center',
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
            jQuery('#hosts-table').restoreRow(lastsel);
            jQuery('#hosts-table').editRow(id, true, null,
                                           function (data) {
                                              growl('Domain saved!', false, false);
                                              $("#hosts-table").trigger("reloadGrid"); 
                                              return true;
                                           },
                                           null, null, null,
                                           function (rowid, response) {
                                              growl('Sorry, an error occured!', true, false);
                                           }
                                          );

            lastsel = id;
         }
      }, // end onSelectRow
      gridComplete: function () {
         var ids = jQuery("#hosts-table").jqGrid('getDataIDs');
         for (var i = 0; i < ids.length; i++) {
            //var cl = ids[i];
            html = "<button type='button' class='hosts-table-deletebutton' title='Delete this domain' ><img src='/Resources/img/16x16/cross_circle_frame.png' alt='' /></button>";
            jQuery("#hosts-table").jqGrid('setRowData', ids[i], { action: html });
         }
         $("#hosts-table button.hosts-table-deletebutton").click(function () {
            var gr = jQuery("#hosts-table").jqGrid('getGridParam', 'selrow');
            if (gr != null)
               jQuery("#hosts-table").jqGrid('delGridRow', gr, { reloadAfterSubmit: false });
         });

      }
   });


});



/// Save the new site host
function postNewSiteHost() {
   var host = $("#newhostname").val();

   if (_isNull(host)) {
      growl('To add a new domain you must specify it!', true, true);
      return false;
   }

   $.post(urlAddHost, {
      newhostname: host
   }, function (data) {

      showMessage(data);

      if (data.indexOf("<!--msg-alert") == -1) {
         // reload the grid
         $("#host-table").trigger("reloadGrid");

         // reset form
         $("#newhostname").val("");
      }
   });
}