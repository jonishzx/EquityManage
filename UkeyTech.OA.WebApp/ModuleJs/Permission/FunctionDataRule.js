function init() {
    LoadFuncDataRuleGrid();
}

$(document).ready(function () {
    //权限获取
    LoadPageModuleFunction("FunctionDataRule", init);
});

function LoadHighQueryGrid(where) {
    LoadFuncDataRuleGrid("", where);
}
function LoadFuncDataRuleGrid(code, where) {
    if (permission.Browse)
        $('#tbFuncDataRule').datagrid({
            nowrap: false,
            striped: true,
            fit: true,
            border: false,
            url: DataRuleList,
            queryParams: { CodeOrName: code },
            frozenColumns: [[
	                { field: 'ck', checkbox: true }
			]],
            columns: [[
                    { field: 'Code', title: '数据权限代码', width: GetWidth(0.15), align: 'left' },
                    { field: 'Name', title: '数据权限名称', width: GetWidth(0.15), align: 'left' },
                    { field: 'Priority', title: '优先级', width: GetWidth(0.1), align: 'center' },
                    { field: 'Descn', title: '说明', width: GetWidth(0.2), align: 'left' },
                    { field: 'Status', title: '状态', width: GetWidth(0.14), align: 'left',
                        formatter: function (value, rec) {
                            return value == "1" ? "启用" : "禁用";
                        }
                    }
			]],
            pagination: true,
            rownumbers: true,
            singleSelect: true,
            toolbar: [{
                text: permission.Create ? '添加' : '',
                iconCls: permission.Create ? 'icon-add' : "null",
                handler: function () {
                    SetBackFunc(FunctionAddSuccess);
                    SetWin(460, 500, 'FuncDataRuleAdd', '添加数据权限');
                }
            },
            '-',
            {
                text: permission.Edit ? '修改' : '',
                iconCls: permission.Edit ? 'icon-edit' : "null",
                handler: function () {
                    var DataPermissionId = getGridSelection('#tbFuncDataRule', 'DataPermissionId');

                    if (DataPermissionId != "") {
                        SetBackFunc(FunctionEditSuccess);
                        SetWin(460, 500, 'FuncDataRuleEdit?id=' + DataPermissionId, '修改数据权限');
                    }
                }
            },
            '-',
            {
                text: permission.Delete ? '删除' : '',
                iconCls: permission.Delete ? 'icon-cut' : "null",
                handler: function () {
                    DeleteFunctions();
                }
            }],
            onBeforeLoad: function () {
                RemoveForbidButton();
            }
        });
}

function FunctionAddSuccess() {
    LoadFuncDataRuleGrid();
    setTimeout("MsgShow('系统提示','添加成功。');", 500);
}

function FunctionEditSuccess() {
    LoadFuncDataRuleGrid();
    setTimeout("MsgShow('系统提示','修改成功。');", 500);
}

function DeleteFunctions() {
    var DataPermissionIds = getGridSelections('#tbFuncDataRule', 'DataPermissionId');

    if (DataPermissionIds != "") {
        $.messager.confirm('Question', '确定要删除数据权限?', function (r) {
            if (r) {
                $.ajax({
                    type: "POST",
                    url: DeleteUrl,
                    data: { id: DataPermissionIds },
                    dataType: "json",
                    success: function (json) {
                        LoadFuncDataRuleGrid();
                        setTimeout("MsgShow('系统提示','删除成功。');", 500);
                    }
                });
            }
        });
    }
}