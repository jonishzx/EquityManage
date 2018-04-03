var hiddenGroupId = null;

function getQueryDataParams() {
    return { searchtext: $("#SearchText").val() };
}


function init() {
    window.queryData = function () {
        var stext = getQueryDataParams().searchtext;
        if (stext != "")
            LoadCustomFormTypeGrid(stext);
        else
            LoadCustomFormTypeGrid();
    }

    queryData();
}

$(document).ready(function () {
    //权限获取
    LoadPageModuleFunction("CustomFormType", init);
});

function LoadCustomFormTypeGrid(code) {
    $('#DataGrid').datagrid({
        nowrap: false,
        striped: true,
        fit: true,
        border: false,
        url: baseurl + "/GetCustomFormTypeList",
        queryParams: { CodeOrName: code },
        columns: [[
                   { field: 'ck', checkbox: true },
					{ field: 'Code', title: '编号', width: 100 },
					{ field: 'Name', title: '名称', width: 150 },
                    { field: 'Visible', title: '显示', width: 50,
                        formatter: function (value, rec) {
                            switch (value) {
                                case 1:
                                    return "可见";
                                case 0:
                                    return "隐藏";
                                default:
                                    return "隐藏";
                            };
                        }
                    },
					{ field: 'UpdateTime', title: '更新时间', width: 150,
					    formatter: function (value, rec) {
					        return DateHandler(value);
					    }
					}
			]],
        pagination: true,
        pageSize: 100,
        pageList: [100, 150, 200, 300],
        rownumbers: true,
        pageNumber: 1,
        singleSelect: true,
        toolbar: [{
            text: permission.Create ? '添加' : '',
            iconCls: permission.Create ? 'icon-add' : "null",
            handler: function () {
                SetBackFunc(AddSuccess);
                SetWin(480, 200, baseurl + '/Create', '添加' + getTitle());
            }
        },
            '-',
            {
                text: permission.Edit ? '修改' : '',
                iconCls: permission.Edit ? 'icon-edit' : "null",
                handler: function () {
                    var selId = getGridSelection('#DataGrid', 'Code');

                    if (selId != "") {
                        SetBackFunc(EditSuccess);
                        SetWin(480, 200, baseurl + '/Edit/' + selId, '修改' + getTitle());
                    }
                }
            },
            '-',
            {
                text: permission.Delete ? '删除' : '',
                iconCls: permission.Delete ? 'icon-cut' : "null",
                handler: function () {
                    var ids = getGridSelections("DataGrid", "Code");
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
    LoadCustomFormTypeGrid();
    setTimeout("MsgShow('系统提示','添加成功。');", 500);
}

function EditSuccess(func) {
    LoadCustomFormTypeGrid();
    setTimeout("MsgShow('系统提示','修改成功。');", 500);
}

function DeleteSuccess(func) {
    LoadCustomFormTypeGrid();
    setTimeout("MsgShow('系统提示','删除成功。');", 500);
}