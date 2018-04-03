var hiddenFormId = null;

function getQueryDataParams() {
    return { searchtext: $("#SearchText").val() };
}

function init() {
    window.queryData = function () {
        var stext = getQueryDataParams().searchtext;
        if (stext != "")
            LoadFormForm(stext);
        else
            LoadFormForm();
    }

    initFormColumnGrid();
    queryData();
}

$(document).ready(function () {
    //权限获取
    LoadPageModuleFunction("CustomForm", init);
});

function LoadFormForm(code) {
    if (permission.Browse)
        $('#DataGrid').datagrid({
            nowrap: false,
            striped: true,
            fit: true,
            border: false,
            url: baseurl + "/GetCustomFormList",
            queryParams: { CodeOrName: code },
            columns: [[
                   { field: 'ck', checkbox: true },
					{ field: 'ID', title: '编号', width: 40 },
					{ field: 'FormName', title: '名称', width: 100 },
                    { field: 'FormType', title: '表单类型', width: 60 },
                    { field: 'PsyTableName', title: '物理表名', width: 200 },
                    { field: 'Status', title: '状态', width: 50,
                        formatter: function (value, rec) {
                            switch (value) {
                                case 1:
                                    return "可用";
                                case 0:
                                    return "无效";
                                default:
                                    return "隐藏";
                            };
                        }
                    },
                    { field: 'HasExits', title: '物理表', width: 50,
                        formatter: function (value, rec) {
                            switch (value) {
                                case 1:
                                    return "已生成";
                                default:
                                    return "";
                            };
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
                    SetWin(480, 500, baseurl + '/Create', '添加' + getTitle());
                }
            },
            '-',
            {
                text: permission.Edit ? '修改' : '',
                iconCls: permission.Edit ? 'icon-edit' : "null",
                handler: function () {
                    var selId = getGridSelection('#DataGrid', 'ID');

                    if (selId != "") {
                        SetBackFunc(EditSuccess);
                        SetWin(480, 500, baseurl + '/Edit/' + selId, '修改' + getTitle());
                    }
                }
            },
            '-',
            {
                text: permission.Delete ? '删除' : '',
                iconCls: permission.Delete ? 'icon-cut' : "null",
                handler: function () {
                    var ids = getGridSelections("#DataGrid", "ID");
                    if (ids != null && ids != '' && confirm('你确定删除选中的记录?'))
                        deleteItems({ delids: ids }, baseurl + '/Delete', DeleteSuccess);
                }
            }
            //,
            // '-',
            //{
            //    text: permission.Edit ? '生成数据表' : '',
            //    iconCls: permission.Edit ? 'icon-next' : "null",
            //    handler: function () {
            //        if (hiddenFormId) {
            //            CreateTable();
            //        }
            //    }
            //},
            // {
            //     text: permission.Edit ? '删除数据表' : '',
            //     iconCls: permission.Edit ? 'icon-cancel' : "null",
            //     handler: function () {
            //         if (hiddenFormId) {

            //             DeleteTable();
            //         }
            //     }
            // }
            // ,
            // {
            //     text: permission.Edit ? '表单设计' : '',
            //     iconCls: permission.Edit ? 'icon-edit' : "null",
            //     handler: function () {
            //         var selId = getGridSelection('#DataGrid', 'ID');

            //         if (selId != "") {
            //             SetBackFunc(SaveDesignSuccess);
            //             //SetWin(760, 670, baseurl + '/FormDesgin?id=' + selId, '修改' + getTitle());
            //             window.open(baseurl + '/FormDesgin?id=' + selId,"new");
            //         }
            //     }
            // }
            ],
            onBeforeLoad: function () {
                RemoveForbidButton();
            },
            onClickRow: function (index, data) {
                if (index > -1) {
                    if (hiddenFormId != data.ID.toString()) {
                        hiddenFormId = data.ID.toString();
                        LoadFormColumn();
                    }
                }
            }
        });
}

function CreateTable() {
    //新建数据表
    $.messager.confirm('Question', '确定要建立数据表?', function (r) {
        if (r) {
            $.ajax({
                type: "POST",
                url: baseurl + "/CreateTable",
                data: { FormID: hiddenFormId },
                success: function (json) {
                    parseMessage(json, LoadFormForm);
                }
            });
        }
    });
}

function DeleteTable() {
    //新建数据表
    $.messager.confirm('Question', '确定要删除数据表?数据表的内容将被清空!', function (r) {
        if (r) {
            $.ajax({
                type: "POST",
                url: baseurl + "/DeleteTable",
                data: { FormID: hiddenFormId },
                success: function (json) {
                    parseMessage(json);
                }
            });
        }
    });
}

function AddSuccess() {
    LoadFormForm();
    setTimeout("MsgShow('系统提示','添加成功。');", 500);
}

function EditSuccess(func) {
    LoadFormForm();
    setTimeout("MsgShow('系统提示','修改成功。');", 500);
}

function SaveDesignSuccess(func) {    
    setTimeout("MsgShow('系统提示','表单设计保存成功。');", 500);
}

function DeleteSuccess(func) {
    LoadFormForm();
    $('#FormColumnGrid').datagrid("load", { FormID: 0 });
    setTimeout("MsgShow('系统提示','删除成功。');", 500);
}

//**********表单字段设置*****************
function initFormColumnGrid() {
    if (!permission.ColumnView)
        return;

    $('#FormColumnGrid').datagrid({
        nowrap: false,
        striped: true,
        fit: true,
        border: false,
        url: baseurl + "/GetCustomFormColumnList",
        columns: [[
                   { field: 'ck', checkbox: true },
					{ field: 'ColCaption', title: '列标题', width: 100 },
                    { field: 'ColName', title: '列名', width: 100 },
                    { field: 'ColType', title: '列类型', width: 60 },
                    { field: 'Size', title: '长度', width: 40 },
                    { field: 'FPSize', title: '精度', width: 40 },
                    { field: 'SelectTypeId', title: '来源数据', width: 60,
                        formatter: function (value, rec) {
                            switch (value) {
                                case -1:
                                    return "";
                                default:
                                    var url = baseurl + "/PreViewSelectType" + "?SelectTypeId=" + value;
                                    return "<a href='javascript:void(0);' onclick=' SetWin(480, 360, \"" + url + "\", \"预览选择数据\");' >预览</a>";
                            };
                        }
                    },
                    { field: 'Status', title: '状态', width: 50,
                        formatter: function (value, rec) {
                            switch (value) {
                                case 1:
                                    return "可用";
                                case 0:
                                    return "无效";
                                default:
                                    return "";
                            };
                        }
                    },
                     { field: 'Required', title: '必填', width: 50,
                         formatter: function (value, rec) {
                             switch (value) {
                                 case 1:
                                     return "是";
                                 case 0:
                                     return "否";
                                 default:
                                     return "";
                             };
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
            text: permission.EditColumn ? '添加' : '',
            iconCls: permission.EditColumn ? 'icon-add' : "null",
            handler: function () {
                if (!hiddenFormId) {
                    setTimeout("MsgShow('系统提示','请点击选择表单后再添加表单字段。');", 500);
                    return;
                }
                SetBackFunc(AddColumnSuccess);
                SetWin(480, 520, baseurl + '/CreateColumn' + "?FormID=" + hiddenFormId, '添加' + getTitle());
            }
        },
            '-',
            {
                text: permission.EditColumn ? '修改' : '',
                iconCls: permission.EditColumn ? 'icon-edit' : "null",
                handler: function () {
                    var selId = getGridSelection('#FormColumnGrid', 'ID');

                    if (selId != "") {
                        SetBackFunc(EditColumnSuccess);
                        SetWin(480, 520, baseurl + '/EditColumn/' + selId, '修改' + getTitle());
                    }
                }
            },
              '-',
            {
                text: permission.EditColumn ? '删除' : '',
                iconCls: permission.Delete ? 'icon-cut' : "null",
                handler: function () {
                    var ids = getGridSelections("#FormColumnGrid", "ID");
                    if (ids != null && ids != '' && confirm('你确定删除选中的记录?'))
                        deleteItems({ delids: ids }, baseurl + '/DeleteColumn', DeleteColumnSuccess);
                }
            }
           ],
        onBeforeLoad: function () {
            RemoveForbidButton();
        }
    });
}
function LoadFormColumn() {
    if (!permission.ColumnView)
        return;

    $('#FormColumnGrid').datagrid("load", { FormID: hiddenFormId });
}

function AddColumnSuccess() {
    LoadFormColumn();
    setTimeout("MsgShow('系统提示','添加成功。');", 500);
}

function EditColumnSuccess(func) {
    LoadFormColumn();
    setTimeout("MsgShow('系统提示','修改成功。');", 500);
}

function DeleteColumnSuccess(func) {
    LoadFormColumn();
    setTimeout("MsgShow('系统提示','删除成功。');", 500);
}

//**********表单字段设置********
