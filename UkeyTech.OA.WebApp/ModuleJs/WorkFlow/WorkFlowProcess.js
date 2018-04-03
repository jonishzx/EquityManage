var hiddenGroupId = null;

function getQueryDataParams() {
    return { searchtext: $("#SearchText").val() };
}

function init() {
    window.queryData = function () {
        var stext = getQueryDataParams().searchtext;
        if (stext != "")
            LoadWorkFlowProcessGrid(stext);
        else
            LoadWorkFlowProcessGrid();
    }

    queryData();
}

$(document).ready(function () {
    //权限获取
    LoadPageModuleFunction("WorkFlowProcess", init);
});


function UploadWorkflowProcess(id) {

    SetWin(300, 160, baseurl + '/UploadProcess/' + id, '流程定义文件上传');
    return false;
}

function LoadWorkFlowProcessGrid(code) {
    if (permission.Browse)
        $('#DataGrid').datagrid({
            nowrap: false,
            striped: true,
            fit: true,
            border: false,
            url: baseurl + "/GetWorkFlowProcessList",
            columns: [[
                   { field: 'ck', checkbox: true },
					{ field: 'Name', title: '流程代码', width: 120 },
					{ field: 'DisplayName', title: '显示名称', width: 120 },
					{ field: 'Version', title: '版本', width: 40 },
					{ field: 'Description', title: '描述', width: 120 },
                    { field: 'State', title: '已发布', width: 50,
                        formatter: function (value, rec) {
                            if (value == true)
                                return "是";
                            else
                                return "否";
                        }
                    },
					{ field: 'PublishUser', title: '修改人', width: 80 },
					{ field: 'PublishTime', title: '修改时间', width: 150,
					    formatter: function (value, rec) {
					        return DateHandler(value);
					    }
					},
					{ field: 'AAA', title: '上传', width: 60, formatter: function (val, rec) {
					    return newlinkbutton("", "上传", 'UploadWorkflowProcess("' + rec.Id + '")', "上传");
					}
					},
					{ field: 'CCC', title: '下载', width: 60, formatter: function (val, rec) {
					    return newlink("", "下载", baseurl + '/DownloadWorkflowProcess/' + rec.Id , '', "下载");
					}
					}
                     ,
					{ field: 'DDD', title: '设计流程', width: 80, formatter: function (val, rec) {
					    return newlinkbutton("", "流程设计", 'openNewWinByLink("' +
                                                baseurl + '/DesignWorkFlow/' + rec.Id + '");', 'wfdesigner', "设计");
					}
					}
			]],
            pagination: true,
            pageSize: 30,
            pageList: [20, 30, 50],
            rownumbers: true,
            pageNumber: 1,
            singleSelect: true,
            toolbar: [{
                text: permission.Create ? '添加' : '',
                iconCls: permission.Create ? 'icon-add' : "null",
                handler: function () {
                    SetBackFunc(AddSuccess);

                    SetWin(480, 300, baseurl + '/CreateProcess', '添加' + getTitle());
                }
            },
        {
            text: permission.Create ? '修改' : '',
            iconCls: permission.Create ? 'icon-edit' : "null",
            handler: function () {
                SetBackFunc(EditSuccess);
                var selId = getGridSelection('#DataGrid', 'Id');
                SetWin(480, 300, baseurl + '/EditProcess/' + selId, '修改' + getTitle());
            }
        },
            '-',
            {
                text: permission.Edit ? '上传' : '',
                iconCls: permission.Edit ? 'icon-upload' : "null",
                handler: function () {
                    SetBackFunc(EditSuccess);
                    SetWin(300, 160, baseurl + '/UploadProcess', '上传流程文件' + getTitle());
                }
            }],
            onBeforeLoad: function () {
                RemoveForbidButton();
            }
        });
}


function AddSuccess() {
    LoadWorkFlowProcessGrid();
    setTimeout("MsgShow('系统提示','添加成功。');", 500);
}

function EditSuccess(func) {
    LoadWorkFlowProcessGrid();
    setTimeout("MsgShow('系统提示','修改成功。');", 500);
}

function DeleteSuccess(func) {
    LoadWorkFlowProcessGrid();
    setTimeout("MsgShow('系统提示','删除成功。');", 500);
}