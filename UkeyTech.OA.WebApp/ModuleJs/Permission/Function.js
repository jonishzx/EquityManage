function init() {
    LoadFunctionGrid();
}

$(document).ready(function () {
    //权限获取
    LoadPageModuleFunction("Function", init);
});

function LoadHighQueryGrid(where) {
    LoadFunctionGrid("", where);
}
function LoadFunctionGrid(code, where) {
    if (permission.Browse)
    $('#tbFunction').datagrid({
        nowrap: false,
        striped: true,
        fit: true,
        border: false,
        url: FunctionList,
        queryParams: {  CodeOrName: code},
        frozenColumns: [[
	                { field: 'ck', checkbox: true }
			]],
        columns: [[
                    { field: 'FunctionCode', title: '功能代码', width: GetWidth(0.15), align: 'left' },
                    { field: 'FunctionName', title: '功能名称', width: GetWidth(0.15), align: 'left' },
                    { field: 'FunctionTag', title: '功能类型', width: GetWidth(0.1), align: 'center' },
                    { field: 'ViewOrd', title: '排序', width: GetWidth(0.05), align: 'left' },
                    { field: 'Descn', title: '说明', width: GetWidth(0.2), align: 'left' }
                    
			]],
        pagination: true,
        pageSize: 15,
        pageList: [10, 15, 20, 30],
        rownumbers: true,
    
        toolbar: [{
            text: permission.Create ? '添加' : '',
            iconCls: permission.Create ? 'icon-add' : "null",
            handler: function () {
                SetBackFunc(FunctionAddSuccess);
                SetWin(460, 380, 'FunctionAdd', '添加功能');
            }
        },
            '-',
            {
                text: permission.Edit ? '修改' : '',
                iconCls: permission.Edit ? 'icon-edit' : "null",
                handler: function () {
                    var functionId = getGridSelection('#tbFunction', 'FunctionID');

                    if (functionId != "") {
                        SetBackFunc(FunctionEditSuccess);
                        SetWin(460, 380, 'FunctionEdit?FunctionID=' + functionId, '修改功能');
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
    LoadFunctionGrid();
    setTimeout("MsgShow('系统提示','添加成功。');", 500);
}

function FunctionEditSuccess() {
    LoadFunctionGrid();
    setTimeout("MsgShow('系统提示','修改成功。');", 500);
}

function DeleteFunctions() {
    var functionIds = getGridSelections('#tbFunction', 'FunctionID');

    if (functionIds != "") {
        $.messager.confirm('Question', '确定要删除功能?', function (r) {
            if (r) {
                $.ajax({
                    type: "POST",
                    url: DeleteFunctionsUrl,
                    data: { functionIds: functionIds },
                    dataType: "json",
                    success: function (json) {
                        LoadFunctionGrid();
                        setTimeout("MsgShow('系统提示','删除成功。');", 500);
                    }
                });
            }
        });
    }
}