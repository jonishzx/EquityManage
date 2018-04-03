<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/AdminEdit.Master"
    Inherits="System.Web.Mvc.ViewPage" %>
<%@ Register Src="../Shared/PopupWin.ascx" TagName="PopupWin" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
   表单设计预览信息<%=ViewData["EditStatus"] %>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Script" runat="server">
<script type="text/javascript" src="<%=Url.Content("~/Scripts/Calendar/WdatePicker.js")%>"></script>

<script type="text/javascript">
    var returnValue;
    var baseurl = '<%=Url.Action("PreViewSelectType","CustomForm")%>';
    var currName;
    function PopupType(selcoltype, obj) {
        currName = $(obj);
        SetWin(510, 410, baseurl + '?SelectTypeId=' + selcoltype, '数据选择');
    }
    var model = <%=ViewData["current"]%>;
    function setJsonToForm() {
        if(model) {
            for(var p in model){
                $("[name='" + p + "']").val(model[p]);
            }
        }

        $("input[DataType='INTEGER']").numberspinner();
        $("input[DataType='FLOAT']").numberbox();
    }
    $(function () {
        setJsonToForm();
    });
</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="editpage" region="center" class="CenterForm">
        <div align="center" region="center">
            <%using (Html.BeginForm())
           { %>
                <%=TempData["ParsedFormContent"]%>
           <%} %>
           <% TempData.Keep("ParsedFormContent"); %>
        </div>
    </div>
    
    <div region="south" border="false" class="SouthForm form-action">
      <a class="easyui-linkbutton" icon="icon-ok" href="javascript:void(0)"
                onclick="SubmitForm();" 
                id="A1">测试</a>
     <a class="easyui-linkbutton" icon="icon-cancel" href="javascript:void(0)"
                onclick="window.close();" 
                id="btnCancel">关闭</a>
    </div>
    <uc2:PopupWin ID="PopupWin1" runat="server" />   
</asp:Content>
<asp:Content ID="FooterContent" ContentPlaceHolderID="FootBlock" runat="server">
</asp:Content>
