
<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<UkeyTech.WebFW.Model.Attachment>" Title="系统附件表
编辑" MasterPageFile="~/Areas/Admin/Views/Shared/AdminEdit.Master"%>
<%@ Import Namespace="UkeyTech.OA.WebApp" %>
<%@ Register src="~/Areas/Admin/Views/Shared/Loading.ascx" tagname="Loading" tagprefix="ld" %>

<%@ Register src="~/Areas/Admin/Views/Shared/PopupWin.ascx" tagname="PopupWin" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Script" runat="server">     
     <script type="text/javascript" src="<%=Url.Content("~/Scripts/ajaxupload.3.5.js")%>"></script>
     <style type="text/css">
       .w80{width:80px !important;}
        .w120{width:120px;}
        .w100p{width:90%;float:left;}
        .w100p label,.w50p label,.w30p label{width:90px !important;}
        .w50p{width:48%;float:left;}        
        .w50p input,.w50p select {width:48% !important;}
        select {height:22px;}
        .w30p{width:25%;float:left;}
        .w30p input{ width:51% !important;}
        .clr input{width:105px !important;}
        .zcolumn{width: 49%; float: left;position:relative;left:0;top:0;}
        .uploadContainer{}
        .upload {
            background: none repeat scroll 0 0 #F2F2F2;
            border: 1px solid #CCCCCC;
            color: #3366CC;
            cursor: pointer !important;
            font-family: Arial,Helvetica,sans-serif;
            font-size: 1em;
            font-weight: bold;
            text-align: center;
            display: block;
            float:left;
            width: 80px;
            height:20px;
            padding:2px;
        }
    </style>
     <script type="text/javascript">
         $(function () {
             if ($("#Bytes").val()) {
                  $("#spFileSize").html(getFileSize($("#Bytes").val()));
             }
             //file upload
             //image file upload plugin
             var a1 = new AjaxUpload($('#upload'), {
                 action: '<%=Url.Action("UploadFile","Attachment",new {area = "Admin"})%>' + "?guid=<% = Model.TargetID %>&type=ModuleFileUpload&cfgtype=<%=Request["cfgtype"]%>&target=~/upload/<%= Model.TargetType %>/<% = Model.TargetID %>&t=" + new Date().toString(),
                 name: 'uploadfile',
                 onSubmit: function (file, ext) {

                     $('#status').text('上传中...');
                     $("#A1").hide();
                 },
                 onComplete: function (file, response) {
                     //On completion clear the status
                     $('#status').text('');
                     //Add uploaded file to list
                     var rtnMsg = response.split("##");
                     if (rtnMsg[0] === "success") {

                         $('#status').text('上传成功');

                         var fino = strToJson(rtnMsg[1]);
                         //设置文件名及标题
                         $("#Title").val(fino.filename.replace(fino.ext, ""));
                         $("#FileName").val(fino.filename);
                         $("#FilePath").val(fino.url);
                         $("#spFileSize").html(getFileSize(fino.size));
                         $("#Bytes").val(fino.size);

                     } else {
                         alert(response);
                     }
                     $("#A1").show();
                 }
             });
         });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <ld:Loading ID="Loading1" runat="server" />
    <div region="center" border="false" class="CenterForm" style="position:relative;left:0;top:0;">       
        <%using (Html.BeginForm())
          {%>
        <div class="ym-form linearize-form ym-columnar">
            <div class="ym-form-fields">
                 <div class="ym-fbox-text w100p" style="height: 30px;margin: 0 0 0 20px;">
                    <div>
                        <span id="upload" class="upload"><span>选择文件</span></span><span id="status"></span>                        
                    </div>
                 </div>
                 <div class="ym-fbox-text w100p" style="display:none">
                    <label for="FilePath">文件路径：</label>
                    <input id="FilePath" name="FilePath" type="text" readonly=readonly
                    class="form-item-text easyui-validatebox ym-readonly" value="<%= Model.FilePath %>" maxlength="2550" />                                      
                
                 </div>
                 <div class="ym-fbox-text w100p">
                    <label for="Title">附件标题：</label>
                    <input id="Title" name="Title" type="text" 
                    class="form-item-text easyui-validatebox " value="<%= Model.Title %>" maxlength="255" style="width:260px !important;" />                                      
                 </div>
                 <div class="ym-fbox-text w100p" style="display:none">
                    <label for="FileName">文件名称：</label>
                    <input id="FileName" name="FileName" type="text" 
                    class="form-item-text easyui-validatebox " value="<%= Model.FileName %>" maxlength="255" />                                      
                 </div>
                <div class="ym-fbox-text w50p" style="display:none;">
                    <label for="AttachmentID"></label>
                    <input id="AttachmentID" name="AttachmentID" type="hidden"
                    class="form-item-text easyui-validatebox easyui-numberspinner" value="<%= Model.AttachmentID %>" maxlength="4" />                                      
                 </div>
              
                <div class="ym-fbox-text w50p" style="display:none;">
                    <label for="TargetID">目标ID</label>
                    <input id="TargetID" name="TargetID" type="text" 
                    class="form-item-text easyui-validatebox " value="<%= Model.TargetID %>" maxlength="36" />                                      
                 </div>
                <div class="ym-fbox-text w50p"  style="display:none;">
                    <label for="TargetType">目标类型(表名)</label>
                    <input id="TargetType" name="TargetType" type="text" 
                    class="form-item-text easyui-validatebox " value="<%= Model.TargetType %>" maxlength="50" />                                      
                 </div>
                <div class="ym-fbox-text w50p">
                    <label for="Bytes">文件大小：</label>
                    <span id="spFileSize"></span>
                    <input id="Bytes" name="Bytes" type="text" readonly=readonly style="display: none;"
                    class="form-item-text ym-readonly ym-hideall" value="<%= Model.Bytes %>" maxlength="4" />                                     
                 </div>
           
                <div class="ym-fbox-select w100p clr ym-hideall">
                    <label for="ViewOrder">显示顺序：</label>
                    <input id="ViewOrder" name="ViewOrder" type="text" 
                    class="easyui-validatebox easyui-numberspinner" value="<%= Model.ViewOrder %>" maxlength="4" />                                      
                 </div>
                   <div class="ym-fbox-text w100p ">
                    <label for="Descn">描述：</label>
                    <textarea id="Descn" name="Descn" type="text" 
                    class="form-item-text " maxlength="250"><%= Model.Descn%></textarea>
                 </div>
               
                <div class="ym-fbox-text w50p">
                    <label for="Creator">创建人：</label>
                    <input id="Creator" name="Creator" type="text" readonly=readonly
                    class="form-item-text ym-readonly" value="<%= Model.CreatorName %>" maxlength="36" />                                      
                 </div>
                <div class="ym-fbox-text w50p">
                    <label for="UpdateTime">更新时间：</label>
                    <input id="UpdateTime" name="UpdateTime" type="text" readonly=readonly
                    class="form-item-text ym-readonly" value="<%= Model.UpdateTime %>" maxlength="10" />                                      
                 </div>
                </div>
            </div>
     
        <%}%>
    </div>
    <div region="south" border="false" class="SouthForm form-action">
        <a class="easyui-linkbutton" icon="icon-ok" href="#" onclick="SubmitForm();" id="A1">
            确定</a> <a class="easyui-linkbutton" icon="icon-cancel" href="javascript:void(0)"
                onclick="CloseTheWin();" runat="server" id="btnCancel">取消</a>
    </div>
</asp:Content>
<asp:Content ID="FooterContent" ContentPlaceHolderID="FootBlock" runat="server">
</asp:Content>

