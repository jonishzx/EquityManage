function Task() {
    return {
        
    };
}

function ShowTaskPanel(type) {
    if (type == 'FORM' || type == 'TOOL')
        $("#DurationPanel").show();
    else
        $("#DurationPanel").hide();

    $("#FORMTask, #TOOLTask,#SUBFLOWTask").hide();
    $("#" + type + "Task").show();
}

//获取任务的序号
function getTaskIndex(id) {
    var ogData = getTasksFormJsonData("Activity", hiddenActId);
    var i = 0;
    for (i in ogData.tasks) {
        if (ogData.tasks[i].Id.value == id)
            break;
    }

    return { index: i, data: ogData };
}

function ShowPerfomerDialog() {
   
    var output = getTaskIndex(hiddenTaskId);
    var i = output.index;
    var ogData = output.data;

    if (ogData.tasks[i].Performer) {
        var name, displayname, descn='', assignmenttype='', assignmenthandler='', performervalue='';
        if (ogData.tasks[i].Performer) {
            name = ogData.tasks[i].Performer.Name;
            displayname = ogData.tasks[i].Performer.DisplayName;
            assignmenttype = ogData.tasks[i].Performer.AssignmentType;
            assignmenthandler = ogData.tasks[i].Performer.AssignmentHandler;
            performervalue = ogData.tasks[i].Performer.PerformerValue;
            descn = ogData.tasks[i].Performer.Description;
        }
     
        $("#PerformerName").val(name ? name : "Performer");
        $("#PerformerDisplayName").val(displayname ? displayname : "操作员");
        $("#AssignmentHandler").val(assignmenthandler ? assignmenthandler : defaulthandler);
        $("#AssignmentType").val(assignmenttype);
        $("#PerformerValue").val(performervalue);
        $("#PerformerDescription").val(descn);
    }
    else {
        $("#PerformerName").val("Performer");
        $("#PerformerDisplayName").val("");
        $("#AssignmentHandler").val("");
        $("#AssignmentType").val("");
        $("#PerformerValue").val("");
        $("#PerformerDescription").val("");
    }

    performerwin.window('open');
}

function savePerformer() {
    var output = getTaskIndex(hiddenTaskId);
    var i = output.index;
    var ogData = output.data;

    if (!ogData.tasks[i].Performer)
        ogData.tasks[i].Performer = { 
            Name:"Performer",
            DisplayName:"操作员",
            Description:"",
            AssignmentHandler: "",
            AssignmentType: "",
            PerformerValue: ""
        };

    var name = $("#PerformerName").val();
    var displayname = $("#PerformerDisplayName").val();
    var assignmenthandler = $("#AssignmentHandler").val();
    var assignmenttype = $("#AssignmentType").val();
    var performervalue = $("#PerformerValue").val();
    var descn = $("#PerformerDescription").val();

    ogData.tasks[i].Performer.Name = name;
    ogData.tasks[i].Performer.DisplayName = displayname;
    ogData.tasks[i].Performer.Description = descn;
    ogData.tasks[i].Performer.AssignmentHandler = assignmenthandler;
    ogData.tasks[i].Performer.AssignmentType = assignmenttype;
    ogData.tasks[i].Performer.PerformerValue = performervalue;

    $("#Performer").val(ogData.tasks[i].Performer.DisplayName + " , " + ogData.tasks[i].Performer.Name);

    performerwin.window("close");
}

var returnValue;
function ShowUserDialog() {
    SetWin(510, 640, baseurl + '/UserMgmtSelector', '参与者选择');
}

function setUserSelection(val) {
    $("#PerformerValue").val(val);
}

function getFormTypeText(type) {
    switch (type) {
        case "EDIT":
            return "编辑表单";
        case "LIST":
            return "列表表单";
        case "VIEW":
            return "只读表单";
    }

    return null;
}
//显示窗体编辑界面
function ShowFormDialog(type) {

    formwin.window('open');

    var output = getTaskIndex(hiddenTaskId);
    var i = output.index;
    var ogData = output.data;

    var name, displayname, uri = '', descn='', uiscript='';
    if (type == 'EDIT' && ogData.tasks[i].EditForm) {
        name = ogData.tasks[i].EditForm.Name;
        displayname = ogData.tasks[i].EditForm.DisplayName;
        uri = ogData.tasks[i].EditForm.Uri;
        uiscript = ogData.tasks[i].EditForm.UIScript;
        descn = ogData.tasks[i].EditForm.Description;
    }
    else if (type == 'LIST' && ogData.tasks[i].ListForm) {
        name = ogData.tasks[i].ListForm.Name;
        displayname = ogData.tasks[i].ListForm.DisplayName;
        uri = ogData.tasks[i].ListForm.Uri;
        uiscript = ogData.tasks[i].ListForm.UIScript;
        descn = ogData.tasks[i].ListForm.Description;
    }
    else if (type == 'VIEW' && ogData.tasks[i].ViewForm) {
        name = ogData.tasks[i].ViewForm.Name;
        displayname = ogData.tasks[i].ViewForm.DisplayName;
        uri = ogData.tasks[i].ViewForm.Uri;
        uiscript = ogData.tasks[i].ViewForm.UIScript;
        descn = ogData.tasks[i].ViewForm.Description;
    }
    $("#FormName").val(name ? name : ogData.props.Id.value + "表单");
    $("#FormDisplayName").val(name ? name : ogData.props.DisplayName.value + getFormTypeText(type));
    $("#FormUri").val(uri);
    $("#FormUIScript").val(uiscript);
    $("#FormDescription").val(descn);

    $(formwin).data("type", type);
}

function saveForm() {
    var output = getTaskIndex(hiddenTaskId);
    var i = output.index;
    var ogData = output.data;

    var name = $("#FormName").val();
    var displayname = $("#FormDisplayName").val();
    var uri = $("#FormUri").val();
    var descn = $("#FormDescription").val();
    var uiscript = $("#FormUIScript").val();
    var type = $(formwin).data("type");
    var emptyform = { Name: "", DisplayName: "", Uri: "", Description: "", UIScript: "" };
     if (type == 'EDIT') {
         if (!ogData.tasks[i].EditForm)
             ogData.tasks[i].EditForm = emptyform;
        
        ogData.tasks[i].EditForm.Name = name;
        ogData.tasks[i].EditForm.DisplayName = displayname;
        ogData.tasks[i].EditForm.Uri = uri;
        ogData.tasks[i].EditForm.UIScript = uiscript;
        ogData.tasks[i].EditForm.Description = descn;  
        
         $("#EditFormDetail").val(ogData.tasks[i].EditForm.DisplayName 
         + " , " + ogData.tasks[i].EditForm.Uri);      
    }
    else if (type == 'LIST') {
        if(!ogData.tasks[i].ListForm)
            ogData.tasks[i].ListForm = emptyform;
        
        ogData.tasks[i].ListForm.Name = name;
        ogData.tasks[i].ListForm.DisplayName = displayname;
        ogData.tasks[i].ListForm.Uri = uri;
        ogData.tasks[i].ListForm.UIScript = uiscript;
        ogData.tasks[i].ListForm.Description = descn;

         $("#ListFormDetail").val(ogData.tasks[i].ListForm.DisplayName 
         + " , " + ogData.tasks[i].ListForm.Uri);
    }
    else if (type == 'VIEW') {
        if(!ogData.tasks[i].ViewForm)
            ogData.tasks[i].ViewForm = emptyform;
        
        ogData.tasks[i].ViewForm.Name = name;
        ogData.tasks[i].ViewForm.DisplayName = displayname;
        ogData.tasks[i].ViewForm.Uri = uri;
        ogData.tasks[i].ViewForm.UIScript = uiscript;
        ogData.tasks[i].ViewForm.Description = descn;

         $("#ViewFormDetail").val(ogData.tasks[i].ViewForm.DisplayName 
         + " , " + ogData.tasks[i].ViewForm.Uri);
     }

     formwin.window("close");
}

function addnewTask(actid, type) {
    var ogData = getTasksFormJsonData("Activity", actid);
    var newindex = ogData.tasks && ogData.tasks.length ? ogData.tasks.length : 0;

    var newid = actid + "_" + type + "Task_" + (newindex + 1);
    if (!ogData.tasks)
        ogData.tasks = [];

    var newobj = { Id: { value: newid },
        Name: { value: type + "Task_" + (newindex + 1) },
        DisplayName: { value: '新建任务' },
        Description: { value: '' },
        Type: { value: type },
        LoopStrategy: { value: 'REDO' },
        TaskInstanceCreator: { value: '' },
        TaskInstanceRunner: { value: '' },
        TaskInstanceCompletionEvaluator: { value: '' },
        Priority: { value: '1' },
        CompletionStrategy: { value: 'ANY' },
        DefaultView: 'EDITFORM',
        Performer: { Name: '',
            DisplayName: '',
            Description: '',
            AssignmentType: 'Handler',
            AssignmentHandler: '',
            PerformerValue: ''
        },
        EditForm: {
            Name: '', DisplayName: '', Uri: '', Description: ''
        }
    };
    ogData.tasks.push(newobj);

    return newobj;
}

//读取到窗体
function loadData(type) {
    
    $("#TaskForm").tabs("select", 0);

    if (hiddenTaskId) {

        var output = getTaskIndex(hiddenTaskId);
        var i = output.index;
        var ogData = output.data;

        //base
        $("#TaskName").val(ogData.tasks[i].Name.value);
        $("#TaskId").val(ogData.tasks[i].Id.value);
        $("#TaskDisplayName").val(ogData.tasks[i].DisplayName.value);
        $("#TaskDescn").val(ogData.tasks[i].Description.value);

        $("#TaskInstanceCreator").val(ogData.tasks[i].TaskInstanceCreator.value);
        $("#TaskInstanceRunner").val(ogData.tasks[i].TaskInstanceRunner.value);
        $("#TaskInstanceCompletionEvaluator").val(ogData.tasks[i].TaskInstanceCompletionEvaluator.value);
        $("#LoopStrategy").val(ogData.tasks[i].LoopStrategy.value);

        //duration
        if (ogData.tasks[i].Duration) {
            $("#Duration").val(ogData.tasks[i].Duration.Value);
            $("#IsBusinessTime").attr("checked", ogData.tasks[i].Duration.IsBusinessTime == "TRUE");
            $("#Unit").val(ogData.tasks[i].Duration.Unit);
        }
        //FORMTask
        if (type == "FORM") {
            //performer 
            if (ogData.tasks[i].Performer) {
                $("#Performer").val(ogData.tasks[i].Performer.DisplayName + " , " + ogData.tasks[i].Performer.Name);
            }

            //form
            if (ogData.tasks[i].EditForm) {
                $("#EditFormDetail").val(ogData.tasks[i].EditForm.DisplayName + " , " + ogData.tasks[i].EditForm.Uri);
            }

            if (ogData.tasks[i].ListForm) {
                $("#ListFormDetail").val(ogData.tasks[i].ListForm.DisplayName + " , " + ogData.tasks[i].ListForm.Uri);
            }

            if (ogData.tasks[i].ViewForm) {
                $("#ViewFormDetail").val(ogData.tasks[i].ViewForm.DisplayName + " , " + ogData.tasks[i].ViewForm.Uri);
            }
        }
        else if (type == "TOOL") {
            //tool task
            if (ogData.tasks[i].Application) {
                $("#AppName").val(ogData.tasks[i].Application.Name);
                $("#AppDisplayName").val(ogData.tasks[i].Application.DisplayName);
                $("#Handler").val(ogData.tasks[i].Application.Handler);
                $("#Parameters").val(ogData.tasks[i].Application.Parameters);
                $("#AppDescription").val(ogData.tasks[i].Application.Description);
            }
            if($("#AppName").val()=='')
                $("#AppName").val(hiddenTaskId + ".TASKAPP");
        }
        else if (type == "SUBFLOW") {
            //subflow task
            if (ogData.tasks[i].SubWorkflowProcess) {
                $("#SubFlowName").val(ogData.tasks[i].SubWorkflowProcess.Name);
                $("#SubFlowDisplayName").val(ogData.tasks[i].SubWorkflowProcess.DisplayName);
                $("#WorkflowProcessId").val(ogData.tasks[i].SubWorkflowProcess.WorkflowProcessId);
                $("#SubFlowDescription").val(ogData.tasks[i].SubWorkflowProcess.Description);
            }
        }
    }// if
    else {
        //加入新的任务
        var newobj = addnewTask(hiddenActId, type);

        $("#TaskName").val(newobj.Name.value);
        $("#TaskId").val(newobj.Id.value);
        $("#TaskDisplayName").val(newobj.DisplayName.value);
        $("#TaskDescn").val("");

        $("#TaskInstanceCreator").val("");
        $("#TaskInstanceRunner").val("");
        $("#TaskInstanceCompletionEvaluator").val("");
        $("#LoopStrategy").val("");

        $("#Duration").val("");
        $("#IsBusinessTime").attr("checked", false);
        $("#Unit").val("");

        $("#AppName").val(newobj.Id.value + ".TASKAPP");
        $("#AppDisplayName").val("");
        $("#Handler").val("");
        $("#Parameters").val("");
        $("#AppDescription").val("");

        $("#SubFlowName").val("");
        $("#SubFlowDisplayName").val("");
        $("#WorkflowProcessId").val("");
        $("#SubFlowDescription").val("");

        $("#EditFormDetail").val("");
        $("#ListFormDetail").val("");
        $("#ViewFormDetail").val("");
        $("#Performer").val("");

        loadTaskGrid(hiddenActId);
       
    }//end if

    $("#TaskForm").show();
    $("#Buttons").show();
    ShowTaskPanel(type);
}

function loadTaskGrid(id) {
    var ogData = getTasksFormJsonData("Activity", id);
    var newmodel, jsondata = { total: (ogData && ogData.tasks) ? ogData.tasks.length : 0, rows: [] };
    hiddenActId = id;
    for (var m in ogData.tasks) {
        newmodel = { Name: ogData.tasks[m].Name.value,
            DisplayName: ogData.tasks[m].DisplayName.value,
            Id: ogData.tasks[m].Id.value,
            Type: ogData.tasks[m].Type.value
        };
        jsondata.rows.push(newmodel);
    }
    $("#TaskGrid").datagrid("loadData", jsondata);

    if (jsondata.rows.length > 0) {
        //load first task
        hiddenTaskId = jsondata.rows[0].Id;
        loadData(jsondata.rows[0].Type);
    }
}

function showTaskDialog(name, id) {


    hiddenTaskId = -1;
    $("#TaskForm").hide();

    $("#Buttons").hide();

    loadTaskGrid(id);

    taskwin.window("setTitle", name + "-任务编辑");
    taskwin.window('open');
}

function saveTask() { 
    var output = getTaskIndex(hiddenTaskId);
    var i = output.index;
    var ogData = output.data;

    var task = ogData.tasks[i];

    task.Id.value = $("#TaskId").val();
    task.Name.value = $("#TaskName").val();
    task.DisplayName.value = $("#TaskDisplayName").val();
    task.Description.value = $("#TaskDescn").val();
    
    task.TaskInstanceCreator.value = $("#TaskInstanceCreator").val();
    task.TaskInstanceRunner.value = $("#TaskInstanceRunner").val();
    task.TaskInstanceCompletionEvaluator.value = $("#TaskInstanceCompletionEvaluator").val();
    task.LoopStrategy.value = $("#LoopStrategy").val();

    if(task.Type == 'TOOL' || task.Type == 'FORM')
    {
        task.Duration = {
            Value:$("#Duration").val(),
            Unit: $("#Unit").val(),
            IsBusinessTime: $("#IsBusinessTime").attr("checked")
        };
    }
    else{
        task.Duration = null; 
    }

    
    if(task.Type.value == 'FORM' ){
        task.CompletionStrategy.value = $("#CompletionStrategy").val();
        task.DefaultView = $("#DefaultView").val();
    }
    else if (task.Type.value == 'TOOL') {
        task.Application = {
            Name:$("#AppName").val(),
            DisplayName:$("#AppDisplayName").val(),
            Handler:$("#Handler").val(),
            Description:$("#AppDescription").val()         
        };
    }
    else if (task.Type.value == 'SUBFLOW') {
          task.Application = {
            Name:$("#SubFlowName").val(),
            SubFlowDisplayName:$("#SubFlowDisplayName").val(),
            WorkflowProcessId:$("#WorkflowProcessId").val(),
            SubFlowDescription:$("#SubFlowDescription").val()         
        };
    }

    ogData.tasks[i] = task;

    setTimeout("MsgShow('系统提示','任务修改成功。');", 1000);
    taskwin.window("close");   
}

function deleteTasks(){
    $.messager.confirm('Question', '确定要删除任务?', function (r) {
          if (r) {
            var output = getTaskIndex(hiddenTaskId);
            var i = output.index;
            var ogData = output.data;
            delete ogData.tasks[i];
            loadTaskGrid(hiddenActId);
            $("#TaskForm").hide();
        }
    });
}


function save() {
    $.ajax({
        type: "post",
        url: baseurl + '/DesignWorkFlow' + "?t=" + new Date().toString(),
        data: { id: currid, ProcessContent: $("#output").val() },
        dataType: "text",
        success: function (result) {
            if (result == "") {
                alert('保存成功');
            }
            else {
                alert(result);
            }
        }
    });
}


var hiddenActId, hiddenTaskId;
var taskwin, formwin, performerwin;
var flow;
$(function () {
    var flowdesigner =
    $('#myflow').myflow({
        basePath: "",
        restore: jsondata,
        tools: {
            save: {
                onclick: function (data) {
                    var json = eval("(" + data + ")");

                    var startnode = getSepcialStates(json.states, "StartNode");
                    var endnodes = getSepcialStates(json.states, "EndNode");
                    var activities = getSepcialStates(json.states, "Activity");
                    var synchronizers = getSepcialStates(json.states, "Synchronizer");

                    if (!startnode || endnodes.length == 0 || activities.length == 0) {
                        alert('流程至少具有开始一个开始节点,一个结束节点及一个活动');
                        return;
                    }
                    else {
                        var count = activities.length + endnodes.length + (synchronizers ? synchronizers.length : 0) + 1;
                      
                        if (json.paths.length < (count - 1)) {
                            alert('请检查是否有图形未进行连线');
                            return;
                        }
                    }

                    var output = fpdlFormatter(json);
                    $("#output").val(output);
                    save();
                }
            },
            xml: {
                onclick: function (data) {
                    var output = fpdlFormatter(eval("(" + data + ")"));
                    $("#output").val(output);
                    $("#output").select();
                    $("#ViewXML").window("open");
                }
            }
        },
        addRectHandler: function (rect) {
            //原始数据添加            
            jsondata.states[rect.props.Id.value] = rect;
        },
        removeRectHandler: function (id) {
            //原始数据删除
            jsondata.states[id] = null;
            delete jsondata.states[id];
        },
        addPatHhandler: function (rect) {
            //原始数据添加            
            jsondata.paths[rect.props.Id.value] = rect;
        },
        removePathHandler: function (id) {
            //原始数据删除
            jsondata.paths[id] = null;
            delete jsondata.paths[id];
        }
    });

    var windowWidth = document.documentElement.clientWidth;
    var windowHeight = document.documentElement.clientHeight;
    var h = 600, w = 800;

    taskwin = $("#myflow_taskconfig");
    taskwin.window({
        title: '活动-任务属性配置',
        width: w,
        modal: true,
        shadow: true,
        closed: true,
        top: Math.max(0, (windowHeight - h) / 4),
        left: Math.max(0, (windowWidth - w) / 3),
        height: h,
        resizable: false
        //onClose: (fn == undefined) ? function () { } : fn
    });

    formwin = $("#FromEditor");
    var fh = 320, fw = 500;
    formwin.window({
        title: '表单设置',
        width: fw,
        modal: true,
        shadow: true,
        closed: true,
        top: Math.max(0, (windowHeight - fh) / 4),
        left: Math.max(0, (windowWidth - fw) / 3),
        height: fh,
        resizable: false
    });
    $("#saveForm").click(saveForm);
    $("#cancelForm").click(function () { formwin.window("close"); });

    performerwin = $("#PerformerEditor");
    var ph = 320, pw = 500;
    performerwin.window({
        title: '任务参与者设置',
        width: pw,
        modal: true,
        shadow: true,
        closed: true,
        top: Math.max(0, (windowHeight - ph) / 4),
        left: Math.max(0, (windowWidth - pw) / 3),
        height: ph,
        resizable: false
    });
    $("#savePerformer").click(savePerformer);
    $("#cancelPerformer").click(function () { performerwin.window("close"); });

    $("#saveTask").click(saveTask);

    $('#myflow_props').window({
        title: "属性",
        modal: false,
        shadow: false,
        closed: false
    });
    $("#myflow_props").parent().css("left", windowWidth - $("#myflow_props").parent().css("width").replace("px", "") - 20);
    $("#myflow_props").parent().css("top", 10);

    $("#TaskForm,#FORMTask, #TOOLTask,#SUBFLOWTask").hide();

    $("#TaskGrid").datagrid({
        nowrap: false,
        striped: true,
        fit: true,
        border: false,
        queryParams: {},
        columns: [[
                    { field: 'ck', checkbox: true },
					{ field: 'Name', title: '流程代码', width: 80 },
					{ field: 'DisplayName', title: '显示名称', width: 100 },
                    { field: 'Type', title: '类型', width: 70, align: 'center',
                        formatter: function (value, rec) {
                            switch (value) {
                                case "FORM":
                                    return "表单任务";
                                case "TOOL":
                                    return "工具任务";
                                case "SUBFLOW":
                                    return "子流程任务";
                            }
                            
                            return null;
                        }
                    }
			]],
        pagination: false,
        singleSelect: true,
        onClickRow: function (index, data) {
            if (index > -1) {
                if (hiddenTaskId != data.Id) {
                    hiddenTaskId = data.Id;
                    loadData(data.Type);
                }
            }
        },
        toolbar: [{
            text: '表单',
            iconCls: 'icon-add',
            handler: function () {
                hiddenTaskId = '';
                loadData('FORM');
            }
        },
            {
                text: '工具',
                iconCls: 'icon-add',
                handler: function () {
                    hiddenTaskId = '';
                    loadData('TOOL');
                }
            },
        {
            text: '子流程',
            iconCls: 'icon-add',
            handler: function () {
                hiddenTaskId = '';
                loadData('SUBFLOW');
            }
        },
        '-',
            {
                text: '删除',
                iconCls: 'icon-cut',
                handler: function () {
                    deleteTasks();
                }
            }]
    });

    $("#ViewXML").window({
        width: 600,
        height: 600,
        title: "查看输出的XML",
        modal: false,
        shadow: false,
        closed: true,
        resizable: false
    });


    //test activity
    //showTaskDialog("LoanProcess.Submit_application_activity");

});