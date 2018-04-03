<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/AdminEdit.Master"
    Inherits="UkeyTech.OA.WebApp.Extenstion.EnchanceViewPage<UkeyTech.WebFW.Model.Form>" %>
    <%@ Import Namespace="UkeyTech.OA.WebApp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    表单信息-<%=ViewData["EditStatus"] %>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Script" runat="server">
    <script type="text/javascript">
        function showPsyTableName(flag) {
            if (flag){
                $("#PsyTableName").attr("readonly",true);
                $("#PsyTableName").val('<%=ViewData["AutoFreakText"]%>');
                $("#AutoCreateTableName").attr("value", "1");
                $("#PsyTableName").removeAttr("required", "false");
            }
            else{
                $("#PsyTableName").attr("readonly",false); 
                $("#PsyTableName").val("");
                
                if(<%= Model.ID%> > 0)
                    $("#PsyTableName").val("<%=Model.PsyTableName %>");
                else     
                    $("#PsyTableName").val("");
                
                $("#AutoCreateTableName").attr("value", "0");
                $("#PsyTableName").attr("required", "true");
            }
        }

        $(function () {
            showPsyTableName(<%= Model.AutoCreateTableName == 1 ? "true" : "false" %>);
        });
    </script>
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
                    <label for="FormName">
                        表单名称:<sup class="ym-required">*</sup></label>
                    <input name="FormName" type="text" class="form-item-text easyui-validatebox" value="<%=Model.FormName %>"
                        maxlength="25" required="true" />
                    <div class="form-clear-left">
                    </div>
                    <%= Html.ValidationMessage("FormName")%>
                </div>
                <div class="ym-fbox-text" style="display:none;">
                    <label for="AutoCreateTableName">
                        自动生成表<br />单物理表名:</label>
                        <input type="checkbox" class="checkbox" id="AutoCreateTableName" name="AutoCreateTableName" value='<%=Model.AutoCreateTableName%>'
                            <%=Model.AutoCreateTableName == 1 ? "checked" : ""%> onclick="showPsyTableName(this.checked);" />
                </div>
                <div id="dvPsyTableName" class="ym-fbox-text">
                    <label for="PsyTableName">
                        物理表名称:<sup class="ym-required">*</sup></label>
                    
                    <input id="PsyTableName" name="PsyTableName" type="text" class="form-item-text easyui-validatebox"
                        value="<%=Model.PsyTableName %>" maxlength="50" required="true" />
                    <div class="form-clear-left">
                    </div>
                    <%= Html.ValidationMessage("PsyTableName")%>
                </div>
                 <div class="ym-fbox-text">
                    <label for="FormCode">
                        表单代码:<sup class="ym-required">*</sup></label>
                    <input name="FormCode" type="text" class="form-item-text easyui-validatebox" value="<%=Model.FormCode %>"
                        maxlength="25" required="true" />
                    <div class="form-clear-left">
                    </div>
                    <%= Html.ValidationMessage("FormCode")%>
                </div>
                <div class="ym-fbox-text">
                    <label for="FormType">
                        表单类型:</label>
                     <%Html.RenderPartial(Helper.DictDropDownListPath,
                                    new ViewDataDictionary(new
                                    {
                                        ID = "FormType",
                                        DictID = "CustomFormType",
                                        AddEmptyItem = true,
                                        Value = Model.FormType
                                    }));%>
                    <div class="form-clear-left">
                    </div>
                    <%= Html.ValidationMessage("FormType")%>
                </div>
                <div class="ym-fbox-text">
                     <label for="ImageUrl">
                        功能图标:</label>
                      <input id="ImageUrl" name="ImageUrl" type="text" class="form-item-text easyui-validatebox"
                        value="<%=Model.ImageUrl %>" maxlength="250" />
                </div>
                <div class="ym-fbox-text">
                     <label for="ExternalFormUrl">
                        表单地址<a onclick="alert('该项填写后自动指向特定的窗体，而不自动根据列生成窗体');">?</a>:</label>
                      <input id="ExternalFormUrl" name="ExternalFormUrl" type="text" class="form-item-text easyui-validatebox"
                        value="<%=Model.ExternalFormUrl %>" maxlength="150" />
                </div>
                  <div class="ym-fbox-text">
                     <label for="ExternalListUrl">
                        列表地址<a onclick="alert('该项填写后自动指向特定的列表窗体，而不自动根据列生成列表窗体');">?</a>:</label>
                      <input id="Text1" name="ExternalListUrl" type="text" class="form-item-text easyui-validatebox"
                        value="<%=Model.ExternalListUrl %>" maxlength="150" />
                </div>
                <div class="ym-fbox-text">
                    <label for="PROCESS_ID">
                        绑定工作流程:</label>
                    <select id="PROCESS_ID" name="PROCESS_ID" class="form-item-text easyui-validatebox">
                        <%
              foreach (var m in (List<FireWorkflow.Net.Engine.Definition.WorkflowDefinition>)ViewData["WorkFlowList"])
              {
                        %>
                        <option value='<%=m.ProcessId%>' <%=Model.PROCESS_ID  == m.ProcessId ? "selected=selected" : "" %>>
                            <%=m.DisplayName%></option>
                        <%}%>
                    </select>
                </div>
                <div class="ym-fbox-select">
                    <label for="Status">
                        状态:</label>
                    <span class="ym-fbox-check">
                    <%
              var radioButtonList = new SelectList(new List<ListItem> {
                                        new ListItem { Text = "可用", Value="1"},                                      
                                        new ListItem { Text = "无效", Value="0"}
                                    }, "Value", "Text", Model.Status);
              var htmlAttributes = new Dictionary<string, object> {
                                    { "class", "radioButtonList" }
                                };
              foreach (var radiobutton in radioButtonList)
              { %>
                     <label>
                        <%=radiobutton.Text%></label>
                    <%=Html.RadioButton("Status", radiobutton.Value, radiobutton.Selected, htmlAttributes)%>                   
                    <% } %>
                    </span>
                    <div class="form-clear-left">
                    </div>
                </div>
                                   <%if (IsEdit)
                      { %>
                    <div class="ym-fbox-text">
                        <label >
                            限定访问角色:</label>
                        <div class="form-element">
                            <%
                          Html.RenderPartial(Helper.PopupControlPath,
                                      new ViewDataDictionary(new
                                      {
                                          IDControlName = "LmtRoleIds",
                                          TextControlName = "LmtRoleName",
                                          DictID = "AllEnabledRole",
                                          Value = Model.LmtRoleIds,
                                          MutilSelect = true,
                                          Width = "150"
                                      }));%>
                            <div class="form-clear-left">
                            </div>
                        </div>
                    </div>
                    <div class="ym-fbox-text">
                        <label >
                            限定访问组织:</label>
                        <div class="form-element">
                            <%
                          Html.RenderPartial(Helper.PopupControlPath,
                                      new ViewDataDictionary(new
                                      {
                                          IDControlName = "LmtGroupIds",
                                          TextControlName = "LmtGroupName",
                                          DictID = "AllEnabledGroup",
                                          Value = Model.LmtGroupIds,
                                          MutilSelect = false,
                                          Width = "150"
                                      }));%>
                            <div class="form-clear-left">
                            </div>
                        </div>
                    </div>
                    <div class="ym-fbox-text">
                        <label >
                           限定访问岗位:</label>
                        <div class="form-element">
                            <%
                          Html.RenderPartial(Helper.PopupControlPath,
                                      new ViewDataDictionary(new
                                      {
                                          IDControlName = "LmtPosIds",
                                          TextControlName = "LmtPosName",
                                          DictID = "AllEnabledPosition",
                                          Value = Model.LmtPosIds,
                                          MutilSelect = false,
                                          Width = "150"
                                      }));%>
                            <div class="form-clear-left">
                            </div>
                        </div>
                    </div>
                    <%} %>
                <div class="ym-fbox-text">
                    <label for="Descn">
                        说明:</label>
                    <textarea name="Descn"><%=Model.Descn%></textarea>
                    <div class="form-clear-left">
                    </div>
                </div>
            </div>
            <%} %>
        </div>
        <div region="south" border="false" class="SouthForm form-action">
            <a class="easyui-linkbutton" icon="icon-ok" href="#" onclick="SubmitForm();" id="A1">
                确定</a> <a class="easyui-linkbutton" icon="icon-cancel" href="javascript:void(0)"
                    onclick="CloseTheWin();" id="btnCancel">取消</a>
        </div>
</asp:Content>
<asp:Content ID="FooterContent" ContentPlaceHolderID="FootBlock" runat="server">
</asp:Content>
