<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/AdminEdit.Master"
    Inherits="System.Web.Mvc.ViewPage<UkeyTech.WebFW.Model.DictItem>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    表单类型信息-<%=ViewData["EditStatus"] %>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Script" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editpage" region="center" class="CenterForm">
        <div align="center" region="center">
            <%using (Html.BeginForm())
              { %>
            <div class="ym-form linearize-form ym-columnar zcolumn">
                <div class="ym-form-fields">
                    <div class="form-message">
                        <%=ViewData["StateText"]%></div>
                    <div class="ym-fbox-text">
                        <label class="ym-fbox-text-label w150 required imgfront">
                            代码:</label>
                        <div class="form-element">
                            <input name="Code" type="text" class="ym-fbox-text-text easyui-validatebox" value="<%=Model.Code %>"
                                maxlength="25" required="true" />
                        </div>
                        <div class="form-clear-left">
                        </div>
                        <%= Html.ValidationMessage("Code")%>
                    </div>
                    <div class="ym-fbox-text">
                        <label class="ym-fbox-text-label w150 required imgfront">
                            名称:</label>
                        <div class="form-element">
                            <input name="Name" type="text" class="ym-fbox-text-text easyui-validatebox" value="<%=Model.Name %>"
                                maxlength="25" required="true" />
                        </div>
                        <div class="form-clear-left">
                        </div>
                        <%= Html.ValidationMessage("Name")%>
                    </div>
                    <div class="ym-fbox-text">
                        <label class="ym-fbox-text-label w150 required imgfront">
                            显示:</label>
                        <div class="radioButtonList">
                            <%
                  var radioButtonList = new SelectList(new List<ListItem> {
                                    new ListItem { Text = "可见", Value="1",},
                                    new ListItem { Text = "隐藏", Value="0",}}, "Value", "Text", Model.Status);
                  var htmlAttributes = new Dictionary<string, object> {                                    
                                    { "style", "width:100px !important" }                         
                                };
                  foreach (var radiobutton in radioButtonList)
                  { %>
                            <%=Html.RadioButton("Visible", radiobutton.Value, radiobutton.Selected, htmlAttributes)%>
                            <label>
                                <%=radiobutton.Text%></label>
                            <% } %>
                            <div class="form-clear-left">
                            </div>
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
                onclick="CloseTheWin();" id="btnCancel">取消</a>
    </div>
</asp:Content>
<asp:Content ID="FooterContent" ContentPlaceHolderID="FootBlock" runat="server">
</asp:Content>
