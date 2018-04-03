<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<div id="dlgGroupPositionContainer" style="display:none;">
<div id="dlgGroupPosition" title="部门岗位设置" style="width: 480px; height: 200px;">
    <table cellpadding="2">
        <tr>
            <td>
                用户：
            </td>
            <td>
                <input id="CAdminName" type="text" class="form-item-text easyui-validatebox" value=""
                    readonly="readonly" maxlength="25"  />
            </td>
        </tr>
        <tr>
            <td>
                部门：
            </td>
            <td class="addonly">

                <%
                    Html.RenderPartial(UkeyTech.OA.WebApp.Helper.PopupControlPath,
                        new ViewDataDictionary(new
                        {
                            IDControlName = "pAdminGroupIds",
                            TextControlName = "pAdminGroupName",
                            DictID = "AllEnabledGroup",
                            //Value = ViewData["AdminGroupIds"],
                            MutilSelect = false,
                            Width = "300"
                        }));%>
            </td>
        </tr>
        <tr>
            <td>
                岗位：
            </td>
            <td class="addonly">

                <%
                    Html.RenderPartial(UkeyTech.OA.WebApp.Helper.PopupControlPath,
                        new ViewDataDictionary(new
                        {
                            IDControlName = "AdminPosIds",
                            TextControlName = "AdminPosName",
                            DictID = "AllEnabledPosition",
                            //Value = ViewData["AdminPosIds"],
                            MutilSelect = false,
                            Width = "300"
                        }));%>
            </td>
        </tr>
        <tr>
            <td>
                角色：
            </td>
            <td>
                <%
                    Html.RenderPartial(UkeyTech.OA.WebApp.Helper.PopupControlPath,
                        new ViewDataDictionary(new
                        {
                            IDControlName = "pAdminRoleIds",
                            TextControlName = "pAdminRoleName",
                            DictID = "AllEnabledRole",
                            //Value = ViewData["AdminRoleIds"],
                            MutilSelect = false,
                            Width = "300"
                        }));%>
            </td>
        </tr>
    </table>
    <div region="south" border="false" class="SouthForm">
        <a class="easyui-linkbutton" icon="icon-ok" href="#" onclick="saveAdminGroupPosition();"
            id="A1">确定</a> <a class="easyui-linkbutton" icon="icon-cancel" onclick="$('#dlgGroupPosition').window('close');"
                runat="server" id="btnCancel">取消</a>
    </div>
</div>
</div>
<script type="text/javascript">
    function cleanAdminGroupPositionValues() {
        $("#pAdminGroupIds").val('');
        $("#pAdminGroupName").val('');
        $("#AdminPosIds").val('');
        $("#AdminPosName").val('');
        $("#pAdminRoleIds").val('');
        $("#pAdminRoleName").val('');
    }
    function showAdminGroupPosition(isUpdate) {
        if (isUpdate) {
            $("#dlgGroupPosition").find(".addonly").find("div.popuptool").hide();
        }
        else {
            cleanAdminGroupPositionValues();
            $("#dlgGroupPosition").find(".addonly").find("div.popuptool").show();

        }
        if (hidAdminId) {
           




            $('#dlgGroupPosition').dialog({
                modal: true
            });
        }
        else { 
            $.messager.alert('提示','请先选择一个操作员再编辑其部门岗位');
        }
    }
    function saveAdminGroupPosition() {
        ShowLoading('提交中,请稍候...');
        post('<%=Url.Action("AddUserGroupPosition","Account")%>',
        {
            adminId: hidAdminId,
            groupId: $("#pAdminGroupIds").val(),
            positionId: $("#AdminPosIds").val(),
            roleId: $("#pAdminRoleIds").val(),
            changeDefault: typeof(window.isDefaultGroupRole) != "undefined" ? window.isDefaultGroupRole : false
        },
            function (json) {
                parseMessage(json, function () {
                    $('#GroupPosition').datagrid("reload");
                    $("#dlgGroupPosition").dialog("close");
                    HideLoading();
                });
            }
        );
    }
    function removeAminGroupPosition(groupId, positionId, roleId) {
        ShowLoading('提交中,请稍候...');
        post('<%=Url.Action("DeleteUserGroupPosition","Account")%>',
        {
            adminId: hidAdminId,
            groupId: groupId,
            positionId: positionId,
            roleId: roleId
        },
            function (json) {
                parseMessage(json, function () {
                    $('#GroupPosition').datagrid("reload");
                    $("#dlgGroupPosition").dialog("close");
                    HideLoading();
                });
            }
        );
    }
   
</script>
