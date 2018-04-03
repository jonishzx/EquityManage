var hiddenRoleId = null;

function init() {
    LoadRoleTree();
    LoadRoleGrid();
}

$(document).ready(function () {
    //权限获取
    LoadPageModuleFunction("Role", init);
});

function LoadRoleTree() {
    $('#treeRole').tree({
        url: GetRoleTree,
        onClick: function (node) {
            LoadRoleGrid('', node.id);
        }
    });
}

function LoadRoleGrid(code, parentId) {
    $('#tbRole').datagrid({
        nowrap: false,
        striped: true,
        fit: true,
        border: false,
        url: RoleList,
        queryParams: { ParentID: parentId, CodeOrName: code },
        columns: [[
                    { field: 'RoleCode', title: '角色代码', width: GetWidth(0.1), align: 'left' },
                    { field: 'RoleName', title: '角色名称', width: GetWidth(0.1), align: 'left' },
                    { field: 'RoleTag', title: '角色类型', width: GetWidth(0.08), align: 'center',
                        formatter: function (value, rec) {
                            return value == "System" ? '系统' : '自定义';
                        }
                    },
                    { field: 'UpdateTime', title: '修改时间', width: GetWidth(0.15), align: 'left',
                        formatter: function (value, rec) {
                            return DateHandler(value);
                        }
                    }
			]],
        pagination: true,
        pageSize: 50,
        pageList: [50,100],
        rownumbers: true,
        singleSelect: true,
        pageNumber: 1,
        toolbar: [{
            text: permission.Create ? '添加' : '',
            iconCls: permission.Create ? 'icon-add' : "null",
            handler: function () {
                SetBackFunc(RoleAddSuccess);
                SetWin(420, 350, 'RoleAdd?ParentID=' + parentId, '添加角色');
            }
        },
            '-',
            {
                text: permission.Edit ? '修改' : '',
                iconCls: permission.Edit ? 'icon-edit' : "null",
                handler: function () {
                    var roleType = getGridSelection('tbRole', 'RoleTag');

                    if (roleType == 'System') {
                        MsgAlert('系统提示', '系统内置角色不能修改');
                    }
                    else {
                        var roleId = getGridSelection('tbRole', 'RoleID');

                        if (roleId != "") {
                            SetBackFunc(RoleEditSuccess);
                            SetWin(460, 350, 'RoleEdit?RoleID=' + roleId, '修改角色');
                        }
                    }
                }
            },
            '-',
            {
                text: permission.Delete ? '删除' : '',
                iconCls: permission.Delete ? 'icon-cut' : "null",
                handler: function () {
                    var roleType = getGridSelection('tbRole', 'RoleType');
                    if (roleType == '系统') {
                        MsgAlert('系统提示', '系统内置角色不能删除');
                    }
                    else {
                        DeleteRoles();
                    }
                }
            }],
        onClickRow: function (index, data) {
            if (index > -1) {
                if (hiddenRoleId != data.RoleID) {
                    hiddenRoleId = data.RoleID;
                    LoadRoleUser();
                }
            }
        },
        onBeforeLoad: function () {
            RemoveForbidButton();
        }
    });
}

function RoleAddSuccess() {
    LoadRoleTree();
    setTimeout("LoadRoleGrid();", 500);
    setTimeout("MsgShow('系统提示','添加成功。');", 1000);
}

function RoleEditSuccess() {
    LoadRoleTree();
    setTimeout("LoadRoleGrid();", 500);
    setTimeout("MsgShow('系统提示','修改成功。');", 1000);
}

function DeleteRoles() {
    var roleId = getGridSelection('tbRole', 'RoleID');

    if (roleId != "") {
        var rows = $('#tbRoleUser').datagrid('getRows');

        if (rows.length > 0) {
            MsgAlert('系统提示', '需先删除关联用户才能删除角色');
            return;
        }

        $.messager.confirm('Question', '确定要删除角色?', function (r) {
            if (r) {
                $.ajax({
                    type: "POST",
                    url: DeleteRolesUrl,
                    data: { roleId: roleId },
                    success: function (json) {
                        LoadRoleTree();
                        setTimeout("LoadRoleGrid();", 500);
                        setTimeout("MsgShow('系统提示','删除成功。');", 1000);

                        if (hiddenRoleId == roleId) {
                            hiddenRoleId = null;
                        }
                    }
                });
            }
        });
    }
}

function LoadRoleUser() {
    $('#tbRoleUser').pagination.defaults.displayMsg = '';
    $('#tbRoleUser').datagrid({
        nowrap: false,
        striped: true,
        border: false,
        fit: true,
        url: RoleUserList,
        queryParams: { RoleID: hiddenRoleId },
        frozenColumns: [[
	                { field: 'ck', checkbox: true }
				]],
        columns: [[
                    { field: 'LoginName', title: '用户代码', width: GetWidth(0.12), align: 'left' },
                    { field: 'AdminName', title: '用户名称', width: GetWidth(0.06), align: 'left' }
				]],
        pageSize: 20,
        pagination: true,
        rownumbers: true,
        pageNumber: 1,
        toolbar: [{
            text: permission.JoinUser ? '添加' : '',
            iconCls: permission.JoinUser ? 'icon-add' : "null",
            handler: function () {
                if (hiddenRoleId != null) {
                    OpenUserDialog();
                    LoadUserGrid();
                }
            }
        },
        '-',
        {
            text: permission.JoinUser ? '删除' : '',
            iconCls: permission.JoinUser ? 'icon-cut' : "null",
            handler: function () {
                if (hiddenRoleId != null) {
                    DeleteRoleUser();
                }
            }
        }],
        onBeforeLoad: function () {
            RemoveForbidButton();
        }
    });

    var p = $('#tbRoleUser').datagrid('getPager');
    if (p) {
        $(p).pagination({
            showPageList: false,
            showRefresh: false
        });
    }
}

function DeleteRoleUser() {
    var userIds = getGridSelections('tbRoleUser', 'AdminId');

    if (userIds != "") {
        $.messager.confirm('Question', '确定要删除关联用户?', function (r) {
            if (r) {
                $.ajax({
                    type: "POST",
                    url: DeleteRoleUserUrl,
                    data: { roleId: hiddenRoleId ,userIds:userIds.SlashFilter()},
                    dataType: "json",
                    success: function (json) {
                        LoadRoleUser();
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
                AddRoleUser();
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

function AddRoleUser() {
    var userIds = getGridSelections('tbUser', 'AdminId');

    if (userIds != "") {
        $.ajax({
            type: "POST",
            url: AddRoleUserUrl,
            dataType : "html",
            data: { roleId : hiddenRoleId ,userIds:userIds.SlashFilter()},
            success: function (json) {
                LoadRoleUser();
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
        url: RoleNoUserRef,
        queryParams: {  RoleID: hiddenRoleId, CodeOrName: code },
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
        rownumbers: true,
        pageNumber: 1
    });

    var p = $('#tbUser').datagrid('getPager');
    if (p) {
        $(p).pagination({
            showPageList: false,
            showRefresh: false
        });
    }
}