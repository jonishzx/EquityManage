var hiddenModuleId = null; //能为NULL或值
var hiddenOwnerTitle = null; //能为NULL或值
var hiddenOwnerValue = null; //能为NULL或值或空字符串
var hiddenFuncPermissionId = null; //当前选择的权限ID

function LoadGrid(code) {
    var title = GetSelectedTabTitle();
    return LoadRoleGrid(code);
}

function GetSelectedTabTitle() {
    return $('#tabs').tabs('getSelected').panel('options').title;
}

function GetSelectedGridKey() {
    var title = GetSelectedTabTitle();
    return getGridSelection('#tbRole', 'RoleID');
}

function GetSelectedGridKeyName() {
    var title = GetSelectedTabTitle();
    return getGridSelection('#tbRole', 'RoleName');
}
function ClonePermission() {
    var clonevalue = $("#CloneRoleValue").val();
    var roleId = getGridSelection('tbRole', 'RoleID');
    var title = GetSelectedTabTitle();
    if (!clonevalue) {
        $.messager.show({
            title: '提示',
            msg: '请选择克隆的' + title,
            timeout: 2000,
            showType: 'slide'
        });
        return;
    }
    if (roleId != "") {

        $.messager.confirm('Question', '确定要克隆权限?', function (r) {
            if (r) {
                ShowLoading('提交中,请稍候...');
                $.ajax({
                    type: "POST",
                    url: clonePermissionUrl,
                    data: { ownerTitle: title, targetOwnerValue: clonevalue, CloneRoleValue: roleId },
                    success: function (json) {
                        parseMessage(json, function () {
                            $("#dlgCloneFuncPermission").dialog("close");
                            LoadModuleGrid();

                            setTimeout("MsgShow('系统提示','克隆成功。');", 500);
                            $("#dlgCloneFuncPermission").dialog("close");
                            HideLoading();
                        });

                    }
                });
            }
        });
    }
}
function PopupClonePermission() {
    if (hiddenOwnerTitle && hiddenOwnerValue) {
        var title = GetSelectedGridKeyName();
        $("#CurrOwenerName").val(title);
        $('#dlgCloneFuncPermission').dialog({
            modal: true
        });
    }
    return;
    if (hiddenOwnerTitle && hiddenOwnerValue) {

        SetBackFunc(CloneSuccess);
        SetWin(420, 400, 'CloneFuncPermission?ownerTitle=' + encodeURIComponent(hiddenOwnerTitle)
                        + "&targetOwnerValue=" + hiddenOwnerValue
                        + "&targetOwnerValueText=" + encodeURIComponent(title), title + '权限克隆');
    }
}
function DeleteRoles() {
    var roleId = getGridSelection('tbRole', 'RoleID');

    if (roleId != "") {

        $.messager.confirm('Question', '确定要删除角色?', function (r) {
            if (r) {
                $.ajax({
                    type: "POST",
                    url: DeleteRolesUrl,
                    data: { roleId: roleId },
                    success: function (json) {
                        parseMessage(json, function () {
                            $('#tbRole').datagrid("reload");
                            setTimeout("MsgShow('系统提示','删除成功。');", 500);

                            if (hiddenOwnerValue == roleId) {
                                hiddenOwnerValue = null;
                            }
                        });

                    }
                });
            }
        });
    }
}

function LoadRoleGrid(code, name) {
    $('#tbRole').datagrid({
        nowrap: false,
        striped: true,
        fit: true,
        border: false,
        url: rolebaseurl,
        queryParams: { Code: code, Name: name },
        columns: [[
                    { field: 'RoleCode', title: '角色代码', width: GetWidth(0.1), align: 'left' },
                    { field: 'RoleName', title: '角色名称', width: GetWidth(0.1), align: 'left' }
			]],
        singleSelect: true,
        pagination: true,
        pageSize: 50,
        pageList: [50,100],
        rownumbers: true,
        toolbar: [{
            text: permission.Create ? '添加' : '',
            iconCls: permission.Create ? 'icon-add' : "null",
            handler: function () {
                SetBackFunc(RoleAddSuccess);
                SetWin(420, 350, 'RoleAdd?ParentID=' + hiddenOwnerValue, '添加角色');
            }
        },
            '-',
            {
                text: permission.Edit ? '修改' : '',
                iconCls: permission.Edit ? 'icon-edit' : "null",
                handler: function () {
                    var roleType = getGridSelection('tbRole', 'RoleTag');

                    if (roleType == 'System') {
                        MsgAlert('系统提示', '系统内置角色不能修改');
                    }
                    else {
                        var roleId = getGridSelection('tbRole', 'RoleID');

                        if (roleId != "") {
                            SetBackFunc(RoleEditSuccess);
                            SetWin(460, 350, 'RoleEdit?RoleID=' + roleId, '修改角色');
                        }
                    }
                }
            },
            '-',
            {
                text: permission.Delete ? '删除' : '',
                iconCls: permission.Delete ? 'icon-cut' : "null",
                handler: function () {
                    var roleType = getGridSelection('tbRole', 'RoleType');
                    if (roleType == '系统') {
                        MsgAlert('系统提示', '系统内置角色不能删除');
                    }
                    else {
                        DeleteRoles();
                    }
                }
            }],
        onClickRow: function (index, data) {
            if (index > -1) {
                reloadAll();
            }
        },
        onLoadSuccess: function () {
            reloadAll();
        }
    });
}

function reloadAll() {
    LoadModuleGrid();
    hiddenModuleId = -1;
    hiddenFuncPermissionId = -1;
    LoadModuleFunction();
}
function RoleAddSuccess() {
    setTimeout("LoadRoleGrid();", 500);
    setTimeout("MsgShow('系统提示','添加成功。');", 1000);
}

function RoleEditSuccess() {
    setTimeout("LoadRoleGrid();", 500);
    setTimeout("MsgShow('系统提示','修改成功。');", 1000);
}
function CloneSuccess() {
    setTimeout("LoadRoleGrid();", 500);
    setTimeout("MsgShow('系统提示','克隆权限成功。');", 1000);
}

function SetGridData(objId, field, fieldValue) {
    var rows = $("#" + objId).datagrid('getRows');

    $("#" + objId).parent().find("> .datagrid-view2 > .datagrid-body table tr").each(function (i) {
        if (fieldValue != undefined) {
            $(this).find("td[field='" + field + "'] input:enabled[type='checkbox']").attr("checked", fieldValue);
        }
        else {
            rows[i][field] = ($(this).find("td[field='" + field + "'] input:enabled[type='checkbox']").is(':checked'));
        }
    });
}
function SaveDataRule() {
    if (hiddenFuncPermissionId) {
        var rows = $('#tbModuleFunctionDataRule').datagrid('getRows');
        if (rows.length > 0) {
            var funcs = $("input[type='radio']:checked").attr("datavalue");

            $.ajax({
                type: "POST",
                url: SetDataRulePermissionurl,
                data: { DataPermissionId: (funcs ? funcs : null), FuncPermissionID: hiddenFuncPermissionId, ownerTitle: hiddenOwnerTitle.SlashFilter(), ownerValue: hiddenOwnerValue.toString().SlashFilter() },
                success: function (json) {
                    LoadModuleFunction();
                    hiddenFuncPermissionId = json.split(':')[1];
                    LoadModuleFunctionDataRule();

                    MsgShow('系统提示', '保存成功。');
                }
            });
        }
    }
}


function Save() {
    if (hiddenOwnerTitle != null && hiddenOwnerValue != null && hiddenOwnerValue != "" && hiddenModuleId != null && hiddenModuleId > 0) {
        SetGridData("tbModuleFunction", "IsAllow");
        SetGridData("tbModuleFunction", "IsDeny");
        var rows = $('#tbModuleFunction').datagrid('getRows');

        if (rows.length > 0) {
            var funcs = "";

            for (var i = 0; i < rows.length; i++) {
                if (rows[i]["IsAllow"] == true || rows[i]["IsDeny"] == true) {
                    funcs += (rows[i]["FunctionID"].toString() + JS_EASYUI_SPLIT_CHAT + rows[i]["IsAllow"].toString() + JS_EASYUI_SPLIT_CHAT + rows[i]["IsDeny"].toString() + JS_EASYUI_ADV_SPLIT_CHAT);
                }
            }

            $.ajax({
                type: "POST",
                url: SetFuncPermissionurl,
                data: { moduleId: hiddenModuleId, ownerTitle: hiddenOwnerTitle.SlashFilter(), ownerValue: hiddenOwnerValue.toString().SlashFilter(), funcs: funcs.SlashFilter() },

                success: function (json) {
                    //LoadModuleGrid();
                    //只更新某一条数据
                    var updrow = $('#tbModule').treegrid("getSelected");
                    for (var i = 0; i < rows.length; i++) {
                        if (rows[i]["IsAllow"] == true || rows[i]["IsDeny"] == true) {
                            updrow[rows[i]["FunctionID"].toString()] = ((rows[i]["IsAllow"] ? "True" : "False") + JS_EASYUI_SPLIT_CHAT + (rows[i]["IsDeny"] ? "True" : "False"));
                        }
                        else {
                            updrow[rows[i]["FunctionID"].toString()] = ("False" + JS_EASYUI_SPLIT_CHAT + "False");
                        }
                    }
                    $('#tbModule').treegrid('update', {
                        id: hiddenModuleId,
                        row: updrow
                    });

                    LoadModuleFunction();

                    MsgShow('系统提示', '保存成功。');
                }
            });
        }
    }
}

function GetRelationFunctionlist(functionId) {
    $.ajax({
        type: "POST",
        url: GetRelationFunctionListUrl,
        data: { functionID: functionId },
        dataType: "text",
        success: function (result) {
            if (result == "")
                return;

            var funclist = result.d.split(')(');
            var rows = $("#tbModuleFunction").datagrid('getRows');

            for (var i = 0; i < rows.length; i++) {
                var id = rows[i]["FunctionID"];
                var code = rows[i]["FunctionCode"];

                for (x = 0; x < funclist.length; x++) {
                    if (funclist[x].toString() == id) {
                        var divCode = $("#tbModuleFunction").parent().find("> .datagrid-view2 > .datagrid-body table tr td[field='FunctionCode'] div[innerHTML='" + code + "']");

                        if (divCode.length > 0) {
                            divCode.parent().parent().find("td[field='IsAllow'] input:enabled[type='checkbox']").attr("checked", true);
                        }

                        break;
                    }
                }
            }
        }
    });
}

function ViewReport() {
    var ownerTitle = GetSelectedTabTitle();
    var ownerName = GetSelectedGridKeyName();
    var ownerValue = GetSelectedGridKey();

    if (ownerTitle != null && ownerValue != null && ownerValue != "") {
        var ownerCode = null;

        switch (ownerTitle) {
            case "角色":
                ownerCode = "Role";
                break;
            case "组织架构":
                ownerCode = "Group";
                break;
            case "岗位":
                ownerCode = "Position";
                break;
            case "用户":
                ownerCode = "User";
                break;
        }

        SetWin(640, 480, 'FuncPermissionView?OwnerCode=' + ownerCode + '&OwnerValue=' + ownerValue, (ownerTitle + "：" + ownerName));
    }
    else {
        alert("请选择拥有者列表中的项目再点击查看报表");
    }
}