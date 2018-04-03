var hiddenGroupId = null;

function init() {
    LoadGroupTree();
    LoadGroupGrid();
}

$(document).ready(function () {
    //权限获取
    LoadPageModuleFunction("Group", init)
});

function LoadGroupTree() {
    $('#treeGroup').tree({
        url: GetGroupTree,
        onClick: function (node) {
            LoadGroupGrid('', node.id);
        }
    });
}

function LoadGroupGrid(code, parentId, where) {
    $('#tbGroup').treegrid({
        nowrap: false,
        striped: true,
        fit: true,
        border: false,
        url: GroupList,
        treeField: "GroupCode",
        idField: 'GroupID',
        queryParams: { ParentID: parentId, CodeOrName: code},
        columns: [[
                    { field: 'GroupCode', title: '代码', width: GetWidth(0.15), align: 'left' },
                    { field: 'GroupName', title: '组织架构名称', width: GetWidth(0.15), align: 'left' },
                    { field: 'FullName', title: '全名', width: GetWidth(0.2), align: 'left' }
                   // field: 'EmpName', title: '部门负责人', width: GetWidth(0.2), align: 'left' }
//                    { field: 'UpdateTime', title: '修改日期', width: GetWidth(0.2), align: 'left',
//                        formatter: function (value, rec) {
//                            return DateHandler(value);
//                        }
//                    }
			]],
        onBeforeLoad: function (row, param) {
            if (row) {
                $(this).treegrid('options').url = GroupList + "?ParentID=" + row.PositionID;
            } else {
                $(this).treegrid('options').url = GroupList;
            }
        },
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
                SetBackFunc(GroupAddSuccess);
                SetWin(480, 450, 'GroupAdd?ParentID=' + parentId, '添加组织架构');
            }
        },
            '-',
            {
                text: permission.Edit ? '修改' : '',
                iconCls: permission.Edit ? 'icon-edit' : "null",
                handler: function () {
                    var groupId = getGridSelection('#tbGroup', 'GroupID');

                    if (groupId != "") {
                        SetBackFunc(GroupEditSuccess);
                        SetWin(480, 450, 'GroupEdit?GroupID=' + groupId, '修改组织架构');
                    }
                }
            },
            '-',
            {
                text: permission.Delete ? '删除' : '',
                iconCls: permission.Delete ? 'icon-cut' : "null",
                handler: function () {
                    DeleteGroups();
                }
            }],
        onClickRow: function (data) {
            if (data.GroupID) {
                hiddenGroupId = data.GroupID;
                LoadGroupUser();
            }
        },
        onBeforeLoad: function () {
            RemoveForbidButton();
        }
    });
}

function GroupAddSuccess() {
    LoadGroupTree();
    setTimeout("LoadGroupGrid();", 500);
    setTimeout("MsgShow('系统提示','添加成功。');", 1000);
}

function GroupEditSuccess() {
    LoadGroupTree();
    setTimeout("LoadGroupGrid();", 500);
    setTimeout("MsgShow('系统提示','修改成功。');", 1000);
}

function DeleteGroups() {
    var groupId = getGridSelection('#tbGroup', 'GroupID');

    if (groupId != "") {
        //注意，只有在单选时可以采用删除前判断
        var rows = $('#tbGroupUser').datagrid('getRows');

        if (rows.length > 0) {
            MsgAlert('系统提示', '需先删除关联用户才能删除组织架构');
            return;
        }

        $.messager.confirm('Question', '确定要删除组织架构?', function (r) {
            if (r) {
                $.ajax({
                    type: "POST",
                    url: DeleteGroupsUrl,
                    data:{ groupIds: groupId  },
                    dataType: "json",
                    success: function (json) {
                        LoadGroupTree();
                        setTimeout("LoadGroupGrid();", 500);
                        setTimeout("MsgShow('系统提示','删除成功。');", 1000);

                        if (hiddenGroupId == groupId) {
                            hiddenGroupId = null;
                        }
                    }
                });
            }
        });
    }
}

function LoadGroupUser() {
    $('#tbGroupUser').pagination.defaults.displayMsg = '';
    $('#tbGroupUser').datagrid({
        nowrap: false,
        striped: true,
        border: false,
        fit: true,
        url: GroupUserList,
        queryParams: { GroupID: hiddenGroupId },
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
//            text: permission.JoinGroupUser ? '添加' : '',
//            iconCls: permission.JoinGroupUser ? 'icon-add' : "null",
//            handler: function () {
//                if (hiddenGroupId != null) {
//                    OpenUserDialog();
//                    LoadUserGrid();
//                }
//            }
//        },
//        '-',
//        {
//            text: permission.JoinGroupUser ? '删除' : '',
//            iconCls: permission.JoinGroupUser ? 'icon-cut' : "null",
//            handler: function () {
//                if (hiddenGroupId != null) {
//                    DeleteGroupUser();
//                }
//            }
//        }],
        onBeforeLoad: function () {
            RemoveForbidButton();
        }
    });

    var p = $('#tbGroupUser').datagrid('getPager');
    if (p) {
        $(p).pagination({
            showPageList: false,
            showRefresh: false
        });
    }
}

function DeleteGroupUser() {
    var userIds = getGridSelections('#tbGroupUser', 'AdminId');

    if (userIds != "") {
        $.messager.confirm('Question', '确定要删除关联用户?', function (r) {
            if (r) {
                $.ajax({
                    type: "POST",
                    url: DeleteGroupUserUrl,
                    data: { groupId:  hiddenGroupId ,userIds: userIds.SlashFilter()},
                    dataType: "json",
                    success: function (json) {
                        LoadGroupUser();
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
                AddGroupUser();
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

function AddGroupUser() {
    var userIds = getGridSelections('#tbUser', 'AdminId');

    if (userIds != "") {
        $.ajax({
            type: "POST",
            url: AddGroupUserUrl,
            data: { groupId: hiddenGroupId,userIds: userIds.SlashFilter() },
            dataType: "json",
            success: function (json) {
                LoadGroupUser();
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
        url: GroupNoUserRef,
        queryParams: { roupID: hiddenGroupId, CodeOrName: code },
        frozenColumns: [[
	                { field: 'ck', checkbox: true }
			]],
        columns: [[
                    { field: 'LoginName', title: '用户代码', width: 100, align: 'left' },
                    { field: 'AdminName', title: '用户名称', width: 120, align: 'left' }  
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