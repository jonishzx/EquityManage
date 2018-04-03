var hiddenModuleId = null;

function init() {
    LoadModuleTree();
    LoadModuleGrid();
}

$(document).ready(function () {
    //权限获取
    LoadPageModuleFunction("Module", init);
});

function LoadModuleTree() {
    $(".treeModule").each(function() {
        var systemid = $(this).attr("id").replace("treeModule_", "");
        $(this).tree({
            url: ModuleTree + "&SystemID=" + systemid,
            onClick: function(node) {

                //当前节点是系统

                //当前节点是模块
                if ($('.treeModule').tree('isLeaf', node.target)) {
                    LoadModuleGrid(undefined, undefined, node.id);
                }
                else {
                    LoadModuleGrid(undefined, undefined, node.id);
                }
            }
        }); 
    });
}

function LoadModuleGrid(code, systemId, mid, where) {
    $('#tbModule').datagrid({
        nowrap: false,
        striped: true,
        fit: true,
        border: false,
        url: ModuleList,
        queryParams: { SystemID: systemId, ParentID: mid, CodeOrName: code },
        columns: [[
                   { field: 'ModuleCode', title: '模块代码', width: GetWidth(0.1), align: 'left' },
                   { field: 'ModuleName', title: '模块名称', width: GetWidth(0.1), align: 'left' },
                   { field: 'ViewOrd', title: '排序', width: GetWidth(0.04), align: 'left' },
                   { field: 'Status', title: '状态', width: GetWidth(0.04), align: 'center',
                       formatter: function (value, rec) {
                           return value == 1 ? "启用" : "未启用";
                       }
                   },
                   { field: 'UpdateTime', title: '修改日期', width: GetWidth(0.18), align: 'center',
                       formatter: function (value, rec) {
                           return DateHandler(value);
                       }
                   }
			]],
        pagination: true,
        pageSize: 50,
        pageList: [50,100],
        rownumbers: true,
        pageNumber: 1,
        singleSelect: true,
        toolbar: [{
            text: permission.Create ? '添加' : '',
            iconCls: permission.Create ? 'icon-add' : "null",
            handler: function () {
                SetBackFunc(ModuleAddSuccess);
                SetWin(510, 410, 'ModuleAdd?SystemID=' + systemId + "&ParentID=" + mid, '添加模块');
            }
        },
            '-',
            {
                text: permission.Edit ? '修改' : '',
                iconCls: permission.Edit ? 'icon-edit' : "null",
                handler: function () {
                    var moduleId = getGridSelection('tbModule', 'ModuleID');

                    if (moduleId != "") {
                        SetBackFunc(ModuleEditSuccess);
                        SetWin(510, 410, 'ModuleEdit?ModuleID=' + moduleId, '修改模块');
                    }
                }
            },
            '-',
            {
                text: permission.Delete ? '删除' : '',
                iconCls: permission.Delete ? 'icon-cut' : "null",
                handler: function () {
                    DeleteModules();
                }
            }],
        onClickRow: function (index, data) {
            if (index > -1) {
                if (hiddenModuleId != data.ModuleID) {
                    hiddenModuleId = data.ModuleID;
                    LoadModuleFunction();
                }
            }
        },
        onBeforeLoad: function () {
            RemoveForbidButton();
        }
    });
}

function ModuleAddSuccess() {
    LoadModuleTree();
    setTimeout("LoadModuleGrid();", 500);
    setTimeout("MsgShow('系统提示','添加成功。');", 1000);
}

function ModuleEditSuccess() {
    LoadModuleTree();
    setTimeout("LoadModuleGrid();", 500);
    setTimeout("MsgShow('系统提示','修改成功。');", 1000);
}

function DeleteModules() {
    var moduleId = getGridSelection('#tbModule', 'ModuleID');

    if (moduleId != "") {
        var funcRows = $('#tbModuleFunction').datagrid('getRows');

        if (funcRows.length > 0) {
            MsgAlert('系统提示', '需先删除关联功能才能删除模块');
            return;
        }

//        var dataRows = $('#tbModuleDataItem').datagrid('getRows');

//        if (dataRows.length > 0) {
//            MsgAlert('系统提示', '需先删除关联数据项才能删除模块');
//            return;
//        }

        $.messager.confirm('Question', '确定要删除模块?', function (r) {
            if (r) {
                $.ajax({
                    type: "POST",
                    url: DeleteModuleUrl,
                    data: { moduleIds: moduleId},
                    success: function (json) {
                        LoadModuleTree();
                        setTimeout("LoadModuleGrid();", 500);
                        setTimeout("MsgShow('系统提示','删除成功。');", 1000);

                        if (hiddenModuleId == moduleId) {
                            hiddenModuleId = null;
                        }
                    }
                });
            }
        });
    }
}

function LoadModuleFunction() {
 
    $('#tbModuleFunction').pagination.defaults.displayMsg = '';
    $('#tbModuleFunction').datagrid({
        nowrap: false,
        striped: true,
        border: false,
        fit: true,
        url: ModuleFunctionListUrl,
        queryParams:{ ModuleID: hiddenModuleId },
        frozenColumns: [[
	                { field: 'ck', checkbox: true }
				]],
        columns: [[
                    { field: 'FunctionCode', title: '功能代码', width: GetWidth(0.08), align: 'left' },
                    { field: 'FunctionName', title: '功能名称', width: GetWidth(0.08), align: 'left' }
				]],
        pageSize: 1000,
        pagination: false,
        rownumbers: true,
        pageNumber: 1,
        toolbar: [{
            text: permission.JoinFunction ? '添加' : '',
            iconCls: permission.JoinFunction ? 'icon-add' : "null",
            handler: function () {
                if (hiddenModuleId != null) {
                    OpenFunctionDialog();
                    LoadFunctionGrid();
                }
            }
        },
        '-',
        {
            text: permission.JoinFunction ? '删除' : '',
            iconCls: permission.JoinFunction ? 'icon-cut' : "null",
            handler: function () {
                if (hiddenModuleId != null) {
                    DeleteModuleFunction();
                }
            }
        }],
        onBeforeLoad: function () {
            RemoveForbidButton();
        }
    });

    var p = $('#tbModuleFunction').datagrid('getPager');
    if (p) {
        $(p).pagination({
            showPageList: false,
            showRefresh: false
        });
    }
}

function DeleteModuleFunction() {
    var functionIds = getGridSelections('#tbModuleFunction', 'FunctionID');

    if (functionIds != "") {
        $.messager.confirm('Question', '确定要删除?', function (r) {
            if (r) {
                $.ajax({
                    type: "POST",

                    url: DeleteModuleFunctionUrl,
                    data: {moduleId:hiddenModuleId,functionIds: functionIds},
                    dataType: "json",
                    success: function (json) {
                        LoadModuleFunction();
                        setTimeout("MsgShow('系统提示','删除成功。');", 500);
                    }
                });
            }
        });
    }
}

function OpenFunctionDialog() {
    var dlg = $("#dlgFunction");
    dlg.css("display", "block");
    dlg.dialog({
        closed: false,
        showType: null,
        modal: true,
        buttons: [{
            text: '保存',
            iconCls: 'icon-save',
            handler: function () {
                AddModuleFunction();
                CloseFunctionDialog();
            }
        }, {
            text: '取消',
            iconCls: 'icon-cancel',
            handler: function () {
                CloseFunctionDialog();
            }
        }]
    });
}

function CloseFunctionDialog() {
    $('#dlgFunction').dialog({ closed: true });
}

function AddModuleFunction() {
    var functionIds = getGridSelections('tbFunction', 'FunctionID');

    if (functionIds != "") {
        $.ajax({
            type: "POST",
            url: AddModuleFunctionUrl,
            data: { moduleId: hiddenModuleId, functionIds: functionIds.SlashFilter() },
            success: function(json) {
                LoadModuleFunction();
                setTimeout("MsgShow('系统提示','添加成功。');", 500);
            }
        });
    }
}

function LoadFunctionGrid(code) {
    $('#tbFunction').datagrid({
        height: 295,
        width: 635,
        nowrap: false,
        striped: true,
        border: false,
        url: NotJoinFunctionByModuleUrl,
        queryParams: { ModuleID: hiddenModuleId, CodeOrName: code },
        frozenColumns: [[
	                { field: 'ck', checkbox: true }
			]],
        columns: [[
                    { field: 'FunctionCode', title: '功能代码', width: GetWidth(0.1), align: 'left' },
                    { field: 'FunctionName', title: '功能名称', width: GetWidth(0.1), align: 'left' },
                    { field: 'FunctionTag', title: '功能类型', width: GetWidth(0.1), align: 'center' }
			]],
        pagination: false,
        pageSize: 15,
        pageList: [10, 15, 20, 30],
        pageNumber: 1,
        rownumbers: true
    });

    var p = $('#tbFunction').datagrid('getPager');
    if (p) {
        $(p).pagination({
            showPageList: false,
            showRefresh: false
        });
    }
}