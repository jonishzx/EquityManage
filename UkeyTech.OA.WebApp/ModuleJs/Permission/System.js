function init() {
    LoadSystemGrid();
}

$(document).ready(function () {
    //权限获取
    LoadPageModuleFunction("PMSystem", init);
});

function LoadHighQueryGrid(where) {
    LoadSystemGrid("", where);
}

function LoadSystemGrid(code, where) {
    if (permission.Browse)
    $('#tbSystem').datagrid({
        nowrap: false,
        striped: true,
        fit: true,
        border: false,
        url:PMSystemList,
        queryParams: { CodeOrName: code},
        columns: [[
                    { field: 'SystemCode', title: '系统代码', width: GetWidth(0.15), align: 'left' },
                    { field: 'SystemName', title: '系统名称', width: GetWidth(0.15), align: 'left' },
                    { field: 'Descn', title: '系统说明', width: GetWidth(0.15), align: 'left' },
                    { field: 'Status', title: '系统启用', width: GetWidth(0.08), align: 'center',
                        formatter: function (value, rec) {
                            return value == "1" ? "启用" : "未启用";
                        }
                    },
                    { field: 'UpdateTime', title: '修改日期', width: GetWidth(0.2), align: 'left',
                        formatter: function (value, rec) {
                            return DateHandler(value);
                        }
                    }
			]],
        pagination: true,
        pageSize: 15,
        pageList: [10, 15, 20, 30],
        rownumbers: true,
        singleSelect: true,
        pageNumber: 1,
        toolbar: [{
            text: permission.Create ? '添加' : '',
            iconCls: permission.Create ? 'icon-add' : "null",
            handler: function () {
                SetBackFunc(SystemAddSuccess);
                SetWin(460, 300, 'PMSystemAdd', '添加系统');
            }
        },
            '-',
            {
                text: permission.Edit ? '修改' : '',
                iconCls: permission.Edit ? 'icon-edit' : "null",
                handler: function () {
                    var systemId = getGridSelection('tbSystem', 'SystemID');

                    if (systemId != "") {
                        SetBackFunc(SystemEditSuccess);
                        SetWin(460, 300, 'PMSystemEdit?SystemID=' + systemId, '修改系统');
                    }
                }
            },
            '-',
            {
                text: permission.Delete ? '删除' : '',
                iconCls: permission.Delete ? 'icon-cut' : "null",
                handler: function () {
                    DeleteSystems();
                }
            }],
        onClickRow: function (index, data) {
            if (index > -1) {
                LoadSystemModule(data.SystemID);
            }
        },
        onBeforeLoad: function () {
            RemoveForbidButton();
        }
    });

}


function SystemAddSuccess() {
    LoadSystemGrid();
    setTimeout("MsgShow('系统提示','添加成功。');", 500);
}

function SystemEditSuccess() {
    LoadSystemGrid();
    setTimeout("MsgShow('系统提示','修改成功。');", 500);
}

function DeleteSystems() {
    var systemId = getGridSelection('tbSystem', 'SystemID');

    if (systemId != "") {
          var rows = $('#treeModule li');

        if (rows.length > 0) {
            MsgAlert('系统提示', '需先删除下属模块才能删除系统');
            return;
        }

        $.messager.confirm('Question', '确定要删除系统?', function (r) {
            if (r) {
                $.ajax({
                    type: "POST",
                  
                    url: DeleteSystemsUrl,
                    data: { SystemID: systemId  },
                    dataType: "json",
                    success: function (json) {
                        LoadSystemGrid()
                        setTimeout("MsgShow('系统提示','删除成功。');", 500);
                    }
                });
            }
        });
    }
}

function LoadSystemModule(systemId) {
    $("#treeModule").tree({
        url: GetModuleTree + "&SystemID=" + systemId,
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
}