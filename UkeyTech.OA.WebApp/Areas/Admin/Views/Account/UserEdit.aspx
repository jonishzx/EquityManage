<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/AdminEdit.Master"
    Inherits="UkeyTech.OA.WebApp.Extenstion.EnchanceViewPage<UkeyTech.WebFW.Model.Admin>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    我的信息管理
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Script" runat="server">  
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="editpage" region="center" class="CenterForm">
        <div align="center" region="center">
            <%using (Html.BeginForm())
              { %>
            <div class="form-fieldset-wrap">
                <div class="form-message">
                    <%=ViewData["StateText"]%></div>
                <div class="form-item">
                    <label class="form-item-label w150 required imgfront">
                        用户名:</label>
                    <div class="form-element">
                        <input name="AdminName" type="text" readonly="readonly" class="form-item-text easyui-validatebox" value="<%=Model.AdminName %>" 
                            maxlength="25" required="true" />
                        <%= Html.ValidationMessage("AdminName")%></div>
                    <div class="form-clear-left">
                    </div>
                </div>
                <div class="form-item ">
                    <label class="form-item-label w150 required imgfront">
                        登录名:</label>
                    <div class="form-element">
                        <input name="LoginName" type="text" readonly="readonly" class="form-item-text easyui-validatebox" value="<%=Model.LoginName %>"
                            maxlength="25" required="true" />
                        <%= Html.ValidationMessage("LoginName")%></div>
                    <div class="form-clear-left">
                    </div>
                </div>
                  <div class="form-item ">
                    <label class="form-item-label w150 required imgfront">
                        邮件地址:</label>
                    <div class="form-element">
                        <input name="Email" type="text" class="form-item-text easyui-validatebox" validType="email" value="<%=Model.Email %>"
                            maxlength="50" required="true" />
                        <%= Html.ValidationMessage("Email")%></div>
                    <div class="form-clear-left">
                    </div>
                </div>            
                <div class="form-item ">
                    <label class="form-item-label w150 required imgfront">
                        备 注:</label>
                    <div class="form-element">
                        <textarea name="Descn" class="form-item-text textarea w150"><%=Model.Descn%></textarea>
                        <div class="form-clear-left">
                        </div>
                    </div>
                </div>
            </div>
            <%} %>
        </div>
    </div>
    <div region="south" border="false" class="SouthForm form-action">
        <a class="easyui-linkbutton" icon="icon-ok" href="#" onclick="SubmitForm();" id="A1">
            确定</a> <a class="easyui-linkbutton" icon="icon-cancel" href="javascript:void(0)"
                onclick="CloseTheWin();" 
                id="btnCancel">取消</a>
    </div>
</asp:Content>
<asp:Content ID="FooterContent" ContentPlaceHolderID="FootBlock" runat="server">

</asp:Content>
