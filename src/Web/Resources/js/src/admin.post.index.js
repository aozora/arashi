/// <summary>
/// admin.post.index
/// Manage post list
/// </summary>
$(function () {

   $(".post-delete").click(function (e) {
      e.preventDefault();

      var $this = $(this);

      if (confirm("Are you sure to delete this post ?")) {

         var href = $this.attr("href");
         $this.attr("href", "#");

         $.post(href, {}, function (data) {

            // if the delete was successfull delete the file row from the DOM
            if (data.indexOf("<!--msg-alert") > -1) {
               showMessage(data);
            } else {
               // remove the row of current file
               $this.parents('tr').eq(0).remove();
               showMessage(data);
            }
         });

      } // end if confirm
   });

});

