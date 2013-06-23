<%@ Control Language="C#" Inherits="Arashi.Web.Mvc.Views.AdminViewUserControlBase<Arashi.Web.Areas.Admin.Models.UploadModel>" %>
<%@ Import Namespace="Arashi.Core.Domain.Extensions" %>
<div id="upload-container" class="ui-widget ui-state-default ui-corner-all">
   <div id="upload-title">
      <img alt="upload" src="/Resources/img/32x32/upload.png" class="icon" />
      <h3>Upload files</h3>
   </div>
   <div id="upload-inner-container" class="ui-widget-content">
      <p>To transfer to Arashi the selected files click on the 'Upload' button below:</p>
      <div id="AllFinishedDiv" style="display: none;">All files finished (javascript triggered).</div>
      <div id="SingleFileFinishedDiv" style="display: none;">Single file upload finished (javascript triggered).</div>
      <div id="ErrorDiv" style="display: none;">Error occurred during upload (javascript triggered).</div>
      <div id="MaximumFileSizeDiv" style="display: none;">Selected file is bigger than maximum file size.</div>

      <div id="upload-filelist"></div>

      <br />
      <div id="TotalSelected"></div>
      <%--<div id="TotalPercentage"></div>--%>
      <div id="TotalUploaded"></div>

      <div id="upload-silverlightControlHost" >
         <object id="slprogress"
                 data="data:application/x-silverlight-2," 
                 type="application/x-silverlight-2" 
                 width="500" height="20">
            <param name="source" value="/ClientBin/mpost.SilverlightMultiFileUpload.Progress.xap" />
            <param name="minRuntimeVersion" value="4.0.50401.0" />
            <param name="autoUpgrade" value="true" />
            <a href="http://go.microsoft.com/fwlink/?LinkID=149156&v=4.0.50401.0" style="text-decoration:none">
               <img src="http://go.microsoft.com/fwlink/?LinkId=108181" alt="Get Microsoft Silverlight" style="border-style:none"/>
            </a>
         </object>
         <iframe id="_sl_historyFrame2" style='visibility: hidden; height: 0; width: 0; border: 0px'></iframe>
      </div>
   </div>
   <br />
   <p class="align-center">
      <a id="start-upload-button" 
         href="#"
         class="button ui-shadow" >
         Upload!
      </a>
      &nbsp;|&nbsp;
      <a id="close-upload-container" href="#" class="">Close</a>
   </p>
</div>
