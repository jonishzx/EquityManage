<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage" ValidateRequest="false" %>
<%@ Register Src="../Shared/ScriptBlock.ascx" TagName="ScriptBlock" TagPrefix="uc1" %>
<%@ Register Src="../Shared/PopupWin.ascx" TagName="PopupWin" TagPrefix="uc2" %>
<%@ Register Src="DesignerFormEditor.ascx" TagName="FormEditor" TagPrefix="uc3" %>
<%@ Register Src="DesignerPerformerEditor.ascx" TagName="PerformerEditor" TagPrefix="uc4" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <%=Clover.Config.WebSiteConfig.Config.WebAppName%>
    </title>
    <script type="text/javascript">
        var rooturl = '<%= Clover.Web.Core.Utility.ConvertAbsoulteUrl("~/") %>'
    </script>
    <script type="text/javascript" src="<%=Url.Content("~/ModuleJs/WorkFlow/lib/raphael-min.js")%>"></script>
    <%:Styles.Render("~/styles/theme/" + UkeyTech.OA.WebApp.Helper.GetSkin())%>
    <script type="text/javascript">
        if (typeof (Object) === "undefined") {
            window.location.reload();
        }
    </script>
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/jquery-1.7.2.min.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/EasyUI/jquery.easyui.min.1.3.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/scripts/EasyUI/locale/easyui-lang-zh_CN.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/scripts/common.min.js")%>"></script>
    <script type="text/javascript">     
        ShowLoading();
        var jsondata = <%=ViewData["ProcessContent"]%>;
        var baseurl = '<%=Url.Action("","WorkFlow")%>';
        var currid = '<%=ViewData["id"] %>';
        var defaulthandler = "FireWorkflow.Net.CustomExtension.GPMBasedAssignmentHandler,FireWorkflow.Net.CustomExtension";
        var defaultForms = [
            {text: "清空", val:""},
            {text: "业务编辑表单", val:"~/Admin/CustomForm/UserSubmitDataUpdate"},
            {text: "审核表单", val:"~/Admin/CustomForm/ApproveInfo"}          
         ];

         var defaultToolTasks = [
            {text: "清空", val:""},
            {text: "SQL操作", val:"FireWorkflow.Net.CustomExtension.SQLServerTaskAppHandler,FireWorkflow.Net.CustomExtension"}
            ,{text: "控制台测试", val:"FireWorkflow.Net.CustomExtension.ConsoleTestTaskAppHandler,FireWorkflow.Net.CustomExtension"}
            ,{text: "WinForm测试", val:"FireWorkflow.Net.CustomExtension.WinFormTestTaskAppHandler,FireWorkflow.Net.CustomExtension"}
         ];
    
        function resize(){
            $("#myflow_props").parent().css("left", $(window).width() +  $(document).scrollLeft() - $("#myflow_props").width()-30);
            $("#myflow_props").parent().css("top", $(document).scrollTop() + 10);
        }

        
        var returnValue;
        function popupCallback(id, name){
            $.ajax({
                type: "post",
                url:  '<%=Url.Action("GetFormClolumns","CustomForm")%>' + "?t=" + new Date().toString(),
                data: { FormId: id},
                dataType: "json",
                success: function (json) {
                    if(json){
                        var data = {total:1, rows:[]};
                        $.each(json.rows, function(){
                            data.rows.push({Label:this.ColCaption, InputName:this.ColName, Visible:1,  Editable:1});
                        });
                        $('#dg').edatagrid("loadData",data);
                    }
                }
            });
        }

        function getUIControlData(){
            var data = $('#dg').edatagrid("getData");
            return jsonToStr(data.rows);
        }

            function loadUIControlData(data){
            $('#dg').edatagrid("loadData",data);
        }

        $(document).ready(function () {
            $(window).scroll(resize);
            $(window).resize(resize);

            var options = $("#formlist").prop('options');

            $.each(defaultForms, function(){
                options[options.length] = new Option(this.text, this.val);
            });
            
            $("#formlist").change(function(){
                $("#FormUri").val($(this).find("option:selected").attr("value"));
            }); 

            options = $("#toollist").prop('options');
            $.each(defaultToolTasks, function(){
                options[options.length] = new Option(this.text, this.val);
            });
            
            $("#toollist").change(function(){
                $("#Handler").val($(this).find("option:selected").attr("value"));
            });

            $('#dg').edatagrid({
              columns:[[  
                    {field:'Label',title:'名称',width:80, editor:{type:'validatebox',options:{required:true}}},
                    {field:'InputName',title:'输入框',width:80, editor:{type:'combobox',options:{required:true}}},
                    {field:'Visible',title:'可见',width:30,
                    formatter:function(value){  
                        if(value == 1)
                            return "√";
                        else
                            return "";
                    },   
                    editor:{  
                        type:'checkbox',  
                        options:{  
                            on: '1',  
                            off: ''  
                        }  
                    }},
                    {field:'Editable',title:'可修改',width:30,  
                      formatter:function(value){  
                            if(value == 1)
                                return "√";
                            else 
                              return "";
                        },   
                        editor:{  
                            type:'checkbox',  
                            options:{  
                                on: '1',  
                                off: ''  
                            }  
                        }
                    }
               ]]
            });

        });

    
    </script>
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/EasyUI/jquery.edatagrid.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/ModuleJs/WorkFlow/myflow.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/ModuleJs/WorkFlow/myflow.all.js")%>"></script>

    <style type="text/css">
      body{margin:0;padding:0;text-align:left;font-family:Arial,sans-serif,Helvetica,Tahoma;font-size:12px;line-height:1.5;color:black;}
      .spinner{float:left}
      .node{width:80px;vertical-align:middle;border:1px solid #fff}
      .mover{border:1px solid #ddd;background-color:#ddd}
      .selected{background-color:#ddd}
      #myflow_tooltip{width:100px;height:60px}
      #myflow_props th{letter-spacing:2px;text-align:left;padding:6px;background:#ddd;width:80px}
      #myflow_props td{background:#fff;padding:6px;width:203px}
      #myflow_props td input,#myflow_props td select{width:200px;height:20px}
      #pointer,#path,#task, #state{background-repeat:no-repeat;background-position:center}
      #myflow_tools{top:2px!important;left:2px!important}
      .popup{background:none repeat scroll 0 0 transparent!important;height:25px;margin:0!important;position:absolute!important;right:2px;width:25px!important;text-align:left;cursor:pointer;z-index:99999;padding:0 0 5px 0!important}
      #FormUri{width:150px !important;}
      #Handler{width:150px !important;}
    </style>
    <uc1:ScriptBlock ID="ScriptBlockA" />
</head>
<body>   
<div style="margin:0 0 0 100px;z-index:99999999;">

    <uc2:PopupWin ID="PopupWin1" runat="server" />
  
    <div>
        <div style="position: absolute; background-color: #fff; top: 2px !important; left: 2px !important;
            cursor: default; padding: 1px; width: 90px;">
            <div id="myflow_tools" style="background-color: #fff; padding: 1px;"
                class="easyui-panel" title="工具集">
                <div class="node" id="popSaveWin">
                    <img src="<%=Url.Content("~/Content/Images/save.gif")%>" />&nbsp;&nbsp;保存
                </div>
                <div class="node" id="popActList">
                    <img src="<%=Url.Content("~/Content/Images/16/node_elements_multiple.gif")%>" />&nbsp;&nbsp;活动一览
                </div>
                 <div class="node" id="popTranList">
                    <img src="<%=Url.Content("~/Content/Images/16/swimlanes_multiple.gif")%>" />&nbsp;&nbsp;迁移一览
                </div>
                <div class="node" id="myflow_xml">
                    <img src="<%=Url.Content("~/Content/Images/viewxml.gif")%>" />&nbsp;&nbsp;XML
                </div>
                <div>
                    <hr />
                </div>
                <div class="node selectable" id="pointer">
                    <img src="<%=Url.Content("~/Content/Images/select16.gif")%>" />&nbsp;&nbsp;选择</div>
                <div class="node selectable" id="path">
                    <img src="<%=Url.Content("~/Content/Images/16/flow_sequence.png")%>" />&nbsp;&nbsp;流程转移</div>
                <div>
                    <hr />
                </div>
                <div class="node state" id="StartNode" type="StartNode">
                    <img src="<%=Url.Content("~/Content/Images/16/start_event_empty.png")%>" />&nbsp;&nbsp;开始</div>
                <div class="node state" id="Activity" type="SkipActivity">
                    <img src="<%=Url.Content("~/Content/Images/16/task_empty.png")%>" />&nbsp;&nbsp;跳过</div>
                <div class="node state" id="Div1" type="FORMActivity">
                    <img src="<%=Url.Content("~/Content/Images/16/task_form.png")%>" />&nbsp;&nbsp;表单活动</div>
                <div class="node state" id="Div2" type="TOOLActivity">
                    <img src="<%=Url.Content("~/Content/Images/16/task_tool.png")%>" />&nbsp;&nbsp;工具活动</div>
                <div class="node state" id="Div3" type="SUBFLOWActivity">
                    <img src="<%=Url.Content("~/Content/Images/16/task_subflow.png")%>" />&nbsp;&nbsp;子流程</div>
                <div class="node state" id="Synchronizer" type="Synchronizer">
                    <img src="<%=Url.Content("~/Content/Images/16/gateway_Synchronizer.png")%>" />&nbsp;&nbsp;转发点</div>
                <div class="node state" id="EndNode" type="EndNode">
                    <img src="<%=Url.Content("~/Content/Images/16/end_event_terminate.png")%>" />&nbsp;&nbsp;结束</div>
            </div>
        </div>
        <div id="myflow" style="padding: 0 0 0 100px; float: left; background-image: url(<%=Url.Content("~/Content/Images/bg.png")%>);">
        </div>
    </div>
    <div id="myflow_props" style="background-color: #fff; top: 0; left: 0; padding: 3px;
        width: 330px; height: 250px" class="easyui-window" title="属性"
        minimizable="false" maximizable="false" closable="false" draggable="false">
        <table border="1" width="100%" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                </td>
            </tr>
        </table>
    </div>
    <div id="myflow_tooltip">
    </div>
   
    
    <div id="myflow_taskconfig" style='padding: 2px; background: #fafafa;'>
        <div class="easyui-layout" fit="true" style="overlfow: auto;">
            <div region="west" split="true" title="当前任务" style="width: 250px; padding: 0;">
                <table id="TaskGrid">
                </table>
            </div>
            <div region="center" title="任务设置">
                <div id="TaskForm" class="easyui-tabs" style="width: 480px; height: 480px" border="fasle">
                    <div title="基本设置">
                        <div class="ym-form linearize-form ym-columnar">
                            <div class="ym-form-fields">
                                <div class="ym-fbox-text ym-hideall">
                                    <label for="TaskId">
                                        任务标示：<sup class="ym-required">*</sup></label>
                                    <input id="TaskId" name="TaskId" readonly="readonly" type="text" class="form-item-text" />
                                </div>
                                <div class="ym-fbox-text ym-hideall">
                                    <label for="TaskName">
                                        任务名称：<sup class="ym-required">*</sup></label>
                                    <input id="TaskName" name="TaskName" readonly="readonly" type="text" class="form-item-text" />
                                </div>
                                <div class="ym-fbox-text">
                                    <label for="TaskDisplayName">
                                        显示名称：<sup class="ym-required">*</sup></label>
                                    <input id="TaskDisplayName" name="TaskDisplayName" type="text" class="form-item-text" />
                                </div>
                                <div class="ym-fbox-text">
                                    <label for="TaskDescn">
                                        备注：</label>
                                    <input id="TaskDescn" name="TaskDescn" type="text" class="form-item-text" />
                                </div>
                            </div>
                        </div>
                        <div class="ym-form linearize-form ym-columnar">
                            <div class="ym-form-fields">
                                <div id="FORMTask">
                                    <div class="ym-fbox-text">
                                        <label for="Performer">
                                            参与者：</label>
                                        <input id="Performer" name="Performer" type="text" class="form-item-text easyui-validatebox"
                                            missingmessage="必填" required="true" />
                                        <input type="button" class="popup" onclick="ShowPerfomerDialog();" value="....." />
                                    </div>
                                    <div class="ym-fbox-text">
                                        <label for="CompletionStrategy">
                                            分配策略：</label>
                                        <select id="CompletionStrategy" name="CompletionStrategy">
                                            <option value='ALL'>全部</option>
                                            <option value='ANY'>任意一个</option>
                                        </select>
                                    </div>                                  
                                    <div class="ym-fbox-text ym-hideall">
                                        <label for="AllowSelectNextActor">
                                           可选择下步：</label>
                                        <input type=checkbox id="AllowSelectNextStep" style="width:20px;" onclick="if(!$(this).is(':checked'))$('#AllowSelectNextActor').removeAttr('checked');"/>
                                        <span style="float:left;">名称过滤(Regex):</span><input type=text id="NextStepFilter" style="width:150px"  />
                                    </div>
                                     <div class="ym-fbox-text">
                                        <label for="AllowSelectNextActor">
                                            可选择人员：</label>
                                        <input type=checkbox id="AllowSelectNextActor"  style="width:20px;" onclick="if($(this).is(':checked')){$('#AllowSelectNextStep').attr('checked',true);}else{$('#AllowSelectNextStep').removeAttr('checked');}" />
                                        <span style="float:left;">选择器:</span><input type=text id="NextActorSelector" style="width:150px" />
                                    </div>
                                    <div class="ym-fbox-text">
                                        <label for="RejectToFirst">
                                            退回到第一步：</label>
                                        <input type=checkbox id="RejectToFirst" style="width:20px;" />
                                    </div>
                                    <div class="ym-fbox-text"  >
                                        <label for="SkipActWhenActorIsSame"  >
                                            通过用户跳过：</label>
                                        <input type="checkbox" id="SkipActWhenActorIsSame" style="width:20px;" onclick="if($(this).is(':checked'))$('#SkipActWhenNoActors').attr('checked',true);" />
                                    </div>
                                     <div class="ym-fbox-text" >
                                        <label for="SkipActWhenNoActors" >
                                            无用户时跳过：</label>
                                        <input type="checkbox" id="SkipActWhenNoActors" style="width:20px;" />
                                    </div>
                                    <div class="ym-fbox-text" >
                                        <label for="CanWithDraw" >
                                            可取回：</label>
                                        <input type="checkbox" id="CanWithDraw" style="width:20px;" />
                                    </div>
                                    <div class="ym-fbox-text ym-hideall">
                                        <label for="SkipActWhenExpired">
                                            任务过期时跳过：</label>
                                        <input type=checkbox id="SkipActWhenExpired" style="width:20px;" />
                                    </div>
                                   
                                    <div class="ym-fbox-text">
                                        <label for="DefaultView">
                                            表单类型：</label>
                                        <select id="DefaultView" name="DefaultView">
                                            <option value='EDITFORM'>编辑表单</option>
                                            <option value='VIEWFORM'>只读表单</option>
                                            <option value='LISTFORM'>列表表单</option>
                                        </select>
                                    </div>
                                    <div class="ym-fbox-text">
                                        <label for="EditFormDetail">
                                            编辑表单设置：</label>
                                        <input id='EditFormDetail' name='EditFormDetail' readonly="readonly" />
                                        <input type="button" class="popup" onclick="ShowFormDialog('EDIT');" value="......" />
                                    </div>
                                    <div class="ym-fbox-text ym-hideall">
                                        <label for="ListFormDetail">
                                            列表表单设置：</label>
                                        <input id='ListFormDetail' name='ListFormDetail' readonly="readonly" />
                                        <input type="button" class="popup" onclick="ShowFormDialog('LIST');" value="......" />
                                    </div>
                                    <div class="ym-fbox-text ym-hideall">
                                        <label for="ViewFormDetail">
                                            只读表单设置：</label>
                                        <input id='ViewFormDetail' name='ViewFormDetail' readonly="readonly" />
                                        <input type="button" class="popup" onclick="ShowFormDialog('VIEW');" value="......." />
                                    </div>
                                </div>
                                <div id="TOOLTask">
                                    <div class="ym-fbox-text ym-hideall">
                                        <label for="AppName">
                                            应用标示：<sup class="ym-required">*</sup></label>
                                        <input id='AppName' name='AppName' readonly="readonly" />
                                    </div>
                                    <div class="ym-fbox-text">
                                        <label for="AppDisplayName">
                                            应用显示名称：<sup class="ym-required">*</sup></label>
                                        <input id='AppDisplayName' name='AppDisplayName' />
                                    </div>
                                    <div class="ym-fbox-text">
                                        <label for="Handler">
                                            启动的应用：<sup class="ym-required">*</sup></label>
                                        <span class="ym-fbox-check clearCss">
                                            <input id='Handler' name='Handler' />
                                            <select id="toollist"></select>
                                        </span>
                                    </div>
                                        <div class="ym-fbox-text">
                                        <label for="Handler">
                                            传入参数：<sup class="ym-required">*</sup></label>
                                        <input id='Parameters' name='Parameters' />
                                    </div>
                                    <div class="ym-fbox-text">
                                        <label for="AppDescription">
                                            描述：</label>
                                        <input id='AppDescription' name='AppDescription' />
                                    </div>
                                </div>
                                <div id="SUBFLOWTask">
                                    <div class="ym-fbox-text">
                                        <label for="SubFlowName">
                                            子流程标示：<sup class="ym-required">*</sup></label>
                                        <input id='SubFlowName' name='SubFlowName'/>
                                    </div>
                                    <div class="ym-fbox-text">
                                        <label for="SubFlowDisplayName">
                                            子流程名称：<sup class="ym-required">*</sup></label>
                                        <input id='SubFlowDisplayName' name='SubFlowDisplayName' />
                                    </div>
                                    <div class="ym-fbox-text">
                                        <label for="WorkflowProcessId">
                                            子流程ID：<sup class="ym-required">*</sup></label>                                        
                                        <select id="WorkflowProcessId" name="WorkflowProcessId" style="width:250px;"></select>
                                    </div>
                                    <div class="ym-fbox-text">
                                        <label for="SubFlowDescription">
                                            描述：</label>
                                        <input id='SubFlowDescription' name='SubFlowDescription' />
                                    </div>
                                </div>
                                <div id="DurationPanel" class="ym-fbox-text">
                                    <label for="Duration">
                                        任务期限：</label>
                                    <span class="clearall">
                                        <input id="Duration" name="Duration" type="text" class="easyui-numberspinner" min="0"
                                            max="1000" required="true" missingmessage="必填" />
                                        <span for='IsBusinessTime'>工作日? </span>
                                        <input id='IsBusinessTime' name='IsBusinessTime' type='checkbox' value='True' style="width: auto;" />
                                        <select id="Unit" name="Unit" style="width: 60px;">
                                            <option value='Null'>默认</option>
                                            <option value='YEAR'>年</option>
                                            <option value='MONTH'>月</option>
                                            <option value='WEEK'>周</option>
                                            <option value='DAY'>日</option>
                                            <option value='MINUTE'>分</option>
                                            <option value='SECOND'>秒</option>
                                        </select>
                                    </span>
                                </div>
                                <div class="ym-fbox-text ym-hideall">
                                    <label for="TaskInstanceCreator">
                                        实例创建器：</label>
                                    <input id="TaskInstanceCreator" name="TaskInstanceCreator" type="text" class="form-item-text" />
                                </div>
                                <div class="ym-fbox-text ym-hideall">
                                    <label for="TaskInstanceRunner">
                                        实例运行器：</label>
                                    <input id="TaskInstanceRunner" name="TaskInstanceRunner" type="text" class="form-item-text" />
                                </div>
                                <div class="ym-fbox-text">
                                    <label for="TaskInstanceCompletionEvaluator">
                                        终结判定器：</label>
                                    <input id="TaskInstanceCompletionEvaluator" name="TaskInstanceCompletionEvaluator"
                                        type="text" class="form-item-text" />
                                </div>
                                <div class="ym-fbox-text">
                                    <label for="WorkItemWithDrawHandler">
                                        取回处理器：</label>
                                    <input id="WorkItemWithDrawHandler" name="WorkItemWithDrawHandler"
                                        type="text" class="form-item-text" />
                                </div>
                                <div class="ym-fbox-text">
                                    <label for="LoopStrategy">
                                        重复执行策略：</label>
                                    <select id="LoopStrategy" name="LoopStrategy">
                                        <option value='REDO'>重做</option>
                                        <option value='SKIP'>只执行一次</option>
                                        <option value='NONE'>无</option>
                                    </select>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="Buttons" class="ym-fbox-button">
                    <a id="saveTask" href='javascript:void(0);' class="easyui-linkbutton" icon="icon-ok">
                        保存</a>
                </div>
            </div>
        </div>
    </div>
    <div id="ViewXML">
        <textarea id="output" rows="3" style="width: 100%; height: 500px;"></textarea>
    </div>
    <uc3:FormEditor ID="fe1" runat="server" />
    <uc4:PerformerEditor ID="pe1" runat="server" />
    <div id="SaveWindow">
        <div class="ym-form linearize-form ym-full">
        <div class="ym-form-fields">
            <div class="ym-fbox-check">
                <label for="SaveAsNewVer">
                    保存为新版本：</label>
                <input id='SaveAsNewVer' type=checkbox value="true" name='SaveAsNewVer' />
            </div>
            <div class="ym-fbox-check">
                <label for="IsRelease">
                    发布：</label>
                <input id='IsRelease' type=checkbox checked=checked value="true" name='IsRelease' />
            </div>
         </div>        
        <div class="ym-fbox-button">
            <a id="myflow_save" href='javascript:void(0);' class="easyui-linkbutton" icon="icon-ok">
                保存</a> <a id="cancelSave" href='javascript:void(0);' class="easyui-linkbutton" icon="icon-cancel">
                    取消</a>
        </div>
    </div>
    </div>
    <div id="subflowform">
        <table id="ProcessGrid">
        </table>
    </div>
    <div id="datafieldForm" title="流程变量">       
        <table id="DataFields" toolbar="#datafieldstoolbar" fitColumns="true" singleSelect="true">
        </table>       
        <div id="datafieldstoolbar">
		    <a href="#" class="easyui-linkbutton" iconCls="icon-add" plain="true" onclick="javascript:$('#DataFields').edatagrid('addRow')">新建</a>
		    <a href="#" class="easyui-linkbutton" iconCls="icon-remove" plain="true" onclick="javascript:$('#DataFields').edatagrid('destroyRow')">删除</a>
		    <a href="#" class="easyui-linkbutton" iconCls="icon-save" plain="true" onclick="javascript:$('#DataFields').edatagrid('saveRow')">保存</a>
		    <a href="#" class="easyui-linkbutton" iconCls="icon-undo" plain="true" onclick="javascript:$('#DataFields').edatagrid('cancelRow')">取消</a>
	    </div>   
    </div>
    <div id="actForm" title="活动一览">       
        <table id="activityList"  fitColumns="true" singleSelect="true">
        </table>       
     
    </div>
    <div id="transForm" title="迁移一览">       
        <table id="transList" fitColumns="true" singleSelect="true">
        </table>       
          
    </div>   
</div>
</body>
</html>
