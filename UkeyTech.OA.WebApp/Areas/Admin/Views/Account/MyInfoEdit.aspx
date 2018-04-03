<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/AdminEdit.Master"
    Inherits="UkeyTech.OA.WebApp.Extenstion.EnchanceViewPage<UkeyTech.WebFW.Model.Admin>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    管理员信息-<%=ViewData["EditStatus"] %>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Script" runat="server">  
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   <div region="center" border="false" class="CenterForm" style="position: relative; left: 0; top: 0;">
            <%using (Html.BeginForm())
              { %>
            <div class="ym-form linearize-form ym-columnar zcolumn">
                <div class="form-message">
                    <%=ViewData["StateText"]%></div>
                <div class="ym-form-fields">
                    <div class="ym-fbox-text w100pct">
                        <label for="AdminName">用户名：</label>
                        <input name="AdminName" type="text" class="form-item-text easyui-validatebox ym-readonly" value="<%=Model.AdminName %>" 
                            maxlength="25" required="true" readonly="readonly" />
                        <%= Html.ValidationMessage("AdminName")%>
                    </div>
                    <div class="ym-fbox-text w100pct">
                       <label for="LoginName">登录名：</label>
                        <input name="LoginName" readonly="readonly" type="text" class="form-item-text easyui-validatebox ym-readonly" value="<%=Model.LoginName %>"
                            maxlength="25" required="true" />
                        <%= Html.ValidationMessage("LoginName")%>
                    </div>
                    <div class="ym-fbox-text w100pct">
                        <label for="Email">
                            邮件地址：</label>
                            <input name="Email" type="text" class="form-item-text easyui-validatebox" validType="email" value="<%=Model.Email %>"
                            maxlength="50" />
                        <%= Html.ValidationMessage("Email")%>
                    </div>
                    <div class="ym-fbox-text w100pct">
                        <label for="EmailPwd">
                            邮件密码：</label>
                            <input name="EmailPwd" type="password" class="form-item-text easyui-validatebox"
                                value="<%=Model.EmailPwd %>" maxlength="64" />
                            <%= Html.ValidationMessage("EmailPwd")%>
                    </div>
                    <div class="ym-fbox-text w100pct">
                        <label for="MobilePhone">
                            联系电话：</label>
                        <input name="MobilePhone"  type="text" class="form-item-text easyui-validatebox"
                                value="<%=Model.MobilePhone %>" maxlength="25" />
                            <%= Html.ValidationMessage("MobilePhone")%>
                    </div>
                    <div class="ym-fbox-text w100pct" style="<%= UkeyTech.OA.WebApp.Helper.ShowUIElement(IsEdit)%>">
                    <label for="Password">
                        密码(最长16位)：</label>
                        <input name="Password" type="password" class="form-item-text" maxlength="16" />
                        <%= Html.ValidationMessage("Password")%>
                    </div>
                    <div class="ym-fbox-text w100pct" style="<%= UkeyTech.OA.WebApp.Helper.ShowUIElement(IsEdit)%>">
                    <label for="ConfirmPwd">
                        新密码确认：</label>
                        <input name="ConfirmPwd" type="password" class="form-item-text" maxlength="16" />
                        <%= Html.ValidationMessage("ConfirmPwd")%>
                    </div>           
                    <div class="ym-fbox-text w100pct">
                    <label for="Descn">
                        备 注：</label>
                        <textarea name="Descn" class="form-item-text textarea w150"><%=Model.Descn%></textarea>
                    </div>
                </div>
            </div>
            <%} %>
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
