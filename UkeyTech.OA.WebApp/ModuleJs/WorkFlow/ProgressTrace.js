var hiddenId, hidTaskId;
function getQueryDataParams() {
    return { searchtext: $("#SearchText").val() };
}

function init() {
    window.queryData = function () {
        var stext = getQueryDataParams().searchtext;
        if (stext != "")
            LoadWorkflowInstance(stext);
        else
            LoadWorkflowInstance();
    }

    queryData();

    $("#DGTaskInstance").datagrid({
        nowrap: false,
        striped: true,
        fit: true,
        border: false,
        url: baseurl + "/ProcessInstanceTaskList",
        queryParams: {},
        columns: [[
					{ field: 'StepNumber', title: '步骤', width: 40 },
					{ field: 'DisplayName', title: '流程名称', width: 100 },
                    { field: 'State', title: '状态', width: 70,
                        formatter: function (value, rec) {
                            switch (value) {
                                case 0:
                                    return '初始化';
                                case 1:
                                    return '进行中';
                                case 7:
                                    return '已经结束';
                                case 9:
                                    return '已被撤销';
                            }
                        }
                    },
                     { field: 'TaskType', title: '任务类型', width: 70,
                         formatter: function (value, rec) {
                             switch (value) {
                                 case 0:
                                     return '工具';
                                 case 2:
                                     return '表单';
                                 case 1:
                                     return '子流程';
                                 case 3:
                                     return '';
                             }
                         }
                     },

                    { field: 'CreatedTime', title: '创建时间', width: 150,
                        formatter: function (value, rec) {
                            return DateHandler(value);
                        }
                    },
                    { field: 'StartedTime', title: '启动时间', width: 150,
                        formatter: function (value, rec) {
                            return DateHandler(value);
                        }
                    },
                    { field: 'EndTime', title: '结束时间', width: 150,
                        formatter: function (value, rec) {
                            return DateHandler(value);
                        }
                    },
                    { field: 'ExpiredTime', title: '到期时间', width: 150,
                        formatter: function (value, rec) {
                            return DateHandler(value);
                        }
                    }

			]],
        pagination: true,
        pageSize: 15,
        pageList: [10, 15, 20, 30],
        rownumbers: true,
        pageNumber: 1,
        singleSelect: true,
        onBeforeLoad: function () {
            RemoveForbidButton();
        },
        onClickRow: function (index, data) {
            if (index > -1) {
                if (hidTaskId != data.Id) {
                    hidTaskId = data.Id;

                    loadTaskWorkItems(hidTaskId);
                }
            }
        }
    });

    $("#DGTaskVar").datagrid({
        nowrap: false,
        striped: true,
        fit: true,
        border: false,
        url: baseurl + "/ProcessInstanceVars",
        queryParams: {},
        columns: [[
					{ field: 'Name', title: '名称', width: 100 },
					{ field: 'ValueType', title: '类型', width: 120 },
                    { field: 'StringValue', title: '值', width: 120 }
			]],
        pagination: true,
        pageSize: 15,
        pageList: [10, 15, 20, 30],
        rownumbers: true,
        pageNumber: 1,
        singleSelect: true,
        onBeforeLoad: function () {
            RemoveForbidButton();
        }
    });
}

$(document).ready(function () {
    //权限获取
    LoadPageModuleFunction("MyWorkItemView", init);
});

function ViewProcess(id, workflowprocessid) {
    window.open(baseurl + "/ProcessTraceView?id=" + id + "&workflowprocessid=" + workflowprocessid + "&t=" + new Date().toString(), "progressview");
}
function LoadTaskInstance(id) {
    $("#DGTaskInstance").datagrid("load", { processinstanceId: id })
}
function LoadTaskVar(id) {
    id = id ? id : hiddenId;
    $("#DGTaskVar").datagrid("load", { processinstanceId: id });
}


function getStausName(state) {
    switch (state) {
        case 0:
            return '<span class="initialized">' + '初始化中<span>' + "</span>";
        case 1:
            return '<span class="running">' + '运行中' + "</span>";
        case 7:
            return '<span class="completed">' + '已完成' + "</span>";
        case 9:
            return '<span class="canceled">' + '被撤销' + "</span>";

    }
}
var template = "<div><label>工作任务</label>{6}</div>"
                      + "<div><label>领取人</label>{0}</div>"
                      + "<div><label>委托人</label><span style='color:blue;'>{8}</span></div>"
                      + "<div><label>完成人</label>{7}</div>"
                      + "<div><label>状态</label>{1}</div>"
                      + "<div><label>开始时间</label>{2}</div>"
                      + "<div><label>签收时间</label>{3}</div>"
                      + "<div><label>结束时间</label>{4}</div>"
                      + "<div><label>说明</label>{5}</div>"

function loadTaskWorkItems(id) {
    $("#workitems").html("读取中...");

    $.ajax({
        url: baseurl + "/ProcessTraceWorkItems",
        data: {
            id: id,
            t: new Date().toString()
        },
        dataType: "json",
        success: function (json) {

            if (json.total > 0) {
                var output = "";
                for (var i in json.rows) {
                    output += "<pre class='searchbox'>"
                    output += template.replace("{0}", json.rows[i].ActorName ? json.rows[i].ActorName : "无")
                                .replace("{1}", getStausName(json.rows[i].State))
                                .replace("{2}", DateHandler(json.rows[i].CreatedTime))
                                .replace("{3}", json.rows[i].ClaimedTime ? DateHandler(json.rows[i].ClaimedTime) : "")
                                .replace("{4}", json.rows[i].EndTime ? DateHandler(json.rows[i].EndTime) : "")
                                .replace("{5}", json.rows[i].Comments ? json.rows[i].Comments : "")
                                .replace("{6}", json.rows[i].DisplayName ? json.rows[i].DisplayName : "")
                                .replace("{8}", json.rows[i].AssignActors ? json.rows[i].AssignActors : "")
                                .replace("{7}", json.rows[i].CompleteActorName ? json.rows[i].CompleteActorName : "");
                    output += "</pre>"
                }

                $('#workitems').html(output);
                delete output;
            }
            else {
                $("#workitems").html("");
            }
        }
    });
}
function getQueryDataParams() {
    return {
        startdate: $("#StartDate").val(),
        enddate: $("#EndDate").val(),
        processid: $("#PROCESS_ID").val()
    };
}
function LoadWorkflowInstance(code) {
    if (permission.Browse)
        $('#DataGrid').treegrid({
            nowrap: false,
            striped: true,
            fit: true,
            border: false,
            url: baseurl + "/FindProcessInstanceList",
            queryParams: getQueryDataParams(),
            treeField: "DisplayName",
            idField: 'Id',
            columns: [[
					{ field: 'DisplayName', title: '流程显示名称', width: 200 },
                    { field: 'State', title: '状态', width: 80,
                        formatter: function (value, rec) {
                            switch (value) {
                                case 0:
                                    return '初始化';
                                case 1:
                                    return '进行中';
                                case 7:
                                    return '已经结束';
                                case 9:
                                    return '已被撤销';
                            }
                        }
                    },
					{ field: 'CCC', title: '子流程', width: 50, align: "center",
					    formatter: function (value, rec) {
					        return rec.ParentTaskInstanceId ? "√" : "";
					    }
					},
                    { field: 'CreatorName', title: '创建者/流程来源', width: 150,
                        formatter: function (value, rec) {
                            if (rec.ParentTaskInstanceName) {
                                return rec.ParentTaskInstanceName
                            }
                            else {
                                return value;
                            }
                        }
                    },
                     {
                         field: 'BizInfo',
                         title: '业务信息',
                         width: 380
                     },
                    { field: 'CreatedTime', title: '开始时间', width: 150,
                        formatter: function (value, rec) {
                            return DateHandler(value);
                        }
                    },
                    { field: 'StartedTime', title: '启动时间', width: 150,
                        formatter: function (value, rec) {
                            return DateHandler(value);
                        }
                    },
                    { field: 'ExpiredTime', title: '到期时间', width: 150,
                        formatter: function (value, rec) {
                            return DateHandler(value);
                        }
                    },
					{ field: 'AAA', title: '查看', width: 60, formatter: function (val, rec) {
					    return newlinkbutton("", "查看", 'ViewProcess("' + rec.Id + "\",\"" + rec.WorkflowProcessId + '")', "查看");
					}
					}
			]],
            pagination: true,
            pageSize: 15,
            pageList: [10, 15, 20, 30],
            rownumbers: true,
            pageNumber: 1,
            singleSelect: true,
            onClickRow: function (index, data) {
                var oldid = hiddenId;
                if (data && index > -1) {
                    hiddenId = data.Id;
                }
                else {
                    hiddenId = index.Id;
                }

                if (oldid != hiddenId) {
                    $("#workitems").html("");

                    LoadTaskInstance(hiddenId);

                    setTimeout(LoadTaskVar, 1000);
                }
            }
        });
}
           