var hiddenBoxId = null;
var hiddenMessageId = null;
var hiddenBoxType = "InBox";
var hiddenUserId = "";

function init() {
    LoadMessageBoxTree();
    LoadMessageGrid('', hiddenBoxId, hiddenBoxType);
}
function queryData() {
    LoadMessageGrid('', hiddenBoxId, hiddenBoxType);
}
function reloadMessage(){
    $('#tbMessage').datagrid("reload");
    reloadMessageBoxTree();
}
$(document).ready(function () {
    //默认选中第一个
    if (boxtree && boxtree.length > 0)
        hiddenBoxId = boxtree[0].id;
    //权限获取
    //LoadPageModuleFunction("Message", init)
    init();
});
function getBoxType(treenodes, nodeid) {
    for (var i = 0; i < treenodes.length; i++) {
        if (treenodes[i].id == nodeid) {
            return treenodes[i].BoxType;
        }
        if (treenodes[i].children) {
            var otype = getBoxType(treenodes[i].children, nodeid);
            if (otype)
                return otype;
        }
    }
}
function reloadMessageBoxTree() {
    $.getJSON(
            baseurl + "/UserMessageBox?t" + new Date().toString(),
            function (data) {
                boxtree = data;
                $('#tMessageBox').tree("loadData", boxtree);
            }
        );
   
}
function LoadMessageBoxTree() {
    $('#tMessageBox').tree({
        onClick: function (node) {
            hiddenBoxId = node.id;
            hiddenBoxType = getBoxType(boxtree, hiddenBoxId);
            LoadMessageGrid('', node.id, hiddenBoxType);
        }
    });
    $('#tMessageBox').tree("loadData", boxtree);
}
function hasReadFormat(rec,value) {
    if (!rec.ReadTime || rec.ReadTime == '0001-01-01T00:00:00') {
        return "<b>" + value + "</b>"
    }
    return value;
}
function getMsgColumns(boxtype) {
    switch (boxtype) {
        case "InBox":
            return [[
                    { field: 'ck', checkbox: true },
                    { field: 'Title', title: '标题', width: GetWidth(0.2), align: 'left', formatter: function (value, rec) {
                        return hasReadFormat(rec, value);
                    }
                    },
                    { field: 'SenderName', title: '发送人', width: GetWidth(0.2), align: 'left', formatter: function (value, rec) {
                        return hasReadFormat(rec, value);
                    }
                    },
                    { field: 'SendTime', title: '发送时间', width: GetWidth(0.2), align: 'left', formatter: function (value, rec) {
                        return hasReadFormat(rec, DateHandler(value));
                    }
                    }

			]];
        case "Recyle":
            return [[
             { field: 'ck', checkbox: true },
                    { field: 'Title', title: '标题', width: GetWidth(0.2), align: 'left', formatter: function (value, rec) {
                        return hasReadFormat(rec, value);
                    }
                    },
                    { field: 'SenderName', title: '发送人', width: GetWidth(0.2), align: 'left', formatter: function (value, rec) {
                        return hasReadFormat(rec, value);
                    }
                    },
                    { field: 'ReceiveTime', title: '接收时间', width: GetWidth(0.2), align: 'left', formatter: function (value, rec) {
                        return hasReadFormat(rec, DateHandler(value));
                    }
                    }
			]];
        case "OutBox":
            return [[
                     
                    { field: 'Title', title: '标题', width: GetWidth(0.2), align: 'left' },
                    { field: 'ReceiversName', title: '收件人', width: GetWidth(0.3), align: 'left' },
                    { field: 'SendTime', title: '发送时间', width: GetWidth(0.2), align: 'left', formatter: function (value, rec) {
                        return DateHandler(value);
                    } 
                    }

			]];
        case "Draft":
            return [[
                    { field: 'ck', checkbox: true },
                    { field: 'Title', title: '标题', width: GetWidth(0.2), align: 'left' },
                    { field: 'ReceiversName', title: '收件人', width: GetWidth(0.2), align: 'left' },
                    { field: 'CreateTime', title: '创建时间', width: GetWidth(0.2), align: 'left', formatter: function (value, rec) {
                        return DateHandler(value);
                    } 
                    }

			]];
        default:
            return [[
                    { field: 'Title', title: '标题', width: GetWidth(0.2), align: 'left' },
                    { field: 'SenderName', title: '发送人', width: GetWidth(0.2), align: 'left' },
                    { field: 'SenderTime', title: '发送时间', width: GetWidth(0.2), align: 'left', formatter: function (value, rec) {
                        return DateHandler(value);
                    } 
                    }

			]];
    }
}
function LoadMessageGrid(code, boxid, boxtype) {
    $("#messageContent").contentWindow = null;
    $('#tbMessage').datagrid({
        nowrap: false,
        striped: true,
        fit: true,
        border: false,
        url: baseurl + "/MessageList",
        idField: 'MessageId',
        queryParams: { BoxId: boxid, CodeOrName: $("#SearchText").val(), BoxType: boxtype },
        columns: getMsgColumns(boxtype),
        pagination: true,
        pageSize: 15,
        pageList: [10, 15, 20, 30],
        rownumbers: true,
        pageNumber: 1,
        toolbar: [{
            text: permission.Create ? '新建' : '',
            iconCls: permission.Create ? 'icon-add' : "null",
            handler: function () {
                SetBackFunc(MessageSaveSuccess);
                SetWinWithMaxSize("新建消息", baseurl + '/CreateMessage?BoxId=' + hiddenBoxId);
            }
        },

            {
                text: permission.Edit && boxtype == "Draft" ? '修改' : '',
                iconCls: permission.Edit && boxtype == "Draft" ? 'icon-edit' : "null",
                handler: function () {
                    var MessageId = getGridSelection('#tbMessage', 'MessageId');

                    if (MessageId != "") {
                        SetBackFunc(MessageSaveSuccess);
                        SetWinWithMaxSize('修改消息', baseurl + '/EditMessage/' + MessageId);
                    }
                }
            },
             {
                 text: permission.Edit && boxtype == "Draft" ? '发送' : '',
                 iconCls: permission.Edit && boxtype == "Draft" ? 'icon-edit' : "null",
                 handler: function () {
                     var MessageId = getGridSelection('#tbMessage', 'MessageId');

                     if (MessageId != "") {
                         SendMessage();
                     }
                 }
             },
              '-',
             {
                 text: permission.Edit && boxtype == "OutBox" ? '撤回' : '',
                 iconCls: permission.Edit && boxtype == "OutBox" ? 'icon-edit' : "null",
                 handler: function () {
                     WithDrawMessage();
                 }
             },
            '-',
             {
                 text: permission.Edit && boxtype == "InBox" ? '回收' : '',
                 iconCls: permission.Delete && boxtype == "InBox" ? 'icon-cut' : "null",
                 handler: function () {
                     RecyleMessage();
                 }
             },
            {
                text: permission.Edit && boxtype == "Recyle" ? '恢复' : '',
                iconCls: permission.Delete && boxtype == "Recyle" ? 'icon-cut' : "null",
                handler: function () {
                    RestoreRecyledMessage();
                }
            },
            {
                text: permission.Edit && (boxtype == "Recyle" || boxtype == "Draft") ? '删除' : '',
                iconCls: permission.Delete && (boxtype == "Recyle" || boxtype == "Draft") ? 'icon-cut' : "null",
                handler: function () {
                    DeleteMessage();
                }
            },
            {
                text: permission.Edit && boxtype == "Recyle" ? '清空' : '',
                iconCls: permission.Delete && boxtype == "Recyle" ? 'icon-cut' : "null",
                handler: function () {
                    CleanRecyleMessage();
                }
            }],
        onClickRow: function (idx, data) {
            if (data.MessageId) {
                hiddenMessageId = data.MessageId;
                $("#messageContent").show();
                $("#messageContent").attr("src", baseurl + '/ViewMessage/' + hiddenMessageId + "?mode=inframe");
                $("#messageContent").height($("#messageContent").parent().height());
                setTimeout(reloadMessage,1500);
            }
        },
        onDblClickRow: function (idx, data) {
            if (data.MessageId) {
                hiddenMessageId = data.MessageId;
                SetWinWithMaxSize("浏览消息", baseurl + '/ViewMessage/' + hiddenMessageId);
                setTimeout(reloadMessage,1500);
            }
        },
        onBeforeLoad: function () {
            RemoveForbidButton();
        }
    });
}

function MessageSaveSuccess() {
    LoadMessageBoxTree();
    reloadMessageBoxTree();
    setTimeout("LoadMessageGrid();", 500);
    setTimeout("MsgShow('系统提示','保存成功。');", 1000);
}

function SendMessage() {
    var MessageId = getGridSelections('#tbMessage', 'MessageId');

    if (MessageId) {

        $.messager.confirm('Question', '确定要发送指定的消息?', function (r) {
            if (r) {
                $.ajax({
                    type: "POST",
                    url: baseurl + "/Send",
                    data: { MessageIds: MessageId, userid: hiddenUserId },
                    dataType: "text",
                    success: function (json) {
                        setTimeout("MsgShow('系统提示','发送成功。');", 200);
                        reloadMessage();
                        if (hiddenMessageId == MessageId) {
                            hiddenMessageId = null;
                        }
                    }
                });
            }
        });
    }
}
function RestoreRecyledMessage() {
    var MessageId = getGridSelections('#tbMessage', 'MessageId');

    if (MessageId) {

        $.messager.confirm('Question', '确定要恢复指定的消息?', function (r) {
            if (r) {
                $.ajax({
                    type: "POST",
                    url: baseurl + "/Restore",
                    data: { MessageIds: MessageId, userid: hiddenUserId },
                    dataType: "text",
                    success: function (json) {
                        setTimeout("MsgShow('系统提示','恢复消息成功。');", 200);
                        reloadMessage();
                        if (hiddenMessageId == MessageId) {
                            hiddenMessageId = null;
                        }
                    }
                });
            }
        });
    }
}

function RecyleMessage() {
    var MessageId = getGridSelections('#tbMessage', 'MessageId');

    if (MessageId) {

        $.messager.confirm('Question', '确定要回收指定的消息?', function (r) {
            if (r) {
                $.ajax({
                    type: "POST",
                    url: baseurl + "/Recyle",
                    data: { MessageIds: MessageId, userid: hiddenUserId },
                    dataType: "text",
                    success: function (json) {
                        $("#messageContent").hide();
                        setTimeout("MsgShow('系统提示','回收消息成功。');", 200);
                        reloadMessage();

                        if (hiddenMessageId == MessageId) {
                            hiddenMessageId = null;
                        }
                    }
                });
            }
        });
    }
}

function DeleteMessage() {
    var MessageId = getGridSelections('#tbMessage', 'MessageId');

    if (MessageId) {

        $.messager.confirm('Question', '确定要彻底删除指定的消息?', function (r) {
            if (r) {
                $.ajax({
                    type: "POST",
                    url: baseurl + "/Delete",
                    data: { MessageIds: MessageId, userid: hiddenUserId },
                    dataType: "text",
                    success: function (json) {
                        $("#messageContent").hide();
                        setTimeout("MsgShow('系统提示','删除消息成功。');", 200);
                        reloadMessage();

                        if (hiddenMessageId == MessageId) {
                            hiddenMessageId = null;
                        }
                    }
                });
            }
        });
    }
}
function CleanRecyleMessage() {
    var MessageId = getGridSelections('#tbMessage', 'MessageId');

    if (MessageId) {

        $.messager.confirm('Question', '确定要清空回收站的消息?', function (r) {
            if (r) {
                $.ajax({
                    type: "POST",
                    url: baseurl + "/Clean",
                    data: { BoxId: hiddenBoxId, userid: hiddenUserId },
                    dataType: "text",
                    success: function (json) {
                        $("#messageContent").hide();
                        setTimeout("MsgShow('系统提示','清空消息成功。');", 200);
                        reloadMessage();

                        if (hiddenMessageId == MessageId) {
                            hiddenMessageId = null;
                        }
                    }
                });
            }
        });
    }
}
function WithDrawMessage() {
    var MessageId = getGridSelections('#tbMessage', 'MessageId');

    if (MessageId) {

        $.messager.confirm('Question', '确定要撤回指定的已发送消息?', function (r) {
            if (r) {
                $.ajax({
                    type: "POST",
                    url: baseurl + "/WithDraw",
                    data: { MessageId: MessageId, userid: hiddenUserId },
                    dataType: "text",
                    success: function (json) {
                        setTimeout("MsgShow('系统提示','未读的消息撤回成功。');", 200);
                        reloadMessage();

                        if (hiddenMessageId == MessageId) {
                            hiddenMessageId = null;
                        }
                    }
                });
            }
        });
    }
}


