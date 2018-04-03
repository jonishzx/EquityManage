var hiddenModuleId = null; //能为NULL或值
var hiddenOwnerTitle = null; //能为NULL或值
var hiddenOwnerValue = null; //能为NULL或值或空字符串
var hiddenFuncPermissionId = null; //当前选择的权限ID

function LoadGrid(code) {
    var title = GetSelectedTabTitle();

    if (title == "用户") {
        return LoadUserGrid(code);
    } else if (title == "角色") {
        return LoadRoleGrid(code);
    } else if (title == "组织架构") {
        return LoadGroupGrid(code);
    }
    else if (title == "岗位") {
        return LoadPositionGrid(code);
    }
}

function GetSelectedTabTitle() {
    return $('#tabs').tabs('getSelected').panel('options').title;
}

function GetSelectedGridKey() {
    var title = GetSelectedTabTitle();

    if (title == "用户") {
        return getGridSelection('#tbUser', 'UniqueId');
    } else if (title == "角色") {
        return getGridSelection('#tbRole', 'RoleID');
    } else if (title == "组织架构") {
        return getGridSelection('#tbGroup', 'GroupID');
    }
    else if (title == "岗位") {
        return getGridSelection('#tbPosition', 'PositionID');
    }
}

function GetSelectedGridKeyName() {
    var title = GetSelectedTabTitle();

    if (title == "用户") {
        return getGridSelection('#tbUser', 'UserName');
    } else if (title == "角色") {
        return getGridSelection('#tbRole', 'RoleName');
    } else if (title == "组织架构") {
        return getGridSelection('#tbGroup', 'GroupName');
    } else if (title == "岗位") {
        return getGridSelection('#tbPosition', 'PositionName');
    }
}

function LoadUserGrid(code) {
   
    $('#tbUser').datagrid({
        nowrap: false,
        striped: true,
        fit: true,
        border: false,
        url: userbaseurl,
        queryParams: {CodeOrName: code },
        columns: [[
                    { field: 'LoginName', title: '用户代码', width: GetWidth(0.14), align: 'left' },
                    { field: 'AdminName', title: '用户名称', width: GetWidth(0.06), align: 'left' }
			]],
        singleSelect: true,
        pagination: true,
        pageSize: 50,
        pageList: [50,100],
        rownumbers: true,
        onClickRow: function (index, data) {
            if (index > -1) {
                LoadModuleFunction();
            }
        }
    });
}

function LoadRoleGrid(code) {
    $('#tbRole').datagrid({
        nowrap: false,
        striped: true,
        fit: true,
        border: false,
        url: rolebaseurl,
        queryParams: { CodeOrName: code },
        columns: [[
                    { field: 'RoleCode', title: '角色代码', width: GetWidth(0.1), align: 'left' },
                    { field: 'RoleName', title: '角色名称', width: GetWidth(0.1), align: 'left' }
			]],
        singleSelect: true,
        pagination: true,
        pageSize: 50,
        pageList: [50, 100],
        rownumbers: true,
        onClickRow: function (index, data) {
            if (index > -1) {
                LoadModuleFunction();
            }
        }
    });
}

function LoadGroupGrid(code) {
    $('#tbGroup').datagrid({
        nowrap: false,
        striped: true,
        fit: true,
        border: false,
        url: groupbaseurl,
        queryParams: { CodeOrName: code },
        columns: [[
                    { field: 'GroupCode', title: '组代码', width: GetWidth(0.1), align: 'left' },
                    { field: 'GroupName', title: '组名称', width: GetWidth(0.1), align: 'left' }
			]],
        singleSelect: true,
        pagination: true,
        pageSize: 15,
        pageList: [10, 15, 20, 30],
        rownumbers: true,
        onClickRow: function (index, data) {
            if (index > -1) {
                LoadModuleFunction();
            }
        }
    });
}

function LoadPositionGrid(code) {
    $('#tbPosition').datagrid({
        nowrap: false,
        striped: true,
        fit: true,
        border: false,
        url: rolebaseurl,
        queryParams: { CodeOrName: code },
        columns: [[
                    { field: 'PositionCode', title: '岗位代码', width: GetWidth(0.1), align: 'left' },
                    { field: 'PositionName', title: '岗位名称', width: GetWidth(0.1), align: 'left' }
			]],
        singleSelect: true,
        pagination: true,
        pageSize: 15,
        pageList: [10, 15, 20, 30],
        rownumbers: true,
        onClickRow: function (index, data) {
            if (index > -1) {
                LoadModuleFunction();
            }
        }
    });
}

function LoadModuleGrid(code) {
    $('#tbModule').datagrid({
        nowrap: false,
        striped: true,
        fit: true,
        border: false,
        url: modulebaseurl,
        queryParams: { CodeOrName: code },
        columns: [[
                    { field: 'ModuleCode', title: '模块代码', width: GetWidth(0.1), align: 'left' },
                    { field: 'ModuleName', title: '模块名称', width: GetWidth(0.1), align: 'left' }
			]],
        singleSelect: true,
        pagination: true,
        pageSize: 15,
        pageList: [10, 15, 20, 30],
        rownumbers: true,
        onClickRow: function (index, data) {
            if (index > -1) {
                hiddenModuleId = data.ModuleID.toString();
                LoadModuleFunction();
            }
        }
    });
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
    if (hiddenOwnerTitle != null && hiddenOwnerValue != null && hiddenOwnerValue != "" && hiddenModuleId != null) {
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