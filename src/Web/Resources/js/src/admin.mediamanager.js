/// <summary>
/// admin.mediamanager.js
/// Media Manager
/// </summary>

$(function () {

   // bind the click to show the upload box
   //   $("#newMediaLink").click(function() {
   //      $("#upload-container").position({ my: "center", at: "center", of: window });
   //      $("#upload-container").show();
   //   });

   $("#start-upload-button").click(function () {
      StartUpload();
   });

   $("#close-upload-container").click(function () {
      $("#upload-container").fadeOut(function () {
         
         // clear the upload list
         ClearList();

         // reload the page if one or more have been uploaded
         if (document.getElementById("slmfu").Content.Files.TotalUploadedFiles > 0)
            location.reload(true);
      });

   });



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



var slCtl = null;

function onSilverlightError(sender, args) {
   var appSource = "";
   if (sender != null && sender != 0) {
      appSource = sender.getHost().Source;
   }

   var errorType = args.ErrorType;
   var iErrorCode = args.ErrorCode;

   if (errorType == "ImageError" || errorType == "MediaError") {
      return;
   }

   var errMsg = "Unhandled Error in Silverlight Application " + appSource + "\n";

   errMsg += "Code: " + iErrorCode + "    \n";
   errMsg += "Category: " + errorType + "       \n";
   errMsg += "Message: " + args.ErrorMessage + "     \n";

   if (errorType == "ParserError") {
      errMsg += "File: " + args.xamlFile + "     \n";
      errMsg += "Line: " + args.lineNumber + "     \n";
      errMsg += "Position: " + args.charPosition + "     \n";
   }
   else if (errorType == "RuntimeError") {
      if (args.lineNumber != 0) {
         errMsg += "Line: " + args.lineNumber + "     \n";
         errMsg += "Position: " + args.charPosition + "     \n";
      }
      errMsg += "MethodName: " + args.methodName + "     \n";
   }

   throw new Error(errMsg);
}


//DO NOT FORGET TO REGISTER THIS FUNCTION WITH THE SILVERIGHT CONTROL
// OnPluginLoaded="pluginLoaded"
function pluginLoaded(sender) {
   //IMPORTANT: Make sure this is the same ID as the ID in your <OBJECT tag (<object id="MultiFileUploader" etc)
   slCtl = document.getElementById("slmfu");
//console.log("pluginLoaded");

   //Register All Files Finished Uploading event
   slCtl.Content.Files.AllFilesFinished = AllFilesFinished;

   //Register single file finished event
   slCtl.Content.Files.SingleFileUploadFinished = SingleFileFinished;

   //Register Error occurred during uploading event
   slCtl.Content.Files.ErrorOccurred = ShowErrorDiv;

   //Register MaximumFileSizeReached during selecting files
   slCtl.Content.Control.MaximumFileSizeReached = ShowMaximumFileSizeDiv;

   slCtl.Content.Files.FileAdded = UpdateFileList;
   slCtl.Content.Files.FileRemoved = UpdateFileList;
   slCtl.Content.Files.StateChanged = UpdateFileList;

   slCtl.Content.Files.TotalPercentageChanged = UpdateTotalPercentage;

   //Set your custom parameter using javascript
   //This parameter will be available in the webservice and you can use it for your business logic
   //Or use it to identity the upload to a sinle row in your database
   //slCtl.Content.Files.CustomParams = "custom_id=1"; 
}


function ShowNumberOfFilesUploaded() {
   if (slCtl != null) {
      alert("Total Files Uploaded: " + slCtl.Content.Files.TotalUploadedFiles);
   }
}

function ShowTotalNumberOfFilesSelected() {
   if (slCtl != null) {
      alert("Total Files Selected: " + slCtl.Content.Files.TotalFilesSelected);
   }
}

function ShowUploadProgress() {
   if (slCtl != null) {
      alert("Progress: " + slCtl.Content.Files.Percentage);
   }
}



//This function is registred in the pluginLoaded function (slCtl.Content.Files.AllFilesFinished = AllFilesFinished;)
function AllFilesFinished() {
   //document.getElementById('AllFinishedDiv').style.display = 'block';
   $("#AllFinishedDiv").show();
}

//This function is registred in the pluginLoaded function (slCtl.Content.Files.SingleFileUploadFinished = SingleFileFinished;)
function SingleFileFinished() {
   //document.getElementById('SingleFileFinishedDiv').style.display = 'block';
   $("#SingleFileFinishedDiv").show();
}

//This function is registred in the pluginLoaded function (slCtl.Content.Files.ErrorOccurred = ShowErrorDiv;)
function ShowErrorDiv() {
   //document.getElementById('ErrorDiv').style.display = 'block';
   $("#ErrorDiv").show();
}


function ShowMaximumFileSizeDiv() {
   document.getElementById('MaximumFileSizeDiv').style.display = 'block';
}

//Draws a list of files with the state of each file
function UpdateFileList() {
   var list = "<table class='grid ui-state-default'><thead><tr><td class='align-center'>Name</td><td class='align-center'>Status</td><td></td></tr></thead><tbody class='ui-widget-content'>";
   var userFile;

   var i = 0;
   for (i = 0; i < slCtl.Content.Files.FileList.length; i++) {
      userFile = slCtl.Content.Files.FileList[i];
      list += "<tr><td>" + userFile.FileName + "</td><td>" + userFile.StateString + "</td><td></td></tr>";
   }

   list += "</tbody></table>"

   //document.getElementById('upload-filelist').innerHTML = list;
   $("#upload-filelist").html(list);

   //Update the other statistics...
   UpdateTotalSelected();
//   UpdateTotalPercentage();
   UpdateTotalUploaded();

   $("#upload-container").position({ my: "center", at: "center", of: window });
   $("#upload-container").show();
}



//Updates the number of total files div element
function UpdateTotalSelected() {
//   document.getElementById('TotalSelected').innerHTML = slCtl.Content.Files.TotalFilesSelected + " files selected";
   $("#TotalSelected").html(slCtl.Content.Files.TotalFilesSelected + " files selected");
}

//Updates the total percentage div element
function UpdateTotalPercentage() {
//   document.getElementById('TotalPercentage').innerHTML = slCtl.Content.Files.Percentage * 100 + "% uploaded";
   $("#TotalPercentage").html(slCtl.Content.Files.Percentage * 100 + "% uploaded");
}

//Updates the number of uploaded files div element
function UpdateTotalUploaded() {
//   document.getElementById('TotalUploaded').innerHTML = slCtl.Content.Files.TotalUploadedFiles + " files uploaded";
   $("#TotalUploaded").html(slCtl.Content.Files.TotalUploadedFiles + " files uploaded");
}

//Actions
function StartUpload() {
   if (slCtl != null) {
      slCtl.Content.Control.StartUpload();
   }
}

function ClearList() {
   if (slCtl != null) {
      slCtl.Content.Control.ClearList();
   }
}