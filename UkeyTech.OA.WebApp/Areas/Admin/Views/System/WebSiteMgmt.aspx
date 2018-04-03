<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/AdminEdit.Master"
    Inherits="System.Web.Mvc.ViewPage<Clover.Config.WebSiteSetting.WebSiteConfigInfo>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    网站信息管理
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Script" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editpage" region="center">
        <div align="center" region="center">
            <%using (Html.BeginForm())
              { %>
           <div class="ym-form linearize-form ym-columnar zcolumn">
                <div class="ym-form-fields">
                <div class="ym-fbox-text">
                    <label class="ym-fbox-text-label w150 required imgfront">
                        网站名称:</label>
                    <div class="form-element">
                        <input name="WebAppName" type="text"  value="<%=Model.WebAppName %>"
                            maxlength="25" />
                        <%= Html.ValidationMessage("WebAppName")%></div>
                    <div class="form-clear-left">
                    </div>
                </div>
                <div class="ym-fbox-text ">
                    <label class="ym-fbox-text-label w150 required imgfront">
                        对外地址:</label>
                    <div class="form-element">
                        <input name="Weburl" type="text"  value="<%=Model.Weburl %>"
                            maxlength="25" />
                        <%= Html.ValidationMessage("Weburl")%></div>
                    <div class="form-clear-left">
                    </div>
                </div>
                <div class="ym-fbox-text">
                    <label class="ym-fbox-text-label w150 required imgfront">
                        所属公司:</label>
                    <div class="form-element">
                        <input name="Company" type="text"  value="<%=Model.Company%>"
                            maxlength="12" />
                        <%= Html.ValidationMessage("Company")%></div>
                    <div class="form-clear-left">
                    </div>
                </div>
                <div class="ym-fbox-text">
                    <label class="ym-fbox-text-label w150 required imgfront">
                        软件供应商:</label>
                    <div class="form-element">
                        <input name="Supplier" type="text"  value="<%=Model.Supplier%>"
                            maxlength="12" />
                        <%= Html.ValidationMessage("Supplier")%></div>
                    <div class="form-clear-left">
                    </div>
                </div>
                <div class="ym-fbox-text ">
                    <label class="ym-fbox-text-label w150 required imgfront">
                        版权声明:</label>
                    <div class="form-element">
                        <textarea name="Copyright" class="ym-fbox-text-text textarea w300"><%=Model.Copyright%></textarea>
                        <div class="form-clear-left">
                        </div>
                    </div>
                </div>
                 <div class="ym-fbox-text ">
                    <label class="ym-fbox-text-label w150 required imgfront">
                        是否启用密码策略:</label>
                    <div class="form-element">
                         <input type="checkbox" name="UsePassWordStrategy" <%=Model.UsePassWordStrategy ? "checked":"" %> />
                         <div class="form-clear-left">
                        </div>
                    </div>
                </div>
                 <div class="ym-fbox-text ">
                    <label class="ym-fbox-text-label w150 required imgfront">
                        密码更新周期(天):</label>
                    <div class="form-element">
                         <input type="text" name="ChangePasswordPeriod" value='<%=Model.ChangePasswordPeriod%>' />
                         <div class="form-clear-left">
                        </div>
                    </div>
                </div>
                <div class="ym-fbox-text ">
                    <label class="ym-fbox-text-label w150 required imgfront">
                        密码验证表达式:</label>
                    <div class="form-element">
                         <input type="text" name="PasswordRegex" value='<%=Model.PasswordRegex%>' />
                         <div class="form-clear-left">
                        </div>
                    </div>
                </div>
                <div class="ym-fbox-text ">
                    <label class="ym-fbox-text-label w150 required imgfront">
                        策略验证失败提示:</label>
                    <div class="form-element">
                         <input type="text" name="PasswordNotMatchMessage" value='<%=Model.PasswordNotMatchMessage%>' />
                         <div class="form-clear-left">
                        </div>
                    </div>
                </div>
                </div>
                <%} %>
            </div>
        </div>
    </div>
    <div region="south" border="false" class="SouthForm form-action">
        <a class="easyui-linkbutton" icon="icon-ok" href="#" onclick="SubmitForm();" id="A1">
            确定</a>
    </div>
</asp:Content>
