/// <summary>
/// admin.messages.js
/// Manage the messages
/// </summary>

$(function(){

   // Bind click events for the hover links
   $("div.hover-actions a.view").click(function(){
      viewMessage($(this));
      return false; // this stop the event bubbling!
   });
   
   $("div.hover-actions a.delete").click(function(){
      deleteComment($(this), $(this).attr("href"));
      return false; // this stop the event bubbling!
   });
   
   $("div.hover-actions a.reset").click(function(){
      return resetMessage($(this));
   });
   
});



function viewMessage(link) {

   $.get($(link).attr("href"), function (data) {
      var $dialog = $('<div></div>');
      $dialog
         .append(data)
         .dialog({
            width: 500,
            height: 300,
            buttons: {
				            Close: function() {
					            $(this).dialog('close');
				            }
                     }
            });
   });

}



function resetMessage(link) { 
   
   return confirm('Do you really want to reset the attemps count and resend the selected message?');

}



function deleteComment(link, url) {
   
   if (!confirm('Do you really want to delete the selected message?'))
      return false;

   $.post(url, {}, function(data){
      
      // if the delete was successfull delete the comment row from the DOM
      if (data.indexOf("<!--msg-alert") > -1) {
         showMessage(data);
      } else {
         // remove the row of current comment
         $(link).parents('tr').eq(0).remove();
         showMessage(data);
      }
   });
}

