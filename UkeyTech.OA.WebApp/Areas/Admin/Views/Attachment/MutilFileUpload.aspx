<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <meta name="viewport" content="width=device-width" />
    <title>多文件上传</title>
    <%:Styles.Render("~/styles/upload")%>
    <%:Scripts.Render("~/bundles/upload")%>
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/plupload/browserplus-min.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/plupload/plupload.full.min.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/plupload/jquery.ui.plupload/jquery.ui.plupload.min.js")%>"></script>
</head>
<body>
   
    <div id="uploader" style="height:300px;">
        <p>由于你的浏览器不支持Flash, Silverlight 及 HTML5,请使用上传按钮上传文件.</p>
    </div>


<script type="text/javascript">
   <%{ 
              var configkey = Request["cfgtype"] ?? "CommonUpload";
              var config = Clover.Config.FileUploadConfig.GetFUConfig(configkey); 
              var strMaxSize = config.MaxFileSize > 1024 ? ((config.MaxFileSize / 1024 + 1).ToString() + "mb") : (config.MaxFileSize + "kb");
  %>    
function initMutilFileUpload() {    
    $("#uploader").hide();
    
    $("#uploader").plupload({
            // General settings
            runtimes : 'html5,flash,silverlight,html4',
            url : "<%=Url.Action("QuickAttachmentUpload", "Attachment", new{Area = "Admin"})%>?cfgtype=<%=Request["type"] %>&TargetID=<%=Request["TargetID"] %>&TargetType=<%=Request["TargetType"] %>",
            // Maximum file size
            max_file_size : '<%=strMaxSize %>',
            chunk_size: '1mb',
          
            // Specify what files to browse for
            filters : [
                {title : "Office files", extensions : "doc,docx,xls,xlsx,ppt,pptx,mpp,mppx,vsd,vsdx,rtf,txt,xml,pdf,pps,cad"},
                {title : "Image files", extensions : "jpg,gif,png,bmp,tiff"},
                {title : "Zip files", extensions : "zip,rar,7z"},
                {title : "Media files", extensions : "mp3,mp4,avi,rmvb,flv,mkv,rm,wav"},
                {title : "Other Files", extensions : "bak"}
            ],  
            // Rename files by clicking on their titles
            rename: true,
          // Sort files
            sortable: true,
            // Enable ability to drag'n'drop files onto the widget (currently only HTML5 supports that)
            dragdrop: true,
            // Views to activate
            views: {
                list: true,
                thumbs: true, // Show thumbs
                active: 'thumbs'
            },
            // Flash settings
            flash_swf_url : '/Scripts/plupload/Moxie.swf',
            // Silverlight settings
            silverlight_xap_url : '/Scripts/plupload/Moxie.xap',
            init : {
                UploadComplete:function(){
                    window.parent.RunBackFunc();
                    window.parent.CloseWin();
                }
            }
     });
     $("#uploader").show();
}
$(function(){
  initMutilFileUpload();
});
<% }%>
</script>
</body>
</html>
