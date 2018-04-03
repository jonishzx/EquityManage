<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/AdminEdit.Master"
    Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    表单浏览<%=ViewData["EditStatus"] %>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Script" runat="server">
    <script type="text/javascript">
        var model = <%=ViewData["current"]%>;
        function setJsonToForm() {
            if(model) {
                for(var p in model){
                    $("[name='" + p + "']").val(model[p]).attr("disabled","disabled");                    
                }
            }

           
            $("#CustomForm").click(function(){return false;});
            $(".easyui-combobox").combobox("disable");
        }
       
        $(function () {
            setJsonToForm();
        });            
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <div class="editpage" region="center" class="CenterForm">
        <div align="center" region="center">
           <div id="CustomForm">
               <%=TempData["ParsedFormContent"]%>
           </div>         
    </div>
    <div region="south" border="false" class="SouthForm form-action">
     <a class="easyui-linkbutton" icon="icon-cancel" href="javascript:void(0)"
                onclick="if(window.parent.location.href.indexOf('Home')>0)history.go(-1);else CloseTheWin();" 
                id="btnCancel">取消</a>
    </div>
</asp:Content>
<asp:Content ID="FooterContent" ContentPlaceHolderID="FootBlock" runat="server">
</asp:Content>
