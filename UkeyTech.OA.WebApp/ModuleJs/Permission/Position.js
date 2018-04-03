var hiddenPositionId = null, hiddenGroupId= null;

function init() {
    LoadPositionTree();
    LoadPositionGrid();
}

$(document).ready(function () {
    //权限获取
    LoadPageModuleFunction("Position", init)
});

function LoadPositionTree() {
    $('#treePosition').tree({
        url: GetGroupTree,
        onClick: function (node) {
            currentGroupId = node.id;
            LoadPositionGrid('');
        }
    });
}
var currentGroupId;

function LoadPositionGrid(code, where) {
    $('#tbPosition').treegrid({
        nowrap: false,
        striped: true,
        fit: true,
        border: false,
        url: PositionList   ,
        treeField: "PositionName",
        idField: 'PositionID',
        queryParams: { ParentID: currentGroupId, CodeOrName: code },
        frozenColumns: [[
           { field: 'PositionName', title: '岗位名称', width: 200, align: 'left' }
		]],
        columns: [[
//                    { field: 'GroupName', title: '所属部门', width: 100, align: 'left' },
//                    { field: 'ParentPositionName', title: '上级岗位', width: 100, align: 'left' },
        //                    { field: 'ParentGroupName', title: '上级岗位部门', width: 100, align: 'left'},
                    {field: 'PositionLevel', title: '岗位等级', width: 60, align: 'right' },
                    { field: 'Status', title: '状态', width: GetWidth(0.08), align: 'left',
                        formatter: function (value, rec) {
                            switch (value) {
                                case 1:
                                    return '可用';
                                default:
                                    return '无效';
                            }
                        }
                    }
			]],
           
		onBeforeLoad:function(row,param){
			if (row){
			    $(this).treegrid('options').url = PositionList + "?ParentID=" + row.PositionID;
			} else {
                $(this).treegrid('options').url = PositionList;
			}
		},
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
                SetBackFunc(PositionAddSuccess);
                SetWin(480, 320, 'PositionAdd?GroupId=' + currentGroupId, '添加岗位');
            }
        },
            '-',
            {
                text: permission.Edit ? '修改' : '',
                iconCls: permission.Edit ? 'icon-edit' : "null",
                handler: function () {
                    var PositionId = getGridSelection('#tbPosition', 'PositionID');

                    if (PositionId != "") {
                        SetBackFunc(PositionEditSuccess);
                        SetWin(480, 320, 'PositionEdit?PositionID=' + PositionId, '修改岗位');
                    }
                }
            },
            '-',
            {
                text: permission.Delete ? '删除' : '',
                iconCls: permission.Delete ? 'icon-cut' : "null",
                handler: function () {
                    DeletePositions();
                }
            }],
            onClickRow: function (data) {
            if (data.PositionID) {
                hiddenPositionId = data.PositionID;
                hiddenGroupId = data.GroupId;
                LoadPositionUser();
            }
        },
        onBeforeLoad: function () {
            RemoveForbidButton();
        }
    });
}

function PositionAddSuccess() {
    setTimeout("LoadPositionGrid();", 500);
    setTimeout("MsgShow('系统提示','添加成功。');", 1000);
}

function PositionEditSuccess() {
    setTimeout("LoadPositionGrid();", 500);
    setTimeout("MsgShow('系统提示','修改成功。');", 1000);
}

function RunBackFunc() {
    LoadPositionGrid();
}

function PositionDelSuccess() {
    setTimeout("LoadPositionGrid();", 500);
    setTimeout("MsgShow('系统提示','删除成功。');", 1000);
}

function DeletePositions() {
    var PositionId = getGridSelection('#tbPosition', 'PositionID');

    if (PositionId != "") {
        //注意，只有在单选时可以采用删除前判断
        var rows = $('#tbPositionUser').datagrid('getRows');

        if (rows.length > 0) {
            MsgAlert('系统提示', '需先删除关联用户才能删除岗位');
            return;
        }

        $.messager.confirm('Question', '确定要删除岗位?', function (r) {
            if (r) {
                $.ajax({
                    type: "POST",
                    url: DeletePositionsUrl,
                    data:{ PositionIds: PositionId  },
                    dataType: "json",
                    success: function (json) {
                        LoadPositionTree();                    
                        parseMessage("1", PositionDelSuccess);
                        if (hiddenPositionId == PositionId) {
                            hiddenPositionId = null;
                        }
                    }
                });
            }
        });
    }
}

function LoadPositionUser() {
    $('#tbPositionUser').pagination.defaults.displayMsg = '';

    $('#tbPositionUser').datagrid({
        nowrap: false,
        striped: true,
        border: false,
        fit: true,
        url: PositionUserList,
        queryParams: { PositionID: hiddenPositionId },
        frozenColumns: [[
	                { field: 'ck', checkbox: true }
				]],
        columns: [[
                    { field: 'LoginName', title: '用户代码', width: 70, align: 'left' },
                    { field: 'AdminName', title: '用户名称', width: 100, align: 'left' }
				]],
        pageSize: 15,
        pagination: true,
        rownumbers: true,
        pageNumber: 1,
//        toolbar: [{
//            text: permission.JoinPositionUser ? '添加' : '',
//            iconCls: permission.JoinPositionUser ? 'icon-add' : "null",
//            handler: function () {
//                if (hiddenPositionId != null) {
//                    OpenUserDialog();
//                    LoadUserGrid();
//                }
//            }
//        },
//        '-',
//        {
//            text: permission.JoinPositionUser ? '删除' : '',
//            iconCls: permission.JoinPositionUser ? 'icon-cut' : "null",
//            handler: function () {
//                if (hiddenPositionId != null) {
//                    DeletePositionUser();
//                }
//            }
//        }],
        onBeforeLoad: function () {
            RemoveForbidButton();
        }
    });

    var p = $('#tbPositionUser').datagrid('getPager');
    if (p) {
        $(p).pagination({
            showPageList: false,
            showRefresh: false
        });
    }
}

function DeletePositionUser() {
    var userIds = getGridSelections('#tbPositionUser', 'AdminId');

    if (userIds != "") {
        $.messager.confirm('Question', '确定要删除关联用户?', function (r) {
            if (r) {
                $.ajax({
                    type: "POST",
                    url: DeletePositionUserUrl,
                    data: { PositionId:  hiddenPositionId ,userIds: userIds.SlashFilter()},
                    dataType: "json",
                    success: function (json) {
                        LoadPositionUser();
                        setTimeout("MsgShow('系统提示','删除成功。');", 500);
                    }
                });
            }
        });
    }
}

function OpenUserDialog() {
    var dlg = $("#dlgUser");
    dlg.css("display", "block");
    dlg.dialog({
        closed: false,
        showType: null,
        modal: true,
        buttons: [{
            text: '保存',
            iconCls: 'icon-save',
            handler: function () {
                AddPositionUser();
                CloseUserDialog();
            }
        }, {
            text: '取消',
            iconCls: 'icon-cancel',
            handler: function () {
                CloseUserDialog();
            }
        }]
    });
}

function CloseUserDialog() {
    $('#dlgUser').dialog({ closed: true });
}

function AddPositionUser() {
    var userIds = getGridSelections('#tbUser', 'AdminId');

    if (userIds != "") {
        $.ajax({
            type: "POST",
            url: AddPositionUserUrl,
            data: { PositionId: hiddenPositionId,GroupID: hiddenGroupId, userIds: userIds.SlashFilter() },
            dataType: "json",
            success: function (json) {
                LoadPositionUser();
                setTimeout("MsgShow('系统提示','添加成功。');", 500);
            }
        });
    }
}

function LoadUserGrid(code) {
    $('#tbUser').datagrid({
        height: 295,
        width: 635,
        nowrap: false,
        striped: true,
        border: false,
        url: PositionNoUserRef,
        queryParams: { roupID: hiddenPositionId, CodeOrName: code },
        frozenColumns: [[
	                { field: 'ck', checkbox: true }
			]],
        columns: [[
                    { field: 'LoginName', title: '用户代码', width: GetWidth(0.12), align: 'left' },
                    { field: 'AdminName', title: '用户名称', width: GetWidth(0.08), align: 'left' }  
			]],
        pagination: true,
        pageSize: 15,
        pageList: [10, 15, 20, 30],
        pageNumber: 1,
        rownumbers: true
    });

    var p = $('#tbUser').datagrid('getPager');
    if (p) {
        $(p).pagination({
            showPageList: false,
            showRefresh: false
        });
    }
}