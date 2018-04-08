var hiddenGroupId = null;

function getQueryDataParams() {
    return { loginid: $("#txtLoginId").val(), adminname: $("#txtName").val(),GroupId: hiddGroupId };
}
var hiddGroupId;

function searchAllGroup() {
    hiddGroupId = '';
    $('#treeGroup').find(".tree-node-selected").removeClass("tree-node-selected");
    $("#dvAllGroup").addClass("tree-node-selected");
    
    queryData();
}

function loadGroupPosition() { 
    $("#GroupPosition").datagrid({
        nowrap: false,
        striped: true,
        fit: true,
        border: false,
        url: GroupPositionUrl,
        queryParams: { adminid: hidAdminId },
        columns: [[
					{ field: 'GroupName', title: '部门', width: 80 },
					{ field: 'PositionName', title: '岗位', width: 80 },
                    { field: 'RoleName', title: '角色', width: 40 },
                    { field: 'Status', title: '默认', width: 50, align: "center",
                        formatter: function (value, rec) {
                            var isSelected = rec.CurrGroupId == (rec.GroupId ? rec.GroupId : "") && rec.CurrPositionId == (rec.PositionId ? rec.PositionId : "");
                            return '<a groupid="'
                            + (rec.GroupId ? rec.GroupId : "")
                            + '" posid="' + (rec.PositionId ? rec.PositionId : "")
                           + '" groupname="' + (rec.GroupName ? rec.GroupName : "")
                           + '" posname="' + (rec.PositionName ? rec.PositionName : "")
                           + '" roleid="' + (rec.RoleId ? rec.RoleId : "")
                           + '" rolename="' + (rec.RoleName ? rec.RoleName : "")
                           + '" class="setDefaultGroupPos icon ' + (isSelected ? "icon-light" : "icon-unlight") + '" href="#">' + "&nbsp;" + '</a>';
                        }
                    }
			]],
        pageSize: 100,
        pageList: [100, 200, 300],
        rownumbers: false,
        pageNumber: 1,
        singleSelect: true,
        toolbar: [
//            {
//            text: permission.Create ? '添加' : '',
//            iconCls: permission.Create ? 'icon-add' : "null",
//            handler: function () {
//                var adminname = getGridSelection('#DataGrid', 'AdminName');
//                $("#CAdminName").val(adminname);
//                showAdminGroupPosition();
//                }
//            },
            {
                text: permission.Delete ? '删除' : '',
                iconCls: permission.Delete ? 'icon-cut' : "null",
                handler: function () {
                    if (hidAdminId && confirm('你确定删除选中的记录?')) {
                        var posId = getGridSelection('#GroupPosition', 'PositionId');
                        var groupId = getGridSelection('#GroupPosition', 'GroupId');
                        var roleId = getGridSelection('#GroupPosition', 'RoleId');

                        removeAminGroupPosition(groupId, posId, roleId);
                    }
                }
            }],
        onLoadSuccess: function () {
            $(".setDefaultGroupPos").click(function () {
                post(GroupPositionPostUrl,
                        {
                            adminId: hidAdminId,
                            groupId: $(this).attr("groupid"),
                            positionId: $(this).attr("posid"),
                            groupName: $(this).attr("groupname"),
                            positionName: $(this).attr("posname"),
                            roleId: $(this).attr("roleid"),
                            changedefault: true
                        },
                         function (text, data) {
                             $('#GroupPosition').datagrid("reload");
                         }
                    );
            });
        }
    });
}

function init() {
    window.queryData = function () {
        LoadAccountGrid();
    }
    queryData();
    
    $('#treeGroup').tree({
        url: GetGroupTree,
        onClick: function (node) {
            hiddGroupId = node.id;
            LoadAccountGrid();
            $("#dvAllGroup").removeClass("tree-node-selected");
        }
    });

    //loadGroupPosition();
}

$(document).ready(function () {
    //权限获取
    LoadPageModuleFunction("Account", init);
});

function popupPassword(id) {

    SetWin(480, 320, baseurl + '/ChangePassword/' + id, '修改密码');
    return false;
}
var hidAdminId = "";
function LoadAccountGrid(code) {
    if (permission.Browse)
        $('#DataGrid').datagrid({
            nowrap: false,
            striped: true,
            fit: true,
            border: false,
            url: baseurl + "/GetAccountList",
            queryParams: getQueryDataParams(),
            columns: [[
                   { field: 'ck', checkbox: true },
					{ field: 'AdminName', title: '用户名称', width: 100 },
					{ field: 'LoginName', title: '登录帐号', width: 80 },
                    { field: 'Email', title: '邮件地址', width: 120 },
					//{ field: 'GroupNames', title: '用户角色', width: 250 },
                    { field: 'Status', title: '状态', width: 40,
                        formatter: function (value, rec) {
                            switch (value) {
                                case 1:
                                    return "有效";
                                case 0:
                                    return "无效";
                                default:
                                    return "删除";
                            };
                        }
                    },
					{ field: 'AAA', title: '修改密码', width: 70, formatter: function (val, rec) {
					        return newlinkbutton("", "修改密码", 'popupPassword("' + rec.AdminId + '")', "点击修改密码");
					    }
					}
			]],
            pagination: true,
            pageSize: 100,
            pageList: [100, 200, 300],
            rownumbers: true,
            pageNumber: 1,
            singleSelect: true,
            toolbar: [{
                text: permission.Create ? '添加' : '',
                iconCls: permission.Create ? 'icon-add' : "null",
                handler: function () {
                    SetBackFunc(AddSuccess);
                    SetWin(640, $(document).height() < 600 ? $(document).height() : 600, baseurl + '/Create/?GroupId=' + (hiddGroupId ? hiddGroupId : ""), '添加' + getTitle());
                }
            },
            '-',
            {
                text: permission.Edit ? '修改' : '',
                iconCls: permission.Edit ? 'icon-edit' : "null",
                handler: function () {
                    var selId = getGridSelection('#DataGrid', 'AdminId');

                    if (selId != "") {
                        SetBackFunc(EditSuccess);
                        SetWin(640, $(document).height() < 600 ? $(document).height() : 600, baseurl + '/Edit/' + selId, '修改' + getTitle());
                    }
                }
            },
            '-',
            {
                text: permission.Delete ? '删除' : '',
                iconCls: permission.Delete ? 'icon-cut' : "null",
                handler: function () {
                    var ids = getGridSelections("DataGrid", "AdminId");
                    if (ids != null && ids != '' && confirm('你确定删除选中的记录?'))
                        deleteItems({ delids: ids }, baseurl + '/Delete', DeleteSuccess);
                }
            }],
            onClickRow: function (index, data) {
                //if (index > -1) {
                //    hidAdminId = data.AdminId;
                //    loadGroupPosition();
                //}
            },
            onBeforeLoad: function () {
                RemoveForbidButton();
            }
        });
}


function AddSuccess() {
    LoadAccountGrid();
    setTimeout("MsgShow('系统提示','添加成功。');", 500);
}

function EditSuccess(func) {
    LoadAccountGrid();
    setTimeout("MsgShow('系统提示','修改成功。');", 500);
}

function DeleteSuccess(func) {
    LoadAccountGrid();
    setTimeout("MsgShow('系统提示','删除成功。');", 500);
}