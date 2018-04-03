var hiddenPositionId = null;

function init() {
    LoadDictTree();
    LoadDictItemsGrid();
}

$(document).ready(function () {
    //权限获取
    LoadPageModuleFunction("Dictionary", init);
});

//#region dict op
function LoadDictTree(code) {
    $('#treeDictionary').tree({
        url: GetDictTree + "?NameOrCode=" + (code ? encodeURIComponent(code) : ""),
        onClick: function (node) {
            currentDictId = node.id
            LoadDictItemsGrid('');
        },
        onLoadSuccess: function () {
            $('#treeDictionary').tree("collapseAll");
        }
    });
}
var currentDictId;

function AddDict() {
    SetBackFunc(AddDictSuccess);
    SetWin(480, 300, baseurl + '/CreateDict', '添加' + getTitle());
}
function EditDict() {
    if (!currentDictId) {
        alert("请选择需要修改的字典项目");
        return;
    }
    SetBackFunc(EditDictSuccess);
    SetWin(480, 300, baseurl + '/EditDict/' + currentDictId, '修改' + getTitle());
}
function AddDictSuccess() {
    setTimeout("LoadDictTree();", 500);
    setTimeout("MsgShow('系统提示','添加成功。');", 1000);
}
function EditDictSuccess() {
    setTimeout("LoadDictTree();", 500);
    setTimeout("MsgShow('系统提示','修改成功。');", 1000);
}

function RefreshDict() {
    $.ajax({
        type: "POST",
        url: RefreshDictUrl,
        dataType: "text",
        success: function (json) {
            MsgShow('系统提示', '刷新成功。');
        }
    });
}
function DeleteDict() {
     
    if (currentDictId) {
        //注意，只有在单选时可以采用删除前判断
      
        $.messager.confirm('Question', '确定要删除字典信息?', function (r) {
            if (r) {
                $.ajax({
                    type: "POST",
                    url: DeleteDictUrl,
                    data:{ ID: currentDictId  },
                    dataType: "text",
                    success: function (json) {

                        setTimeout("LoadDictTree();", 200);
                        setTimeout("LoadDictItemsGrid();", 500);
                        setTimeout("MsgShow('系统提示','删除成功。');", 1000);

                        currentDictId = null;
                    }
                });
            }
        });
    }
}
//#endregion dict op

//#region dict items

//#endregion dict items

function LoadDictItemsGrid(code, where) {
    $('#tbDictionary').datagrid({
        nowrap: false,
        striped: true,
        fit: true,
        border: false,
        url: DictItemsUrl,
        idField: 'Code',
        queryParams: { ParentID: currentDictId, CodeOrName: code },
        columns: [[
                    { field: 'Code', title: '代码', width: 120, align: 'left' },
                    { field: 'Name', title: '名称', width: 120, align: 'left' },
	                { field: 'Value', title: '值', width: 120, align: 'left' },
                    { field: 'Status', title: '状态', width: 50, align: 'left',
                        formatter: function (value, rec) {
                            switch (value) {
                                case "1":
                                    return '可用';
                                default:
                                    return '无效';
                            }
                        }
                    },
                    { field: 'ViewOrder', title: '显示顺序', width: 50, align: 'left' },
                    { field: 'UpdateTime', title: '修改日期', width: GetWidth(0.12), align: 'left',
                        formatter: function (value, rec) {
                            return DateHandler(value);
                        }
                    }
			]],
        pagination: true,
        pageSize: 15,
        pageList: [10, 15, 20, 30],
        rownumbers: true,
        pageNumber: 1,
        singleSelect: true,
        toolbar: [{
            text: permission.Edit ? '添加' : '',
            iconCls: permission.Edit ? 'icon-add' : "null",
            handler: function () {
                if (!currentDictId) {
                    alert('请在左边树选择所属字典,再进行操作');
                    return;
                }
                SetBackFunc(DictItemAddSuccess);
                SetWin(480, 320, 'CreateDictItem?dictid=' + currentDictId, '添加字典明细');
            }
        },
            '-',
            {
                text: permission.Edit ? '修改' : '',
                iconCls: permission.Edit ? 'icon-edit' : "null",
                handler: function () {
                    var dictid = getGridSelection('#tbDictionary', 'DictID');
                    var code = getGridSelection('#tbDictionary', 'Code');

                    if (dictid && code) {
                        SetBackFunc(DictEditSuccess);
                        SetWin(480, 320, 'EditDictItem?DictID=' + dictid + '&Code=' + code, '修改字典项目');
                    }
                }
            },
            '-',
            {
                text: permission.Edit ? '删除' : '',
                iconCls: permission.Edit ? 'icon-cut' : "null",
                handler: function () {
                    DeleteDictItem();
                }
            }]
        ,
        onBeforeLoad: function () {
            RemoveForbidButton();
        }
    });
}

function DictItemAddSuccess() {
    setTimeout("LoadDictItemsGrid();", 500);
    setTimeout("MsgShow('系统提示','添加成功。');", 1000);
}

function DictEditSuccess() {
    setTimeout("LoadDictItemsGrid();", 500);
    setTimeout("MsgShow('系统提示','修改成功。');", 1000);
}

function DeleteDictItem() {
       var dictid = getGridSelection('#tbDictionary', 'DictID');
       var code = getGridSelection('#tbDictionary', 'Code');

    if (dictid && code) {
        //注意，只有在单选时可以采用删除前判断
      
        $.messager.confirm('Question', '确定要删除字典项目信息?', function (r) {
            if (r) {
                $.ajax({
                    type: "POST",
                    url: DeleteDictItemUrl,
                    data:{ DictID: dictid, Code:code   },
                    dataType: "text",
                    success: function (json) {
                       
                        setTimeout("LoadDictItemsGrid();", 500);
                        setTimeout("MsgShow('系统提示','删除成功。');", 1000);
                    }
                });
            }
        });
    }
}