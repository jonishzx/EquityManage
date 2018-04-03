var hiddenFormId = null;



function getQueryDataParams() {
    return { searchtext: $("#SearchText").val() };
}

function init() {
    window.queryData = function () {
        var stext = getQueryDataParams().searchtext;
        if (stext != "")
            LoadBizList(stext);
        else
            LoadBizList();
    }

    initAssignUserGrid();
    queryData();
}

$(document).ready(function () {
    //权限获取
    LoadPageModuleFunction("UserBizAssign", init);

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
        height:190
    });
});

function LoadBizList(code) {
    if (permission.Browse)
        $('#DataGrid').datagrid({
            nowrap: false,
            striped: true,
            fit: true,
            border: false,
            url: baseurl + "/GetUserAssignCustomFormList",
            queryParams: { CodeOrName: code },
            frozenColumns: [[
                { field: 'ck', checkbox: true }
            ]],
            columns: [[
                { field: 'ID', title: '编号', width: 30 },
                { field: 'FormName', title: '业务名称', width: 110 },
                { field: 'FormType', title: '业务类型', width: 100 },
                { field: 'UserName', title: '被委托用户(时间段)', width: 200 }
            ]],
            pagination: true,
            pageSize: 100,
            pageList: [100, 150, 200, 300],
            rownumbers: true,
            pageNumber: 1,
            //            singleSelect: true,
            toolbar: [{
                text: permission.Edit ? '批量修改委托' : '',
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
                                deleteItems({ formid: allBillId }, baseurl + '/DeleteAssignUserAll', DeleteColumnAllSuccess);
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
                                allBillId = allBillId + rows[i].ID;
                                if (i + 1 < rows.length)
                                    allBillId += ",";
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
            url: baseurl + "/LoadAssignUserList",
            queryParams: { formid: hiddenFormId ? hiddenFormId : 0 },
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
//                            deleteItems({ delids: ids, formid: hiddenFormId }, baseurl + '/DeleteAssignUser', DeleteColumnSuccess);
//                    }
//                },
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
//                             post(baseurl + '/SetAssignDate', { ids: ids, formid: hiddenFormId, begindates: begindate, enddates: enddate }, EditColumnSuccess);
//                     }
//                 },
//                  {
//                    text: permission.Edit ? '修改委托时间' : '',
//                    iconCls: permission.Edit ? 'icon-save' : "null",
//                    handler: function () {

//                        if (!hiddenFormId) {
//                            setTimeout("MsgShow('系统提示','请点击选择单个业务后再设置委托时间。');", 500);
//                            return;
//                        }

//                        openAlterDate();
//                    }
//                }
//                  ,{
//                      text: permission.Edit ? '清空委托时间' : '',
//                      iconCls: permission.Edit ? 'icon-cut' : "null",
//                      handler: function () {

//                          var ids = getGridSelections("#UserAssignGrid", "AdminId");
//                          if (ids != null && ids != '' && confirm('你确定处理选中的记录?'))
//                              post(baseurl + '/CleanAssignDate', { ids: ids, formid: hiddenFormId }, DeleteColumnAllSuccess);
//                      }
//                  }
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

    function loadUserGrid(formId) {
        $('#UserGrid').datagrid({
            nowrap: false,
            striped: true,
            width: 'auto',
            height: 300,
            border: false,
            url: baseurl + "/UnAssignUserListSel",
            queryParams: { formid: hiddenFormId ? hiddenFormId : 0, CodeOrName: $("#tbUserNameOrCode").val(), level: "Less" },
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
            },
            onDblClickRow: function (idx, data) {
                var rows = $('#DataGrid').datagrid("getSelections"); //获取你选择的所有行	
                if (rows.length == 0) {
                    setTimeout("MsgShow('系统提示','请点击选择业务后再设置委托时间。');", 500);
                    return;
                }
                if (rows.length > 0) {
                    var allBillId = '';

                    for (var i = 0; i < rows.length; i++) {
                        allBillId = allBillId + rows[i].ID;
                        if (i + 1 < rows.length)
                            allBillId += ",";
                    }

                    assignToUsers(allBillId, true);
                    LoadBizList();
                }
            }
        });
    }

    function CloseDialog() {
        $("#Dialog").dialog("close");
    }
    function assignToUsers(ids, mutilUsers) {
        

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
        if (userids && ids) {
            $.ajax({
                type: "POST",
                url: baseurl + (mutilUsers ? "/AllAssignUserList" : "/AssignUserList"),
                data: { addids: userids, formid: ids, begindate: begindate, enddate: enddate },
                dataType: "html",
                beforeSend: ShowLoading,
                error: HideLoading,
                success: function (json) {
                    if (mutilUsers)
                        parseMessage(json, refreshall);
                    else
                        parseMessage(json, init);

                    CloseDialog();
                }
            });
        }
    }
    function openAssignToUsers() {

        //弹出用户选择框                
        $("#Dialog").dialog("open");
        loadUserGrid('');

        $("#submit").unbind("click");
        $("#submit").click(function () {
            assignToUsers(hiddenFormId, false);
            LoadBizList();
        });
    }

    function openAssignToUsersAll(formIds) {
        //弹出用户选择框                
        $("#Dialog").dialog("open");
        loadUserGrid(formIds);

        $("#submit").unbind("click");
        $("#submit").click(function () {
            assignToUsers(formIds, true);
            LoadBizList();
        });
    }

    //修改委托时间
    function alterAssignDate(ids, mutilUsers) {
        var begindate = $("#AllStartAppDate").val();
        var enddate = $("#AllEndAppDate").val();
      
        if (!begindate || !enddate) {
            alert('请选择委托时间范围');
            return;
        }
      
        if (ids) {
            $.ajax({
                type: "POST",
                url: baseurl + (mutilUsers ? "/AllAlterDate" : "/AlterDate"),
                data: { formid: ids, begindate: begindate, enddate: enddate },
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

    function LoadAssignUserList() {
        if (!permission.Browse)
            return;

        $('#UserAssignGrid').datagrid("load", { formid: hiddenFormId });
    }

    function AddColumnSuccess() {
        LoadAssignUserList();
        setTimeout("MsgShow('系统提示','添加成功。');", 500);
    }

    function EditColumnSuccess(func) {
        LoadAssignUserList();
        setTimeout("MsgShow('系统提示','修改成功。');", 500);
    }

    function DeleteColumnSuccess(func) {
        LoadAssignUserList();
        LoadBizList();
        setTimeout("MsgShow('系统提示','删除成功。');", 500);
    }

    function DeleteColumnAllSuccess(func) {
        init();
        setTimeout("MsgShow('系统提示','解除成功。');", 500);
    }


//**********业务字段设置********
