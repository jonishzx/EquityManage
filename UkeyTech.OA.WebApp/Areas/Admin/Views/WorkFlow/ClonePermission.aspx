<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/AdminEdit.Master"
    Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    流程上传
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Script" runat="server">  
<link rel="stylesheet" type="text/css" href="<%=Url.Content("~/Content/ui.fileupload.css")%>"/>
<script type="text/javascript" src="<%=Url.Content("~/Scripts/ajaxupload.3.5.js")%>"></script>
<script type="text/javascript">
    $(function(){
        //file upload
        //image file upload plugin
        var a1 = new AjaxUpload($('#upload'), {
            action: '<%=Url.Action("UploadProcess","WorkFlow")%>' + "?t=" + new Date().toString(),
            name: 'uploadfile',
            onSubmit: function (file, ext) {
                if (!(ext && /^(xml|txt)$/.test(ext))) {
                    // extension is not allowed 
                    $('#status').text('只允许上传 XML 或 Txt 文件');
                    return false;
                }
                $('#status').text('上传中...');
            },
            onComplete: function (file, response) {
                //On completion clear the status
                $('#status').text('');
                //Add uploaded file to list
                var rtnMsg = response.split("##");
                if (rtnMsg[0] === "success") {
                    alert("上传成功");
                    var p = GetRealParent(); p.RunBackFunc(); p.CloseWin();

                } else {
                    alert(response);
                }
            }
        });
    });
</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="editpage" region="center" class="CenterForm">
        <div align="center" region="center">
            <div class="uploadContainer">
                <span id="upload" class="upload"><span>上传</span></span>                          
            </div>
            <div id="status"></div>
        </div>
    </div>
    <div region="south" border="false" class="SouthForm form-action">
      <a class="easyui-linkbutton" icon="icon-cancel" href="javascript:void(0)"
                onclick="CloseTheWin();" 
                id="btnCancel">取消</a>
    </div>
</asp:Content>
<asp:Content ID="FooterContent" ContentPlaceHolderID="FootBlock" runat="server">

</asp:Content>
