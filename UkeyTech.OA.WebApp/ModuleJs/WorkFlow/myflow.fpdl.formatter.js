function fpdlFormatter(data) {
    var o = '';
    o += '<?xml version="1.0" encoding="utf-8" standalone="yes"?>';
    //header
    o += '<fpdl:WorkflowProcess xmlns:fpdl="http://www.fireflow.org/Fireflow_Process_Definition_Language"' +
    ' Id="' + data.props.props.Id.value + '" Name="' + data.props.props.Name.value + '" DisplayName="' + data.props.props.DisplayName.value
    + '" ResourceFile="" ResourceManager="" TaskInstanceCreator="' + data.props.props.TaskInstanceCreator.value + '">';

    //datafields

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

            for (var b in ogData.tasks) {
              o += '<fpdl:Task Id="' + ogData.tasks[b].Id.value + '" Name="' + ogData.tasks[b].Name.value
                + '" DisplayName="' + ogData.tasks[b].DisplayName.value + '"';

              o += ' TaskInstanceCreator="' + ogData.tasks[b].TaskInstanceCreator.value + '" ';
              o += ' TaskInstanceRunner="' + ogData.tasks[b].TaskInstanceRunner.value + '" ';
              o += ' TaskInstanceCompletionEvaluator="' + ogData.tasks[b].TaskInstanceCompletionEvaluator.value + '" ';

              if( ogData.tasks[b].Type.value =='FORM' && ogData.tasks[b].CompletionStrategy){
                  o += ' CompletionStrategy="' + ogData.tasks[b].CompletionStrategy.value + '" ';
              }

              if (ogData.tasks[b].Type.value == 'FORM' && ogData.tasks[b].DefaultView) {
                  o += ' DefaultView="' + ogData.tasks[b].DefaultView + '"';
              }

              o += ' Type="' + ogData.tasks[b].Type.value + '" LoopStrategy="' + ogData.tasks[b].LoopStrategy.value 
                + '" Priority="' + ogData.tasks[b].Priority.value + '">';
              o += '<fpdl:Description>' + ogData.tasks[b].Description.value + '</fpdl:Description>'

              if(ogData.tasks[b].Performer){
                  o += '<fpdl:Performer Name="' + ogData.tasks[b].Performer.Name + '" DisplayName="' + ogData.tasks[b].Performer.DisplayName
                    + '" AssignmentType="' + ogData.tasks[b].Performer.AssignmentType
                   + '">';
                  o += '<fpdl:Description>' + ogData.tasks[b].Performer.Description + '</fpdl:Description>'
                  o += '<fpdl:AssignmentHandler>' + ogData.tasks[b].Performer.AssignmentHandler + '</fpdl:AssignmentHandler>'
                  o += '<fpdl:AssignmentType>' + ogData.tasks[b].Performer.AssignmentType + '</fpdl:AssignmentType>'
                  o += '<fpdl:PerformerValue><![CDATA[' + ogData.tasks[b].Performer.PerformerValue + ']]></fpdl:PerformerValue>'
                  
                  o += '</fpdl:Performer>';
              }

              if(ogData.tasks[b].EditForm){
                  o += '<fpdl:EditForm Name="' + ogData.tasks[b].EditForm.Name + '" DisplayName="' + ogData.tasks[b].EditForm.DisplayName + '">';
                  o += '<fpdl:Description>' + ogData.tasks[b].EditForm.Description + '</fpdl:Description>'
                  o += '<fpdl:Uri>' + ogData.tasks[b].EditForm.Uri + '</fpdl:Uri>'
                  o += '<fpdl:UIScript><![CDATA[' + ogData.tasks[b].EditForm.UIScript + ']]></fpdl:UIScript>'
                  o += '</fpdl:EditForm>';
              }

              if(ogData.tasks[b].ListForm){
                  o += '<fpdl:ListForm Name="' + ogData.tasks[b].ListForm.Name + '" DisplayName="' + ogData.tasks[b].ListForm.DisplayName + '">';
                  o += '<fpdl:Description>' + ogData.tasks[b].ListForm.Description + '</fpdl:Description>'
                  o += '<fpdl:Uri>' + ogData.tasks[b].EditForm.Uri + '</fpdl:Uri>'
                  o += '<fpdl:UIScript><![CDATA[' + ogData.tasks[b].EditForm.UIScript + ']]></fpdl:UIScript>'
                  o += '</fpdl:ListForm>';
              }

              if(ogData.tasks[b].ViewForm){
                  o += '<fpdl:ViewForm Name="' + ogData.tasks[b].ViewForm.Name + '" DisplayName="' + ogData.tasks[b].ViewForm.DisplayName + '">';
                  o += '<fpdl:Description>' + ogData.tasks[b].ViewForm.Description + ']]></fpdl:Description>'
                  o += '<fpdl:Uri>' + ogData.tasks[b].ViewForm.Uri + '</fpdl:Uri>'
                  o += '<fpdl:UIScript><![CDATA[' + ogData.tasks[b].EditForm.UIScript + '</fpdl:UIScript>'
                  o += '</fpdl:ViewForm>';
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
              if (ogData.tasks[b].Application) {
                  o += '<fpdl:Application Name="' + ogData.tasks[b].Application.Name + '" DisplayName="' + ogData.tasks[b].Application.DisplayName + '">';
                  o += '<fpdl:Description>' + ogData.tasks[b].Application.Description + '</fpdl:Description>'
                  o += '<fpdl:Handler>' + ogData.tasks[b].Application.Handler + '</fpdl:Handler>'
                  o += '<fpdl:Parameters>' + ogData.tasks[b].Application.Parameters + '</fpdl:Parameters>'
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
        if (jsondata.states[state].type == type && jsondata.states[state].props.Id.value == id)
            return jsondata.states[state];
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

function getSepcialStates(input, type) {
    var states = [];
    for (var state in input) {
        if (input[state].type == type)
            states.push(input[state]);
    }
    
    return states;
}