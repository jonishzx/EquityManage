var hiddenModuleId = null; //能为NULL或值
var hiddenOwnerTitle = null; //能为NULL或值
var hiddenOwnerValue = null; //能为NULL或值或空字符串

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
    else if (title == "部门岗位") {
        return LoadGroupPositionGrid(code);
    }
}

function GetSelectedTabTitle() {
    return $('#tabs').tabs('getSelected').panel('options').title;
}

function GetSelectedGridKey() {
    var title = GetSelectedTabTitle();

    if (title == "用户") {
        return getGridSelections('#tbUser', 'UniqueId');
    } else if (title == "角色") {
        return getGridSelections('#tbRole', 'RoleCode');
    } else if (title == "组织架构") {
        return getGridSelections('#tbGroup', 'GroupCode');
    }
    else if (title == "岗位") {
        return getGridSelections('#tbPosition', 'PositionCode');
    }
    else if (title == "部门岗位") {
        return getGridSelections('#tbGroupPosition', 'GroupCode,PositionCode');
    }
}

function GetSelectedGridKeyName() {
    var title = GetSelectedTabTitle();

    if (title == "用户") {
        return getGridSelections('#tbUser', 'LoginName');
    } else if (title == "角色") {
        return getGridSelections('#tbRole', 'RoleName');
    } else if (title == "组织架构") {
        return getGridSelections('#tbGroup', 'GroupName');
    }
    else if (title == "岗位") {
        return getGridSelections('#tbPosition', 'PositionName');
    }
    else if (title == "部门岗位") {
        return getGridSelections('#tbGroupPosition', 'GroupName,PositionName');
    }
}

function LoadUserGrid(code) {
    $('#tbUser').datagrid({
        nowrap: false,
        striped: true,
        fit: true,
        border: false,
        url: userbaseurl,
        queryParams: {adminname: code },
        columns: [[
                    { field: 'ck', checkbox: true },
                    { field: 'LoginName', title: '用户代码', width: 150, align: 'left' },
                    { field: 'AdminName', title: '用户名称', width: 150, align: 'left' }
			]],
        pagination: true,
        pageSize:  100,
        pageList: [50, 100, 300],
        rownumbers: true,
        onClickRow: function (index, data) {
          
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
                    { field: 'ck', checkbox: true },
                    { field: 'RoleCode', title: '角色代码', width: 150, align: 'left' },
                    { field: 'RoleName', title: '角色名称', width: 150, align: 'left' }
			]],
        pagination: true,
        pageSize:  100,
        pageList: [50, 100, 300],
        rownumbers: true,
        onClickRow: function (index, data) {
         
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
                    { field: 'ck', checkbox: true },
                    { field: 'GroupCode', title: '组织代码', width: 150, align: 'left' },
                    { field: 'GroupName', title: '组织名称', width: 150, align: 'left' }
			]],
        pagination: true,
        pageSize: 100,
        pageList: [100, 200, 300],
        rownumbers: true,
        onClickRow: function (index, data) {
           
        }
    });
}

function LoadPositionGrid(code) {
    $('#tbPosition').datagrid({
        nowrap: false,
        striped: true,
        fit: true,
        border: false,
        url: postionbaseurl,
        queryParams: { CodeOrName: code },
        columns: [[
                    { field: 'ck', checkbox: true },
               
                    { field: 'PositionCode', title: '岗位代码', width: 150, align: 'left' },
                    { field: 'PositionName', title: '岗位名称', width: 150, align: 'left' }
			]],
        pagination: true,
        pageSize: 100,
        pageList: [100, 200, 300],
        rownumbers: true,
        onClickRow: function (index, data) {
            
        }
    });
}

function LoadGroupPositionGrid(code) {
    $('#tbGroupPosition').datagrid({
        nowrap: false,
        striped: true,
        fit: true,
        border: false,
        url: grouppostionbaseurl,
        queryParams: { CodeOrName: code },
        columns: [[
                    { field: 'ck', checkbox: true },
                    { field: 'GroupCode', title: '组织代码', width: 100, align: 'left' },
                    { field: 'GroupName', title: '组织名称', width: 220, align: 'left' },
                    { field: 'PositionCode', title: '岗位代码', width: 150, align: 'left' },
                    { field: 'PositionName', title: '岗位名称', width: 150, align: 'left' }
			]],
        pagination: true,
        pageSize: 100,
        pageList: [100, 200, 300],
        rownumbers: true,
        onClickRow: function (index, data) {

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