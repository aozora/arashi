/// <summary>
/// admin.comments.js
/// Manage the post comments
/// </summary>

$(function(){

   // Bind click events for the hover links
   $("div.hover-actions a.change-status").click(function(){
      changeStatus($(this));
      return false; // this stop the event bubbling!
   });
   
   $("div.hover-actions a.delete").click(function(){
      deleteComment($(this), $(this).attr("href"));
      return false; // this stop the event bubbling!
   });
   
   $("div.hover-actions a.reply-link").click(function(){
      showReplyContainer($(this));
      return false; // this stop the event bubbling!
   });
   
   $("#reply-cancel-link").click(function(){
      $("#reply-container").fadeOut();
   });
   
   // Stop the submit if the content text is not filled
   $("#replytocommentform").submit(function(){
      if ($("#replyContent").val() == '') {
         growl('Please fill the reply text before submit!', true, true);
         return false;
      }
      
      return true;
   });
   
});



function changeStatus(link) {
   $.post($(link).attr("href"), {}, function(data){
      if (data.indexOf("<!--msg-") > -1) {
         showMessage(data);
      } else {
         // reload row of current comment
         $(link).parents('tr').eq(0).replaceWith(data);
         growl("Comment status updated!", false, false);
      }
   });
}



function deleteComment(link, url) {
   
   if (!confirm('Do you really want to delete the selected comment?'))
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



function showReplyContainer(link) {

   // move the reply container after the current row
   $(link).parents('tr').eq(0).after( $("#reply-container") );
   
   // empty the textarea
   $("#reply-content").val('');
   
   // Set the comment id
   $("#replyToCommentId").val($(link).attr("rel"));
  
   // show the reply box
   $("#reply-container").fadeIn();
}


