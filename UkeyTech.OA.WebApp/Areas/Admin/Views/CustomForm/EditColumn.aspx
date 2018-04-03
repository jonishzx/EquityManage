<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/AdminEdit.Master"
    Inherits="UkeyTech.OA.WebApp.Extenstion.EnchanceViewPage<UkeyTech.WebFW.Model.FormColumn>" %>

<%@ Register Src="../Shared/PopupWin.ascx" TagName="PopupWin" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    表单信息-<%=ViewData["EditStatus"] %>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Script" runat="server">
    <script type="text/javascript">
        function showPsyColName(flag) {
            if(isedit)
                return;

            if (flag){
                $("#ColName").attr("readonly",true);
                $("#ColName").val('<%=ViewData["AutoFreakText"]%>');
                $("#AutoCreateTableName").attr("value", "1");
                $("#ColName").removeAttr("required", "false");
            }
            else{
                $("#ColName").attr("readonly",false); 
                if(<%= Model.ID%> > 0)
                    $("#ColName").val("<%=Model.ColName %>");
                else     
                    $("#ColName").val("");
                $("#AutoCreateTableName").attr("value", "0");
                $("#ColName").attr("required", "true");
            }
        }

        //change the column dbtype
        function changeColDbType(){
            var currselVal = $("#ColType").val();
            $("#Size").attr("readonly",false);
           
            switch(currselVal){
                case "String":
                    $("#Size").val(50);
                    break;
                case "Text":
                    $("#Size").val(1000);
                    break;
                case "INTEGER":
                    $("#Size").val(4);
                    $("#Size").attr("readonly",true);
                    break;
                case "FLOAT":
                    $("#Size").val(18);
                    $("#Size").attr("readonly",true);
                    break;
                case "DATETIME":
                    $("#Size").val(8);
                    $("#Size").attr("readonly",true);
                    break;
                case "BOOLEAN":
                    $("#Size").val(1);
                    $("#Size").attr("readonly",true);
                    break;
            }
        
            $("#floatSize").css("display", currselVal == "FLOAT" ? "" : "none");          
        }

        //change the selecttype
        function changeSelectColType(){
            var currselVal = $("#SelectColType").val();
            loadSelectTypes(currselVal); 
        }

        //get the selecttype using ajax
        function loadSelectTypes(selectcoltype){
            $.ajax({
                type: "GET",
                url: "<%=Url.Action("GetSelectTypeList","CustomForm")%>" + "?selColTypeId=" + selectcoltype + "&t=" + new Date().toLocaleString(),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function(msg) {
                    $("#SelectTypeId").get(0).options.length = 0;
                    $("#SelectTypeId").get(0).options[0] = new Option("无", "-1");   
                    $.each(msg.rows, function(index, item) {
                        $("#SelectTypeId").get(0).options[$("#SelectTypeId").get(0).options.length] 
                            = new Option(item.Name, item.DictID);
                    });
                },
                error: function() {
                    alert("读取失败");
                }
            });
        }

        //preview select data
        function previewSelectType(){

            var seltypeid = $("#SelectTypeId").val();
            if(seltypeid != "" && seltypeid != '-1'){
                var url = "<%=Url.Action("PreViewSelectType","CustomForm")%>" + "?SelectTypeId=" + seltypeid;
                SetWin(360, 320, url, '预览选择数据');
            }
        }
        var isedit = <%=IsEdit?"true":"false"%>
        $(function () {
            if(!isedit)
                showPsyColName(<%= Model.AutoCreateTableName == 1 ? "true" : "false" %>);
            changeColDbType();
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
                    <label for="">
                        表单</label>
                    <%=TempData["FormName"]%>
                    <div class="form-clear-left">
                    </div>
                    <%= Html.ValidationMessage("FormID")%>
                </div>
                <div class="ym-fbox-text">
                    <label for="ColCaption">
                        字段标题：<sup class="ym-required">*</sup></label>
                    <input name="ColCaption" type="text" class="form-item-text easyui-validatebox" value="<%=Model.ColCaption %>"
                        maxlength="25" required="true" />
                    <div class="form-clear-left">
                    </div>
                    <%= Html.ValidationMessage("ColCaption")%>
                </div>
                <div class="ym-fbox-text">
                    <label for="AutoCreateTableName">
                        自动生成<br />
                        物理字段名:</label>
                    <input type="checkbox" class="checkbox" id="AutoCreateTableName" name="AutoCreateTableName"
                        value='<%=Model.AutoCreateTableName%>' <%=Model.AutoCreateTableName == 1 ? "checked" : ""%>
                        onclick="if(isedit) return false;showPsyColName(this.checked);" />
                </div>
                <div id="dvColName" class="ym-fbox-text">
                    <label for="ColName">
                        物理字段名:<sup class="ym-required">*</sup></label>
                    <input id="ColName" name="ColName" type="text" class="form-item-text easyui-validatebox" <%=(IsEdit!=null && (bool)IsEdit)?"readonly":"" %>
                        value="<%=Model.ColName %>" maxlength="25" required="true" />
                    <div class="form-clear-left">
                    </div>
                    <%= Html.ValidationMessage("ColName")%>
                </div>
                <div class="ym-fbox-text">
                    <label for="ColType">
                        字段类型:</label>
                    <select id="ColType" name="ColType" class="form-item-text easyui-validatebox" onchange="changeColDbType()"
                        required="true">
                        <%
              foreach (var m in (List<UkeyTech.WebFW.Model.DictItem>)ViewData["FormColumnType"])
              {
                        %>
                        <option value='<%=m.Code%>' <%=Model.ColType == m.Code ? "selected=selected" : "" %>>
                            <%=m.Name%></option>
                        <%}%>
                    </select>
                    <div class="form-clear-left">
                    </div>
                    <%= Html.ValidationMessage("ColType")%>
                </div>
                <div class="ym-fbox-select">
                    <label for="Size">
                        字段长度:<sup class="ym-required">*</sup></label>
                        <span class="clearCss2">
                    <input id="Size" name="Size" type="text" class="easyui-numberspinner" min="1" max="8000"
                        required="true" value="<%=Model.Size %>" maxlength="250" maxlength="300" />
                        </span>
                    <div class="form-clear-left">
                    </div>
                    <%= Html.ValidationMessage("Size")%>
                </div>
                <div id="floatSize" class="ym-fbox-text">
                    <label for="FPSize">
                        浮点位数:<sup class="ym-required">*</sup></label>
                    <span class="clearall">
                        <input name="FPSize" type="text" class="easyui-numberspinner" min="0" max="10" required="true"
                        value="<%=Model.FPSize %>" maxlength="250" maxlength="300" />
                          <div class="form-clear-left">
                        </div>
                        <%= Html.ValidationMessage("FPSize")%>
                    </span>
                  
                </div>
            </div>
            <div class="ym-fbox-text">
                    <label for="Required">
                    是必填项:</label>
                    <input type="checkbox" class="checkbox" id="Required" name="Required" value='1'
                        <%=Model.Required == 1 ? "checked" : ""%>  />
            </div>
            <div id="dvSelectColType" class="ym-fbox-text">
                <label for="SelectColType">
                    数据来源:</label>
                <span class="ym-fbox-check clearCss">
                    <select id="SelectColType" name="SelectColType" class="easyui-validatebox" onchange="changeSelectColType()">
                        <option value="-1">无</option>
                        <%
              foreach (var m in (List<UkeyTech.WebFW.Model.DictItem>)ViewData["SelectColType"])
              {
                        %>
                        <option value='<%=m.Code%>' <%=Model.SelectColType.ToString() == m.Code ? "selected=selected" : "" %>>
                            <%=m.Name%></option>
                        <%}%>
                    </select>
                    <label for="SelectTypeId">
                        来源选择:</label>
                    <select id="SelectTypeId" name="SelectTypeId" class="easyui-validatebox">
                        <option value="-1">无</option>
                        <%
              if (Model.ID != 0)
                  foreach (var m in (List<UkeyTech.WebFW.Model.Dictionary>)ViewData["SelectType"])
                  {
                        %>
                        <option value='<%=m.DictID%>' <%=Model.SelectTypeId.ToString() == m.DictID ? "selected=selected" : "" %>>
                            <%=m.Name%></option>
                        <%}%>
                    </select>
                    <a href="#" onclick="previewSelectType()">预览</a> </span>
            </div>
            <div class="ym-fbox-select">
                <label for="Status">
                    状态:</label>
                <span class="ym-fbox-check">
                    <%
                            var radioButtonList = new SelectList(new List<ListItem> {
                                    new ListItem { Text = "可用", Value="1",},
                                    new ListItem { Text = "无效", Value="0",}}, "Value", "Text", Model.Status);
                            var htmlAttributes = new Dictionary<string, object> {
                                    { "class", "radioButtonList" },                           
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
            <div class="ym-fbox-select">
                <label for="Status">
                    可操作性:</label>
                <span class="ym-fbox-check">
                     <label for="OPStatus">可见</label>
                     <input type=checkbox onclick="if(!$(this).attr('checked'))$('[name=\'OPStatus\']').attr('checked',false);" name="OPStatus" value="1" <%= Model.OPStatus >=1 ? "checked=checked" : "" %> />
                     <label for="OPStatus">可修改</label>
                     <input type=checkbox onclick="if($(this).attr('checked'))$('[name=\'OPStatus\']:lt(2)').attr('checked',true);" name="OPStatus" value="2" <%= Model.OPStatus >=2 ? "checked=checked" : "" %>/>                    
                     <label for="OPStatus">发起人需填写</label>
                     <input type=checkbox  nonclick="if($(this).attr('checked'))$('[name=\'OPStatus\']').attr('checked',true);" name="OPStatus" value="3" <%= Model.OPStatus >=3 ? "checked=checked" : "" %>/>
                     <label for="IsProcessVar">流程变量</label>
                     <input type=checkbox  name="IsProcessVar" value="1" <%= Model.IsProcessVar==1 ? "checked=checked" : "" %>/>
                </span>
                   <div class="form-clear-left">
                </div>
            </div>
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
    <uc2:PopupWin ID="PopupWin1" runat="server" />
</asp:Content>
<asp:Content ID="FooterContent" ContentPlaceHolderID="FootBlock" runat="server">
</asp:Content>
