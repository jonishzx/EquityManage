var hiddenGroupId = null;

function init() {
    window.queryData = function () {
        var stext = getQueryDataParams().searchtext;
        if (stext != "")
            LoadWidgetGrid(stext);
        else
            LoadWidgetGrid();
    }

    queryData();
}

function getQueryDataParams() {
    return { searchtext: $("#SearchText").val() };
}

$(document).ready(function () {
    //权限获取
    LoadPageModuleFunction("Widget", init);
});

function LoadWidgetGrid(code) {
    $('#DataGrid').datagrid({
        nowrap: false,
        striped: true,
        fit: true,
        border: false,
        url: baseurl + "/GetWidgetList",
        queryParams: { CodeOrName: code },
        columns: [[
                   { field: 'ck', checkbox: true },                    
					{ field: 'WidgetCode', title: '代码', width: 80 },
					{ field: 'WidgetName', title: '名称', width: 120 },
                    { field: 'WidgetTag', title: '视图', width: 100 },
					{ field: 'CreateTime', title: '加入时间', width: 150,
					    formatter: function (value, rec) {
					        return DateHandler(value);
					    }
					},
                   { field: 'Descn', title: '备注', width: 300 }  
			]],
        pagination: true,
        pageSize: 15,
        pageList: [10, 15, 20, 30],
        rownumbers: true,
        pageNumber: 1,
        singleSelect: true,
        toolbar: [{
            text: permission.Create ? '添加' : '',
            iconCls: permission.Create ? 'icon-add' : "null",
            handler: function () {
                SetBackFunc(AddSuccess);
                SetWin(480, 440, baseurl + '/Create', '添加' + getTitle());
            }
        },
            '-',
            {
                text: permission.Edit ? '修改' : '',
                iconCls: permission.Edit ? 'icon-edit' : "null",
                handler: function () {
                    var selId = getGridSelection('#DataGrid', 'WidgetID');

                    if (selId != "") {
                        SetBackFunc(EditSuccess);
                        SetWin(480, 440, baseurl + '/Edit/' + selId, '修改' + getTitle());
                    }
                }
            },
            '-',
            {
                text: permission.Delete ? '删除' : '',
                iconCls: permission.Delete ? 'icon-cut' : "null",
                handler: function () {
                    var ids = getGridSelections("DataGrid", "WidgetID");
                    if (ids != null && ids != '' && confirm('你确定删除选中的记录?'))
                        deleteItems({ delids: ids }, baseurl + '/Delete', DeleteSuccess);
                }
            }],
        onBeforeLoad: function () {
            RemoveForbidButton();
        }
    });
}


function AddSuccess() {
    LoadWidgetGrid();
    setTimeout("MsgShow('系统提示','添加成功。');", 500);
}

function EditSuccess(func) {
    LoadWidgetGrid();
    setTimeout("MsgShow('系统提示','修改成功。');", 500);
}

function DeleteSuccess(func) {
    LoadWidgetGrid();
    setTimeout("MsgShow('系统提示','删除成功。');", 500);
}