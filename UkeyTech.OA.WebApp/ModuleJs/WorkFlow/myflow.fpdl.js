function ValidateRect(type, rects) {
    if (type == 'StartNode') {
        for (var m in rects) {
            if (rects[m].uiobject().type == 'StartNode') {
                alert('工作流只能有一个开始节点');
                return false;
            }
        }
    }
    return true;
}
function ValidatePath(from, to, rects, paths) {
    var rst = true;

    var fromType = from.uiobject().type;
    var toType = to.uiobject().type;
    var fromid = from.uiobject().props.Id.value;
    var toid = to.uiobject().props.Id.value;

    if ((fromType == 'EndNode' && toType == 'StartNode')
        || (fromType == 'StartNode' && toType == 'EndNode')) {
        alert("结束节点不能直接连接到开始节点");
        rst = false;
    }
    else if (fromType == 'Synchronizer' && toType == 'Synchronizer') {
        alert("转发点不能直接连接到转发点,你需要在他们之间添加活动");
        rst = false;
    }
    else if (fromType == 'Activity' && toType == 'Activity') {
        alert("活动不能直接连接到活动,你需要在他们之间添加转发点");
        rst = false;
    }  
    else if ((fromType == 'Synchronizer' && toType == 'EndNode')
        || (fromType == 'EndNode' && toType == 'Synchronizer')
        || (fromType == 'StartNode' && toType == 'Synchronizer')
        || (fromType == 'Synchronizer' && toType == 'StartNode')) {
        alert("转发点不能与结束节点或是开始节点相连,只有活动能与开始节点或结束节点相连");
        rst = false;
    }else if (fromType == 'Activity' || toType == 'Activity') {
        for (var p in paths) {
            if (!paths[p])
                continue;

            if ((paths[p].from().uiobject().props.Id.value == fromid 
                   && paths[p].from().uiobject().type == 'Activity')
                 || (paths[p].to().uiobject().props.Id.value == toid 
                   && paths[p].to().uiobject().type == 'Activity')) {
                alert("活动只能存在一入一出的连接");
                rst = false;
                break;
            } //end if
        } //end for 
    } 
    else{

    }

    for (var p in paths) {
        if (!paths[p])
            continue;

        //重复路径不允许
        if ((paths[p].from().uiobject().props.Id.value == fromid && paths[p].to().uiobject().props.Id.value == toid) ||
                (paths[p].to().uiobject().props.Id.value == fromid && paths[p].from().uiobject().props.Id.value == toid)
                ) {
            alert("从'" + from.text() + "'到'" + to.text() + "'连接已经存在");
            rst = false;
            break;
        } //end if
       
    } //end for

    return rst;
}

(function ($) {    
    var myflow = $.myflow;

    $.extend(true, myflow.config.rect, {
        attr: {
            r: 8,
            fill: '#F6F7FF',
            stroke: '#03689A',
            "stroke-width": 2,
            cursor:"pointer"
            
        },
        dbclick:function(uiobject, rect){           
            if(uiobject.type == "Activity"  && typeof(showTaskDialog) != "undefined")
                showTaskDialog(uiobject.props.DisplayName.value, uiobject.props.Id.value);  
        }
    });

    $.extend(true, myflow.config.path.props, {
        Id: { name: 'Id', label: '标识', value: 'Transition', editor: function () { return new myflow.editors.hiddenEditor(); } },
        Name: { name: 'Name', label: '名称', value: '', editor: function () { return new myflow.editors.hiddenEditor(); } },    
        Description: { name: 'Description', label: '描述', value: '', editor: function () { return new myflow.editors.inputEditor(); } },
        Condition: { name: 'Condition', label: '触发条件', value: '', editor: function () { return new myflow.editors.inputEditor(); } }
    });

    $.extend(true, myflow.config.props.props, {
        Id: { name: 'Id', label: '标识', value: '', editor: function () { return new myflow.editors.hiddenEditor(); } },
        Name: { name: 'Name', label: '名称', value: '新建流程', editor: function () { return new myflow.editors.hiddenEditor(); } },
        DisplayName: { name: 'DisplayName', label: '显示名称', value: '', editor: function () { return new myflow.editors.inputEditor(); } },
        Description: { name: 'Description', label: '描述', value: '', editor: function () { return new myflow.editors.inputEditor(); } },
        TaskInstanceCreator: { name: 'TaskInstanceCreator', label: '全局任务启动器', value: '', editor: function () { return new myflow.editors.inputEditor(); } }
    });

    $.extend(true, myflow.config.tools.states, {
        StartNode: {
            showType: 'image',
            type: 'StartNode',
            name: { text: 'StartNode' },
            text: { text: '开始' },
            img: { src: rooturl + 'Content/Images/48/start_event_empty.png', width: 48, height: 48   },
            attr: { width: 50, heigth: 50 },
            props: {
                Id: { name: 'Id', label: '标识', value: 'Process.StartNode', editor: function () { return new myflow.editors.hiddenEditor(); } },
                Name: { name: 'Name', label: '名称', value: 'StartNode', editor: function () { return new myflow.editors.hiddenEditor(); } },
                DisplayName: { name: 'DisplayName', label: '显示名称', value: '开始', editor: function () { return new myflow.editors.inputEditor(); } },
                Description: { name: 'Description', label: '描述', value: '', editor: function () { return new myflow.editors.inputEditor(); } }
            }
        },
        EndNode: {
            showType: 'image',
            type: 'EndNode',
            name: { text: 'EndNode' },
            text: { text: '结束' },
            img: { src: rooturl + 'Content/Images/48/end_event_terminate.png', width: 48, height: 48 },
            attr: { width: 50, heigth: 50 },
            props: {
                Id: { name: 'Id', label: '标识', value: 'Process.EndNode', editor: function () { return new myflow.editors.hiddenEditor(); } },
                Name: { name: 'Name', label: '名称', value: 'EndNode', editor: function () { return new myflow.editors.hiddenEditor(); } },
                DisplayName: { name: 'DisplayName', label: '结束', value: '', editor: function () { return new myflow.editors.inputEditor(); } },
                Description: { name: 'Description', label: '描述', value: '', editor: function () { return new myflow.editors.inputEditor(); } }
            }
        },
        Activity: {
            showType: 'text',
            type: 'Activity',
            name: { text: 'Activity' },
            text: { text: '新建活动' },
            img: { src: rooturl + 'Content/Images/48/task_empty.png', width: 48, height: 48 },
            props: {
                Id: { name: 'Id', label: '标识', value: 'Process.Activity', editor: function () { return new myflow.editors.hiddenEditor(); } },
                Name: { name: 'Name', label: '名称', value: 'Activity', editor: function () { return new myflow.editors.hiddenEditor(); } },
                DisplayName: { name: 'DisplayName', label: '显示名称', value: '新建活动', editor: function () { return new myflow.editors.inputEditor(); } },
                Description: { name: 'Description', label: '描述', value: '', editor: function () { return new myflow.editors.inputEditor(); } },
                CompletionStrategy: { name: 'CompletionStrategy', label: '结束策略', value: 'ALL', editor: function () { return new myflow.editors.selectEditor([{ name: '全部任务结束', value: 'ALL' }, { name: '任意一个任务结束', value: 'ANY'}]); } },
                tasks: { name: 'tasks', label: '任务设置', value: '', editor: function () { return new myflow.editors.taskEditor(); } }
            }
        },
       FORMActivity: {
            showType: 'image&text',
            type: 'Activity',
            uitype:'TOOLActivity',
            name: { text: 'FORMActivity' },
            text: { text: '表单活动' },
            img: { src: rooturl + 'Content/Images/48/task_form.png', width: 48, height: 48 },
            props: {
                Id: { name: 'Id', label: '标识', value: 'Process.Activity', editor: function () { return new myflow.editors.hiddenEditor(); } },
                Name: { name: 'Name', label: '名称', value: 'Activity', editor: function () { return new myflow.editors.hiddenEditor(); } },
                DisplayName: { name: 'DisplayName', label: '显示名称', value: '表单活动', editor: function () { return new myflow.editors.inputEditor(); } },
                Description: { name: 'Description', label: '描述', value: '', editor: function () { return new myflow.editors.inputEditor(); } },
                CompletionStrategy: { name: 'CompletionStrategy', label: '结束策略', value: 'ALL', editor: function () { return new myflow.editors.selectEditor([{ name: '全部任务结束', value: 'ALL' }, { name: '任意一个任务结束', value: 'ANY'}]); } },
                tasks: { name: 'tasks', label: '任务设置', value: '', editor: function () { return new myflow.editors.taskEditor(); } }
            },
            attr: {
                width: 90,
                height: 80,
                r: 5,
                fill: "90-#fff-#4256B5",
                stroke: "#000",
                "stroke-width": 1
            },            
            callback:function(id){
                //加入一个默认的表单任务
                addnewTask(id, 'FORM');
            }

        },
        TOOLActivity: {
            showType: 'image&text',
            type: 'Activity',
            uitype:'TOOLActivity',
            name: { text: 'ToolActivity' },
            text: { text: '工具活动' },
            img: { src: rooturl + 'Content/Images/48/task_tool.png', width: 48, height: 48 },
            props: {
                Id: { name: 'Id', label: '标识', value: 'Process.Activity', editor: function () { return new myflow.editors.hiddenEditor(); } },
                Name: { name: 'Name', label: '名称', value: 'Activity', editor: function () { return new myflow.editors.hiddenEditor(); } },
                DisplayName: { name: 'DisplayName', label: '显示名称', value: '工具活动', editor: function () { return new myflow.editors.inputEditor(); } },
                Description: { name: 'Description', label: '描述', value: '', editor: function () { return new myflow.editors.inputEditor(); } },
                CompletionStrategy: { name: 'CompletionStrategy', label: '结束策略', value: 'ALL', editor: function () { return new myflow.editors.selectEditor([{ name: '全部任务结束', value: 'ALL' }, { name: '任意一个任务结束', value: 'ANY'}]); } },
                tasks: { name: 'tasks', label: '任务设置', value: '', editor: function () { return new myflow.editors.taskEditor(); } }
            },
            attr: {
                width: 90,
                height: 80,
                r: 5,
                fill: "90-#fff-#71EE4E",
                stroke: "#71EE4F",
                "stroke-width": 1
            },            
            callback:function(id){
                //默认加入工具任务
                addnewTask(id, 'TOOL');
            }
        },
         SUBFLOWActivity: {
            showType: 'image&text',
            type: 'Activity',
            uitype:'SUBFLOWActivity',
            name: { text: 'SubFlowActivity' },
            text: { text: '子流程活动' },
            img: { src: rooturl + 'Content/Images/48/task_subflow.png', width: 48, height: 48 },
            props: {
                Id: { name: 'Id', label: '标识', value: 'Process.Activity', editor: function () { return new myflow.editors.hiddenEditor(); } },
                Name: { name: 'Name', label: '名称', value: 'Activity', editor: function () { return new myflow.editors.hiddenEditor(); } },
                DisplayName: { name: 'DisplayName', label: '显示名称', value: '子流程活动', editor: function () { return new myflow.editors.inputEditor(); } },
                Description: { name: 'Description', label: '描述', value: '', editor: function () { return new myflow.editors.inputEditor(); } },
                CompletionStrategy: { name: 'CompletionStrategy', label: '结束策略', value: 'ALL', editor: function () { return new myflow.editors.selectEditor([{ name: '全部任务结束', value: 'ALL' }, { name: '任意一个任务结束', value: 'ANY'}]); } },
                tasks: { name: 'tasks', label: '任务设置', value: '', editor: function () { return new myflow.editors.taskEditor(); } }
            },
            attr: {
                width: 90,
                height: 80,
                r: 5,
                fill: "90-#fff-#021392",
                stroke: "#4256B5",
                "stroke-width": 1
            },            
            callback:function(id){
                //默认加入子流程任务
                addnewTask(id, 'SUBFLOW');
            }
        },
         SkipActivity: {
            showType: 'text',
            type: 'Activity',
            uitype:'SkipActivity',
            name: { text: 'Skip' },
            text: { text: '跳过' },
            img: { src: rooturl + 'Content/Images/48/task_empty.png', width: 48, height: 48 },
            props: {
                Id: { name: 'Id', label: '标识', value: 'Process.Activity', editor: function () { return new myflow.editors.hiddenEditor(); } },
                Name: { name: 'Name', label: '名称', value: 'Activity', editor: function () { return new myflow.editors.hiddenEditor(); } },
                DisplayName: { name: 'DisplayName', label: '显示名称', value: '跳过', editor: function () { return new myflow.editors.inputEditor(); } },
                Description: { name: 'Description', label: '描述', value: '', editor: function () { return new myflow.editors.inputEditor(); } },
                CompletionStrategy: { name: 'CompletionStrategy', label: '结束策略', value: 'ALL', editor: function () { return new myflow.editors.selectEditor([{ name: '全部任务结束', value: 'ALL' }, { name: '任意一个任务结束', value: 'ANY'}]); } },
                tasks: { name: 'tasks', label: '任务设置', value: '', editor: function () { return new myflow.editors.taskEditor(); } }
            },
            attr: {
                width: 30,
                height: 20,
                r: 5,
                fill: "90-#fff-#fff",
                stroke: "#4256B5",
                "stroke-width": 1
            },
            showText:function(rectClass, c, index){
                 rectClass.text.text = rectClass.props.DisplayName.value
                    = "跳过";
            }
        },
        Synchronizer: {
            showType: 'image',
            type: 'Synchronizer',
            name: { text: 'Synchronizer' },
            text: { text: '转发点' },
            img: { src: rooturl + 'Content/Images/48/gateway_Synchronizer.png', width: 32, height: 32 },
            attr: { width: 32, heigth: 32 },
            props: {
                Id: { name: 'Id', label: '标识', value: 'Process.EndNode', editor: function () { return new myflow.editors.hiddenEditor(); } },
                Name: { name: 'Name', label: '名称', value: 'EndNode', editor: function () { return new myflow.editors.hiddenEditor(); } },
                DisplayName: { name: 'DisplayName', label: '显示名称', value: '', editor: function () { return new myflow.editors.hiddenEditor(); } },
                Description: { name: 'Description', label: '描述', value: '', editor: function () { return new myflow.editors.hiddenEditor(); } }

            }
        }
    });
})(jQuery);