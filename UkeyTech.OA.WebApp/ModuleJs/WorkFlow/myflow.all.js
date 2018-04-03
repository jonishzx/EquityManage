//#region myflow.fpdl.js
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
    } else if (fromType == 'Activity' || toType == 'Activity') {
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
    else {

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
            cursor: "pointer"

        },
        dbclick: function (uiobject, rect) {
            if (uiobject.type == "Activity" && typeof(showTaskDialog) != "undefined")
                showTaskDialog(uiobject.props.DisplayName.value, uiobject.props.Id.value);
        }
    });

    $.extend(true, myflow.config.path.props, {
        Id: { name: 'Id', label: '标识', value: 'Transition', editor: function () { return new myflow.editors.hiddenEditor(); } },
        Name: { name: 'Name', label: '显示名称', value: '', editor: function () { return new myflow.editors.hiddenEditor(); } },
        Description: { name: 'Description', label: '描述', value: '', editor: function () { return new myflow.editors.inputEditor(); } },
        Condition: { name: 'Condition', label: '触发条件', value: '', editor: function () { return new myflow.editors.inputEditor(); } }
    });

    $.extend(true, myflow.config.props.props, {
        Id: { name: 'Id', label: '标识', value: '', editor: function () { return new myflow.editors.hiddenEditor(); } },
        Name: { name: 'Name', label: '名称', value: '新建流程', editor: function () { return new myflow.editors.hiddenEditor(); } },
        DisplayName: { name: 'DisplayName', label: '显示名称', value: '', editor: function () { return new myflow.editors.inputEditor(); } },
        Description: { name: 'Description', label: '描述', value: '', editor: function () { return new myflow.editors.inputEditor(); } },
        TaskInstanceCreator: { name: 'TaskInstanceCreator', label: '全局任务启动器', value: '', editor: function () { return new myflow.editors.inputEditor(); } },
        DataFields: { name: 'datafields', label: '流程参数设置', value: '', 
            editor: function () { 
                return new myflow.editors.popupEditor(showDataFieldsDialog); 
            } 
        }
    });

    $.extend(true, myflow.config.tools.states, {
        StartNode: {
            showType: 'image',
            type: 'StartNode',
            name: { text: 'StartNode' },
            text: { text: '开始' },
            img: { src: rooturl + 'Content/Images/48/start_event_empty.png', width: 48, height: 48 },
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
                CompletionStrategy: { name: 'CompletionStrategy', label: '结束策略', value: 'ALL', editor: function () { return new myflow.editors.selectEditor([{ name: '任意一个任务结束', value: 'ANY'},{ name: '全部任务结束', value: 'ALL' }]); } },
                tasks: { name: 'tasks', label: '任务设置', value: '', editor: function () { return new myflow.editors.taskEditor(); } }
            }
        },
        FORMActivity: {
            showType: 'image&text',
            type: 'Activity',
            uitype: 'TOOLActivity',
            name: { text: 'FORMActivity' },
            text: { text: '表单活动' },
            img: { src: rooturl + 'Content/Images/48/task_form.png', width: 48, height: 48 },
            props: {
                Id: { name: 'Id', label: '标识', value: 'Process.Activity', editor: function () { return new myflow.editors.hiddenEditor(); } },
                Name: { name: 'Name', label: '名称', value: 'Activity', editor: function () { return new myflow.editors.hiddenEditor(); } },
                DisplayName: { name: 'DisplayName', label: '显示名称', value: '表单活动', editor: function () { return new myflow.editors.inputEditor(); } },
                Description: { name: 'Description', label: '描述', value: '', editor: function () { return new myflow.editors.inputEditor(); } },
                CompletionStrategy: { name: 'CompletionStrategy', label: '结束策略', value: 'ALL', editor: function () { return new myflow.editors.selectEditor([{ name: '任意一个任务结束', value: 'ANY'},{ name: '全部任务结束', value: 'ALL' }]); } },
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
            callback: function (id) {
                //加入一个默认的表单任务
                addnewTask(id, 'FORM');
            }

        },
        TOOLActivity: {
            showType: 'image&text',
            type: 'Activity',
            uitype: 'TOOLActivity',
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
            callback: function (id) {
                //默认加入工具任务
                addnewTask(id, 'TOOL');
            }
        },
        SUBFLOWActivity: {
            showType: 'image&text',
            type: 'Activity',
            uitype: 'SUBFLOWActivity',
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
            callback: function (id) {
                //默认加入子流程任务
                addnewTask(id, 'SUBFLOW');
            }
        },
        SkipActivity: {
            showType: 'text',
            type: 'Activity',
            uitype: 'SkipActivity',
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
            showText: function (rectClass, c, index) {
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
//#endregion

//#region myflow.editors.js
(function ($) {
    var myflow = $.myflow;

    $.extend(true, myflow.editors, {
        inputEditor: function () {
            var _props, _k, _div, _src, _r;
            this.init = function (props, k, div, src, r) {
                _props = props; _k = k; _div = div; _src = src; _r = r;

                $('<input style=""/>').val(props[_k].value).change(function () {
                    if (_k == 'DisplayName' && _src.setText) {
                        _src.setText($(this).val());
                        $(this).parents('table').find("#ptext").find('input').val($(this).val());
                    }
                    props[_k].value = $(this).val();
                }).appendTo('#' + _div);

                $('#' + _div).data('editor', this);
            }
            this.destroy = function () {
                $('#' + _div + ' input').each(function () {
                    _props[_k].value = $(this).val();
                });
            }
        },
        readonlyEditor: function () {
            var _props, _k, _div, _src, _r;
            this.init = function (props, k, div, src, r) {
                _props = props; _k = k; _div = div; _src = src; _r = r;

                $('<label>' + props[_k].value + "</label>").appendTo('#' + _div);

                $('#' + _div).data('editor', this);
            }
            this.destroy = function () {

            }
        },
        hiddenEditor: function () {
            var _props, _k, _div, _src, _r;
            this.init = function (props, k, div, src, r) {
                _props = props; _k = k; _div = div; _src = src; _r = r;

                //$('<label>' + props[_k].value + "</label>").appendTo('#' + _div);
                $('#' + _div).parent().parent().hide();
                $('#' + _div).data('editor', this);
            }
            this.destroy = function () {

            }
        },
        taskEditor: function () {
            var _props, _k, _div, _src, _r;
            this.init = function (props, k, div, src, r) {
                _props = props; _k = k; _div = div; _src = src; _r = r;

                $('<input  type="button" value="设置"/>').change(function () {
                    props[_k].value = $(this).val();
                }).click(function () {
                    showTaskDialog(props.DisplayName.value, props.Id.value);
                }).appendTo('#' + _div);

                $('#' + _div).data('editor', this);
            }
            this.destroy = function () {
                $('#' + _div + ' input').each(function () {
                    _props[_k].value = $(this).val();
                });
            }
        },
        popupEditor: function (func) {
            var _props, _k, _div, _src, _r;
            this.init = function (props, k, div, src, r) {
                _props = props; _k = k; _div = div; _src = src; _r = r;

                $('<input  type="button" value="设置"/>').change(function () {
                    props[_k].value = $(this).val();
                }).click(function () {
                    func(props)                    
                }).appendTo('#' + _div);

                $('#' + _div).data('editor', this);
            }
            this.destroy = function () {
                $('#' + _div + ' input').each(function () {
                    _props[_k].value = $(this).val();
                });
            }
        },
        selectEditor: function (arg) {
            var _props, _k, _div, _src, _r;
            this.init = function (props, k, div, src, r) {
                _props = props; _k = k; _div = div; _src = src; _r = r;

                var sle = $('<select/>').val(props[_k].value).change(function () {
                    props[_k].value = $(this).val();
                }).appendTo('#' + _div);

                if (typeof arg === 'string') {
                    $.ajax({
                        type: "GET",
                        url: arg,
                        success: function (data) {
                            var opts = eval(data);
                            if (opts && opts.length) {
                                for (var idx = 0; idx < opts.length; idx++) {
                                    sle.append('<option value="' + opts[idx].value + '">' + opts[idx].name + '</option>');
                                }
                                sle.val(_props[_k].value);
                            }
                        }
                    });
                } else {
                    for (var idx = 0; idx < arg.length; idx++) {
                        sle.append('<option value="' + arg[idx].value + '">' + arg[idx].name + '</option>');
                    }
                    sle.val(_props[_k].value);
                }

                $('#' + _div).data('editor', this);
            };
            this.destroy = function () {
                $('#' + _div + ' input').each(function () {
                    _props[_k].value = $(this).val();
                });
            };
        }
    });

})(jQuery);
//#endregion

//#region myflow.fpdl.formatter.js
function fpdlFormatter(data) {
    var o = '';
    o += '<?xml version="1.0" encoding="utf-8" standalone="yes"?>';
    //header
    o += '<fpdl:WorkflowProcess xmlns:fpdl="http://www.fireflow.org/Fireflow_Process_Definition_Language"' +
    ' Id="' + data.props.props.Id.value + '" Name="' + data.props.props.Name.value + '" DisplayName="' + data.props.props.DisplayName.value
    + '" ResourceFile="" ResourceManager="" TaskInstanceCreator="' + data.props.props.TaskInstanceCreator.value + '">';

    //datafields
    if(data.datafields){
        o += '<fpdl:DataFields>';
        for (var a in data.datafields) {
            var m = data.datafields[a];
            o += '<fpdl:DataField Id="' + m.Name + '" Name="' + m.Name
        + '" DisplayName="' + m.DisplayName  + '" DataType="' + m.DataType  + '" InitialValue="' + m.InitialValue + '">';
            
            o += '</fpdl:DataField>';
        }
        o += '</fpdl:DataFields>'
    }    
    //*****************begin states*************************
    //StartNode
    var startnode = getSepcialStates(data.states, "StartNode")[0];
    if (startnode) {
        o += '<fpdl:StartNode Id="' + startnode.props.Id.value + '" Name="' + startnode.props.Name.value + '" DisplayName="' + startnode.props.DisplayName.value + '">'
        o = renderUIExtAttr(o, startnode);
        o += '</fpdl:StartNode>';
        delete startnode;
    }
    //EndNode

    var endnodes = getSepcialStates(data.states, "EndNode");
    if (endnodes) {
        o += '<fpdl:EndNodes>';
        for (var a in endnodes) {
            o += '<fpdl:EndNode Id="' + endnodes[a].props.Id.value + '" Name="' + endnodes[a].props.Name.value
        + '" DisplayName="' + endnodes[a].props.DisplayName.value + '">';

            o = renderUIExtAttr(o, endnodes[a]);
            o += '</fpdl:EndNode>';
        }
        o += '</fpdl:EndNodes>'
        delete endnodes;
    }

    //activities
    var activities = getSepcialStates(data.states, "Activity");
    o += '<fpdl:Activities>';
    for (var a in activities) {
        if (!a)
            continue;

        o += '<fpdl:Activity Id="' + activities[a].props.Id.value + '" Name="' + activities[a].props.Name.value
        + '" DisplayName="' + activities[a].props.DisplayName.value + '" CompletionStrategy="' + activities[a].props.CompletionStrategy.value + '">';

        var ogData = getTasksFormJsonData("Activity", activities[a].props.Id.value);

        if (ogData && ogData.tasks && ogData.tasks.length > 0) {

            o += '<fpdl:Tasks>';
            var isFormTask = false;
            for (var b in ogData.tasks) {
              isFormTask = ogData.tasks[b].Type.value == 'FORM';
              o += '<fpdl:Task Id="' + ogData.tasks[b].Id.value + '" Name="' + ogData.tasks[b].Name.value
                + '" DisplayName="' + ogData.tasks[b].DisplayName.value + '"';
              
              o += ' TaskInstanceCreator="' + ogData.tasks[b].TaskInstanceCreator.value + '" ';
              o += ' TaskInstanceRunner="' + ogData.tasks[b].TaskInstanceRunner.value + '" ';
              o += ' TaskInstanceCompletionEvaluator="' + ogData.tasks[b].TaskInstanceCompletionEvaluator.value + '" ';
              if (ogData.tasks[b].WorkItemWithDrawHandler) {
                  o += ' WorkItemWithDrawHandler="' + ogData.tasks[b].WorkItemWithDrawHandler.value + '" ';
              }
              if (isFormTask){
                  if (ogData.tasks[b].CompletionStrategy) {
                      o += ' CompletionStrategy="' + ogData.tasks[b].CompletionStrategy.value + '" ';
                  }
                  if (ogData.tasks[b].DefaultView) {
                      o += ' DefaultView="' + ogData.tasks[b].DefaultView + '"';
                  }
                  //for user select next step or actor
                  if (ogData.tasks[b].AllowSelectNextActor) {
                      o += ' AllowSelectNextActor="' + ogData.tasks[b].AllowSelectNextActor.value + '" ';
                  }
                  if (ogData.tasks[b].NextActorSelector) {
                      o += ' NextActorSelector="' + ogData.tasks[b].NextActorSelector.value + '" ';
                  }
                  if (ogData.tasks[b].AllowSelectNextStep) {
                      o += ' AllowSelectNextStep="' + ogData.tasks[b].AllowSelectNextStep.value + '" ';
                  }
                  if (ogData.tasks[b].NextStepFilter) {
                      o += ' NextStepFilter="' + ogData.tasks[b].NextStepFilter.value + '" ';
                  }

                  if (ogData.tasks[b].RejectToFirst) {
                      o += ' RejectToFirst="' + ogData.tasks[b].RejectToFirst.value + '" ';
                  }
                  if (ogData.tasks[b].SkipActWhenActorIsSame) {
                      o += ' SkipActWhenActorIsSame="' + ogData.tasks[b].SkipActWhenActorIsSame.value + '" ';
                  }
                  if (ogData.tasks[b].SkipActWhenExpired) {
                      o += ' SkipActWhenExpired="' + ogData.tasks[b].SkipActWhenExpired.value + '" ';
                  }
                  if (ogData.tasks[b].SkipActWhenNoActors) {
                      o += ' SkipActWhenNoActors="' + ogData.tasks[b].SkipActWhenNoActors.value + '" ';
                  }

                  if (ogData.tasks[b].CanWithDraw) {
                      o += ' CanWithDraw="' + ogData.tasks[b].CanWithDraw.value + '" ';
                  }
              }

          
           

              o += ' Type="' + ogData.tasks[b].Type.value + '" LoopStrategy="' + ogData.tasks[b].LoopStrategy.value 
                + '" Priority="' + ogData.tasks[b].Priority.value + '">';
              o += '<fpdl:Description>' + ogData.tasks[b].Description.value + '</fpdl:Description>'

              if (isFormTask){ 
                if(ogData.tasks[b].Performer) {
                  o += '<fpdl:Performer Name="' + ogData.tasks[b].Performer.Name + '" DisplayName="' + ogData.tasks[b].Performer.DisplayName
                    + '" AssignmentType="' + ogData.tasks[b].Performer.AssignmentType
                   + '">';
                  o += '<fpdl:Description>' + ogData.tasks[b].Performer.Description + '</fpdl:Description>'
                  o += '<fpdl:AssignmentHandler>' + ogData.tasks[b].Performer.AssignmentHandler + '</fpdl:AssignmentHandler>'
                  o += '<fpdl:AssignmentType>' + ogData.tasks[b].Performer.AssignmentType + '</fpdl:AssignmentType>'
                  o += '<fpdl:PerformerValue><![CDATA[' + ogData.tasks[b].Performer.PerformerValue + ']]></fpdl:PerformerValue>'
                  
                  o += '</fpdl:Performer>';
                }
                var form;
                  if (ogData.tasks[b].EditForm) {
                      form = ogData.tasks[b].EditForm;
                      o += '<fpdl:EditForm Name="' + form.Name + '" DisplayName="' + form.DisplayName + '">';
                      o += '<fpdl:Description>' + form.Description + '</fpdl:Description>'
                      o += '<fpdl:Uri>' + form.Uri + '</fpdl:Uri>'
                      o += '<fpdl:UIScript>' + (form.UIScript ? '<![CDATA[' + form.UIScript + ']]>' : "") + '</fpdl:UIScript>'
                      o += '<fpdl:UIControl>' + (form.UIControl ? '<![CDATA[' + form.UIControl + ']]>' : "") + '</fpdl:UIControl>'
                      o += '</fpdl:EditForm>';
                  }

                  if (ogData.tasks[b].ListForm) {
                      form = ogData.tasks[b].ListForm;
                      o += '<fpdl:ListForm Name="' + form.Name + '" DisplayName="' + form.DisplayName + '">';
                      o += '<fpdl:Description>' + form.Description + '</fpdl:Description>'
                      o += '<fpdl:Uri>' + form.Uri + '</fpdl:Uri>'
                      o += '<fpdl:UIScript>' + (form.UIScript ? '<![CDATA[' + form.UIScript + ']]>' : "") + '</fpdl:UIScript>'
                      o += '<fpdl:UIControl>' + (form.UIControl ? '<![CDATA[' + form.UIControl + ']]>' : "") + '</fpdl:UIControl>'
                      o += '</fpdl:ListForm>';
                  }

                  if (isFormTask && ogData.tasks[b].ViewForm) {
                      form = ogData.tasks[b].ViewForm;
                      o += '<fpdl:ViewForm Name="' + form.Name + '" DisplayName="' + form.DisplayName + '">';
                      o += '<fpdl:Description>' + form.Description + '</fpdl:Description>'
                      o += '<fpdl:Uri>' + form.Uri + '</fpdl:Uri>'
                      o += '<fpdl:UIScript>' + (form.UIScript ? '<![CDATA[' + form.UIScript + ']]>' : "") + '</fpdl:UIScript>'
                      o += '<fpdl:UIControl>' + (form.UIControl ? '<![CDATA[' + form.UIControl + ']]>' : "") + '</fpdl:UIControl>'
                      o += '</fpdl:ViewForm>';
                  }
              }
              if (ogData.tasks[b].Duration) {
                  o += '<fpdl:Duration Value="' + ogData.tasks[b].Duration.Value
                  + '" Unit="' + ogData.tasks[b].Duration.Unit
                  + '" IsBusinessTime="' + ogData.tasks[b].Duration.IsBusinessTime 
                  + '">';
                  o += '<fpdl:Description>' + ogData.tasks[b].ViewForm.Description + '</fpdl:Description>'
                  o += '<fpdl:Uri>' + ogData.tasks[b].ViewForm.Uri + '</fpdl:Uri>'
                  o += '</fpdl:Duration>';
              }

              //工具任务启动类
              if (ogData.tasks[b].Type.value == 'TOOL' && ogData.tasks[b].Application) {
                  o += '<fpdl:Application Name="' + ogData.tasks[b].Application.Name + '" DisplayName="' + ogData.tasks[b].Application.DisplayName + '">';
                  o += '<fpdl:Description>' + ogData.tasks[b].Application.Description + '</fpdl:Description>'
                  o += '<fpdl:Handler>' + ogData.tasks[b].Application.Handler + '</fpdl:Handler>'
                  o += '<fpdl:Parameters><![CDATA[' + ogData.tasks[b].Application.Parameters + ']]></fpdl:Parameters>'
                  o += '</fpdl:Application>';
              }

              //子流程
              if (ogData.tasks[b].Type.value == 'SUBFLOW' && ogData.tasks[b].SubWorkflowProcess) {
                  o += '<fpdl:SubWorkflowProcess Name="' + ogData.tasks[b].SubWorkflowProcess.Name + '" DisplayName="' + ogData.tasks[b].SubWorkflowProcess.DisplayName + '">';
                  o += '<fpdl:Description>' + ogData.tasks[b].SubWorkflowProcess.Description + '</fpdl:Description>'
                  o += '<fpdl:WorkflowProcessId>' + ogData.tasks[b].SubWorkflowProcess.WorkflowProcessId + '</fpdl:WorkflowProcessId>'
                  o += '</fpdl:SubWorkflowProcess>';
              }

              o += '</fpdl:Task>'
          }
          
          o += '</fpdl:Tasks>';     
        }

        o = renderUIExtAttr(o, activities[a]);

        o += '</fpdl:Activity>';
    }
    o += '</fpdl:Activities>'
    
    //Synchronizer
    var synchronizers = getSepcialStates(data.states, "Synchronizer");
    if (synchronizers) {
        o += '<fpdl:Synchronizers>';
        for (var a in synchronizers) {
            o += '<fpdl:Synchronizer Id="' + synchronizers[a].props.Id.value + '" Name="' + synchronizers[a].props.Name.value
        + '" DisplayName="' + synchronizers[a].props.DisplayName.value + '">';

            o = renderUIExtAttr(o, synchronizers[a]);

            o += '</fpdl:Synchronizer>';
        }
        o += '</fpdl:Synchronizers>'
        delete synchronizers;
    }
    //**************end states**********************


    //************transition*******************
    var transitions = data.paths;
    if (transitions) {
        o += '<fpdl:Transitions>';
        for (var a in transitions) {
            o += '<fpdl:Transition Id="' + transitions[a].props.Id.value + '" Name="' + transitions[a].props.Name.value
        + '" DisplayName="' + transitions[a].text.text + '" '
        + ' From="' + data.states[transitions[a].from].props.Id.value + '" '
        + ' To="' + data.states[transitions[a].to].props.Id.value + '">'

            o += '<fpdl:Condition><![CDATA[' + transitions[a].props.Condition.value + ']]></fpdl:Condition>'

            o += '<fpdl:ExtendedAttributes>'

            if (transitions[a].dots && transitions[a].dots.length > 0) {
                o += '<fpdl:ExtendedAttribute Name="FIRE_FLOW.edgePointList" Value="'
                for (var pos in transitions[a].dots) {
                    o += '(' + transitions[a].dots[pos].x + "," + transitions[a].dots[pos].y + ')'
                }
                o += '"/>'
            }

            if (transitions[a].textPos) {
                o += '<fpdl:ExtendedAttribute Name="FIRE_FLOW.labelPosition" Value="(' + transitions[a].textPos.x + ',' + transitions[a].textPos.y + ')"/>'
            }

            o += '</fpdl:ExtendedAttributes>'

            o += '</fpdl:Transition>';
        }
        o += '</fpdl:Transitions>'
    }
    //************transition*******************

    delete activities;
    delete transitions;
    

    o += '</fpdl:WorkflowProcess>';
    return o;
}

function getTasksFormJsonData(type, id) {
    for (var state in jsondata.states) {
        if (state != undefined && jsondata.states[state].type == type && jsondata.states[state].props && jsondata.states[state].props.Id.value == id) {
            return jsondata.states[state];
        }
    }
    return null;
}

function renderUIExtAttr(o, obj){
    o += '<fpdl:ExtendedAttributes>'

    o += '<fpdl:ExtendedAttribute Name="FIRE_FLOW.bounds.x" Value="' + obj.attr.x + '"/>';
    o += '<fpdl:ExtendedAttribute Name="FIRE_FLOW.bounds.y" Value="' + obj.attr.y + '"/>';
    o += '<fpdl:ExtendedAttribute Name="FIRE_FLOW.bounds.width" Value="' + obj.attr.width + '"/>';
    o += '<fpdl:ExtendedAttribute Name="FIRE_FLOW.bounds.height" Value="' + obj.attr.height + '"/>';

    o += '</fpdl:ExtendedAttributes>'
    return o;
}
//#endregion

//#myflow.UI.js

//#region util
function getSepcialStates(input, type) {
    var states = [];
    for (var state in input) {
        if (input[state].type == type)
            states.push(input[state]);
    }
    
    return states;
}
//#endregion

//#region 用户选择
function ShowPerfomerDialog() {
   
    var output = getTaskIndex(hiddenTaskId);
    var i = output.index;
    var ogData = output.data;
   
    if (ogData.tasks[i].Performer) {
        var name, displayname, descn, assignmenttype, assignmenthandler, performervalue;
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
    changeAssignmentType();
    performerwin.window('open');
}

var popupperformerurl = "";
function changeAssignmentType() {
    var assignmenttype = $("#AssignmentType").val();
    switch(assignmenttype){
        case "CreatorGroupPosition":
        case "LastStepUserGroupPosition":
        case "CurrentUserGroupPosition":
         $("#PopupPerformerValue").show();
         popupperformerurl = baseurl + '/UserMgmtSelector?type=Position';
        break;
         case "GroupPosition":
         $("#PopupPerformerValue").show();
         popupperformerurl = baseurl + '/UserMgmtSelector?type=GroupPosition';
        break;
        case "Handler":
         $("#dvHandler").show();
         $("#PopupPerformerValue").show();
         popupperformerurl = baseurl + '/UserMgmtSelector';
        break;
    default:
        $("#dvHandler").hide();
        $("#PopupPerformerValue").hide();
        var txt = $("#AssignmentType").find(":selected").text();
        $("#PerformerValue").val(txt);
        break;
    }
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

    /*
    //不需要再作检查，你可以使用LastStepUserGroupPosition指定必须根据流程定义来指派
    if (assignmenttype == "CurrentUserGroupPosition" && ogData.tasks[i].LoopStrategy.value != "REDO") {
        alert("选择指派方式“通过上一个步骤处理人的默认部门指派特定岗位”时，任务的重复执行策略必须为“重做”");
        return;
    }
    */
    
    ogData.tasks[i].Performer.Name = name;
    ogData.tasks[i].Performer.DisplayName = displayname;
    ogData.tasks[i].Performer.Description = descn;
    ogData.tasks[i].Performer.AssignmentHandler = assignmenthandler;
    ogData.tasks[i].Performer.AssignmentType = assignmenttype;
    ogData.tasks[i].Performer.PerformerValue = performervalue;

    $("#Performer").val(ogData.tasks[i].Performer.DisplayName + " , " + performervalue);

    //一起保存task的内容
    saveTask();

    performerwin.window("close");
}

function ShowUserDialog() {
    SetWin(800, 600, popupperformerurl, '参与者选择');
}

function setUserSelection(val) {
    $("#PerformerValue").val(val);
}
//#endregion

//#region 显示窗体编辑界面
function ShowFormDialog(type) {

    formwin.window('open');

    var output = getTaskIndex(hiddenTaskId);
    var i = output.index;
    var ogData = output.data;

    var name = '', displayname = '', uri = '', descn = '', uiscript = '', uicontrol, form;
    if (type == 'EDIT' && ogData.tasks[i].EditForm) {
        form = ogData.tasks[i].EditForm;
    }
    else if (type == 'LIST' && ogData.tasks[i].ListForm) {
        form = ogData.tasks[i].ListForm;
    }
    else if (type == 'VIEW' && ogData.tasks[i].ViewForm) {
        form = ogData.tasks[i].ViewForm;
    }

    if (form) {
        name = form.Name;
        displayname = form.DisplayName;
        uri = form.Uri;
        $("#formlist").val(uri);
        uiscript = form.UIScript ? form.UIScript : "";
        uicontrol = form.UIControl ? form.UIControl : "";
        descn = form.Description;
    }

    $("#FormName").val(name ? name : ogData.props.Id.value + "表单");
    $("#FormDisplayName").val(name ? name : ogData.props.DisplayName.value + getFormTypeText(type));
    $("#FormUri").val(uri);
    $("#FormUIScript").val(uiscript);
    $("#FormUIControl").val(uicontrol);
    $("#FormDescription").val(descn);
    
    if (uicontrol)
        loadUIControlData({ total: 0, rows: strToJson(uicontrol) });
    else
        loadUIControlData({ total: 0, rows: [] });

    $(formwin).data("type", type);
}

function loadForm(form) {
    if (getUIControlData)
        $("#FormUIControl").val(getUIControlData());

    var name = $("#FormName").val();
    var displayname = $("#FormDisplayName").val();
    var uri = $("#FormUri").val();
    var descn = $("#FormDescription").val();
    var uiscript = $("#FormUIScript").val();
    var uicontrol = $("#FormUIControl").val();

    form.Name = name;
    form.DisplayName = displayname;
    form.Uri = uri;
    form.UIScript = uiscript;
    form.UIControl = uicontrol;
    form.Description = descn;
}

function saveForm() {
    var output = getTaskIndex(hiddenTaskId);
    var i = output.index;
    var ogData = output.data;
    var type = $(formwin).data("type");
    var emptyform = { Name: "", DisplayName: "", Uri: "", Description: "", UIScript: "", UIControl: "" };
     if (type == 'EDIT') {
         if (!ogData.tasks[i].EditForm)
             ogData.tasks[i].EditForm = emptyform;

         loadForm(ogData.tasks[i].EditForm);

         $("#EditFormDetail").val(ogData.tasks[i].EditForm.DisplayName 
         + " , " + ogData.tasks[i].EditForm.Uri);      
    }
    else if (type == 'LIST') {
        if(!ogData.tasks[i].ListForm)
            ogData.tasks[i].ListForm = emptyform;
        
        loadForm(ogData.tasks[i].ListForm);
        
         $("#ListFormDetail").val(ogData.tasks[i].ListForm.DisplayName 
         + " , " + ogData.tasks[i].ListForm.Uri);
    }
    else if (type == 'VIEW') {
        if(!ogData.tasks[i].ViewForm)
            ogData.tasks[i].ViewForm = emptyform;
        loadForm(ogData.tasks[i].ViewForm);
       
         $("#ViewFormDetail").val(ogData.tasks[i].ViewForm.DisplayName 
         + " , " + ogData.tasks[i].ViewForm.Uri);
     }

     //一起保存task内容
     saveTask();

     formwin.window("close");
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
}
//#endregion

//#region 任务信息
function getTaskTypeName(value) {
    switch (value) {
        case "FORM":
            return "表单任务";
        case "TOOL":
            return "工具任务";
        case "SUBFLOW":
            return "子流程任务";
    }
    return "";
}

function getTaskIndex(id) {
    var ogData = getTasksFormJsonData("Activity", hiddenActId);
    var idx = 0;
    for (var i in ogData.tasks) {
        if (ogData.tasks[i].Id.value == id)
            idx = i;
            break;
    }

    return { index: idx, data: ogData };
}

function setTaskIndex(id, idx, task) {
    for (var state in jsondata.states) {
        if (jsondata.states[state].props.Id.value == id)
        {
            jsondata.states[state].tasks[idx] = task;
            break;
        }
    }
}

function ShowTaskPanel(type) {
    if (type == 'FORM' || type == 'TOOL')
        $("#DurationPanel").show();
    else
        $("#DurationPanel").hide();

    $("#FORMTask, #TOOLTask,#SUBFLOWTask").hide();
    $("#" + type + "Task").show();
}

function loadTask(type) {
    
    $("#TaskForm").tabs("select", 0);

    clearTaskData();

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
        if (ogData.tasks[i].WorkItemWithDrawHandler)
            $("#WorkItemWithDrawHandler").val(ogData.tasks[i].WorkItemWithDrawHandler.value);
        $("#LoopStrategy").val(ogData.tasks[i].LoopStrategy.value);

        //duration
        if (ogData.tasks[i].Duration) {
            $("#Duration").val(ogData.tasks[i].Duration.Value);
            $("#IsBusinessTime").attr("checked", ogData.tasks[i].Duration.IsBusinessTime == "TRUE");
            $("#Unit").val(ogData.tasks[i].Duration.Unit);
        }
        //FORMTask
        if (type == "FORM") {
            
            if (ogData.tasks[i].AllowSelectNextStep) {
                $("#AllowSelectNextStep").attr("checked", getBool(ogData.tasks[i].AllowSelectNextStep.value));
             }
            
            if (ogData.tasks[i].AllowSelectNextActor) {
                $("#AllowSelectNextActor").attr("checked", getBool(ogData.tasks[i].AllowSelectNextActor.value));
            }
              if (ogData.tasks[i].NextActorSelector) {
                 $("#NextActorSelector").val(ogData.tasks[i].NextActorSelector.value);
            }
            if (ogData.tasks[i].NextStepFilter) {
                 $("#NextStepFilter").val(ogData.tasks[i].NextStepFilter.value);
             }

             //reject setting
             if (ogData.tasks[i].RejectToFirst) {
                 $("#RejectToFirst").attr("checked", getBool(ogData.tasks[i].RejectToFirst.value));
             }
             //skip setting
             if (ogData.tasks[i].SkipActWhenActorIsSame) {
                 $("#SkipActWhenActorIsSame").attr("checked", getBool(ogData.tasks[i].SkipActWhenActorIsSame.value));
             }
             if (ogData.tasks[i].SkipActWhenExpired) {
                 $("#SkipActWhenExpired").attr("checked", getBool(ogData.tasks[i].SkipActWhenExpired.value));
             }
             if (ogData.tasks[i].SkipActWhenNoActors) {
                 $("#SkipActWhenNoActors").attr("checked", getBool(ogData.tasks[i].SkipActWhenNoActors.value));
             }

             if (ogData.tasks[i].CanWithDraw) {
                 $("#CanWithDraw").attr("checked", getBool(ogData.tasks[i].CanWithDraw.value));
             }
            
            if (ogData.tasks[i].CompletionStrategy) {
                 $("#CompletionStrategy").val(ogData.tasks[i].CompletionStrategy.value);
            }   
                     
            //performer 
            if (ogData.tasks[i].Performer) {
                $("#Performer").val(ogData.tasks[i].Performer.DisplayName + " , " + ogData.tasks[i].Performer.PerformerValue);
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
                $("#toollist").val(ogData.tasks[i].Application.Handler);
            }
            if($("#AppName").val()=='')
                $("#AppName").val(hiddenTaskId + ".TASKAPP");
        }
        else if (type == "SUBFLOW") {
            //subflow task
            if (ogData.tasks[i].SubWorkflowProcess) {
                $("#SubFlowName").val(ogData.tasks[i].SubWorkflowProcess.Name);
                $("#SubFlowDisplayName").val(ogData.tasks[i].SubWorkflowProcess.DisplayName);
                //$("#WorkflowProcessId").val(ogData.tasks[i].SubWorkflowProcess.WorkflowProcessId);
                $("#WorkflowProcessId").combogrid("setValue", ogData.tasks[i].SubWorkflowProcess.WorkflowProcessId)
                fixCobmboInIE();
                $("#SubFlowDescription").val(ogData.tasks[i].SubWorkflowProcess.Description);
            }
        }
    }// if
    else {
        //加入新的任务
        var newobj = addnewTask(hiddenActId, type)
        var newid = newobj.Id.value;
        $("#TaskName").val(newobj.Name.value);
        $("#TaskId").val(newid);
        $("#TaskDisplayName").val(newobj.DisplayName.value);
        $("#AppName").val(newid + ".TASKAPP");
       
        loadTaskGrid(hiddenActId);
       
    }//end if

    $("#TaskForm").show();
    $("#Buttons").show();
    ShowTaskPanel(type);
}
function getBool(val){
    if(typeof(val) == "boolean")
        return val;
    else  if(typeof(val) == "string")
        return val == "true";

    return false;
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
        loadTask(jsondata.rows[0].Type);
    }
}

function showTaskDialog(name, id) {

    $("#TaskForm").hide();

    $("#Buttons").hide();

    loadTaskGrid(id);

    taskwin.window("setTitle", name + "-任务编辑");
    taskwin.window('open');
}

function addnewTask(actid, type) {
    var ogData = getTasksFormJsonData("Activity", actid);
    var newindex = ogData.tasks && ogData.tasks.length ? ogData.tasks.length : 0;

    var newid = actid + "_" + type + "Task_" + (newindex + 1);
    if (!ogData.tasks)
        ogData.tasks = [];

    var newobj = { Id: { value: newid },
        Name: { value: type + "Task_" + (newindex + 1) },
        DisplayName: { value: '新建' + getTaskTypeName(type) + (newindex ? "_" + newindex : "") },
        Description: { value: '' },
        Type: { value: type },
        LoopStrategy: { value: 'REDO' },
        TaskInstanceCreator: { value: '' },
        TaskInstanceRunner: { value: '' },
        TaskInstanceCompletionEvaluator: { value: '' },
        WorkItemWithDrawHandler : {value:''},
        Priority: { value: '1' },
        AllowSelectNextStep: { value: 'false' },
        AllowSelectNextActor: { value: 'ANY' },
        NextActorSelector: { value: '' },
        NextStepFilter: { value: '' },
        AllowSelectNextStep: { value: 'false' },
       
        RejectToFirst: { value: 'false' },
        SkipActWhenActorIsSame: { value: 'false' },
        SkipActWhenExpired: { value: 'false' },
        SkipActWhenNoActors: { value: 'false' },
        CanWithDraw: { value: 'false' },
        
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
    task.TaskInstanceCompletionEvaluator = { value: $("#TaskInstanceCompletionEvaluator").val() };
    task.WorkItemWithDrawHandler = { value: $("#WorkItemWithDrawHandler").val() };
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
        task.AllowSelectNextStep.value = $("#AllowSelectNextStep").is(":checked");
        task.AllowSelectNextActor.value = $("#AllowSelectNextActor").is(":checked");
        task.NextActorSelector.value = $("#NextActorSelector").val();
        task.NextStepFilter.value = $("#NextStepFilter").val();

        task.RejectToFirst = { value: $("#RejectToFirst").is(":checked") };
        task.SkipActWhenActorIsSame = { value: $("#SkipActWhenActorIsSame").is(":checked") };
        task.SkipActWhenExpired = { value: $("#SkipActWhenExpired").is(":checked") };
        task.SkipActWhenNoActors = { value: $("#SkipActWhenNoActors").is(":checked") };
        task.CanWithDraw = { value: $("#CanWithDraw").is(":checked") };
        
        task.DefaultView = $("#DefaultView").val();
    }
    else if (task.Type.value == 'TOOL') {
        task.Application = {
            Name:$("#AppName").val(),
            DisplayName:$("#AppDisplayName").val(),
            Handler:$("#Handler").val(),
            Description:$("#AppDescription").val(),
            Parameters: $("#Parameters").val() 
        };
    }
    else if (task.Type.value == 'SUBFLOW') {
        task.SubWorkflowProcess = {
            Name:$("#SubFlowName").val(),
            DisplayName:$("#SubFlowDisplayName").val(),
            WorkflowProcessId: $("#WorkflowProcessId").combogrid("getValue"),
            Description:$("#SubFlowDescription").val()         
        };
    }

    ogData.tasks[i] = task;
    setTaskIndex(i, hiddenTaskId, task);

}

function saveTaskAndClose() {
    saveTask();
    //setTimeout("MsgShow('系统提示','任务修改成功。');", 1000);
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

function clearTaskData() {
    $("#TaskName").val("");
    $("#TaskId").val("");
    $("#TaskDisplayName").val("");
    $("#TaskDescn").val("");

    $("#TaskInstanceCreator").val("");
    $("#TaskInstanceRunner").val("");
    $("#TaskInstanceCompletionEvaluator").val("");
    $("#WorkItemWithDrawHandler").val("");
    $("#LoopStrategy").val("");

    $("#AllowSelectNextStep").attr("checked", false);
    $("#AllowSelectNextActor").attr("checked", false);
    $("#NextActorSelector").val("");
    $("#NextStepFilter").val("");

    $("#RejectToFirst").attr("checked", false);
    $("#SkipActWhenActorIsSame").attr("checked", false);
    $("#SkipActWhenExpired").attr("checked", false);
    $("#SkipActWhenNoActors").attr("checked", false);
    $("#CanWithDraw").attr("checked", false);
    
    $("#Duration").val("");
    $("#IsBusinessTime").attr("checked", false);
    $("#Unit").val("");

    $("#AppName").val("");
    $("#AppDisplayName").val("");
    $("#Handler").val("");
    $("#Parameters").val("");
    $("#AppDescription").val("");

    $("#SubFlowName").val("");
    $("#SubFlowDisplayName").val("");
    $("#WorkflowProcessId").val("");
    $("#WorkflowProcessId").combogrid("setValue", "");
    $("#SubFlowDescription").val("");

    $("#EditFormDetail").val("");
    $("#ListFormDetail").val("");
    $("#ViewFormDetail").val("");
    $("#Performer").val("");
}

function initTaskUI() {

    var h = 600, w = 750;
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

    $("#TaskForm,#FORMTask, #TOOLTask,#SUBFLOWTask").hide();

    $('#WorkflowProcessId').combogrid({
        panelWidth: 450,
        idField: 'Name',
        url: baseurl + "/GetWorkFlowProcessList?exceptid=" + jsondata.props.props.Id.value,
        textField: 'DisplayName',
        columns: [[
				    { field: 'Name', title: '流程代码', width: 120 },
					{ field: 'DisplayName', title: '显示名称', width: 120 },
					{ field: 'Version', title: '版本', width: 40 },
					{ field: 'Description', title: '描述', width: 120 }
				]],
        onClickRow: function (rowIndex, rowData) {
            $("#SubFlowDisplayName").val(rowData.DisplayName + "-子流程")
        }
    });

    $("#TaskGrid").datagrid({
        nowrap: false,
        striped: true,
        fit: true,
        border: false,
        queryParams: {},
        columns: [[
                    { field: 'ck', checkbox: true },
					{ field: 'DisplayName', title: '显示名称', width: 100 },
                    { field: 'Type', title: '类型', width: 70, align: 'center',
                        formatter: function (value, rec) {
                            return getTaskTypeName(value);
                        }
                    }
			]],
        pagination: false,
        singleSelect: true,
        onClickRow: function (index, data) {
            if (index > -1) {
                if (hiddenTaskId != data.Id) {
                    hiddenTaskId = data.Id;
                    loadTask(data.Type);
                }
            }
        },
        toolbar: [{
            text: '表单',
            iconCls: 'icon-add',
            handler: function () {
                hiddenTaskId = '';
                loadTask('FORM');
            }
        },
            {
                text: '工具',
                iconCls: 'icon-add',
                handler: function () {
                    hiddenTaskId = '';
                    loadTask('TOOL');
                }
            },
        {
            text: '子流程',
            iconCls: 'icon-add',
            handler: function () {
                hiddenTaskId = '';
                loadTask('SUBFLOW');
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
    formwin = $("#FromEditor");
    var fh = 530, fw = 500;
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
    var ph = 270, pw = 500;
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

    $("#saveTask").click(saveTaskAndClose);

    //检查选中的分配类型
    $("#AssignmentType").change(function () {
        changeAssignmentType();
    });

    //重复执行策略
    $("#LoopStrategy").change(function () {
        saveTask();
    });
}
//#endregion

function getDataTypeName(val) {
    switch (val) {
        case "STRING":
            return "字符串";
        case "INTEGER":
            return "整型";
        case "DATETIME":
            return "日期时间";
        case "FLOAT":
            return "数值";
        case "BOOLEAN":
            return "布尔";

    }
}

//#region 流程活动一览
function initActivityListUI() {
    $("#activityList").datagrid({
        nowrap: false,
        striped: true,
        fit: true,
        border: false,
        columns: [[
					{ field: 'Id', title: '代码', width: 100 },
                    { field: 'DisplayName', title: '名称', width: 100 }
			]],
        onDblClickRow: function (idx,data) {
            if (data.Id) {
                showTaskDialog(data.DisplayName, data.Id);
            }
        },
        rownumbers: true,
        singleSelect: true
    });

    var w = 750, h = 600;
    window.actForm = $("#actForm");
    actForm.window({
        title: '流程活动一览(双击编辑步骤信息)',
        width: w,
        height: h,
        modal: true,
        shadow: true,
        closed: true,
        top: Math.max(0, (windowHeight - h) / 4),
        left: Math.max(0, (windowWidth - w) / 3),
        resizable: false
        //onClose: (fn == undefined) ? function () { } : fn
    });
  
}

function showActFormDialog() {
    if (jsondata.states) {
        var activities = getSepcialStates(jsondata.states, "Activity");
        if (activities.length) {
            var data = [];
            for (var i = 0; i < activities.length; i++) {
                data.push({ Code: activities[i].props.Name.value, DisplayName: activities[i].props.DisplayName.value, Id: activities[i].props.Id.value });
            }
            $('#activityList').datagrid("loadData", data);
        }
    }
    actForm.window('open');
}

//#region 流程迁移一览
function initTransListUI() {
    $("#transList").edatagrid({
        nowrap: false,
        striped: true,
        fit: true,
        border: false,
        columns: [[
                    { field: 'DisplayName', title: '显示文字', width: 100 },
                    { field: 'FromAct', title: '来源步骤', width: 100 },
                    { field: 'ToAct', title: '目标步骤', width: 100 },
                    { field: 'Condition', title: '条件', width: 100, editor: { type: 'text'} }
			]],
        rownumbers: true,
        onAfterSave: function (index, row) {
            for (var i in jsondata.paths) {
                if (i.replace(".","_") == row.Id.replace(".","_")) {
                    jsondata.paths[i].props.Condition.value = row.Condition;
                    designer.flowinit();
                    break;
                }
            }
        }
    });

    var w = 750, h = 600;
    window.ptransForm = $("#transForm");
    ptransForm.window({
        title: '流程迁移活动一览（单击可编辑条件）',
        width: w,
        height: h,
        modal: true,
        shadow: true,
        closed: true,
        top: Math.max(0, (windowHeight - h) / 4),
        left: Math.max(0, (windowWidth - w) / 3),
        resizable: false
        //onClose: (fn == undefined) ? function () { } : fn
    });

}

function showTransDialog() {
    if (jsondata.paths) {
        var paths = jsondata.paths;
        //var synchronizers = getSepcialStates(jsondata.states, "Synchronizer");
        if (paths) {
            var data = [];
            for (var i in paths) {
                var f = jsondata.states[paths[i].from].props.Id.value.indexOf("Synchronizer_")<0 ? 
                jsondata.states[paths[i].from].props.DisplayName.value:"转发点";

                var t = jsondata.states[paths[i].to].props.Id.value.indexOf("Synchronizer_")<0 ?
                jsondata.states[paths[i].to].props.DisplayName.value : "转发点";

                if (jsondata.states[paths[i].to].props.Id.value.indexOf("EndNode") >= 0)
                    t = "结束";
                  
                data.push({
                    DisplayName: paths[i].text.text,
                    Id: paths[i].props.Id.value,
                    FromAct: f,
                    ToAct: t,
                    Conditon: paths[i].props.Condition.value
                });
            }
            $('#transList').edatagrid("loadData", data);
        }
    }
    ptransForm.window('open');
}
//#endregion

//#region 流程迁移一览

//#endregion

//#region 流程变量编辑
function initDataFieldsUI() {
    $("#DataFields").edatagrid({
        nowrap: false,
        striped: true,
        fit: true,
        border: false,
        columns: [[
					{ field: 'Name', title: '代码', width: 100, editor: { type: 'validatebox', options: { required: true}} },
                    { field: 'DisplayName', title: '名称', width: 100, editor: { type: 'validatebox', options: { required: true}} },
					{ field: 'DataType', title: '类型', width: 120,
                            formatter: function (value, rec) {
                                return getDataTypeName(value);
                        },
					    editor: {
					        type: 'combobox',
					        options: {
					            valueField: 'Value',
					            textField: 'Name',
					            data: [
                                    { Name: "字符串", Value: "STRING" },
                                    { Name: "整型", Value: "INTEGER" },
                                    { Name: "日期时间", Value: "DATETIME" },
                                    { Name: "数值", Value: "FLOAT" },
                                    { Name: "布尔", Value: "BOOLEAN" }
                                ],
					            required: true
					        }
					    }
					},
                    { field: 'InitialValue', title: '初始值', width: 120, editor: { type: 'text'} }
			]],
        onAfterSave: function (index, row) {
            //提示不能重复,并将其保存在json
        },
        rownumbers: true,
        singleSelect: true
    });

    var w = 750, h = 600;
    datafieldWin = $("#datafieldForm");
    datafieldWin.window({
        title: '流程变量设置(用于条件设置或活动变量设置)',
        width: w,
        height: h,
        modal: true,
        shadow: true,
        closed: true,
        top: Math.max(0, (windowHeight - h) / 4),
        left: Math.max(0, (windowWidth - w) / 3),
        resizable: false
        //onClose: (fn == undefined) ? function () { } : fn
    });

    if (jsondata.datafields && jsondata.datafields.length > 0)
        $('#DataFields').edatagrid("loadData", jsondata.datafields);
}

function showDataFieldsDialog(props) {
    datafieldWin.window('open');
}
//#endregion

var hiddenActId, hiddenTaskId;
var taskwin, formwin, performerwin, datafieldWin;
var flow;
var windowWidth = document.documentElement.clientWidth;
var windowHeight = document.documentElement.clientHeight;

var Designer = function () {
    var savewin = $("#SaveWindow");
    function flowinit() {
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
                            var pathcount = 0;
                            for (var m in json.paths) {
                                pathcount++;
                            }

                            if (pathcount < (count - 1)) {
                                alert('请检查是否有图形未进行连线');
                                return;
                            }
                        }

                        //流程变量赋值
                        json.datafields = $("#DataFields").edatagrid("getData").rows;
                        var output = fpdlFormatter(json);
                        $("#output").val(output);

                        $.ajax({
                            type: "post",
                            url: baseurl + '/DesignWorkFlow' + "?t=" + new Date().toString(),
                            data: { id: currid, ProcessContent: $("#output").val(), SaveAsNewVer: $("#SaveAsNewVer:checked").val(), IsRelease: $("#IsRelease:checked").val() },
                            dataType: "text",
                            success: function (result) {
                                var rst = result.split(":");
                                if (rst[0] == "1") {
                                    alert('保存成功');
                                    if ($("#SaveAsNewVer").attr("checked")) {
                                        window.location.href = baseurl + '/DesignWorkFlow' + "?id=" + rst[1];
                                    }
                                    else {
                                        savewin.window("close");
                                    }
                                }
                                else {
                                    alert(result);
                                }
                            }
                        });
                    }
                },
                xml: {
                    onclick: function (data) {
                        var json = eval("(" + data + ")");
                        if ($("#DataFields").edatagrid("getData").rows)
                            json.datafields = $("#DataFields").edatagrid("getData").rows;
                        var output = fpdlFormatter(json);
                        $("#output").val(output);
                        //$("#output").select();
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
    }
    this.flowinit = function () { flowinit(); }
    this.init = function () {

        flowinit();
        var sh = 150, sw = 250;
        savewin.window({
            title: '保存流程设计',
            width: sw,
            modal: true,
            shadow: true,
            closed: true,
            top: Math.max(0, (windowHeight - sh) / 2),
            left: Math.max(0, (windowWidth - sw) / 2),
            height: sh,
            resizable: false
        });
        $("#popSaveWin").click(function () { savewin.window("open"); });
        $("#cancelSave").click(function () { savewin.window("close"); });

        $('#myflow_props').window({
            title: "属性",
            modal: false,
            shadow: false,
            closed: false
        });
        $("#myflow_props").parent().css("left", windowWidth - $("#myflow_props").parent().css("width").replace("px", "") - 20);
        $("#myflow_props").parent().css("top", 10);

        $("#ViewXML").window({
            width: 600,
            height: 600,
            title: "查看输出的XML",
            modal: false,
            shadow: false,
            closed: true,
            resizable: false
        });

        initDataFieldsUI();

        initTaskUI();

        //流程一览
        initActivityListUI();
        $("#popActList").click(function () { showActFormDialog() });

        //迁移一览
        initTransListUI();
        $("#popTranList").click(function () { showTransDialog() });
    }

    return this;
}

$(function () {
    ShowLoading();
    window.designer = new Designer();
    designer.init();

    HideLoading();
    //test activity
    //showTaskDialog("LoanProcess.Submit_application_activity");
    //$("#FromEditor").window("open");
});
//#endregion