var hiddenGroupId = null;

function getQueryDataParams() {
    return { startdate: $("#StartDate").val(), enddate: $("#EndDate").val() };
}

function init() {
    window.queryData = function () {
        LoadUserCalendar();
    }
    queryData();
}

$(document).ready(function () {
    //权限获取
    LoadPageModuleFunction("WFUserCalendar", init);
});

function LoadUserCalendar(code) {
    if (permission.Browse)
        $('#DataGrid').datagrid({
            nowrap: false,
            striped: true,
            fit: true,
            border: false,
            url: baseurl + "/GetWFUserCalendarList",
            queryParams: getQueryDataParams(),
            columns: [[
					{ field: 'Name', title: '日程名称', width: 120 },
					{
					    field: 'BeginDate', title: '开始时间', width: 130, formatter: function (value, rec) {
					        return DateHandler(value);
					    }
					},
                    {
                        field: 'EndDate', title: '结束时间', width: 130, formatter: function (value, rec) {
                            return DateHandler(value);
                        }
                    },
                    {
                        field: 'CreateTime', title: '创建时间', width: 130, formatter: function (value, rec) {
                            return DateHandler(value);
                        }
                    },
                      {
                          field: 'LastUpdateTime', title: '更新时间', width: 130, formatter: function (value, rec) {
                              return DateHandler(value);
                          }
                      },
                      {
                          field: 'Comment', title: '备注', width: 200
                      },
                    {
                        field: 'State', title: '状态', width: 40,
                        formatter: function (value, rec) {
                            switch (value) {
                                case 0:
                                    return "无效";
                                case 1:
                                    return "有效";
                                case 9:
                                    return "取消";
                            };
                        }
                    }
            ]],
            pagination: true,
            pageSize: 20,
            pageList: [20, 50, 100],
            rownumbers: true,
            pageNumber: 1,
            singleSelect: true,
            toolbar: [{
                text: permission.Create ? '添加' : '',
                iconCls: permission.Create ? 'icon-add' : "null",
                handler: function () {
                    SetBackFunc(AddSuccess);
                    SetWin(640, 360, baseurl + '/Create', '添加' + getTitle());
                }
            },
            '-',
            {
                text: permission.Edit ? '修改' : '',
                iconCls: permission.Edit ? 'icon-edit' : "null",
                handler: function () {
                    var selId = getGridSelection('#DataGrid', 'Id');

                    if (selId != "") {
                        SetBackFunc(EditSuccess);
                        SetWin(640, 520, baseurl + '/Edit/' + selId, '修改' + getTitle());
                    }
                }
            },
            '-',
            {
                text: permission.Delete ? '取消' : '',
                iconCls: permission.Delete ? 'icon-cut' : "null",
                handler: function () {
                    var ids = getGridSelections("DataGrid", "Id");
                    if (ids != null && ids != '' && confirm('你确定取消选中的记录?'))
                        deleteItems({ id: ids }, baseurl + '/DisableUserCalendar', DeleteSuccess);
                }
            }],
            onBeforeLoad: function () {
                RemoveForbidButton();
            }
        });
}


function AddSuccess() {
    LoadUserCalendar();
    setTimeout("MsgShow('系统提示','添加成功。');", 500);
}

function EditSuccess(func) {
    LoadUserCalendar();
    setTimeout("MsgShow('系统提示','修改成功。');", 500);
}

function DeleteSuccess(func) {
    LoadUserCalendar();
    setTimeout("MsgShow('系统提示','设置成功。');", 500);
}