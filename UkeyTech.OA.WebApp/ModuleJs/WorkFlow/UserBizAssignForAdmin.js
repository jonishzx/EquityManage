var hiddenFormId = null;
var hiddenUserId = null;

document.onkeydown = function (evt) {
    var evt = window.event ? window.event : evt;
    var targetId = (evt.target) ? evt.target.id : evt.srcElement.id;
    if (targetId != "UserId" && evt.keyCode == 13) {
        document.getElementById("btnSubmit").click();
    }
};

function getQueryDataParams() {
    return { searchtext: $("#SearchText").val() };
}

function reloadBizListAndAssignList() {
    LoadBizList();
    LoadAssignUserList();
}

function init() {
    LoadSelUserList();
    window.queryData = function() {
        var stext = getQueryDataParams().searchtext;
        if (stext != "")
            LoadBizList(stext);
        else
            LoadBizList();
    };

    initAssignUserGrid();
    queryData();
}

$(document).ready(function () {
    //权限获取
    LoadPageModuleFunction("AdminUserBizAssign", init);

    $("#Dialog").css("display", "inline");

    $("#Dialog").dialog({
        title: "提示",
        modal: false,
        shadow: false,
        closed: true,
        width: 500,
        height: 520
    });

    $("#AllDateAlter").css("display", "inline");

    $("#AllDateAlter").dialog({
        title: "提示",
        modal: true,
        shadow: false,
        closed: true,
        width: 300,
        height: 190
    });
});

function LoadSelUserList(code) {
    if (permission.Browse)
        $('#SelUserGrid').datagrid({
            nowrap: false,
            striped: true,
            fit: true,
            border: false,
            url: baseurl + "/GetAccountList",
            queryParams: { adminname: code, GroupId: "" },
            columns: [[

              { field: 'LoginName', title: '用户代码', width: 70, align: 'left' },
              { field: 'AdminName', title: '用户名称', width: 100, align: 'left' }
            ]],
            pagination: true,
            pageSize: 100,
            pageList: [100, 200, 300],
            rownumbers: true,
            pageNumber: 1,
            singleSelect: true,
            onBeforeLoad: function () {
                RemoveForbidButton();
            },
            onClickRow: function (index, data) {
                if (index > -1) {
                    if (hiddenUserId != data.AdminId.toString()) {
                        hiddenUserId = data.AdminId.toString();
                        LoadBizList();
                        if (hiddenFormId && hiddenUserId) {
                            LoadAssignUserList();
                        }
                    }
                }
            }
        });
}

function LoadBizList(code) {
    if (permission.Browse)
        $('#DataGrid').datagrid({
            nowrap: false,
            striped: true,
            fit: true,
            border: false,
            url: baseurl + "/GetCustomFormList",
            queryParams: { CodeOrName: code, UserId: hiddenUserId },
            frozenColumns: [[
                { field: 'ck', checkbox: true }
            ]],
            columns: [[
                { field: 'ID', title: '编号', width: 20 },
                { field: 'FormName', title: '业务名称', width: 110 },
                { field: 'FormType', title: '业务类型', width: 100 },
                { field: 'UserName', title: '被委托用户(委托时间段)', width: 200 }
            ]],
            pagination: true,
            pageSize: 100,
            pageList: [100, 150, 200, 300],
            rownumbers: true,
            pageNumber: 1,
            //            singleSelect: true,
            toolbar: [{
                text: permission.Edit ? '批量设置委托' : '',
                iconCls: permission.Edit ? 'icon-add' : "null",
                handler: function () {

                    var rows = $('#DataGrid').datagrid("getSelections"); //获取你选择的所有行	
                    if (rows.length == 0) {
                        setTimeout("MsgShow('系统提示','请点击选择业务后再设置委托用户。');", 500);
                        return;
                    }
                    if (rows.length > 0) {
                        var allBillId = '';

                        for (var i = 0; i < rows.length; i++) {
                            allBillId = allBillId + ',' + rows[i].ID;

                        }
                        openAssignToUsersAll(allBillId);
                    }
                }
            },
                '-',
                {
                    text: permission.Edit ? '批量解除委托' : '',
                    iconCls: permission.Edit ? 'icon-cut' : "null",
                    handler: function () {
                        var rows = $('#DataGrid').datagrid("getSelections");

                        if (rows.length == 0) {
                            alert('请选择要做委托的业务审批流程');
                            return;
                        }
                        if (confirm('你确定解除选中记录的委托?')) {
                            if (rows.length > 0) {
                                var allBillId = '';

                                for (var i = 0; i < rows.length; i++) {
                                    allBillId = allBillId + ',' + rows[i].ID;

                                }
                                deleteItems({ formid: allBillId, userid: hiddenUserId }, baseurl + '/DeleteAssignUserAllForAdmin', DeleteColumnAllSuccess);
                            }
                        }
                    }
                }, {
                    text: permission.Edit ? '批量修改委托时间' : '',
                    iconCls: permission.Edit ? 'icon-save' : "null",
                    handler: function () {

                        var rows = $('#DataGrid').datagrid("getSelections"); //获取你选择的所有行	
                        if (rows.length == 0) {
                            setTimeout("MsgShow('系统提示','请点击选择业务后再设置委托时间。');", 500);
                            return;
                        }
                        if (rows.length > 0) {
                            var allBillId = '';

                            for (var i = 0; i < rows.length; i++) {
                                allBillId = allBillId + ',' + rows[i].ID;

                            }

                            openAlterDateToAll(allBillId);
                        }
                    }
                }
            ],
            onBeforeLoad: function () {
                RemoveForbidButton();
            },
            onClickRow: function (index, data) {
                if (onFormGridMultiSelect()) {
                    return;
                }
                if (index > -1) {
                    if (hiddenFormId == -1) {
                        hiddenFormId = null;
                        return;
                    }
                    if (hiddenFormId != data.ID.toString()) {
                        hiddenFormId = data.ID.toString();
                        if (hiddenFormId && hiddenUserId)
                            LoadAssignUserList();
                    }
                }
            },
            onUnselect: function (index, data) {
                if (index > -1) {
                    $('#UserAssignGrid').datagrid("loadData", { total: 0, rows: [] })
                }
                hiddenFormId = -1;
            }
        });
        }
        function onFormGridMultiSelect() {
            var rows = $('#DataGrid').datagrid("getSelections");
            if (rows.length > 1) {
                $('#UserAssignGrid').datagrid("loadData", { total: 0, rows: [] });
                hiddenFormId = null;
                return true;
            }
            else if (rows.length == 1) {
                hiddenFormId = rows[0].ID.toString();
                LoadAssignUserList();
                return true;
            }
            return false;
        }
//**********已委托用户设置*****************

function initAssignUserGrid() {
        if (!permission.Browse)
            return;

        $('#UserAssignGrid').datagrid({
            nowrap: false,
            striped: true,
            fit: true,
            border: false,
            url: baseurl + "/LoadAssignUserListForAdmin",
            queryParams: { formid: hiddenFormId ? hiddenFormId : 0, userid : hiddenUserId },
            columns: [[
                { field: 'ck', checkbox: true },
                { field: 'AdminName', title: '用户名', width: 100 },
                { field: 'LoginName', title: '账号', width: 100 },
                { field: 'AssginBeginDate', title: '开始时间', width: 140, editor: { type: 'validatebox', options: { required: true, validType: 'dateimeYMdHm'} }, formatter: function (value, rec) {
                    return DateHandler(value);
                }
                },
                { field: 'AssginEndDate', title: '结束时间', width: 140, editor: { type: 'validatebox', options: { required: true, validType: 'dateimeYMdHm'} }, formatter: function (value, rec) {
                    return DateHandler(value);
                }
                }
            ]],
            pagination: true,
            pageSize: 100,
            pageList: [100, 150, 200, 300],
            rownumbers: true,
            pageNumber: 1,
            singleSelect: false,
//            toolbar: [{
//                text: permission.Edit ? '设置委托用户' : '',
//                iconCls: permission.Edit ? 'icon-add' : "null",
//                handler: function () {
//                    if (!hiddenFormId) {
//                        setTimeout("MsgShow('系统提示','请点击选择业务后再设置委托用户。');", 500);
//                        return;
//                    }
//                  
//                    openAssignToUsers();
//                }
//            },
//                '-',
//                {
//                    text: permission.Edit ? '解除委托' : '',
//                    iconCls: permission.Edit ? 'icon-cut' : "null",
//                    handler: function () {
//                        var ids = getGridSelections("#UserAssignGrid", "AdminId");
//                        if (ids != null && ids != '' && confirm('你确定处理选中的记录?'))
//                            deleteItems({ delids: ids, formid: hiddenFormId, userid: hiddenUserId }, baseurl + '/DeleteAssignUserForAdmin', DeleteColumnSuccess);
//                    }
//                },/*
//                 {
//                     text: permission.Edit ? '保存委托时间' : '',
//                     iconCls: permission.Edit ? 'icon-save' : "null",
//                     handler: function () {
//                         if (!$('#UserAssignGrid').edatagrid('saveRow'))
//                             return;
//                         var rows = $("#UserAssignGrid").datagrid("getData").rows;
//                         var ids = "", begindate = "", enddate = "";

//                         for (var i = 0; i < rows.length; i++) {
//                             ids += rows[i].AdminId + ",";
//                             if (!rows[i].AssginBeginDate || !rows[i].AssginEndDate)
//                                 return false;
//                             begindate += rows[i].AssginBeginDate + ",";
//                             enddate += rows[i].AssginEndDate + ",";
//                         }

//                         if (ids != null && ids != '' && confirm('你确定处理选中的记录?'))
//                             post(baseurl + '/SetAssignDateForAdmin', { ids: ids, formid: hiddenFormId, begindates: begindate, enddates: enddate, userid: hiddenUserId }, EditColumnSuccess);
//                     }
//                 },*/
//                  {
//                      text: permission.Edit ? '设置委托时间' : '',
//                      iconCls: permission.Edit ? 'icon-save' : "null",
//                      handler: function () {

//                          if (!hiddenFormId) {
//                              setTimeout("MsgShow('系统提示','请点击选择单个业务后再设置委托时间。');", 500);
//                              return;
//                          }

//                          openAlterDate();
//                      }
//                  }
//                 /*
//                  ,{
//                      text: permission.Edit ? '清空委托时间' : '',
//                      iconCls: permission.Edit ? 'icon-cut' : "null",
//                      handler: function () {

//                          var ids = getGridSelections("#UserAssignGrid", "AdminId");
//                          if (ids != null && ids != '' && confirm('你确定处理选中的记录?'))
//                              post(baseurl + '/CleanAssignDate', { ids: ids, formid: hiddenFormId }, DeleteColumnAllSuccess);
//                      }
//                  }*/
//            ],
            onBeforeLoad: function () {
                RemoveForbidButton();
            }
        });
    }

/***************委托用户设置***************/

    function queryUserData() {
        loadUserGrid($("#tbUserNameOrCode").val());
    }
    function querySelUserData() {
        LoadSelUserList($("#txtSelNameOrCode").val());
    }
    function loadUserGrid(code) {
        $('#UserGrid').datagrid({
            nowrap: false,
            striped: true,
            width: 'auto',
            height: 300,
            border: false,
            url: baseurl + "/UnAssignUserListSelForAdmin",
            queryParams: { CodeOrName: $("#adminname").val(), formid: (hiddenFormId ? hiddenFormId : 0), userid: hiddenUserId },
            columns: [[
                { field: 'AdminName', title: '用户名', width: 280 },
                { field: 'LoginName', title: '账号', width: 120 }
            ]],
            pagination: true,
            pageSize: 100,
            pageList: [100, 150, 200, 300],
            rownumbers: true,
            pageNumber: 1,
            singleSelect: true,
            onBeforeLoad: function () {
                RemoveForbidButton();
            }
            , onDblClickRow: function (idx, data) {
                if (window.mutilFormSelected){
                    var formids = getGridSelections("#DataGrid", "ID");
                    assignToUsers(formids, true);
                }
                else
                    assignToUsers(hiddenFormId, false);
            }
        });
    }

    function CloseDialog() {
        $("#Dialog").dialog("close");
    }

    function assignToUsers(ids, mutilUsers) {

        var begindate = $("#StartAppDate").val();
        var enddate = $("#EndAppDate").val();
        var begindate = $("#StartAppDate").val();
        var enddate = $("#EndAppDate").val();
        var userids = getGridSelections('#UserGrid', 'AdminId');

        if (!begindate || !enddate) {
            alert('请选择委托时间范围');
            return;
        }
        if (!userids) {
            alert('请选择接受委托的用户');
            return;
        }
        if (userids) {
            $.ajax({
                type: "POST",
                url: baseurl + (mutilUsers ? "/AllAssignUserListForAdmin" : "/AssignUserListForAdmin"),
                data: { addids: userids, formid: ids, begindate: begindate, enddate: enddate, userid: hiddenUserId },
                dataType: "html",
                beforeSend: ShowLoading,
                error: HideLoading,
                success: function (json) {
                    parseMessage(json, reloadBizListAndAssignList);
                    CloseDialog();
                }
            });
        }
    }

    function openAssignToUsers() {
        window.mutilFormSelected = false;
        //弹出用户选择框                
        $("#Dialog").dialog("open");
        loadUserGrid('');

        $("#submit").unbind("click");
        $("#submit").click(function () {
            var formids = getGridSelections("#DataGrid", "ID");
            assignToUsers(formids, true);
        });
    }

    function openAssignToUsersAll   (formIds) {
        window.mutilFormSelected = true;    
        //弹出用户选择框                
        $("#Dialog").dialog("open");
        loadUserGrid('');

        $("#submit").unbind("click");
        $("#submit").click(function () {
            assignToUsers(formIds, true);
        });
    }

    function LoadAssignUserList () {
        if (!permission.Browse)
            return;
        if (hiddenFormId && hiddenUserId)   
            $('#UserAssignGrid').datagrid("load", { formid: hiddenFormId, userid: hiddenUserId });
    }

    function AddColumnSuccess() {
        reloadBizListAndAssignList();
        setTimeout("MsgShow('系统提示','添加成功。');", 500);
    }

    function EditColumnSuccess(func) {
        reloadBizListAndAssignList();
        setTimeout("MsgShow('系统提示','修改成功。');", 500);
    }

    function DeleteColumnSuccess(func) {
        reloadBizListAndAssignList();
        setTimeout("MsgShow('系统提示','删除成功。');", 500);
    }

    function DeleteColumnAllSuccess(func) {
        reloadBizListAndAssignList();
        setTimeout("MsgShow('系统提示','解除成功。');", 500);
    }

    //修改委托时间
    function alterAssignDate(ids, mutilUsers) {
        var begindate = $("#AllStartAppDate").val();
        var enddate = $("#AllEndAppDate").val();

        if (!begindate || !enddate) {
            alert('请选择委托时间范围');
            return;
        }
        if (!hiddenUserId) {
            alert('请选择委托的用户');
            return;
        }
        if (ids) {
            $.ajax({
                type: "POST",
                url: baseurl + (mutilUsers ? "/AllAlterDateForAdmin" : "/AlterDateForAdmin"),
                data: { userid: hiddenUserId, formid: ids, begindate: begindate, enddate: enddate },
                dataType: "html",
                beforeSend: ShowLoading,
                error: HideLoading,
                success: function (json) {
                    if (mutilUsers)
                        parseMessage(json, refreshall);
                    else
                        parseMessage(json, init);

                    CloseAlterDateDialog();
                }
            });
        }
    }
    function refreshall() {
        LoadAssignUserList();
        LoadBizList();
    }
    function openAlterDateToAll(formIds) {

        //弹出委托日期更改选择框                
        $("#AllDateAlter").dialog("open");
        $("#alterSubmit").unbind("click");
        $("#dvTitleMessage").html("批量修改所需业务的委托日期")
        $("#alterSubmit").click(function () {
            var formids = getGridSelections("#DataGrid", "ID");
            alterAssignDate(formids, true);
            setTimeout(refreshall, 300);
        });
    }

    function openAlterDate() {
        //弹出委托日期更改选择框（单业务）                
        $("#AllDateAlter").dialog("open");
        $("#dvTitleMessage").html("批量修改当前业务的委托日期")
        $("#alterSubmit").unbind("click");
        $("#alterSubmit").click(function () {
            alterAssignDate(hiddenFormId, false);
            setTimeout(refreshall, 300);
        });
    }

    function CloseAlterDateDialog() {
        $("#AllDateAlter").dialog("close");
    }
//**********业务字段设置********
