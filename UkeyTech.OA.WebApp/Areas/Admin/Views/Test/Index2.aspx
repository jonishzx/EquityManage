<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Register Src="../Shared/PopupWin.ascx" TagName="PopupWin" TagPrefix="uc2" %>
<%@ Register Src="../Widget/UserMessage.ascx" TagName="UserMessage" TagPrefix="uc1" %>
<%@ Import Namespace="UkeyTech.OA.WebApp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Index2</title>
    <%:Styles.Render("~/styles/theme/skin_0")%>
    <script type="text/javascript">
        if (typeof (Object) === "undefined") {
            window.location.reload();
        }
        var rooturl = '<%= Clover.Web.Core.Utility.ConvertAbsoulteUrl("~/") %>';
        var commonpopupurl = '<%=Url.Action("PopupSelectView","Utility")%>';
    </script>
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/jquery-1.7.2.min.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/EasyUI/jquery.easyui.min.js")%>"></script>    
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/common.min.js")%>"></script>
     <!--
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/dust-full.min.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/jquery-ui.min.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/plupload/plupload.full.min.js")%>"></script>
    
    
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/plupload/i18n/zh_CN.js")%>"></script>
    -->
    <script type="text/javascript">
        //全局属性-单元格宽/全局属性-单元格高
        var gobalgridwidth, gobalgridheight;
        var permission = { Create: true, Edit: true }
        $(document).ready(function () {
            gobalgridwidth = document.body.clientWidth * 0.99;
            gobalgridheight = document.body.clientHeight * 0.99;

            if (self.frameElement != undefined) {
                gobalgridwidth = self.frameElement.clientWidth * 0.97;
                gobalgridheight = self.frameElement.clientHeight * 0.98 - 80; //"auto";
            }

            //document.getElementById("testNum").innerHTML = fmtDecimal("-123123123.145",',');
            
        });

     
    </script>
         <script src="../../../../Scripts/jscripts/tiny_mce/tiny_mce.js" type="text/javascript"></script>
    <script type="text/javascript">
        function InitTiny() {
            tinyMCE.init({
                // General options
                mode: "textareas",
                theme: "advanced",
                plugins: "style,layer,table,advhr,advimage,advlink,inlinepopups,insertdatetime,preview,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,advlist",
                language: "zh",
                width: "350",
                height: "300",
                // Theme options
                theme_advanced_buttons1: "bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,formatselect,fontselect,fontsizeselect",
                theme_advanced_buttons2: "cut,copy,paste,pastetext,pasteword,|,search,replace,|,bullist,numlist,|,outdent,indent,blockquote,|,undo,redo,|,link,unlink,anchor,image,code,|,insertdate,inserttime,preview,|,forecolor,backcolor",
                theme_advanced_buttons3: "tablecontrols,|,hr,removeformat,visualaid,|,sub,sup,|,charmap,media,advhr,|,print,|,ltr,rtl,|,fullscreen",
                theme_advanced_buttons4: "insertlayer,moveforward,movebackward,absolute,|,styleprops,|,cite,abbr,acronym,del,ins,attribs,|,visualchars,nonbreaking,pagebreak,restoredraft",

                theme_advanced_toolbar_location: "top",
                theme_advanced_toolbar_align: "left",
                theme_advanced_statusbar_location: "bottom",
                theme_advanced_resizing: true,

                content_css: "css/content.css"
            });
        }

        function myEndRequestHandler(sender, args) {
            InitTiny();
        }

        $(document).ready(function () {
            myEndRequestHandler(null, null);
        });
        </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="testNum">
    </div>
    <div id="uploader1">
        <p>
            由于你的浏览器不支持Flash, Silverlight 及 HTML5,请使用上传按钮上传文件.</p>
    </div>
    <script type="text/javascript">
$(function(){ 
 });
    </script>
    <uc1:UserMessage ID="UserMessage1" Visible="False" runat="server" />
    <%=Clover.Core.Common.StringHelper.FmtDecimal(-1000605.4560,2)%>
    <%=Clover.Core.Common.StringHelper.FmtDecimal(-10654502)%>
    <%=Clover.Core.Common.StringHelper.GetFmtDecimal("-10654500.123123")%>
    <div>
        单据号</div>
    <div>
        <%=ViewData["BillNo"] %></div>
         <textarea id="MailBody" name="MailBody" class="form-item-text easyui-validatebox "
                            style="height: 50px;" rows="5" maxlength="500" required="true"></textarea>
    <!--
      <%Html.RenderPartial(Helper.DictComboTreePath,
                                    new ViewDataDictionary(new
                                    {
                                        ID = "Sex",
                                        DictID = "EmpMgmt",                                        
                                        Value = "",
                                        Enabled = true
                                    }));%>

                                      <%
         
            Html.RenderPartial(Helper.PopupControlPath,
                                    new ViewDataDictionary(new
                                    {
                                        IDControlName = "ID1",
                                        TextControlName = "Name1",
                                        DictID = "STCSourceBank",
                                        //TextValue = "222",
                                        Value = "A39A66FC-AF49-4D74-A09B-9CB5B4264928",
                                        Height = "200",
                                        
                                        MutilSelect = true
                                    }));%>
                                     -->
                                  
                                     <%Html.RenderPartial(Helper.AttachmentPath,
                                    new ViewDataDictionary(new
                                    {
                                        AllowEditAttachment = true,
                                        MutilUpload = true,
                                        TargetID = "894ca700-e0e4-4a36-8930-037d100ed6eb",
                                        TargetType = "Biz_FincLoanBussiness",
                                        ActionName = "GetFincLoanBusiList"
                                        
                                    }));%> 

              
    </form>
</body>
</html>
