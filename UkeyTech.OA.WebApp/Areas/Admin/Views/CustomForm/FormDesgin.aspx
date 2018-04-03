<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/AdminEdit.Master"
    Inherits="System.Web.Mvc.ViewPage<UkeyTech.WebFW.Model.Form>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    表单设计信息-<%=ViewData["EditStatus"] %>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Script" runat="server">
<script type="text/javascript" src="<%=Url.Content("~/Scripts/jscripts/tiny_mce/tiny_mce.js")%>"></script>
<link type="text/css" href="<%=Url.Content("~/Scripts/jscripts/tiny_mce/plugins/media/css/media.css")%>" rel="stylesheet" />
<script type="text/javascript">
    var formid = '<%=Model.ID%>';
    var baseurl = '<%=Url.Action("","CustomForm")%>';
    var formname = '<%=Model.FormName%>';
    function InitTiny() {
        
        tinyMCE.init({
            // General options
            mode: "textareas",
            theme: "advanced",
            plugins: "customform,pagebreak,style,layer,table,advhr,advimage,advlink,inlinepopups,insertdatetime,preview,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,advlist",
            language: "zh",
            width: "100%",
            height: "550",

            // Theme options
            theme_advanced_buttons1: "bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,formatselect,fontselect,fontsizeselect",
            theme_advanced_buttons2: "cut,copy,paste,pastetext,pasteword,|,search,replace,|,bullist,numlist,|,outdent,indent,blockquote,|,undo,redo,|,link,unlink,anchor,image,code,|,insertdate,inserttime,preview,|,forecolor,backcolor",
            theme_advanced_buttons3: "tablecontrols,|,hr,removeformat,visualaid,|,sub,sup,|,charmap,advhr,|,print,|,ltr,rtl,|,fullscreen",
            theme_advanced_buttons4: "insertlayer,moveforward,movebackward,absolute,|,styleprops,|,cite,abbr,acronym,del,ins,attribs,|,visualchars,nonbreaking,pagebreak,restoredraft,customform",

            theme_advanced_toolbar_location: "top",
            theme_advanced_toolbar_align: "left",
            theme_advanced_statusbar_location: "bottom",
            theme_advanced_resizing: true,
            extended_valid_elements: "span[*]"
          
        });
    }

    function preview() {
        window.open(baseurl + "/FormDesignPreview?id=" + formid, "_blank");
    }

    $(function () {
        InitTiny();
    });
</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editpage" region="center" class="CenterForm" style="overflow:auto;">
        <div align="center" region="north" style="padding:2px 0;">
            <h3><%=Model.FormName %>-表单设计</h3>
        </div>
        <div align="center" region="center" style="padding:5px 0;">
            <%using (Html.BeginForm())
              { %>
            <textarea id="FormDesignContent" name="FormDesignContent" style="height:150px;width:500px">
                <%= Model.FormDesignContent %>
            </textarea>
            <%} %>
        </div>
    </div>
    <div region="south" border="false" class="SouthForm form-action">
 
        <a class="easyui-linkbutton" icon="icon-print" href="#" onclick="preview();" id="A2">
            预览</a>
        <a class="easyui-linkbutton" icon="icon-ok" href="#" onclick="SubmitForm();" id="A1">
            确定</a> <a class="easyui-linkbutton" icon="icon-cancel" href="javascript:void(0)"
                onclick="CloseTheWin();" 
                id="btnCancel">取消</a>
    </div>


</asp:Content>
<asp:Content ID="FooterContent" ContentPlaceHolderID="FootBlock" runat="server">
</asp:Content>
