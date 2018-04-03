<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/AdminEdit.Master"
    Inherits="System.Web.Mvc.ViewPage<UkeyTech.WebFW.Model.Widget>" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    小部件信息-<%=ViewData["EditStatus"] %>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Script" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
        <div region="center">
            <%using (Html.BeginForm())
              { %>
            <div class="ym-form linearize-form ym-columnar">
                <div class="form-message">
                    <%=ViewData["StateText"]%></div>
                <div class="ym-form-fields">
                 <div class="ym-fbox-text">
                    <label for="WidgetCode">
                        部件代码:<sup class="ym-required">*</sup></label>
                        <input name="WidgetCode" type="text" class="form-item-text easyui-validatebox" value="<%=Model.WidgetCode %>"
                            maxlength="25" required="true" />
                    <div class="form-clear-left">
                    </div>
                    <%= Html.ValidationMessage("WidgetCode")%>
                </div>
                <div class="ym-fbox-text">
                    <label for="WidgetName">
                        部件名:<sup class="ym-required">*</sup></label>
                        <input name="WidgetName" type="text" class="form-item-text easyui-validatebox" value="<%=Model.WidgetName %>"
                            maxlength="25" required="true"/>
                    <div class="form-clear-left">
                    </div>
                    <%= Html.ValidationMessage("WidgetName")%>
                </div>
                  <div class="ym-fbox-text">
                    <label for="WidgetTag">
                        所属类别:<sup class="ym-required">*</sup></label>
                         <select id="WidgetTag" name="WidgetTag" class="form-item-text easyui-validatebox"
                        required="true">
                        <%
                            var views = (Dictionary<string, string>)ViewData["WidgetViews"];
                            foreach (var key in views.Keys)
                            {
                        %>
                        <option value='<%=key%>' <%=Model.WidgetTag == key ? "selected=selected" : "" %>>
                            <%=key%></option>
                        <%}%>
                        </select>
                    <div class="form-clear-left">
                    </div>
                     <%= Html.ValidationMessage("WidgetTag")%>
                </div>
                  <div class="ym-fbox-text">
                    <label for="Target">
                        URL:<sup class="ym-required">*</sup></label>
                        <input name="Target" type="text" class="form-item-text easyui-validatebox" value="<%=Model.Target %>"
                            maxlength="100" required="true"/>
                    <div class="form-clear-left">
                    </div>
                    <%= Html.ValidationMessage("Target")%>
                </div>
             
                <div class="ym-fbox-text">
                    <label for="Parameters">
                        参数:</label>
                        <textarea name="Parameters" ><%=Model.Parameters%></textarea>
                        <div class="form-clear-left">
                        </div>
                </div>
                 <div class="ym-fbox-text">
                    <label for="UIParamters">
                        UI参数:</label>
                        <textarea name="UIParamters" ><%=Model.UIParamters%></textarea>
                        <div class="form-clear-left">
                        </div>
                </div>
                <div class="ym-fbox-text">
                    <label for="Descn">
                        备 注:</label>
                        <textarea name="Descn" ><%=Model.Descn%></textarea>
                        <div class="form-clear-left">
                        </div>
                </div>
            </div>
            <%} %>
        </div>
        </div>
    <div region="south" border="false" class="SouthForm form-action">
        <a class="easyui-linkbutton" icon="icon-ok" href="#" onclick="SubmitForm();" id="A1">
            确定</a> <a class="easyui-linkbutton" icon="icon-cancel" href="javascript:void(0)"
                onclick="CloseTheWin();" id="btnCancel">取消</a>
    </div>
</asp:Content>
<asp:Content ID="FooterContent" ContentPlaceHolderID="FootBlock" runat="server">
</asp:Content>
