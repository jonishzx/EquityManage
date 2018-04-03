<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/AdminEdit.Master"
    Inherits="System.Web.Mvc.ViewPage" %>

<%@ Register Src="../Shared/PopupWin.ascx" TagName="PopupWin" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    表单审核-<%=ViewData["EditStatus"] %>
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
        function returnList(){
            window.location.href = '<%=Url.Action("MyWorkItems", "WorkFlow") %>'
        }
        $(function () {
            setJsonToForm();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="CustomForm" region="center">
        <%=TempData["ParsedFormContent"]%>
        <% TempData.Keep("ParsedFormContent"); %>
    </div>
    <div region="south" border="false" class="SouthForm form-action" style="text-align:left;height: 350px">
           <%using (Html.BeginForm())
             { %>
        <div class="ym-form linearize-form ym-columnar">
            <div class="ym-form-fields">
                <div class="ym-fbox-text">
                    <label for="decision">
                        审核意见:</label>
                    <select name="decision">
                        <option value="Y">同意</option>
                        <option value="N">退审</option>
                    </select>
                    <a href="javasript:void(0);" onclick="showApproveInfo();">历史审核记录</a>
                </div>
                <div class="ym-fbox-text" >
                    <label for="WidgetName">
                        审核意见:</label>
                    <textarea name="comments" class="easyui-validatebox" rows="5" ></textarea>
                </div>
            </div>
            <div class="ym-fbox-buttons" style="text-align:center;">
            <a class="easyui-linkbutton" icon="icon-ok" href="#" onclick="SubmitForm();" id="A1">
                提交</a> <a class="easyui-linkbutton" icon="icon-cancel" href="javascript:void(0)"
                    onclick="returnList();" id="btnCancel">取消</a>
             </div>
        </div>
        <%} %>
    </div>
    <uc2:PopupWin ID="PopupWin1" runat="server" />
</asp:Content>
<asp:Content ID="FooterContent" ContentPlaceHolderID="FootBlock" runat="server">
</asp:Content>
