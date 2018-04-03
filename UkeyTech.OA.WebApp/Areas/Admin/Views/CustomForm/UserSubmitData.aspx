<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/AdminEdit.Master"
    Inherits="System.Web.Mvc.ViewPage" %>
<%@ Register Src="../Shared/PopupWin.ascx" TagName="PopupWin" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
   表单信息提交<%=ViewData["EditStatus"] %>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Script" runat="server">
<script type="text/javascript" src="<%=Url.Content("~/Scripts/Calendar/WdatePicker.js")%>"></script>

<script type="text/javascript">
    var returnValue;
    var baseurl = '<%=Url.Action("PreViewSelectType","CustomForm")%>';
    var currName,currId;
    function PopupType(selcoltype, name) {
        currName = $(name);
        
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
    function gotoList(){
         var listurl = '<%=Url.Action("AddWorkItemList","CustomForm")%>';
         window.location = listurl;
    }
  
    <%if(ViewData["UIScript"]!=null && ViewData["UIScript"].ToString().IndexOf("<script>")>=0) {%>
    //窗体脚本
    <%= ViewData["UIScript"]%>
    <%}%>
    $(function () {
        <%if(ViewData["UIScript"]!=null && ViewData["UIScript"].ToString().IndexOf("<script>")<0) {%>
        //窗体脚本
        <%= ViewData["UIScript"]%>
        <%}%>

        //UI 控制        
        var uicontrol = <%= ViewData["UIControl"]!=null && ViewData["UIControl"].ToString() != string.Empty ? ViewData["UIControl"].ToString(): "{}"%>;

        $.each(uicontrol, function(){
            var ctrls =  $("#" + this.InputName + ",input[name='" + this.InputName + "']");
            if(this.Editable != 1){
                $(ctrls).attr("readonly","readonly");
            }

            if(this.Visible != 1){
                $(ctrls).remove();
            }
        });

        setJsonToForm();
    });
</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="editpage" region="center" class="CenterForm">
        <div align="center" region="center">
         
           <div id="CustomForm">
               <%using (Html.BeginForm())
               { %>
                    <%=TempData["ParsedFormContent"]%>
               <%} %>
           </div>         
    </div>
    
    <div region="south" border="false" class="SouthForm form-action">
      <a class="easyui-linkbutton" icon="icon-ok" href="javascript:void(0)"
                onclick="SubmitForm();" 
                id="A1">提交</a>
     <a class="easyui-linkbutton" icon="icon-cancel" href="javascript:void(0)"
                onclick="if(window.parent.location.href.indexOf('Home')>0)history.go(-1);else CloseTheWin();" 
                id="btnCancel">取消</a>
    </div>
    <uc2:PopupWin ID="PopupWin1" runat="server" />   
</asp:Content>
<asp:Content ID="FooterContent" ContentPlaceHolderID="FootBlock" runat="server">
</asp:Content>
