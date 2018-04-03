
function OpenUserDialog() {
    LoadUserGrid();
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
                AddUser();
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

function AddUser() {
    var userIds = getGridSelections('tbUser', 'AdminId');
    if (userIds != "") {
        $.ajax({
            type: "POST",
            url: addUserUrl,
            data: { userIds:userIds.SlashFilter()},
            success: function (json) {
                LoadGrid();
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
        url: noUserRefUrl,
        queryParams: {  codeOrName: code },
        frozenColumns: [[
	                { field: 'ck', checkbox: true }
			]],
        columns: [[
                    { field: 'LoginName', title: '用户代码', width: GetWidth(0.08), align: 'left' },
                    { field: 'AdminName', title: '用户名称', width: GetWidth(0.10), align: 'left' }
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