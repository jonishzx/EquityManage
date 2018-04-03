<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/AdminEdit.Master"
    Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    修改密码
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Script" runat="server">
<script type="text/javascript">
    $(function () {
       $('#bodydiv').css({ 
            position:'absolute', 
            left: ($(window).width() - $('#bodydiv').outerWidth())/2, 
            top: ($(window).height() - $('#bodydiv').outerHeight())/2 + $(document).scrollTop()
        });

        document.onkeydown = function (evt) {
            var evt = window.event ? window.event : evt;
            if (evt.keyCode == 13) {
                SubmitForm();
            }
        }
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editpage" region="center" style="MARGIN-RIGHT: auto; MARGIN-LEFT: auto;vertical-align:middle">
        <div id="bodydiv" align="center" style="width:400px;margin:auto;vertical-align:middle;border-top:solid 1px #aabccf;border-left:solid 1px #aabccf;border-right:solid 1px #aabccf;">
            <%using (Html.BeginForm("ChangePassword", "Account", FormMethod.Post, new { @id = "Target", @name = "Target" }))
              { %>
            <div class="form-fieldset-wrap">
                <div class="form-message">
                    <%=TempData["StateText"]%></div>
                <div class="form-item" style='<%= (bool)TempData["ShowOldPassword"] ? "": "display:none;" %>'>
                    <label class="form-item-label w150 required imgfront">
                        原密码:</label>
                    <div class="form-element">
                        <input name="OldPwd" type="password" class="form-item-text" maxlength="16" />
                        <%= Html.ValidationMessage("OldPwd")%></div>
                    <div class="form-clear-left">
                    </div>
                </div>
                <div class="form-item ">
                    <label class="form-item-label w150 required imgfront">
                        新密码(最长16位):</label>
                    <div class="form-element">
                        <input name="NewPwd" type="password" class="form-item-text" maxlength="16" />
                        <%= Html.ValidationMessage("NewPwd")%></div>
                    <div class="form-clear-left">
                    </div>
                </div>
                <div class="form-item ">
                    <label class="form-item-label w150 required imgfront">
                        新密码确认:</label>
                    <div class="form-element">
                        <input name="ConfirmPwd" type="password" class="form-item-text" maxlength="16" />
                        <%= Html.ValidationMessage("ConfirmPwd")%></div>
                    <div class="form-clear-left">
                    </div>
                </div>
            </div>
            <%} %>
            <div  class="SouthForm form-action" style="border-top:solid 1px #aabccf;border-bottom:solid 1px #aabccf;">
            <a class="easyui-linkbutton" icon="icon-ok" href="#" onclick="SubmitForm();" id="A1">
                确定</a> <a class="easyui-linkbutton" icon="icon-cancel" href="javascript:void(0)"
                    onclick="CloseTheWin();" style='<%= (bool)TempData["ShowOldPassword"] ? "display:none;": "" %>'
                    id="btnCancel">取消</a>
            </div>
        </div>
         
    </div>
</asp:Content>
<asp:Content ID="FooterContent" ContentPlaceHolderID="FootBlock" runat="server">
</asp:Content>
